using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Define matrix: A[i,j] is element at row i column j

namespace NumericalAnalysis
{
    public class MatrixDecomposition
    {
        #region GaussianElimination

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix"></param>
        public static void GaussChangeLastCol(ref double[,] matrix)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            for(int i = 0; i< iMax;i++)
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
        public static bool GaussianElimination(ref double[,] matrix, out Dictionary<int,Dictionary<int,double>> roots)
        {
            int[] firstPosDif0; 
            ForwardSubtraction(ref matrix, out firstPosDif0);
            BackwardSubtraction(ref matrix, firstPosDif0, out roots);
            return true;
        }

        /// <summary>
        /// Subtract matrix from front to back.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="_firstPosDif0"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static bool ForwardSubtraction(ref double[,] matrix, out int[] _firstPosDif0 , double? eps = 1e-4)
        {
            // Sort 
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);; // First position that is different from 0

            GaussChangeLastCol(ref matrix);

            //Rearrange matrix to a more reversed trapzoid form
            UpperTrapezoidSortSwap(null, ref matrix, out int [] firstPosDif0);
            //Main matrix elimination loop
            for(int i = 0; i < iMax; i++) 
            {
                for (int di = i+1; di < iMax; di++)
                {
                    double mulCoef = matrix[di, firstPosDif0[i]] / matrix[i, firstPosDif0[i]];
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
                    PrintMatrix(matrix);
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
        public static bool BackwardSubtraction(ref double[,] processedMatrix, int[] firstPosDif0, out Dictionary<int, Dictionary<int, double>> roots , double? eps = null)    
        {
            int iMax = processedMatrix.GetLength(0);
            int jMax = processedMatrix.GetLength(1);
            int[] lastPosDif0 = new int[iMax]; //last position that is different from 0
            for (int i = iMax-1; i >=0 ; i--)
            {
                for (int j = jMax-1; j > firstPosDif0[i]; j--) 
                {
                    if (processedMatrix[i, j] == 0)
                    {
                        lastPosDif0[i] = j;
                        break;
                    }
                }
                lastPosDif0[i] = jMax-1;
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
                for(int indexOfVar = jMax-1;indexOfVar>firstPosDif0[i];indexOfVar--)
                {
                    root.Add(indexOfVar, 0);
                    Console.WriteLine("Added x_{0}",indexOfVar);
                }
                for(int j = lastPosDif0[i]; j >= firstPosDif0[i]; j--)
                {
                    processedMatrix[i, j] = processedMatrix[i, j] / processedMatrix[i, firstPosDif0[i]];
                }
                PrintMatrix(processedMatrix);
                for (int j = lastPosDif0[i] ; j > firstPosDif0[i]; j--)
                {
                    if (roots.ContainsKey(j))
                    {
                        foreach (KeyValuePair<int, double> x_j in roots[j]) 
                        {
                            Console.WriteLine("x_j.Key : {0} ",x_j.Key);
                        }
                        foreach (KeyValuePair<int, double> x_j in roots[j])
                        {
                            root[x_j.Key] = root[x_j.Key] -  processedMatrix[i,j] *  x_j.Value ;
                        }
                        root.Remove(j);
                    }
                    else
                    {
                        root[j] -=processedMatrix[i, j];
                    }
                }
                roots.Add(firstPosDif0[i], root);
            }
            return true;


        }

        #endregion

        //In Vietnam , Complete pivoting method of Gauss elimination is called Gauss-Jordan???
        #region GaussJordanElimination

        /// <summary>
        /// Main function.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="roots"></param>
        /// <returns></returns>
        public static bool GaussJordanElimination(ref double[,] matrix, out Dictionary<int,Dictionary<int,double>> roots)
        {
            int[] firstPosDif0;
            PrioritizeSubtraction(ref matrix, out firstPosDif0);
            BackwardSubtraction(ref matrix, firstPosDif0, out roots);
            return true;
        }

        public static bool PrioritizeSubtraction(ref double[,] matrix, out int [] _firstPosDif0, double? eps = null)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

            Dictionary<int, bool> chosenCols = new Dictionary<int, bool>(jMax);
            Dictionary<int, bool> chosenRows = new Dictionary<int, bool>(iMax);

            for(int i =0;i<iMax;i++)
            {
                chosenRows.Add(i, false);
            }
            for(int j = 0;j<jMax;j++)
            {
                chosenCols.Add(j, false);
            }

            for (int k = 0; k < iMax; k++)
            {
                double maxOfMatrix = 0;
                int col = 0, row = 0;
                //Find the pivot point
                for (int i = 0; i < iMax; i++)
                {
                    if (chosenRows[i]) continue;
                    for (int j = 0; j < jMax; j++)
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
                for (int i = 0;i < iMax;i++)
                {
                    if (chosenRows[i]) continue;
                    double muCoef = matrix[i, col] / matrix[row, col];
                    for(int di = 0; di < iMax;di++)
                    {
                        matrix[di, col] = matrix[di, col] - muCoef * matrix[row, col];
                    }
                }
                PrintMatrix(matrix);
            }
            UpperTrapezoidSortSwap(null,ref matrix, out _firstPosDif0);
            PrintMatrix(matrix);
            return true;
        }

        #endregion

        #region LU decomposition



        #endregion

        /// <summary>
        /// Sort array to a upper trapezoidial form.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="firstPosDif0">Array of first position that is different from 0 in each row of matrix</param>
        /// <param name="matrix">Input matrix</param>
        public static void UpperTrapezoidSortSwap<T>( int[] firstPosDif0, ref T[,] matrix, out int[] _firstPosDiff0)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

            if (firstPosDif0 == null)
            {
                firstPosDif0 = new int[iMax]; // First position that is different from 0
                for (int i = 0; i < iMax; i++)
                {
                    for (int j = 0; j < jMax; j++)
                    {
                        if (!matrix[i, j].Equals(0))
                        {
                            firstPosDif0[i] = j;
                            break;
                        }
                    }
                    firstPosDif0[i] = 0;
                }
            }

            //SelectionSort
            for (int i = 0; i< iMax;i++)
            {
                int minIndex = i;
                for (int j = i;j<iMax;j++)
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
            for(int j =0;j<jMax;j++)
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
            for(int i = 0; i< iMax;i++)
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

        public static void PrintMatrix<T>( T[,] matrix)
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
        public static void PrintArray<T>(T[] array)
        {
            int len = array.Length;
            Console.Write("array: ");
            for (int i = 0; i < len; i++)
                Console.Write(array[i].ToString()+ " ");
            Console.WriteLine();
        }
    }
}
