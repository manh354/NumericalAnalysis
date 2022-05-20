using System;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;


namespace NumericalAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.Unicode;
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            while (true)
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("vi-VN");
                Algorithms.Chapter1Main();
                Console.Write("Chapter2: ");
                string i = Console.ReadLine();
                switch (i)
                {
                    case "1":
                        Console.WriteLine("Matrix file location: \"Input.txt\", press any key to continue.");
                        Console.ReadLine();
                        Console.WriteLine("Processing matrix");
                        string fileLocation = @"MatrixInput.txt";
                        double[,] matrix;
                        if (!InOutProcessing.MatrixInput(out matrix, fileLocation))
                        {
                            Console.WriteLine("Invalid Inputs");
                            break;
                        }
                        MatrixDecomposition.PrintMatrix(matrix);
                        Dictionary<int, Dictionary<int, double>> roots;
                        MatrixDecomposition.GaussMain(ref matrix, out roots);
                        InOutProcessing.MatrixRootOutput(roots,matrix.GetLength(1)-1);
                        break;
                    case "2":
                        Console.WriteLine("Matrix file location: \"Input.txt\", press any key to continue.");
                        Console.ReadLine();
                        Console.WriteLine("Processing matrix");
                        string fileLocation2 = @"MatrixInput.txt";
                        double[,] matrix2;
                        if (!InOutProcessing.MatrixInput(out matrix2, fileLocation2))
                        {
                            Console.WriteLine("Invalid Inputs");
                            break;
                        }
                        MatrixDecomposition.PrintMatrix(matrix2);
                        Dictionary<int, Dictionary<int, double>> roots2;
                        MatrixDecomposition.GaussJordanMain(ref matrix2, out roots2);
                        InOutProcessing.MatrixRootOutput(roots2, matrix2.GetLength(1) - 1);
                        break;
                    case "3":
                        Console.WriteLine("Matrix file location: \"Input.txt\", press any key to continue.");
                        Console.ReadLine();
                        Console.WriteLine("Processing matrix");
                        string fileLocation3 = @"MatrixInput.txt";
                        double[,] matrix3;
                        if (!InOutProcessing.MatrixInput(out matrix3, fileLocation3))
                        {
                            Console.WriteLine("Invalid Inputs");
                            break;
                        }
                        MatrixDecomposition.LUmain(ref matrix3);
                        break;
                    case "4":
                        Console.WriteLine("Matrix file location: \"Input.txt\", press any key to continue.");
                        Console.ReadLine();
                        Console.WriteLine("Processing matrix");
                        string fileLocation4 = @"MatrixInput.txt";
                        double[,] matrix4;
                        if (!InOutProcessing.MatrixInput(out matrix4, fileLocation4))
                        {
                            Console.WriteLine("Invalid Inputs");
                            break;
                        }
                        MatrixDecomposition.JacobiIterativeMain(ref matrix4, 0.000000000001);
                        break;
                    case "5":
                        Console.WriteLine("Matrix file location: \"Input.txt\", press any key to continue.");
                        Console.ReadLine();
                        Console.WriteLine("Processing matrix");
                        string fileLocation5 = @"MatrixInput.txt";
                        double[,] matrix5;
                        if (!InOutProcessing.MatrixInput(out matrix5, fileLocation5))
                        {
                            Console.WriteLine("Invalid Inputs");
                            break;
                        }
                        MatrixDecomposition.JacobiIterativeMain(ref matrix5, 0.000000000001);
                        break;
                    default:
                        break;
                }
            }

        }



    }
}
