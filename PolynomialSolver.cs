
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
        public static bool PolySolverRecursive(float[] coef, float eps, out Dictionary<int, float> roots)
        {
            roots = new Dictionary<int, float>();
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


        public static float[] PolyDerivative(float[] coef)
        {
            float[] new_coef = new float[coef.Length - 1];
            for (int i = 0; i < coef.Length - 1; i++)
            {
                new_coef[i] = coef[i + 1] * (i + 1);
            }
            return new_coef;
        }

        public static bool BisectionRootsFinder(float[] coef, Dictionary<int, float> extremums, float eps, out Dictionary<int, float> roots)
        {
            roots = new Dictionary<int, float>();
            int num = extremums.Count + 1; //Number of iterations.
            float root_radius = 1 + SMath.Abs(coef.Max()) / coef[coef.Length - 1];
            int index = 0;
            for (int i = 0; i < num; i++)
            {
                float a, b;
                a = (i == 0) ? -root_radius : extremums[i - 1];
                b = (i + 1 == num) ? root_radius : extremums[i];
                if (BisectionRootFinder(coef, a, b, eps, out float root))
                {
                    roots.Add(index, root);
                    ++index;
                }
            }
            return true;
        }

        public static bool BisectionRootFinder(float[] coef, float a, float b, float eps, out float root)
        {
            int maxItr = (int)SMath.Log2(1 / eps) * 500;
            if (PolyValueCalc(coef, a) * PolyValueCalc(coef, b) > 0)
            {
                root = float.NaN;
                return false;
            };
            int sign = SMath.Sign(PolyValueCalc(coef, a));
            int itr = 0;
            float c, z;
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
        public static float PolyValueCalc(float[] coef, float x)
        {
            float sum = 0;
            float temp = 1;
            for (int i = 0; i < coef.Length; i++)
            {
                sum += coef[i] * temp;
                temp *= x;
            }
            return sum;
        }
    }
}
