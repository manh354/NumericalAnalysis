﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Define matrix: A[i,j] is element at row i column j

namespace NumericalAnalysis
{
    public partial class Algorithms
    {

        public static void Chapter2Main()
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
                    if (!InOutProcessing.MatrixInput(out matrix, fileLocation))
                    {
                        Console.WriteLine("Invalid Inputs");
                        break;
                    }
                    MatrixDecomposition.PrintMatrix(matrix);
                    Dictionary<int, Dictionary<int, double>> roots;
                    MatrixDecomposition.GaussMain(ref matrix, out roots);
                    InOutProcessing.MatrixRootOutput(roots, matrix.GetLength(1) - 1);
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
                    PrintMatrix(matrix, true);
                    firstPosDif0[di] = updateFirstPosDiff;
                    PrintArray(firstPosDif0);
                }
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
                PrintMatrix(processedMatrix,true);
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
            GaussJordanAddingRoots(ref matrix, firstPosDif0, changedPos, out roots);
            return true;
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
                        if (maxOfMatrix < Math.Abs(matrix[i, j]))
                        {
                            maxOfMatrix = matrix[i, j];
                            row = i;
                            col = j;
                        }
                    }
                }
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

        #region LU decomposition

        public static bool LUmain(ref double[,] matrix)
        {
            int n = matrix.GetLength(0);

            double[,] U, L;
            LUdecomposition(ref matrix, out U, out L);
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
            for (int k = 0; k < n; k++)
            {
                U[k, k] = matrix[k, k];
                L[k, k] = 1;
                for (int i = k + 1; i < n; i++)
                {
                    L[i, k] = matrix[i, k] / U[k, k];
                    U[k, i] = matrix[k, i];
                    U[i, k] = 0;
                    L[k, i] = 0;
                }
                for (int i = k + 1; i < n; i++)
                    for (int j = k + 1; j < n; j++)
                        matrix[i, j] = matrix[i, j] - L[i, k] * U[k, j];
            }
            PrintMatrix(matrix); PrintMatrix(U); PrintMatrix(L);
            return true;
        }

        public static bool MatrixMultiplier(double[,] leftMatrix, double[,] rightMatrix, out double[,] result)
        {
            int lNumRow = leftMatrix.GetLength(0);
            int lNumCol = leftMatrix.GetLength(1);
            int rNumRow = rightMatrix.GetLength(0);
            int rNumCol = rightMatrix.GetLength(1);
            if (lNumCol != rNumRow)
            {
                result = null;
                return false;
            }
            result = new double[lNumRow, rNumCol];
            for (int i = 0; i < lNumRow; i++)
            {
                for (int j = 0; j < rNumCol; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < lNumCol; k++)
                        result[i, j] += leftMatrix[i, k] * rightMatrix[k, j];
                }
            }
            return true;
        }

        #endregion

        #region SimpleIterative

        public void SimpleIterativeMain()
        {
            MatrixNormCheck();
        }

        public void MatrixNormCheck()
        {

        }

        public void SimpleIterator()
        {

        }

        #endregion

        #region JacobiIterative

        //Require B matrix's norm < q < 1 ; x = Bx + b

        public static void JacobiIterativeMain(ref double[,] matrix, double eps = 1e-7)
        {
            double[] seed = new double[matrix.GetLength(0)];
            for (int n = 0; n < seed.Length; n++)
            {
                seed[n] = 0;
            }
            string s;
            if (!JacobiIterativeMethod(ref matrix, seed, out double[] root, eps, out s))
            {
                Console.WriteLine(s);
                return;
            }
            PrintArray(root, true, "root");
        }

        public static bool JacobiIterativeMethod(ref double[,] matrix, double[] seed, out double[] root, double eps, out string s)
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
                        temp -= matrix[i, j] * root[j];
                    }
                    root2[i] = temp / matrix[i, i];
                    //Console.WriteLine("temp2: " + root2[i]);
                    //Console.WriteLine("temp: " + root[i]);
                    //Console.WriteLine("Diff: " + Math.Abs(root2[i] - root[i]));
                }
                itr++;
                PrintArray(root2, true);
                //Console.WriteLine(JacobiIterativeRootDistance(root, root2, eps));
            } while (!JacobiIterativeRootDistance(root, root2, eps));
            ChangeLastColSide(ref matrix);
            s = null;
            return true;
        }

        public static bool JacobiIterativeRootDistance(double[] root1, double[] root2, double eps)
        {
            int size = root1.Length;
            for (int i = 0; i < size; i++)
                if (Math.Abs(root1[i] - root2[i]) > eps)
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

        #region Utilities

        /// <summary>
        /// Sort array to a upper trapezoidial form.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="firstPosDif0">Array of first position that is different from 0 in each row of matrix</param>
        /// <param name="matrix">Input matrix</param>
        public static void UpperTrapezoidSortSwap<T>(int[] firstPosDif0, ref T[,] matrix, out int[] _firstPosDiff0)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

            if (firstPosDif0 == null)
            {
                firstPosDif0 = new int[iMax]; // First position that is different from 0
                for (int i = 0; i < iMax; i++)
                {
                    firstPosDif0[i] = 0;
                    for (int j = 0; j < jMax; j++)
                    {
                        if (!matrix[i, j].Equals(0d))
                        {
                            firstPosDif0[i] = j;
                            break;
                        }
                    }
                }
            }

            //SelectionSort
            for (int i = 0; i < iMax; i++)
            {
                int minIndex = i;
                for (int j = i; j < iMax; j++)
                {
                    if (firstPosDif0[minIndex] > firstPosDif0[j])
                        minIndex = j;
                }
                if (i != minIndex)
                {
                    Swap(ref firstPosDif0[minIndex], ref firstPosDif0[i]);
                    SwapRow(ref matrix, minIndex, i);
                }
            }

            _firstPosDiff0 = firstPosDif0;
        }

        /// <summary>
        /// Swap 2 rows
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix">Input matrix</param>
        /// <param name="row1">First row</param>
        /// <param name="row2">Second row</param>
        public static void SwapRow<T>(ref T[,] matrix, int row1, int row2)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            for (int j = 0; j < jMax; j++)
            {
                Swap(ref matrix[row1, j], ref matrix[row2, j]);
            }
            return;
        }

        /// <summary>
        /// Swap 2 columns
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix">Input matrix</param>
        /// <param name="col1">First Column</param>
        /// <param name="col2">Second Column</param>
        public static void SwapCol<T>(ref T[,] matrix, int col1, int col2)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            for (int i = 0; i < iMax; i++)
            {
                Swap(ref matrix[i, col1], ref matrix[i, col2]);
            }
            return;
        }

        /// <summary>
        /// Swap 2 entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">Entity 1</param>
        /// <param name="b">Entity 2</param>
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        public static void PrintMatrix<T>(T[,] matrix)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            for (int i = 0; i < iMax; i++)
            {
                for (int j = 0; j < jMax; j++)
                    Console.Write(matrix[i, j].ToString() + " ");
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static void PrintMatrix(double[,] matrix, bool flippedLastCol = false)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            for (int i = 0; i < iMax; i++)
            {
                for (int j = 0; j < jMax; j++)
                {
                    if(flippedLastCol&&j==jMax-1)
                    {
                        Console.Write((-matrix[i, j]).ToString() + " ");
                    }    
                    else Console.Write(matrix[i, j].ToString() + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static void PrintArray<T>(T[] array, bool printVertically = false, string s = "array")
        {
            if (!printVertically)
            {
                int len = array.Length;
                Console.Write("{0}: ", s);
                for (int i = 0; i < len; i++)
                    Console.Write(array[i].ToString());
                Console.WriteLine();
                return;
            }
            int len2 = array.Length;
            Console.WriteLine("{0}: ", s);
            for (int i = 0; i < len2; i++)
            {
                Console.WriteLine(array[i].ToString() + " ");
            }
        }

        #endregion
    }
}