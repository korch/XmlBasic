using System;
using System.Linq;
using System.Xml.Linq;
using XMLBasic.Entities;
using XMLBasic.Interfaces;

namespace XMLBasic.Readers
{
    public class PatentReader : BaseReader
    {
        public override string TypeOfRecord => "patent";
        public override IEntity ReadElement(XElement element)
        {
            if (element is null) {
                throw new ArgumentNullException($"Can't read the element. Element is null!");
            }

            return new Patent {
                Name = GetAttribute(element, "name"),
                Inventors = GetElement(element, "inventors").Elements("inventor")
                    .Select(e => new Inventor {
                        Name = GetAttribute(e, "name"),
                        SurName = GetAttribute(e, "surname")
                    }).ToList(),
                Country = GetAttribute(element, "country"),
                RegistrationNumber = GetAttribute(element, "registrationNumber"),
                ClaimDate = GetDate(GetAttribute(element, "claimDate")),
                PublicationDate = GetDate(GetAttribute(element, "publicationDate")),
                PapersCount = int.Parse(GetAttribute(element, "papersCount")),
                Note = GetElement(element, "note").Value
            };
        }
    }
}
