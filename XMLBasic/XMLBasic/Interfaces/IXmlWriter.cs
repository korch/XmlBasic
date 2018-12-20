using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLBasic.Interfaces
{
    public interface IXmlWriter
    {
        Type TypeOfElement { get; }
        void Write(XmlWriter writer, IEntity record);
    }
}
