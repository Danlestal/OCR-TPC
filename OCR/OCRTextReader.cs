

using System;
using Tesseract;

namespace OCR
{
    public class OCRTextReader
    {
        public void Read(string filepath)
        {
            using (var engine = new TesseractEngine(@"C:\programacion\PecanSoft\OCR-TPC\OCR\Tesseract\langdata\spa\", "es", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(filepath))
                {
                    using (var page = engine.Process(img))
                    {
                        var text = page.GetText();
                        Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());
                        Console.WriteLine("Text (GetText): \r\n{0}", text);
                    }
                }
            }
        }
    }
}
