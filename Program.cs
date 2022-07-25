using System;
using System.Globalization;
using System.Threading;


namespace NumericalAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.Unicode;
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("vi-VN");
            while (true)
            {
                Console.WriteLine("Nhập 2 để chọn phương trình f(x) = a, nhập 3 để chọn Ax = b, nhập 4 để chọn A^-1 , nhập 5 để chọn giá trị riêng/vt riêng , nhập 6 để chọn A = USVt ");
                switch (Console.ReadLine())
                {
                    case "2":
                        Algorithms.Chapter2Main();
                        break;
                    case "3":
                        Algorithms.Chapter3Main();
                        break;
                    case "4":
                        Algorithms.Chapter4Main();
                        break;
                    case "5":
                        Algorithms.Chapter5Main();
                        break;
                    case "6":
                        Algorithms.Chapter6Main();
                        break;
                    case "break":
                        goto BREAK;
                        break;
                }
            }

            BREAK: return;
        }
    }
}