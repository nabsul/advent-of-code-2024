
var debug = false;

Solve("test.txt", 2000);
Solve("input.txt", 2000);
GetBestPrice("test.txt", 2000);
GetBestPrice("test2.txt", 2000);
GetBestPrice("input.txt", 2000);

void Solve(string file, int iterations)
{
    var nums = Parse(file).ToArray();
    var res = nums.Select(n => GetNthSecretNumber(n, iterations)).ToArray();

    Console.WriteLine($"Results {file}:");
    if (debug)
    {
        foreach (var (k, v) in nums.Zip(res))
        {
            Console.WriteLine($"{k}: {v}");
        }
    }
    Console.WriteLine($"Sum: {res.Sum()}");
    Console.WriteLine();
}

void GetBestPrice(string file, int iterations)
{
    var res = new Dictionary<(int, int, int, int), int>();
    var nums = Parse(file).ToArray();
    foreach (var n in nums)
    {
        foreach (var (k, v) in Prices(n, iterations))
        {
            if (!res.TryGetValue(k, out var prev))
            {
                prev = 0;
            }
            res[k] = v + prev;
        }
    }

    var (key, val) = res.MaxBy(p => p.Value);
    Console.WriteLine($"Best price {file}: {key} -> {val}");
}

Dictionary<(int, int, int, int), int> Prices(long num, int iterations)
{
    var res = new Dictionary<(int, int, int, int), int>();

    foreach(var (key, val) in GetPrices(num).Take(iterations).Skip(3))
    {
        if (!res.TryGetValue(key, out var prev))
        {
            res.Add(key, val);
        }
    }

    return res;
}

IEnumerable<((int, int, int, int), int)> GetPrices(long num)
{
    var key = (0, 0, 0, 0);
    var prev = (int)(num % 10);
    foreach (var s in GetSecretNumbers(num).Select(n => (int)(n % 10)))
    {
        var diff = s - prev;
        var (a, b, c, d) = key;
        key = (b, c, d, diff);
        yield return (key, s);
        prev = s;
    }
}

long GetNthSecretNumber(long num, int n)
{
    return GetSecretNumbers(num).Skip(n-1).First();
}

IEnumerable<long> GetSecretNumbers(long num)
{
    while (true)
    {
        num = NextSecretNumber(num);
        yield return num;
    }
}

long NextSecretNumber(long num)
{
    var step1 = Prune(Mix(num, num * 64));
    var step2 = Prune(Mix(step1, step1 / 32));
    return Prune(Mix(step2, step2 * 2048));
}

long Mix(long a, long b) => a ^ b;
long Prune(long a) => a % 16777216;

IEnumerable<long> Parse(string file)
{
    using (var reader = new StreamReader(file))
    {
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return long.Parse(line);
        }
    }
}