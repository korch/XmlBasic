using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLBasic.Interfaces;

namespace XMLBasic.Entities
{
    public class Patent : IEntity
    {
        public string Name { get; set; }
        public List<Inventor> Inventors { get; set; }
        public string Country { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime ClaimDate { get; set; }
        public DateTime PublicationDate { get; set; }
        public int PapersCount { get; set; }
        public string Note { get; set; }
    }
}
