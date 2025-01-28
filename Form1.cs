using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Flappy_Bird
{
    public partial class Form1 : Form
    {
        Player bird;
        Bot bot;
        Pipe pipe_1;
        Pipe pipe_2;
        Pipe pipe_3;
        Pipe pipe_4;

        bool play = false;
        int SelectedColor = 1;
        bool IsPlayingWithBot = false;
        int frame = 3;

        AudioPlayer sound_player = new AudioPlayer();

        Timer timer2;

        bool keyPressed = false;

        int between_space_1 = 150;
        int between_space_2 = 150;
        bool even = false;
        int score = 0;
        int highest_score = 0;

        float gravity;

        Rectangle border = new Rectangle(0, 0, 816, 488);
        Image background_img = Properties.Resources.background;

        int currentFrame = 1;
        int currentLimit = 24;

        Image yellow_bird_img1 = new Bitmap(Properties.Resources.yellowbird_downflap);
        Image yellow_bird_img2 = new Bitmap(Properties.Resources.yellowbird_midflap);
        Image yellow_bird_img3 = new Bitmap(Properties.Resources.yellowbird_upflap);

        Image red_bird_img1 = new Bitmap(Properties.Resources.redbird_downflap);
        Image red_bird_img2 = new Bitmap(Properties.Resources.redbird_midflap);
        Image red_bird_img3 = new Bitmap(Properties.Resources.redbird_upflap);

        Image blue_bird_img1 = new Bitmap(Properties.Resources.bluebird_downflap);
        Image blue_bird_img2 = new Bitmap(Properties.Resources.bluebird_midflap);
        Image blue_bird_img3 = new Bitmap(Properties.Resources.bluebird_upflap);

        int b1 = 0;
        int b2 = 781;

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            timer1.Interval = 10;
            timer1.Tick += new EventHandler(update);
            timer1.Tick += new EventHandler(background_move);

            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.DrawItem += new DrawItemEventHandler(listBox1_DrawItem);
            listBox1.SelectedIndexChanged += new EventHandler(listBox1_SelectedIndexChanged);

            listBox2.DrawMode = DrawMode.OwnerDrawFixed;
            listBox2.DrawItem += new DrawItemEventHandler(listBox2_DrawItem);
            listBox2.SelectedIndexChanged += new EventHandler(listBox2_SelectedIndexChanged);

            listBox1.SelectedIndex = 0;
            listBox2.SelectedIndex = 0;
            trackBar1.Value = 100;

            timer2 = new Timer();
            timer2.Interval = 1000;
            timer2.Tick += new EventHandler(counting);

            Invalidate();
        }

        void background_move(object sender, EventArgs e)
        {
            if (b1 < -781)
            {
                b1 = 781;
            }
            b1 -= 1;
            b2 -= 1;
            if (b2 < -781)
            {
                b2 = 781;
            }
            Invalidate();
        }

        public void InitializeGame()
        {
            score = 0;
            label8.Text = "Счёт: 0";
            label1.Text = "Счёт: 0";
            bird = new Player(200, 200);
            if (IsPlayingWithBot)
            {
                bot = new Bot(180, 270);
            }
            pipe_1 = new Pipe(443, -100, true);
            pipe_2 = new Pipe(443, 300);
            pipe_3 = new Pipe(886, -50, true);
            pipe_4 = new Pipe(886, 350);

            gravity = 0;

            timer1.Start();
        }

        private void update(object sender, EventArgs e)
        {
            if (Collide(bird, pipe_1) || Collide(bird, pipe_2)
               || Collide(bird, pipe_3) || Collide(bird, pipe_4)
               || !Collide(bird, border))
            {
                sound_player.Play("hit");
                bird.isAlive = false;
                if (IsPlayingWithBot)
                bot.isAlive = false;
                timer1.Stop();
                panel3.Visible = true;
                pictureBox5.Visible = false;
                sound_player.Play("svistun-konec");
            }

            if (IsPlayingWithBot)
            {
                if (Collide(bot, pipe_1) || Collide(bot, pipe_2)
               || Collide(bot, pipe_3) || Collide(bot, pipe_4)
               || !Collide(bot, border))
                {
                    sound_player.Play("hit");
                    bird.isAlive = false;
                    bot.isAlive = false;
                    timer1.Stop();
                    panel2.Visible = true;
                    pictureBox5.Visible = false;
                }
            }

            if (bird.gravity_value != 0.1f)
                bird.gravity_value += 0.005f;
            gravity += bird.gravity_value;
            bird.y += gravity;
            bird.rec.X = bird.x;
            bird.rec.Y = bird.y;
            if (IsPlayingWithBot)
            {
                bot.rec.X = bot.x;
                bot.rec.Y = bot.y;
            }

            if (pipe_1.x + 70 == bird.x + 50 || pipe_1.x + 71 == bird.x + 50 || pipe_1.x + 72 == bird.x + 50)
            {
                sound_player.Play("point");
                even = true;
                score++;
                if (score > highest_score)
                    highest_score = score;
                label1.Text = "Счёт: " + score;
                label8.Text = "Счёт: " + score;
                label9.Text = "Лучший счёт: " + highest_score;
                label2.Text = "Лучший счёт: " + highest_score;
            }
            if (pipe_3.x + 70 == bird.x + 50 || pipe_3.x + 71 == bird.x + 50 || pipe_3.x + 72 == bird.x + 50)
            {
                sound_player.Play("point");
                even = false;
                score++;
                if (score > highest_score)
                    highest_score = score;
                label1.Text = "Счёт: " + score;
                label8.Text = "Счёт: " + score;
                label9.Text = "Лучший счёт: " + highest_score;
                label2.Text = "Лучший счёт: " + highest_score;
            }
            if (IsPlayingWithBot)
            {
                if (even)
                {
                    if (bot.y < pipe_3.y + 250 + between_space_2 / 2 - 17)
                        bot.y += 0.3f;
                    if (bot.y > pipe_3.y + 250 + between_space_2 / 2 - 17)
                        bot.y -= 0.3f;
                }
                else
                {
                    if (bot.y < pipe_1.y + 250 + between_space_1 / 2 - 17)
                        bot.y += 0.3f;
                    if (bot.y > pipe_1.y + 250 + between_space_1 / 2 - 17)
                        bot.y -= 0.3f;
                }
            }
            if (SelectedColor == 1)
            {
                if (currentFrame == 1) bird.bird_img = red_bird_img1;
                if (currentFrame == 7) bird.bird_img = red_bird_img2;
                if (currentFrame == 13) bird.bird_img = red_bird_img3;
                if (currentFrame == 19) bird.bird_img = red_bird_img2;

                if (IsPlayingWithBot)
                {
                    if (currentFrame == 1) bot.bird_img = blue_bird_img3;
                    if (currentFrame == 7) bot.bird_img = blue_bird_img2;
                    if (currentFrame == 13) bot.bird_img = blue_bird_img1;
                    if (currentFrame == 19) bot.bird_img = blue_bird_img2;
                }
            }

            if (SelectedColor == 2)
            {
                if (currentFrame == 1) bird.bird_img = yellow_bird_img1;
                if (currentFrame == 7) bird.bird_img = yellow_bird_img2;
                if (currentFrame == 13) bird.bird_img = yellow_bird_img3;
                if (currentFrame == 19) bird.bird_img = yellow_bird_img2;

                if (IsPlayingWithBot)
                {
                    if (currentFrame == 1) bot.bird_img = red_bird_img3;
                    if (currentFrame == 7) bot.bird_img = red_bird_img2;
                    if (currentFrame == 13) bot.bird_img = red_bird_img1;
                    if (currentFrame == 19) bot.bird_img = red_bird_img2;
                }
            }

            if (SelectedColor == 3)
            {
                if (currentFrame == 1) bird.bird_img = blue_bird_img1;
                if (currentFrame == 7) bird.bird_img = blue_bird_img2;
                if (currentFrame == 13) bird.bird_img = blue_bird_img3;
                if (currentFrame == 19) bird.bird_img = blue_bird_img2;

                if (IsPlayingWithBot)
                {
                    if (currentFrame == 1) bot.bird_img = yellow_bird_img3;
                    if (currentFrame == 7) bot.bird_img = yellow_bird_img2;
                    if (currentFrame == 13) bot.bird_img = yellow_bird_img1;
                    if (currentFrame == 19) bot.bird_img = yellow_bird_img2;
                }
            }

            if (currentFrame < currentLimit)
                currentFrame++;
            else currentFrame = 1;

            if (IsPlayingWithBot)
            {
                if (bird.isAlive && bot.isAlive)
                {
                    MovePipes();
                }
            }
            else if (bird.isAlive)
            {
                MovePipes();
            }

            Invalidate();
        }

        private bool Collide(Player bird, Pipe pipe)
        {
            if (bird.rec.IntersectsWith(pipe.rec))
            {
                return true;
            }
            return false;
        }

        private bool Collide(Bot bot, Pipe pipe)
        {
            if (bot.rec.IntersectsWith(pipe.rec))
            {
                return true;
            }
            return false;
        }

        private bool Collide(Player bird, Rectangle rec)
        {
            if (bird.rec.IntersectsWith(rec))
            {
                return true;
            }
            return false;
        }

        private bool Collide(Bot bot, Rectangle rec)
        {
            if (bot.rec.IntersectsWith(rec))
            {
                return true;
            }
            return false;
        }

        private void CreateNewPipes()
        {
            if (pipe_1.x < bird.x - 270)
            {
                Random r = new Random();
                int y1, y2;
                y1 = r.Next(-150, 0);
                y2 = r.Next(100, 150);
                pipe_1 = new Pipe(816, y1, true);
                pipe_2 = new Pipe(816, y1 + 250 + y2);
                between_space_1 = pipe_2.y - (pipe_1.y + 250);
            }
            if (pipe_3.x < bird.x - 270)
            {
                Random r = new Random();
                int y1, y2;
                y1 = r.Next(-100, -50);
                y2 = r.Next(100, 150);
                pipe_3 = new Pipe(816, y1, true);
                pipe_4 = new Pipe(816, y1 + 250 + y2);
                between_space_2 = pipe_4.y - (pipe_3.y + 250);
            }
        }

        private void MovePipes()
        {
            pipe_1.x -= 3;
            pipe_2.x -= 3;
            pipe_3.x -= 3;
            pipe_4.x -= 3;
            pipe_1.rec.X = pipe_1.x;
            pipe_1.rec.Y = pipe_1.y;
            pipe_2.rec.X = pipe_2.x;
            pipe_2.rec.Y = pipe_2.y;
            pipe_3.rec.X = pipe_3.x;
            pipe_3.rec.Y = pipe_3.y;
            pipe_4.rec.X = pipe_4.x;
            pipe_4.rec.Y = pipe_4.y;
            CreateNewPipes();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            graphics.DrawImage(background_img, b1, 0);
            graphics.DrawImage(background_img, b2, 0);

            if (play)
            {
                graphics.DrawImage(pipe_1.pipe_img, pipe_1.x, pipe_1.y, pipe_1.width, pipe_1.height);
                graphics.DrawImage(pipe_2.pipe_img, pipe_2.x, pipe_2.y, pipe_2.width, pipe_2.height);
                graphics.DrawImage(pipe_3.pipe_img, pipe_3.x, pipe_3.y, pipe_3.width, pipe_3.height);
                graphics.DrawImage(pipe_4.pipe_img, pipe_4.x, pipe_4.y, pipe_4.width, pipe_4.height);
                if (IsPlayingWithBot)
                {
                    graphics.DrawImage(bot.bird_img, bot.x, bot.y, bot.width, bot.height);
                }
                graphics.DrawImage(bird.bird_img, bird.x, bird.y, bird.width, bird.height);

                //graphics.DrawRectangle(Pens.Red, bird.x+5, bird.y+5, bird.width-10, bird.height-10);
                //graphics.DrawRectangle(Pens.Red, bot.x + 5, bot.y + 5, bot.width - 10, bot.height - 10);
                //graphics.DrawRectangle(Pens.Red, pipe_1.rec);
                //graphics.DrawRectangle(Pens.Red, pipe_2.rec);
            }
        }

        public class AudioData
        {
            public WaveOutEvent outputDevice;
            public AudioFileReader audioFile;
            public bool repeat = false;
            public bool manual = false;
            public AudioData(string file, bool repeat = false)
            {
                this.repeat = repeat;
                this.manual = repeat;
                outputDevice = new WaveOutEvent();
                audioFile = new AudioFileReader(file);
                outputDevice.Init(audioFile);
                outputDevice.PlaybackStopped += OutputDevice_PlaybackStopped;
            }

            private void OutputDevice_PlaybackStopped(object sender, StoppedEventArgs e)
            {
                audioFile.CurrentTime = new TimeSpan(0);
                if (repeat && manual)
                {
                    outputDevice.Play();
                }
            }
        }
        public class AudioPlayer
        {
            public Dictionary<string, AudioData> sounds = new Dictionary<string, AudioData>();

            public AudioPlayer()
            {
                string path = Directory.GetCurrentDirectory();
                DirectoryInfo info = new DirectoryInfo(path);
                info = info.Parent.Parent.Parent;
                path = info.FullName + "\\Flappy Bird\\sounds";
                foreach (string file in Directory.GetFiles(path))
                {
                    string[] parts = Path.GetFileNameWithoutExtension(file).Split('_');
                    sounds.Add(parts[0], new AudioData(file, parts.Length > 1));
                }
            }
            public void Play(string file_name)
            {
                sounds[file_name].manual = true;
                sounds[file_name].audioFile.CurrentTime = new TimeSpan(0);
                sounds[file_name].outputDevice.Play();
            }
            public void Stop(string file_name)
            {
                sounds[file_name].manual = false;
                sounds[file_name].outputDevice.Stop();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (!keyPressed)
                {
                    button1.PerformClick();
                    keyPressed = true;
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                keyPressed = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsPlayingWithBot)
            {
                if (bird.isAlive && bot.isAlive)
                {
                    gravity = 0;
                    bird.gravity_value = -0.125f;
                    sound_player.Play("wing");
                }
            }
            else if (bird.isAlive)
            {
                gravity = 0;
                bird.gravity_value = -0.125f;
                sound_player.Play("wing");
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            InitializeGame();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            sound_player.Play("back");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            bird.isAlive = false;
            if (IsPlayingWithBot)
            bot.isAlive = false;
            timer1.Stop();
            Invalidate();
            play = false;
            pictureBox4.Visible = false;
            pictureBox5.Visible = false;
            panel1.Visible = true;
            pictureBox8.Visible = true;
            label1.Visible = false;
            button1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            pictureBox7.Visible = false;
        }

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            pictureBox4.Image = Properties.Resources.home_dark;
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.Image = Properties.Resources.home;
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            int index = e.Index;
            Graphics g = e.Graphics;
            foreach (int selectedIndex in this.listBox1.SelectedIndices)
            {
                if (index == selectedIndex)
                {
                    e.DrawBackground();
                    g.FillRectangle(new SolidBrush(Color.Firebrick), e.Bounds);
                }
            }

            Font font = listBox1.Font;
            Color colour = listBox1.ForeColor;
            string text = listBox1.Items[index].ToString();

            g.DrawString(text, font, new SolidBrush(Color.Black), (float)e.Bounds.X, (float)e.Bounds.Y);
            e.DrawFocusRectangle();
        }

        private void listBox2_DrawItem(object sender, DrawItemEventArgs e)
        {
            int index = e.Index;
            Graphics g = e.Graphics;
            foreach (int selectedIndex in listBox2.SelectedIndices)
            {
                if (index == selectedIndex)
                {
                    e.DrawBackground();
                    g.FillRectangle(new SolidBrush(Color.Firebrick), e.Bounds);
                }
            }
            Font font = listBox2.Font;
            Color colour = listBox2.ForeColor;
            string text = listBox2.Items[index].ToString();

            g.DrawString(text, font, new SolidBrush(Color.Black), (float)e.Bounds.X, (float)e.Bounds.Y);
            e.DrawFocusRectangle();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Invalidate();
            if (listBox1.SelectedIndex == 0)
                SelectedColor = 1;
            if (listBox1.SelectedIndex == 1)
                SelectedColor = 2;
            if (listBox1.SelectedIndex == 2)
                SelectedColor = 3;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Invalidate();
            if (listBox2.SelectedIndex == 0)
                IsPlayingWithBot = false;
            if (listBox2.SelectedIndex == 1)
                IsPlayingWithBot = true;
        }

        private void pictureBox5_MouseHover(object sender, EventArgs e)
        {
            pictureBox5.Image = Properties.Resources.pause_dark;
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            pictureBox5.Image = Properties.Resources.pause;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Countdown();
            pictureBox8.Visible = false;
        }

        private void Countdown()
        {
            timer2.Start();
            pictureBox9.Visible = true;
            panel1.Visible = false;
        }

        private void counting(object sender, EventArgs e)
        {
            if (frame == 0)
            {
                frame = 3;
                timer2.Stop();
                pictureBox9.Visible = false;
                InitializeGame();
                play = true;
                panel1.Visible = false;
                label1.Visible = true;
                button1.Visible = true;
                pictureBox4.Visible = true;
                pictureBox5.Visible = true;
            }
            if (frame == 3) pictureBox9.Image = Properties.Resources._3;
            if (frame == 2) pictureBox9.Image = Properties.Resources._2;
            if (frame == 1) pictureBox9.Image = Properties.Resources._1;
            frame--;
            Invalidate();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (pictureBox7.Visible == false)
            {
                timer1.Stop();
                pictureBox7.Visible = true;
            }
            else
            {
                timer1.Start();
                pictureBox7.Visible = false;
            }
        }

        private void pictureBox8_MouseHover(object sender, EventArgs e)
        {
            pictureBox8.Image = Properties.Resources.settings_dark;
        }

        private void pictureBox8_MouseLeave(object sender, EventArgs e)
        {
            pictureBox8.Image = Properties.Resources.settings;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (groupBox1.Visible == true)
            {
                groupBox1.Visible = false;
                panel1.Visible = true;
            }
            else
            {
                groupBox1.Visible = true;
                panel1.Visible = false;
            }
        }

        private void pictureBox6_MouseHover(object sender, EventArgs e)
        {
            pictureBox6.Image = Properties.Resources.play_dark;
        }

        private void pictureBox6_MouseLeave(object sender, EventArgs e)
        {
            pictureBox6.Image = Properties.Resources.play;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            float volume = trackBar1.Value / 100f;
            sound_player.sounds["back"].outputDevice.Volume = volume;
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            panel1.Visible = true;
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            timer1.Start();
            pictureBox7.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            pictureBox5.Visible = true;
            InitializeGame();
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            pictureBox5.Visible = true;
            InitializeGame();
        }

        private void pictureBox12_MouseHover(object sender, EventArgs e)
        {
            pictureBox12.Image = Properties.Resources.retry_dark;
        }

        private void pictureBox12_MouseLeave(object sender, EventArgs e)
        {
            pictureBox12.Image = Properties.Resources.retry;
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.retry_dark;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.retry;
        }
    }
}
