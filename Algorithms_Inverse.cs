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
            Console.Write("Nhập 1 để chọn nghịch đảo bằng PP Gauss \nNhập 2 để chọn nghịch đảo bằng PP Cholesky \nNhập 3 để chọn nghịch đảo bằng PP Jacobi \nNhập 4 để chọn nghịch đảo bằng PP Viền quanh");
            string i = Console.ReadLine();
            string fileLocation = @"MatrixInputInverse.txt";
            Console.WriteLine("Matrix file location: \"MatrixInputInverse.txt\", press any key to continue.");
            Console.ReadLine();
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

                case "3":
                    if (!InOutProcessing.MatrixInput(out matrix, out double[] _, fileLocation))
                    {
                        Console.WriteLine("Invalid Inputs");
                        break;
                    }
                    Algorithms.PrintMatrix(matrix);
                    Console.WriteLine("Nhập epsilon cho ma trận:");
                    double eps;
                    while(!double.TryParse(Console.ReadLine(), out eps))
                    {
                        Console.WriteLine("Nhập lại eps:");
                    }
                    Algorithms.InverseJacobiMain(matrix,eps);
                    break;
                case "4":
                    if (!InOutProcessing.MatrixInput(out matrix, out double[] _, fileLocation))
                    {
                        Console.WriteLine("Invalid Inputs");
                        break;
                    }
                    Algorithms.PrintMatrix(matrix);
                    if(!Algorithms.InverseBoundaryMain(matrix))
                        Console.WriteLine("Ma trận đầu vào không thể nghịch đảo.") ;
                    break;
            }
        }

        #region InverseGaussJordan
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
            InverseCheckResult(matrix, inversedMatrix, out var check);
            PrintMatrix(check);
            return true;
        }
        public static bool InverseGaussJordan(double[,] matrix, out double[,] inversedMatrix, out string er, double eps = 1e-6)
        {
            inversedMatrix = null;
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

            //Rearrange A to a more reversed trapzoid form
            //Main A elimination loop
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
                    //Console.WriteLine("frac: " + A[di, firstPosDif0[i]]+"\\" + A[i, firstPosDif0[i]]);
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
            for(int i = 0;i<iMax;i++)
            {
                if (firstPosDif0[i] > iMax - 1)
                {
                    er = "Det của A bằng 0 nên A không thể nghịch đảo được!";
                    return false;
                }    
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
            //PrintMatrix(A);
            return true;

        }
        #endregion

        #region InverseJacobi
        //////////////////////////////
        //////////////////////////////
        //      INVERSE JACOBI      //
        //////////////////////////////
        //..........................//

        public static bool InverseJacobiMain(double[,] matrix, double eps, double[,] seed = null)
        {
            if(seed == null)
            {
                seed = new double[matrix.GetLength(0), matrix.GetLength(1)];
            }    
            if (!InverseJacobi(matrix ,out var inversedMatrix, out var er, eps))
            {
                Console.WriteLine("Phương pháp Jacobi thất bại, lỗi {0}",er);
                return false;
            }
            Console.WriteLine("Ma trận nghịch đảo tìm được là:");
            PrintMatrix(inversedMatrix);
            InverseCheckResult(matrix, inversedMatrix, out var check);
            PrintMatrix(check);
            return true;
        }

        public static bool InverseJacobi(double[,] A,out double[,] inversedMatrix, out string er, double eps =1e-7)
        {
            inversedMatrix = null;
            er = null;
            // Sort 
            int iMax = A.GetLength(0);
            int jMax = A.GetLength(1); ; // First position that is different from 0

            if (iMax != jMax)
            {
                inversedMatrix = null;
                er = "Not a square matrix";
                return false;
            }

            double[,] T = new double[iMax, iMax];
            //Khai báo ma trận T là ma trận nghịch đảo của đường chéo ma trận A.
            for (int i = 0; i < iMax; i++)
            {
                T[i, i] = 1/A[i, i];
            }

            //Khai báo biến X double
            double[,] X = new double[iMax,iMax];

            // Đi tìm lamda = max abs aii / min abs aii

            // Tính ma trận B ?
            double[,] B = null;

            // Kiểm tra A có thoả mãn các chuẩn của lặp Jacobi hay không?
            int mode = InverseJacobiMatrixNormCheck(A);

            double lambda = 0;

            switch (mode)
            {
                // TH1: Ma trận chéo chội hàng
                case 1:
                    B = InverseJacobiCreateMatrixB(A, 1); // Hàm này tương đương với việc tính B = E - TA, nhưng hiệu quả hơn
                    lambda = 1;
                    break;

                // TH2: Ma trận chéo trội cột
                case 2:
                    B = InverseJacobiCreateMatrixB(A,2); // Hàm này tương đương với việc tính B = E - AT , nhưng hiệu quả hơn
                    double minL, maxL;
                    minL = maxL = A[0, 0];
                    for (int i = 0; i < iMax; i++)
                    {
                        if (A[i, i] < minL) minL = A[i, i];
                        if (A[i, i] > maxL) maxL = A[i, i];
                    }
                    lambda = maxL / minL;
                    break;

                // TH3: Ma trận không thoả điều kiện chuẩn Jacobi
                case 0:
                    er = "Không thoả mãn các chuẩn Jacobi";
                    return false;
            }
            double[,] X2 = new double[iMax,iMax];
            // Chọn điểm xuất phát của X là chính ma trận A (có thể hội tụ lâu, chọn ma trận 1 hay hơn)
            SetSameValue(X2, A);

            // Tìm chuẩn của ma trận B theo loại chuẩn mà thoả A (để đi tìm eps0)
            InverseJacobiMatrixNormFinder(B, mode, out double q);
            double eps0 = eps * (1 - q)/(q*lambda);
            Console.WriteLine("Epsilon0 có giá trị bằng {0}",eps0);
            int count = 1;
            do
            {
                // X = (B*X)+T
                SetSameValue(X, X2);
                X2 = Add2Matrix(Mul2Matrix(B, X), T);
                Console.WriteLine("X tại lần lặp {0}",count);
                PrintMatrix(X2);
                count++;
            } while (!InverseJacobiMatrixDistance(X, X2,mode,eps0));
            inversedMatrix = X2;
            return true;
        }

        public static bool InverseJacobiMatrixDistance(double[,] X, double[,] X2, int mode ,double eps)
        {
            InverseJacobiMatrixNormFinder(Subtract2Matrix(X, X2), mode,out double q);
            if (q < eps)
            {
                Console.WriteLine("Giá trị của q là: {0}", q);
                return true;
            }    
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
                    if (sum >= Math.Abs(matrix[j, j]))
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
            double sumMax = double.MinValue;

            switch (mode)
            {
                case 1:
                    for (int i = 0; i < iMax; i++)
                    {
                        sum = 0;
                        for (int j = 0; j < jMax; j++)
                        {
                            sum += Math.Abs(matrix[i, j]);
                        }
                        sum -= Math.Abs(matrix[i, i]);
                        if (sum > sumMax)
                            sumMax = sum;
                    }
                    q = sumMax;
                    break;
                case 2:
                    for (int j = 0; j < jMax; j++)
                    {
                        sum = 0;
                        for (int i = 0; i < iMax; i++)
                        {
                            sum += Math.Abs(matrix[i, j]);
                        }
                        sum -= Math.Abs(matrix[j, j]);
                        if (sum > sumMax)
                            sumMax = sum;
                    }
                    q = sumMax;
                    break;
            }
            /*if(q >=1)
            {
                Console.WriteLine("CẢNH BÁO: q trong hàm này không nên có giá trị >= 1 ({0})", q);
            } */   
        }
        // Hàm này tạo ra ma trận B
        public static double[,] InverseJacobiCreateMatrixB(double[,] A, int mode)
        {
            int n = A.GetLength(0);
            if (mode == 1)
            {
                double[,] B = new double[n, n];

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i == j)
                        {
                            B[i, j] = 0;
                        }
                        else
                        {
                            B[i, j] = -A[i, j] / A[i, i];
                        }
                    }
                }
                return B;
            }
            if (mode == 2)
            {
                double[,] B1 = new double[n, n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i == j)
                        {
                            B1[i, j] = 0;
                        }
                        else
                        {
                            B1[j, i] = -A[j, i] / A[i, i];
                        }
                    }
                }
                return B1;
            }
            else throw new Exception("MODE IS NOT VALID");
        }
        #endregion

        #region Inverse Newton

        //////////////////////////////
        //////////////////////////////
        //    INVERSE NEWTON        //
        //////////////////////////////
        //..........................//

        // Ý tưởng là đưa về cách lặp Xn+1 = Xn(2 - AXn)

        public static void InverseNewtonMain(double[,] A, double eps = 1e-6)
        {

        }
        /*public static bool InverseNewton(double[,] A, out double[,] inversedMatrix, out string er, double eps = 1e-6)
        {

        }*/


        #endregion

        /*public static bool InverseNewton(double[,] A, out double[,] inversedMatrix, out string er, double eps = 1e-6)
        {

        }*/


        //////////////////////////////
        //////////////////////////////
        //  INVERSE BOUNDARY        //
        //////////////////////////////
        //..........................//

        public static bool InverseBoundaryMain(double[,] matrix)
        {
            if(!InverseBoundary(matrix,out double[,] invMatrix,out string er))
            {
                Console.WriteLine("Giải ma trận gặp lỗi: {0}", er);
                return false;
            }
            PrintMatrix(invMatrix);
            InverseCheckResult(matrix, invMatrix, out var check);
            PrintMatrix(check);
            return true;
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

            //Kiểm tra ma trận vuông ?
            if (iMax != jMax) 
            {
                er = "Ma trận không vuông!";
                return false;
            }

            double[,] oMatrix = new double[iMax, jMax];// oMatrix (original Matrix - biến này lưu giá trị ban đầu của ma trận)
            double[,] tMatrix = null; // tMatrix (transposed matrix - biến này lưu giá trị đã chuyển vị của ma trận)

            SetSameValue(oMatrix, matrix);

            /* Đi tìm nghịch đảo của ma trận 2*2, làm nền tảng để
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
                InverseBoundarySeperateRowCol(matrix, i, out double[,] An_1n_1, out double[,] Ann_1, out double[,] An_1n, out double Ann);

                double[,] X = Mul2Matrix(An_1n_1i, An_1n);
                Console.WriteLine("Ma trận An-1 n-1 nghịch đảo có giá trị:");
                PrintMatrix(An_1n_1i);
                Console.WriteLine("Ma trận An-1 n có giá trị:");
                PrintMatrix(An_1n);
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
                An_1n[i,0] = matrix[i, n-1];
                Ann_1[0,i] = matrix[n-1, i];
                for (int j = 0; j < n-1; j++)
                {
                    An_1n_1[i, j] = matrix[i, j]; 
                }
            }
            Ann = matrix[n-1, n-1];
            return true;
        }

        public static void InverseBoundaryCombineMatrix(double[,] Bn_1n_1, double[,] Bnn_1, double[,] Bn_1n, double Bnn , out double[,] Ani)
        {
            int n_1 = Bn_1n_1.GetLength(0);
            int n = n_1 + 1;
            Ani = new double[n, n];
            for (int i = 0; i < n_1; i++)
            {
                Ani[i, n_1] = Bn_1n[i, 0];
                Ani[n_1, i] = Bnn_1[0, i];
                for (int j = 0; j < n_1; j++)
                {
                    Ani[i, j] = Bn_1n_1[i, j];
                }
            }
            Ani[n_1, n_1] = Bnn;
        }
    }
}
