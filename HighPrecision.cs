using LongCalc;
using System;
using System.Collections.Generic;

namespace NumericalAnalysis
{
    class HighPrecision
    {
        /// <summary>
        /// Find e by bisection method.
        /// </summary>
        public static void BT1()
        {
            int pres = (int)bf.GlobalPrecision;
            bf a, b, eps;
            Console.WriteLine("Input a,b,eps: ");
            a = new bf(Console.ReadLine());
            b = new bf(Console.ReadLine());
            eps = new bf(Console.ReadLine());
            int sign = (a.Log() - 1).Sign;
            int itr = 0;
            firstStep:
            bf c = (a + b) / 2;
            bf z = c.Log() - 1;

            Console.WriteLine("ITERATION: " + itr + "    e= " + c.toString());

            if (z == 0)
            {
                Console.WriteLine("e= " + c.toString());
                return;
            }
            if (z * sign < 0) { b = c; }
            else a = c;
            if ((b - a).Abs() < eps)
            {
                Console.WriteLine("e= " + c.toString());
                return;
            }
            else
            {
                if (itr > pres * 20)
                {
                    Console.WriteLine("diff= " + z.toString());
                    return;
                }
                itr++;
                goto firstStep;
            }
        }

        /// <summary>
        /// Find e by using Newton method.
        /// </summary>
        public static void NewtonBT1()
        {
            int pres = (int)bf.GlobalPrecision;
            bf seed, eps;
            Console.WriteLine("Input seed,eps: ");
            seed = new bf(Console.ReadLine());
            eps = new bf(Console.ReadLine());
            int itr = 0;
            bf p0 = seed;
            firstStep:
            bf f = p0.Log() - 1;
            bf fd = 1 / p0;
            bf p = p0 - f / fd;
            Console.WriteLine("ITERATION: " + itr + "    e= " + p.toString());
            if ((p - p0).Abs() < eps)
            {
                Console.WriteLine("pi= " + p.toString());
                Console.WriteLine("{0} Iteration(s) done", itr + 1);
                return;
            }
            if (itr > pres * 20)
            {
                Console.WriteLine("diff= " + p.toString());
                return;
            }
            p0 = p;
            itr++;
            goto firstStep;

        }



        /// <summary>
        /// Find e by using False Position method.
        /// </summary>
        public static void FalsePosBT1()
        {
            int pres = (int)bf.GlobalPrecision;
            bf a, b, eps;
            Console.WriteLine("Input a,b,eps: ");
            a = new bf(Console.ReadLine());
            b = new bf(Console.ReadLine());
            eps = new bf(Console.ReadLine());
            /* f(x) = log(x) - 1;
             * f'(x) = 1/x;
             * f"(x) = -1/(x^2);
             *
             */
            bf fa = (a.Log() - 1);
            bf fb = (b.Log() - 1);
            bf ddfa = -1 / (a * a);
            if (fa.Sign * fb.Sign >= 0)
            {
                Console.WriteLine("Not possible to apply false position method");
            }
            bf d, fd, x_0;
            if (fa.Sign * ddfa.Sign > 0)
            {
                d = a;
                fd = fa;
                x_0 = b;
            }
            else
            {
                d = b;
                fd = fb;
                x_0 = a;
            }
            bf M1 = 1.0 / a;
            bf m1 = 1.0 / b;
            int itr = 0;
            firstStep:
            bf fx = x_0.Log() - 1;
            bf x_1 = x_0 - fx * (x_0 - d) / (fx - fd);
            bf deltaX = x_1 - x_0;
            if (deltaX.Abs() <= eps)
            {
                Console.WriteLine("S, itr: {0} ,final result: {1}", itr + 1, x_1.toString());
                Console.WriteLine("diff= " + deltaX.toString());
                return;
            }
            else
            {
                if (itr > pres * 20)
                {
                    Console.WriteLine("F, itr: {0} ,final result: {1}", itr + 1, x_1.toString());
                    Console.WriteLine("diff= " + deltaX.toString());
                    return;
                }
                itr++;
                x_0 = x_1;
                goto firstStep;
            }
        }

        public delegate bf f(bf a, bf b, bf eps, out bf sol);

        public void Bisection() { }

