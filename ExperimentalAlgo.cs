using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalAnalysis
{
    public partial class Algorithms
    {

        static public bool SimpleIterativeMainEx(ref double[,] matrix, double[] seed, double eps = 1e-7)
        {
            if (!MatrixRefactorAndNormCheckEx( matrix, out double q, out int mode))
            { Console.WriteLine("Norm does not satisfy condition."); return false; }
            //PrintMatrix(matrix);
            if (!SimpleIteratorEx(ref matrix, seed, mode, q, out double[] root, eps, out string s))
            {
                Console.WriteLine(s);
                return false;
            }
            return true;
        }

        static public bool MatrixRefactorAndNormCheckEx(double[,] matrix, out double q, out int mode)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

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
            if (!MultiSINormCheckEx(matrix2, out q))
            {
                for (int n = 0; n < iMax; n++)
                {
                    matrix2[n, n] -= 2;
                }
                if (!MultiSINormCheckEx(matrix2, out q))
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

        public static bool MultiSINormCheckEx(double[,] matrix, out double q)
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

        static public bool SimpleIteratorEx(ref double[,] matrix, double[] seed, int mode, double q, out double[] root, double eps, out string s)
        {

            SeperatingAb(matrix, out double[,] A, out double[] b);

            PrintMatrix(A); PrintArray(b);

            root = seed;
            double[] root2 = new double[root.Length];
            SetSameValue(root2, root);
            
            int itr = 0, itrMax = Convert.ToInt32(Math.Sqrt(1 / eps));

            if (mode == 1) goto MODE1;
            if (mode == 2) goto MODE2;

            MODE1:
            double[,] Alpha1 = AddMarixWithnT(A, 1);
            //PrintMatrix(Alpha1);
            do
            {
                if (itr > itrMax)
                {
                    s = "Does not converge";
                    return false;
                }
                SetSameValue(root, root2);
                root2 = Add2Vector(MulMatrixWithVector(Alpha1, root), MulVectorWithN(b, -1));
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
                //PrintArray(MulMatrixWithVector(Alpha2, root));
                //PrintArray(Add2Vector(MulMatrixWithVector(Alpha2, root), b));
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
    }
}
