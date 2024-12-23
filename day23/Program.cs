var file = "test.txt";

var lines = File.ReadAllLines(file)
    .Select(l => l.Split('-'));

Dictionary<string, HashSet<string>> dict = [];

foreach (var line in lines)
{
    void Update(string key, string value)
    {
        if (!dict.TryGetValue(key, out var hash))
        {
            dict[key] = hash = [];
        }

        dict[key].Add(value);
    }
    Update(line[0], line[1]);
    Update(line[1], line[0]);
}

