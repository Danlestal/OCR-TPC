
using Spire.Pdf;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

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


            //Image emf = doc.SaveAsImage(0, Spire.Pdf.Graphics.PdfImageType.Metafile);
            emf.Save(outputPath, format);
        }
    }
}
