using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using XMLBasic.Interfaces;

namespace XMLBasic.Writers
{
    public abstract class BaseWriter : IXmlWriter
    {
        public abstract Type TypeOfElement { get; }
        public abstract void Write(XmlWriter xmlWriter, IEntity record);
        protected string GetShortDateString(DateTime date)
        {
            return date.ToString(CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, CultureInfo.InvariantCulture);
        }

        protected void WriteAttribute(XElement element, string attributeName, string value)
        {
            if (string.IsNullOrEmpty(value)) {
                throw new ArgumentNullException($"Can't write the attribute from the element. Attribute is null or empty!");
            }

            element.SetAttributeValue(attributeName, value);
        }

        protected void WriteElement(XElement element, string newElementName, object value)
        {
            if (value == null) {
                throw new ArgumentNullException($"Can't write the element. Element is null!");
            }

            var record = new XElement(newElementName, value);
            element.Add(record);
        }
    }
}
