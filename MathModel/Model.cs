using System;
using System.Collections.Generic;

namespace MathModel
{
    public class Model
    {
        public Model(GeometricParams geometricParams,
                     MaterialParams materialParams,
                     ProcessParams processParams,
                     EmpiricCoeffs empiricCoeffs,
                     SolveMethodParams solveMethodParams)
        {
            GeometricParams = geometricParams;
            MaterialParams = materialParams;
            ProcessParams = processParams;
            EmpiricCoeffs = empiricCoeffs;
            SolveMethodParams = solveMethodParams;
        }

        /// <summary>
        /// Solves the model, described by params
        /// </summary>
        /// <returns>Solution</returns>
        public Solution Solve()
        {
            List<double> z = new List<double>();
            List<double> T = new List<double>();
            List<double> eta = new List<double>();

            double F = Model.F(GeometricParams.H, GeometricParams.W);
            double Q_CH = Model.Q_CH(GeometricParams.H,
                                     GeometricParams.W,
                                     ProcessParams.V_u,
                                     F);
            double gamma = Model.gamma(GeometricParams.H, ProcessParams.V_u);
            double q_gamma = Model.q_gamma(GeometricParams.H,
                                           GeometricParams.W,
                                           EmpiricCoeffs.mu_0,
                                           EmpiricCoeffs.n,
                                           gamma);
            double q_alpha = Model.q_alpha(GeometricParams.W,
                                           ProcessParams.T_u,
                                           EmpiricCoeffs.b,
                                           EmpiricCoeffs.T_r,
                                           EmpiricCoeffs.alpha_u);
            int N = Model.N(GeometricParams.L, SolveMethodParams.delta_z);

            for (int i = 0; i < N; ++i)
            {
                double z_i = Model.z_i(i, SolveMethodParams.delta_z);
                double T_i = Model.T_i(GeometricParams.W,
                                       MaterialParams.ro,
                                       MaterialParams.c,
                                       MaterialParams.T_0,
                                       EmpiricCoeffs.b,
                                       EmpiricCoeffs.T_r,
                                       EmpiricCoeffs.alpha_u,
                                       Q_CH,
                                       q_gamma,
                                       q_alpha,
                                       z_i);
                double eta_i = Model.eta_i(EmpiricCoeffs.mu_0,
                                           EmpiricCoeffs.b,
                                           EmpiricCoeffs.T_r,
                                           EmpiricCoeffs.n,
                                           gamma,
                                           T_i);
                z.Add(z_i);
                T.Add(T_i);
                eta.Add(eta_i);
            }

            double Q = Model.Q(MaterialParams.ro, Q_CH);
            double z_p = Model.z_i(N, SolveMethodParams.delta_z);
            double T_p = Model.T_p(GeometricParams.W,
                                   MaterialParams.ro,
                                   MaterialParams.c,
                                   MaterialParams.T_0,
                                   EmpiricCoeffs.b,
                                   EmpiricCoeffs.T_r,
                                   EmpiricCoeffs.alpha_u,
                                   Q_CH,
                                   q_gamma,
                                   q_alpha,
                                   z_p);
            double eta_p = Model.eta_p(EmpiricCoeffs.mu_0,
                                       EmpiricCoeffs.b,
                                       EmpiricCoeffs.T_r,
                                       EmpiricCoeffs.n,
                                       gamma,
                                       T_p);

            return new Solution(F, Q_CH, gamma, q_gamma, q_alpha, N, z, T, eta);
        }

        public GeometricParams GeometricParams { get; set; }
        public MaterialParams MaterialParams { get; set; }
        public ProcessParams ProcessParams { get; set; }
        public EmpiricCoeffs EmpiricCoeffs { get; set; }
        public SolveMethodParams SolveMethodParams { get; set; }

        #region Functions
        /// <summary>
        /// Расчёт коэффициента геометрической формы канала
        /// </summary>
        /// <param name="H">Глубина, м</param>
        /// <param name="W">Ширина, м</param>
        /// <returns></returns>
        private static double F(double H, double W)
             => (0.125 * Math.Pow(H / W, 2)) - (0.625 * (H / W)) + 1;

        /// <summary>
        /// Расчёт расхода поступательного потока материала через канал, м^3/c
        /// </summary>
        /// <param name="H">Глубина, м</param>
        /// <param name="W">Ширина, м</param>
        /// <param name="V_u">Скорость крышки, м/с</param>
        /// <param name="F">Коэффициент геометрической формы канала</param>
        /// <returns></returns>
        private static double Q_CH(double H, double W, double V_u, double F)
             => ((H * W * V_u) / 2) * F;

