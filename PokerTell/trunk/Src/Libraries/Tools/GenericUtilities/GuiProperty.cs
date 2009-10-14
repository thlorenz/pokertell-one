/*
 * User: Thorsten Lorenz
 * Date: 6/22/2009
 * 
*/
using System;
using System.Drawing;
using System.Xml.Serialization;
using Tools.Extensions;

namespace Tools.GenericUtilities
{
    /// <summary>
    /// Allows any property to have a location or a Relative Location
    /// </summary>
    [Serializable]
    public class GuiProperty<T> : ICloneable
    {
        [XmlIgnore]
        public T Content { get; set; }
        
        // Point location;
        [XmlIgnore]
        public Point Location { get; set; }

        public RelativeLocation LocationRelative { get; set; }
        
        public GuiProperty() : this(default(T)) { }
        
        public GuiProperty(T contents)
        {
            this.Content = contents;
            this.Location = new Point(0, 0);
            this.LocationRelative = new RelativeLocation(1.0, 1.0);
        }
        
        public GuiProperty(T contents, Point location)
        {
            this.Content = contents;
            this.Location = location;
            this.LocationRelative = new RelativeLocation(1.0, 1.0);
        }
        
        public GuiProperty(T contents, int x, int y)
        {
            this.Content = contents;
            this.Location = new Point(x, y);
            this.LocationRelative = new RelativeLocation(1.0, 1.0);
        }
        
        public GuiProperty(T contents, RelativeLocation locationRelative)
        {
            this.Content = contents;
            this.LocationRelative = locationRelative;
            this.Location = new Point(0, 0);
        }
        
        public GuiProperty(T contents, double xFactor, double yFactor)
        {
            this.Content = contents;
            this.LocationRelative = new RelativeLocation(xFactor, yFactor);
            this.Location = new Point(0, 0);
        }
        
        public object Clone()
        {
            T copiedContents = (this.Content is ICloneable) 
                                   ? (T) (this.Content as ICloneable).Clone()
                                   : this.Content;
            
            var clone = new GuiProperty<T>(copiedContents, this.Location);
           
            clone.LocationRelative = (RelativeLocation) this.LocationRelative.Clone();
            return clone;
        }
        
        public override string ToString()
        {
            return string.Format("\n[ GuiProperty: ContentsType={0} Content={1} Location={2} LocationRelative={3} ]", 
                                 Content.GetTypeNullSafe(),
                                 Content.ToStringNullSafe(),
                                 Location.ToStringNullSafe(),
                                 LocationRelative.ToStringNullSafe());
        }
        
        public override int GetHashCode()
        {
            int contentsHashCode = (this.Content != null) ? this.Content.GetHashCode() : 0;
            return contentsHashCode ^ this.Location.ToString().GetHashCode() ^ this.LocationRelative.GetHashCode();
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