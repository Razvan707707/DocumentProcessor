namespace DocumentProcessor.Core;

public class DocumentProcessingFacade
{
    public ProcessingResult Process(string content)
    {
        try
        {
            // Detecție simplă a formatului
            bool isXml = content.TrimStart().StartsWith("<");

            // Selectăm baza lanțului
            IDocumentParser baseParser = isXml ? new XmlParserAdapter() : new JsonDocumentParser();

            // Aplicăm toți decoratorii ca niște foi de ceapă
            IDocumentParser pipeline = new LoggingDocumentParser(
                new ValidationDocumentParser(
                    new CachingDocumentParser(baseParser)
                )
            );

            // Execuția lanțului
            var document = pipeline.Parse(content);

            // Simulăm salvarea
            string path = $"output_{Guid.NewGuid().ToString().Substring(0, 5)}.txt";
            File.WriteAllText(path, $"Titlu: {document.Title}\nContinut: {document.Content}");

            return new ProcessingResult { IsSuccess = true, Message = "Procesat cu succes", FilePath = path };
        }
        catch (Exception ex)
        {
            // Facade gestionează eroarea intern, fără a bloca aplicația
            return new ProcessingResult { IsSuccess = false, Message = $"Eroare la procesare: {ex.Message}" };
        }
    }
}