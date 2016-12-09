using Microsoft.BizTalk.Streaming;
using Microsoft.BizTalk.XPath;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BizTalkComponents.PipelineComponents.ValidateCustom
{
    public class XmlMessageReader : IMessageReader
    {
        public string ReadValue(Stream msg, string expression)
        {
            VirtualStream virtualStream = new VirtualStream(VirtualStream.MemoryFlag.AutoOverFlowToDisk);
            ReadOnlySeekableStream readOnlySeekableStream = new ReadOnlySeekableStream(msg, virtualStream);

            XmlTextReader xmlTextReader = new XmlTextReader(readOnlySeekableStream);
            XPathCollection xPathCollection = new XPathCollection();
            XPathReader xPathReader = new XPathReader(xmlTextReader, xPathCollection);
            xPathCollection.Add(expression);

            string value = string.Empty;

            while (xPathReader.ReadUntilMatch())
            {
                if (xPathReader.Match(0))
                {
                    if (xPathReader.NodeType == XmlNodeType.Attribute)
                    {
                        value = xPathReader.GetAttribute(xPathReader.Name);
                    }
                    else
                    {
                        value = xPathReader.ReadString();
                    }

                    break;
                }
            }

            readOnlySeekableStream.Position = 0;
            readOnlySeekableStream.Seek(0, SeekOrigin.Begin);

            return value;
        }
    }
}
