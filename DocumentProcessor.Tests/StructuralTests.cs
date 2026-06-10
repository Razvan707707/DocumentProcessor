using DocumentProcessor.Core;
using NUnit.Framework;

namespace DocumentProcessor.Tests;

// Mock simplu pentru a număra de câte ori se apelează parser-ul intern
public class MockCountingParser : IDocumentParser
{
    public int CallCount { get; private set; }
    public Document Parse(string content)
    {
        CallCount++;
        return new Document { Title = "Valid", Content = content };
    }
}

[TestFixture]
public class StructuralTests
{
    [Test]
    public void Adapter_XmlAndJson_ReturnEquivalentDocument()
    {
        var xmlAdapter = new XmlParserAdapter();
        var jsonParser = new JsonDocumentParser();

        string xml = "<Doc><Title>Test</Title><Body>Body content</Body></Doc>";
        string json = "{\"Title\":\"Test\",\"Content\":\"Body content\"}";

        var docFromXml = xmlAdapter.Parse(xml);
        var docFromJson = jsonParser.Parse(json);

        Assert.That(docFromXml.Title, Is.EqualTo(docFromJson.Title));
        Assert.That(docFromXml.Content, Is.EqualTo(docFromJson.Content));
    }

    [Test]
    public void ValidationDecorator_EmptyTitle_ThrowsValidationException()
    {
        var mockParser = new MockCountingParser();
        var validationDecorator = new ValidationDocumentParser(mockParser);

        // Mock-ul returneaza un document, dar il modificam manual sa testam validarea
        var invalidJson = "{\"Title\":\"\",\"Content\":\"1234567890\"}";
        var badParser = new JsonDocumentParser();
        var pipeline = new ValidationDocumentParser(badParser);

        Assert.Throws<ValidationException>(() => pipeline.Parse(invalidJson));
    }

    [Test]
    public void CachingDecorator_CallsInnerParserOnlyOnce_ForSameContent()
    {
        var mockParser = new MockCountingParser();
        var cachingDecorator = new CachingDocumentParser(mockParser);

        string testContent = "Test content";

        cachingDecorator.Parse(testContent); // Primul apel -> parseaza
        cachingDecorator.Parse(testContent); // Al doilea apel -> cache

        Assert.That(mockParser.CallCount, Is.EqualTo(1));
    }

    [Test]
    public void Facade_InvalidContent_ReturnsIsSuccessFalse_WithoutThrowing()
    {
        var facade = new DocumentProcessingFacade();
        string invalidContent = "{\"Title\":\"\",\"Content\":\"scurt\"}";

        var result = facade.Process(invalidContent);

        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Message, Does.Contain("titlu"));
    }
}