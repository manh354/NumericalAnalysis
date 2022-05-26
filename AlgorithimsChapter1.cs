using System;

namespace NumericalAnalysis
{
    public partial class Algorithms
    {

        public static bool InputProcessing<T>(object? a, object? b, Func<T, T> f, Func<T, T> df = null, Func<T, T> ddf = null, bool checkDF = false, bool checkDDF = false)
        {
            if (checkDF)
                if (df == null) return true;
            if (checkDDF)
                if (ddf == null) return false;
            if (a != null && b != null && f != null)
                return true;
            return false;
        }


        public static void Chapter1Main()
        {
            double? a, b, eps;
            NAFunc _f, _df, _ddf;
            while (!InOutProcessing.FunctionInput(out a, out b, out eps, out _f, out _df, out _ddf))
            {
                Console.WriteLine(Properties._string.PleaseRecheckInput);
                Console.ReadLine();
            }
            Func<double, double> f = _f.ToFunc(), df = _df.ToFunc(), ddf = _ddf.ToFunc();
            Console.WriteLine(Properties._string.ConvertInputSuccess);
            Console.WriteLine(Properties._string.TypeInChapter1Switch);
            string s1 = Console.ReadLine();
            double? root = null;
            switch (s1)
            {
                case "1":
                    if (!BisectionMethod(a, b, out root, f, eps.Value))
                        Console.WriteLine(Properties._string.FailedChapter1_Bisection);
                    Console.WriteLine("Root: {0}", root);
                    break;
                case "2":
                    if (!FalsePositionMethod(a, b, out root, f, df, ddf, eps.Value))
                        Console.WriteLine(Properties._string.FailedChapter1_FalsePos);
                    Console.WriteLine("Root: {0}", root);

                    break;
                case "3":
                    if (!NewtonMethod(a, b, out root, f, df, ddf, eps.Value))
                        Console.WriteLine(Properties._string.FailedChapter1_Newton);
                    Console.WriteLine("Root: {0}", root);
                    break;
                case "4":
                    double temp, temp2;
                    Console.WriteLine("Input starting position:");
                    double.TryParse(Console.ReadLine(), out temp);
                    Console.WriteLine("Input functions q:");
                    double.TryParse(Console.ReadLine(), out temp2);
                    if (!SingularIterative(a, b, temp, out root, f, eps.Value, temp2))
                        Console.WriteLine(Properties._string.FailedChapter1_SingularIterative);
                    break;
            }
        }


        #region Bisection

        public static bool BisectionMethod(double? _a, double? _b, out double? root, Func<double, double> f, double eps = 1e-10)
        {
            if (!InputProcessing(_a, _b, f))
            {
                root = null;
                return false;
            }
            double a = _a.Value;
            double b = _b.Value;
            double c = 0;
            int itr = 0; int maxItr = 5 * (int)(1 / eps);
            while (Math.Abs(a - b) >= eps)
            {
                if (itr > maxItr)
                {
                    root = null;
                    return false;
                }
                c = (a + b) / 2;
                double fa = f(a); int Sign_fa = Math.Sign(fa);
                double fc = f(c);
                if (Math.Abs(f(c)) < eps)
                {
                    root = c;
                    return true;
                }
                if (Sign_fa * fc > 0)
                {
                    a = c;
                }
                else b = c;
                itr++;
                Console.WriteLine("iteration {0}: c = {1}", itr, c);
            }
            root = c;
            return true;
        }


        #endregion

        #region FalsePosition

        public static bool FalsePositionMethod(double? _a, double? _b, out double? root, Func<double, double> f, Func<double, double> df, Func<double, double> ddf, double eps = 1e-10)
        {
            if (!InputProcessing(_a, _b, f, df, ddf, true, true))
            {
                root = null;
                return false;
            }
            double a = _a.Value;
            double b = _b.Value;
            double fa = f(a), fb = f(b);
            int fa_sign = Math.Sign(fa), fb_sign = Math.Sign(fa);

            //Find Max and min of df
            double? m1df = null, M1df = null, m1ddf = null, M1ddf = null;
            bool? dfCS = null, ddfCS = null;

            if (!FindBothYMaxMin(a, b, out m1df, out M1df, df, eps, safe: false))
            {
                Console.WriteLine("Err: N-A1");
            };
            if (!FindBothYMaxMin(a, b, out m1ddf, out M1ddf, ddf, eps, safe: false))
            {
                Console.WriteLine("Err: N-A2");
            };

            if (m1df.Value * M1df.Value <= 0 || m1ddf.Value * M1ddf.Value <= 0)
            {
                Console.WriteLine("Err: N-B1");
                root = null;
                return false;
            }

            AbsMaxMin(M1df, m1df, out M1df, out m1df);

            double eps0 = eps * m1df.Value / (Math.Abs(M1df.Value) - m1df.Value);
            Console.WriteLine("eps0:{0} ", eps0);

            double d, fd, x0, x1;

            if (fa_sign * ddf(a) > 0)
            {
                d = a;
                fd = fa;
                x0 = b;
            }
            else
            {
                d = b;
                fd = fb;
                x0 = a;
            }

            int itr = 0; int itrMax = Convert.ToInt32(1d / eps);

            x1 = x0 - f(x0) * (x0 - d) / (f(x0) - fd);

            while (Math.Abs(x1 - x0) >= eps0)
            {
                if (itr > itrMax)
                {
                    root = null;
                    return false;
                }
                x0 = x1;
                x1 = x0 - f(x0) * (x0 - d) / (f(x0) - fd);
                Console.WriteLine("iteration {0}: x1 = {1}", itr, x1);
                itr++;
            }
            root = x1;
            return true;
        }

