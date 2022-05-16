using System;
using System.Collections.Generic;


namespace NumericalAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Chapter1: ");
                string i = Console.ReadLine();
                if (i == "0") break;
                
                switch (i)
                {
                    case "1":
                        LowPrecision.BT1();
                        break;
                    case "2":
                        LowPrecision.BT2();
                        break;
                    case "3":
                        LowPrecision.BT3();
                        break;
                    case "4":
                        LowPrecision.BT4();
                        break;
                    case "5":
                        Console.Clear();
                        break;
                    case "hp" or "HP" or "Hp" or "hP":
                        Console.WriteLine("HIGH PRECISION MODE ACTIVATED!!! - THIS WILL USE LOTS OF MEMORY");
                        LongCalc.bf.GlobalPrecision = 100;
                        while (true)
                        {
                            Console.Write("What do you want me to do? (HP): ");
                            string hp = Console.ReadLine();
                            if (hp == "0") break;
                            switch (hp)
                            {
                                case "bs" or "BS" or "Bs" or "bS":
                                    Console.WriteLine("BISECTION MODE");
                                    while (true)
                                    {
                                        Console.Write("What do you want me to do? (HP)(BS): ");
                                        string bs = Console.ReadLine();
                                        if (bs == "0") break;
                                        switch (bs)
                                        {
                                            case "1":
                                                HighPrecision.BT1();
                                                break;
                                            case "2":
                                                HighPrecision.BT2();
                                                break;
                                            case "3":
                                                Console.WriteLine("NOT IMPLEMENTED!");
                                                break;
                                            case "4":
                                                HighPrecision.BT4();
                                                break;
                                        }
                                    }
                                    break;
                                case "fp" or "FP" or "Fp" or "fP":
                                    Console.WriteLine("FALSE POSITION MODE");
                                    while (true)
                                    {
                                        Console.Write("What do you want me to do? (HP)(FP): ");
                                        string bs = Console.ReadLine();
                                        if (bs == "0") break;
                                        switch (bs)
                                        {
                                            case "1":
                                                HighPrecision.FalsePosBT1();
                                                break;
                                            case "2":
                                                Console.WriteLine("NOT IMPLEMENTED!");
                                                break;
                                            case "3":
                                                Console.WriteLine("NOT IMPLEMENTED!");
                                                break;
                                            case "4":
                                                Console.WriteLine("NOT IMPLEMENTED!");
                                                break;
                                        }
                                    }
                                    break;
                                case "nt" or "NT" or "Nt" or "nT":
                                    Console.WriteLine("NEWTON MODE");
                                    while (true)
                                    {
                                        Console.Write("What do you want me to do? (HP)(NT): ");
                                        string nt = Console.ReadLine();
                                        if (nt == "0") break;
                                        switch (nt)
                                        {
                                            case "1":
                                                HighPrecision.NewtonBT1();
                                                break;
                                            case "2":
                                                HighPrecision.NewtonBT2();
                                                break;
                                            default:
                                                nt = "0";
                                                break;
                                        }
                                    }
                                    break;
                                case "pr" or "PR" or "Pr" or "pR":
                                    Console.Write("Input PRECISION (default 100): ");
                                    int pres;
                                    Int32.TryParse(Console.ReadLine(), out pres);
                                    LongCalc.bf.GlobalPrecision = pres;
                                    break;
                                case "5":
                                    Console.Clear();
                                    break;
                                default:
                                    hp = "0";
                                    break;

                            }
                        }
                        break;
                    case "db" or "DB" or "Db" or "dB":
                        Console.WriteLine("DEBUG MODE !!! CHOSE FUNCTIONS TO TEST.");
                        while (true)
                        {
                            string k = Console.ReadLine();
                            if (k == "0") break;
                            switch (k)
                            {
                                case "1":
                                    Console.WriteLine("POLY_VALUE_CALC: ");
                                    float[] val1 = new float[5];
                                    for (int h = 0; h < 5; h++)
                                    {
                                        Console.Write("{0}: ", h);
                                        float.TryParse(Console.ReadLine(), out val1[h]);
                                    }
                                    Console.Write("x: ");
                                    float x; float.TryParse(Console.ReadLine(), out x);
                                    Console.WriteLine("Value: {0} ", PolynomialSolver.PolyValueCalc(val1, x));
                                    break;
                                case "2":
                                    Console.WriteLine("POLY_DERIVATIVE: ");
                                    float[] val2 = new float[5];
                                    for (int h = 0; h < 5; h++)
                                    {
                                        Console.Write("{0}: ", h);
                                        float.TryParse(Console.ReadLine(), out val2[h]);
                                    }
                                    foreach (float a in PolynomialSolver.PolyDerivative(val2))
                                        Console.WriteLine("Value: {0} ", a);
                                    break;

                                default:
                                    k = "0";
                                    break;
                            }
                        }
                        break;
                    default:
                        i = "0";
                        break;
                }
                Console.Write("Chapter2: ");
                i = Console.ReadLine();
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
                        MatrixDecomposition.GaussianElimination(ref matrix, out roots);
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
                        MatrixDecomposition.GaussJordanElimination(ref matrix2, out roots2);
                        InOutProcessing.MatrixRootOutput(roots2, matrix2.GetLength(1) - 1);
                        break;
                    default:
                        break;
                }
            }

        }



    }
}
