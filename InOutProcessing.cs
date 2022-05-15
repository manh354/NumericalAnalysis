using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NumericalAnalysis
{
    public class InOutProcessing
    {
        public static bool MatrixInput(out double[,] matrix, string fileLocation )
        {
            if (!File.Exists(fileLocation))
            {
                matrix = null;
                return false;
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
            Console.WriteLine("iMax = {0} ",iMax);
            Queue<string[]> s_processed = new Queue<string[]>();
            foreach(string rowRaw in s)
            {
                s_processed.Enqueue(rowRaw.Split(' '));
            }
            jMax = s_processed.Peek().Length;
            Console.WriteLine("jMax = {0}", jMax);
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
                    if (variable.Key == maxRoot)
                        Console.Write(" {0} ", variable.Value);
                    else
                        Console.Write(" {0}*x{1} ", variable.Value, variable.Key+1);
                }
                Console.WriteLine();
            }
        }
    }
}
