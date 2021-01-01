using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LectureImage
{
    class Pixel
    {
        private int[] pixels;

        public Pixel(int[] pixels)
        {
            this.pixels = new int[3];
            for (int i = 0; i < pixels.Length; i++)
            {
                this.pixels[i] = pixels[i];
            }
        }

        public int[] Pixels
        {
            get { return pixels; }  //accès en lecture
            set { pixels = value; } //accès en écriture
        }

    }
}