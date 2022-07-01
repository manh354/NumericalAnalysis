using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalAnalysis
{
    public partial class Algorithms
    {
        public static void Chapter4Main()
        {
            Console.Write("Nhập 1 để chọn phương pháp nghịch đảo bằng PP Gauss");
            string i = Console.ReadLine();
            switch (i)
            {
                case "1":
                    Console.WriteLine("Matrix file location: \"Input.txt\", press any key to continue.");
                    Console.ReadLine();
                    Console.WriteLine("Processing matrix");
                    string fileLocation = @"MatrixInputInverse.txt";
                    double[,] matrix;
                    if (!InOutProcessing.MatrixInput(out matrix, out double[] _, fileLocation))
                    {
                        Console.WriteLine("Invalid Inputs");
                        break;
                    }
                    Algorithms.PrintMatrix(matrix);
                    Algorithms.InverseGaussJordanMain(matrix, out var inversedMatrix);
                    break;
            }
        }

        //////////////////////////////
        //////////////////////////////
        //  INVERSE GAUSS JORDAN    //
        //////////////////////////////
        //..........................//
        public static bool InverseGaussJordanMain(double[,] matrix, out double[,] inversedMatrix, double eps = 1e-6)
        {
            if (!InverseGaussJordan(matrix, out inversedMatrix, out var er,eps))
            {
                Console.WriteLine(er);
                return false;
            }
            PrintMatrix(inversedMatrix);
            return true;
        }
        public static bool InverseGaussJordan(double[,] matrix, out double[,] inversedMatrix, out string er, double eps = 1e-6)
        {
            er = null;
            // Sort 
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1); ; // First position that is different from 0
            
            if(iMax !=jMax)
            {
                inversedMatrix = null;
                er = "Not a square matrix";
                return false;
            }

            double[,] agumentedMatrix = new double[iMax, 2 * jMax];

            //Generating an agumented Matrix
            for (int i = 0; i < iMax; i++)
            {
                for (int j = 0; j < jMax; j++)
                {
                    agumentedMatrix[i, j] = matrix[i, j];
                }
                agumentedMatrix[i, i + jMax] = 1;
            }

            Console.WriteLine("Agumented matrix:");
            PrintMatrix(agumentedMatrix);

            int[] firstPosDif0 =null;

            //Rearrange matrix to a more reversed trapzoid form
            //Main matrix elimination loop
            for (int i = 0; i < iMax; i++)
            {
                // Sort everytime
                UpperTrapezoidSortSwap(null, ref agumentedMatrix, out firstPosDif0);
                for (int di = 0; di < iMax; di++)
                {
                    if (di == i) continue;
                    double mulCoef =0;
                    try
                    {
                        mulCoef = agumentedMatrix[di, firstPosDif0[i]] / agumentedMatrix[i, firstPosDif0[i]];
                    }
                    catch (DivideByZeroException)
                    {
                        inversedMatrix = null;
                        er = "Attempted to divide by 0";
                        return false;
                    }
                    int updateFirstPosDiff = firstPosDif0[di];
                    //Console.WriteLine("frac: " + matrix[di, firstPosDif0[i]]+"\\" + matrix[i, firstPosDif0[i]]);
                    //Console.WriteLine("mul: " + mulCoef);
                    for (int j = 0; j < 2*jMax; j++)
                    {
                        agumentedMatrix[di, j] -= mulCoef * agumentedMatrix[i, j];

                        if (Math.Abs(agumentedMatrix[di, j]) < eps)
                            agumentedMatrix[di, j] = 0;

                        // There is a case in which column i and i+1 have the same value, that means after subtracting 2 rows, there are 2 zeros next to each other.
                        // This if block ensure firstPosDiff is corrected.
                        
                        if (Math.Abs(agumentedMatrix[di, j]) == 0 && Math.Abs(agumentedMatrix[di, updateFirstPosDiff]) == 0 && j == updateFirstPosDiff)
                        {
                            updateFirstPosDiff = j + 1;
                        }
                    }
                    firstPosDif0[di] = updateFirstPosDiff;   
                }
                Console.WriteLine("itr: {0}", i + 1);
                PrintMatrix(agumentedMatrix, true);
                PrintArray(firstPosDif0, s: "First position that is different from 0:");
            }
            for(int i = 0; i < iMax; i++)
            {
                double divCoef = agumentedMatrix[i,firstPosDif0[i]];
                for(int j =  0; j < 2*jMax; j++)
                {
                    agumentedMatrix[i,j] /= divCoef;
                }    
            }

            inversedMatrix = new double[iMax, jMax];
            for (int i = 0; i < iMax; i++)
                for (int j = jMax; j < 2 * jMax; j++)
                    inversedMatrix[i, j-jMax] = agumentedMatrix[i, j];
            //PrintMatrix(matrix);
            return true;
        }

        public static bool InverseNewton(double[,] matrix, out double[,] invertedMatrix, out string er, double eps = 1e-6)
        {

        }
    }
}
