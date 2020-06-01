using System;
using System.Collections.Generic;
using System.Linq;

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
            this.z = z.ToList().AsReadOnly();
            this.T = T.ToList().AsReadOnly();
            this.eta = eta.ToList().AsReadOnly();
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

        public Solution RoundUp()
        {
            var F = RoundUpNumber(this.F);
            var Q_CH = RoundUpNumber(this.Q_CH);
            var gamma = RoundUpNumber(this.gamma);
            var q_gamma = RoundUpNumber(this.q_gamma);
            var q_alpha = RoundUpNumber(this.q_alpha);
            var Q = RoundUpNumber(this.Q);
            var T_p = RoundUpNumber(this.T_p);
            var eta_p = RoundUpNumber(this.eta_p);
            var T = this.T.Select(e => RoundUpNumber(e));
            var eta = this.eta.Select(e => RoundUpNumber(e));
            return new Solution(F, Q_CH, gamma, q_gamma, q_alpha, this.N, Q, T_p, eta_p, this.z, T, eta);
        }

        private double RoundUpNumber(double numberToRoundUp, bool forceInteger = false)
        {
            int digitsToLeave;
            if (forceInteger)
            {
                digitsToLeave = 0;
            }
            else
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

                digitsToLeave = (int)Math.Ceiling(Math.Abs(Math.Log10(numberToRoundUp))) + 2;
            }

            return Math.Round(numberToRoundUp, digitsToLeave);
        }
    }
}
