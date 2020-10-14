using System;

namespace GA
{
    class Program
    {
        static void Main(string[] args)
        {
            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(2, 1, 0, 6);
            geneticAlgorithm.Solve(-4,4);
            Console.WriteLine("Hello World!");
        }
    }
}
