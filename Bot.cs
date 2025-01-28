using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flappy_Bird
{
    public class Bot
    {
        public float x;
        public float y;

        public int width;
        public int height;

        public Image bird_img;
        public RectangleF rec;

        public bool isAlive;

        public Bot(float x, float y)
        {
            this.x = x;
            this.y = y;
            width = 50;
            height = 34;
            rec = new RectangleF(x + 5, y + 5, width - 10, height - 10);
            isAlive = true;
            bird_img = new Bitmap(Properties.Resources.bluebird_midflap);
        }
    }
}
