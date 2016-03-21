namespace OCR
{
    public class OcrData
    {

        public OcrData(string text, float confidence)
        {
            Text = text;
            Confidence = confidence;
        }

        public string Text { get; private set; }
        public float Confidence { get; private set; }
    }
}