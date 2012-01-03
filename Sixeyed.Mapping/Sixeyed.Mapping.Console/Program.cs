using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Sixeyed.Mapping.Tests;

namespace Sixeyed.Mapping.ConsoleTest
{
    class Program
    {
        public const int PerformanceIterations = 10000;

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Used for performance test sessions");
            Console.WriteLine("m: manual map, a: automap, ax: automap exact, ar: automap reused, s: static map");
            Console.WriteLine("c: manual class map, cs: static class map; ca: actions class map");
            Console.WriteLine("drm: data reader manual; dra: data reader auto; drn: data reader auto nested; drs: data reader static");
            Console.WriteLine("x: exit");
            var input = Console.ReadLine();
            while (input != "x")
            {
                Process(input);
                input = Console.ReadLine();
            }
        }

        private static void Process(string input)
        {
            switch (input)
            {
                case "m":
                    RunManual();
                    break;
                case "a":
                    RunAuto();
                    break;
                case "ax":
                    RunAutoExact();
                    break;
                case "ar":
                    RunAutoReused();
                    break;
                case "s":
                    RunStatic();
                    break;
                case "c":
                    RunClassManual();
                    break;
                case "cs":
                    RunClassStatic();
                    break;
                case "ca":
                    RunClassStaticActions();
                    break;
                case "drm":
                    RunDataReaderManual();
                    break;
                case "dra":
                    RunDataReaderAuto();
                    break;
                case "drn":
                    RunDataReaderAutoNested();
                    break;
                case "drs":
                    RunDataReaderStatic();
                    break;
                default:
                    break;
            }
        }

        private static void RunDataReaderStatic()
        {
            var test = new DataReaderMapPerformanceTest();
            test.PerformanceIterations = PerformanceIterations;
            Console.WriteLine("Starting FromSqlCe_StaticMap");
            var stopwatch = Stopwatch.StartNew();
            test.FromSqlCe_StaticMap();
            stopwatch.Stop();
            Console.WriteLine("FromSqlCe_StaticMap completed in: {0} ms", stopwatch.ElapsedMilliseconds);
        }

        private static void RunDataReaderAutoNested()
        {
            var test = new DataReaderMapPerformanceTest();
            test.PerformanceIterations = PerformanceIterations;
            Console.WriteLine("Starting FromSqlCe_AutoMap_Nested");
            var stopwatch = Stopwatch.StartNew();
            test.FromSqlCe_AutoMap_Nested();
            stopwatch.Stop();
            Console.WriteLine("FromSqlCe_AutoMap_Nested completed in: {0} ms", stopwatch.ElapsedMilliseconds);
        }

        private static void RunDataReaderAuto()
        {
            var test = new DataReaderMapPerformanceTest();
            test.PerformanceIterations = PerformanceIterations;
            Console.WriteLine("Starting FromSqlCe_AutoMap");
            var stopwatch = Stopwatch.StartNew();
            test.FromSqlCe_AutoMap();
            stopwatch.Stop();
            Console.WriteLine("FromSqlCe_AutoMap completed in: {0} ms", stopwatch.ElapsedMilliseconds);
        }

        private static void RunDataReaderManual()
        {
            var test = new DataReaderMapPerformanceTest();
            test.PerformanceIterations = PerformanceIterations;
            Console.WriteLine("Starting FromSqlCe_Manual");
            var stopwatch = Stopwatch.StartNew();
            test.FromSqlCe_Manual();
            stopwatch.Stop();
            Console.WriteLine("FromSqlCe_Manual completed in: {0} ms", stopwatch.ElapsedMilliseconds);
        }

        private static void RunClassManual()
        {
            var test = new ClassMapPerformanceTest();
            test.PerformanceIterations = PerformanceIterations;
            Console.WriteLine("Starting Class Manual");
            var stopwatch = Stopwatch.StartNew();
            test.UserToUserModel_Manual();
            stopwatch.Stop();
            Console.WriteLine("Class Manual completed in: {0} ms", stopwatch.ElapsedMilliseconds);
        }

        private static void RunClassStatic()
        {
            var test = new ClassMapPerformanceTest();
            test.PerformanceIterations = PerformanceIterations;
            Console.WriteLine("Starting Class Static");
            var stopwatch = Stopwatch.StartNew();
            test.FullUserToPartialUser();
            stopwatch.Stop();
            Console.WriteLine("Class Static completed in: {0} ms", stopwatch.ElapsedMilliseconds);
        }

        private static void RunClassStaticActions()
        {
            var test = new ClassMapPerformanceTest();
            test.PerformanceIterations = PerformanceIterations;
            Console.WriteLine("Starting Class Static Actions");
            var stopwatch = Stopwatch.StartNew();
            test.FullUserToPartialUser_Actions();
            stopwatch.Stop();
            Console.WriteLine("Class Static Actions completed in: {0} ms", stopwatch.ElapsedMilliseconds);
        } 

        private static void RunManual()
        {
            var test = new PerformanceTest();
            Console.WriteLine("Starting Manual map");
            var stopwatch = Stopwatch.StartNew();
            test.UserToUserModel_Manual();
            stopwatch.Stop();
            Console.WriteLine("Manual completed in: {0} ms", stopwatch.ElapsedMilliseconds);
        }

        private static void RunAutoExact()
        {
            var test = new PerformanceTest();            
            Console.WriteLine("Starting AutoMap Exact");
            var stopwatch = Stopwatch.StartNew();
            test.UserToUserModel_AutoMap_ExactNaming();
            stopwatch.Stop();
            Console.WriteLine("AutoMap Exact completed in: {0} ms", stopwatch.ElapsedMilliseconds);
        }        

        private static void RunAuto()
        {
            var test = new PerformanceTest();
            Console.WriteLine("Starting AutoMap");
            var stopwatch = Stopwatch.StartNew();
            test.UserToUserModel_AutoMap();
            stopwatch.Stop();
            Console.WriteLine("AutoMap completed in: {0} ms", stopwatch.ElapsedMilliseconds);
        }

        private static void RunAutoReused()
        {
            var test = new PerformanceTest();
            Console.WriteLine("Starting AutoMap_Reused");
            var stopwatch = Stopwatch.StartNew();
            test.UserToUserModel_AutoMap_Reused();
            stopwatch.Stop();
            Console.WriteLine("AutoMap_Reused completed in: {0} ms", stopwatch.ElapsedMilliseconds);
        }

        private static void RunStatic()
        {
            var test = new PerformanceTest();
            Console.WriteLine("Starting Static");
            var stopwatch = Stopwatch.StartNew();
            test.UserToUserModel_StaticMap();
            stopwatch.Stop();
            Console.WriteLine("Static completed in: {0} ms", stopwatch.ElapsedMilliseconds);
        }
    }
}
