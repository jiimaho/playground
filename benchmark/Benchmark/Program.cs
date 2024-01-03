using Benchmark;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

// BenchmarkRunner.Run<Looping>(new DebugInProcessConfig());
BenchmarkRunner.Run<JsonBenchmark>();
