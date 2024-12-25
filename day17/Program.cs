Run("test.txt");
Run("input.txt");

void Run(string file)
{
    var (ops, a, b, c) = Parse(file);
    var output = RunProgram(ops, a, b, c).ToArray();
    Console.WriteLine($"Program {file} output: {string.Join(",", output)}\n");
}

IEnumerable<long> RunProgram(long[] ops, long a, long b, long c)
{
    Console.WriteLine($"Running program with a={a}, b={b}, c={c}");
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

(long[] ops, long a, long b, long c) Parse(string file)
{
    var lines = File.ReadAllLines(file);
    var a = long.Parse(lines[0].Split(": ")[1]);
    var b = long.Parse(lines[1].Split(": ")[1]);
    var c = long.Parse(lines[2].Split(": ")[1]);
    var code = lines[4].Split(": ")[1].Split(',').Select(long.Parse).ToArray();
    return (code, a, b, c);
}
