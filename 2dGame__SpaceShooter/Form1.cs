using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace _2dGame__SpaceShooter
{
    public partial class Form1 : Form
    {
        // Media
        WindowsMediaPlayer gameMedia;
        WindowsMediaPlayer shootgMedia;

        PictureBox[] stars;
        int backgroundspeed;
        Random rnd;
        int playerSpeed;

        PictureBox[] munitions; //ammo
        int MunitionSpeed;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundspeed = 4;
            playerSpeed = 4;

            MunitionSpeed = 20;
            munitions = new PictureBox[3];
            // Load Images för ammo
            Image munition = Image.FromFile(@"C:\Users\FelixEdenborgh\source\repos\CSharp\2dGame__SpaceShooter\2dGame__SpaceShooter\bin\Debug\asserts\munition.png");


            for (int i = 0; i < munitions.Length; i++)
            {
                munitions[i] = new PictureBox();
                munitions[i].Size = new Size(8, 8);
                munitions[i].Image = munition;
                munitions[i].SizeMode = PictureBoxSizeMode.Zoom;
                munitions[i].BorderStyle = BorderStyle.None;
                this.Controls.Add(munitions[i]);
            }

            // Creat WMP
            gameMedia = new WindowsMediaPlayer();
            shootgMedia = new WindowsMediaPlayer();

            // Load all songs
            gameMedia.URL = "songs\\GameSong.mp3";
            shootgMedia.URL = "songs\\shoot.mp3";

            // Setup Songs Settings
            gameMedia.settings.setMode("loop", true);
            gameMedia.settings.volume = 5;
            shootgMedia.settings.volume = 1;

            // Star
            stars = new PictureBox[15];
            rnd = new Random();

            // När spelet startar
            gameMedia.controls.play();

            for(int i = 0; i < stars.Length; i++)
            {
                // lägger stjärnor i Pictureboxen
                stars[i] = new PictureBox();
                stars[i].BorderStyle = BorderStyle.None;
                // Ger dem sedan en random location på kartan
                stars[i].Location = new Point(rnd.Next(20, 500), rnd.Next(-10, 400));
                if(i % 2 == 1)
                {
                    stars[i].Size = new Size(2, 2);
                    stars[i].BackColor = Color.Wheat;
                }
                else
                {
                    stars[i].Size = new Size(3, 3);
                    stars[i].BackColor = Color.DarkGray;
                }

                this.Controls.Add(stars[i]);
            }
        }

        // skötter timern
        private void MoveBgTimer_Tick(object sender, EventArgs e)
        {
            // Några stjärnor rör sig snabbare en andra
            for(int i = 0; i < stars.Length/2;i++) 
            {
                stars[i].Top += backgroundspeed;

                // Kollar så att när de når slutet av skärmen att de startar om uppeifrån igen
                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;
                }
            }

            // Dess stärnor är lite segare
            for(int i = stars.Length / 2; i < stars.Length;i++)
            {
                stars[i].Top += backgroundspeed -2;

                // Kollar så att när de når slutet av skärmen att de startar om uppeifrån igen
                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;
                }
            }
        }


        // Skötter Playern
        private void LeftMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Left > 10)
            {
                Player.Left -= playerSpeed;
            }
        }

        private void RightMoveTimer_Tick(object sender, EventArgs e)
        {
            if(Player.Right < 580)
            {
                Player.Left += playerSpeed;
            }
        }

        private void DownMoveTimer_Tick(object sender, EventArgs e)
        {
            if(Player.Top < 400)
            {
                Player.Top += playerSpeed;
            }
        }

        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            if(Player.Top > 10)
            {
                Player.Top -= playerSpeed;
            }
        }

        // Kollar om spelaren har tryckt på någon av knapparna och startar timern i såfall
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Right)
            {
                RightMoveTimer.Start();
            }
            if(e.KeyCode == Keys.Left)
            {
                LeftMoveTimer.Start();
            }
            if(e.KeyCode == Keys.Down)
            {
                DownMoveTimer.Start();
            }
            if(e.KeyCode == Keys.Up)
            {
                UpMoveTimer.Start();
            }

        }

        // Kollar om spelaren har släppt knappen
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            RightMoveTimer.Stop();
            LeftMoveTimer.Stop();
            DownMoveTimer.Stop();
            UpMoveTimer.Stop();
        }

        // ammo handler
        // En simple forloop som går igenom ammo arrayn, när ammon går i topen av skärmen så går de tillbaka till där Playern är och börjar om
        // Det kommer fortsätta så länge timmern spelas
        private void MoveMunitionTimer_Tick(object sender, EventArgs e)
        {
            // Ljudet för skotten
            shootgMedia.controls.play();


            for(int i = 0; i < munitions.Length; i++)
            {
                if (munitions[i].Top > 0)
                {
                    munitions[i].Visible = true;
                    munitions[i].Top -= MunitionSpeed;
                }
                else
                {
                    munitions[i].Visible = false;
                    munitions[i].Location = new Point(Player.Location.X + 20, Player.Location.Y - i * 30);
                }
            }
        }
    }
}
