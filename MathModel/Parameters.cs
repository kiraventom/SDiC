using System;
using System.ComponentModel.DataAnnotations;

namespace MathModel.Parameters
{
    internal sealed class ParameterNotPositiveException : Exception
    {
        public ParameterNotPositiveException(string parameterName)
        {
            throw new Exception(ExceptionStringHead + parameterName + ExceptionStringTail);
        }

        private const string ExceptionStringHead = "Значение параметра \"";
        private const string ExceptionStringTail = "\" не может быть не положительным";
    }

    public abstract class Params
    {

    }

    /// <summary>
    /// Геометрические параметры канала
    /// </summary>
    [Display(Name = "GeometricParams", Description = "Геометрические параметры канала")]
    public sealed class GeometricParams : Params
    {
        private double w;
        private double h;
        private double l;

        internal GeometricParams() { }

        /// <param name="W">W, ширина, м</param>
        /// <param name="H">H, глубина, м</param>
        /// <param name="L">L, длина, м</param>
        public GeometricParams(double W, double H, double L)
        {
            this.Width = W;
            this.Height = H;
            this.Length = L;
        }
        /// <summary>
        /// Ширина, м
        /// </summary>
        [Display(Name = "Width", Description = "Ширина, м")]
        public double Width { get => w; set => w = value > 0 ? value : throw new ParameterNotPositiveException("Ширина"); }
        /// <summary>
        /// Глубина, м
        /// </summary>
        [Display(Name = "Height", Description = "Глубина, м")]
        public double Height { get => h; set => h = value > 0 ? value : throw new ParameterNotPositiveException("Глубина"); }
        /// <summary>
        /// Длина, м
        /// </summary>
        [Display(Name = "Length", Description = "Длина, м")]
        public double Length { get => l; set => l = value > 0 ? value : throw new ParameterNotPositiveException("Длина"); }
    }

    /// <summary>
    /// Параметры свойств материала
    /// </summary>
    [Display(Name = "MaterialParams", Description = "Параметры свойств материала")]
    public sealed class MaterialParams : Params
    {
        private double _ro;
        private double _c;
        private double t_0;

        internal MaterialParams() { }

        /// <param name="ro">ρ, плотность, кг/м^3</param>
        /// <param name="c">c, средняя удельная теплоемкость, Дж/(кг*С)</param>
        /// <param name="T_0">T0, температура плавления, С</param>
        public MaterialParams(double ro, double c, double T_0)
        {
            this.Density = ro;
            this.AverageSpecificHeatCapacity = c;
            this.MeltingTemperature = T_0;
        }
        /// <summary>
        /// Плотность, кг/м^3
        /// </summary>
        [Display(Name = "Density", Description = "Плотность, кг/м^3")]
        public double Density { get => _ro; set => _ro = value > 0 ? value : throw new ParameterNotPositiveException("Плотность"); }
        /// <summary>
        /// Cредняя удельная теплоемкость, Дж/(кг*С)
        /// </summary>
        [Display(Name = "AverageSpecificHeatCapacity", Description = "Cредняя удельная теплоемкость, Дж/(кг*С)")]
        public double AverageSpecificHeatCapacity { get => _c; set => _c = value > 0 ? value : throw new ParameterNotPositiveException("Средняя удельная теплоемкость"); }
        /// <summary>
        /// Температура плавления, С
        /// </summary>
        [Display(Name = "MeltingTemperature", Description = "Температура плавления, С")]
        public double MeltingTemperature { get => t_0; set => t_0 = value > 0 ? value : throw new ParameterNotPositiveException("Температура плавления"); }
    }

    /// <summary>
    /// Режимные параметры процесса
    /// </summary>
    [Display(Name = "ProcessParams", Description = "Режимные параметры процесса")]
    public sealed class ProcessParams : Params
    {
        private double v_u;
        private double t_u;

        internal ProcessParams() { }

        /// <param name="V_u">Vu, скорость крышки, м/с</param>
        /// <param name="T_u">Tu, температура крышки, C</param>
        public ProcessParams(double V_u, double T_u)
        {
            this.LidSpeed = V_u;
            this.LidTemperature = T_u;
        }
        /// <summary>
        /// Cкорость крышки, м/с
        /// </summary>
        [Display(Name = "LidSpeed", Description = "Cкорость крышки, м/с")]
        public double LidSpeed { get => v_u; set => v_u = value > 0 ? value : throw new ParameterNotPositiveException("Скорость крышки"); }
        /// <summary>
        /// Температура крышки, C
        /// </summary>
        [Display(Name = "LidTemperature", Description = "Температура крышки, C")]
        public double LidTemperature { get => t_u; set => t_u = value > 0 ? value : throw new ParameterNotPositiveException("Температура крышки"); }
    }

