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
            return positionElems.Select(elem => GetPositionFrom(elem)).ToList();
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

        public static IPositionViewModel GetPositionFrom(XElement element)
        {
            var left = GetDoubleFrom(element.Element("Left"), 10.0);

            var top = GetDoubleFrom(element.Element("Top"), 10.0);

            return new PositionViewModel(left, top);
        }

        public static XElement XElementForPositions(string name, IList<IPositionViewModel> positions)
        {
            var elem = new XElement(name);
            positions.ForEach(p => elem.Add(new XElement("Position", new XElement("Left", p.Left), new XElement("Top", p.Top))));
            return elem;
        }
    }
}