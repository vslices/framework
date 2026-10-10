using BenchmarkDotNet.Running;
using SampleWorkflow.Benchmarks;

BenchmarkRunner.Run<UpdateTodoPostgreSqlBenchmark>();
