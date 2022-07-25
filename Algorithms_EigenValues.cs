using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalAnalysis
{
    public partial class Algorithms
    {
        public static void Chapter5Main()
        {
            string fileLocation = @"MatrixEigenInput.txt";
            if (!InOutProcessing.MatrixInput(out double[,] matrix, out double[] _, fileLocation))
            {
                Console.WriteLine("Invalid Inputs");
            }
            Algorithms.PrintMatrix(matrix);
            
            DanilevskiMain(matrix);
            
        }

        public static void DanilevskiMain(double[,] matrix)
        {
            Danilevski(matrix);
        }

        /*public bool DanilevskiConvertToFrobenius(double[,] matrix, out double[,] frobenius)
        {
            int i = matrix.GetLength(0);
            int j = matrix.GetLength(1);
            if (i != j)
            {
                Console.WriteLine("Fri");
            }
        }*/
    }
}
