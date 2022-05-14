using System;
using System.Collections.Generic;
using SMath = System.Math;

namespace NumericalAnalysis
{
    class LowPrecision
    {
        public static void BT1()
        {
            double a, b, eps;
            Console.WriteLine("Input a,b,eps: ");
            double.TryParse(Console.ReadLine(), out a);
            double.TryParse(Console.ReadLine(), out b);
            double.TryParse(Console.ReadLine(), out eps);
            int sign = SMath.Sign(SMath.Log(a) - 1);
            firstStep:
            double c = (a + b) / 2;
            double z = System.Math.Log(c) - 1;
            if (z == 0)
            {
                Console.WriteLine("x= " + c);
                return;
            }
            if (z * sign < 0) { b = c; }
            else a = c;
            if (SMath.Abs(b - a) < eps)
            {
                Console.WriteLine("x= " + c);
                return;
            }
            else goto firstStep;
        }


        public static void BT2()
        {
            double a, b, eps;

            Console.WriteLine("Input a,b,eps: ");
            double.TryParse(Console.ReadLine(), out a);
            double.TryParse(Console.ReadLine(), out b);
            double.TryParse(Console.ReadLine(), out eps);
            int sign = SMath.Sign(SMath.Tan(a / 4) - 1);
            firstStep:
            double c = (a + b) / 2;
            double z = SMath.Tan(c / 4) - 1;
            if (z < eps)
            {
                Console.WriteLine("x= " + a);
                return;
            }
            if (z * sign < 0) { b = c; }
            else a = c;
            if (SMath.Abs(b - a) < eps)
            {
                Console.WriteLine("x= " + c);
                return;
            }
            else goto firstStep;
        }



        public static void BT3()
        {
            int n;
            double a;
            double eps;
            Console.WriteLine("Input n,a,eps: ");
            Int32.TryParse(Console.ReadLine(), out n);
            double.TryParse(Console.ReadLine(), out a);
            double.TryParse(Console.ReadLine(), out eps);
            double x = 0, y = a;
            int sign = SMath.Sign(SMath.Pow(x, n) - a);
            firstStep:
            double c = (x + y) / 2;
            double z = SMath.Pow(c, n) - a;
            if (z < eps)
            {
                Console.WriteLine("x= " + a);
                return;
            }
            if (z * sign < 0) { y = c; }
            else x = c;
            if (SMath.Abs(y - x) < eps)
            {
                Console.WriteLine("x= " + c);
                return;
            }
            else goto firstStep;
        }

        //Polynomial solver
        public static void BT4()
        {
            Console.WriteLine("Input n, coefficients from a0 to an, eps: ");
            int n; Int32.TryParse(Console.ReadLine(), out n);
            float[] coef = new float[n + 1];
            for (int i = 0; i < n + 1; i++)
            {
                Console.Write("a[{0}]: ", i);
                float.TryParse(Console.ReadLine(), out coef[i]);
            }
            Console.Write("eps: ");
            float eps; float.TryParse(Console.ReadLine(), out eps);
            Dictionary<int, float> result;
            if (PolynomialSolver.PolySolverRecursive(coef, eps, out result))
            {
                if (result.Count == 0)
                {
                    Console.WriteLine("The Given Function Does Not Has Any Real Solutions!");
                    return;
                }
                foreach (KeyValuePair<int, float> a in result)
                {
                    Console.WriteLine("ROOT[{0}]: {1}", a.Key, a.Value);
                }
            }
            return;
        }
    }
}