        // Find PI//
        public static void BT2()
        {
            int pres = (int)bf.GlobalPrecision;
            bf a, b, eps;
            Console.WriteLine("Input a,b,eps: ");
            a = new bf(Console.ReadLine());
            b = new bf(Console.ReadLine());
            eps = new bf(Console.ReadLine());
            int sign = ((a / 4).Tan() - 1).Sign;
            int itr = 0;
            firstStep:
            bf c = (a + b) / 2;
            bf z = (c / 4).Tan() - 1;

            Console.WriteLine("ITERATION: " + itr + "    e= " + c.toString());

            if (z == 0)
            {
                Console.WriteLine("pi= " + c.toString());
                return;
            }
            if (z * sign < 0) { b = c; }
            else a = c;
            if ((b - a).Abs() < eps)
            {
                Console.WriteLine("pi= " + c.toString());
                return;
            }
            else
            {
                if (itr > pres * 20)
                {
                    Console.WriteLine("diff= " + z.toString());
                    return;
                }
                itr++;
                goto firstStep;
            }
        }

        public static void NewtonBT2()
        {
            int pres = (int)bf.GlobalPrecision;
            bf seed, eps;
            Console.WriteLine("Input seed,eps: ");
            seed = new bf(Console.ReadLine());
            eps = new bf(Console.ReadLine());
            int itr = 0;
            bf p0 = seed;
            firstStep:
            bf f = (p0 / 4).Tan() - 1;
            bf p0_4_cos = (p0 / 4).Cos();
            bf fd = 0.25 / (p0_4_cos * p0_4_cos);
            bf p = p0 - f / fd;

            Console.WriteLine("ITERATION: " + itr + "    pi= " + p.toString());
            if ((p - p0).Abs() < eps)
            {
                Console.WriteLine("pi= " + p.toString());
                Console.WriteLine("{0} Iteration(s) done", itr + 1);
                return;
            }
            else
            {
                if (itr > pres * 20)
                {
                    Console.WriteLine("diff= " + p.toString());
                    return;
                }
                p0 = p;
                itr++;
                goto firstStep;
            }
        }
        public static void FalsePosBT2()
        {
            int pres = (int)bf.GlobalPrecision;
            bf a, b, eps;
            Console.WriteLine("Input a,b,eps: ");
            a = new bf(Console.ReadLine());
            b = new bf(Console.ReadLine());
            eps = new bf(Console.ReadLine());
            /* f(x) = tan(a/4)-1;
             * f'(x) = 0.25/cos^2(a/4);
             * f"(x) = 0.125*tan(a/4)/cos^2(a/4);
             *
             */
            bf fa = (a.Log() - 1);
            bf fb = (b.Log() - 1);
            bf ddfa = -1 / (a * a);
            if (fa.Sign * fb.Sign >= 0)
            {
                Console.WriteLine("Not possible to apply false position method");
            }
            bf d, fd, x_0;
            if (fa.Sign * ddfa.Sign > 0)
            {
                d = a;
                fd = fa;
                x_0 = b;
            }
            else
            {
                d = b;
                fd = fb;
                x_0 = a;
            }
            bf M1 = 1.0 / a;
            bf m1 = 1.0 / b;
            int itr = 0;
            firstStep:
            bf fx = x_0.Log() - 1;
            bf x_1 = x_0 - fx * (x_0 - d) / (fx - fd);
            bf deltaX = x_1 - x_0;
            if (deltaX.Abs() <= eps)
            {
                Console.WriteLine("S, itr: {0} ,final result: {1}", itr + 1, x_1.toString());
                Console.WriteLine("diff= " + deltaX.toString());
                return;
            }
            else
            {
                if (itr > pres * 20)
                {
                    Console.WriteLine("F, itr: {0} ,final result: {1}", itr + 1, x_1.toString());
                    Console.WriteLine("diff= " + deltaX.toString());
                    return;
                }
                itr++;
                x_0 = x_1;
                goto firstStep;
            }
        }


        public static void BT4()
        {
            Console.WriteLine("Input n, coefficients from a0 to an, eps: ");
            int n; Int32.TryParse(Console.ReadLine(), out n);
            bf[] coef = new bf[n + 1];
            for (int i = 0; i < n + 1; i++)
            {
                Console.Write("a[{0}]: ", i);
                coef[i] = Console.ReadLine();
            }
            Console.Write("eps: ");
            bf eps = Console.ReadLine();
            Dictionary<int, bf> result;
            if (PolynomialSolver.PolySolverRecursive(coef, eps, out result))
            {
                if (result.Count == 0)
                {
                    Console.WriteLine("The Given Function Does Not Have Any Real Solutions!");
                    return;
                }
                foreach (KeyValuePair<int, bf> a in result)
                {
                    Console.WriteLine("|-------ROOT[{0}]-------| {1}", a.Key, a.Value.toString());
                }
            }
            return;
        }

    }
}
