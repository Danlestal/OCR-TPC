
using Spire.Pdf;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace OCR
{
    public class PDFToImageConverter
    {

        public static void ConvertToImage(string inputPath, string outputPath)
        {

            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException(inputPath);
            }

            var doc = new PdfDocument();
            doc.LoadFromFile(inputPath);

            Image emf = doc.SaveAsImage(0, Spire.Pdf.Graphics.PdfImageType.Metafile);
            emf.Save(outputPath, ImageFormat.Png);
        }
    }
}
