using System;
using System.Diagnostics;
using System.Text.Json;

namespace Task1;

public static class Program
{
    public static void Main()
    {
        var f = new F(1, 2, 3, 4, 5, new[] { 10, 20, 30, 40, 50 });
        var dto = new FPublic { I1 = 1, I2 = 2, I3 = 3, I4 = 4, I5 = 5, Mas = new[] { 10, 20, 30, 40, 50 } };

        string csvSample = CsvSerializer.Serialize(f);
        string jsonSample = JsonSerializer.Serialize(dto);

        Console.WriteLine("CSV sample:");
        Console.WriteLine(csvSample);
        Console.WriteLine("JSON sample:");
        Console.WriteLine(jsonSample);
        Console.WriteLine();

        int n = 200_000;

        Warmup(f, csvSample, dto, jsonSample);

        var tCsvSer = Measure(() =>
        {
            string last = "";
            for (int i = 0; i < n; i++)
                last = CsvSerializer.Serialize(f);
            Sink(last);
        });

        var tJsonSer = Measure(() =>
        {
            string last = "";
            for (int i = 0; i < n; i++)
                last = JsonSerializer.Serialize(dto);
            Sink(last);
        });

        var tCsvDes = Measure(() =>
        {
            F last = null!;
            for (int i = 0; i < n; i++)
                last = CsvSerializer.Deserialize(csvSample);
            Sink(last.ToString());
        });

        var tJsonDes = Measure(() =>
        {
            FPublic last = null!;
            for (int i = 0; i < n; i++)
                last = JsonSerializer.Deserialize<FPublic>(jsonSample)!;
            Sink(last.I1);
        });

        int w = 2_000;
        var tWriteLine = Measure(() =>
        {
            for (int i = 0; i < w; i++)
                Console.WriteLine(csvSample);
        });

        Console.WriteLine();
        Console.WriteLine($"Iterations: {n}");
        Console.WriteLine($"CSV serialize (cached reflection): {tCsvSer.TotalMilliseconds:F3} ms");
        Console.WriteLine($"JSON serialize (System.Text.Json): {tJsonSer.TotalMilliseconds:F3} ms");
        Console.WriteLine($"CSV deserialize: {tCsvDes.TotalMilliseconds:F3} ms");
        Console.WriteLine($"JSON deserialize: {tJsonDes.TotalMilliseconds:F3} ms");
        Console.WriteLine($"Console.WriteLine x{w}: {tWriteLine.TotalMilliseconds:F3} ms");
    }

    private static void Warmup(F f, string csv, FPublic dto, string json)
    {
        for (int i = 0; i < 2000; i++)
        {
            _ = CsvSerializer.Serialize(f);
            _ = CsvSerializer.Deserialize(csv);
            _ = JsonSerializer.Serialize(dto);
            _ = JsonSerializer.Deserialize<FPublic>(json);
        }
    }

    private static TimeSpan Measure(Action action)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        var sw = Stopwatch.StartNew();
        action();
        sw.Stop();
        return sw.Elapsed;
    }

    private static void Sink(object _)
    {
    }
}
