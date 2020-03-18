using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Histogram
{
    public partial class Form2 : Form
    {
        NormalizationAndEqualization Image { get; set; }
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Image = new NormalizationAndEqualization(textBox1.Text);
            pictureBox1.Image = Image.OriginalImage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Image.CreateGrayLevelImage();
            pictureBox2.Image = Image.GrayLevelImage;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Image.Normalize();
            pictureBox3.Image = Image.NormalizedImage;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Image.Equalize();
            pictureBox4.Image = Image.EqualizedImage;
        }
    }
}
