Solve("test.txt");
Solve("input.txt");

FixWires("test2.txt");

void Solve(string file)
{
    var (values, ops) = Parse(file);
    var lookup = ops.ToDictionary(o => o.Output);
    PopulateValues(values, lookup);
    var outputs = values.Where(v => v.Key.StartsWith("z")).OrderBy(v => v.Key).Reverse().ToList();
    long result = 0;
    foreach (var output in outputs)
    {
        //Console.WriteLine($"{output.Key}: {output.Value}");
        result <<= 1;
        result |= (uint)output.Value;
    }
    Console.WriteLine($"Result of {file}: {result}");
}

void PopulateValues(Dictionary<string, int> values, Dictionary<string, Operation> lookup)
{
    foreach (var op in lookup.Values)
    {
        if (values.ContainsKey(op.Output)) continue;
        Resolve(values, lookup, op.Output);
    }
}

int Resolve(Dictionary<string, int> values, Dictionary<string, Operation> ops, string val)
{
    if (values.TryGetValue(val, out var value)) return value;
    var op = ops[val];
    var input1 = Resolve(values, ops, op.Input1);
    var input2 = Resolve(values, ops, op.Input2);
    var result = Calculate(op.Name, input1, input2);
    values[val] = result;
    return result;
}

int Calculate(string op, int input1, int input2)
{
    return op switch
    {
        "AND" => input1 & input2,
        "OR" => input1 | input2,
        "XOR" => input1 ^ input2,
        _ => throw new Exception("Unknown operation"),
    };
}

(Dictionary<string, int>, List<Operation>) Parse(string file)
{
    var values = new Dictionary<string, int>();
    var ops = new List<Operation>();
    foreach (var line in File.ReadAllLines(file))
    {
        if (line.Contains(':'))
        {
            var parts = line.Split(": ");
            values[parts[0]] = int.Parse(parts[1]);
        }
        else if (line.Contains("->"))
        {
            var parts = line.Split(' ');
            ops.Add(new Operation(parts[1], parts[0], parts[2], parts[4]));
        }
    }
    return (values, ops);
}

(string, string)[] FixWires(string file)
{
    var (_, ops) = Parse(file);
    var lookup = ops.ToDictionary(o => o.Output);
    var result = new List<(string, string)>();
    var inputs = GenerateRandomInputs().Take(100);
    var badBits = FindBadBits(inputs, ops);
    Console.WriteLine("Bad bits: {0}", Convert.ToString(badBits, 2).PadLeft(64, '0'));
    return [.. result];
}

IEnumerable<(long, long)> GenerateRandomInputs()
{
    var random = new Random();
    while (true)
    {
        yield return (random.Next(), random.Next());
    }
}

long FindBadBits(IEnumerable<(long, long)> inputs, List<Operation> ops)
{
    long res = 0;
    var lookup = ops.ToDictionary(o => o.Output);
    foreach (var (input1, input2) in inputs)
    {
        var expected = input1 + input2;
        var actual = Compute(lookup, input1, input2);
        res |= expected ^ actual;
        Console.WriteLine("Input1: {0}, Input2: {1}, Expected: {2}, Actual: {3}", input1, input2, expected, actual);
    }
    return res;
}

long Compute(Dictionary<string, Operation> lookup, long input1, long input2)
{
    var values = ExpandInputs('x', input1).Concat(ExpandInputs('y', input2)).ToDictionary(v => v.Item1, v => v.Item2);
    //Console.WriteLine("Values: {0}", string.Join(", ", values.Select(v => $"{v.Key}: {v.Value}")));
    PopulateValues(values, lookup);
    var outputs = values.Where(v => v.Key.StartsWith("z")).OrderBy(v => v.Key).Reverse().ToList();
    long result = 0;
    foreach (var output in outputs)
    {
        result <<= 1;
        result |= (uint)output.Value;
    }
    return result;
}

IEnumerable<(string, int)> ExpandInputs(char prefix, long value)
{
    for (int i = 0; i < 64; i++)
    {
        yield return ($"{prefix}{i:D2}", (int)(value & 1));
        value >>= 1;
    }
}

record Operation(string Name, string Input1, string Input2, string Output);
