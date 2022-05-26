using System;
using System.Collections.Generic;
using System.Linq;

namespace NumericalAnalysis
{
    public partial class Algorithms
    {
        #region GradientIterator

        public static bool GradientIterator(double a, double b, double seed, out double? x, out double? y, Func<double, double> f, double eps = 10e-7, double gamma = 0.1, bool findMin = true)
        {
            List<double> xFound = new List<double>();
            xFound.Add(a);
            xFound.Add(b);
            double x0, x1 = seed;
            double y0, y1;
            int itr = 0, itrMax = Convert.ToInt32(Math.Sqrt(1 / eps));

            // Gradient Descent

            if (findMin)
            {
                do
                {
                    if (itr > itrMax) // descent two long => stop
                    {
                        x = null;
                        y = null;
                        return false;
                    }
                    x0 = x1;
                    if (!nth_Derivative(x0, f, 1, out double? df, eps))
                    {
                        x = null;
                        y = null;
                        return false;
                    }

                    x1 = x0 - gamma * df.Value; //MAIN

                    if (x1 > b || x1 < a) // descent out of range => stop
                    {
                        x = null;
                        y = null;
                        return false;
                    }
                    y1 = f(x1);
                    y0 = f(x0);

                } while (Math.Abs(y0 - y1) > eps);
                y = y1;
                x = x1;
                return true;
            }

            // Gradient Acsend

            else
            {
                do
                {
                    if (itr > itrMax)
                    {
                        x = null;
                        y = null;
                        return false;
                    }
                    x0 = x1;
                    if (!nth_Derivative(x0, f, 1, out double? df, eps))
                    {
                        x = null;
                        y = null;
                        return false;
                    }

                    x1 = x0 + gamma * df.Value; //MAIN

                    y1 = f(x1);
                    y0 = f(x0);

                } while (Math.Abs(y0 - y1) < eps);
                y = y1;
                x = x1;
                return true;
            }

        }

        public static bool FindMax(double a, double b, out double? xMax, out double? yMax, Func<double, double> f, double eps = 10e-7, int step = 100, bool safe = false)
        {
            if (step == 0)
            {
                xMax = null;
                yMax = null;
                return false;
            }
            List<(double, double)> values = new List<(double, double)>(5);
            values.Add((a, f(a)));
            values.Add((b, f(b)));
            double seed = a;
            int bias = 1;
            double gamma = (b - a) / step;
            while ((seed += bias * (b - a) / step) < b)
            {
                double? x, y;
                if (GradientIterator(a, b, seed, out x, out y, f, eps, gamma * 10, false))
                {
                    if (Math.Abs(x.Value - values[values.Count() - 1].Item1) < eps)
                    {
                        bias++;
                        if (seed < x.Value)
                            seed = x.Value;
                        continue;
                    }
                    values.Add((x.Value, y.Value));
                    if (bias > 1) bias--;
                    continue;
                }
            }
            xMax = double.MinValue;
            yMax = double.MinValue;
            foreach ((double, double) pair in values)
            {
                if (pair.Item2 > yMax)
                {
                    xMax = pair.Item1;
                    yMax = pair.Item2;
                }
            }
            Console.WriteLine("yMax: {0}", yMax);
            return true;
        }

        public static bool FindMin(double a, double b, out double? xMax, out double? yMax, Func<double, double> f, double eps = 10e-7, int step = 100, bool safe = false)
        {
            if (step == 0)
            {
                xMax = null;
                yMax = null;
                return false;
            }
            List<(double, double)> values = new List<(double, double)>(5);
            values.Add((a, f(a)));
            values.Add((b, f(b)));
            double seed = a;
            int bias = 1;
            double gamma = (b - a) / step;
            while ((seed += bias * (b - a) / step) < b)
            {
                double? x, y;
                if (GradientIterator(a, b, seed, out x, out y, f, eps, gamma * 10, true))
                {
                    if (Math.Abs(x.Value - values[values.Count()].Item1) < eps)
                    {
                        bias++;
                        if (seed < x.Value)
                        {
                            seed = x.Value;
                        }
                        continue;
                    }
                    values.Add((x.Value, y.Value));
                    if (bias > 1) bias--;
                    continue;
                }
                else bias++;
            }
            xMax = double.MaxValue;
            yMax = double.MaxValue;
            foreach ((double, double) pair in values)
            {
                if (pair.Item2 < yMax)
                {
                    xMax = pair.Item1;
                    yMax = pair.Item2;
                }
            }
            return true;
        }

        public static bool FindBothYMaxMin(double a, double b, out double? yMax, out double? yMin, Func<double, double> f, double eps = 10e-7, int step = 100, bool safe = false)
        {
            double? x;
            if (!FindMax(a, b, out x, out yMax, f, eps, step, safe) ||
                !FindMin(a, b, out x, out yMin, f, eps, step, safe))
            {
                yMin = null;
                yMax = null;
                return false;
            }
            return true;
        }
        #endregion

        public static void AbsMaxMin(double max, double min, out double absMax, out double absMin)
        {
            if (max * min <= 0)
            {
                double temp1 = Math.Abs(max), temp2 = Math.Abs(min);
                max = temp1 > temp2 ? temp1 : temp2;
                min = 0;
            }
            else
            {
                double temp1 = Math.Abs(max), temp2 = Math.Abs(min);
                max = temp1 > temp2 ? temp1 : temp2;
                min = temp1 > temp2 ? temp2 : temp1;
            }
            absMax = max; absMin = min;
        }
        public static void AbsMaxMin(double? max, double? min, out double? absMax, out double? absMin)
        {
            if (max == null || min == null) { absMax = null; absMin = null; return; }
            if (max * min <= 0)
            {
                double temp1 = Math.Abs(max.Value), temp2 = Math.Abs(min.Value);
                max = temp1 > temp2 ? temp1 : temp2;
                min = 0;
            }
            else
            {
                double temp1 = Math.Abs(max.Value), temp2 = Math.Abs(min.Value);
                max = temp1 > temp2 ? temp1 : temp2;
                min = temp1 > temp2 ? temp2 : temp1;
            }
            absMax = max; absMin = min;
        }

    }
}
