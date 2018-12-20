using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using XMLBasic.Entities;
using XMLBasic.Interfaces;

namespace XMLBasic.Writers
{
    public class PaperWriter : BaseWriter
    {
        public override Type TypeOfElement => typeof(Paper);
        public override void Write(XmlWriter xmlWriter, IEntity record)
        {
            var paper = (Paper)record;
            if (paper is null) {
                throw new ArgumentNullException($"Can't write the element! Element of Papers is null!");
            }

            var element = new XElement("book");
            WriteAttribute(element, "name", paper.Name);
            WriteAttribute(element, "publicationCity", paper.PublicationCity);
            WriteAttribute(element, "publishingHouseName", paper.PublishingHouseName);
            WriteAttribute(element, "publicationYear", paper.PublicationYear.ToString());
            WriteAttribute(element, "papersCount", paper.PapersCount.ToString());
            WriteElement(element, "note", paper.Note);
            WriteAttribute(element, "number", paper.Number.ToString());
            WriteAttribute(element, "date", paper.Date.ToString(
                CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, CultureInfo.InvariantCulture));
            WriteAttribute(element, "isbn", paper.ISBN);

            element.WriteTo(xmlWriter);
        }
    }
}
