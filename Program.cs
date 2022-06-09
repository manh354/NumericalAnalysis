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
                Console.WriteLine("Nhập 2 để chọn chương 2, nhập 3 để chọn chương 3, nhập 4 để chọn chương 4");
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
                    case "break":
                        goto BREAK;
                        break;
                }
            }

            BREAK: return;
        }
    }
}