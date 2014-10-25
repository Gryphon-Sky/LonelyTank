using System;
using System.Collections.Generic;

[Serializable]
public struct Position
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region exposed
    
    public int X;
    public int Y;

    #endregion

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    #region creation

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region utility
    
    public static bool operator==(Position pos1, Position pos2)
    {
        return pos1.Equals(pos2);
    }

    public static bool operator!=(Position pos1, Position pos2)
    {
        return !pos1.Equals(pos2);
    }
    
    public override string ToString()
    {
        return string.Format("({0}, {1})", X, Y);
    }

    public override bool Equals(object other)
    {
        if(!(other is Position))
        {
            return false;
        }
        Position pos = (Position)other;
        return X.Equals(pos.X) && Y.Equals(pos.Y);
    }

    public override int GetHashCode()
    {
        return 2221 * Y + X;
    }

    public class Comparer: IComparer<Position>
    {
        public int Compare(Position pos1, Position pos2)
        {
            int yCompare = pos1.Y.CompareTo(pos2.Y);
            if(yCompare != 0)
            {
                return yCompare;
            }

            return pos1.X.CompareTo(pos2.X);
        }
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
