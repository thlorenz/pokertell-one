/*
 * User: Thorsten Lorenz
 * Date: 6/22/2009
 * 
*/
using System;
using System.Drawing;

namespace Tools.GenericUtilities
{
    [Serializable]
    public class RelativeLocation : ICloneable
    {
        public double XFactor { get; set; }
        public double YFactor { get; set; }
	    
        public RelativeLocation() : this(0.5, 0.5) { }
	    
        public RelativeLocation(Point location, Size size)
        {
            this.XFactor = (double) location.X / (double) size.Width;
            this.YFactor = (double) location.Y / (double) size.Height;
        }
	    
        public RelativeLocation(double xFactor, double yFactor)
        {
            this.XFactor = xFactor;
            this.YFactor = yFactor;
        }
	    
        public Point GetLocationFor(Size size)
        {
            int x = (int) (this.XFactor * (double) size.Width);
            int y = (int) (this.YFactor * (double) size.Height);
	        
            return new Point(x, y);
        }
	    
        public object Clone()
        {
            return this.MemberwiseClone();
        }
	    
        public override string ToString()
        {
            return string.Format("{{XFactor={0} YFactor={1}}}", XFactor, YFactor);
        }
	    
        public override int GetHashCode()
        {
            return this.XFactor.GetHashCode() ^ this.YFactor.GetHashCode();
        }
	    
        public override bool Equals(object obj)
        {
            if (obj == null) {
                return false;
            }
            else {
                return this.GetHashCode().Equals(obj.GetHashCode());
            }
        }
    }
}