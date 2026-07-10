using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace FurnitureERP.Infrastructure.Documents.Templates;

public abstract class ERPDocumentBase
    : IDocument
{
    public abstract void Compose(IDocumentContainer container);

    public byte[] GeneratePdf()
    {
        return Document.Create(Compose).GeneratePdf();
    }

    public DocumentMetadata GetMetadata()
        => DocumentMetadata.Default;
}