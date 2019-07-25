using System;
using System.Collections.Generic;
using System.Text;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace Samr.ERP.Core.Services
{
    public class PdfConverterService
    {
        private readonly IConverter _pdfConverter;

        public PdfConverterService(
            IConverter pdfConverter
            )
        {
            _pdfConverter = pdfConverter;
        }
        public byte[] ConvertToPdf(string html)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                //DocumentTitle = "EmployeeCard",
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = html,
                WebSettings = { DefaultEncoding = "utf-8" },
            };
            
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var pdfBytes = _pdfConverter.Convert(pdf);
            return pdfBytes;
        }
    }
}
