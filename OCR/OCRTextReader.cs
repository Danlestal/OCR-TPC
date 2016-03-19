

using System;
using Tesseract;

namespace OCR
{
    public class OCRTextReader
    {
        public OcrData Read(string filepath)
        {
            OcrData result = null;
            using (var engine = new TesseractEngine(@".\tessdata", "spa", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(filepath))
                {
                    using (var page = engine.Process(img))
                    {
                        result = new OcrData(page.GetText(), page.GetMeanConfidence());
                    }
                }
            }

            return result;
        }
      
    }
}
