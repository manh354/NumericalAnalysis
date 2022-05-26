
using System;
using System.Collections.Generic;
using System.Linq;
using SMath = System.Math;

namespace NumericalAnalysis
{
    class PolynomialSolver
    {

        /// <summary>
        /// Find roots recursively
        /// </summary>
        /// <param name="coef"> coefficent : a0 + a1.x + a2.x^2 + .... + an.x^n = 0.</param>
        /// <param name="roots"></param>
        /// <returns></returns>
        public static bool PolySolverRecursive(double[] coef, double eps, out Dictionary<int, double> roots)
        {
            roots = new Dictionary<int, double>();
            if (coef.Length < 2)
            {
                Console.WriteLine("INVALID: ARRAY LENGTH < 2");
                roots = null;
                return false;
            }
            //base case
            if (coef.Length == 2)
            {
                roots.Add(0, -coef[0] / coef[1]);
                return true;
            }
            //Main recursive loop
            if (PolySolverRecursive(PolyDerivative(coef), eps, out roots))
            {
                BisectionRootsFinder(coef, roots, eps, out roots);
                return true;
            }
            else return false;

        }


        public static double[] PolyDerivative(double[] coef)
        {
            double[] new_coef = new double[coef.Length - 1];
            for (int i = 0; i < coef.Length - 1; i++)
            {
                new_coef[i] = coef[i + 1] * (i + 1);
            }
            return new_coef;
        }

        public static bool BisectionRootsFinder(double[] coef, Dictionary<int, double> extremums, double eps, out Dictionary<int, double> roots)
        {
            roots = new Dictionary<int, double>();
            int num = extremums.Count + 1; //Number of iterations.
            double root_radius = 1 + SMath.Abs(coef.Max()) / coef[coef.Length - 1];
            int index = 0;
            for (int i = 0; i < num; i++)
            {
                double a, b;
                a = (i == 0) ? -root_radius : extremums[i - 1];
                b = (i + 1 == num) ? root_radius : extremums[i];
                if (BisectionRootFinder(coef, a, b, eps, out double root))
                {
                    roots.Add(index, root);
                    ++index;
                }
            }
            return true;
        }

        public static bool BisectionRootFinder(double[] coef, double a, double b, double eps, out double root)
        {
            int maxItr = (int)SMath.Log2(1 / eps) * 500;
            if (PolyValueCalc(coef, a) * PolyValueCalc(coef, b) > 0)
            {
                root = double.NaN;
                return false;
            };
            int sign = SMath.Sign(PolyValueCalc(coef, a));
            int itr = 0;
            double c, z;
            do
            {
                itr++;
                c = (a + b) / 2;
                z = PolyValueCalc(coef, c);
                if (z == 0)
                {
                    root = c;
                    return true;
                }
                if (z * sign <= 0) b = c;
                else a = c;
                if (itr > maxItr)
                {
                    root = float.NaN;
                    return false;
                }
            } while (SMath.Abs(b - a) >= eps);
            root = c;
            return true;
        }


        /// <summary>
        /// Calculate value of Polynomial at x.
        /// </summary>
        /// <param name="coef">Array of coefficients: a0, a1,..., an </param>
        /// <param name="x">Value of x</param>
        /// <returns></returns>
        public static double PolyValueCalc(double[] coef, double x)
        {
            double sum = 0;
            double temp = 1;
            for (int i = 0; i < coef.Length; i++)
            {
                sum += coef[i] * temp;
                temp *= x;
            }
            return sum;
        }
    }
}
