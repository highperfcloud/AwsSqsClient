using BenchmarkDotNet.Running;

namespace Aws.Sqs.Core.Benchmarks
{
    class Program
    {
        static void Main(string[] args) => _ = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
