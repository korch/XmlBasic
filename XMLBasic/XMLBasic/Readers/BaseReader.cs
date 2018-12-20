using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XMLBasic.Interfaces;

namespace XMLBasic.Readers
{
    public abstract class BaseReader : IXmlReader
    {
        public abstract string TypeOfRecord { get; }
        public abstract IEntity ReadElement(XElement element);
        protected string GetAttribute(XElement element, string attributeName)
        {
            var attribute = element.Attribute(attributeName);
            if (string.IsNullOrEmpty(attribute?.Value)) {
                throw new ArgumentException("Can't get attribute from the element. Attribute is null or empty.");
            }

            return attribute?.Value;
        }

        protected XElement GetElement(XElement parentElement, string elementName)
        {
            var element = parentElement.Element(elementName);
            if (string.IsNullOrEmpty(element?.Value)) {
                throw new ArgumentException("Can't get element. Element is null or empty.");
            }

            return element;
        }

        protected DateTime GetDate(string date)
        {
            if (string.IsNullOrEmpty(date)) {
                return default(DateTime);
            }

            return DateTime.ParseExact(date, CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern,
                CultureInfo.InvariantCulture.DateTimeFormat);
        }
    }
}
