namespace Js.Snippets.CSharp.XmlUtils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// Extension methods for XML processing
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// Converts between <see cref="XDocument"/> and <see cref="XmlDocument"/> XML documents.
        /// </summary>
        /// <param name="document">The XML document.</param>
        /// <returns>A <see cref="XmlDocument"/> representation of the given XML document.</returns>
        public static XmlDocument ToXmlDocument(this XDocument document)
        {
            using (var xmlReader = document.CreateReader())
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlReader);
                return xmlDocument;
            }
        }

        /// <summary>
        /// Converts between <see cref="XDocument"/> and <see cref="XmlDocument"/> XML documents.
        /// </summary>
        /// <param name="element">The root element of the XML fragment.</param>
        /// <returns>A <see cref="XmlDocument"/> representation of the given XML fragment.</returns>
        public static XmlDocument ToXmlDocument(this XElement element)
        {
            var output = new StringBuilder();
            var settings = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = false };
            using (var writer = XmlWriter.Create(output, settings))
            {
                element.WriteTo(writer);
            }

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(output.ToString());
            return xmlDocument;
        }

        /// <summary>
        /// Converts between <see cref="XmlDocument"/> and <see cref="XDocument"/> XML documents.
        /// </summary>
        /// <param name="document">The XML document.</param>
        /// <returns>A <see cref="XDocument"/> representation of the given XML document.</returns>
        public static XDocument ToXDocument(this XmlDocument document)
        {
            using (var reader = new XmlNodeReader(document))
            {
                reader.MoveToContent();
                return XDocument.Load(reader);
            }
        }
    }
}
