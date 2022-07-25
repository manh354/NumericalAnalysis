namespace NumericalAnalysis {
    
    
    using System.Collections.Generic;
    
    using System;
    
    using System.Linq;
    
    public static partial class Algorithms {
        
        // 
        //         Solve special case A[k][k-1] = 0 and A[k][j] != 0 (j < k-1)
        //         Switch column (row) k-1 and column (row) j
        //         Args:
        //             A: 2-D numpy array
        //             k: Element manipulation
        //         Return:
        //             Matrix is smilarity A
        //     
        public static double[,] specialCase1(double[,] A, int k ,int j)
        {
            int n = A.GetLength(0);
            var result = new double[n, n];
            SetSameValue(result, A);
            SwapRow(ref result, k - 1, j);
            SwapCol(ref result, k - 1, j);
            return result;
        }
        
        // 
        //         Solve special case A[k][k-1] = 0 and A[k][j] = 0 (j<k-1)
        //         Args:
        //             2-D numpy array
        //             k: Element manipulation
        //         Return:
        //             subFrobenius, B 
        //     
        public static void specialCase2(double[,] A, int k , out double[,] subFrobenius,out double[,] B) {
            int n = A.GetLength(0);
            int m = A.GetLength(1);
            subFrobenius = CopyFromMatrix(A, k, n - 1, k, m - 1);
            B = CopyFromMatrix(A, 0, k-1, 0, k-1);
        }

        // 
        //         Find similar matrix (simple case - A[k][k-1]!=0) 
        //         Args:
        //             A: 2-D numpy array
        //             k: Element manipulation - A[k][k-1]
        //         Return:
        //             Similarity transformation of matrix A
        //     
        public static void findSimpleA(double[,] A, int k, out double[,] similarA, out double[,] M, out double[,] inversedM) {
            int n = A.GetLength(0);
            M = EyeMatrix(n);

            // Đi tìm M
            for (int j = 0; j < n; j++)
            {
                M[k - 1, j] = A[k, j];
            }

            // Đi tim nghịch đảo của M
            inversedM = EyeMatrix(n);
            for (int i = 0; i < n; i++)
            {
                inversedM[k - 1, i] = -A[k, i] / A[k, k - 1];
            }
            inversedM[k - 1, k - 1] = 1 / A[k, k - 1];

            similarA = Mul2Matrix(Mul2Matrix(M, A), inversedM);

        }
        
        // 
        //         Get characteristic polynomial from Frobenius matrix
        //         Args:
        //             A: 2-D Frobenius matrix
        //         Return:
        //             p: 1-d coefficients (high --> low)
        //     
        public static void getCharPolynomial(double[,] A, out double[,] p) {
            var n = A.GetLength(0);
            p = MulMatrixWithN(OneArray(n+1), Math.Pow(-1, n));

            for (int i = 1; i < n + 1; i++)
                p[0, i] = -p[0, i] * A[0,i-1];
                
            return;
        }
        
        // 
        //         Multiple coeffictients of polynomials
        //         p: list coefficients of polynomials
        //     
        public static object mulPoly(double[,] p) {
            var res = p[0,0];
            for(int i = 1; i < p.GetLength(1); i++) {
                res *= p[0,i];
            }
            return res;
        }
        
        // 
        //         Danilevski method for find eigenvalues, eigenvectors
        //         Args:
        //             A: 2-d numpy array
        //         Output:
        //             list_eigenvalues: Eigenvalues
        //             list_eigenvectors: Eigenvectors
        //     
        public static Tuple<List<double>, List<double[,]>> Danilevski(double[,] A) {
            double[,] p;
            double[,] v;
            List<double> eigenValues;
            int t;
            double[,] X;
            double[,] y;
            double[,] M;
            double[,] inverseM;
            var all_polynomial = new List<object>();
            var n = A.GetLength(0);
            var n2 = A.GetLength(1);
            if(n!=n2)
            {
                Console.WriteLine("Cảnh báo: ma trận không vuông");
                return null;
            }    
            double[,] back = EyeMatrix(n);
            double[,] similar = new double[n,n];
            SetSameValue(similar,A);
            var list_eigenvalues = new List<double>();
            var list_eigenvectors = new List<double[,]>();
            var charFunc = new List<object>();
            int k = n - 1;
            int m = n;
            while (k > 0) {
                if (similar[k,k - 1] != 0) {
                    //Simple case
                    findSimpleA(similar, k,out similar, out M,out inverseM);
                    back = Mul2Matrix(back,inverseM);
                } else {
                    var non = false;
                    for (int j =0;j<k-1;j++) {
                        //tìm similar[k][j] if equal 0
                        if (similar[k,j] != 0) {
                            similar = specialCase1(similar, k, j);
                            SwapCol(ref back, j, k - 1);
                            non = true;
                            k = k + 1;
                            break;
                        }
                    }
                    if (!non) {
                        //Decompose columns
                        //                 print("Case 2")
                        for (int j=k; j<m-1;j++) {
                            M = EyeMatrix(m);
                            /* M[:k, j + 1] = -similar[:k, j] */
                            for (int di = 0; di < k; di++)
                            {
                                M[di, j + 1] = -similar[di, j];
                            }
                                
                            //Inverse M
                            inverseM = EyeMatrix(m);
                            /* inverseM[:k, j + 1] = similar[:k, j] */
                            for (int di = 0; di < k; di++)
                                inverseM[di, j + 1] = similar[di, j];
                            similar = Mul2Matrix(Mul2Matrix(M, similar), inverseM);
                            back = Mul2Matrix(back, inverseM);
                        }
                        var tt = false;
                        //Find similar A[j, m-1] != 0
                        for(int j = k-1;j>-1;j--) {
                            if (similar[j,m - 1] != 0) {
                                M = EyeMatrix(m);
                                var x = new double[m - k, m];
                                #region x = M[k:m, :]
                                int dix = 0;
                                for(int di = k; di < m; di++)
                                {
                                    for(int dj = 0;dj<m;dj++)
                                    {
                                        x[dix, dj] = M[di, dj];
                                    }
                                    dix++;
                                }
                                #endregion
                                y = new double[1, m];
                                #region y = M[k-1, :]
                                for (int dj = 0; dj < m; dj++)
                                {
                                    y[0, dj] = M[k - 1, dj];
                                }
                                #endregion
                                var M_0_k_1 = new double[k - 1, m];
                                #region M[0:k-1]
                                for (int di = 0; di < k - 1; di++)
                                {
                                    for(int dj = 0;dj<m;dj++)
                                    {
                                        M_0_k_1[di, dj] = M[di, dj];
                                    }    
                                }
                                #endregion
                                M = VStack(M_0_k_1, x, y);
                                TransposeMatrix(M,out double[,] M1);
                                /*similar = M.dot(similar).dot(M1);*/
                                similar = Mul2Matrix(Mul2Matrix(M, similar), M1);
                                back = Mul2Matrix(back,M1);
                                k = m;
                                tt = true;
                                break;
                            }
                        }
                        if (!tt) {
                            X = CopyFromMatrix(similar, k, similar.GetLength(0) - 1, k, similar.GetLength(1)-1);
                            //Get size of X
                            t = X.GetLength(0);
                            eigenValues = findValue(X);
                            foreach (var j in Enumerable.Range(0, eigenValues.Count())) {
                                Console.WriteLine("Vector rieng cua: ", eigenValues[j]);
                                list_eigenvalues.Add(eigenValues[j]);
                                y = new double[t,1];
                                for (int i = 0; i < t; i++) 
                                {
                                    y[i, 0] = Math.Pow(eigenValues[j], i);
                                }
                                for(int i = 0; i < t / 2; i++)
                                {
                                    y[i, 0] = y[t - 1 - i, 0];
                                }
                                v = Zeros(n,1);
                                p = Zeros(m,1);
                                int dix = 0;
                                for(int i = k; i < m; i++)
                                {
                                    p[i, 0] = y[dix, 0];
                                    dix++;
                                }
                                    
                                p = Mul2Matrix(back,p);
                                for (int i = 0; i < m; i++)
                                    v[i, 0] = p[i, 0];
                                list_eigenvectors.Add(v);
                                PrintMatrix(v);
                            }
                            m = k;
                            similar = CopyFromMatrix(similar,0,k-1,0,k-1);
                            back = EyeMatrix(m);
                            //         print("K: ",k)
                        }
                    }
                }
                k = k - 1;
            }
            X = similar;
            t = X.GetLength(0);
            eigenValues = findValue(X);
            foreach (var j in Enumerable.Range(0, eigenValues.Count)) {
                Console.WriteLine("Vector rieng cua: {0}", eigenValues[j]);
                list_eigenvalues.Add(eigenValues[j]);
                y = new double[t, 1];
                for (int i = 0; i < t; i++)
                {
                    y[i, 0] = Math.Pow(eigenValues[j], i);
                }
                for (int i = 0; i < t / 2; i++)
                {
                    (y[i, 0] , y[t - 1 - i, 0]) = ( y[t - 1 - i, 0], y[i, 0]);
                }
                v = Zeros(n, 1);
                p = Zeros(m, 1);
                int dix = 0;
                for (int i = k; i < m; i++)
                {
                    p[i, 0] = y[dix, 0];
                    dix++;
                }

                p = Mul2Matrix(back, p);
                for (int i = 0; i < m; i++)
                    v[i, 0] = p[i, 0];
                list_eigenvectors.Add(v);
                PrintMatrix(v);
            }
            return Tuple.Create(list_eigenvalues, list_eigenvectors);
        }
        
        // 
        //         Find eigenvalue for frobenius matrix
        //         Args:
        //             A: 2-d numpy array
        //         Return:
        //             Eigenvalue
        //     
        public static List<double> findValue(double[,] A) {
            getCharPolynomial(A, out var _p);
            var p = new double[_p.GetLength(1)];
            for(int j = 0; j < _p.GetLength(1); j++)
            {
                p[j] = _p[0, j];
            }
            Array.Reverse(p);
            var eigenValues = findRoots(p);
            
            eigenValues.Sort();
            return eigenValues;
        }

        public static double[,] A =
        {{1, 2, 3, 4 }, {2, 1, 2, 3 }, {3, 2, 1, 2 }, {4, 3, 2, 1 } };



        public static List<double> findRoots(double[] a)
        {
            //roots = np.zeros(len(a)-1)
            var roots = new List<double>();
            foreach (var r in Enumerable.Range(0, a.Count() - 1))
            {
                var root = newtonHorner(a, 0);
                if (root == double.NaN)
                {
                    break;
                }
                else
                {
                    roots.Add(root);
                    Console.WriteLine("Root {0}",root);
                    a = quotient(a, root);
                }
            }
            return roots;
        }

        public static double[] quotient(double[] a, double x)
        {
            var q = Zeros(a.Length);
            q[a.Length - 1] = a[a.Length - 1];
            for(int i = a.Length-2;i>-1;i--)
            {
                q[i] = x * q[i + 1] + a[i];
            }
            double[] q2 = new double[a.Length - 1];
            for(int i = 1;i<q.Length;i++)
            {
                q2[i-1] = q[i];
            }    
            return q2;
        }

        public static double newtonHorner(double[] a, double x)
        {
            var i = 0;
            var px = horner(a, x);
            var qx = hornerDeriv(a, x);
            while (Math.Abs(px) > 1E-07 && i < 1000)
            {
                if (qx == 0)
                {
                    // avoid divide by zero
                    return double.NaN;
                }
                x = x - px / qx;
                px = horner(a, x);
                //qx = horner(q,x) # Power Rule
                qx = hornerDeriv(a, x);
                i = i + 1;
            }
            if (i == 1000)
            {
                // hit the iteration cap (divierging or stuck in loop)
                return double.NaN;
            }
            var r = Math.Round(x);
            if (Math.Abs(horner(a, r)) < Math.Abs(horner(a, x)))
            {
                x = r;
            }
            return x;
        }

        public static double hornerDeriv(double[] a, double x)
        {
            var q = quotient(a, x);
            return horner(q, x);
        }

        public static double horner(double[] a, double x)
        {
            var result = a[a.Length - 1];
            for(int i = a.Length-2; i>-1;i--)
            {
                result = a[i] + x * result;
            }
            return result;
        }
    }
}
