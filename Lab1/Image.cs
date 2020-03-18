using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Histogram
{
	class Image
	{			
		public Bitmap MyImage { get; set; }		
		public Bitmap ColorCorrectionImage { get; set; }
		public Bitmap GreyWorldCorrectionImage { get; set; }
		public Bitmap CorrectionFunctionImage { get; set; }
		public Bitmap BackCorrectionFunctionImage { get; set; }
		public int Width { get; }
		public int Height { get; }
		public int[] RedArray { get; set; }
		public int[] GreenArray { get; set; }
		public int[] BlueArray { get; set; }	
		public Bitmap RedHisto { get; set; }
		public Bitmap GreenHisto { get; set; }
		public Bitmap BlueHisto { get; set; }

		public Bitmap RedHistogram { get; set; }
		public Image()
		{
			MyImage = new Bitmap("Image.jpg");
			Width = MyImage.Width;
			Height = MyImage.Height;
			ColorCorrectionImage = new Bitmap(MyImage.Width, MyImage.Height);
			GreyWorldCorrectionImage = new Bitmap(MyImage.Width, MyImage.Height);
			CorrectionFunctionImage = new Bitmap(MyImage.Width, MyImage.Height);
			BackCorrectionFunctionImage = new Bitmap(Width, Height);
			CreateArrays();
		}
		public void CreateArrays()
		{
			RedArray = new int[256];
			GreenArray = new int[256];
			BlueArray = new int[256];
			var array = new int[256];
			int sizeX = MyImage.Size.Width;
			int sizeY = MyImage.Size.Height;
			for (int x = 0; x < sizeX; x++)
				for (int y = 0; y < sizeY; y++)
				{
					RedArray[MyImage.GetPixel(x, y).R]++;
					GreenArray[MyImage.GetPixel(x, y).G]++;
					BlueArray[MyImage.GetPixel(x, y).B]++;
				}
			int max = FindMaximum();
			NormalizeArray(RedArray, max);
			NormalizeArray(GreenArray, max);
			NormalizeArray(BlueArray, max);
		}
		public int FindMaximum()
		{
			int max;
			int maxT = RedArray.Max();
			max = maxT;
			maxT = GreenArray.Max();
			if (maxT > max)
				max = maxT;
			maxT = BlueArray.Max();
			if (maxT > max)
				max = maxT;
			return max;
		}
		private void NormalizeArray(int[] array, int max)
		{			
			for (int i = 0; i < 256; i++)
				array[i] = 250 * array[i] / max;
		}
		public void CreateHistograms()
		{
			RedHisto = new Bitmap(256, 250);
			GreenHisto = new Bitmap(256, 250);
			BlueHisto = new Bitmap(256, 250);
			int height = 250;			
			for (int i = 0; i < 256; i++)
				for (int j = 0; j < height; j++)
				{
					RedHisto.SetPixel(i, j, Color.White);
					GreenHisto.SetPixel(i, j, Color.White);
					BlueHisto.SetPixel(i, j, Color.White);
				}
					
			for (int i = 0; i < 256; i++)
			{
				for (int j = height - 1; j >= height - RedArray[i]; j--)
					RedHisto.SetPixel(i, j, Color.Red);
				for (int j = height - 1; j >= height - GreenArray[i]; j--)
					GreenHisto.SetPixel(i, j, Color.Green);
				for (int j = height - 1; j >= height - BlueArray[i]; j--)
					BlueHisto.SetPixel(i, j, Color.Blue);
			}
			RedHisto.Save("RedHisto.jpg");
			GreenHisto.Save("GreenHisto.jpg");
			BlueHisto.Save("BlueHisto.jpg");
		}

		public void ColorCorrection(int x, int y, Color color)
		{

			double koeffR;
			if (MyImage.GetPixel(x, y).R == 0)
				koeffR = 1;
			else
				koeffR = (double)color.R / MyImage.GetPixel(x, y).R;
			if (koeffR > 1)
				koeffR = 1;

			double koeffG;
			if (MyImage.GetPixel(x, y).G == 0)
				koeffG = 1;
			else
				koeffG = (double)color.G / MyImage.GetPixel(x, y).G;
			if (koeffG > 1)
				koeffG = 1;

			double koeffB;
			if (MyImage.GetPixel(x, y).B == 0)
				koeffB = 1;
			else
				koeffB = (double) color.B / MyImage.GetPixel(x, y).B;
			if (koeffB > 1)
				koeffB = 1;			
			
			Console.WriteLine(koeffR);
			Console.WriteLine(koeffG);
			Console.WriteLine(koeffB);
			int width = MyImage.Width;
			int height = MyImage.Height;
			for (int i = 0; i < width; i++)
				for (int j = 0; j < height; j++)
				{
					int red = (int)(MyImage.GetPixel(i, j).R * koeffR);
					int green = (int)(MyImage.GetPixel(i, j).G * koeffG);
					int blue = (int)(MyImage.GetPixel(i, j).B * koeffB);
					ColorCorrectionImage.SetPixel(i, j, Color.FromArgb(red, green, blue));					
				}					
		}

		public void GreyWorldCorrection()
		{
			double redAv = 0;
			double greenAv = 0;
			double blueAv = 0;
			double av;
			int width = MyImage.Width;
			int height = MyImage.Height;
			int n = width * height;
			for (int i = 0; i < width; i++)
				for (int j = 0; j < height; j++)
				{
					redAv += MyImage.GetPixel(i, j).R;
					greenAv += MyImage.GetPixel(i, j).G;
					blueAv += MyImage.GetPixel(i, j).B;
				}
			redAv = redAv / n;
			greenAv = greenAv / n;
			blueAv = blueAv / n;
			av = (redAv + greenAv + blueAv) / 3;
			for (int i = 0; i < width; i++)
				for (int j = 0; j < height; j++)
				{					
					int red = (int)(MyImage.GetPixel(i, j).R * av / redAv);
					if (red > 255)
						red = 255;
					int green = (int)(MyImage.GetPixel(i, j).G * av / greenAv);
					if (green > 255)
						green = 255;
					int blue = (int)(MyImage.GetPixel(i, j).B * av / blueAv);
					if (blue > 255)
						blue = 255;
					GreyWorldCorrectionImage.SetPixel(i, j, Color.FromArgb(red, green, blue));
				}
		}
		public void CorrectionFunction()
		{
			int width = MyImage.Width;
			int height = MyImage.Height;
			for (int i = 0; i < width; i++)
				for (int j = 0; j < height; j++)
				{
					//int partR = (int)(Math.Sqrt((double)MyImage.GetPixel(i, j).R / 255) * Math.Sqrt(255));					
					//int partG = (int)(Math.Sqrt((double)MyImage.GetPixel(i, j).G / 255) * Math.Sqrt(255));					
					//int partB = (int)(Math.Sqrt((double)MyImage.GetPixel(i, j).B / 255) * Math.Sqrt(255));	
					int partR = (int)Math.Sqrt(MyImage.GetPixel(i, j).R);					
					int partG = (int)Math.Sqrt(MyImage.GetPixel(i, j).G);					
					int partB = (int)Math.Sqrt(MyImage.GetPixel(i, j).B);
					CorrectionFunctionImage.SetPixel(i, j, Color.FromArgb(partR, partG, partB));
				}
		}

		public Bitmap BackCorrectionFunction()
		{
			int width = MyImage.Width;
			int height = MyImage.Height;
			for (int i = 0; i < width; i++)
				for (int j = 0; j < height; j++)
				{
					int r = CorrectionFunctionImage.GetPixel(i, j).R * CorrectionFunctionImage.GetPixel(i, j).R;
					int g = CorrectionFunctionImage.GetPixel(i, j).G * CorrectionFunctionImage.GetPixel(i, j).G;
					int b = CorrectionFunctionImage.GetPixel(i, j).B * CorrectionFunctionImage.GetPixel(i, j).B;
					BackCorrectionFunctionImage.SetPixel(i, j, Color.FromArgb(r, g, b));
				}
			return BackCorrectionFunctionImage;
		}
	}
}
