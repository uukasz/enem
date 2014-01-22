using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Point
    {
        #region CONSTRUCTORS

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point() : this(0.0, 0.0)
        { }

        #endregion

        #region PROPERTIES

        private double _x;

        public double X
        {
            get { return _x; }
            private set { _x = value; }
        }

        private double _y;

        public double Y
        {
            get { return _y; }
            private set { _y = value; }
        } 

        #endregion

        #region PUBLIC METHODS

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }

        public override bool Equals(object obj)
        {
            Point point = obj as Point;
            if (point == null) return false;
            return X == point.X && Y == point.Y;
        }

        #endregion

    }
}
