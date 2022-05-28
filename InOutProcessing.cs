using System;
using System.Collections.Generic;
using System.IO;

namespace NumericalAnalysis
{
    public class InOutProcessing
    {
        public static bool MatrixInput(out double[,] matrix, out double[] seed, string fileLocation = @"MatrixInput.txt")
        {
            if (!File.Exists(fileLocation))
            {
                fileLocation = @"MatrixInput.txt";
                File.Create(fileLocation);
                Console.WriteLine("Input file not detected, created file: MatrixInput.txt at .exe containing folder.");
            }
            int iMax = 0, jMax;
            Queue<string> s = new Queue<string>(); string _s;
            using StreamReader sr = File.OpenText(fileLocation);
            while ((_s = sr.ReadLine()) != null)
            {
                if (s.Contains("//")) continue;
                s.Enqueue(_s);
                if (_s.Contains("seed:")) continue;
                iMax++;
            }
            if (iMax == 0)
            {
                matrix = null;
                seed = null;
                return false;
            }
            //Console.WriteLine("iMax = {0} ",iMax);
            Queue<string[]> s_processed = new Queue<string[]>();
            string[] sseed = null;
            seed = new double[iMax];

            foreach (string rowRaw in s)
            {
                
                Console.WriteLine(rowRaw);
                if (rowRaw.Contains("seed:"))
                {
                    sseed = rowRaw.Replace("seed:",null).Split(' ');
                    continue;
                }
                s_processed.Enqueue(rowRaw.Trim().Split(' '));
            }
            jMax = s_processed.Peek().Length;
            //Console.WriteLine("jMax = {0}", jMax);
            matrix = new double[iMax, jMax];
            int i = 0;
            foreach (string[] row in s_processed)
            {
                int j = 0;
                foreach (string elem in row)
                {
                    double temp;
                    if (!double.TryParse(elem, out temp))
                    {
                        matrix = null;
                        seed = null;
                        return false;
                    }
                    matrix[i, j] = temp;
                    j++;
                }
                i++;
            }
            int l = 0;
            foreach (string elem in sseed)
            {
                //Console.WriteLine("elem: {0}", elem);
                double temp;
                if (!double.TryParse(elem, out temp))
                {
                    continue;
                }
                else
                {
                    seed[l] = temp;
                    l++;
                }
            }
            return true;
        }
        public static void MatrixRootOutput(Dictionary<int, Dictionary<int, double>> roots, int maxRoot)
        {
            foreach (KeyValuePair<int, Dictionary<int, double>> root in roots)
            {
                Console.Write("x{0} = ", root.Key + 1);
                foreach (KeyValuePair<int, double> variable in root.Value)
                {
                    if (variable.Value == 0) continue;
                    if (variable.Key == maxRoot)
                        Console.Write(" {0} ", variable.Value);
                    else
                        Console.Write(" {0}*x{1} ", variable.Value, variable.Key + 1);
                }
                Console.WriteLine();
            }
        }

