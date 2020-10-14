using System;

namespace GA
{
    class Program
    {
        static void Main(string[] args)
        {
            
            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(2, 1, -57, 6);
            geneticAlgorithm.Solve(1,10);
            Console.WriteLine("Hello World!");
        }
    }
}
