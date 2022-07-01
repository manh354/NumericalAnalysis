﻿using System;
using System.Collections.Generic;

//Define matrix: A[i,j] is element at row i column j

namespace NumericalAnalysis
{
    public partial class Algorithms
    {

        public static void Chapter3Main()
        {
            Console.Write(Properties._string.TypeInChapter2Switch);
            string i = Console.ReadLine();
            switch (i)
            {
                case "1":
                    Console.WriteLine("Matrix file location: \"Input.txt\", press any key to continue.");
                    Console.ReadLine();
                    Console.WriteLine("Processing matrix");
                    string fileLocation = @"MatrixInput.txt";
                    double[,] matrix;
                    if (!InOutProcessing.MatrixInput(out matrix, out double[] seed1, fileLocation))
                    {
                        Console.WriteLine("Invalid Inputs");
                        break;
                    }
                    Algorithms.PrintMatrix(matrix);
                    Dictionary<int, Dictionary<int, double>> roots;
                    if(Algorithms.GaussMain(ref matrix, out roots))
                        InOutProcessing.MatrixRootOutput(roots, matrix.GetLength(1) - 1);
                    else
                        Console.WriteLine("Failed Gauss");
                    break;
                case "2":
                    Console.WriteLine("Matrix file location: \"Input.txt\", press any key to continue.");
                    Console.ReadLine();
                    Console.WriteLine("Processing matrix");
                    string fileLocation2 = @"MatrixInput.txt";
                    double[,] matrix2;
                    if (!InOutProcessing.MatrixInput(out matrix2, out double[] seed2, fileLocation2))
                    {
                        Console.WriteLine("Invalid Inputs");
                        break;
                    }
                    Algorithms.PrintMatrix(matrix2);
                    Dictionary<int, Dictionary<int, double>> roots2;
                    if(Algorithms.GaussJordanMain(ref matrix2, out roots2))
                        InOutProcessing.MatrixRootOutput(roots2, matrix2.GetLength(1) - 1);
                    else
                    {
                        Console.WriteLine("Failed Gauss Jordan");
                    }
                    break;
                case "3":
                    Console.WriteLine("Matrix file location: \"Input.txt\", press any key to continue.");
                    Console.ReadLine();
                    Console.WriteLine("Processing matrix");
                    string fileLocation3 = @"MatrixInput.txt";
                    double[,] matrix3;
                    if (!InOutProcessing.MatrixInput(out matrix3, out double[] seed3, fileLocation3))
                    {
                        Console.WriteLine("Invalid Inputs");
                        break;
                    }
                    Algorithms.LUmain(ref matrix3);
                    break;
                case "4":
                    Console.WriteLine("Matrix file location: \"Input.txt\", press any key to continue.");
                    Console.ReadLine();
                    Console.WriteLine("Processing matrix");
                    string fileLocation4 = @"MatrixInput.txt";
                    double[,] matrix4;
                    if (!InOutProcessing.MatrixInput(out matrix4, out double[] seed4, fileLocation4))
                    {
                        Console.WriteLine("Invalid Inputs");
                        break;
                    }
                    Algorithms.JacobiIterativeMain(ref matrix4, seed4, 1e-5);
                    break;
                case "5":
                    Console.WriteLine("Matrix file location: \"Input.txt\", press any key to continue.");
                    Console.ReadLine();
                    Console.WriteLine("Processing matrix");
                    string fileLocation5 = @"MatrixInput.txt";
                    double[,] matrix5;
                    if (!InOutProcessing.MatrixInput(out matrix5, out double[] seed5, fileLocation5))
                    {
                        Console.WriteLine("Invalid Inputs");
                        break;
                    }
                    Algorithms.JacobiIterativeMain(ref matrix5, seed5, 1e-5);
                    break;
                case "6":
                    Console.WriteLine("Matrix file location: \"Input.txt\", press any key to continue.");
                    Console.ReadLine();
                    Console.WriteLine("Processing matrix");
                    string fileLocation6 = @"MatrixInput.txt";
                    double[,] matrix6;
                    if (!InOutProcessing.MatrixInput(out matrix6, out double[] seed6, fileLocation6))
                    {
                        Console.WriteLine("Invalid Inputs");
                        break;
                    }
                    Algorithms.SimpleIterativeMain(ref matrix6, seed6, 1e-5);
                    break;
                case "7":
                    Console.WriteLine("Matrix file location: \"Input.txt\", press any key to continue.");
                    Console.ReadLine();
                    Console.WriteLine("Processing matrix");
                    string fileLocation7 = @"MatrixInput.txt";
                    double[,] matrix7;
                    if (!InOutProcessing.MatrixInput(out matrix7, out double[] seed7, fileLocation7))
                    {
                        Console.WriteLine("Invalid Inputs");
                        break;
                    }
                    //
                    break;
                default:
                    break;
            }
        }