        public static bool FunctionInput(out double? a, out double? b, out double? eps, out NAFunc f, out NAFunc df, out NAFunc ddf, string fileLocation = @"FunctionInput.txt")
        {
            if (!File.Exists(fileLocation))
            {
                File.Create(fileLocation);
                Console.WriteLine(Properties._string.NoInputFileChapter1);
                a = b = eps = null;
                f = df = ddf = null;
                return false;
            }
            Queue<string> _s = new Queue<string>();
            using StreamReader sr = File.OpenText(fileLocation);
            string s;
            while ((s = sr.ReadLine()) != null)
            {
                if (s.Contains("//")) continue;
                _s.Enqueue(s);
            }
            Queue<string> _sprocessed = new Queue<string>();
            while (_s.Count != 0)
            {
                _sprocessed.Enqueue(_s.Dequeue().Trim());
            }

            a = b = eps = null;
            f = df = ddf = null;

            while (_sprocessed.Count != 0)
            {
                string s2 = _sprocessed.Dequeue();
                double temp;
                if (s2.Contains("a:"))
                {
                    //Console.WriteLine("Processing a");
                    if (double.TryParse(s2.Replace("a:", null), out temp))
                        a = temp;
                    else
                    {
                        Console.WriteLine(Properties._string.FailedChapter1_Var_a);
                        a = null;
                    }
                    continue;
                }
                if (s2.Contains("b:"))
                {
                    //Console.WriteLine("Processing b");
                    if (double.TryParse(s2.Replace("b:", null), out temp))
                        b = temp;
                    else
                    {
                        Console.WriteLine(Properties._string.FailedChapter1_Var_b);
                        b = null;
                    }
                    continue;
                }
                if (s2.Contains("eps:"))
                {
                    //Console.WriteLine("Processing eps");
                    if (double.TryParse(s2.Replace("eps:", null), out temp))
                        eps = temp;
                    else
                    {
                        Console.WriteLine(Properties._string.FailedChapter1_Var_eps);
                        eps = null;
                    }
                    continue;
                }
                if (s2.Contains("f:") && (!s2.Contains("df:")))
                {
                    f = new NAFunc(s2.Replace("f:", null));
                    if (f == null)
                        Console.WriteLine(Properties._string.FailedChapter1_Func_f);
                    continue;
                }
                if (s2.Contains("df:") && (!s2.Contains("ddf:")))
                {
                    df = new NAFunc(s2.Replace("df:", null));
                    if (df == null)
                        Console.WriteLine(Properties._string.FailedChapter1_Func_df);
                    continue;
                }
                if (s2.Contains("ddf:"))
                {
                    ddf = new NAFunc(s2.Replace("ddf:", null));
                    if (ddf == null)
                        Console.WriteLine(Properties._string.FailedChapter1_Func_ddf);
                    continue;
                }
            }
            if (a == null || b == null || eps == null || f == null || df == null || ddf == null)
                return false;
            return true;
        }

        public static bool PolyInput(out List<double> coefs, out double? eps, string fileLocation = @"Poly.txt")
        {
            if (!File.Exists(fileLocation))
            {
                File.Create(fileLocation);
                Console.WriteLine(Properties._string.NoInputFileChapter1);
                eps = null;
                coefs= null;
                return false;
            }
            eps = null;
            coefs = new List<double>();

            Queue<string> _s = new Queue<string>();
            using StreamReader sr = File.OpenText(fileLocation);
            string s;
            while ((s = sr.ReadLine()) != null)
            {
                if (s.Contains("//")) continue;
                _s.Enqueue(s);
            }
            Queue<string> _sprocessed = new Queue<string>();
            while (_s.Count != 0)
            {
                _sprocessed.Enqueue(_s.Dequeue().Trim());
            }
            while (_sprocessed.Count!=0)
            {
                string s2 = _sprocessed.Dequeue();
                if(s2.Contains("coefs:"))
                {
                    string[] scoefs = s2.Replace("coefs:", null).Split(" ");
                    foreach(string s4 in scoefs)
                    {
                        double temp;
                        if (!double.TryParse(s4, out temp))
                        {
                            coefs = null;
                            break;
                        }
                        coefs.Add(temp);
                    }    
                }
                if (s2.Contains("eps:"))
                {
                    double temp;
                    //Console.WriteLine("Processing eps");
                    if (double.TryParse(s2.Replace("eps:", null), out temp))
                        eps = temp;
                    else
                    {
                        Console.WriteLine(Properties._string.FailedChapter1_Var_eps);
                        eps = null;
                    }
                    continue;
                }
            }
            if ( eps == null || coefs == null)
                return false;
            return true;
        }

        public void MatrixOutput(double[] roots, string outputLoc =  "MatrixOutput.txt")
        {
            StreamWriter sw = new StreamWriter(outputLoc);
            foreach(double root in roots)
            {
                sw.Write(root.ToString() + " ");
            }    
        }

        public void ReplaceMatrix(double[,] A, double[] roots)
        {

        }
    }
}
