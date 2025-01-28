using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flappy_Bird
{
    public class Player
    {
        public float x;
        public float y;

        public int width;
        public int height;

        public float gravity_value;

        public Image bird_img;
        public RectangleF rec;

        public bool isAlive;

        public Player(float x, float y)
        {
            this.x = x;
            this.y = y;
            width = 50;
            height = 34;
            rec = new RectangleF(x + 5, y + 5, width - 10, height - 10);
            gravity_value = 0.1f;
            isAlive = true;
            bird_img = new Bitmap(Properties.Resources.redbird_midflap);
        }
    }
}