        /// <summary>
        /// Расчёт скорости деформации сдвига материала, 1/c
        /// </summary>
        /// <param name="H">Глубина, м</param>
        /// <param name="V_u">Скорость крышки, м/с</param>
        /// <returns></returns>
        private static double gamma(double H, double V_u)
            => V_u / H;

        /// <summary>
        /// Расчёт удельного теплового потока за счёт вязкого трения в потоке, Вт/м
        /// </summary>
        /// <param name="H">Глубина, м</param>
        /// <param name="W">Ширина, м</param>
        /// <param name="mu_0">Коэффициент консистенции материала при температуре приведения, Па*с^n</param>
        /// <param name="n">Индекс течения материала</param>
        /// <param name="gamma">Cкорость деформации сдвига материала</param>
        /// <returns></returns>
        private static double q_gamma(double H, double W, double mu_0, double n, double gamma)
            => H * W * mu_0 * Math.Pow(gamma, n + 1);

        /// <summary>
        /// Расчёт удельного теплового потока за счёт теплообмена с крышкой канала, Вт/м
        /// </summary>
        /// <param name="W">Ширина, м</param>
        /// <param name="T_u">Температура крышки, C</param>
        /// <param name="b">Температурный коэффициент вязкости материала, 1/C</param>
        /// <param name="T_r">Температура приведения, С</param>
        /// <param name="alpha_u">Коэффициент теплоотдачи от крышки канала к материалу, Вт/(м^2*С)</param>
        /// <returns></returns>
        private static double q_alpha(double W, double T_u, double b, double T_r, double alpha_u)
            => W * alpha_u * (1/b - T_u + T_r);

        /// <summary>
        /// Расчёт числа шагов вычислений по длине канала N
        /// </summary>
        /// <param name="L">Длина, м</param>
        /// <param name="delta_z">Шаг расчета по длине канала, м</param>
        /// <returns></returns>
        private static int N(double L, double delta_z)
            => (int)Math.Round(L / delta_z);

        /// <summary>
        /// Расчёт координаты текущего поперечного сечения канала z_i
        /// </summary>
        /// <param name="i">Номер шага</param>
        /// <param name="delta_z">Шаг расчета по длине канала, м</param>
        /// <returns></returns>
        private static double z_i(int i, double delta_z)
            => i * delta_z;

        /// <summary>
        /// Расчёт температуры материала T_i в текущем поперечном сечении канала
        /// </summary>
        /// <param name="W">Ширина, м</param>
        /// <param name="ro">Плотность, кг/м^3</param>
        /// <param name="c">Средняя удельная теплоемкость, Дж/(кг*С)</param>
        /// <param name="T_0">Температура плавления, С</param>
        /// <param name="b">Температурный коэффициент вязкости материала, 1/C</param>
        /// <param name="T_r">Температура приведения, С</param>
        /// <param name="alpha_u">Коэффициент теплоотдачи от крышки канала к материалу, Вт/(м^2*С)</param>
        /// <param name="Q_ch">Расход поступательного потока материала через канал</param>
        /// <param name="q_gamma">Удельный тепловой поток за счёт вязкого трения в потоке</param>
        /// <param name="q_alpha">Удельный тепловой поток за счёт теплообмена с крышкой канала</param>
        /// <param name="z_i">Координата текущего поперечного сечения канала</param>
        /// <returns></returns>
        private static double T_i(double W, double ro, double c, double T_0, double b,
            double T_r, double alpha_u, double Q_ch, double q_gamma, double q_alpha, double z_i)
        {
            double _A = (b * q_gamma + W * alpha_u) / (b * q_alpha);
            double _B = (-b * q_alpha * z_i) / (ro * c * Q_ch);
            double _C = b * (T_0 - T_r - ((q_alpha * z_i) / (ro * c * Q_ch)));
            double _RESULT = T_r + (1/b) * Math.Log(_A * (1 - Math.Pow(Math.E, _B)) + Math.Pow(Math.E, _C));
            return _RESULT;
        }

