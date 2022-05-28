using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Api.Services
{
    public class InvoiceService
    {
        private readonly DatabaseContext _context;

        public InvoiceService(DatabaseContext context)
        {
            _context = context;
        }

        public MemoryStream ProduceExcel(int orderId)
        {
            using var memoryStream = new MemoryStream();
            using var package = new ExcelPackage(memoryStream);

            var worksheet = package.Workbook.Worksheets.Add($"InvoiceOrder{orderId}");

            var orderData = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (orderData is null)
                throw new Exception("Order data not found");

            var products = _context.OrderedProducts.Where(p => p.OrderId == orderId)
                .ToList();

            var buyer = _context.Users
                .Include(u => u.PrimaryAddress)
                .FirstOrDefault(u => u.Id == orderData.UserId);

            var worker = _context.Users
                .FirstOrDefault(u => u.Id == orderData.WorkerId);

            if (buyer is not null)
            {
                AddBuyerDataToSheet(worksheet, buyer);
            }

            if (worker is not null)
            {
                AddWorkerDataToSheet(worksheet, worker);
            }
            
            AddProductListHeaders(worksheet);

            var rowId = 8;
            var productNumber = 1;
            var nettoSum = 0f;
            foreach (var orderedProduct in products)
            {
                worksheet.Cells[$"A{rowId}"].LoadFromText(productNumber.ToString());
                worksheet.Cells[$"B{rowId}"].LoadFromText(orderedProduct.Name);
                worksheet.Cells[$"C{rowId}"].LoadFromText(orderedProduct.Price.ToString());
                worksheet.Cells[$"D{rowId}"].LoadFromText(orderedProduct.Count.ToString());
                var nettoPrice = orderedProduct.Count * orderedProduct.Price;
                worksheet.Cells[$"E{rowId}"].LoadFromText(nettoPrice.ToString());
                worksheet.Cells[$"F{rowId}"].LoadFromText((nettoPrice * 1.23f).ToString());
                
                nettoSum += nettoPrice;
                
                rowId++;
                productNumber++;
            }

            rowId++;
            worksheet.Cells[$"E{rowId}"].LoadFromText("Suma netto");
            worksheet.Cells[$"F{rowId}"].LoadFromText("Suma brutto");
            rowId++;
            worksheet.Cells[$"E{rowId}"].LoadFromText(nettoSum.ToString());
            worksheet.Cells[$"F{rowId}"].LoadFromText((nettoSum * 1.23f).ToString());
            
            package.Save();

            return memoryStream;
        }

        private static void AddProductListHeaders(ExcelWorksheet? worksheet)
        {
            worksheet.Cells["A7"].LoadFromText("Produkty");
            worksheet.Cells["B7"].LoadFromText("Nazwa");
            worksheet.Cells["C7"].LoadFromText("Cena jednostkowa");
            worksheet.Cells["D7"].LoadFromText("Ilość");
            worksheet.Cells["E7"].LoadFromText("Suma netto");
            worksheet.Cells["F7"].LoadFromText("Suma brutto");
        }

        private static void AddBuyerDataToSheet(ExcelWorksheet? worksheet, User? buyer)
        {
            worksheet.Cells["A1"].LoadFromText("Kupujący");
            worksheet.Cells["A2"].LoadFromText("Imie:");
            worksheet.Cells["A3"].LoadFromText("Nazwisko:");
            worksheet.Cells["A4"].LoadFromText("Adres:");

            worksheet.Cells["B2"].LoadFromText(buyer.Name);
            worksheet.Cells["B3"].LoadFromText(buyer.Surname);
            worksheet.Cells["B4"]
                .LoadFromText(
                    $"{buyer.PrimaryAddress.Country} {buyer.PrimaryAddress.City} {buyer.PrimaryAddress.Postcode} {buyer.PrimaryAddress.Street} {buyer.PrimaryAddress.Number}");
        }

        private static void AddWorkerDataToSheet(ExcelWorksheet? worksheet, User? worker)
        {
            worksheet.Cells["C1"].LoadFromText("Sprzedający");
            worksheet.Cells["C2"].LoadFromText("Imie:");
            worksheet.Cells["C3"].LoadFromText("Nazwisko:");
            worksheet.Cells["C4"].LoadFromText("Identyfikator:");

            worksheet.Cells["D2"].LoadFromText(worker.Name);
            worksheet.Cells["D3"].LoadFromText(worker.Surname);
            worksheet.Cells["D4"].LoadFromText(worker.Id.ToString());
        }
    }
}