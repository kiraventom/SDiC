using App.Common.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using App.Common.CustomEventArgs;
using System.Data;
using System.Reflection.Metadata;
using System.Diagnostics;
using MathModel.Parameters;

namespace App.Main
{
    public sealed class MainModel : Model
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
            var roundedSolution = rawSolution.RoundUp();
            return roundedSolution;
        }

        public IEnumerable<string> GetMaterialsNames()
        {
            var materials = Context.Material.AsEnumerable();
            return from material in materials select material.Name;
        }

        ~MainModel() => Context.Dispose();
    }
}