        #region GaussianElimination

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix"></param>
        public static void ChangeLastColSide(ref double[,] matrix)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            for (int i = 0; i < iMax; i++)
            {
                matrix[i, jMax - 1] *= -1;
            }
        }

        /// <summary>
        /// Main function.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="roots"></param>
        /// <returns></returns>
        public static bool GaussMain(ref double[,] matrix, out Dictionary<int, Dictionary<int, double>> roots)
        {
            int[] firstPosDif0;
            ForwardElimination(ref matrix, out firstPosDif0);
            BackwardSubstitution(ref matrix, firstPosDif0, out roots);
            return true;
        }

        /// <summary>
        /// Subtract matrix from front to back.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="_firstPosDif0"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static bool ForwardElimination(ref double[,] matrix, out int[] _firstPosDif0, double? eps = 1e-4)
        {
            // Sort 
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1); ; // First position that is different from 0

            ChangeLastColSide(ref matrix);

            //Rearrange matrix to a more reversed trapzoid form
            UpperTrapezoidSortSwap(null, ref matrix, out int[] firstPosDif0);
            //Main matrix elimination loop
            for (int i = 0; i < iMax; i++)
            {
                for (int di = i + 1; di < iMax; di++)
                {
                    double mulCoef = matrix[di, firstPosDif0[i]] / matrix[i, firstPosDif0[i]];
                    //Console.WriteLine("frac: " + matrix[di, firstPosDif0[i]]+"\\" + matrix[i, firstPosDif0[i]]);
                    //Console.WriteLine("mul: " + mulCoef);
                    int updateFirstPosDiff = firstPosDif0[di];
                    for (int j = firstPosDif0[di]; j < jMax; j++)
                    {
                        matrix[di, j] -= mulCoef * matrix[i, j];

                        if (Math.Abs(matrix[di, j]) < eps)
                            matrix[di, j] = 0;

                        // There is a case in which column i and i+1 have the same value, that means after subtracting 2 rows, there are 2 zeros next to each other.
                        // This if block ensure firstPosDiff is corrected.

                        if (Math.Abs(matrix[di, j]) == 0 && Math.Abs(matrix[di, updateFirstPosDiff]) == 0 && j == updateFirstPosDiff)
                        {
                            updateFirstPosDiff = j + 1;
                        }
                    }
                    firstPosDif0[di] = updateFirstPosDiff;
                }
                Console.WriteLine("itr: {0}", i + 1);
                PrintMatrix(matrix, true);
                PrintArray(firstPosDif0,s:"First position that is different from 0:");
            }
            _firstPosDif0 = firstPosDif0;
            //PrintMatrix(matrix);
            return true;
        }

        /// <summary>
        /// Subtract matrix from back to front.
        /// </summary>
        /// <param name="processedMatrix"></param>
        /// <param name="firstPosDif0"></param>
        /// <param name="roots"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static bool BackwardSubstitution(ref double[,] processedMatrix, int[] firstPosDif0, out Dictionary<int, Dictionary<int, double>> roots, double? eps = null)
        {
            int iMax = processedMatrix.GetLength(0);
            int jMax = processedMatrix.GetLength(1);
            int[] lastPosDif0 = new int[iMax]; //last position that is different from 0
            for (int i = iMax - 1; i >= 0; i--)
            {
                for (int j = jMax - 1; j > firstPosDif0[i]; j--)
                {
                    if (processedMatrix[i, j] == 0)
                    {
                        lastPosDif0[i] = j;
                        break;
                    }
                }
                lastPosDif0[i] = jMax - 1;
            }
            roots = new Dictionary<int, Dictionary<int, double>>();
            if (firstPosDif0[firstPosDif0.Length - 1] > iMax - 1)
            {
                roots = null;
                return false;
            }
            for (int i = iMax - 1; i >= 0; i--)
            {
                Dictionary<int, double> root = new Dictionary<int, double>();
                for (int indexOfVar = jMax - 1; indexOfVar > firstPosDif0[i]; indexOfVar--)
                {
                    root.Add(indexOfVar, 0);
                    //Console.WriteLine("Added x_{0}",indexOfVar);
                }
                for (int j = lastPosDif0[i]; j >= firstPosDif0[i]; j--)
                {
                    processedMatrix[i, j] = processedMatrix[i, j] / processedMatrix[i, firstPosDif0[i]];
                }
                for (int j = lastPosDif0[i]; j > firstPosDif0[i]; j--)
                {
                    if (roots.ContainsKey(j))
                    {
                        //foreach (KeyValuePair<int, double> x_j in roots[j]) 
                        //{
                        //    Console.WriteLine("x_j.Key : {0} ",x_j.Key);
                        //}
                        foreach (KeyValuePair<int, double> x_j in roots[j])
                        {
                            root[x_j.Key] = root[x_j.Key] - processedMatrix[i, j] * x_j.Value;
                        }
                        root.Remove(j);
                    }
                    else
                    {
                        root[j] -= processedMatrix[i, j];
                    }
                }
                roots.Add(firstPosDif0[i], root);
            }
            return true;


        }

        #endregion

        #region GaussJordanElimination

        /// <summary>
        /// Main function.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="roots"></param>
        /// <returns></returns>
        public static bool GaussJordanMain(ref double[,] matrix, out Dictionary<int, Dictionary<int, double>> roots)
        {
            int[] firstPosDif0;
            PrioritizedSubtraction(ref matrix, out firstPosDif0, out int[] changedPos);
            if(GaussJordanAddingRoots(ref matrix, firstPosDif0, changedPos, out roots));
                return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="_firstPosDif0"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static bool PrioritizedSubtraction(ref double[,] matrix, out int[] _firstPosDif0, out int[] changedPos, double? eps = 1e-7)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

            ChangeLastColSide(ref matrix);

            bool[] chosenCols = new bool[jMax - 1]; // Minus 1 to not include matrix b in choosing pivot points
            bool[] chosenRows = new bool[iMax];

            for (int i = 0; i < iMax; i++)
            {
                chosenRows[i] = false;
            }
            for (int j = 0; j < jMax - 1; j++) // Minus 1 to not include matrix b in choosing pivot points
            {
                chosenCols[j] = false;
            }

            for (int k = 0; k < iMax; k++)
            {
                double maxOfMatrix = 0;
                int col = 0, row = 0;
                //Find the pivot point
                for (int i = 0; i < iMax; i++)
                {
                    if (chosenRows[i]) continue;
                    for (int j = 0; j < jMax - 1; j++) // Minus 1 to not include matrix b in choosing pivot points
                    {
                        if (chosenCols[j]) continue;
                        if (Math.Abs(matrix[i, j]) == 1)
                        {
                            row = i;
                            col = j;
                            goto NEXT;
                        }
                        if (maxOfMatrix < Math.Abs(matrix[i, j]))
                        {
                            maxOfMatrix = Math.Abs(matrix[i, j]);
                            row = i;
                            col = j;
                        }
                    }
                }
                NEXT:
                chosenCols[col] = true;
                chosenRows[row] = true;
                for (int i = 0; i < iMax; i++)
                {
                    if (i == row) continue;
                    double muCoef = matrix[i, col] / matrix[row, col];
                    //Console.WriteLine("muCoef : {0} ", muCoef);
                    for (int j = 0; j < jMax; j++)
                    {
                        matrix[i, j] = matrix[i, j] - muCoef * matrix[row, j];
                        if (Math.Abs(matrix[i, j]) < eps) matrix[i, j] = 0; // Rounding matrix at position to ensure no errors.
                    }
                }
                PrintMatrix(matrix, true);
            }
            SwapColGaussJordan(chosenCols, ref matrix, out changedPos);
            UpperTrapezoidSortSwap(null, ref matrix, out _firstPosDif0);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processedMatrix"></param>
        /// <param name="firstPosDif0"></param>
        /// <param name="roots"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static bool GaussJordanAddingRoots(ref double[,] processedMatrix, int[] firstPosDif0, int[] changedPos, out Dictionary<int, Dictionary<int, double>> roots, double? eps = null)
        {
            int iMax = processedMatrix.GetLength(0);
            int jMax = processedMatrix.GetLength(1);
            roots = new Dictionary<int, Dictionary<int, double>>();

            if (firstPosDif0[firstPosDif0.Length - 1] > iMax - 1)
            {
                roots = null;
                return false;
            }
            for (int i = iMax - 1; i >= 0; i--)
            {
                Dictionary<int, double> root = new Dictionary<int, double>();
                for (int indexOfVar = jMax - 1; indexOfVar > firstPosDif0[i]; indexOfVar--)
                {
                    root.Add(changedPos[indexOfVar], 0);
                    //Console.WriteLine("Added x_{0}",indexOfVar);
                }
                for (int j = jMax - 1; j >= firstPosDif0[i]; j--)
                {
                    processedMatrix[i, j] = processedMatrix[i, j] / processedMatrix[i, firstPosDif0[i]];
                }
                for (int j = jMax - 1; j > firstPosDif0[i]; j--)
                {
                    root[changedPos[j]] -= processedMatrix[i, j];
                }
                roots.Add(changedPos[firstPosDif0[i]], root);
            }
            return true;
        }

        public static void SwapColGaussJordan(bool[] chosenCols, ref double[,] matrix, out int[] changedPos)
        {
            int jMax = matrix.GetLength(1);
            changedPos = new int[jMax];
            for (int n = 0; n < jMax; n++)
            {
                changedPos[n] = n;
            }
            for (int j = 0; j < jMax - 2; j++)
                if (!chosenCols[j])
                    for (int dj = jMax - 2; dj >= 0; dj--)
                        if (chosenCols[dj])
                        {
                            SwapCol(ref matrix, j, dj);
                            Swap(ref changedPos[j], ref changedPos[dj]);
                            break;
                        }
        }

        #endregion

        #region LU decomposition and solver

        public static bool LUmain(ref double[,] matrix)
        {
            int n = matrix.GetLength(0);

            SeperatingAb(matrix, out double[,] A, out double[] b);

            double[,] U, L;
            LUdecomposition(ref matrix, out U, out L);
            LUSolveForY(L, b, out double[] y);
            LUSolveForX(U, y, out double[] x);
            PrintArray(x);
            return false;
        }


        public static bool LUdecomposition(ref double[,] matrix, out double[,] U, out double[,] L)
        {
            int n = matrix.GetLength(0);
            U = new double[n, n];
            L = new double[n, n];
            if (matrix.GetLength(1) != n + 1)
            {
                return false;
            }
            for (int i = 0; i < n; i++)
            {
                // Upper Triangular
                for (int k = i; k < n; k++)
                {
                    // Summation of L(i, j) * U(j, k)
                    double sum = 0;
                    for (int j = 0; j < i; j++)
                        sum += (L[i, j] * U[j, k]);

                    // Evaluating U(i, k)
                    U[i, k] = matrix[i, k] - sum;
                }

                // Lower Triangular
                for (int k = i; k < n; k++)
                {
                    if (i == k)
                        L[i, i] = 1; // Diagonal as 1
                    else
                    {
                        // Summation of L(k, j) * U(j, i)
                        double sum = 0;
                        for (int j = 0; j < i; j++)
                            sum += (L[k, j] * U[j, i]);

                        // Evaluating L(k, i)
                        L[k, i]
                            = (matrix[k, i] - sum) / U[i, i];
                    }
                }
            }
            PrintMatrix(matrix, false); PrintMatrix(U, false); PrintMatrix(L, false);
            return true;
        }

        // Ax = b  =>  LUx = b => Ly = b  => Ux = y
        //  1                   u   u   u   u
        //  l   1                   u   u   u   
        //  l   l   1                   u   u
        //  l   l   l   1                   u

        public static void LUSolveForY(double[,] L, double[] b, out double[] y)
        {
            int iMax = L.GetLength(0);
            int jMax = L.GetLength(1);

            y = new double[iMax];

            for (int i = 0; i < iMax; i++)
            {
                double yTemp = b[i];
                for (int j = 0; j < i; j++)
                {
                    yTemp -= L[i, j] * y[j];
                }
                y[i] = yTemp;
            }
        }

        public static void LUSolveForX(double[,] U, double[] y, out double[] x)
        {
            int iMax = U.GetLength(0);
            int jMax = U.GetLength(1);

            x = new double[iMax];
            for (int i = iMax - 1; i > 0; i--)
            {
                double xTemp = y[i];
                for (int j = iMax-1; j > i; j--)
                {
                    xTemp -= U[i, j] * x[j];
                }
                x[i] = xTemp;
            }
        }

        #endregion

        #region Choleski

        /*public static bool CholeskiDecomposition(double[,] matrix, double eps = 1e-7, out double[,] lower)
        {
            lower = null;
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            if (!SeperatingAb(matrix, out double[,] A, out double[] b))
                return false;
            if (!IsSquareMartrixHermitian(A))
            {
                Console.WriteLine("Matrix is not Hermitian");
                return false;
            }
            lower = new double[iMax, iMax];
            for (int i = 0; i < iMax; i++)
                for (int j = i; j < iMax; j++)
                {
                    double Sum = 0;
                    for (int k = 0; k < j; k++)
                    {
                        Sum += lower[j, k] * lower[j, k];
                    }
                    if (i == j)
                        lower[j, i] = Math.Sqrt(matrix[i, i] - Sum);
                    else
                        lower[j, i] = (matrix[j, i] - Sum) / lower[j, i];
                }
                        


        }*/

        public static bool IsSquareMartrixHermitian(double[,] matrix)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            for (int i = 0; i < iMax; i++)
                for (int j = i; j < jMax; j++)
                    if (matrix[i, j] != matrix[iMax - i - 1, jMax - j - 1])
                        return false;
            return true;
        }

        #endregion

        #region SimpleIterative

        static public bool SimpleIterativeMain(ref double[,] matrix, double[] seed, double eps = 1e-7)
        {
            if (!MatrixRefactorAndNormCheck(matrix, out double q, out int mode))
            { Console.WriteLine("Norm does not satisfy condition."); return false; }
            //PrintMatrix(matrix);
            if (!SimpleIterator(ref matrix, seed, mode, q, out double[] root, eps, out string s))
            {
                Console.WriteLine(s);
                return false;
            }
            return true;
        }

        static public bool MatrixRefactorAndNormCheck(double[,] matrix, out double q, out int mode)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

            for (int n = 0; n < iMax; n++)
            {
                if(matrix[n,n]<0)
                {
                    for (int m = 0; m < jMax; m++)
                        matrix[n, m] *= -1;
                }    
            }

            double[,] matrix2 = new double[iMax, jMax];
            SetSameValue(matrix2, matrix);

            if (iMax != jMax - 1)
            {
                Console.WriteLine("Not a square matrix");
                q = -1;
                mode = -1;
                return false;
            }
            for (int n = 0; n < iMax; n++)
            {
                matrix2[n, n] += 1;
            }
            if (!MultiSINormCheck(matrix2, out q))
            {
                for (int n = 0; n < iMax; n++)
                {
                    matrix2[n, n] -= 2;
                }
                if (!MultiSINormCheck(matrix2, out q))
                {
                    q = -1;
                    mode = -1;
                    return false;
                }
                mode = 2;
                return true;
            }
            mode = 1;
            return true;
        }

        public static bool MultiSINormCheck(double[,] matrix, out double q)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

            if (iMax != jMax - 1)
            {
                Console.WriteLine("Not a square matrix");
                q = -1;
                return false;
            }
            q = -1;

            // Kiểm tra chuẩn vô cùng 
            MODE1:
            for (int i = 0; i < iMax; i++)
            {
                double sum = 0;
                for (int j = 0; j < jMax - 1; j++)
                {
                    sum += Math.Abs(matrix[i, j]);
                }
                if (sum >= 1)
                {
                    q = -1;
                    goto MODE2;
                }
                if (sum > q)
                    q = sum;
            }

            // Kiểm tra chuẩn 1
            MODE2:
            for (int j = 0; j < jMax - 1; j++)
            {
                double sum = 0;
                for (int i = 0; i < iMax; i++)
                {
                    sum += Math.Abs(matrix[i, j]);
                }
                if (sum >= 1)
                {
                    q = -1;
                    return false;
                }
                if (sum > q)
                    q = sum;
            }
            return true;
        }

        static public bool SimpleIterator(ref double[,] matrix, double[] seed, int mode, double q, out double[] root, double eps, out string s)
        {

            SeperatingAb(matrix, out double[,] A, out double[] b);

            PrintMatrix(A); PrintArray(b);

            root = seed;
            double[] root2 = new double[root.Length];
            SetSameValue(root2, root);

            int itr = 0, itrMax = Convert.ToInt32(Math.Sqrt(1 / eps));

            double eps0 = eps * (1 - q) / q;

            if (mode == 1) goto MODE1;
            if (mode == 2) goto MODE2;

            MODE1:
            double[,] Alpha1 = AddMarixWithnT(A, 1);
            double[] Beta1 = MulVectorWithN(b, -1);
            //PrintMatrix(Alpha1);
            do
            {
                if (itr > itrMax)
                {
                    s = "Does not converge";
                    return false;
                }
                SetSameValue(root, root2);
                root2 = Add2Vector(MulMatrixWithVector(Alpha1, root), Beta1);
                itr++;
                Console.WriteLine("itr:{0}", itr);
                //PrintArray(root, true, "root");
                PrintArray(root2, true, "root");
                //Console.WriteLine(JacobiIterativeRootDistance(root, root2, eps));
            } while (!JacobiIterativeRootDistance(root, root2, eps));
            goto END;


            MODE2:
            double[,] Alpha2 = AddMarixWithnT(MulMatrixWithN(A, -1), 1);
            //PrintMatrix(Alpha2);
            do
            {
                if (itr > itrMax)
                {
                    s = "Does not converge";
                    return false;
                }
                SetSameValue(root, root2);
                root2 = Add2Vector(MulMatrixWithVector(Alpha2, root), b);
                itr++;
                Console.WriteLine("itr:{0}", itr);
                //PrintArray(root, true, "root");
                PrintArray(root2, true, "root");
                //Console.WriteLine(JacobiIterativeRootDistance(root, root2, eps));
            } while (!JacobiIterativeRootDistance(root, root2, eps));
            goto END;
            END:
            s = null;
            return true;
        }


        #endregion

        #region JacobiIterative

        //Require B matrix's norm < q < 1 ; x = Bx + b

        public static void JacobiIterativeMain(ref double[,] matrix, double[] seed, double eps = 1e-7)
        {
            double q;
            int n = JacobiMatrixNormCheck(matrix, out q);
            Console.WriteLine("n:{0}", n);
            if (n == 0)
            {
                Console.WriteLine("Norm check failed");
                return;
            }

            double[] root = null;
            if (n == 1)
            {
                JacobiNormalizeByRow(ref matrix);
                // HORIBLE CODING PRACTICE
                JacobiMatrixNormCheck(matrix, out q);
                string s;
                if (!JacobiIterativeRow(ref matrix, seed, out root, q, eps, out s))
                {
                    Console.WriteLine(s);
                    return;
                }
            }
            if (n == 2)
            {
                JacobiNormalizeByCol(ref matrix, out double[] diag);
                string s;
                // HORIBLE CODING PRACTICE
                JacobiMatrixNormCheck(matrix, out q);
                if (!JacobiIterativeCol(ref matrix, seed, diag, out root, q, eps, out s))
                {
                    Console.WriteLine(s);
                    return;
                }
            }
            PrintArray(root, true, "root");
        }

        public static void JacobiNormalizeByRow(ref double[,] matrix)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

            for (int i = 0; i < iMax; i++)
            {
                double divCoef = matrix[i, i];
                for (int j = 0; j < jMax; j++)
                {
                    matrix[i, j] /= divCoef;
                }
            }

        }

        public static void JacobiNormalizeByCol(ref double[,] matrix, out double[] diag)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

            diag = new double[iMax];

            for (int j = 0; j < jMax - 1; j++)
            {
                diag[j] = matrix[j, j];
                for (int i = 0; i < iMax; i++)
                {
                    matrix[i, j] /= diag[j];
                }
            }
        }

        public static int JacobiMatrixNormCheck(double[,] matrix, out double q)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

            double summax = 0;


            // Kiểm tra Ma trận chéo chội hàng // Check if is diagonally dominant by row
            for (int i = 0; i < iMax; i++)
            {
                double sum = 0;
                for (int j = 0; j < jMax - 1; j++)
                {
                    sum += Math.Abs(matrix[i, j]);
                }
                sum -= Math.Abs(matrix[i, i]);
                if (sum >= Math.Abs(matrix[i, i]))
                {
                    goto MODE2;
                }
                if (summax < sum)
                {
                    summax = sum;
                }
            }
            q = summax;
            return 1;

            // Kiểm tra Ma trận chéo chội cột // Check if is diagonally dominant by column
            MODE2:

            summax = 0;

            for (int j = 0; j < jMax - 1; j++)
            {
                double sum = 0;
                for (int i = 0; i < iMax; i++)
                {
                    sum += Math.Abs(matrix[i, j]);
                }
                sum -= Math.Abs(matrix[j, j]);
                if (sum > Math.Abs(matrix[j, j]))
                {
                    goto MODE0;
                }
                if (sum > summax)
                {
                    summax = sum;
                }
            }
            q = summax;
            return 2;


            // Không là ma trận chéo chội // Is not a diagonally dominant matrix
            MODE0:
            q = -1;
            return 0;

        }

        // Ma trận chéo chội hàng
        public static bool JacobiIterativeRow(ref double[,] matrix, double[] seed, out double[] root, double q, double eps, out string s)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

            SeperatingAb(matrix, out double[,] A, out double[] b);

            root = seed;
            double[] root2 = new double[iMax];
            SetSameValue(root2, root);

            double eps0 = eps * ((1 - q) / q);

            Console.WriteLine("eps0: {0}", eps0);

            int itr = 0, itrMax = Convert.ToInt32(Math.Sqrt(1 / eps));

            double[,] Alpha = AddMarixWithnT(MulMatrixWithN(A, -1), 1); // alpha = I-A

            do
            {
                SetSameValue(root, root2); // Means root = root2
                if (itr > itrMax)
                {
                    s = "Does not converge";
                    return false;
                }
                root2 = Add2Vector(MulMatrixWithVector(Alpha, root), b); // x = alpha*x+b
                itr++;
                Console.WriteLine("itr: {0}", itr);
                PrintArray(root2, true, "root");
                //Console.WriteLine(JacobiIterativeRootDistance(root, root2, eps));
            } while (!JacobiIterativeRootDistance(root, root2, eps0));
            s = null;
            return true;
        }

        // Ma trận chéo chội cột
        public static bool JacobiIterativeCol(ref double[,] matrix, double[] seed, double[] diag, out double[] root, double q, double eps, out string s)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

            SeperatingAb(matrix, out double[,] A, out double[] b);

            root = seed;
            double[] root2 = new double[iMax];
            SetSameValue(root2, root);

            double minDiag = double.MaxValue;
            for (int n = 0; n < iMax; n++)
            {
                double absDiag = Math.Abs(diag[n]);
                if (absDiag < minDiag)
                {
                    minDiag = absDiag;
                }
            }
            double eps0 = eps * minDiag * ((1 - q) / q);

            int itr = 0, itrMax = Convert.ToInt32(Math.Sqrt(1 / eps));

            double[,] Alpha = AddMarixWithnT(MulMatrixWithN(A, -1), 1); // alpha = I-A
            do
            {
                SetSameValue(root, root2);
                if (itr > itrMax)
                {
                    s = "Does not converge";
                    return false;
                }
                root2 = Add2Vector(MulMatrixWithVector(Alpha, root), b); // x = alpha*x+b
                itr++;
                PrintArray(root2, true, "root");
                itr++;
                PrintArray(root2, true, "x* root");
                //Console.WriteLine(JacobiIterativeRootDistance(root, root2, eps));
            } while (!JacobiIterativeRootDistance(root, root2, eps0));

            for (int k = 0; k < iMax; k++)
            {
                root[k] /= diag[k];
            }

            ChangeLastColSide(ref matrix);
            s = null;
            return true;
        }
        public static bool JacobiIterativeRootDistance(double[] root1, double[] root2, double eps0)
        {
            int size = root1.Length;
            double sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += Math.Abs(root1[i] - root2[i]);
                
            }
            if (sum > eps0)
                return false;
            return true;
        }

        #endregion

        #region Gauss-Seidel

        public static void GaussSeidelIterativeMain(ref double[,] matrix, double eps = 1e-7)
        {
            double[] seed = new double[matrix.GetLength(0)];
            for (int n = 0; n < seed.Length; n++)
            {
                seed[n] = 0;
            }
            string s;
            if (!GaussSeidelIterativeMethod(ref matrix, seed, out double[] root, eps, out s))
            {
                Console.WriteLine(s);
                return;
            }
            PrintArray(root, true, "root");
        }

        public static bool GaussSeidelIterativeMethod(ref double[,] matrix, double[] seed, out double[] root, double eps, out string s)
        {
            ChangeLastColSide(ref matrix);

            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            if (iMax != jMax - 1)
            {
                s = "Not a square matrix";
                root = null;
                return false;
            }
            root = new double[iMax];
            double[] root2 = new double[iMax];
            for (int k = 0; k < iMax; k++)
            {
                root2[k] = root[k];
            }
            int itr = 0, itrMax = Convert.ToInt32(Math.Sqrt(1 / eps));
            do
            {
                for (int k = 0; k < iMax; k++)
                {
                    root[k] = root2[k];
                }
                if (itr > itrMax)
                {
                    s = "Does not converge";
                    return false;
                }
                for (int i = 0; i < iMax; i++)
                {
                    double temp = 0;
                    for (int j = 0; j < jMax; j++)
                    {
                        if (i == j) continue;
                        if (j == jMax - 1)
                        {
                            temp -= matrix[i, j];
                            continue;
                        }
                        temp -= matrix[i, j] * root2[j];
                    }
                    root2[i] = temp / matrix[i, i];
                    //Console.WriteLine("temp2: " + root2[i]);
                    //Console.WriteLine("temp: " + root[i]);
                    //Console.WriteLine("Diff: " + Math.Abs(root2[i] - root[i]));
                }
                itr++;
                PrintArray(root2, true);
                //Console.WriteLine(JacobiIterativeRootDistance(root, root2, eps));
            } while (!GaussSeidelIterativeRootDistance(root, root2, eps));
            ChangeLastColSide(ref matrix);
            s = null;
            return true;
        }

        public static bool GaussSeidelIterativeRootDistance(double[] root1, double[] root2, double eps)
        {
            int size = root1.Length;
            for (int i = 0; i < size; i++)
                if (Math.Abs(root1[i] - root2[i]) > eps)
                    return false;
            return true;
        }

        #endregion

    }
}