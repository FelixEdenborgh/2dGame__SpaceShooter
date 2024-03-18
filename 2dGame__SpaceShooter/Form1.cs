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
        WindowsMediaPlayer explosion;

        PictureBox[] stars;
        int backgroundspeed;
        Random rnd;
        int playerSpeed;

        PictureBox[] munitions; //ammo
        int MunitionSpeed;

        PictureBox[] enemies;
        int enemiSpeed;

        PictureBox[] enemiesMunition;
        int enemiesMuntitionSpeed;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundspeed = 4;
            playerSpeed = 4;
            enemiSpeed = 4;
            enemiesMuntitionSpeed = 4;

            MunitionSpeed = 20;
            munitions = new PictureBox[3];
            // Load Images för ammo
            Image munition = Image.FromFile(@"C:\Users\FelixEdenborgh\source\repos\CSharp\2dGame__SpaceShooter\2dGame__SpaceShooter\bin\Debug\asserts\munition.png");

            //Enemys
            //Load Images for Enemys
            Image enemi1 = Image.FromFile("asserts\\E1.png");
            Image enemi2 = Image.FromFile("asserts\\E2.png");
            Image enemi3 = Image.FromFile("asserts\\E3.png");
            Image boss1 = Image.FromFile("asserts\\Boss1.png");
            Image boss2 = Image.FromFile("asserts\\Boss2.png");

            enemies = new PictureBox[10];

            //Initalise EnemyPictureBoxes
            for(int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new PictureBox();
                enemies[i].Size = new Size(40, 40);
                enemies[i].SizeMode = PictureBoxSizeMode.Zoom;
                enemies[i].BorderStyle = BorderStyle.None;
                enemies[i].Visible = false;
                this.Controls.Add(enemies[i]);
                enemies[i].Location = new Point((i + 1) * 50, -50);
            }

            enemies[0].Image = boss1;
            enemies[1].Image = enemi2;
            enemies[2].Image = enemi3;
            enemies[3].Image = enemi3;
            enemies[4].Image = enemi1;
            enemies[5].Image = enemi3;
            enemies[6].Image = enemi2;
            enemies[7].Image = enemi3;
            enemies[8].Image = enemi2;
            enemies[9].Image = boss2;



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
            explosion = new WindowsMediaPlayer();

            // Load all songs
            gameMedia.URL = "songs\\GameSong.mp3";
            shootgMedia.URL = "songs\\shoot.mp3";
            explosion.URL = "songs\\boom.mp3";

            // Setup Songs Settings
            gameMedia.settings.setMode("loop", true);
            gameMedia.settings.volume = 5;
            shootgMedia.settings.volume = 1;
            explosion.settings.volume = 6;



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

            // Enemies Munition
            enemiesMunition = new PictureBox[10];

            for(int i = 0; i < enemiesMunition.Length; i++)
            {
                enemiesMunition[i] = new PictureBox();
                enemiesMunition[i].Size = new Size(2, 25);
                enemiesMunition[i].Visible = false;
                enemiesMunition[i].BackColor = Color.Yellow;
                int x = rnd.Next(0, 10);
                // skötter hur skotten ska gå ifrån fienderna -20 blir i mitten av fienden
                enemiesMunition[i].Location = new Point(enemies[x].Location.X, enemies[x].Location.Y - 20);
                this.Controls.Add(enemiesMunition[i]);
            }

            gameMedia.controls.play();

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

                    Collision();
                }
                else
                {
                    munitions[i].Visible = false;
                    munitions[i].Location = new Point(Player.Location.X + 20, Player.Location.Y - i * 30);
                }
            }
        }

        private void MoveEnemysTimer_Tick(object sender, EventArgs e)
        {
            MoveEnemies(enemies, enemiSpeed);
        }

        private void MoveEnemies(PictureBox[] array, int speed)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Visible = true;
                array[i].Top += speed;

                if (array[i].Top > this.Height)
                {
                    array[i].Location = new Point((i + 1) * 50, -200);
                }
            }
        }

        public void Collision()
        {
            for(int i = 0; i < enemies.Length; i++)
            {
                if (munitions[0].Bounds.IntersectsWith(enemies[i].Bounds) 
                    || munitions[1].Bounds.IntersectsWith(enemies[i].Bounds) || munitions[2].Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.controls.play();
                    enemies[i].Location = new Point((i + 1)*50, -100);
                }

                if (Player.Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.settings.volume = 30;
                    explosion.controls.play();
                    Player.Visible = false;
                    Gameover("");
                }
            }
        }

        private void Gameover(String str)
        {
            gameMedia.controls.stop();
            StopTimers();
        }

        private void StopTimers()
        {
            MoveBgTimer.Stop();
            MoveEnemysTimer.Stop();
            MoveMunitionTimer.Stop();
            EnemysMutionTimer.Stop();
        }

        private void StartTimer()
        {
            MoveBgTimer.Start();
            MoveEnemysTimer.Start();
            MoveMunitionTimer.Start();
            EnemysMutionTimer.Start();
        }

        private void EnemysMutionTimer_Tick(object sender, EventArgs e)
        {
            for(int i = 0; i < munitions.Length; i++)
            {
                if (enemiesMunition[i].Top < this.Height)
                {
                    enemiesMunition[i].Visible = true;
                    enemiesMunition[i].Top += enemiesMuntitionSpeed;

                    CollisionWithEnemisMunition();
                }
                else
                {
                    enemiesMunition[i].Visible = false;
                    int x = rnd.Next(0, 10);
                    enemiesMunition[i].Location = new Point(enemies[x].Location.X + 20, enemies[x].Location.Y + 30);
                }
            }
        }

        private void CollisionWithEnemisMunition()
        {
            for(int i = 0; i < enemiesMunition.Length; i++)
            {
                if (enemiesMunition[i].Bounds.IntersectsWith(Player.Bounds))
                {
                    enemiesMunition[i].Visible = false;
                    explosion.settings.volume = 30;
                    explosion.controls.play();
                    Player.Visible = false;
                    Gameover("Game Over");
                }
            }
        }
    }
}
