using MathModel.Parameters;
using System;
using System.Collections.Generic;

namespace MathModel
{
    public sealed class Model
    {
        /// <summary>
        /// Создаёт модель с указаными параметрами
        /// </summary>
        /// <param name="W">Ширина, м</param>
        /// <param name="H">Глубина, м</param>
        /// <param name="L">Длина, м</param>
        /// <param name="ro">Плотность, кг/м^3</param>
        /// <param name="c">Средняя удельная теплоемкость, Дж/(кг*С)</param>
        /// <param name="T_0">Температура плавления, С</param>
        /// <param name="V_u">Скорость крышки, м/с</param>
        /// <param name="T_u">Температура крышки, C</param>
        /// <param name="mu_0">Коэффициент консистенции материала при температуре приведения, Па*с^n</param>
        /// <param name="b">Температурный коэффициент вязкости материала, 1/C</param>
        /// <param name="T_r">Температура приведения, С</param>
        /// <param name="n">Индекс течения материала</param>
        /// <param name="alpha_u">Коэффициент теплоотдачи от крышки канала к материалу, Вт/(м^2*С)</param>
        /// <param name="delta_z">Шаг расчета по длине канала, м</param>
        public Model(double W, double H, double L, double ro, double c, double T_0, double V_u, double T_u, double mu_0,
            double b, double T_r, double n, double alpha_u, double delta_z)
        {
            GeometricParams = new GeometricParams(W, H, L);
            MaterialParams = new MaterialParams(ro, c, T_0);
            ProcessParams = new ProcessParams(V_u, T_u);
            EmpiricCoeffs = new EmpiricCoeffs(mu_0, b, T_r, n, alpha_u);
            SolveMethodParams = new SolveMethodParams(delta_z);
        }

        /// <summary>
        /// Возвращает решение модели
        /// </summary>
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

            return new Solution(F, Q_CH, gamma, q_gamma, q_alpha, N, Q, T_p, eta_p, z, T, eta);
        }

        public GeometricParams GeometricParams { get; }
        public MaterialParams MaterialParams { get; }
        public ProcessParams ProcessParams { get; }
        public EmpiricCoeffs EmpiricCoeffs { get; }
        public SolveMethodParams SolveMethodParams { get; }

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
}
