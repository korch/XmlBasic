using System;
using System.Linq;
using System.Xml.Linq;
using XMLBasic.Entities;
using XMLBasic.Interfaces;

namespace XMLBasic.Readers
{
    public class PaperReader : BaseReader
    {
        public override string TypeOfRecord => "paper";
        public override IEntity ReadElement(XElement element)
        {
            if (element is null) {
                throw new ArgumentNullException($"Can't read the element. Element is null!");
            }

            return new Paper {
                Name = GetAttribute(element, "name"),
                PublicationCity = GetAttribute(element, "publicationCity"),
                PublishingHouseName = GetAttribute(element, "publishingHouseName"),
                PublicationYear = int.Parse(GetAttribute(element, "publicationYear")),
                PapersCount = int.Parse(GetAttribute(element, "papersCount")),
                Note = GetElement(element, "note").Value,
                Number = int.Parse(GetAttribute(element, "number")),
                Date = GetDate(GetAttribute(element, "date")),
                ISBN = GetAttribute(element, "isbn")
            };
        }
    }
}
