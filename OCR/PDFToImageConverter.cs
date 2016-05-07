
using Spire.Pdf;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System;

namespace OCR
{
    public class PDFToImageConverter
    {

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


        public static void ConvertToImage(int index, string inputPath, string outputPath, int resX, int resY, ImageFormat format)
        {

            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException(inputPath);
            }

            var doc = new PdfDocument();
            doc.LoadFromFile(inputPath, FileFormat.PDF);
            Image emf = doc.SaveAsImage(index, resX, resY);

            Bitmap myBitmap = new Bitmap(emf);
            const float limit = 0.35f;
            for (int i = 0; i < myBitmap.Width; i++)
            {
                for (int j = 0; j < myBitmap.Height; j++)
                {
                    Color c = myBitmap.GetPixel(i, j);
                    if (c.GetBrightness() > limit)
                    {
                        myBitmap.SetPixel(i, j, Color.White);
                    }
                }
            }

            myBitmap.Save(outputPath, format);

        }


        public static void ConvertToImage(string inputPath, int resX, int resY, ImageFormat format)
        {
            string outputPath = Path.GetFileNameWithoutExtension(inputPath);
            string extension = GetImageExtension(format);

            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException(inputPath);
            }

            var doc = new PdfDocument();
            doc.LoadFromFile(inputPath, FileFormat.PDF);

            for(int pageIndex = 0; pageIndex < doc.Pages.Count; pageIndex++)
            { 
                Image emf = doc.SaveAsImage(pageIndex, resX, resY);
                emf.Save(outputPath + "-"+pageIndex + "." + extension, format);
            }
        }

        private static string GetImageExtension(ImageFormat format)
        {
            return "png";
        }
    }
}
