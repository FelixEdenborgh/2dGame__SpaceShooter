using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2dGame__SpaceShooter
{
    public partial class Form1 : Form
    {
        PictureBox[] stars;
        int backgroundspeed;
        Random rnd;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundspeed = 4;
            stars = new PictureBox[10];
            rnd = new Random();

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
    }
}
