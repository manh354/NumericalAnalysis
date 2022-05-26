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
                Console.WriteLine("Nhập 1 để chọn chương 1, nhập 2 để chọn chương 2");
                switch (Console.ReadLine())
                {
                    case "1":
                        Algorithms.Chapter1Main();
                        break;
                    case "2":
                        Algorithms.Chapter2Main();
                        break;
                    case "3":
                        LowPrecision.BT4();
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