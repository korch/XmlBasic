﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using XMLBasic.Interfaces;

namespace XMLBasic.Entities
{
    public class Book : IEntity
    {
        public string Name { get; set; }
        public List<Author> Authors { get; set; }
        public string PublicationCity { get; set; }
        public string PublishingHouseName { get; set; } 
        public int PublicationYear { get; set; }
        public int PapersCount { get; set; }
        public string Note { get; set; }
        public string ISBN { get; set; }
    }
}
