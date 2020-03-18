using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Histogram
{
    class NormalizationAndEqualization
    {
        public Bitmap OriginalImage { get; set; }
        public int Width { get; }
        public int Height { get; }
        public Bitmap GrayLevelImage { get; set; }
        public int[] Histogram { get; set; }
        public Bitmap NormalizedImage { get; set; }
        public Bitmap EqualizedImage { get; set; }       
        
        public NormalizationAndEqualization(string filename)
        {
            OriginalImage = new Bitmap(filename);
            Width = OriginalImage.Width;
            Height = OriginalImage.Height;
            GrayLevelImage = new Bitmap(Width, Height);
            Histogram = new int[256];
            NormalizedImage = new Bitmap(Width, Height);
            EqualizedImage = new Bitmap(Width, Height);
        }

        public void CreateGrayLevelImage()
        {            
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    int average = (OriginalImage.GetPixel(i, j).R + OriginalImage.GetPixel(i, j).G + OriginalImage.GetPixel(i, j).B) / 3;
                    GrayLevelImage.SetPixel(i, j, Color.FromArgb(average, average, average));
                }
        }

        void FillGrayLevelHistogram()
        {            
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    Histogram[GrayLevelImage.GetPixel(i, j).R]++;
        }

        int FindMinimum()
        {
            for (int i = 0; i < 256; i++)
                if (Histogram[i] != 0)
                    return i;
            return -1;
        }

        int FindMaximum()
        {
            for (int i = 255; i >= 0; i--)
                if (Histogram[i] != 0)
                    return i;
            return -1;
        }
        public void Normalize()
        {
            FillGrayLevelHistogram();
            int min = FindMinimum();
            int max = FindMaximum();
            if (min == -1 || max == -1)
                throw new Exception("Somthing is wrong with the image!");
            int quantityOfIntervals = max - min;
            double intervalLength = (double)255 / quantityOfIntervals;
            double normolizedLevel = 0;            
            for (int i = 0; i < 256; i++)
                Histogram[i] = 0;
            for (int i = min; i <= max; i++)
            {
                Histogram[i] = (int)normolizedLevel;
                normolizedLevel += intervalLength;
                Console.WriteLine(normolizedLevel);
            }                        
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    int value = Histogram[GrayLevelImage.GetPixel(i, j).R];
                    Color grayColor = Color.FromArgb(value, value, value);
                    NormalizedImage.SetPixel(i, j, grayColor);
                }                                             
        }
        public void Equalize()
        {
            const int k = 256;
            double[] h = new double[k];

            // Построение гистограммы
            for (int i = 0; i < k; i++)
                h[i] = 0;
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    h[GrayLevelImage.GetPixel(i, j).R]++;

            int size = Width * Height;
            // Номмирование гистограммы
            for (int i = 0; i < k; i++)
                h[i] = h[i] / size;

            // Построение гистограммы с накоплением
            for (int i = 1; i < k; i++)
                h[i] = h[i - 1] + h[i];

            //Равномерное распределение значений
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    int value = (int)(h[GrayLevelImage.GetPixel(i, j).R] * (k - 1));
                    Color color = Color.FromArgb(value, value, value);
                    EqualizedImage.SetPixel(i, j, color);
                }            
        }
    }
}
