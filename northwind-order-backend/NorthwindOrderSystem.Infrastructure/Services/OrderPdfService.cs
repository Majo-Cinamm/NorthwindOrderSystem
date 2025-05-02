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
using System.IO;

namespace NorthwindOrderSystem.Infrastructure.Services
{
    public class OrderPdfService
    {
        private readonly byte[]? imageData;

        public OrderPdfService()
        {
            var imagePath = Path.Combine(AppContext.BaseDirectory, "Resources", "membrete.png");
            imageData = File.Exists(imagePath) ? File.ReadAllBytes(imagePath) : null;
        }

        public byte[] GenerateOrderPdf(Order order)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontFamily("Helvetica").FontSize(10));

                    page.Header().Height(90).Element(ComposeBrandedHeader);

                    page.Content().PaddingVertical(10).Element(content =>
                    {
                        content.Border(1)
                               .BorderColor(Colors.Grey.Lighten2)
                               .Padding(20)
                               .Column(col =>
                               {
                                   col.Spacing(8);

                                   col.Item().Text($"Order #: {order?.OrderId ?? 0}")
                                       .Bold().FontSize(14).FontColor(Colors.Black);

                                   col.Item().Text($"Customer: {order?.Customer?.CustomerId ?? "N/A"}");
                                   col.Item().Text($"Employee: {order?.Employee?.FirstName} {order?.Employee?.LastName ?? "N/A"}");
                                   col.Item().Text($"Order Date: {order?.OrderDate?.ToShortDateString() ?? "N/A"}");
                                   col.Item().Text($"Shipping Address: {order?.ShipAddress ?? "N/A"} - {order?.ShipCity ?? "N/A"}");

                                   col.Item().PaddingTop(10).Text("Order Details")
                                       .Bold().FontSize(12).Underline();

                                   col.Item().Table(table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(3);
                                           columns.RelativeColumn(1);
                                           columns.RelativeColumn(1);
                                       });

                                       table.Header(header =>
                                       {
                                           header.Cell().Text("Product").SemiBold();
                                           header.Cell().Text("Quantity").SemiBold();
                                           header.Cell().Text("Unit Price").SemiBold();
                                       });

                                       if (order?.OrderDetails != null && order.OrderDetails.Any())
                                       {
                                           foreach (var detail in order.OrderDetails)
                                           {
                                               var productName = detail.Product?.ProductName ?? $"Product #{detail.ProductId}";
                                               table.Cell().Text(productName);
                                               table.Cell().Text(detail.Quantity?.ToString() ?? "0");
                                               table.Cell().Text($"${detail.UnitPrice:F2}");
                                           }
                                       }
                                       else
                                       {
                                           table.Cell().ColumnSpan(3).Text("There is no products in this order.").Italic();
                                       }
                                   });

                                   var totalAmount = order?.OrderDetails?.Sum(od => (od.UnitPrice * (od.Quantity ?? 0))) ?? 0;

                                   col.Item().PaddingTop(10)
                                       .Text($"Total: ${totalAmount:F2}")
                                       .AlignRight()
                                       .Bold();

                               });
                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GenerateAllOrdersPdf(List<Order> orders)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Header().Element(ComposeBrandedHeader);
                    page.Content().Element(content =>
                    {
                        content.Column(col =>
                        {
                            col.Spacing(20);

                            foreach (var order in orders)
                            {
                                var total = order.OrderDetails?.Sum(od => od.UnitPrice * (od.Quantity ?? 0)) ?? 0;

                                col.Item().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(15).Column(orderCol =>
                                {
                                    orderCol.Spacing(6);
                                    orderCol.Item().Text($"Order #: {order.OrderId}").Bold();
                                    orderCol.Item().Text($"Customer: {order.Customer?.ContactName ?? "N/A"}");
                                    orderCol.Item().Text($"Employee: {order.Employee?.FirstName} {order.Employee?.LastName}");
                                    orderCol.Item().Text($"Date: {order.OrderDate?.ToShortDateString() ?? "N/A"}");

                                    if (order.OrderDetails != null && order.OrderDetails.Any())
                                    {
                                        orderCol.Item().Text("Products:").Bold();
                                        foreach (var item in order.OrderDetails)
                                        {
                                            var lineTotal = (item.UnitPrice * (item.Quantity ?? 0));
                                            var productLine = $"- {item.Product?.ProductName ?? $"Product #{item.ProductId}"} ({item.Quantity} × ${item.UnitPrice:F2}) = ${lineTotal:F2}";
                                            orderCol.Item().Text(productLine);
                                        }
                                    }

                                    orderCol.Item().PaddingTop(5).Text($"Total: ${total:F2}").AlignRight().Bold();
                                });
                            }
                        });
                    });
                });
            });

            return document.GeneratePdf();
        }

        private void ComposeBrandedHeader(IContainer container)
        {
            container.Row(row =>
            {
                if (imageData != null)
                {
                    row.ConstantItem(80).Image(imageData).FitWidth();
                }

                row.RelativeItem().PaddingLeft(10).Column(col =>
                {
                    col.Spacing(2);
                    col.Item().Text("RSM US El Salvador")
                        .SemiBold().FontSize(14).FontColor(Colors.Blue.Darken2);

                    col.Item().Text("Blvd. Santa Elena #1, Antiguo Cuscatlán")
                        .FontSize(9).FontColor(Colors.Grey.Darken1);

                    col.Item().Text("Tel: +503 2510-8900")
                        .FontSize(9).FontColor(Colors.Grey.Darken1);
                });
            });
        }

        private IContainer CellStyle(IContainer container)
        {
            return container.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
        }
    }
}
