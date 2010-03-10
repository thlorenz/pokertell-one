namespace Tools.Xml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using FunctionalCSharp;

    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    public static class Utils
    {
        public static string GetStringFrom(XElement element, string alternative)
        {
            if (element != null)
            {
                return element.Value;
            }

            return alternative;
        }

        public static IList<IPositionViewModel> GetPositionsFrom(XElement element)
        {
            var positionElems = element.Elements("Position");
            return positionElems.Select(elem => GetPositionFrom(elem, 10, 10)).ToList();
        }

        public static IEnumerable<string> GetStringsFrom(XElement element, IEnumerable<string> alternative)
        {
            if (element == null)
                return alternative;

            var stringElems = element.Elements("Item");
            if (stringElems == null)
                return alternative;

            return stringElems.Select(elem => elem.Value);
        }

        public static bool GetBoolFrom(XElement element, bool alternative)
        {
            bool result;
            if (element != null && Boolean.TryParse(element.Value, out result))
            {
                return result;
            }

            return alternative;
        }

        public static int GetIntFrom(XElement element, int alternative)
        {
            int result;
            if (element != null && Int32.TryParse(element.Value, out result))
            {
                return result;
            }

            return alternative;
        }

        public static double GetDoubleFrom(XElement element, double alternative)
        {
            double result;
            if (element != null && Double.TryParse(element.Value, out result))
            {
                return result;
            }

            return alternative;
        }

        public static IPositionViewModel GetPositionFrom(XElement element, double alternateLeft, double alternateTop)
        {
            if (element == null)
            {
                return new PositionViewModel(alternateLeft, alternateTop);
            }

            var left = GetDoubleFrom(element.Element("Left"), alternateLeft);

            var top = GetDoubleFrom(element.Element("Top"), alternateTop);

            return new PositionViewModel(left, top);
        }

        public static XElement XElementForPositions(string name, IList<IPositionViewModel> positions)
        {
            var elem = new XElement(name);
            positions.ForEach(p => elem.Add(new XElement("Position", new XElement("Left", p.Left), new XElement("Top", p.Top))));
            return elem;
        }

        public static XElement XElementForCollection<T>(string name, IEnumerable<T> items)
        {
            var elem = new XElement(name);
            if (items != null)
            {
                items.ForEach(i => elem.Add(new XElement("Item", i)));
            }

            return elem;
        }
    }
}