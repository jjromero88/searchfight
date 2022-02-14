using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace TestProject1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Hi, please insert programming languages: ");
            string lenguajes = Console.ReadLine();

            var httpScraper = new WebScraper();
            string result = httpScraper.Getresults(lenguajes);

            Console.WriteLine(result);

            Console.WriteLine("\n \n \n Press ESC to exit");

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
            }



        }
    }
}


