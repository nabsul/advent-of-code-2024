using System.Net.Mail;

public class Computer
{
    long a;
    long b;
    long c;

    List<long> code = [];
    List<long> outputs = [];
    long pos = 0;

    public static void Run(string file)
    {
        var c = new Computer(file);
        c.Run();
        Console.WriteLine($"Program {file} output: " + string.Join(",", c.outputs));
    }

    public static void FixRegister(string file)
    {
        var a = 0;
        var incr = 1;
        var targIdx = 0;
        var maxIdx = new Computer("input.txt").code.Count;

        while (targIdx < maxIdx)
        {
            var c = new Computer("input.txt")
            {
                a = a
            };

            bool failed = false;
            try
            {
                c.Run();
            }
            catch(Exception)
            {
                failed = true;
            }

            if (failed || targIdx >= c.outputs.Count || c.outputs[targIdx] != c.code[targIdx])
            {
                a += incr;
            }

            if (c.outputs.SequenceEqual(c.code))
            {
                Console.WriteLine("Program fixed");
                break;
            }

            targIdx++;
            incr *= 3;
            Console.WriteLine($"Incrementing ({targIdx} / {c.code.Count})");
        }

        Console.WriteLine($"Register A fixed to {a}");
    }

    public Computer(string fileName)
    {
        var lines = File.ReadAllLines(fileName);
        a = long.Parse(lines[0].Split(": ")[1]);
        b = long.Parse(lines[1].Split(": ")[1]);
        c = long.Parse(lines[2].Split(": ")[1]);
        code = lines[4].Split(": ")[1].Split(',').Select(long.Parse).ToList();
    }

    void Run()
    {
        pos = 0;
        while (pos < code.Count)
        {
            Exec();
        }
    }

    Dictionary<(long A, long B, long C), (long Out, long A, long B, long C)> memo = new();

    void Exec()
    {
        var op = code[(int)pos];
        var par = code[(int)pos + 1];
        pos += 2;
        Action<long> func = op switch
        {
            0 => Adv,
            1 => Bxl,
            2 => Bst,
            3 => Jnz,
            4 => Bxc,
            5 => Out,
            6 => Bdv,
            7 => Cdv,
            _ => throw new Exception("Invalid opcode")
        };

        func(par);
    }


    void Adv(long op) => a /= (long)Math.Pow(2, ComboVal(op));
    void Bxl(long op) => b ^= op;
    void Bst(long op) => b = ComboVal(op) % 8;
    void Jnz(long op) => pos = a != 0 ? op : pos;
    void Bxc(long _) => b ^= c;
    void Out(long op)
    {
        var v = ComboVal(op) % 8;
        outputs.Add(v);
    } 

    void Bdv(long op) => b = a / (long)Math.Pow(2, ComboVal(op));
    void Cdv(long op) => c = a / (long)Math.Pow(2, ComboVal(op));
    
    long ComboVal(long op)
    {
        return op switch
        {
            0 => 0,
            1 => 1,
            2 => 2,
            3 => 3,
            4 => a,
            5 => b,
            6 => c,
            _ => throw new Exception("Invalid operand")
        };
    }

    public static void PrintProgram(string file)
    {
        new Computer(file).Decompile();
    }

    void Decompile()
    {
        var ops = new List<string>();
        for (int i = 0; i < code.Count; i += 2)
        {
            var op = code[i];
            var par = code[i + 1];
            var func = op switch
            {
                0 => "Adv",
                1 => "Bxl",
                2 => "Bst",
                3 => "Jnz",
                4 => "Bxc",
                5 => "Out",
                6 => "Bdv",
                7 => "Cdv",
                _ => throw new Exception("Invalid opcode")
            };
            ops.Add($"{func}({par})");
        }
        Console.WriteLine(string.Join("\n", ops));
    }
}
 