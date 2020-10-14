using System;

namespace GA
{
    class Program
    {
        static void Main(string[] args)
        {
            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(2, 1, -20, 6);
            geneticAlgorithm.Solve();
            Console.WriteLine("Hello World!");
        }
    }
}
