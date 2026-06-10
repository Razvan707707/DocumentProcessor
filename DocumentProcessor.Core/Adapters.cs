using System.Text.Json;
using System.Xml;

namespace DocumentProcessor.Core;

// Componenta Legacy care NU poate fi modificată direct
public class LegacyXmlParser
{
    public LegacyDocument ParseXml(XmlDocument xml)
    {
        var titleNode = xml.SelectSingleNode("//Title")?.InnerText ?? "Fara Titlu";
        var bodyNode = xml.SelectSingleNode("//Body")?.InnerText ?? string.Empty;
        return new LegacyDocument { XmlTitle = titleNode, XmlBody = bodyNode };
    }
}

// ADAPTER: Traduce din noul format (string) in vechiul format (XmlDocument)
public class XmlParserAdapter : IDocumentParser
{
    private readonly LegacyXmlParser _legacyParser = new();

    public Document Parse(string content)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(content); // Conversie string -> Xml

        var legacyDoc = _legacyParser.ParseXml(xmlDoc); // Apel cod vechi

        // Mapare inapoi la noul model
        return new Document { Title = legacyDoc.XmlTitle, Content = legacyDoc.XmlBody };
    }
}

// Parser Nativ (JSON) pentru a arăta că pot fi inversate oricând
public class JsonDocumentParser : IDocumentParser
{
    public Document Parse(string content)
    {
        return JsonSerializer.Deserialize<Document>(content) ?? new Document();
    }
}