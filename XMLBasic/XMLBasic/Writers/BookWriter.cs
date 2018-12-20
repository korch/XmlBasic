using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using XMLBasic.Entities;
using XMLBasic.Interfaces;

namespace XMLBasic.Writers
{
    public class BookWriter : BaseWriter
    {
        public override Type TypeOfElement => typeof(Book);

        public override void Write(XmlWriter xmlWriter, IEntity record)
        {
            var book = (Book)record;
            if (book is null) {
                throw new ArgumentNullException($"Can't write the element! Element of Books is null!");
            }

            var element = new XElement("book");
            WriteAttribute(element, "name", book.Name);
            WriteElement(element, "authors",
                book.Authors?.Select(a => new XElement("author",
                    new XAttribute("name", a.Name),
                    new XAttribute("surname", a.SurName)
                ))
            );
            WriteAttribute(element, "publicationCity", book.PublicationCity);
            WriteAttribute(element, "publishingHouseName", book.PublishingHouseName);
            WriteAttribute(element, "publicationYear", book.PublicationYear.ToString());
            WriteAttribute(element, "papersCount", book.PapersCount.ToString());
            WriteElement(element, "note", book.Note);
            WriteAttribute(element, "isbn", book.ISBN); 

            element.WriteTo(xmlWriter);
        }
    }
}
