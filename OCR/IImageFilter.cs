using System.Drawing;

namespace OCR
{
    public interface IImageFilter
    {
        Image ApplyFilter(Image image);
    }
}