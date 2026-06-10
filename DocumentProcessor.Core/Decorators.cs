using System.Diagnostics;

namespace DocumentProcessor.Core;

public class ValidationException : Exception { public ValidationException(string msg) : base(msg) { } }

// Clasa de bază pentru Decoratori
public abstract class DocumentParserDecorator : IDocumentParser
{
    protected readonly IDocumentParser _innerParser;
    protected DocumentParserDecorator(IDocumentParser innerParser) => _innerParser = innerParser;

    public virtual Document Parse(string content) => _innerParser.Parse(content);
}

// Decorator 1: Logging
public class LoggingDocumentParser : DocumentParserDecorator
{
    public LoggingDocumentParser(IDocumentParser innerParser) : base(innerParser) { }

    public override Document Parse(string content)
    {
        var sw = Stopwatch.StartNew();
        Console.WriteLine($"[LOG] Start parsing...");
        var doc = base.Parse(content);
        sw.Stop();
        Console.WriteLine($"[LOG] Finished parsing in {sw.ElapsedMilliseconds}ms. Result length: {doc.Content.Length}");
        return doc;
    }
}

// Decorator 2: Validare
public class ValidationDocumentParser : DocumentParserDecorator
{
    public ValidationDocumentParser(IDocumentParser innerParser) : base(innerParser) { }

    public override Document Parse(string content)
    {
        var doc = base.Parse(content);
        if (string.IsNullOrWhiteSpace(doc.Title)) throw new ValidationException("Documentul trebuie sa aiba un titlu.");
        if (doc.Content.Length < 10) throw new ValidationException("Continutul documentului trebuie sa aiba minim 10 caractere.");
        return doc;
    }
}

// Decorator 3: Caching
public class CachingDocumentParser : DocumentParserDecorator
{
    private readonly Dictionary<string, Document> _cache = new();

    public CachingDocumentParser(IDocumentParser innerParser) : base(innerParser) { }

    public override Document Parse(string content)
    {
        // Folosim direct content-ul ca si cheie pentru simplitate
        if (_cache.TryGetValue(content, out var cachedDoc))
        {
            Console.WriteLine("[CACHE] Returneaza documentul din memorie.");
            return cachedDoc;
        }

        var doc = base.Parse(content);
        _cache[content] = doc;
        return doc;
    }
}