using System;

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
        /// W, ширина, м
        /// </summary>
        public double Width { get => w; set => w = value > 0 ? value : throw new ParameterNotPositiveException("Ширина"); }
        /// <summary>
        /// H, глубина, м
        /// </summary>
        public double Height { get => h; set => h = value > 0 ? value : throw new ParameterNotPositiveException("Глубина"); }
        /// <summary>
        /// L, длина, м
        /// </summary>
        public double Length { get => l; set => l = value > 0 ? value : throw new ParameterNotPositiveException("Длина"); }
    }

    /// <summary>
    /// Параметры свойств материала
    /// </summary>
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
        /// ρ, плотность, кг/м^3
        /// </summary>
        public double Density { get => _ro; set => _ro = value > 0 ? value : throw new ParameterNotPositiveException("Плотность"); }
        /// <summary>
        /// c, средняя удельная теплоемкость, Дж/(кг*С)
        /// </summary>
        public double AverageSpecificHeatCapacity { get => _c; set => _c = value > 0 ? value : throw new ParameterNotPositiveException("Средняя удельная теплоемкость"); }
        /// <summary>
        /// T0, температура плавления, С
        /// </summary>
        public double MeltingTemperature { get => t_0; set => t_0 = value > 0 ? value : throw new ParameterNotPositiveException("Температура плавления"); }
    }

    /// <summary>
    /// Режимные параметры процесса
    /// </summary>
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
        /// Vu, скорость крышки, м/с
        /// </summary>
        public double LidSpeed { get => v_u; set => v_u = value > 0 ? value : throw new ParameterNotPositiveException("Скорость крышки"); }
        /// <summary>
        /// Tu, температура крышки, C
        /// </summary>
        public double LidTemperature { get => t_u; set => t_u = value > 0 ? value : throw new ParameterNotPositiveException("Температура крышки"); }
    }

    /// <summary>
    /// Эмпирические коэффициенты математической модели
    /// </summary>
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
        /// μ0, коэффициент консистенции материала при температуре приведения, Па*с^n
        /// </summary>
        public double ConsistencyCoefficient { get => _mu_0; set => _mu_0 = value > 0 ? value : throw new ParameterNotPositiveException("Коэффициент консистенции материала"); }
        /// <summary>
        /// b, температурный коэффициент вязкости материала, 1/C
        /// </summary>
        public double ViscosityTemperatureCoefficient { get => _b; set => _b = value > 0 ? value : throw new ParameterNotPositiveException("Температурный коэффициент вязкости материала"); }
        /// <summary>
        /// Tr, температура приведения, С
        /// </summary>
        public double CastTemperature { get => t_r; set => t_r = value > 0 ? value : throw new ParameterNotPositiveException("Температура приведения"); }
        /// <summary>
        /// n, индекс течения материала
        /// </summary>
        public double MaterialFlowIndex { get => _n; set => _n = value > 0 ? value : throw new ParameterNotPositiveException("Индекс течения материала"); }
        /// <summary>
        /// αu, коэффициент теплоотдачи от крышки канала к материалу, Вт/(м^2*С)
        /// </summary>
        public double HeatTransferCoefficient { get => _alpha_u; set => _alpha_u = value > 0 ? value : throw new ParameterNotPositiveException("Коэффициент теплоотдачи от крышки канала к материалу"); }
    }

    /// <summary>
    /// Параметры метода решения уравнений модели
    /// </summary>
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
        /// Δz, шаг расчета по длине канала, м
        /// </summary>
        public double CalculationStep { get => _delta_z; set => _delta_z = value > 0 ? value : throw new ParameterNotPositiveException("Шаг расчета по длине канала"); }
    }
}