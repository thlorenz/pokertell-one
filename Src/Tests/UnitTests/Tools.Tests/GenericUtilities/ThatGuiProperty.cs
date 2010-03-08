/*
 * User: Thorsten Lorenz
 * Date: 6/22/2009
 * 
 */


namespace Tools.Tests.GenericUtilities
{
    using System;
    using System.Drawing;

    using NUnit.Framework;

    using PokerTell.UnitTests;

    using Tools.GenericUtilities;
    using Tools.Serialization;

    [TestFixture]
    public class ThatGuiProperty : TestWithLog
    {
        [Test] public void Constructor_with_Location_sets_Value_and_Location()
        {
            var location = new Point(10,10);
            string val = "theValue";
            GuiProperty<string> prop = new GuiProperty<string>(val, location);
            
            Assert.That(prop.Content, Is.EqualTo(val));
            Assert.That(prop.Location, Is.EqualTo(location));
        }
        
        [Test] public void Constructor_with_LocationRelative_sets_Value_and_LocationRelative()
        {
            var locationRelative = new RelativeLocation(2.3, 3.4);
            string val = "theValue";
            GuiProperty<string> prop = new GuiProperty<string>(val, locationRelative);
            
            Assert.That(prop.Content, Is.EqualTo(val));
            Assert.That(prop.LocationRelative, Is.EqualTo(locationRelative));
        }
        
        [Test] public void Constructor_with_Ints_sets_Value_and_Location()
        {
            int x = 3;
            int y = 4;
            
            string val = "theValue";
            GuiProperty<string> prop = new GuiProperty<string>(val, x, y);

            Assert.That(prop.Content, Is.EqualTo(val));
            Assert.That(prop.Location.X, Is.EqualTo(x));
            Assert.That(prop.Location.Y, Is.EqualTo(y));
        }
        
        [Test] public void Constructor_with_Doubles_sets_Value_and_LocationRelative()
        {
            double xFactor =2.3;
            double yFactor = 3.4;
            
            string val = "theValue";
            GuiProperty<string> prop = new GuiProperty<string>(val, xFactor, yFactor);
            
            Assert.That(prop.Content, Is.EqualTo(val));
            Assert.That(prop.LocationRelative.XFactor, Is.EqualTo(xFactor));
            Assert.That(prop.LocationRelative.YFactor, Is.EqualTo(yFactor));
        }
        
        [Test] public void isCloneable()
        {
            double xFactor = 2.3;
            double yFactor = 3.4;
            Point origLocation = new Point(30, 30);
            const string val = "theValue";
            
            GuiProperty<string> original = new GuiProperty<string>(val, xFactor, yFactor);
            original.Location = origLocation;
           
            GuiProperty<string> clone = (GuiProperty<string>) original.Clone();
            
            original.Content = "newContents";
            original.Location.Offset(10, 10);
            original.LocationRelative.XFactor += 0.1;
            original.LocationRelative.YFactor += 0.1;
            
            // Use constant or hardcoded values in Assertions to avoid incorrect testing
            // due to the fact, that the references of the 'check' values just point
            // to the value being checked
            
            Assert.That(clone.LocationRelative.XFactor, Is.EqualTo(xFactor));
            Assert.That(clone.LocationRelative.YFactor, Is.EqualTo(yFactor));
            Assert.That(clone.Location, Is.EqualTo(new Point(30, 30)));
            Assert.That(clone.Content, Is.EqualTo(val));
        }
        
        [Test] public void Clone_clonesContentsIfItImplementsICloneable()
        {
            const string origString = "original";
            var reference = new CloneableClass(origString);
            var original = new GuiProperty<CloneableClass>(reference);
            
            var clone = (GuiProperty<CloneableClass>) original.Clone();
            clone.Content.check = "clone";
            
            Assert.That(original.Content.check, Is.EqualTo(origString));
        }
        
        [Test] public void isXmlSerializableAndSavesRelativeLocationOnly()
        {
            double xFactor = 2.3;
            double yFactor = 3.4;
            const string val = "theValue";
            
            
            GuiProperty<string> original = new GuiProperty<string>(val, xFactor, yFactor);
            original.Location = new Point(20, 20);
            
            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) 
                + @"\temp\guiProperty.xml";
            
            XmlStandardSerializer.SerializeObjectGraph(original, fileName);
            var deserialized = (GuiProperty<string>)
                XmlStandardSerializer.DeserializeObjectGraph(fileName, typeof(GuiProperty<string>));
            
            Assert.That(deserialized.LocationRelative, Is.EqualTo(original.LocationRelative));
            Assert.That(deserialized.Content, Is.Not.EqualTo(original.Content));
            Assert.That(deserialized.Location, Is.Not.EqualTo(original.Location));
        }
        
    }
    
    class CloneableClass : ICloneable
    {
        public string check;
        public CloneableClass(string check) { this.check = check; }
        public object Clone() { return new CloneableClass(this.check); }
    }
}
