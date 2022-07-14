using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalAnalysis
{
    public partial class Algorithms
    {
        public static void Chapter4Main()
        {
            Console.Write("Nhập 1 để chọn nghịch đảo bằng PP Gauss \nNhập 2 để chọn nghịch đảo bằng PP Cholesky \nNhập 3 để chọn nghịch đảo bằng PP Newton \nNhập 4 để chọn nghịch đảo bằng PP Viền quanh");
            string i = Console.ReadLine();
            string fileLocation = @"MatrixInputInverse.txt";
            Console.WriteLine("Matrix file location: \"MatrixInputInverse.txt\", press any key to continue.");
            Console.ReadLine();
            Console.WriteLine("Processing matrix");
            fileLocation = @"MatrixInputInverse.txt";
            double[,] matrix;
            switch (i)
            {
                case "1":
                    if (!InOutProcessing.MatrixInput(out matrix, out double[] _, fileLocation))
                    {
                        Console.WriteLine("Invalid Inputs");
                        break;
                    }
                    Algorithms.PrintMatrix(matrix);
                    Algorithms.InverseGaussJordanMain(matrix);
                    break;

                case "2":
                    if (!InOutProcessing.MatrixInput(out matrix, out double[] _, fileLocation))
                    {
                        Console.WriteLine("Invalid Inputs");
                        break;
                    }
                    Algorithms.PrintMatrix(matrix);
                    Algorithms.InverseGaussJordanMain(matrix);
                    break;
                case "4":
                    if (!InOutProcessing.MatrixInput(out matrix, out double[] _, fileLocation))
                    {
                        Console.WriteLine("Invalid Inputs");
                        break;
                    }
                    Algorithms.PrintMatrix(matrix);
                    Algorithms.InverseBoundaryMain(matrix);
                    break;
            }
        }

        //////////////////////////////
        //////////////////////////////
        //  INVERSE GAUSS JORDAN    //
        //////////////////////////////
        //..........................//
        public static bool InverseGaussJordanMain(double[,] matrix, double eps = 1e-6)
        {
            if (!InverseGaussJordan(matrix, out var inversedMatrix, out var er,eps))
            {
                Console.WriteLine(er);
                return false;
            }
            PrintMatrix(inversedMatrix);
            return true;
        }
        public static bool InverseGaussJordan(double[,] matrix, out double[,] inversedMatrix, out string er, double eps = 1e-6)
        {
            er = null;
            // Sort 
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1); ; // First position that is different from 0
            
            if(iMax !=jMax)
            {
                inversedMatrix = null;
                er = "Not a square matrix";
                return false;
            }

            double[,] agumentedMatrix = new double[iMax, 2 * jMax];

            //Generating an agumented Matrix
            for (int i = 0; i < iMax; i++)
            {
                for (int j = 0; j < jMax; j++)
                {
                    agumentedMatrix[i, j] = matrix[i, j];
                }
                agumentedMatrix[i, i + jMax] = 1;
            }

            Console.WriteLine("Agumented matrix:");
            PrintMatrix(agumentedMatrix);

            int[] firstPosDif0 =null;

            //Rearrange matrix to a more reversed trapzoid form
            //Main matrix elimination loop
            for (int i = 0; i < iMax; i++)
            {
                // Sort everytime
                UpperTrapezoidSortSwap(null, ref agumentedMatrix, out firstPosDif0);
                for (int di = 0; di < iMax; di++)
                {
                    if (di == i) continue;
                    double mulCoef =0;
                    try
                    {
                        mulCoef = agumentedMatrix[di, firstPosDif0[i]] / agumentedMatrix[i, firstPosDif0[i]];
                    }
                    catch (DivideByZeroException)
                    {
                        inversedMatrix = null;
                        er = "Attempted to divide by 0";
                        return false;
                    }
                    int updateFirstPosDiff = firstPosDif0[di];
                    //Console.WriteLine("frac: " + matrix[di, firstPosDif0[i]]+"\\" + matrix[i, firstPosDif0[i]]);
                    //Console.WriteLine("mul: " + mulCoef);
                    for (int j = 0; j < 2*jMax; j++)
                    {
                        agumentedMatrix[di, j] -= mulCoef * agumentedMatrix[i, j];

                        if (Math.Abs(agumentedMatrix[di, j]) < eps)
                            agumentedMatrix[di, j] = 0;

                        // There is a case in which column i and i+1 have the same value, that means after subtracting 2 rows, there are 2 zeros next to each other.
                        // This if block ensure firstPosDiff is corrected.
                        
                        if (Math.Abs(agumentedMatrix[di, j]) == 0 && Math.Abs(agumentedMatrix[di, updateFirstPosDiff]) == 0 && j == updateFirstPosDiff)
                        {
                            updateFirstPosDiff = j + 1;
                        }
                    }
                    firstPosDif0[di] = updateFirstPosDiff;   
                }
                Console.WriteLine("itr: {0}", i + 1);
                PrintMatrix(agumentedMatrix, true);
                PrintArray(firstPosDif0, s: "First position that is different from 0:");
            }
            for(int i = 0; i < iMax; i++)
            {
                double divCoef = agumentedMatrix[i,firstPosDif0[i]];
                for(int j =  0; j < 2*jMax; j++)
                {
                    agumentedMatrix[i,j] /= divCoef;
                }    
            }

            inversedMatrix = new double[iMax, jMax];
            for (int i = 0; i < iMax; i++)
                for (int j = jMax; j < 2 * jMax; j++)
                    inversedMatrix[i, j-jMax] = agumentedMatrix[i, j];
            //PrintMatrix(matrix);
            return true;

        }

        public static bool InverseJacobi(double[,] matrix, double[,] seed,out double[,] inversedMatrix, out string er, double eps =1e-7)
        {
            inversedMatrix = null;
            er = null;
            // Sort 
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1); ; // First position that is different from 0

            if (iMax != jMax)
            {
                inversedMatrix = null;
                er = "Not a square matrix";
                return false;
            }

            double[,] T = new double[iMax, iMax];
            //Khai báo ma trận T double
            for (int i = 0; i < iMax; i++)
            {
                T[i, i] = 1/matrix[i, i];
            }

            //Khai báo biến X double
            double[,] X = seed;

            // Đi tìm lamda = max abs aii / min abs aii
            double lambda = 0; double minL, maxL;
            minL =maxL= matrix[0, 0];
            for (int i = 0; i < iMax; i++)
            {
                if (matrix[i, i] < minL) minL = matrix[i, i];
                if (matrix[i, i] > maxL) maxL = matrix[i, i];
            }
            lambda = maxL / minL;
            // Tính ma trận B ?
            double[,] B = null;
            double[,] A = matrix;

            int mode = InverseJacobiMatrixNormCheck(matrix);

            switch (mode)
            {
                case 1:
                    B = SubtractIwithMatrix(1, Mul2Matrix(T, A));
                    break;
                case 2:
                    B = SubtractIwithMatrix(1, Mul2Matrix(A, T));
                    break;
                case 0:
                    er = "Không thoả mãn các chuẩn Jacobi";
                    return false;
            }
            double[,] X2 = null;
            InverseJacobiMatrixNormFinder(matrix, mode, out double q);
            double eps0 = eps * (1 - q)/(q*lambda);
            do
            {
                // X = (I - TA)X + T
                X2 = Add2Matrix(Mul2Matrix(B, X), T);
            } while (InverseJacobiMatrixDistance(X, X2,mode,eps0));
            inversedMatrix = X2;
            return true;
        }

        public static bool InverseJacobiMatrixDistance(double[,] X, double[,] X2, int mode ,double eps)
        {
            InverseJacobiMatrixNormFinder(Subtract2Matrix(X , X2),mode,out double q);
            if (q < eps)
                return true;
            else return false;
        }

        public static int InverseJacobiMatrixNormCheck(double[,] matrix)
        {
                int iMax = matrix.GetLength(0);
                int jMax = matrix.GetLength(1);

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
                }
                return 1;

            // Kiểm tra Ma trận chéo chội cột // Check if is diagonally dominant by column
            MODE2:

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
                }
                return 2;


            // Không là ma trận chéo chội // Is not a diagonally dominant A
            MODE0:
                return 0;

        }

        public static void InverseJacobiMatrixNormFinder(double[,] matrix, int mode, out double q)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

            q = 0;

            double sum ;

            switch (mode)
            {
                case 1:
                    sum = 0;
                    for (int i = 0; i < iMax; i++)
                    {
                        for (int j = 0; j < jMax; j++)
                        {
                            sum += Math.Abs(matrix[i, j]);
                        }
                        sum -= Math.Abs(matrix[i, i]);
                    }
                    q = sum;
                    break;
                case 2:
                    sum = 0;
                    for (int j = 0; j < iMax; j++)
                    {
                        for (int i = 0; i < jMax; i++)
                        {
                            sum += Math.Abs(matrix[i, j]);
                        }
                        sum -= Math.Abs(matrix[j, j]);
                    }
                    q = sum;
                    break;
            }

        }



        /*public static bool InverseNewton(double[,] matrix, out double[,] inversedMatrix, out string er, double eps = 1e-6)
        {

        }*/


        //////////////////////////////
        //////////////////////////////
        //  INVERSE BOUNDARY        //
        //////////////////////////////
        //..........................//

        public static void InverseBoundaryMain(double[,] matrix)
        {
            if(!InverseBoundary(matrix,out double[,] invMatrix,out string er))
            {
                Console.WriteLine("Giải ma trận gặp lỗi: {0}", er);
                return;
            }
            PrintMatrix(invMatrix);
        }

        public static bool InverseBoundary(double[,] matrix, out double[,] inversedMatrix, out string er)
        {
            er = null;
            inversedMatrix = null;
            /*Phân tách ma trận ban đầu A thành ma trận dạng viền quanh như trong slide , 
             ma trận nghịch đảo A-1 cũng phân tách như trong slide, ta thu được một hệ 
             phương trình và từ đó giải ra được từng ma trận nhỏ hơn. */

            /*Dễ thấy trong cách giải này có tính chất gần như đệ quy, nên ta sẽ làm ngược 
             * từ các ma trận nhỏ lên, và cuối cùng là ma trận lớn nhất. */

            /* cho một ma trận A = a11 a12 ma trận A-1 = b11 b12
                                   a21 a22               b21 b22   
             */

            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

            if (iMax != jMax)
            {
                er = "Ma trận không vuông!";
                return false;
            }

            double[,] oMatrix = new double[iMax, jMax];
            double[,] tMatrix = null;
            matrix.CopyTo(oMatrix,0);

            /* Đi tìm nghịch đảo của ma trận 2*2, làm nền tảng để tìm nghịch
             tìm các nghịch đảo của các ma trận lớn hơn. */
            bool Ted = false;
            double[,] A2 = new double[2,2];
            A2[0, 0] = matrix[0, 0];
            A2[0, 1] = matrix[0, 1];
            A2[1, 0] = matrix[1, 0];
            A2[1, 1] = matrix[1, 1];
            double detA2 = A2[0, 0] * A2[1, 1] - A2[0, 1] * A2[1, 0];

            /* Kiểm tra detA2 có bằng 0, nếu bằng 0, thử xét với ma trận A* = ATA 
              Đặc biệt, sau khi giải bằng A*, khi trả về kết quả ban đầu, ta cần 
             Nhân ma trận tìm được với AT !!! */
            if (detA2 == 0)
            {
                Ted = true;
                TransposeMatrix(matrix, out tMatrix); // Nghịch đảo ma trận ra ma trận At
                double[,] bMatrix = Mul2Matrix(tMatrix, matrix); // Nhân ma trận AtA
                A2 = new double[2, 2];
                A2[0, 0] = bMatrix[0, 0];
                A2[0, 1] = bMatrix[0, 1];
                A2[1, 0] = bMatrix[1, 0];
                A2[1, 1] = bMatrix[1, 1];
                detA2 = A2[0, 0] * A2[1, 1] - A2[0, 1] * A2[1, 0];
                if (detA2 == 0)
                {
                    er = "Ma trận không thể nghịch đảo được!";
                    return false;
                }
                matrix = bMatrix;
                
            }

            double iDetA2 = 1 / detA2;
            double[,] A2i = new double[2, 2];
            A2i[0, 0] = iDetA2* A2[1, 1];
            A2i[0, 1] = -iDetA2 * A2[0, 1];
            A2i[1, 0] = -iDetA2 * A2[1, 0];
            A2i[1, 1] = iDetA2 * A2[0, 0];

            inversedMatrix = A2i;
            if (iMax == 2)
            {
                return true;
            }

            double[,] An_1n_1i = A2i;

            for (int i = 3; i <= iMax; i++)
            {
                InverseBoundarySeperateRowCol(matrix, 3, out double[,] An_1n_1, out double[,] Ann_1, out double[,] An_1n, out double Ann);

                double[,] X = Mul2Matrix(An_1n_1i, An_1n);
                double[,] Y = Mul2Matrix(Ann_1, An_1n_1i);
                double theta = Ann - Mul2Matrix(Ann_1, X)[0, 0];

                if(theta == 0)
                {
                    er = String.Format("Ma trận không khả nghịch tại bước: {0}", i);
                    return false;
                }

                double[,] Bn_1n_1 = Add2Matrix(An_1n_1i, Mul2Matrix(MulMatrixWithN(X, 1 / theta), Y));
                double Bnn = 1 / theta;
                double[,] Bn_1n = MulMatrixWithN(X, -1 / theta);
                double[,] Bnn_1 = MulMatrixWithN(Y, -1 / theta);
                InverseBoundaryCombineMatrix(Bn_1n_1, Bnn_1, Bn_1n, Bnn, out double[,] Ani);

                An_1n_1i = Ani;
            }

            if (Ted)
            {
                inversedMatrix = Mul2Matrix(An_1n_1i, tMatrix);
            }
            else inversedMatrix = An_1n_1i;
            return true;

        }


        public static bool InverseBoundarySeperateRowCol(double[,] matrix, int n, out double[,] An_1n_1, out double[,] Ann_1, out double[,] An_1n, out double Ann)
        {
            int iMax = matrix.GetLength(0);
            int jMax = matrix.GetLength(1);

            An_1n_1 = null;
            An_1n = null;
            Ann_1 = null;
            Ann = 0;

            if (n > iMax)
                return false;

            An_1n_1 = new double[n - 1, n - 1];
            Ann_1 = new double[1, n - 1];
            An_1n = new double[n - 1, 1];
            for (int i = 0; i < n-1; i++)
            {
                An_1n[0,i] = matrix[i, 1];
                Ann_1[i,0] = matrix[1, i];
                for (int j = 0; j < n-1; j++)
                {
                    An_1n_1[i, j] = matrix[i, j]; 
                }
            }
            Ann = matrix[n, n];
            return true;
        }

        public static void InverseBoundaryCombineMatrix(double[,] Bn_1n_1, double[,] Bnn_1, double[,] Bn_1n, double Bnn , out double[,] Ani)
        {
            int n_1 = Bn_1n_1.GetLength(0);
            int n = n_1 + 1;
            Ani = new double[n, n];
            for (int i = 0; i < n_1; i++)
            {
                Ani[i, n_1] = Bn_1n[i, 1];
                Ani[n_1, i] = Bnn_1[1, i];
                for (int j = 0; j < n_1; j++)
                {
                    Ani[i, j] = Bn_1n_1[i, j];
                }
            }
        }
    }
}
