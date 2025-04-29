using NorthwindOrderSystem.Core.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using QuestPDF.Elements;
using QuestPDF.Elements.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindOrderSystem.Infrastructure.Services
{
    public class OrderPdfService
    {
        public byte[] GenerateOrderPdf(Order order)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(14));

                    page.Header()
                        .Text($"Orden #{order?.OrderId ?? 0}")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .Column(col =>
                        {
                            col.Spacing(10);

                            col.Item().Text($"Cliente: {order?.Customer?.CustomerId ?? "N/A"}");
                            col.Item().Text($"Empleado: {order?.Employee?.EmployeeId.ToString() ?? "N/A"}");
                            col.Item().Text($"Fecha de Orden: {order?.OrderDate?.ToShortDateString() ?? "N/A"}");
                            col.Item().Text($"Dirección de Envío: {order?.ShipAddress ?? "N/A"} - {order?.ShipCity ?? "N/A"}");

                            col.Item().Text("Detalles del Pedido").Bold().FontSize(16).Underline();

                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3); // Producto
                                    columns.RelativeColumn(1); // Cantidad
                                    columns.RelativeColumn(1); // Precio
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("Producto").SemiBold();
                                    header.Cell().Text("Cantidad").SemiBold();
                                    header.Cell().Text("Precio Unitario").SemiBold();
                                });

                                if (order?.OrderDetails != null && order.OrderDetails.Any())
                                {
                                    foreach (var detail in order.OrderDetails)
                                    {
                                        table.Cell().Text($"Producto #{detail?.ProductId ?? 0}");
                                        table.Cell().Text(detail?.Quantity?.ToString() ?? "0");
                                        table.Cell().Text($"${detail?.UnitPrice ?? 0:F2}");
                                    }
                                }
                                else
                                {
                                    table.Cell().ColumnSpan(3).Text("No hay productos en esta orden.").Italic();
                                }
                            });

                            col.Item().Text($"Total Freight: ${order?.Freight?.ToString("F2") ?? "0.00"}")
                                .AlignRight().Bold();
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
}
