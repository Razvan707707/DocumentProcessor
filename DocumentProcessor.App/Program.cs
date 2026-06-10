using DocumentProcessor.Core;

Console.WriteLine("=== Testare Design Patterns Structurale ===\n");

var facade = new DocumentProcessingFacade();

string xmlContent = "<Doc><Title>Raport XML</Title><Body>Acesta este un continut suficient de lung.</Body></Doc>";
string jsonContent = "{\"Title\":\"Raport JSON\",\"Content\":\"Acesta este de asemenea un continut corect si lung.\"}";
string invalidJson = "{\"Title\":\"\",\"Content\":\"Prea scurt\"}";

Console.WriteLine("1. Procesare fisier XML valid:");
var r1 = facade.Process(xmlContent);
Console.WriteLine($"Status: {r1.IsSuccess}, Mesaj: {r1.Message}\n");

Console.WriteLine("2. Procesare fisier JSON invalid (Fara Titlu, Continut scurt):");
var r2 = facade.Process(invalidJson);
Console.WriteLine($"Status: {r2.IsSuccess}, Mesaj: {r2.Message}\n");