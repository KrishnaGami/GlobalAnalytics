using GlobalAnalytics.Lib.DTOs;
using GlobalAnalytics.Lib.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GlobalAnalytics.Data.Exporters
{
    public class PdfExporter : IExporter
    {
        public byte[] Export(List<ClientDto> data)
        {
            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Name");
                            header.Cell().Element(CellStyle).Text("Country");
                            header.Cell().Element(CellStyle).Text("Revenue");
                            header.Cell().Element(CellStyle).Text("CreatedDate");
                        });

                        foreach (var item in data)
                        {
                            table.Cell().Element(CellStyle).Text(item.Name);
                            table.Cell().Element(CellStyle).Text(item.Country);
                            table.Cell().Element(CellStyle).Text($"{item.Revenue}");
                            table.Cell().Element(CellStyle).Text(item.CreatedDate.ToShortDateString());
                        }

                        IContainer CellStyle(IContainer container) =>
                            container.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                    });
                });
            });

            return doc.GeneratePdf();
        }
    }
}