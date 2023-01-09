public struct Triangle
{
    public int a;
    public int b;
    public int c;

    public Triangle(int a, int b, int c)
    {
        this.a = a;
        this.b = b;
        this.c = c;
    }

    public int[] ToArray()
    {
        return new int[] { a, b, c };
    }

    public override bool Equals(object o)
    {
        if (o == null || o is not Triangle)
            return false;

        Triangle t = (Triangle)o;

        return a == t.a && b == t.b && c == t.b; 
    }

    public override string ToString()
    {
        return $"a: {a}, b: {b}, c: {c}";
    }
}