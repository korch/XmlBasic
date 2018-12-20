using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMLBasic.Interfaces
{
    public interface IXmlReader
    {
        string TypeOfRecord { get; }
        IEntity ReadElement(XElement element);
    }
}
