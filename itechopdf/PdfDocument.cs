using System;

namespace ItechoPdf
{

    public class PdfDocument
    {
        public PdfSource Source { get; }
        public PdfSettings Settings { get; private set; } = new PdfSettings();

        public HeaderFooter Header { get; private set; }
        public HeaderFooter Footer { get; private set; }

        public PdfDocument(PdfSource source, PdfSettings settings = null)
        {
            Source = source;
            Settings = settings ?? new PdfSettings();
        }

        public void AddStandardHeader(string left, string center, string right, double? spacing = null, bool line = true, int? fontSize = null, string fontName = null)
        {
            Header = new StandardHeaderFooter
            {
                Center = center,
                FontName = fontName,
                FontSize = fontSize,
                Left = left,
                Line = line,
                Right = right,
                Spacing = spacing
            };
        }

        public void SetHeader(PdfSource source, double height, double? spacing = null, bool? line = null)
        {
            SetHeader(new HtmlHeaderFooter(source)
            {   
                Height = height,
                Spacing = spacing,
                Line = line
            });
        }

        public void SetFooter(PdfSource source, double height, double? spacing = null, bool? line = null)
        {
            SetFooter(new HtmlHeaderFooter(source)
            {   
                Height = height,
                Spacing = spacing,
                Line = line
            });
        }
        
        public void SetFooter(string left, string center, string right, int? fontSize = null, string fontName = null)
        {
            SetFooter(new StandardHeaderFooter(left, center, right, fontSize, fontName));
        }
 
        public void SetHeader(string left, string center, string right, int? fontSize = null, string fontName = null)
        {
            SetHeader(new StandardHeaderFooter(left, center, right, fontSize, fontName));
        }

        private void SetHeader(HeaderFooter header)
        {
            Header = header;
        }

        private void SetFooter(HeaderFooter footer)
        {
            Footer = footer;
        }
    
        public void Configure(Action<PdfSettings> settings)
        {
            settings?.Invoke(Settings);
        }
    }
}
