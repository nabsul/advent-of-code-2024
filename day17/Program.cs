Run("test.txt");
Decompile("test.txt");
Run("test2.txt");
Decompile("test2.txt");
Run("input.txt");
Decompile("input.txt");

void Run(string file)
{
    var (ops, a, b, c) = Parse(file);
    var output = RunProgram(ops, a, b, c).ToArray();
    Console.WriteLine($"Program {file} output: {string.Join(",", output)}\n");
}

IEnumerable<long> RunProgram(long[] ops, long a, long b, long c)
{
    Console.WriteLine($"Running program with a={a} ({Convert.ToString(a, 2)}), b={b} ({Convert.ToString(b, 2)}), c={c} ({Convert.ToString(c, 2)})");
    Console.WriteLine($"Program: {string.Join(",", ops)}");
    var pos = 0;
    var comboVals = new long[] { 0, 1, 2, 3, a, b, c };

    while (pos < ops.Length)
    {
        var op = ops[pos];
        var par = ops[pos + 1];
        comboVals[4] = a;
        comboVals[5] = b;
        comboVals[6] = c;
        pos += 2;

        switch (op)
        {
            case 0: a /= (long)Math.Pow(2, comboVals[par]); break;
            case 1: b ^= par; break;
            case 2: b = comboVals[par] % 8; break;
            case 3: pos = a != 0 ? (int)par : pos; break;
            case 4: b ^= c; break;
            case 5: yield return comboVals[par] % 8; break;
            case 6: b = a / (long)Math.Pow(2, comboVals[par]); break;
            case 7: c = a / (long)Math.Pow(2, comboVals[par]); break;
            default: throw new Exception("Invalid opcode");
        }
    }
}

void Decompile(string file)
{
    var (ops, a, b, c) = Parse(file);
    Console.WriteLine($"Decompiling program {file}");
    Console.WriteLine($"Initial values: a={a}, b={b}, c={c}");
    var pos = 0;
    var combos = new string[] { "0", "1", "2", "3", "a", "b", "c" };
    while (pos < ops.Length)
    {
        var line = pos.ToString("D2");
        var op = ops[pos];
        var par = ops[pos + 1];
        pos += 2;

        Console.Write($"{line}: ");
        switch (op)
        {
            case 0: Console.WriteLine($"a /= 2^{combos[par]}"); break;
            case 1: Console.WriteLine($"b ^= {par}"); break;
            case 2: Console.WriteLine($"b = {combos[par]} & 111"); break;
            case 3: Console.WriteLine($"if (a != 0) goto {par}"); break;
            case 4: Console.WriteLine($"b ^= c"); break;
            case 5: Console.WriteLine($"output: {combos[par]} & 111"); break;
            case 6: Console.WriteLine($"b = a / 2^{combos[par]}"); break;
            case 7: Console.WriteLine($"c = a / 2^{combos[par]}"); break;
            default: throw new Exception("Invalid opcode");
        }
    }
    Console.WriteLine();
}

(long[] ops, long a, long b, long c) Parse(string file)
{
    var lines = File.ReadAllLines(file);
    var a = long.Parse(lines[0].Split(": ")[1]);
    var b = long.Parse(lines[1].Split(": ")[1]);
    var c = long.Parse(lines[2].Split(": ")[1]);
    var code = lines[4].Split(": ")[1].Split(',').Select(long.Parse).ToArray();
    return (code, a, b, c);
}

