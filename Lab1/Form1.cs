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
	public partial class Form1 : Form
	{
		Dictionary<string, Color> colors = new Dictionary<string, Color>();
		private Image ImageForProcessing {get; set;}
		public Form1()
		{
			InitializeComponent();
			colors.Add("Red", Color.Red);
			colors.Add("Green", Color.Green);
			colors.Add("Blue", Color.Blue);
			colors.Add("Gray", Color.Gray);
			colors.Add("Yellow", Color.Yellow);			
		}

		private void Button1_Click(object sender, EventArgs e)
		{
			ImageForProcessing = new Image();
			pictureBox1.Image = ImageForProcessing.MyImage;			
		}

		private void PictureBox1_Click(object sender, EventArgs e)
		{

		}

		private void button2_Click(object sender, EventArgs e)
		{
			ImageForProcessing.CreateArrays();
			ImageForProcessing.CreateHistograms();
			//pictureBox2.Image = ImageForProcessing.RedHisto;
			//pictureBox3.Image = ImageForProcessing.GreenHisto;
			//pictureBox4.Image = ImageForProcessing.BlueHisto;
		}

		private void Button2_Click_1(object sender, EventArgs e)
		{
			int x = int.Parse(textBox1.Text);
			int y = int.Parse(textBox2.Text);
			ImageForProcessing.ColorCorrection(x, y, colors[textBox3.Text]);
			pictureBox2.Image = ImageForProcessing.ColorCorrectionImage;
		}
		private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void Button3_Click(object sender, EventArgs e)
		{
			ImageForProcessing.GreyWorldCorrection();
			pictureBox3.Image = ImageForProcessing.GreyWorldCorrectionImage;

		}

		private void button4_Click(object sender, EventArgs e)
		{
			ImageForProcessing.CorrectionFunction();
			pictureBox4.Image = ImageForProcessing.CorrectionFunctionImage;
		}

		private void button5_Click(object sender, EventArgs e)
		{
			pictureBox4.Image = ImageForProcessing.BackCorrectionFunction();			
		}

		private void button6_Click(object sender, EventArgs e)
		{
			Form2 form2 = new Form2();
			form2.Show();
		}
	}
}
