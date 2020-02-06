﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using wkpdftoxcorelib.Core;

namespace wkpdftoxcorelib.Wrapper
{
    public class WkHtmlToPdfWrapper
    {
        private List<string> _tempFiles = new List<string>();

        public LoadSettings LoadSettings { get; } = new LoadSettings();
        public PrintSettings PrintSettings { get; } = new PrintSettings();

        public string GetVersion()
        {
            return WkHtmlToPdf.GetVersion() + (WkHtmlToPdf.ExtendedQt() ? " (Extended QT)" : "");
        }

        public PdfDocument HtmlFileToPdf(string filename)
        {
            return HtmlToPdf(File.ReadAllText(filename));
        }

        public PdfDocument HtmlToPdf(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            FormatHtml(htmlDoc, Environment.CurrentDirectory);    
            using (var sw = new StringWriter())
            {
                htmlDoc.Save(sw);
                var bytes = WkHtmlToPdf.HtmlToPdf(System.Text.Encoding.UTF8.GetBytes(sw.ToString()), ConvertToCoreSettings(LoadSettings, PrintSettings));
                return new PdfDocument(bytes);
            }
        }

        private WkHtmlToPdfSettings ConvertToCoreSettings(LoadSettings loadSettings, PrintSettings printSettings)
        {
            // // 25mm header + 10mm spacing + 1mm margin top
            // // Set margins. Header and footers may affect marings
            // if (PrintSettings.Margins.Top.HasValue)
            // {
            //     if (PrintSettings.Header?.Height == null)
            //     {
            //         throw new Exception("Header height should be explicit when margin top is explicit.");
            //     }
            //     double value = PrintSettings.Margins.Top.Value + (PrintSettings.Header.Spacing ?? 0) + PrintSettings.Header.Height.Value;
            //     GlobalSetting(globalSettings, "margin.top", PrintSettings.Margins.GetMarginValue(value));
            // }

            // if (PrintSettings.Margins.Bottom.HasValue)
            // {
            //     if (PrintSettings.Footer?.Height == null)
            //     {
            //         throw new Exception("Footer height should be explicit when margin bottom is explicit.");
            //     }
            //     double value = PrintSettings.Margins.Bottom.Value + (PrintSettings.Footer.Spacing ?? 0) + PrintSettings.Footer.Height.Value;
            //     GlobalSetting(globalSettings, "margin.bottom", PrintSettings.Margins.GetMarginValue(value));
            // }

            // var htmlDoc = new HtmlDocument();

            // if (settings is HtmlHeaderFooter html)
            // {    
            //     htmlDoc.LoadHtml(html.Html);
            // }

            // if (settings is FileHeaderFooter file)
            // {
            //     htmlDoc.Load(file.FilePath);
            // }

            // FormatHtml(htmlDoc, Environment.CurrentDirectory);    
            // var path = CreateTempFile();
            // htmlDoc.Save(path);
            // ObjectSetting(objectSettings, prefix + ".htmlUrl", path)
            
            
            return new WkHtmlToPdfSettings
            {
                BlockLocalFileAccess = loadSettings.BlockLocalFileAccess,
                DebugJavascript = loadSettings.DebugJavascript,
                JSDelay = LoadSettings.JSDelay,
                LoadErrorHandling = LoadSettings.LoadErrorHandling?.ToString(),
                Password = LoadSettings.Password,
                Proxy = LoadSettings.Proxy,
                StopSlowScript = LoadSettings.StopSlowScript,
                Username = LoadSettings.Username,
                
                Collate = printSettings.Collate,
                ColorMode = printSettings.ColorMode?.ToString(),
                CookieJar = printSettings.CookieJar,
                Copies = printSettings.Copies,
                DefaultEncoding = printSettings.DefaultEncoding,
                DocumentTitle = printSettings.DocumentTitle,
                DPI = printSettings.DPI,
                DumpOutline = printSettings.DumpOutline,
                EnableIntelligentShrinking = printSettings.EnableIntelligentShrinking,
                EnableJavascript = printSettings.EnableJavascript,
                Footer = null,
                Header = null,
                ImageDPI = printSettings.ImageDPI,
                ImageQuality = PrintSettings.ImageQuality,
                IncludeInOutline = PrintSettings.IncludeInOutline,
                LoadImages = PrintSettings.LoadImages,
                MarginBottom = PrintSettings.Margins.GetMarginValue(PrintSettings.Margins.Bottom),
                MarginLeft = PrintSettings.Margins.GetMarginValue(PrintSettings.Margins.Left),
                MarginRight = PrintSettings.Margins.GetMarginValue(PrintSettings.Margins.Right),
                MarginTop = PrintSettings.Margins.GetMarginValue(PrintSettings.Margins.Top),
                MinimumFontSize = PrintSettings.MinimumFontSize,
                Orientation = printSettings.Orientation?.ToString(),
                Outline = printSettings.Outline,
                OutlineDepth = printSettings.OutlineDepth,
                PageOffset = printSettings.PageOffset,
                PagesCount = printSettings.PagesCount,
                PaperHeight = printSettings.PaperSize.Height,
                PaperWidth = printSettings.PaperSize.Width,
                PaperSize = null,
                PrintBackground = PrintSettings.PrintBackground,
                PrintMediaType = PrintSettings.PrintMediaType,
                ProduceForms = PrintSettings.ProduceForms,
                UseCompression = PrintSettings.UseCompression,
                UseExternalLinks = PrintSettings.UseExternalLinks,
                UseLocalLinks = PrintSettings.UseExternalLinks,
            };
        }

        private string CreateTempFile()
        {
            // For some reason it should end in html
            var path = Path.GetTempFileName() + ".html";
            // Keep reference to file can later delete it
            _tempFiles.Add(path);
            return path;
        }

         private void FormatHtml(HtmlDocument htmlDoc, string baseUrl)
        {
            // Fixed paths for resources
            FixedPath(htmlDoc, baseUrl, "//img", "src");
            
            // Add Doctype
            // Add javascript for injection
        }

        private void FixedPath(HtmlDocument htmlDoc, string baseUrl, string xpath, string attribute)
        {
            var nodes = htmlDoc.DocumentNode.SelectNodes(xpath);
            if (nodes == null || nodes.Count == 0)
            {
                return;
            }
            foreach (var node in nodes)
            {
                var attrValue = node.GetAttributeValue(attribute, null);
                if (String.IsNullOrEmpty(attrValue))
                {
                    continue;
                }
                string newValue = FormatUrl(attrValue, baseUrl);
                if (newValue != attrValue)
                {
                    node.SetAttributeValue(attribute, newValue);
                }
            }
        }

        private string FormatUrl(string url, string baseUrl)
        {
            // Check for absolute etc.
            return Path.Join(baseUrl, url);
        }


    }
}
