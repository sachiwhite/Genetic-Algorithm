using System;

namespace GA
{
    class Program
    {
        static void Main(string[] args)
        {
            //f(x,y) = ax^2+by+coefficient = 0
            //f(x,y) = 3x^2+2y-10 = 0
            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(3, 2, -10, 6);
            geneticAlgorithm.Solve(0,10);
        }
    }
}
