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

    public override bool Equals(object o)
    {
        if (o == null || o is not Triangle)
            return false;

        Triangle t = (Triangle)o;

        return a == t.a && b == t.b && c == t.b; 
    }
}