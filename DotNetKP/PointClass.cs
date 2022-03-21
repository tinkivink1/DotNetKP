using System.Drawing;
class PointClass
{
    int x;
    int y;
    int number; 
    public int X
    {
        get
        {
            return x;
        }
        set
        {
            x = value;
        }
    }
    public int Y
    {
        get
        {
            return y;
        }
        set
        {
            y = value;
        }
    }
    public PointClass(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
    }
    public PointClass(int X, int Y, int number)
    {
        this.X = X;
        this.Y = Y;
        this.number = number;
    }
    public PointClass(Point point)
    {
        X = point.X;
        Y = point.Y;
    }
    public Point toStruct()
    {
        return new Point(X, Y);
    }
    public override string ToString()
    {
        return "{" + X + ":" + Y + "}";
    }
    public bool Equals(Point point)
    {
        return (X == point.X && Y == point.Y);
    }
    public int getNumber
    {
        get
        {
            return number;
        }
    }
}
