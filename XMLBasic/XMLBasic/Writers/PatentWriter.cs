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
    public class PatentWriter : BaseWriter
    {
        public override Type TypeOfElement => typeof(Patent);
        public override void Write(XmlWriter xmlWriter, IEntity record)
        {
            var patent = (Patent)record;
            if (patent is null) {
                throw new ArgumentNullException($"Can't write the element! Element of Patents is null!");
            }

            var element = new XElement("book");
            WriteAttribute(element, "name", patent.Name);
            WriteElement(element, "inventors",
                patent.Inventors?.Select(a => new XElement("inventor",
                    new XAttribute("name", a.Name),
                    new XAttribute("surname", a.SurName)
                ))
            );
            WriteAttribute(element, "country", patent.Country);
            WriteAttribute(element, "registrationNumber", patent.RegistrationNumber);
            WriteAttribute(element, "claimDate", patent.ClaimDate.ToString(
                CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, CultureInfo.InvariantCulture));
            WriteAttribute(element, "publicationDate", patent.PublicationDate.ToString(
                CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, CultureInfo.InvariantCulture));
            WriteAttribute(element, "papersCount", patent.PapersCount.ToString());
            WriteElement(element, "note", patent.Note);
           
            element.WriteTo(xmlWriter);
        }
    }
}
