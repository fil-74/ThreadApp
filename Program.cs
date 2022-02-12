using System;
using System.Threading;
using System.IO;

namespace ThreadApp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string file1 = "a.txt";
            string file2 = "b.txt";
            string file3 = "c.txt";
            Generation(file1, file2, file3);

            Console.WriteLine("Начало подсчёта");
             
            var start1 = DateTime.Now;
            double sum1 = Sum(file1, file2, file3);
            var end1 = DateTime.Now;
            var period1 = (end1 - start1).TotalMilliseconds;
            Console.WriteLine($"Последовательное выполнение:\n\tРезультат {sum1}; Время {period1}");

            var start2 = DateTime.Now;
            double sum2 = SumParallel(file1, file2, file3);
            var end2 = DateTime.Now;
            var period2 = (end2 - start2).TotalMilliseconds;
            Console.WriteLine($"Параллельное выполнение:\n\tРезультат {sum2}; Время {period2}");

            Console.Read();

        }

        public static double Sum(string file1, string file2, string file3)
        {
            double sum = 0;
            
            sum += SumInFile(file1);
            sum += SumInFile(file2);
            sum += SumInFile(file3);
            
            return sum;
        }

        public static double SumParallel(string file1, string file2, string file3)
        {
            double sum1 = 0;
            double sum2 = 0;
            double sum3 = 0;

            var t1 = new Thread(() => { sum1 = SumInFile(file1); });
            var t2 = new Thread(() => { sum2 = SumInFile(file2); });
            var t3 = new Thread(() => { sum3 = SumInFile(file3); });

            t1.Start();
            t2.Start();
            t3.Start();

            t1.Join();
            t2.Join();
            t3.Join();

            return sum1 + sum2 + sum3;
        }

        public static double SumInFile(string filename)
        {
            double sum = 0;
            try
            {
                using (var file = new StreamReader(filename))
                {
                    while (!file.EndOfStream)
                    {
                        sum += double.Parse(file.ReadLine());
                        Thread.Sleep(1);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return sum;
        }

        public static void Generation(params string[] files)
        {
            try
            {
                Random rnd = new Random();
                foreach (var file in files)
                {
                    using (StreamWriter main_writer = new StreamWriter(file))
                    {
                        for (int i = 0; i < 100; i++)
                        { main_writer.WriteLine((rnd.Next(0, 10) + rnd.NextDouble()).ToString("f4")); }
                    }
                }
            }
            catch (Exception e)
            { Console.WriteLine(e.Message); }
        }
    }

}
