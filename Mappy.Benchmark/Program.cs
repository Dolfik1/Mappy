using BenchmarkDotNet.Running;

namespace Mappy.Benchmark
{
    static class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SimpleMap>(new AllowNonOptimized());
            //BenchmarkRunner.Run<ComplexMap>(new AllowNonOptimized());
        }
    }
}
