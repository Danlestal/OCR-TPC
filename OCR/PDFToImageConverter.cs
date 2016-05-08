
using Spire.Pdf;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System;
using System.Collections.Generic;

namespace OCR
{
    public class PDFToImageConverter
    {

        public List<IImageFilter> filters;


        public PDFToImageConverter()
        {
            filters = new List<IImageFilter>();
        }


        public void AddFilter(IImageFilter filter)
        {
            filters.Add(filter);
        }

        public static void ConvertToImage(string inputPath, string outputPath, int resX, int resY, ImageFormat format)
        {

            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException(inputPath);
            }

            var doc = new PdfDocument();
            doc.LoadFromFile(inputPath, FileFormat.PDF);
            Image emf = doc.SaveAsImage(0, resX, resY);
            emf.Save(outputPath, format);
        }


        public void ConvertToImage(int index, string inputPath, string outputPath, int resX, int resY, ImageFormat format)
        {

            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException(inputPath);
            }

            var doc = new PdfDocument();
            doc.LoadFromFile(inputPath, FileFormat.PDF);
            Image emf = doc.SaveAsImage(index, resX, resY);


            foreach (IImageFilter filter in filters)
            {
                emf = filter.ApplyFilter(emf);
            }

            emf.Save(outputPath, format);

        }

    }
}
