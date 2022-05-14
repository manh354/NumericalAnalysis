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
        public static bool GaussianElimination(ref double[,] matrix, out Dictionary<int,Dictionary<int,double>> roots)
        {
            int[] firstPosDif0; 
            ForwardSubtraction(ref matrix, out firstPosDif0);
            BackwardSubtraction(ref matrix, firstPosDif0, out roots);
            return true;
        }
        public static bool ForwardSubtraction(ref double[,] matrix, out int[] _firstPosDif0 , double? eps = null)
        {
            // Sort 
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            int[] firstPosDif0 = new int[jMax]; // First position that is different from 0
            for (int i = 0; i < iMax; i++)
            {
                for (int j = 0; j < jMax; j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        firstPosDif0[i] = j;
                        break;
                    }
                }
                firstPosDif0[i] = 0;
            }
            //Rearrange matrix to a more reversed trapzoid form
            SortSwap(ref firstPosDif0, ref matrix);
            //Main matrix elimination loop
            for(int k = 0; k < iMax; k++) 
            {
                for(int dk = k+1; dk < iMax;dk++) 
                {

                    if(firstPosDif0[k] == firstPosDif0[dk])
                    {
                        double mulCoef = matrix[dk, firstPosDif0[dk]] / matrix[k, firstPosDif0[k]];
                        Console.WriteLine(mulCoef);
                        int updateFirstPosDiff = firstPosDif0[k];
                        for(int l = firstPosDif0[dk]; l< jMax;l++)
                        {
                            matrix[dk, l]  -= mulCoef * matrix[k, l];

                            // There is a case in which column i and i+1 have the same value, that means after subtracting 2 rows, there are 2 zeros next to each other.
                            // This if block ensure firstPosDiff is corrected.

                            if(matrix[dk,l] == 0 && matrix[dk,updateFirstPosDiff]==0 && l == updateFirstPosDiff )
                            {
                                updateFirstPosDiff = l+1;
                            }    
                        }
                        PrintMatrix(matrix);
                        firstPosDif0[dk] = updateFirstPosDiff;
                    }    
                }    
            }
            _firstPosDif0 = firstPosDif0;
            //PrintMatrix(matrix);
            return true;
        }

        /// <summary>
        /// Subtract matrix from backward.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="firstPosDif0"></param>
        /// <param name="roots"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static bool BackwardSubtraction(ref double[,] matrix, int[] firstPosDif0, out Dictionary<int, Dictionary<int, double>> roots , double? eps = null)    
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            int[] lastPosDif0 = new int[jMax]; //last position that is different from 0
            for (int i = iMax-1; i >=0 ; i--)
            {
                for (int j = jMax-1; j > firstPosDif0[i]; j--) 
                {
                    if (matrix[i, j] == 0)
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
                root.Add(jMax - 1, matrix[i, jMax-1]/ matrix[i,firstPosDif0[i]]);
                for (int j = lastPosDif0[i] -1 ; j > firstPosDif0[i]; j--)
                {
                    if (j == jMax - 1) break;
                    matrix[i, j] /= matrix[i, firstPosDif0[i]];
                    if (roots.ContainsKey(j))
                    {
                        foreach (KeyValuePair<int, double> x_j in roots[j])
                        {
                            root[x_j.Key] -= - matrix[i,j] *  x_j.Value - matrix[i,x_j.Key];
                        }
                    }
                    else
                    {
                        root.Add(j, -matrix[i, j]);
                    }
                }
                roots.Add(firstPosDif0[i], root);
            }
            return true;


        }

        /// <summary>
        /// Processing Roots from a backward elimination matrix, Actually this can be grouped with backward elimination.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="firstPosDif0"></param>
        /// <param name="lastPosDif0"></param>
        /// <param name="roots"></param>
        /// <returns></returns>
        public static bool RootProcessing(double[,] matrix, int[] firstPosDif0, int[] lastPosDif0, out Dictionary<int, Dictionary<int, double>> roots)
        {
            roots = new Dictionary<int, Dictionary<int, double>>();
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);
            if (firstPosDif0[firstPosDif0.Length - 1] > iMax - 1)
            {
                roots = null;
                return false;
            }    
            for (int i= iMax-1; i>=0;i--)
            {
                Dictionary<int, double> root = new Dictionary<int, double>();
                root.Add(jMax, matrix[jMax, i]);
                for (int j = firstPosDif0[i]+1; j<=lastPosDif0[i];j++)
                {
                    if (roots.ContainsKey(j))
                    {
                        double rootSimplifing;
                        foreach (KeyValuePair<int, double> x_i in roots[i])
                        {
                            rootSimplifing = -matrix[x_i.Key, i] + x_i.Value;
                            root.Add(j, rootSimplifing);
                        }
                    }
                    else
                    {
                        root.Add(j, -matrix[j, i]);
                    }
                }
                roots.Add(firstPosDif0[i],root);
            }
            return true;
        }
        /// <summary>
        /// Sort array to a reversed trapzoid form
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="firstPosDiff0">Array of first position that is different from 0 in each row of matrix</param>
        /// <param name="matrix">Input matrix</param>
        public static void SortSwap<T>(ref int[] firstPosDiff0, ref T[,] matrix)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

            //SelectionSort
            for (int i = 0; i< iMax;i++)
            {
                int minIndex = i;
                for (int j = i;j<iMax;j++)
                {
                    if (firstPosDiff0[minIndex] > firstPosDiff0[j]) 
                        minIndex = j;    
                }
                if (i != minIndex)
                {
                    Swap(ref firstPosDiff0[minIndex], ref firstPosDiff0[i]);
                    SwapRow(ref matrix, minIndex, i);
                }
            }    
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
                    Console.Write(matrix[i, j].ToString()+ " ");
                Console.WriteLine();
            }
        }
    }
}
