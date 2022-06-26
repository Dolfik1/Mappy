using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Validators;
using System.Linq;
using BenchmarkDotNet.Diagnosers;

namespace Mappy.Benchmark
{
    public class AllowNonOptimized : ManualConfig
    {
        public AllowNonOptimized()
        {
            AddDiagnoser(MemoryDiagnoser.Default);
            AddValidator(JitOptimizationsValidator.DontFailOnError); // ALLOW NON-OPTIMIZED DLLS

            AddLogger(DefaultConfig.Instance.GetLoggers().ToArray()); // manual config has no loggers by default
            AddExporter(DefaultConfig.Instance.GetExporters().ToArray()); // manual config has no exporters by default
            AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray()); // manual config has no columns by default
        }
    }
}
