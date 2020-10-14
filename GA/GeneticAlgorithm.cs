using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;

namespace GA
{
    public class GeneticAlgorithm
    {
        private readonly Random random;
        private int a;
        private int b;
        private readonly int coefficient;
        public int populationNumber;

        public GeneticAlgorithm(int a, int b, int coefficient, int populationNumber)
        {
            this.a = a;
            this.b = b;
            this.coefficient = coefficient;
            this.populationNumber = populationNumber;

            random = new Random();
        }
        //2*4*4-32+0
        private double Fitness(int x, int y) => (2 * x * x + y + coefficient);

        public void Solve(int rangeStart, int rangeStop)
        {
            // initialize population
            List<Chromosome> chromosomes = new List<Chromosome>();
            for (int i = 0; i < populationNumber; i++)
            {
                short firstValue = (short)random.Next(rangeStart, rangeStop);
                short secondValue = (short)random.Next(rangeStart, rangeStop);
                chromosomes.Add(new Chromosome(firstValue, secondValue));
            }

            //print initial population
            for (int i = 0; i < chromosomes.Count; i++)
            {
                Console.WriteLine($"Chromosome {i + 1}: {chromosomes[i].A},{chromosomes[i].B}");
            }
            Chromosome perfectChromosome = null;
            int iterations = 1;
            while (perfectChromosome == null)
            {
                Console.WriteLine($"Iteration {iterations}");
                //calculate fitness
                double[] fitnessValues = new double[populationNumber];
                double[] fittingValues = new double[populationNumber];
                for (int i = 0; i < populationNumber; i++)
                {
                    var fitness = Fitness(chromosomes[i].A, chromosomes[i].B);
                    chromosomes[i].Fitness = fitness;
                    fitnessValues[i] = fitness;
                   fittingValues[i] = 1 / fitness;
                }

                if (chromosomes.FirstOrDefault(x => x.Fitness == 0) != null)
                {
                    perfectChromosome = chromosomes.FirstOrDefault(x => x.Fitness == 0);
                    break;
                }

                double fittingSum = fittingValues.Sum();
                for (int i = 0; i < chromosomes.Count; i++)
                {
                    var c = chromosomes[i];
                    Console.WriteLine($"Chromosome {i + 1}: {c}");
                }

                double[] probfitting = new double[populationNumber];
                for (int i = 0; i < populationNumber; i++)
                {
                    probfitting[i] = (fittingValues[i] / fittingSum);
                    chromosomes[i].ProbFitting = probfitting[i];
                }

                double[] randomProbabilities = new double[populationNumber];
                for (int i = 0; i < populationNumber; i++)
                {
                    randomProbabilities[i] = random.NextDouble();
                    Console.WriteLine($"generated probability for chromosome {i + 1}: {randomProbabilities[i]}");
                }

                List<Chromosome> newPopulation = new List<Chromosome>();
                foreach (var probability in randomProbabilities)
                {
                    var sum = 0.0;
                    for (int j = 0; j < probfitting.Length; j++)
                    {
                        var pr = probfitting[j];
                        sum += pr;
                        if (probability < sum)
                        {
                            var bin = pr;
                            int i = 0;
                            Chromosome newChromosome = chromosomes.FirstOrDefault(x => x.ProbFitting == bin);
                            newPopulation.Add(new Chromosome(newChromosome));
                            break;
                        }
                    }
                }

                for (int i = 0; i < newPopulation.Count; i++)
                {
                    var c = newPopulation[i];
                    Console.WriteLine($"Chromosome {i + 1}: {c}");
                    Console.WriteLine("converted:" + newPopulation[i]);
                }

                double crossoverParameter = 0.5;
                List<Chromosome> parents = new List<Chromosome>();
                List<int> parentIndices = new List<int>();
                while (parents.Count < populationNumber / 2)
                {
                    parents.Clear();
                    parentIndices.Clear();
                    double[] tempCrossoverProbabilities = new double[populationNumber];
                    for (int i = 0; i < tempCrossoverProbabilities.Length; i++)
                    {
                        tempCrossoverProbabilities[i] = random.NextDouble();
                    }

                    for (int i = 0; i < tempCrossoverProbabilities.Length; i++)
                    {
                        if ((crossoverParameter < tempCrossoverProbabilities[i]) && parentIndices.Count < 3)
                        {
                            parents.Add(chromosomes[i]);
                            parentIndices.Add(i);
                        }

                    }
                }

                foreach (var parent in parents)
                {
                    Console.WriteLine("Parent: " + parent);
                }

                
                int chromosomeLength = 1;
                foreach (var p in newPopulation)
                {
                    int length = p.BinaryA.Length >= p.BinaryB.Length ? p.BinaryA.Length : p.BinaryB.Length;
                    if (length > chromosomeLength)
                        chromosomeLength = length;
                }
                int crossoverPosition = chromosomeLength>=16 ? chromosomeLength+2 : 2;
                string[] crossoverMatrix = new string[populationNumber / 2];
                for (int i=0;i<parents.Count;i++)
                {
                    var parent = parents[i];
                    var crossoverString = $"{parent.BinaryA.PadLeft(chromosomeLength,'0')}{parent.BinaryB.PadLeft(chromosomeLength,'0')}";
                    crossoverMatrix[i] = crossoverString;
                }
                for (int i = 0; i < populationNumber / 2; i++)
                {
                    int index = i;
                    int partnerIndex = i + 1;
                    if (i == populationNumber / 2 - 1)
                    {
                        partnerIndex = 0;
                    }
                    var chromosomeToCrossover = crossoverMatrix[index];
                    var partnerChromosome = crossoverMatrix[partnerIndex];
                    var start = chromosomeToCrossover.Substring(0, crossoverPosition);
                    var end = partnerChromosome.Substring(crossoverPosition);
                    crossoverMatrix[i] = $"{start}{end}";
                }

                //update parents after crossover
                Console.WriteLine("Parents after crossover");
                for (int i = 0; i < parents.Count; i++)
                {
                    parents[i].UpdateBinaryRepresentation(crossoverMatrix[i]);
                    Console.WriteLine("Parent: " + parents[i]);
                }

                for (int i = 0; i < parentIndices.Count; i++)
                {
                    int index = parentIndices[i];
                    newPopulation[index].UpdateBinaryRepresentation(crossoverMatrix[i]);
                }

                Console.WriteLine("population after crossing-over");
                for (int i = 0; i < newPopulation.Count; i++)
                {
                    Console.WriteLine($"Chromosome {i + 1}:{newPopulation[i]}");

                }
                
                //mutations:
                double mutationParameter = 0.1;
                int numberOfGenesToMutate = (int) Math.Ceiling(mutationParameter * newPopulation.Count *
                                                               chromosomeLength);
                Tuple<int, int>[] mutationTuples =
                    ReturnMutationTuple(numberOfGenesToMutate, newPopulation.Count, chromosomeLength*2);


                for (int i = 0; i < numberOfGenesToMutate; i++)
                {
                    int index = mutationTuples[i].Item1;
                    int position = mutationTuples[i].Item2;
                    newPopulation[index].Mutate(position, position/chromosomeLength, chromosomeLength);
                }

                //genes after mutation:
                Console.WriteLine("genes after mutation:");
                for (int i = 0; i < newPopulation.Count; i++)
                {
                    Console.WriteLine($"Chromosome {i + 1}:{newPopulation[i]}");

                }
                //calculate fitness value again to see if perfect child has appeared

                for (int i = 0; i < newPopulation.Count; i++)
                {
                    newPopulation[i].Fitness = Fitness(newPopulation[i].A, newPopulation[i].B);
                    if (newPopulation[i].Fitness == 0)
                    {
                        perfectChromosome = newPopulation[i];
                        break;
                    }
                }

                if (perfectChromosome == null)
                {
                   chromosomes.Clear();
                   foreach (var chromosome in newPopulation)
                   {
                       chromosomes.Add(new Chromosome(chromosome));
                   }
                }
                iterations++;
            }
            Console.WriteLine($"Solution found in {iterations} iterations! It's {perfectChromosome}");


        }

        private Tuple<int, int>[] ReturnMutationTuple(in int numberOfGenesToMutate, int count, int chromosomeLength)
        {
            List<Tuple<int, int>> mutationTuples = new List<Tuple<int, int>>();
            while (!ContainsOnlyUniqueValues(mutationTuples))
            {
                for (int i = 0; i < numberOfGenesToMutate; i++)
                {
                    var mutatedGeneIndex = random.Next(0, count);
                    var positionOfGene = random.Next(0, chromosomeLength);
                    mutationTuples.Add(new Tuple<int, int>(mutatedGeneIndex, positionOfGene));
                }
            }

            return mutationTuples.ToArray();
        }

        private bool ContainsOnlyUniqueValues(List<Tuple<int, int>> mutationTuples)
        {
            if (mutationTuples.Count == 0)
                return false;
            if (mutationTuples.All(new HashSet<Tuple<int, int>>().Add)) 
                return true;
            mutationTuples.Clear();
            return false;
        }


    }
}
