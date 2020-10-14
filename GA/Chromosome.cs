using System;
using System.Security;

namespace GA
{
    public class Chromosome
    {
        public short A { get; private set; }
        public short B { get; private set; }
        public double Fitness { get; set; }
        public double ProbFitting { get; set; }
        public string BinaryA { get; private set; }
        public string BinaryB { get; private set; }

        public Chromosome(Chromosome chromosomeToCopy)
        {
            A = chromosomeToCopy.A;
            B = chromosomeToCopy.B;
            Fitness = chromosomeToCopy.Fitness;
            ProbFitting = chromosomeToCopy.ProbFitting;
            BinaryA = chromosomeToCopy.BinaryA;
            BinaryB = chromosomeToCopy.BinaryB;

        }
        public Chromosome(short A, short B)
        {
            this.A =A;
            this.B = B;
            BinaryA = Convert.ToString(A, 2);
            BinaryB = Convert.ToString(B, 2);
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
            return $"Chromosome {A}, {B}: {BinaryA}, {BinaryB}";
        }

        public override int GetHashCode()
        {
            return A.GetHashCode() ^ B.GetHashCode();
        }

        public void UpdateBinaryRepresentation(string binaryString)
        {
            var half = binaryString.Length / 2;
            this.BinaryA = binaryString.Substring(0, half);
            this.BinaryB = binaryString.Substring(half);
            ConvertFromBinary();
        }
        private void ConvertFromBinary()
        {
            this.A = Convert.ToInt16(BinaryA, 2);
            this.B = Convert.ToInt16(BinaryB, 2);
        }

        public void Mutate(int position, int partToMutate, int chromosomeLength)
        {
            string newString="";
            string toMutate;
            if (partToMutate==0)
            {
                toMutate = BinaryA;
            }
            else
            {
                toMutate = BinaryB;
                position = position - chromosomeLength + 1;
            }
            for (int i = 0; i < toMutate.Length; i++)
            {
                char currentChar = toMutate[i];
                if(i==position)
                {
                    var condition = currentChar == '0';
                    currentChar = condition ? '1' : '0';
                }
                newString += currentChar;
            }

            if (partToMutate == 0)
                BinaryA = newString;
            else
            {
                BinaryB = newString;
            }
            ConvertFromBinary();
        }
    }
}