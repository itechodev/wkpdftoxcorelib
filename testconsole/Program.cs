﻿
using System;
using System.Text;
using System.Threading.Tasks;
using ItechoPdf;

namespace testconsole
{
    class Program
    {
        static void Create(int i)
        {
            var renderer = new PdfRenderer(settings => {
                // Set global settings for all documents rendered through this service
                settings.DPI = 300;
                settings.Margins.Set(0, 0, 0, 0,Unit.Millimeters);
            });
            
            Console.WriteLine("WkHTML version:" + renderer.GetVersion());

            var cover = renderer.AddDocument(PdfSource.FromFile("res/cover.html"));
            
            var content = renderer.AddDocument(PdfSource.FromHtml($"This PDF is created using thread #{i}"));
            
            content.SetHeader(PdfSource.FromFile("res/header.html"), 25, 5);
            content.SetFooter(PdfSource.FromFile("res/footer.html"), 25, 5);
            
            renderer.RenderToFile($"output-{i}.pdf");
        }

        static void Main(string[] args)
        {
            // Console.WindowWidth;
            // No data is available for encoding 1252
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
      
            Parallel.For(0, 2, i => {
                Create(i);
            });
        }
    }
}

