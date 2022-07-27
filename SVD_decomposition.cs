using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalAnalysis
{
    partial class Algorithms
    {
        public static void Chapter6Main()
        {
            string fileLocation = @"SVDinput.txt";
            if (!InOutProcessing.MatrixInput(out double[,] matrix, out double[] _, fileLocation))
            {
                Console.WriteLine("Invalid Inputs");
            }
            Algorithms.PrintMatrix(matrix);
            SVDDecompositionMain(matrix, out var U, out var S, out var Vt);
            Console.Write("U");
            PrintMatrix(U);
            Console.WriteLine("S");
            PrintMatrix(S);
            Console.WriteLine("Vt");
            PrintMatrix(Vt);
            var test = Subtract2Matrix(Mul2Matrix(U, Mul2Matrix(S, Vt)), matrix);
            PrintMatrix(test);
        }

        // Sử dụng Hàm SVD để khai triển ma trận thành tích 3 ma trận U*S*Vt
        /// <summary>
        /// Hàm khai triển ma trận m*n thành tích 3 ma trận U*S*Vt, với giả thiết bắt buộc là m>n
        /// </summary>
        /// <param name="matrix">Ma trận cần khai triển (m*n)</param>
        /// <param name="U">Ma trận kì dị phải (m*m)</param>
        /// <param name="S">Ma trận đường chéo S (m*n)</param>
        /// <param name="Vt">Ma trận kì dị trái (n*n)</param>
        public static void SVDDecompositionMain(double[,] matrix, out double[,] U, out double[,] S, out double[,] Vt)
        {
            // Cho ma trận A là ma trận cỡ m*n
            // Để khai triển A, ta xét 2 loại ma trận AtA (m*m) và AAt (n*n)

            U = S = Vt = null;

            /* FindRankA: đi tìm rank của ma trận A, trả về hạng của ma trận A  */

            /* NullityA : Đi tìm trị số nullity của A, = n - rankA, trả về số lượng tối đa các vecto onto
             * Độc lập tuyến tính trong không gian Rn mà Av = 0 */

            /* Ta chứng minh được : AtA và AAt là 2 ma trận đối xứng, rank A = rank (AtA), giá trị riêng 
             * của 2 ma trận này đều là số thực dương, các giá trị riêng khác 0 của 2 ma trận này đều bằng 
             * nhau */

            /* Ta tìm S như sau: tìm trị riêng của ma trận AtA , sắp xếp theo thứ tự lớn đến bé từ trên
             * xuống căn bậc 2 trị riêng vừa tìm được là ta
             * có S.*/
            double[,] A = matrix;

            int m = A.GetLength(0);
            int n = A.GetLength(1);

            TransposeMatrix(A, out var At);

            // Cần có một gói chương trình đi tìm trị riêng, vecto riêng... mới ra ma trận Vt và U

            // Ứng với mỗi vecto riêng của AtA ứng với một vecto riêng của V => Vt

            // Ứng với mỗi vecto riêng của AAt ứng với một vecto riêng của U => U

            // Cần chéo hoá trực giao Vt và U

            (var eigenValuesV, var _eigenVectorsV) = Danilevski(Mul2Matrix(At, A));

            var eigenVectorsV = new List<double[]>();
            for(int i = 0; i < _eigenVectorsV.Count ; i++)
            {
                var eVectorV = new double[_eigenVectorsV[0].GetLength(0)];
                for(int j = 0; j < eVectorV.Length; j++)
                {
                    eVectorV[j] = _eigenVectorsV[i][j,0];
                }    
                eigenVectorsV.Add(eVectorV);
            }    

            /*(var eigenValuesU, var _eigenVectorsU) = Danilevski(Mul2Matrix(A, At));

            var eigenVectorsU = new List<double[]>();*/

            // Tạo ma trận Sigma
            S = new double[eigenValuesV.Count, eigenValuesV.Count];
            for(int i = 0;i< eigenValuesV.Count;i++)
            {
                S[i, i] = Math.Sqrt(eigenValuesV[eigenValuesV.Count - 1 - i]);
            }

            List<double[]> normVectorsV;

            normVectorsV = GramSchmidt(eigenVectorsV);

            foreach(var v in normVectorsV)
            {
                Vt = ExpandVer(Vt, TransposeMatrix(Convert1To2(v)));
            }
            Vt = TransposeMatrix(Vt);

            Console.WriteLine("Is orthonorm yet? : "+CheckOrthonormalized(normVectorsV));

            for(int i = 0;i<normVectorsV.Count;i++)
            {
                PrintArray(normVectorsV[i]);
            }    

            int rankA = RankMatrix(A);
            Console.WriteLine("Hạng A :{0}",rankA);

            Console.WriteLine(" Ma trận S");
            PrintMatrix(S);

            int it = 0;
            while (it < rankA)
            {
                double[,] ui = MulMatrixWithN( Mul2Matrix(A, TransposeMatrix(Convert1To2(/*chuyển đổi để nhân*/normVectorsV[it]))), 1 / S[it, it]); // Phép 1/Sii * A * Vit
                U = ExpandVer(U, ui);
                PrintMatrix(U);
                it++;
            }
            if(m<n)
            {
                S = ExpandZero(S,A.GetLength(0),n);

            }
            else
            {

            }

        }

        public static List<double[]> GramSchmidt(List<double[]> v)
        {
            int n = v.Count;
            List<double[]> u = new List<double[]>();
            List<double[]> e = new List<double[]>();
            for(int i = 0;i<n;i++)
            {
                u.Add(v[i]);
                for(int j= 0;j<i;j++)
                {
                    u[i] = Subtract2Vector(u[i], Projection(u[j], v[i]));
                }
                /* PrintArray(u[i],s: string.Format("u{0}",i)); */
                e.Add(NormalizeVector(u[i]));
            }
            return e;
        }

        public static double InnerProduct(double[] u, double[] v)
        {
            double sum = 0;
            for (int i = 0; i < u.Length; i++)
            {
                sum += u[i] * v[i];
            }
            return sum;
        }

        public static double[] NormalizeVector(double[] u)
        {
            int n = u.Length;
            double coef = Math.Sqrt(InnerProduct(u, u));
            Console.WriteLine("Coef: "+coef);
            double[] result = new double[n];
            for(int i = 0;i< n;i++)
            {
                result[i] = u[i] / coef;
            }
            return result;
        }

        public static double[] Projection(double[] onto, double[]original)
        {
            double[] result = MulVectorWithN(onto, InnerProduct(onto, original) / InnerProduct(onto, onto));
            return result;
        }

        public static bool SpecialForwardElimination(ref double[,] matrix, out int[] _firstPosDif0, double? eps = 1e-4)
        {
            // Sort 
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1); ; // First position that is different from 0

            ChangeLastColSide(ref matrix);

            //Rearrange A to a more reversed trapzoid form, return an array of first position in matrix that is diff from 0.
            UpperTrapezoidSortSwap(null, ref matrix, out int[] firstPosDif0);
            //Main A elimination loop
            // Xét các hàng i từ trên xuống
            for (int i = 0; i < iMax; i++)
            {
                int firstPosDif0_OfRow_i = firstPosDif0[i];
                if (firstPosDif0_OfRow_i == jMax)
                    continue;
                UpperTrapezoidSortSwap(firstPosDif0, ref matrix, out firstPosDif0);
                // Với mỗi hàng i, ta xét các hàng di (khử tất cả các hệ số ở hàng phía dưới hàng i)
                for (int di = i + 1; di < iMax; di++)
                {
                    // đi tìm hệ số khử của từng hàng
                    double mulCoef = matrix[di, firstPosDif0[i]] / matrix[i, firstPosDif0[i]];
                    //Console.WriteLine("frac: " + A[di, firstPosDif0[j]]+"\\" + A[j, firstPosDif0[j]]);
                    //Console.WriteLine("mul: " + mulCoef);
                    int updateFirstPosDiff = firstPosDif0[di];
                    for (int j = firstPosDif0[di]; j < jMax; j++)
                    {
                        matrix[di, j] -= mulCoef * matrix[i, j];

                        if (Math.Abs(matrix[di, j]) < eps)
                            matrix[di, j] = 0;

                        // There is a case in which column j and j+1 have the same value, that means after subtracting 2 rows, there are 2 zeros next to each other.
                        // This if block ensure firstPosDiff is corrected.

                        if (Math.Abs(matrix[di, j]) == 0 && Math.Abs(matrix[di, updateFirstPosDiff]) == 0 && j == updateFirstPosDiff)
                        {
                            updateFirstPosDiff = j + 1;
                        }
                    }
                    firstPosDif0[di] = updateFirstPosDiff;
                }
            }
            _firstPosDif0 = firstPosDif0;
            //PrintMatrix(A);
            return true;
        }

        public static int RankMatrix(double[,] A)
        {
            int iMax = A.GetLength(0);
            int jMax = A.GetLength(1);
            double[,] B = new double[iMax, jMax];
            SetSameValue(B, A);
            SpecialForwardElimination(ref B, out var _fpd0);
            var Bt = TransposeMatrix(B);
            SpecialForwardElimination(ref Bt, out var _fpd0_2);
            int rank1Matrix = 0;
            for (int i = iMax - 1; i >= 0; i--)
            {
                if (_fpd0[i] < jMax)
                {
                    rank1Matrix = i + 1;
                    break;
                }
            }
            int rank2Matrix = 0;
            for (int i = jMax - 1; i >= 0; i--)
            {
                if (_fpd0_2[i] < iMax)
                {
                    rank2Matrix = i + 1;
                    break;
                }
            }
            return Math.Min(rank1Matrix,rank2Matrix);
        }

        public static bool CheckOrthonormalized(List<double[]> vectors,double eps = 1e-5)
        {
            int n = vectors.Count;
            for (int i = 0; i < n; i++)
            {
                for (int j = i+1; j < n; j++)
                {
                    if (Math.Abs(InnerProduct(vectors[i], vectors[j])) >= eps)
                        return false;
                }
            }
            return true;
        }

    }
}
