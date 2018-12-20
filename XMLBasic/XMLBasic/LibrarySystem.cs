using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using XMLBasic.Interfaces;

namespace XMLBasic
{
    public class LibrarySystem
    {
        private static string _catalogElementName = "catalog";

        private readonly IDictionary<string, IXmlReader> _readers;
        private readonly IDictionary<Type, IXmlWriter> _writers;

        public LibrarySystem()
        {
            _readers = new Dictionary<string, IXmlReader>();
            _writers = new Dictionary<Type, IXmlWriter>();
        }

        public void AddParsers(params IXmlReader[] readers)
        {
            foreach (var reader in readers) {
                _readers.Add(reader.TypeOfRecord, reader);
            }
        }

        public void AddWriters(params IXmlWriter[] writers)
        {
            foreach (var writer in writers) {
                _writers.Add(writer.TypeOfElement, writer);
            }
        }

        public IEnumerable<IEntity> ReadFrom(TextReader input)
        {
            using (XmlReader xmlReader = XmlReader.Create(input, new XmlReaderSettings {
                IgnoreWhitespace = true,
                IgnoreComments = true
            })) {
                xmlReader.ReadToFollowing(_catalogElementName);
                xmlReader.ReadStartElement();

                do
                {
                    while (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        var node = XNode.ReadFrom(xmlReader) as XElement;
                        IXmlReader parser;
                        if (_readers.TryGetValue(node.Name.LocalName, out parser)) {
                            yield return parser.ReadElement(node);
                        } else {
                            throw new InvalidOperationException($"Founded unknown element tag: {node.Name.LocalName}");
                        }
                    }
                } while (xmlReader.Read());
            }
        }

        public void WriteTo(TextWriter output, IEnumerable<IEntity> catalogEntities)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(output, new XmlWriterSettings())) {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement(_catalogElementName);
                foreach (var catalogEntity in catalogEntities) {
                    IXmlWriter writer;
                    if (_writers.TryGetValue(catalogEntity.GetType(), out writer)) {
                        writer.Write(xmlWriter, catalogEntity);
                    } else {
                        throw new InvalidOperationException($"Cannot find entity writer for type {catalogEntity.GetType().FullName}");
                    }
                }
                xmlWriter.WriteEndElement();
            }
        }
    }
}
