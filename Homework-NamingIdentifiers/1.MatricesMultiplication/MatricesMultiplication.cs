using System;

namespace _1.MatricesMultiplication
{
    public class MatrcicesMultiplication
    {
        static void Main()
        {
            var firstMatrix = new double[,] { { 1, 3 }, { 5, 7 } };
            var secondMatrix = new double[,] { { 4, 2 }, { 1, 5 } };
            var resultMatrix = MultiplyTwoMatrices(firstMatrix, secondMatrix);

            for (int i = 0; i < resultMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < resultMatrix.GetLength(1); j++)
                {
                    Console.Write(resultMatrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        static double[,] MultiplyTwoMatrices(double[,] matrixA, double[,] matrixB)
        {
            if (matrixA.GetLength(1) != matrixB.GetLength(0))
            {
                throw new Exception("Error!");
            }

            var length = matrixA.GetLength(1);
            var resultMatrix = new double[matrixA.GetLength(0), matrixB.GetLength(1)];
            for (int i = 0; i < resultMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < resultMatrix.GetLength(1); j++)
                {
                    for (int k = 0; k < length; k++)
                    {
                        resultMatrix[i, j] += matrixA[i, k]*matrixB[k, j];
                    }
                }
            }

            return resultMatrix;
        }
    }
}