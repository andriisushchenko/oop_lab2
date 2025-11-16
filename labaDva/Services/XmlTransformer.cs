using System.IO;
using System.Xml.Xsl;

namespace labaDva.Services
{
    public class XmlTransformer
    {
        public string Transform(string xmlPath, string xslPath)
        {
            var xslt = new XslCompiledTransform();
            xslt.Load(xslPath);

            using (var sw = new StringWriter())
            {
                xslt.Transform(xmlPath, null, sw);
                return sw.ToString();
            }
        }
    }
}