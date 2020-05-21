using System.Collections.Generic;

namespace MathModel
{
    public sealed class Solution
    {
        internal Solution(double F, double Q_CH, double gamma, double q_gamma, double q_alpha, int N, double Q, double T_p, double eta_p,
                        IEnumerable<double> z, IEnumerable<double> T, IEnumerable<double> eta)
        {
            this.F = F;
            this.Q_CH = Q_CH;
            this.gamma = gamma;
            this.q_gamma = q_gamma;
            this.q_alpha = q_alpha;
            this.N = N;
            this.Q = Q;
            this.T_p = T_p;
            this.eta_p = eta_p;
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
        /// <summary>
        /// Производительность канала Q
        /// </summary>
        public double Q { get; }
        /// <summary>
        /// Температура продукта
        /// </summary>
        public double T_p { get; }
        /// <summary>
        /// Вязкость продукта
        /// </summary>
        public double eta_p { get; }

        public IReadOnlyCollection<double> z { get; }
        public IReadOnlyCollection<double> T { get; }
        public IReadOnlyCollection<double> eta { get; }
    }
}