        /// <summary>
        /// Расчёт вязкости материала η_i в текущем поперечном сечении канала
        /// </summary>
        /// <param name="mu_0">Коэффициент консистенции материала при температуре приведения, Па*с^n</param>
        /// <param name="b">Температурный коэффициент вязкости материала, 1/C</param>
        /// <param name="T_r">Температура приведения, С</param>
        /// <param name="n">Индекс течения материала</param>
        /// <param name="gamma">Cкорость деформации сдвига материала</param>
        /// <param name="T_i">Температура материала в текущем поперечном сечении канала</param>
        /// <returns></returns>
        private static double eta_i(double mu_0, double b, double T_r, double n, double gamma, double T_i)
            => mu_0 * Math.Pow(Math.E, -b * (T_i - T_r)) * Math.Pow(gamma, n - 1);

        /// <summary>
        /// Расчёт производительности канала Q
        /// </summary>
        /// <param name="ro">Плотность, кг/м^3</param>
        /// <param name="Q_ch">Расход поступательного потока материала через канал</param>
        /// <returns></returns>
        private static double Q(double ro, double Q_ch)
            => 3600 * ro * Q_ch;

        /// <summary>
        /// Расчёт температуры T_p продукта
        /// </summary>
        /// <param name="W">Ширина, м</param>
        /// <param name="ro">Плотность, кг/м^3</param>
        /// <param name="c">Средняя удельная теплоемкость, Дж/(кг*С)</param>
        /// <param name="T_0">Температура плавления, С</param>
        /// <param name="b">Температурный коэффициент вязкости материала, 1/C</param>
        /// <param name="T_r">Температура приведения, С</param>
        /// <param name="alpha_u">Коэффициент теплоотдачи от крышки канала к материалу, Вт/(м^2*С)</param>
        /// <param name="Q_ch">Расход поступательного потока материала через канал</param>
        /// <param name="q_gamma">Удельный тепловой поток за счёт вязкого трения в потоке</param>
        /// <param name="q_alpha">Удельный тепловой поток за счёт теплообмена с крышкой канала</param>
        /// <param name="z_n">Координата последнего поперечного сечения канала</param>
        /// <returns></returns>
        private static double T_p(double W, double ro, double c, double T_0, double b, double T_r, double alpha_u,
                                  double Q_ch, double q_gamma, double q_alpha, double z_n)
            => T_i(W, ro, c, T_0, b, T_r, alpha_u, Q_ch, q_gamma, q_alpha, z_n);
    
        /// <summary>
        /// Расчёт вязкости η_p продукта
        /// </summary>
        /// <param name="mu_0">Коэффициент консистенции материала при температуре приведения, Па*с^n</param>
        /// <param name="b">Температурный коэффициент вязкости материала, 1/C</param>
        /// <param name="T_r">Температура приведения, С</param>
        /// <param name="n">Индекс течения материала</param>
        /// <param name="gamma">Cкорость деформации сдвига материала</param>
        /// <param name="T_p">Температура продукта</param>
        /// <returns></returns>
        private static double eta_p(double mu_0, double b, double T_r, double n, double gamma, double T_p)
            => eta_i(mu_0, b, T_r, n, gamma, T_p);

        #endregion
    }

    public class Solution
    {
        public Solution(double F, double Q_CH, double gamma, double q_gamma, double q_alpha, int N,
                        IEnumerable<double> z, IEnumerable<double> T, IEnumerable<double> eta)
        {
            this.F = F;
            this.Q_CH = Q_CH;
            this.gamma = gamma;
            this.q_gamma = q_gamma;
            this.q_alpha = q_alpha;
            this.N = N;
            this.z = z as IReadOnlyCollection<double>;
            this.T = T as IReadOnlyCollection<double>;
            this.eta = eta as IReadOnlyCollection<double>;
        }
        /// <summary>
        /// Коэффициент геометрической формы канала
        /// </summary>
        public double F { get; }
        /// <summary>
        /// Расход поступательного потока материала через канал
        /// </summary>
        public double Q_CH { get; }
        /// <summary>
        /// Cкорость деформации сдвига материала
        /// </summary>
        public double gamma { get; }
        /// <summary>
        /// Удельный тепловой поток за счёт вязкого трения в потоке
        /// </summary>
        public double q_gamma { get; }
        /// <summary>
        /// Удельный тепловой поток за счёт теплообмена с крышкой канала
        /// </summary>
        public double q_alpha { get; }
        /// <summary>
        /// Число шагов вычислений по длине канала
        /// </summary>
        public int N { get; }

        public IReadOnlyCollection<double> z { get; }
        public IReadOnlyCollection<double> T { get; }
        public IReadOnlyCollection<double> eta { get; }
    }

