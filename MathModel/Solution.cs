using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MathModel
{
    public sealed class Solution
    {
        internal Solution(double F, double Q_CH, double gamma, double q_gamma, double q_alpha, int N, double Q, double T_p, double eta_p,
                        IEnumerable<double> z, IEnumerable<double> T, IEnumerable<double> eta)
        {
            this.ChannelGeometricShapeCoefficient = F;
            this.ForwardFlowMaterialConsumption = Q_CH;
            this.MaterialShearStrainRate = gamma;
            this.SpecificHeatFluxForStreamViscousFriction = q_gamma;
            this.SpecificHeatFluxForChannelLidHeatExchange = q_alpha;
            this.CalculationStepAmount = N;
            this.ChannelProductivity = Q;
            this.ChannelTemperature = T_p;
            this.ProductViscosity = eta_p;
            this.CoordinateByChannelLength = z.ToList().AsReadOnly();
            this.Temperature = T.ToList().AsReadOnly();
            this.Viscosity = eta.ToList().AsReadOnly();
        }
        /// <summary>
        /// Коэффициент геометрической формы канала
        /// </summary>
        [Display(Name="ChannelGeometricShapeCoefficient", Description = "Коэффициент геометрической формы канала")]
        public double ChannelGeometricShapeCoefficient { get; }
        /// <summary>
        /// Расход поступательного потока материала через канал
        /// </summary>
        [Display(Name= "ForwardFlowMaterialConsumption", Description = "Расход поступательного потока материала через канал, м^3/с")]
        public double ForwardFlowMaterialConsumption { get; }
        /// <summary>
        /// Cкорость деформации сдвига материала
        /// </summary>
        [Display(Name = "MaterialShearStrainRate", Description = "Cкорость деформации сдвига материала, 1/с")]
        public double MaterialShearStrainRate { get; }
        /// <summary>
        /// Удельный тепловой поток за счёт вязкого трения в потоке
        /// </summary>
        [Display(Name = "SpecificHeatFluxForStreamViscousFriction", Description = "Удельный тепловой поток за счёт вязкого трения в потоке, Вт/м")]
        public double SpecificHeatFluxForStreamViscousFriction { get; }
        /// <summary>
        /// Удельный тепловой поток за счёт теплообмена с крышкой канала
        /// </summary>
        [Display(Name = "SpecificHeatFluxForChannelLidHeatExchange", Description = "Удельный тепловой поток за счёт теплообмена с крышкой канала, Вт/м")]
        public double SpecificHeatFluxForChannelLidHeatExchange { get; }
        /// <summary>
        /// Число шагов вычислений по длине канала
        /// </summary>
        [Display(Name = "CalculationStepAmount", Description = "Число шагов вычислений по длине канала")]
        public int CalculationStepAmount { get; }
        /// <summary>
        /// Производительность канала
        /// </summary>
        [Display(Name = "ChannelProductivity", Description = "Производительность канала, кг/ч")]
        public double ChannelProductivity { get; }
        /// <summary>
        /// Температура продукта
        /// </summary>
        [Display(Name = "ChannelTemperature", Description = "Температура продукта, С")]
        public double ChannelTemperature { get; }
        /// <summary>
        /// Вязкость продукта
        /// </summary>
        [Display(Name = "ProductViscosity", Description = "Вязкость продукта, Па*с")]
        public double ProductViscosity { get; }
        /// <summary>
        /// Координата по длине продукта
        /// </summary>
        [Display(Name = "CoordinateByChannelLength", Description = "Координата по длине продукта, м")]
        public IReadOnlyCollection<double> CoordinateByChannelLength { get; }
        /// <summary>
        /// Температура
        /// </summary>
        [Display(Name = "Temperature", Description = "Температура, С")]
        public IReadOnlyCollection<double> Temperature { get; }
        /// <summary>
        /// Вязкость
        /// </summary>
        [Display(Name = "Viscosity", Description = "Вязкость, Па*с")]
        public IReadOnlyCollection<double> Viscosity { get; }

        public Solution RoundUp()
        {
            var F = RoundUpNumber(this.ChannelGeometricShapeCoefficient);
            var Q_CH = RoundUpNumber(this.ForwardFlowMaterialConsumption);
            var gamma = RoundUpNumber(this.MaterialShearStrainRate);
            var q_gamma = RoundUpNumber(this.SpecificHeatFluxForStreamViscousFriction);
            var q_alpha = RoundUpNumber(this.SpecificHeatFluxForChannelLidHeatExchange);
            var N = (int)RoundUpNumber(this.CalculationStepAmount, 0);
            var Q = RoundUpNumber(this.ChannelProductivity);
            var T_p = RoundUpNumber(this.ChannelTemperature);
            var eta_p = RoundUpNumber(this.ProductViscosity);
            var z = this.CoordinateByChannelLength.Select(e => RoundUpNumber(e, 3));
            var T = this.Temperature.Select(e => RoundUpNumber(e));
            var eta = this.Viscosity.Select(e => RoundUpNumber(e));
            return new Solution(F, Q_CH, gamma, q_gamma, q_alpha, N, Q, T_p, eta_p, z, T, eta);
        }

        private double RoundUpNumber(double numberToRoundUp, int digitsToLeave = -1)
        {
            if (digitsToLeave == -1)
            {
                if (numberToRoundUp == 0)
                {
                    return numberToRoundUp;
                }
                else
                {
                    if (numberToRoundUp >= 10)
                    {
                        digitsToLeave = 1;
                    }
                    else
                    {
                        // we need to round number such way that it will have two digits after first significant figure.
                        // example: for 0.523139 should be rounded up to 0.523; 0.000412355 to 0.000412; etc.
                        // how do we calculate the number of digits?
                        //
                        // first things first, we need to calculate the one-based index of first significant figure after point.
                        // for numbers like 0.1, 0.00001 it's pretty easy - just take absolute value of log10.
                        // example: abs(log10(0.1)) = 1; abs(log10(0.00001)) = 5.
                        //
                        // second, for numbers like 0.523139 or 0.000412355 we'll also need to take their ceiling.
                        // example: abs(log10(0.523139)) ~ 0.28, ceiling(0.28) = 1;
                        // example: abs(log10(0.000412355)) ~ 3.38, ceiling(3.38) = 4.
                        // 
                        // and finally, by condition we need to have two digits after first signinficant figure,
                        // so we just add 2 to the result.

                        digitsToLeave = (int)Math.Ceiling(Math.Abs(Math.Log10(Math.Abs(numberToRoundUp)))) + 2;
                    }
                }
            }

            return Math.Round(numberToRoundUp, digitsToLeave);
        }
    }
}
