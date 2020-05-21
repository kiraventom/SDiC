using System;

namespace MathModel.Parameters
{
    /// <summary>
    /// Геометрические параметры канала
    /// </summary>
    public sealed class GeometricParams
    {
        private double w;
        private double h;
        private double l;

        internal GeometricParams(double W, double H, double L)
        {
            this.W = W > 0 ? W : throw new ArgumentOutOfRangeException("Ширина не может быть не положительной");
            this.H = H > 0 ? H : throw new ArgumentOutOfRangeException("Глубина не может быть не положительной");
            this.L = L > 0 ? L : throw new ArgumentOutOfRangeException("Длина не может быть не положительной");
        }
        /// <summary>
        /// Ширина, м
        /// </summary>
        public double W { get => w; set => w = value > 0 ? value : throw new ArgumentOutOfRangeException("Ширина не может быть не положительной"); }
        /// <summary>
        /// Глубина, м
        /// </summary>
        public double H { get => h; set => h = value > 0 ? value : throw new ArgumentOutOfRangeException("Глубина не может быть не положительной"); }
        /// <summary>
        /// Длина, м
        /// </summary>
        public double L { get => l; set => l = value > 0 ? value : throw new ArgumentOutOfRangeException("Длина не может быть не положительной"); }
    }

    /// <summary>
    /// Параметры свойств материала
    /// </summary>
    public sealed class MaterialParams
    {
        private double _ro;
        private double _c;
        private double t_0;

        internal MaterialParams(double ro, double c, double T_0)
        {
            this.ro = ro > 0 ? ro : throw new ArgumentOutOfRangeException("Плотность не может быть не положительной");
            this.c = c > 0 ? c : throw new ArgumentOutOfRangeException("Средняя удельная теплоемкость не может быть не положительной");
            this.T_0 = T_0 > 0 ? T_0 : throw new ArgumentOutOfRangeException("Температура плавления не может быть не положительной");
        }
        /// <summary>
        /// Плотность, кг/м^3
        /// </summary>
        public double ro { get => _ro; set => _ro = value > 0 ? value : throw new ArgumentOutOfRangeException("Плотность не может быть не положительной"); }
        /// <summary>
        /// Средняя удельная теплоемкость, Дж/(кг*С)
        /// </summary>
        public double c { get => _c; set => _c = value > 0 ? value : throw new ArgumentOutOfRangeException("Средняя удельная теплоемкость не может быть не положительной"); }
        /// <summary>
        /// Температура плавления, С
        /// </summary>
        public double T_0 { get => t_0; set => t_0 = value > 0 ? value : throw new ArgumentOutOfRangeException("Температура плавления не может быть не положительной"); }
    }

    /// <summary>
    /// Режимные параметры процесса
    /// </summary>
    public sealed class ProcessParams
    {
        private double v_u;
        private double t_u;

        internal ProcessParams(double V_u, double T_u)
        {
            this.V_u = V_u > 0 ? V_u : throw new ArgumentOutOfRangeException("Скорость крышки не может быть не положительной");
            this.T_u = T_u > 0 ? T_u : throw new ArgumentOutOfRangeException("Температура крышки не может быть не положительной");
        }
        /// <summary>
        /// Скорость крышки, м/с
        /// </summary>
        public double V_u { get => v_u; set => v_u = value > 0 ? value : throw new ArgumentOutOfRangeException("Скорость крышки не может быть не положительной"); }
        /// <summary>
        /// Температура крышки, C
        /// </summary>
        public double T_u { get => t_u; set => t_u = value > 0 ? value : throw new ArgumentOutOfRangeException("Температура крышки не может быть не положительной"); }
    }

    /// <summary>
    /// Эмпирические коэффициенты математической модели
    /// </summary>
    public sealed class EmpiricCoeffs
    {
        private double _mu_0;
        private double _b;
        private double t_r;
        private double _n;
        private double _alpha_u;

        internal EmpiricCoeffs(double mu_0, double b, double T_r, double n, double alpha_u)
        {
            this.mu_0 = mu_0 > 0 ? mu_0 : throw new ArgumentOutOfRangeException("Коэффициент консистенции материала не может быть не положительным");
            this.b = b > 0 ? b : throw new ArgumentOutOfRangeException("Температурный коэффициент вязкости материала не может быть не положительным");
            this.T_r = T_r > 0 ? T_r : throw new ArgumentOutOfRangeException("Температура приведения не может быть не положительной");
            this.n = n > 0 ? n : throw new ArgumentOutOfRangeException("Индекс течения материала не может быть не положительным");
            this.alpha_u = alpha_u > 0 ? alpha_u : throw new ArgumentOutOfRangeException("Коэффициент теплоотдачи от крышки канала к материалу не может быть не положительным");
        }
        /// <summary>
        /// Коэффициент консистенции материала при температуре приведения, Па*с^n
        /// </summary>
        public double mu_0 { get => _mu_0; set => _mu_0 = value > 0 ? value : throw new ArgumentOutOfRangeException("Коэффициент консистенции материала не может быть не положительным"); }
        /// <summary>
        /// Температурный коэффициент вязкости материала, 1/C
        /// </summary>
        public double b { get => _b; set => _b = value > 0 ? value : throw new ArgumentOutOfRangeException("Температурный коэффициент вязкости материала не может быть не положительным"); }
        /// <summary>
        /// Температура приведения, С
        /// </summary>
        public double T_r { get => t_r; set => t_r = value > 0 ? value : throw new ArgumentOutOfRangeException("Температура приведения не может быть не положительной"); }
        /// <summary>
        /// Индекс течения материала
        /// </summary>
        public double n { get => _n; set => _n = value > 0 ? value : throw new ArgumentOutOfRangeException("Индекс течения материала не может быть не положительным"); }
        /// <summary>
        /// Коэффициент теплоотдачи от крышки канала к материалу, Вт/(м^2*С)
        /// </summary>
        public double alpha_u { get => _alpha_u; set => _alpha_u = value > 0 ? value : throw new ArgumentOutOfRangeException("Коэффициент теплоотдачи от крышки канала к материалу не может быть не положительным"); }
    }

    /// <summary>
    /// Параметры метода решения уравнений модели
    /// </summary>
    public sealed class SolveMethodParams
    {
        private double _delta_z;

        internal SolveMethodParams(double delta_z)
        {
            this.delta_z = delta_z > 0 ? delta_z : throw new ArgumentOutOfRangeException("Шаг расчета по длине канала не может быть не положительным");
        }
        /// <summary>
        /// Шаг расчета по длине канала, м
        /// </summary>
        public double delta_z { get => _delta_z; set => _delta_z = value > 0 ? value : throw new ArgumentOutOfRangeException("Шаг расчета по длине канала не может быть не положительным"); }
    }
}