        #endregion

        #region NewtonMethod

        public static bool NewtonMethod(double? a, double? b, out double? root, Func<double, double> f, Func<double, double> df, Func<double, double> ddf, double eps)
        {
            if (!InputProcessing(a, b, f, df, ddf, true, true))
            {
                root = null;
                return false;
            }
            int sign = Math.Sign(f(a.Value));
            double _seed = sign > 0 ? a.Value : b.Value;
            double? m1df, M1df;
            double? m1ddf, M1ddf;

            //Processing df and ddf
            if (!FindBothYMaxMin(a.Value, b.Value, out m1df, out M1df, df, eps, safe: false))
            {
                Console.WriteLine("Err: N-A1");
            }

            if (!FindBothYMaxMin(a.Value, b.Value, out m1ddf, out M1ddf, ddf, eps, safe: false))
            {
                Console.WriteLine("Err: N-A2");
            }

            if (m1df.Value * M1df.Value <= 0 || m1ddf.Value * M1ddf.Value <= 0)
            {
                Console.WriteLine("Err: N-B1");
                root = null;
                return false;
            }

            //Processing new epsilon to compare.
            double eps0 = eps * m1df.Value;

            double x_0 = _seed, x_1;
            int itr = 0, itrMax = Convert.ToInt32(Math.Sqrt(1 / eps));

            x_1 = x_0 - f(x_0) / df(x_0);

            while (Math.Abs(f(x_1)) >= eps0)
            {
                if (itr > itrMax)
                {
                    root = null;
                    return false;
                }
                x_0 = x_1;
                x_1 = x_0 - f(x_0) / df(x_0);
                Console.WriteLine("iteration {0}: x1 = {1}", itr, x_1);
                itr++;
            };
            root = x_1;
            return true;
        }

        #endregion

        #region SingularIterative

        /// <summary>
        /// Requires contracting function
        /// </summary>
        /// <param name="_seed"></param>
        /// <param name="root"></param>
        /// <param name="phi"></param>
        /// <returns></returns>
        public static bool SingularIterative(double? a, double? b, double? _seed, out double? root, Func<double, double> phi, double eps, double q)
        {
            if (!InputProcessing(a, b, phi, null, null, true, false))
            {
                root = null;
                return false;
            }
            double x_0 = _seed.Value;

            double eps0 = eps * (q / (1 - q));
            int itr = 0, itrMax = Convert.ToInt32(Math.Sqrt(1 / eps));

            double x_1 = phi(x_0);
            while (Math.Abs(x_1 - x_0) < eps0)
            {
                if (itr > itrMax)
                {
                    root = null;
                    return false;
                }
                x_0 = x_1;
                x_1 = phi(0);
                Console.WriteLine("iteration {0}: x1 = {1}", itr, x_1);
                itr++;
            }
            root = x_1;
            return true;
        }

        #endregion

        #region Utilities

        public static bool nth_Derivative(double? _x, Func<double, double> f, int order, out double? result, double eps = 1e-7d)
        {
            if (f == null || _x == null)
            {
                result = null;
                return false;
            }
            double x = _x.Value;
            if (order > 1)
            {
                double? left, right;
                nth_Derivative(x + eps, f, order - 1, out left, eps);
                nth_Derivative(x - eps, f, order - 1, out right, eps);
                result = (left - right) / (2 * eps);
                return true;
            }
            if (order == 1)
            {
                result = (f(x + eps) - f(x - eps)) / (2 * eps);
                return true;
            }
            if (order == 0)
            {
                result = f(x);
                return true;
            }
            result = null;
            return false;

        }


        #endregion
    }
}
