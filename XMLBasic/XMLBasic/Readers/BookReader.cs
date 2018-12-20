using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XMLBasic.Entities;
using XMLBasic.Interfaces;

namespace XMLBasic.Readers
{
    public class BookReader : BaseReader
    {
        public override string TypeOfRecord => "book";
        public override IEntity ReadElement(XElement element)
        {
            if (element is null) {
                throw new ArgumentNullException($"Can't read the element. Element is null!");
            }

            return new Book {
                Name = GetAttribute(element, "name"),
                Authors = GetElement(element, "authors").Elements("author")
                    .Select(e => new Author
                    {
                        Name = GetAttribute(e, "name"),
                        SurName = GetAttribute(e, "surname")
                    }).ToList(),
                PublicationCity = GetAttribute(element, "publicationCity"),
                PublishingHouseName = GetAttribute(element, "publishingHouseName"),
                PublicationYear = int.Parse(GetAttribute(element, "publicationYear")),
                PapersCount = int.Parse(GetAttribute(element, "papersCount")),
                Note = GetElement(element, "note").Value,
                ISBN = GetAttribute(element, "isbn")
            };
        }
    }
}
