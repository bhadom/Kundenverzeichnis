using iText.Kernel.Pdf;
using iText.Kernel.Colors;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using KundenVerzeichnis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using KundenVerzeichnis.Extensions;
using System.Globalization;

namespace KundenVerzeichnis.PDFs
{
    /// <summary>
    /// Contains all methods to generate the pdfs
    /// </summary>
    class BillPDF
    {
        /// <summary>
        /// Generates the pdf for a single bill
        /// </summary>
        /// <param name="notesHelper"></param>
        /// <param name="patientHelper"></param>
        /// <param name="path"></param>
        public static void generateBill(string notesHelper, Patient patientHelper, string path)
        {
            PdfWriter writer = new PdfWriter(path);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph title = new Paragraph($"Rechnung für {patientHelper}").SetFontSize(20);
            document.Add(title);

            Table table = new Table(3, true);
            Cell cell1 = new Cell(1,2).Add(new Paragraph(notesHelper).SetFontSize(12));
            table.AddCell(cell1);
            document.Add(table);
            table.Complete();

            document.Close();
        }

        /// <summary>
        /// Generates the pdf for a monthly bill
        /// </summary>
        /// <param name="treatments"></param>
        /// <param name="month"></param>
        /// <param name="path"></param>
        public static void generateMonth(List<Treatment> treatments, string month, string path) 
        {
            decimal totalSum = treatments.Sum(t => t.Price);

            PdfWriter writer = new PdfWriter(path);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph title = new Paragraph($"Monatsabrechnung {month}").SetFontSize(20);
            document.Add(title);
            Table table = new Table(3, true);
            
            Cell celld = new Cell().Add(new Paragraph("Datum").SetFontSize(15).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.GRAY);
            Cell cellt = new Cell().Add(new Paragraph("Titel").SetFontSize(15).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.GRAY);
            Cell cellp = new Cell().Add(new Paragraph("Preis").SetFontSize(15).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.GRAY);
            table.AddHeaderCell(celld);
            table.AddHeaderCell(cellt);
            table.AddHeaderCell(cellp);

            for (int i = 0; i < treatments.Count; i++) 
            {
                Treatment t = treatments[i];
                Cell cellPrice = new Cell().Add(new Paragraph(Math.Round(t.Price,2).ToString()).SetTextAlignment(TextAlignment.RIGHT));
                Cell cellTitle = new Cell().Add(new Paragraph(t.Title));
                Cell cellDate = new Cell().Add(new Paragraph(t.TreatmentDate.ToString("dd.MM.yyyy")));
                table.AddCell(cellDate);
                table.AddCell(cellTitle);
                table.AddCell(cellPrice);
            }

            document.Add(table);
            table.Complete();

            document.Add(new Paragraph($"Total: {Math.Round(totalSum,2)}"));

            document.Close();
        }

        /// <summary>
        /// Generates the pdf for a yearly bill
        /// </summary>
        /// <param name="treatments"></param>
        /// <param name="year"></param>
        /// <param name="path"></param>
        public static void generateYear(List<Treatment> treatments, int year, string path) 
        {

            PdfWriter writer = new PdfWriter(path);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph title = new Paragraph($"Jahresabrechnung {year}").SetFontSize(20);
            document.Add(title);

            Table table = new Table(2, true);

            Cell cellTitleMonth = new Cell().Add(new Paragraph("Monat").SetFontSize(15).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.GRAY);
            Cell cellTitleSum = new Cell().Add(new Paragraph("Monatstotal").SetFontSize(15).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.GRAY);

            table.AddHeaderCell(cellTitleMonth);
            table.AddHeaderCell(cellTitleSum);

            Dictionary<Byte, Decimal> listMonths = treatments.sumPerMonth(t => t.TreatmentDate, t=>t.Price);
            foreach(KeyValuePair<Byte, Decimal> kbd in listMonths) 
            {
                string month = new DateTime(1999, kbd.Key, 1).ToString("MMMM", CultureInfo.CurrentCulture);
                table.AddCell(new Cell().Add(new Paragraph(month).SetTextAlignment(TextAlignment.LEFT)));
                table.AddCell(new Cell().Add(new Paragraph(Math.Round(kbd.Value, 2).ToString()).SetTextAlignment(TextAlignment.RIGHT)));
            }

            document.Add(table);
            table.Complete();

            decimal totalSum = listMonths.Sum(t => t.Value);
            document.Add(new Paragraph($"Total: {Math.Round(totalSum, 2)}"));

            document.Close();
        }
    }
}