    /// <summary>
    /// Геометрические параметры канала
    /// </summary>
    public class GeometricParams
    {
        public GeometricParams(double W, double H, double L)
        {
            this.W = W > 0 ? W : throw new ArgumentOutOfRangeException("Ширина не может быть не положительной"); 
            this.H = H > 0 ? H : throw new ArgumentOutOfRangeException("Глубина не может быть не положительной");
            this.L = L > 0 ? L : throw new ArgumentOutOfRangeException("Длина не может быть не положительной");
        }
        /// <summary>
        /// Ширина, м
        /// </summary>
        public double W { get; }
        /// <summary>
        /// Глубина, м
        /// </summary>
        public double H { get; }
        /// <summary>
        /// Длина, м
        /// </summary>
        public double L { get; }
    }

    /// <summary>
    /// Параметры свойств материала
    /// </summary>
    public class MaterialParams 
    {
        public MaterialParams(double ro, double c, double T_0)
        {
            this.ro = ro > 0 ? ro : throw new ArgumentOutOfRangeException("Плотность не может быть не положительной");
            this.c = c > 0 ? c : throw new ArgumentOutOfRangeException("Средняя удельная теплоемкость не может быть не положительной");
            this.T_0 = T_0 > 0 ? T_0 : throw new ArgumentOutOfRangeException("Температура плавления не может быть не положительной");
        }
        /// <summary>
        /// Плотность, кг/м^3
        /// </summary>
        public double ro { get; }
        /// <summary>
        /// Средняя удельная теплоемкость, Дж/(кг*С)
        /// </summary>
        public double c { get; }
        /// <summary>
        /// Температура плавления, С
        /// </summary>
        public double T_0 { get; }
    }

    /// <summary>
    /// Режимные параметры процесса
    /// </summary>
    public class ProcessParams
    {
        public ProcessParams(double V_u, double T_u)
        {
            this.V_u = V_u > 0 ? V_u : throw new ArgumentOutOfRangeException("Скорость крышки не может быть не положительной");
            this.T_u = T_u > 0 ? T_u : throw new ArgumentOutOfRangeException("Температура крышки не может быть не положительной");
        }
        /// <summary>
        /// Скорость крышки, м/с
        /// </summary>
        public double V_u { get; }
        /// <summary>
        /// Температура крышки, C
        /// </summary>
        public double T_u { get; }
    }

    /// <summary>
    /// Эмпирические коэффициенты математической модели
    /// </summary>
    public class EmpiricCoeffs
    {
        public EmpiricCoeffs(double mu_0, double b, double T_r, double n, double alpha_u)
        {
            this.mu_0 = mu_0 > 0 ? mu_0 : throw new ArgumentOutOfRangeException("Коэффициент консистенции материала не может быть не положительной");
            this.b = b > 0 ? b : throw new ArgumentOutOfRangeException("Температурный коэффициент вязкости материала не может быть не положительной");
            this.T_r = T_r > 0 ? T_r : throw new ArgumentOutOfRangeException("Температура приведения не может быть не положительной");
            this.n = n > 0 ? n : throw new ArgumentOutOfRangeException("Индекс течения материала не может быть не положительной");
            this.alpha_u = alpha_u > 0 ? alpha_u : throw new ArgumentOutOfRangeException("Коэффициент теплоотдачи от крышки канала к материалу не может быть не положительной");
        }
        /// <summary>
        /// Коэффициент консистенции материала при температуре приведения, Па*с^n
        /// </summary>
        public double mu_0 { get; }
        /// <summary>
        /// Температурный коэффициент вязкости материала, 1/C
        /// </summary>
        public double b { get; }
        /// <summary>
        /// Температура приведения, С
        /// </summary>
        public double T_r { get; }
        /// <summary>
        /// Индекс течения материала
        /// </summary>
        public double n { get; }
        /// <summary>
        /// Коэффициент теплоотдачи от крышки канала к материалу, Вт/(м^2*С)
        /// </summary>
        public double alpha_u { get; }
    }

    /// <summary>
    /// Параметры метода решения уравнений модели
    /// </summary>
    public class SolveMethodParams
    {
        public SolveMethodParams(double delta_z)
        {
            this.delta_z = delta_z > 0 ? delta_z : throw new ArgumentOutOfRangeException("Шаг расчета по длине канала не может быть не положительной");
        }
        /// <summary>
        /// Шаг расчета по длине канала, м
        /// </summary>
        public double delta_z { get; }
    }
}
