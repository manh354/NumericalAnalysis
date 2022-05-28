using System;
using System.Collections.Generic;

namespace NumericalAnalysis
{
    class LowPrecision
    {

        //Polynomial solver
        public static void BT4()
        {
            double? eps;
            InOutProcessing.PolyInput(out List<double> coefs, out eps);
            double[] coef = coefs.ToArray();
            Dictionary<int, double> result;
            if (PolynomialSolver.PolySolverRecursive(coef, eps.Value, out result))
            {
                if (result.Count == 0)
                {
                    Console.WriteLine("Phương trình vô nghiệm!");
                    return;
                }
                foreach (KeyValuePair<int, double> a in result)
                {
                    Console.WriteLine("NGHIỆM[{0}]: {1}", a.Key, a.Value);
                }
            }
            return;
        }
    }
}
