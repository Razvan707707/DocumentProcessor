namespace DocumentProcessor.Core;

public class Document
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public class LegacyDocument
{
    public string XmlTitle { get; set; } = string.Empty;
    public string XmlBody { get; set; } = string.Empty;
}

public class ProcessingResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
}

public interface IDocumentParser
{
    Document Parse(string content);
}