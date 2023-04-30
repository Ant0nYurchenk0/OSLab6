using System.Diagnostics;

namespace task2
{
  internal class Program
  {
    static object padlock = new object();
    static ulong a = 0;

    static void Main(string[] args)
    {

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      var task1 = new Task(changeValueLock);
      var task2 = new Task(changeValueLock);
      task1.Start();
      task2.Start();
      Task.WaitAll(task1, task2);

      stopwatch.Stop();
      Console.WriteLine(a);
      Console.WriteLine(stopwatch.ElapsedMilliseconds);

    }

    private static void changeValueLock()
    {
      for (int i = 0; i < 1_000_000_000; i++)
      {
        lock (padlock)
        {
          a++;
        }
      }
    }

    private static void changeValueSync()
    {
      for (int i = 0; i < 1000; i++)
      {
        var b = a;
        Thread.Sleep(1);
        a =b + 1;
      }

    }

    private static void changeValue()
    {
      for (int i = 0; i < 1_000_000; i++) a++;
    }

  }
}