    /// <summary>
    /// Эмпирические коэффициенты математической модели
    /// </summary>
    [Display(Name = "EmpiricCoeffs", Description = "Эмпирические коэффициенты математической модели")]
    public sealed class EmpiricCoeffs : Params
    {
        private double _mu_0;
        private double _b;
        private double t_r;
        private double _n;
        private double _alpha_u;

        internal EmpiricCoeffs() { }

        /// <param name="mu_0">μ0, коэффициент консистенции материала при температуре приведения, Па*с^n</param>
        /// <param name="b">b, температурный коэффициент вязкости материала, 1/C</param>
        /// <param name="T_r">Tr, температура приведения, С</param>
        /// <param name="n">n, индекс течения материала</param>
        /// <param name="alpha_u">αu, коэффициент теплоотдачи от крышки канала к материалу, Вт/(м^2*С)</param>
        public EmpiricCoeffs(double mu_0, double b, double T_r, double n, double alpha_u)
        {
            this.ConsistencyCoefficient = mu_0;
            this.ViscosityTemperatureCoefficient = b;
            this.CastTemperature = T_r;
            this.MaterialFlowIndex = n;
            this.HeatTransferCoefficient = alpha_u;
        }
        /// <summary>
        /// Коэффициент консистенции материала при температуре приведения, Па*с^n
        /// </summary>
        [Display(Name = "ConsistencyCoefficient", Description = "Коэффициент консистенции материала при температуре приведения, Па*с^n")]
        public double ConsistencyCoefficient { get => _mu_0; set => _mu_0 = value > 0 ? value : throw new ParameterNotPositiveException("Коэффициент консистенции материала"); }
        /// <summary>
        /// Температурный коэффициент вязкости материала, 1/C
        [Display(Name = "ViscosityTemperatureCoefficient", Description = "Температурный коэффициент вязкости материала, 1/C")]
        /// </summary>
        public double ViscosityTemperatureCoefficient { get => _b; set => _b = value > 0 ? value : throw new ParameterNotPositiveException("Температурный коэффициент вязкости материала"); }
        /// <summary>
        /// Температура приведения, С
        /// </summary>
        [Display(Name = "CastTemperature", Description = "Температура приведения, С")]
        public double CastTemperature { get => t_r; set => t_r = value > 0 ? value : throw new ParameterNotPositiveException("Температура приведения"); }
        /// <summary>
        /// Индекс течения материала
        /// </summary>
        [Display(Name = "MaterialFlowIndex", Description = "Индекс течения материала")]
        public double MaterialFlowIndex { get => _n; set => _n = value > 0 ? value : throw new ParameterNotPositiveException("Индекс течения материала"); }
        /// <summary>
        /// Коэффициент теплоотдачи от крышки канала к материалу, Вт/(м^2*С)
        /// </summary>
        [Display(Name = "HeatTransferCoefficient", Description = "Коэффициент теплоотдачи от крышки канала к материалу, Вт/(м^2*С)")]
        public double HeatTransferCoefficient { get => _alpha_u; set => _alpha_u = value > 0 ? value : throw new ParameterNotPositiveException("Коэффициент теплоотдачи от крышки канала к материалу"); }
    }

    /// <summary>
    /// Параметры метода решения уравнений модели
    /// </summary>
    [Display(Name = "SolveMethodParams", Description = "Параметры метода решения уравнений модели")]
    public sealed class SolveMethodParams : Params
    {
        private double _delta_z;

        internal SolveMethodParams() { }

        /// <param name="delta_z">Δz, шаг расчета по длине канала, м</param>
        public SolveMethodParams(double delta_z)
        {
            this.CalculationStep = delta_z;
        }
        /// <summary>
        /// Шаг расчета по длине канала, м
        /// </summary>
        [Display(Name = "CalculationStep", Description = "Шаг расчета по длине канала, м")]
        public double CalculationStep { get => _delta_z; set => _delta_z = value > 0 ? value : throw new ParameterNotPositiveException("Шаг расчета по длине канала"); }
    }
}