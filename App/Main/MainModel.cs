using MathModel;
using MathModel.Parameters;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace App.Main
{
    public sealed class MainModel : Common.Abstraction.Model
    {
        public MainModel()
        {
            Context = new ChemistryDB.ChemistryContext();
            MathModel = new MathModel.Model();
        }

        public AuthorizationDB.User CurrentUser { get; set; }
        private ChemistryDB.ChemistryContext Context { get; }
        private MathModel.Model MathModel { get; set; }

        private ChemistryDB.Material selectedMaterial;
        private ChemistryDB.Material SelectedMaterial 
        {
            get => selectedMaterial;
            set
            {
                selectedMaterial = value;
                var nameValuePairs = Context.ParameterValue
                    .Where(val => val.Material == SelectedMaterial)
                    .Select(val => new { val.Parameter.Name, val.Value });

                // TODO: wrap it up
                foreach (var pair in nameValuePairs)
                {
                    switch (pair.Name)
                    {
                        case "Плотность":
                            MathModel.MaterialParams.Density = pair.Value;
                            break;
                        case "Удельная теплоемкость":
                            MathModel.MaterialParams.AverageSpecificHeatCapacity = pair.Value;
                            break;
                        case "Температура плавления":
                            MathModel.MaterialParams.MeltingTemperature = pair.Value;
                            break;
                        case "Коэффициент консистенции материала при температуре приведения":
                            MathModel.EmpiricCoeffs.ConsistencyCoefficient = pair.Value;
                            break;
                        case "Температурный коэффициент вязкости материала":
                            MathModel.EmpiricCoeffs.ViscosityTemperatureCoefficient = pair.Value;
                            break;
                        case "Температура приведения":
                            MathModel.EmpiricCoeffs.CastTemperature = pair.Value;
                            break;
                        case "Индекс течения материала":
                            MathModel.EmpiricCoeffs.MaterialFlowIndex = pair.Value;
                            break;
                        case "Коэффициент теплоотдачи от крышки канала к материалу":
                            MathModel.EmpiricCoeffs.HeatTransferCoefficient = pair.Value;
                            break;
                        default:
                            throw new NotImplementedException($"There is no parameter with name\"{pair.Name}\"");
                    }
                }
            }
        }

        public void SetSelectedMaterial(string name)
        {
            var material = Context.Material.AsEnumerable().FirstOrDefault(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            SelectedMaterial = material ?? throw new NoNullAllowedException($"Material with name \"{name}\" was not found");
        }

        public void SetParameterValue(string parameterType, string parameterName, double value)
        {
            var selectedParams = MathModel.GetType()
                .GetProperties()
                .Where(prop => prop.GetValue(MathModel) is Params)
                .First(prop => prop.Name.Contains(parameterType))
                .GetValue(MathModel) as Params;

            var parameterProperty = selectedParams.GetType().GetProperty(parameterName)
                ?? throw new ArgumentException("There is no such parameter", parameterName);
            parameterProperty.SetValue(selectedParams, value);
        }

        public MathModel.Solution GetSolution()
        {
            var rawSolution = this.MathModel.Solve();
            Solution = rawSolution.RoundUp();
            return Solution;
        }

        public Solution Solution { get; private set; }

        public IEnumerable<string> GetMaterialsNames()
        {
            var materials = Context.Material.AsEnumerable();
            return from material in materials select material.Name;
        }

        public void SaveReport(string filename) // TODO: fix all of this mess
        {
            static string getDescription(PropertyInfo prop)
            {
                return prop.CustomAttributes.First().NamedArguments.First(arg => arg.MemberName == "Description").TypedValue.Value.ToString();
            }

            File.Delete(filename);

            var outputProperties = Solution.GetType().GetProperties();
            var outputValuesProperties = outputProperties.Where(p => p.PropertyType == typeof(int) || p.PropertyType == typeof(double));
            var outputRangedProperties = outputProperties.Where(p => p.PropertyType == typeof(IReadOnlyCollection<double>));
            var outputValues = outputValuesProperties.Select(p => new { Name = getDescription(p), Value = p.GetValue(Solution) });
            var outputRanges = outputRangedProperties.Select(p => new { Name = getDescription(p), Values = (IReadOnlyCollection<double>)p.GetValue(Solution) });

            var mathModelParams = MathModel.GetType().GetProperties().Select(p => new { Name = getDescription(p), Value = p.GetValue(MathModel) });
            var inputParamsTypes = new List<(string name, IEnumerable<(string name, object value)> value)>();
            foreach (var param in mathModelParams)
            {
                inputParamsTypes.Add((param.Name, param.Value.GetType().GetProperties().Select(prop => (getDescription(prop), prop.GetValue(param.Value)))));
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(new FileInfo(filename));
            using var workbook = package.Workbook;

            ExcelWorksheet worksheet; 
            if (!workbook.Worksheets.Any(sh => sh.Name == "Отчёт о работе программы"))
            {
                worksheet = workbook.Worksheets.Add("Отчёт о работе программы");
            }
            else
            {
                worksheet = workbook.Worksheets.First(sh => sh.Name == "Отчёт о работе программы");
            }

            worksheet.SetValue(1, 1, "Входные данные");
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.SetValue(2, 1, "Тип материала");
            worksheet.Cells[2, 1].Style.Font.Bold = true;
            worksheet.SetValue(2, 2, SelectedMaterial.Name);
            worksheet.Cells[2, 2].Style.Font.Bold = true;

            int currentRow = 3;
            foreach (var inputParamsType in inputParamsTypes)
            {
                worksheet.SetValue(currentRow++, 1, inputParamsType.name);
                worksheet.Cells[currentRow - 1, 1].Style.Font.Bold = true;
                foreach (var inputParam in inputParamsType.value)
                {
                    worksheet.SetValue(currentRow, 1, inputParam.name);
                    worksheet.SetValue(currentRow++, 2, inputParam.value);
                }
            }
            worksheet.Column(1).Width = 70;
            worksheet.Column(2).Width = 20;

            ++currentRow;
            worksheet.SetValue(currentRow++, 1, "Выходные данные");
            worksheet.Cells[currentRow - 1, 1].Style.Font.Bold = true;

            foreach (var outputValue in outputValues)
            {
                worksheet.SetValue(currentRow, 1, outputValue.Name);
                worksheet.SetValue(currentRow++, 2, outputValue.Value);
            }
            var chartsHeaderRow = ++currentRow;

            worksheet.Column(4).Width = 40;
            worksheet.Column(5).Width = 20;
            worksheet.Column(6).Width = 20;

            for (int i = 0; i < outputRanges.Count(); ++i)
            {
                var col = i + 4;
                var outputRange = outputRanges.ElementAt(i);
                currentRow = 2;
                worksheet.SetValue(1, col, outputRange.Name);
                worksheet.Cells[1, col].Style.Font.Bold = true;

                foreach (var value in outputRange.Values)
                {
                    worksheet.SetValue(currentRow++, col, value);
                }
            }

            using var etaChart = worksheet.Drawings.AddLineChart("EtaChart", eLineChartType.LineMarkers);
            int lastRow = currentRow;
            etaChart.SetSize(400, 300);
            etaChart.SetPosition(chartsHeaderRow + 1, 0, 0, 0);
            etaChart.Fill.Color = Color.White;
            etaChart.YAxis.MinValue = outputRanges.ElementAt(1).Values.Min();
            etaChart.YAxis.MaxValue = outputRanges.ElementAt(1).Values.Max();
            var etaSeries = etaChart.Series.Add($"E{chartsHeaderRow + 1}:E{lastRow}", $"D{chartsHeaderRow + 1}:D{lastRow}");
            etaSeries.Marker.Size = 2;
            etaSeries.Header = "Вязкость";

            using var TChart = worksheet.Drawings.AddLineChart("TChart", eLineChartType.LineMarkers);
            TChart.SetSize(400, 300);
            TChart.SetPosition(chartsHeaderRow + 20, 0, 0, 0);
            TChart.Fill.Color = Color.White;
            TChart.YAxis.MinValue = outputRanges.ElementAt(2).Values.Min();
            TChart.YAxis.MaxValue = outputRanges.ElementAt(2).Values.Max();
            var TSeries = TChart.Series.Add($"F{chartsHeaderRow + 1}:F{lastRow}", $"D{chartsHeaderRow + 1}:D{lastRow}");
            TSeries.Marker.Size = 2;
            TSeries.Header = "Температура";

            package.Save();

            worksheet.Dispose();
        }

        ~MainModel() => Context.Dispose();
    }
}
