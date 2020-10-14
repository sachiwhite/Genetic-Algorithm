using System;

namespace GA
{
    public class Chromosome
    {
        public int A { get; private set; }
        public int B { get; private set; }
        public double Fitness { get; set; }
        public double FittingValue { get; set; }
        public double ProbFitting { get; set; }
        public string BinaryRepresentationOfChromosome { get; private set; }

        public Chromosome(Chromosome chromosomeToCopy)
        {
            A = chromosomeToCopy.A;
            B = chromosomeToCopy.B;
            Fitness = chromosomeToCopy.Fitness;
            FittingValue = chromosomeToCopy.FittingValue;
            ProbFitting = chromosomeToCopy.ProbFitting;
            BinaryRepresentationOfChromosome = chromosomeToCopy.BinaryRepresentationOfChromosome;

        }
        public Chromosome(int A, int B)
        {
            this.A =A;
            this.B = B;
            BinaryRepresentationOfChromosome = ConvertToBinary();
        }
        

        public override bool Equals(object? obj)
        {
            if (!(obj is Chromosome))
            {
                return false;
            }

            return this.A == ((Chromosome) obj).A && this.B == ((Chromosome) obj).B;
        }

        public override string ToString()
        {
            string partA = BinaryRepresentationOfChromosome.Substring(0, 4);
            string partB = BinaryRepresentationOfChromosome.Substring(4);
            return $"Chromosome {A}, {B}: {partA}, {partB}";
        }

        public override int GetHashCode()
        {
            return A.GetHashCode() ^ B.GetHashCode();
        }

        public string ConvertToBinary()
        {
            string binaryA = Convert.ToString(this.A,2);
            if (binaryA.Length < 4)
            {
                binaryA = binaryA.PadLeft(4, '0');
            }
            string binaryB = Convert.ToString(this.B,2);
            if (binaryB.Length < 4)
            {
                binaryB = binaryB.PadLeft(4, '0');
            }
            return $"{binaryA}{binaryB}";
        }

        public void UpdateBinaryRepresentation(string binaryString)
        {
            this.BinaryRepresentationOfChromosome = binaryString;
            ConvertFromBinary();
        }
        private void ConvertFromBinary()
        {
            string partA = BinaryRepresentationOfChromosome.Substring(0, 4);
            string partB = BinaryRepresentationOfChromosome.Substring(4);
            this.A = Convert.ToInt32(partA, 2);
            this.B = Convert.ToInt32(partB, 2);
        }

        public void Mutate(in int position)
        {
            string newString="";
            for (int i = 0; i < BinaryRepresentationOfChromosome.Length; i++)
            {
                char currentChar = BinaryRepresentationOfChromosome[i];
                if(i==position)
                {
                    var condition = currentChar == '0';
                    currentChar = condition ? '1' : '0';
                }
                newString += currentChar;
            } 
            UpdateBinaryRepresentation(newString);
        }
    }
}