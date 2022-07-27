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
                    firstPosDif0[i] = jMax;
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
                        Console.Write("{0}\t",(-matrix[i, j]).ToString());
                    }
                    else Console.Write("{0}\t", matrix[i,j].ToString());
                }
                Console.WriteLine();
            }
            Console.WriteLine("=====================================================");
        }

        public static void PrintArray<T>(T[] array, bool printVertically = false, string s = "array")
        {
            if (!printVertically)
            {
                int len = array.Length;
                Console.Write("{0}: ", s);
                for (int i = 0; i < len; i++)
                    Console.Write(array[i].ToString() + " ");
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


        public static bool Mul2Matrix(double[,] leftMatrix, double[,] rightMatrix, out double[,] result)
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

        public static double[,] Mul2Matrix(double[,] leftMatrix, double[,] rightMatrix)
        {
            int lNumRow = leftMatrix.GetLength(0);
            int lNumCol = leftMatrix.GetLength(1);
            int rNumRow = rightMatrix.GetLength(0);
            int rNumCol = rightMatrix.GetLength(1);
            if (lNumCol != rNumRow)
            {
                Console.WriteLine("A FAILED MULTIPLICATION OF MATRICES WAS DETECTED (lNumCol = {0}, rNumRow = {1})",lNumCol,rNumRow);
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

        public static bool AddMatrixWithnI(ref double[,] matrix, int n)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            if (iMax != jMax)
            {
                Console.WriteLine("Cộng ma trận với n*I không thành công do không phải ma trận vuông");
                return false;
            }
            for (int i = 0; i < iMax; i++)
            {
                matrix[i, i] += n;
            }
            return true;
        }

        public static double[,] AddMatrixWithnI(double[,] matrix, int n)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            double[,] result = new double[iMax, jMax];
            for (int i = 0; i < iMax; i++)
                for (int j = 0; j < jMax; j++)
                    result[i, j] = matrix[i, j];
            if (iMax != jMax)
            {
                Console.WriteLine("Cộng ma trận với n*I không thành công do ma trận không vuông");
                return null;
            }
            for (int i = 0; i < iMax; i++)
            {
                result[i, i] += n;
            }
            return result;
        }

        public static double[,] SubtractMatrixWithnI(double[,] matrix, int n)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            double[,] result = new double[iMax, jMax];
            for (int i = 0; i < iMax; i++)
            {
                for (int j = 0; j < jMax; j++)
                {
                    result[i, j] = matrix[i, j];
                }
            }
            for(int i = 0; i < iMax; i++)
                result[i, i] -= n;
            return result;
        }

        public static double[,] SubtractIwithMatrix(int n, double[,] matrix)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            double[,] result = new double[iMax, jMax];
            for (int i = 0; i < iMax; i++)
            {
                for (int j = 0; j < jMax; j++)
                {
                    result[i, j] = -matrix[i, j];
                }
            }
            for (int i = 0; i < iMax; i++)
                result[i, i] += n;
            return result;
        }

        public static double[,] Subtract2Matrix(double[,] A, double[,] B)
        {
            int iMax = A.GetLength(0);
            int jMax = B.GetLength(1);

            double[,] result = new double[iMax, jMax];

            for (int i = 0; i < iMax; i++)
            {
                for (int j = 0; j < jMax; j++)
                {
                    result[i, j] = A[i, j] - B[i, j];
                }
            }
            return result;
        }

        public static bool MulMatrixWithN(ref double[,] matrix, int n)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            
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
            
            for (int i = 0; i < iMax; i++)
            {
                for (int j = 0; j < jMax; j++)
                    result[i,j] += matrix[i, j] * n;
            }
            return result;
        }

        public static double[,] MulMatrixWithN(double[,] matrix, double n)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            double[,] result = new double[iMax, jMax];
            
            for (int i = 0; i < iMax; i++)
            {
                for (int j = 0; j < jMax; j++)
                    result[i, j] += matrix[i, j] * n;
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

        public static double[] MulVectorWithN(double[] x, double n)
        {
            int len = x.Length;
            double[] result = new double[len];
            for (int i = 0; i < len; i++)
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

        public static double[] Subtract2Vector(double[] l, double[] r)
        {
            int xLen = l.Length;
            int ylen = l.Length;
            if (xLen != ylen)
            {
                Console.WriteLine("SUBTRACT TWO MATRICES FAILED");
                return null; 
            }
            double[] result = new double[xLen];
            for (int i = 0; i < xLen; i++)
                result[i] = l[i] - r[i];
            return result;
        }

        public static void SetSameValue(double[]target,double[]source)
        {
            int xLen = target.Length;
            int ylen = target.Length;
            if (xLen != ylen) return ;
            for (int i = 0; i < xLen; i++)
                target[i] =source[i];
        }
        public static void SetSameValue(double[,] target, double[,] source)
        {
            int iTarget = target.GetLength(0);
            int jTarget = source.GetLength(1);
            int iSource = target.GetLength(0);
            int jSource = source.GetLength(1);
            if (iTarget != iSource) return;
            for (int i = 0; i < iTarget; i++)
                for (int j = 0; j < jTarget; j++)
                    target[i, j] = source[i, j];
        }

        public static bool TransposeMatrix(double[,] matrix, out double[,] transposedMatrix)
        {
            transposedMatrix = null;

            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

            transposedMatrix = new double[jMax,iMax];

            for (int i = 0; i < iMax; i++)
                for (int j = 0; j < jMax; j++)
                    transposedMatrix[j, i] = matrix[i, j];
            return true;
        }

        public static double[,] TransposeMatrix(double[,] matrix)
        { 
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

            var transposedMatrix = new double[jMax, iMax];

            for (int i = 0; i < iMax; i++)
                for (int j = 0; j < jMax; j++)
                    transposedMatrix[j, i] = matrix[i, j];
            return transposedMatrix;
        }


        public static double[,] Add2Matrix(double[,] A, double[,] B)
        {
            double[,] result = new double[A.GetLength(0), A.GetLength(0)];
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for(int j = 0;j<A.GetLength(1);j++)
                {
                    result[i,j] = A[i,j] + B[i,j];
                }    
            }
            return result;
        }

        // Triangular matrix

        public static bool TransposeTriangularMatrix(double[,] matrix, out double[,] transposedMatrix)
        {
            transposedMatrix = null;

            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

            if (iMax != jMax)
                return false;

            transposedMatrix = new double[iMax, jMax];

            for (int i = 0; i < iMax; i++)
                for (int j = 0; j <=i; j++)
                    transposedMatrix[j, i] = matrix[i, j];
            return true;
        }

        /*public static bool LinearCheckResult(double[,] matrix, double[] root)
        {
            SeperatingAb(matrix, out double[,] A, out double[] b);
            int a = b.Length;
            double[] AX = MulMatrixWithVector(A, root);
            for(int i = 0;i<a;i++)
            {
                if (AX[i] != b[i])
                    return false;
            }
            return true;
        }*/

        public static void InverseCheckResult(double[,] matrix, double[,] inversed, out double[,] check)
        {
            int n = matrix.GetLength(0);
            check = Mul2Matrix(matrix, inversed);
            return ;
        }

        public static double[,] CopyFromMatrix(double[,] source,int startRow, int endRow, int startCol,int endCol)
        {
            double[,] result = new double[endRow - startRow + 1, endCol - startCol + 1];
            int di = 0;int dj;
            for (int i = startRow; i <= endRow; i++)
            {
                dj = 0;
                for (int j = startCol; j <= endCol; j++)
                {
                    result[di, dj] = source[i, j];
                    dj++;
                }
                di++;
            }
            return result;
        }

        public static void CopyFromMatrix(double[,] source,ref double[,] target ,int sstartRow, int sendRow, int sstartCol, int sendCol,int tstartRow, int tendRow,int tstartCol,int tendCol)
        {
            if(sstartRow-sendRow != tstartRow-tendRow && sstartCol-sendCol != tstartCol-tendCol)
            {
                Console.WriteLine("Lỗi, không thể set value khập khiễng!");
                return;
            }    
            for(int i = sstartRow; i < sstartRow; i++)
            {
                for (int j = sstartCol; j < sendCol; j++)
                {
                    target[tstartRow, tstartCol] = source[i, j];
                    tstartCol++;
                }
                tstartRow++;
            }    
        }

        public static double[,] VStack(params double[][,]? matrixs)
        {
            int n = matrixs[0].GetLength(1);
            int iMax = 0;
            int jMax = n;
            foreach (double[,] matrix in matrixs)
            {
                iMax += matrix.GetLength(0);
                if (jMax < matrix.GetLength(1))
                    jMax = matrix.GetLength(1);
            }
            double[,] result = new double[iMax, jMax];
            int i = 0;
            foreach (double[,] matrix in matrixs)
            {
                for (int di = 0; di < matrix.GetLength(0); di++)
                {
                    int j = 0;
                    for (int dj = 0; dj < matrix.GetLength(1); dj++)
                    {
                        result[i, j] = matrix[di, dj];
                        j++;
                    }
                    i++;
                }
            }
            return result;
        }

        public static double[,] EyeMatrix(int n)
        {
            double[,] eye = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                eye[i, i] = 1; 
            }
            return eye;
        }

        public static double[,] OneArray(int n)
        {
            double[,] result = new double[1, n];
            for (int i = 0; i < n; i++)
            {
                result[0, i] = 1;
            }
            return result;
        }

        public static double[,] Zeros(int m,int n)
        {
            double[,] result = new double[m, n];
            return result;
        }

        public static double[] Zeros(int m)
        {
            double[] result = new double[m];
            return result;
        }

        public static double[,] Convert1To2(double[] x)
        {
            double[,] xx  = new double[1, x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                xx[0, i] = x[i];
            }
            return xx;
        }

        public static double[,] ExpandVer(double[,] TargetMatrix, double[,] vector)
        {
            double[,] result;
            if (TargetMatrix == null)
            {
                result = new double[vector.GetLength(0),vector.GetLength(1)];
                SetSameValue(result, vector);
                return result; 
            }
            result = new double[TargetMatrix.GetLength(0), TargetMatrix.GetLength(1) + 1];
            for(int i =0; i < TargetMatrix.GetLength(0); i++)
            {
                for(int j = 0;j<TargetMatrix.GetLength(1);j++)
                {
                    result[i, j] = TargetMatrix[i, j];
                }    
            }    
            for(int k = 0; k<TargetMatrix.GetLength(0);k++)
            {
                result[k,TargetMatrix.GetLength(1)] = vector[k,0];
            }
            return result;
        }

        public static double[,] ExpandZero(double[,] TargetMatrix, int row,int col)
        {
            double[,] result = new double[row, col];
            for(int i =0; i< TargetMatrix.GetLength(0);i++)
            {
                for(int j = 0;j <TargetMatrix.GetLength(1);j++)
                {
                    result[i, j] = TargetMatrix[i, j]; 
                }    
            }
            return result;
        }
        #endregion
    }
}
