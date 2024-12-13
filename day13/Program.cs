using System.Text.Json;

var parse = new Parser();

var machines = parse.ReadAll().ToArray();

var json = JsonSerializer.Serialize(machines, new JsonSerializerOptions { WriteIndented = true });
Console.WriteLine(json);
