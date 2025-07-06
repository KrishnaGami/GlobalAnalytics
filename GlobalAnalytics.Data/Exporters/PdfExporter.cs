using GlobalAnalytics.Lib.DTOs;
using GlobalAnalytics.Lib.Interfaces;
using log4net;
using QuestPDF.Fluent;

namespace GlobalAnalytics.Data.Exporters
{
    public class PdfExporter : IExporter
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(PdfExporter));

        public byte[] Export(List<ClientDto> data)
        {
            _logger.Info($"Generating PDF export. Rows: {data.Count}");

            try
            {
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(20);
                        page.Header().Text("Client Report").FontSize(18).Bold().AlignCenter();
                        page.Content().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                for (int i = 0; i < 6; i++) columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Name").Bold();
                                header.Cell().Text("Email").Bold();
                                header.Cell().Text("Country").Bold();
                                header.Cell().Text("Industry").Bold();
                                header.Cell().Text("Revenue").Bold();
                                header.Cell().Text("Active").Bold();
                            });

                            foreach (var client in data)
                            {
                                table.Cell().Text(client.Name);
                                table.Cell().Text(client.Email);
                                table.Cell().Text(client.Country);
                                table.Cell().Text(client.Industry);
                                table.Cell().Text(client.Revenue.ToString("C"));
                                table.Cell().Text(client.IsActive ? "Yes" : "No");
                            }
                        });

                        page.Footer().Text($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}").FontSize(10).AlignRight();
                    });
                });

                var stream = new MemoryStream();
                document.GeneratePdf(stream);
                _logger.Info("PDF export generation completed.");
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                _logger.Error("Error during PDF export.", ex);
                return Array.Empty<byte>();
            }
        }
    }
}