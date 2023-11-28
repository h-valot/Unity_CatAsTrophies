public class Point
{
    public int col, row;

    public Point(int col, int row)
    {
        this.col = col;
        this.row = row;
    }
    
    public bool Equals(Point other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return col == other.col && row == other.row;
    }
    
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Point)obj);
    }
}