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
            if (material is null)
            {
                throw new NoNullAllowedException($"Material with name \"{name}\" was not found");
            }
            SelectedMaterial = material;
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

        public void SaveReport(string filename)
        {
            var outputProperties = Solution.GetType().GetProperties();
            var outputValuesProperties = outputProperties.Where(p => p.PropertyType == typeof(int) || p.PropertyType == typeof(double));
            var outputRangedProperties = outputProperties.Where(p => p.PropertyType == typeof(IReadOnlyCollection<double>));
            var outputValues = outputValuesProperties.Select(p => new { p.Name, Value = p.GetValue(Solution) });
            var outputRanges = outputRangedProperties.Select(p => new { p.Name, Values = (IReadOnlyCollection<double>)p.GetValue(Solution) });

            var mathModelParams = MathModel.GetType().GetProperties().Select(p => p.GetValue(MathModel));
            var mathModelParamsProperties = new List<IEnumerable<(string, object)>>();
            foreach (var param in mathModelParams)
            {
                mathModelParamsProperties.Add(param.GetType().GetProperties().Select(p => (p.Name, p.GetValue(param) )));
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filename)))
            {
                using (var workbook = package.Workbook)
                {
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
                    worksheet.SetValue(2, 1, SelectedMaterial.Name);
                    for (int row = 0; row < mathModelParamsProperties.Count; ++row)
                    {
                        (string name, object value)[] mathModelParamProperty = mathModelParamsProperties[row].ToArray();
                        for (int col = 0; col < mathModelParamProperty.Length; ++col)
                        {
                            worksheet.SetValue((row + 1) * 2 + 1, col + 1, mathModelParamProperty[col].name);
                            worksheet.SetValue((row + 1) * 2 + 2, col + 1, mathModelParamProperty[col].value);
                            worksheet.Column(col + 1).Width = 20;
                        }
                    }
                    worksheet.Column(1).Width = 30;

                    worksheet.SetValue(14, 1, "Выходные данные");
                    for (int col = 0; col < outputValues.Count(); ++col)
                    {
                        worksheet.SetValue(15, col + 1, outputValues.ElementAt(col).Name);
                        worksheet.SetValue(16, col + 1, outputValues.ElementAt(col).Value);
                    }

                    for (int col = 0; col < outputRanges.Count(); ++col)
                    {
                        worksheet.SetValue(18, col + 1, outputRanges.ElementAt(col).Name);
                        for (int row = 0; row < outputRanges.ElementAt(col).Values.Count; ++row)
                        {
                            worksheet.SetValue(19 + row, col + 1, outputRanges.ElementAt(col).Values.ElementAt(row));
                        }
                    }

                    var etaChart = worksheet.Drawings.AddLineChart("EtaChart", eLineChartType.LineMarkers);
                    int lastRow = 19 + outputRanges.First().Values.Count - 1;
                    etaChart.SetPosition(18, 0, 4, 0);
                    etaChart.SetSize(600, 300);
                    etaChart.Fill.Color = Color.White;
                    etaChart.YAxis.MinValue = outputRanges.ElementAt(1).Values.Min();
                    etaChart.YAxis.MaxValue = outputRanges.ElementAt(1).Values.Max();
                    var etaSeries = etaChart.Series.Add($"B19:B{lastRow}", $"A19:A{lastRow}");
                    etaSeries.Marker.Size = 2;
                    etaSeries.Header = "η";

                    var TChart = worksheet.Drawings.AddLineChart("TChart", eLineChartType.LineMarkers);
                    TChart.SetPosition(35, 0, 4, 0);
                    TChart.SetSize(600, 300);
                    TChart.Fill.Color = Color.White;
                    TChart.YAxis.MinValue = outputRanges.ElementAt(2).Values.Min();
                    TChart.YAxis.MaxValue = outputRanges.ElementAt(2).Values.Max();
                    var TSeries = TChart.Series.Add($"C19:C{lastRow}", $"A19:A{lastRow}");
                    TSeries.Marker.Size = 2;
                    TSeries.Header = "T";

                    package.Save();
                }
            }
        }

        ~MainModel() => Context.Dispose();
    }
}
