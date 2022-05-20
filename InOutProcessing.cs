﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Flee.PublicTypes;

namespace NumericalAnalysis
{
    public class InOutProcessing
    {
        public static bool MatrixInput(out double[,] matrix, string fileLocation =@"MatrixInput.txt" )
        {
            if (!File.Exists(fileLocation))
            {
                fileLocation = @"MatrixInput.txt";
                File.Create(fileLocation);
                Console.WriteLine("Input file not detected, created file: MatrixInput.txt at .exe containing folder.");
            }
            int iMax=0, jMax;
            Queue<string> s = new Queue<string>(); string _s;
            using StreamReader sr = File.OpenText(fileLocation);
            while ((_s = sr.ReadLine()) != null)
            {
                s.Enqueue(_s);
                iMax++;
            }
            if(iMax ==0)
            {
                matrix = null;
                return false;
            }
            //Console.WriteLine("iMax = {0} ",iMax);
            Queue<string[]> s_processed = new Queue<string[]>();
            foreach(string rowRaw in s)
            {
                s_processed.Enqueue(rowRaw.Split(' '));
            }
            jMax = s_processed.Peek().Length;
            //Console.WriteLine("jMax = {0}", jMax);
            matrix = new double[iMax, jMax];
            int i = 0;
            foreach (string[] row in s_processed)
            {
                int j = 0;
                foreach(string elem in row)
                {
                    double temp;
                    if (!double.TryParse(elem, out temp))
                    {
                        matrix = null;
                        return false;
                    }
                    matrix[i, j] = temp;
                    j++;
                }
                i++;
            }
            return true;
        }
        public static void MatrixRootOutput(Dictionary<int,Dictionary<int,double>> roots, int maxRoot)
        {
            foreach(KeyValuePair<int, Dictionary<int,double>> root in roots )
            {
                Console.Write("x{0} = ",root.Key+1);
                foreach(KeyValuePair<int,double> variable in root.Value)
                {
                    if (variable.Value == 0) continue;
                    if (variable.Key == maxRoot)
                        Console.Write(" {0} ", variable.Value);
                    else
                        Console.Write(" {0}*x{1} ", variable.Value, variable.Key+1);
                }
                Console.WriteLine();
            }
        }


        public static bool FunctionInput(out double? a, out double? b, out double? eps, out Func<double,double> f, out Func<double,double> df, out Func<double,double> ddf, string fileLocation = @"FunctionInput.txt")
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
                _s.Enqueue(s);
            }
            Queue<string> _sprocessed = new Queue<string>();
            while(_s.Count != 0)
            {
                _sprocessed.Enqueue( _s.Dequeue().Trim());
            }

            a = b = eps = null;
            f = df = ddf = null;

            while(_sprocessed.Count != 0)
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
                }
                if (s2.Contains("f:") && (!s2.Contains("df:")))
                {
                    f = ConvertToFunc(s2.Replace("f:",null));
                    if (f == null)
                        Console.WriteLine(Properties._string.FailedChapter1_Func_f);
                }
                if(s2.Contains("df:") && (!s2.Contains("ddf:")))
                {
                    df = ConvertToFunc(s2.Replace("df:", null));
                    if (df == null)
                        Console.WriteLine(Properties._string.FailedChapter1_Func_df);
                }    
                if(s2.Contains("ddf:"))
                {
                    Console.WriteLine(s2.Replace("ddf:", null));
                    ddf = ConvertToFunc(s2.Replace("ddf:", null));
                    if (ddf == null)
                        Console.WriteLine(Properties._string.FailedChapter1_Func_ddf);
                }
            }
            if (a == null || b == null || eps == null || f == null || df == null || ddf == null)
                return false;
            return true;
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
                }
                if (s2.Contains("f:") && (!s2.Contains("df:")))
                {
                    f = new NAFunc(s2.Replace("f:", null));
                    if (f == null)
                        Console.WriteLine(Properties._string.FailedChapter1_Func_f);
                }
                if (s2.Contains("df:") && (!s2.Contains("ddf:")))
                {
                    df = new NAFunc(s2.Replace("df:", null));
                    if (df == null)
                        Console.WriteLine(Properties._string.FailedChapter1_Func_df);
                }
                if (s2.Contains("ddf:"))
                {
                    Console.WriteLine(s2.Replace("ddf:", null));
                    ddf = new NAFunc(s2.Replace("ddf:", null));
                    if (ddf == null)
                        Console.WriteLine(Properties._string.FailedChapter1_Func_ddf);
                }
            }
            if (a == null || b == null || eps == null || f == null || df == null || ddf == null)
                return false;
            return true;
        }

        public static double FleeLibraryAdapter( double x,  string funcExpression)
        {
            ExpressionContext context = new ExpressionContext();
            context.Imports.AddType(typeof(Math));
            context.Variables.Add("x", x);
            IGenericExpression<double> eGeneric = context.CompileGeneric<double>(funcExpression);
            return eGeneric.Evaluate();
        }
        /// <summary>
        /// Old method, very expensive.
        /// </summary>
        /// <param name="funcExpression"></param>
        /// <returns></returns>
        public static Func<double,double> ConvertToFunc(string funcExpression)
        {
            return new Func<double, double>(x => FleeLibraryAdapter(x, funcExpression));
        }
    }
}
