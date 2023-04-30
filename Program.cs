using System.Diagnostics;

namespace OSLab6
{
  internal class Program
  {
    static int[,] matrix1 = new int[1000, 1000];
    static int[,] matrix2 = new int[1000, 1000];
    static void Main()
    {
      initMatrix(ref matrix1);
      initMatrix(ref matrix2);

      if (matrix1.GetLength(0) != matrix2.GetLength(1)) throw new ArgumentException();

      var stopwatch = new Stopwatch();
      stopwatch.Start();
      var res1 = multiplyMatrice(matrix1, matrix2);
      stopwatch.Stop();
      Console.WriteLine(stopwatch.ElapsedMilliseconds);

      stopwatch.Reset();
      stopwatch.Start();
      var res2 = multiplyMatriceThread(matrix1, matrix2);
      stopwatch.Stop();
      Console.WriteLine(stopwatch.ElapsedMilliseconds);

      for (int i = 0; i < matrix1.GetLength(0); i++)
        for (int j = 0; j < matrix2.GetLength(0); j++)
          if (matrix1[i,j] != matrix2[i, j]) throw new Exception();
    }

    private static int[,] multiplyMatrice(int[,] matrix1, int[,] matrix2)
    {
      var matrix = new int[matrix1.GetLength(0), matrix1.GetLength(0)];

      for (int i = 0; i < matrix.GetLength(0); i++)
      {
        for (int j = 0; j < matrix.GetLength(0); j++)
        {
          var row = GetRow(matrix1, i);
          var col = GetColumn(matrix2, j);
          matrix[i, j] = multiplyVectors(row, col);
        }
      }
      return matrix;
    }

    private static int[,] multiplyMatriceThread(int[,] matrix1, int[,] matrix2)
    {
      var matrix = new int[matrix1.GetLength(0), matrix1.GetLength(0)];
      var tasks = new Task[matrix.GetLength(0)];

      for (int i = 0; i < matrix.GetLength(0); i++)
      {
        var counter = i;
        var length = matrix.GetLength(0);
        var task = new Task(() =>
        {
          for (int j = 0; j < length; j++)
          {
            var row = GetRow(matrix1, counter);
            var col = GetColumn(matrix2, j);
            matrix[counter, j] = multiplyVectors(row, col);
          }
        });
        tasks[i] = task;
        task.Start();
      }
      Task.WaitAll(tasks);
      return matrix;
    }

    private static void initMatrix(ref int[,] matrix)
    {
      var random = new Random();
      for (int i = 0; i < matrix.GetLength(0); i++)
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
          matrix[i, j] = i * matrix.GetLength(1) + j;
        }
    }

    private static int multiplyVectors(int[] vector1, int[] vector2)
    {
      if (vector1.Length != vector2.Length) throw new ArgumentException();
      var result = 0;
      for (int i = 0; i < vector1.Length; i++)
      {
        result += vector1[i] * vector2[i];
      }
      //Console.WriteLine(Task.CurrentId);
      //Console.WriteLine(result);
      return result;
    }
    private static int[] GetColumn(int[,] matrix, int columnNumber)
    {
      return Enumerable.Range(0, matrix.GetLength(0))
              .Select(x => matrix[x, columnNumber])
              .ToArray();
    }

    private static int[] GetRow(int[,] matrix, int rowNumber)
    {
      return Enumerable.Range(0, matrix.GetLength(1))
              .Select(x => matrix[rowNumber, x])
              .ToArray();
    }
  }
}