using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR
{
    public class BrightnessFilter : IImageFilter
    {

        private float brightnessLimit;

        public BrightnessFilter(float brightnessLimit)
        {
            this.brightnessLimit = brightnessLimit;
        }

        public Image ApplyFilter(Image image)
        {
            Bitmap myBitmap = new Bitmap(image);

            for (int i = 0; i < myBitmap.Width; i++)
            {
                for (int j = 0; j < myBitmap.Height; j++)
                {
                    Color c = myBitmap.GetPixel(i, j);
                    if (c.GetBrightness() > brightnessLimit)
                    {
                        myBitmap.SetPixel(i, j, Color.White);
                    }
                }
            }

            return myBitmap;
        }
    }
}
