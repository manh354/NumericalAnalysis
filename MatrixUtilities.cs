using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalAnalysis
{
    public partial class Algorithms
    {
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

        public static void PrintMatrix(double[,] matrix, bool flippedLastCol = false)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            for (int i = 0; i < iMax; i++)
            {
                for (int j = 0; j < jMax; j++)
                {
                    if (flippedLastCol && j == jMax - 1)
                    {
                        Console.Write((-matrix[i, j]) + " ");
                    }
                    else Console.Write(matrix[i, j] + " ");
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

        public static double[,] MatrixMultiplier(double[,] leftMatrix, double[,] rightMatrix)
        {
            int lNumRow = leftMatrix.GetLength(0);
            int lNumCol = leftMatrix.GetLength(1);
            int rNumRow = rightMatrix.GetLength(0);
            int rNumCol = rightMatrix.GetLength(1);
            if (lNumCol != rNumRow)
            {
                return null;
            }
            double [,] result = new double[lNumRow, rNumCol];
            for (int i = 0; i < lNumRow; i++)
            {
                for (int j = 0; j < rNumCol; j++)
                {
                    for (int k = 0; k < lNumCol; k++)
                        result[i, j] += leftMatrix[i, k] * rightMatrix[k, j];
                }
            }
            return result;
        }

        public static bool MulMatrixWithVector(double[,] A, double[] x, out double[] result)
        {
            int iMax = A.GetLength(0);
            int jMax = A.GetLength(1);
            int num = x.GetLength(0);
            result = new double[num];
            if (jMax != num) return false;
            for (int i = 0; i < iMax; i++)
            {
                for (int j = 0; j < num; j++)
                    result[i] += A[i, j] * x[j];
            }
            return true;
        }

        public static double[] MulMatrixWithVector(double[,] A, double[] x)
        {
            int iMax = A.GetLength(0);
            int jMax = A.GetLength(1);
            int num = x.GetLength(0);
            double[] result = new double[num];
            if (jMax != num) return null;
            for (int i = 0; i < iMax; i++)
            {
                for (int j = 0; j < num; j++)
                    result[i] += A[i, j] * x[j];
            }
            return result;
        }

        public static bool SeperatingAb(double[,] matrix, out double[,] A, out double[] b)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            if (iMax != jMax - 1) { A = null; b = null; Console.WriteLine("Not square matrix"); return false; }
            A = new double[iMax, iMax]; b = new double[iMax];
            for (int i = 0; i < iMax; i++)
            {
                for (int j = 0; j < jMax - 1; j++)
                {
                    A[i, j] = matrix[i, j];
                }
                b[i] = matrix[i, jMax - 1];
            }
            return true;
        }

        public static bool AddMarixWithnT(ref double[,] matrix, int n)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            if (iMax != jMax)
            {
                Console.WriteLine("Not square matrix");
                return false;
            }
            for (int i = 0; i < iMax; i++)
            {
                matrix[i, i] += n;
            }
            return true;
        }

        public static double[,] AddMarixWithnT(double[,] matrix, int n)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            double[,] result = new double[iMax, jMax];
            for (int i = 0; i < iMax; i++)
                for (int j = 0; j < jMax; j++)
                    result[i, j] = matrix[i, j];
            if (iMax != jMax)
            {
                Console.WriteLine("Not square matrix");
                return null;
            }
            for (int i = 0; i < iMax; i++)
            {
                result[i, i] += n;
            }
            return result;
        }

        public static bool MulMatrixWithN(ref double[,] matrix, int n)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            if (iMax != jMax)
            {
                Console.WriteLine("Not square matrix");
                return false;
            }
            for (int i = 0; i < iMax; i++)
            {
                for (int j = 0; j < jMax; j++)
                    matrix[i, j] *= n;
            }
            return true;
        }

        public static double[,] MulMatrixWithN(double[,] matrix, int n)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            double[,] result = new double[iMax, jMax];
            if (iMax != jMax)
            {
                Console.WriteLine("Not square matrix");
                return null;
            }
            for (int i = 0; i < iMax; i++)
            {
                for (int j = 0; j < jMax; j++)
                    result[i,j] += matrix[i, j] * n;
            }
            return result;
        }

        public static double[] MulVectorWithN(double[] x, int n)
        {            
            int len = x.Length;
            double[] result = new double[len];
            for (int i =0;i<len;i++)
            {
                result[i] = x[i] * n;
            }
            return result;
        }

        public static double[] Add2Vector(double[]x, double[] y)
        {
            int xLen = x.Length;
            int ylen = x.Length;
            if (xLen != ylen) return null;
            double[] result = new double[xLen];
            for (int i = 0; i < xLen; i++)
                result[i] = x[i] + y[i];
            return result;
        }

        public static void SetSameValue(double[]x,double[]y)
        {
            int xLen = x.Length;
            int ylen = x.Length;
            if (xLen != ylen) return ;
            for (int i = 0; i < xLen; i++)
                x[i] =y[i];
        }
        public static void SetSameValue(double[,] x, double[,] y)
        {
            int iMax = x.GetLength(0);
            int jMax = y.GetLength(1);
            int iymax = x.GetLength(0);
            int jymax = y.GetLength(1);
            if (iMax != iymax) return;
            for (int i = 0; i < iMax; i++)
                for (int j = 0; j < jMax; j++)
                    x[i, j] = y[i, j];
        }
        #endregion
    }
}
