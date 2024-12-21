Run("test.txt");
Run("input.txt");
Run("fixed.txt");

void Run(string file)
{
    var c = Parse(file);

    while (c.Pos < c.Code.Length)
    {
        Exec(c);
    }

    Console.WriteLine($"Program {file} output: " + string.Join(",", c.Outputs));
}

void Exec(ComputerState c)
{
    var op = c.Code[c.Pos];
    var par = c.Code[c.Pos + 1];
    c.Pos += 2;
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

    // operations
    void Adv(long op) => c.A /= (long)Math.Pow(2, ComboVal(op));
    void Bxl(long op) => c.B ^= op;
    void Bst(long op) => c.B = ComboVal(op) % 8;
    void Jnz(long op) => c.Pos = c.A != 0 ? (int)op : c.Pos;
    void Bxc(long _) => c.B ^= c.C;
    void Out(long op)
    {
        var v = ComboVal(op) % 8;
        c.Outputs.Add(v);
    } 

    void Bdv(long op) => c.B = c.A / (long)Math.Pow(2, ComboVal(op));
    void Cdv(long op) => c.C = c.A / (long)Math.Pow(2, ComboVal(op));
    
    long ComboVal(long op)
    {
        return op switch
        {
            0 => 0,
            1 => 1,
            2 => 2,
            3 => 3,
            4 => c.A,
            5 => c.B,
            6 => c.C,
            _ => throw new Exception("Invalid operand")
        };
    }
}

ComputerState Parse(string file)
{
    var lines = File.ReadAllLines(file);
    var a = long.Parse(lines[0].Split(": ")[1]);
    var b = long.Parse(lines[1].Split(": ")[1]);
    var c = long.Parse(lines[2].Split(": ")[1]);
    var code = lines[4].Split(": ")[1].Split(',').Select(long.Parse).ToArray();
    return new ComputerState(a, b, c, code);
}

class ComputerState(long a, long b, long c, long[] code)
{
    public int Pos { get; set; } = 0;
    public long A { get; set; } = a;
    public long B { get; set; } = b;
    public long C { get; set; } = c;
    public long[] Code { get; } = code;
    public List<long> Outputs { get; } = [];
}
