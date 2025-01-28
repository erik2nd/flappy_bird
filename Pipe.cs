using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flappy_Bird
{
    class Pipe
    {
        public int x;
        public int y;

        public int width;
        public int height;

        public Image pipe_img;
        public Rectangle rec;

        public Pipe(int x, int y, bool isRotatedImage = false)
        {
            this.x = x;
            this.y = y;
            width = 70;
            height = 250;
            rec = new Rectangle(x, y, width, height - 5);
            pipe_img = new Bitmap(Properties.Resources.pipe);
            if (isRotatedImage)
                pipe_img.RotateFlip(RotateFlipType.Rotate180FlipX);
        }
    }
}