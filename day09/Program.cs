
using System.Text.Json;
using System.Xml;

var input = ReadFile();
if (input.Length % 2 == 0) input = input.Take(input.Length - 1).ToArray();

Console.WriteLine($"Part1 Checksum: {ChecksumPart1(input)}");

input = ReadFile();
if (input.Length % 2 == 0) input = input.Take(input.Length - 1).ToArray();
Console.WriteLine($"Part2 Checksum: {ChecksumPart2(input)}");

int[] ReadFile()
{
    return File.ReadAllText("data.txt")
        .ToCharArray()
        .Select(x => int.Parse(x.ToString()))
        .ToArray();
}

long ChecksumPart1(int[] input)
{
    return Compact(input)
    .Select((v, i) => (long)v * i)
    .Sum();
}

long ChecksumPart2(int[] input)
{
    long sum = 0;

    var data = new LinkedList<Item>(ReadItems(input));

    var curr = data.Last;
    while (curr != null)
    {
        var search = data.First;
        while (search != null && search != curr && curr.Value.Size > search.Next!.Value.Position - search.Value.Position - search.Value.Size)
        {
            search = search.Next;
        }

        if (search != null && search != curr)
        {
            var next = curr.Previous;
            data.Remove(curr);
            data.AddAfter(search, curr);
            curr.Value.Position = search.Value.Position + search.Value.Size;
            curr = next;
            continue;
        }

        curr = curr.Previous;
    }

    foreach (var node in data)
    {
        for (int i = 0; i < node.Size; i++)
        {
            sum += node.Id * (node.Position + i);
        }
    }

    return sum;
}

void CompactPart2(LinkedList<Item> items, LinkedListNode<Item> item)
{
    if (item.Next == null) return;

    var freeSpace = item.Next.Value.Position - item.Value.Position - item.Value.Size;

    var end = items.Last;
    while (end != null && end != item.Next)
    {
        if (end.Value.Size > freeSpace)
        {
            end = end.Previous;
            continue;
        }

        items.Remove(end);
        end.Value.Position = item.Value.Position + item.Value.Size;
        items.AddAfter(item, end);
        break;
    }
}

IEnumerable<Item> ReadItems(int[] input)
{
    int idx = 0;
    int pos = 0;
    
    for (int i = 0; i < input.Length; i++)
    {
        if (i % 2 == 0)
        {
            yield return new Item { Id = idx++, Position = pos, Size = input[i] };
        }
        pos += input[i];
    }
}

IEnumerable<int> Compact(int[] input)
{
    var start = Expand(input).GetEnumerator();
    var end = ExpandReversed(input).GetEnumerator();
    var remaining = input.Sum();

    if (!start.MoveNext() || !end.MoveNext()) throw new InvalidOperationException("Invalid input");

    while (remaining > 0)
    {
        if (start.Current >= 0)
        {
            yield return start.Current;
            remaining--;
            start.MoveNext();
            continue;
        }

        if (end.Current < 0)
        {
            end.MoveNext();
            remaining--;
            continue;
        }

        yield return end.Current;
        end.MoveNext();
        start.MoveNext();
        remaining -= 2;
    }
}

IEnumerable<int> Expand(int[] input)
{
    return input.SelectMany((v, i) => Enumerable.Repeat(i % 2 == 0 ? i / 2 : -1, v));
}

IEnumerable<int> ExpandReversed(int[] input)
{
    return input.Select((v, i) => new { v, i })
        .Reverse()
        .SelectMany(x => Enumerable.Repeat(x.i % 2 == 0 ? x.i / 2 : -1, x.v));
}

class Item
{
    public int Id { get; set; }
    public int Position { get; set; }
    public int Size {get;set;}
    public bool Moved {get;set;}
}
