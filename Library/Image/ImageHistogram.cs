// Compare RGB histograms
//
// Copyright (C) David Laperriere

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace Images
{
    /// <summary>
    /// Compare the RGB histogram distribution of images
    /// </summary>
    internal class ImageHistogram
    {
        /// <summary>
        /// Image size used for calculations
        /// </summary>
        private static int smaller_size = Images.ImageSimilarity.SmallerSize;

        /// <summary>
        /// Similarity of the RGB histogram  of 2 images
        /// </summary>
        /// <param name="image1">path to first image</param>
        /// <param name="image2">path to 2nd image </param>
        /// <returns>correlation of the histograms as a % similarity [-1,1] -> 0-100%</returns>
        public static double CompareRgbHistogram(string path_image1, string path_image2)
        {
            var image1 = Image.FromFile(path_image1, true);
            var image2 = Image.FromFile(path_image2, true);

            return CompareRgbHistogram(image1, image2);
        }

        /// <summary>
        /// Similarity of the RGB histogram  of 2 images
        /// </summary>
        /// <param name="image1">first image</param>
        /// <param name="image2">2nd image </param>
        /// <returns>correlation of the histograms as a % similarity [0,1] -> 0-100%</returns>
        public static double CompareRgbHistogram(Image image1, Image image2)
        {
            double sim = 0;

            // load image to smaller size to speedup calculation
            var image1_bmp = (Bitmap)CommonUtils.ImageUtils.Resize(image1, smaller_size, smaller_size, true);
            var image2_bmp = (Bitmap)CommonUtils.ImageUtils.Resize(image2, smaller_size, smaller_size, true);

            var hist1_r = GetHistogram(image1_bmp, 0);
            var hist2_r = GetHistogram(image2_bmp, 0);
            var hist1_g = GetHistogram(image1_bmp, 1);
            var hist2_g = GetHistogram(image2_bmp, 1);
            var hist1_b = GetHistogram(image1_bmp, 2);
            var hist2_b = GetHistogram(image2_bmp, 2);

            // convert pearson correlation as percentage
            // [-1,1] -> 0-100%
            var correlation_r = CommonUtils.MathUtils.PearsonCorrelation(hist1_r, hist2_r);
            var correlation_g = CommonUtils.MathUtils.PearsonCorrelation(hist1_g, hist2_g);
            var correlation_b = CommonUtils.MathUtils.PearsonCorrelation(hist1_b, hist2_b);

            //var sim_r = 100 * (correlation_r + 1) / 2;
            //var sim_g = 100 * (correlation_g + 1) / 2;
            //var sim_b = 100 * (correlation_b + 1) / 2;

            // [0,1] -> 0-100%
            var sim_r = 0.0;
            if (correlation_r >= 0)
            {
                sim_r = 100.0 * correlation_r;
            }
            var sim_g = 0.0;
            if (correlation_g >= 0)
            {
                sim_g = 100.0 * correlation_g;
            }
            var sim_b = 0.0;
            if (correlation_b >= 0)
            {
                sim_b = 100.0 * correlation_b;
            }

            sim = (0.33 * sim_r + 0.33 * sim_g + 0.33 * sim_b) + 1;

            return sim;
        }

        /// <summary>
        /// Similarity the average RGB histogram of 2 images
        /// </summary>
        /// <param name="image1">path to first image</param>
        /// <param name="image2">path to 2nd image </param>
        /// <returns>correlation of the histograms as a % similarity [-1,1] -> 0-100%</returns>
        public static double CompareAvgRgbHistogram(string path_image1, string path_image2)
        {
            var image1 = Image.FromFile(path_image1, true);
            var image2 = Image.FromFile(path_image2, true);
            return CompareAvgRgbHistogram(image1, image2);
        }

        /// <summary>
        /// Similarity the average RGB histogram of 2 images
        /// </summary>
        /// <param name="image1">first image</param>
        /// <param name="image2">2nd image </param>
        /// <returns>correlation of the histograms as a % similarity [0,1] -> 0-100% -> 0-100%</returns>
        public static double CompareAvgRgbHistogram(Image image1, Image image2)
        {
            double sim = 0;

            // load image to smaller size to speedup calculation

            var image1_bmp = (Bitmap)CommonUtils.ImageUtils.Resize(image1, smaller_size, smaller_size, true);
            var image2_bmp = (Bitmap)CommonUtils.ImageUtils.Resize(image2, smaller_size, smaller_size, true);

            var hist1 = GetHistogram(image1_bmp, 3);
            var hist2 = GetHistogram(image2_bmp, 3);

            // convert pearson correlation as percentage

            var correlation = CommonUtils.MathUtils.PearsonCorrelation(hist1, hist2);
            // [-1,1] -> 0-100%
            //sim = 100 * (correlation + 1) / 2;

            // [0,1] -> 0-100%
            if (correlation < 0)
            {
                sim = 0;
            }
            else
            {
                sim = 100.0 * correlation;
            }
            return sim;
        }

        /// <summary>
        /// Draw average RGB histogram of an image
        /// </summary>
        /// <param name="path_img">Path of image</param>
        /// <returns>Average RGB histogram image</returns>
        public static Bitmap DrawAvgRgbHistogram(string path_img)
        {
            var image = Image.FromFile(path_img, true);
            return DrawAvgRgbHistogram(image);
        }

        /// <summary>
        /// Draw average RGB histogram of an image
        /// </summary>
        /// <param name="image">image</param>
        /// <returns>Average RGB histogram image</returns>
        public static Bitmap DrawAvgRgbHistogram(Image image)
        {
            //based on http://www.codeproject.com/Articles/12125/A-simple-histogram-displaying-control

            int width = 128;
            int height = 64;

            var hist_img = new Bitmap(width, height);

            var image_bmp = (Bitmap)CommonUtils.ImageUtils.Resize(image, smaller_size, smaller_size, true);
            var myHistogram = GetHistogram(image_bmp, 3);

            // Draw histogram
            Graphics g = Graphics.FromImage(hist_img);

            double myYUnit; //this gives the vertical unit used to scale our values
            double myXUnit; //this gives the horizontal unit used to scale our values
            int myOffset = 3; //the offset, in pixels, from the control margins
            var myMaxValue = myHistogram.Max();
            myYUnit = (double)(height - (2 * myOffset)) / myMaxValue;
            myXUnit = (double)(width - (2 * myOffset)) / (myHistogram.Length - 1);
            //Font myFont = new Font("Tahoma", 10);

            Color myColor = Color.Black;
            //Pen myPen = new Pen(new SolidBrush(myColor), myXUnit);
            Pen myPen = new Pen(new HatchBrush(HatchStyle.DarkHorizontal, myColor), (float)myXUnit);

            // backgound
            g.FillRectangle(Brushes.Gray, 0, 0, hist_img.Width, hist_img.Height);

            for (int i = 0; i < myHistogram.Length; i++)
            {
                //draw bar

                g.DrawLine(myPen, new PointF(myOffset + (i * (float)myXUnit),
                   height - myOffset), new PointF(myOffset + (i * (float)myXUnit),
                   height - myOffset - (float)myHistogram[i] * (float)myYUnit));
            }

            return hist_img;
        }

        /// <summary>
        /// Draw RGB histogram of an image (each color as line)
        /// </summary>
        /// <param name="path_img">path to Image</param>
        /// <returns>image of the RGB histogram</returns>
        public static Bitmap DrawRgbHistogram(string path_img)
        {
            var image = Image.FromFile(path_img, true);
            return DrawRgbHistogram(image);
        }

        /// <summary>
        /// Draw RGB histogram of an image (each color as line)
        /// </summary>
        /// <param name="image">image</param>
        /// <returns>image of the RGB histogram</returns>
        public static Bitmap DrawRgbHistogram(Image image)
        {
            //based on http://www.codeproject.com/Articles/12125/A-simple-histogram-displaying-control

            int width = 128;
            int height = 64;

            var hist_img = new Bitmap(width, height);

            // load image to smaller size to speedup calculation
            var image_bmp = (Bitmap)CommonUtils.ImageUtils.Resize(image, smaller_size, smaller_size, true);

            var redHistogram = GetHistogram(image_bmp, 0);
            var greenHistogram = GetHistogram(image_bmp, 1);
            var blueHistogram = GetHistogram(image_bmp, 2);

            // Draw histogram
            Graphics g = Graphics.FromImage(hist_img);

            double myYUnit; //this gives the vertical unit used to scale our values
            double myXUnit; //this gives the horizontal unit used to scale our values
            double myOffset = 3; //the offset, in pixels, from the control margins

            var myMaxValue = redHistogram.Max();
            myMaxValue = Math.Max(myMaxValue, greenHistogram.Max());
            myMaxValue = Math.Max(myMaxValue, blueHistogram.Max());
            myYUnit = (height - (2 * myOffset)) / myMaxValue;
            myXUnit = (width - (2 * myOffset)) / (redHistogram.Length - 1);

            Pen redPen = new Pen(new SolidBrush(Color.DarkRed), (float)myXUnit);
            Pen greenPen = new Pen(new SolidBrush(Color.DarkGreen), (float)myXUnit);
            Pen bluePen = new Pen(new SolidBrush(Color.DarkBlue), (float)myXUnit);

            // backgound
            g.FillRectangle(Brushes.DarkGray, 0, 0, hist_img.Width, hist_img.Height);

            float fudge_factor = 0.6f; // added to G/B to make them visible when all line overlap

            for (int i = 0; i < redHistogram.Length - 1; i++)
            {
                // R/G/B lines (from x to x+1)

                var x1 = (float)(myOffset + (i * myXUnit));
                var y1r = (float)(height - myOffset - redHistogram[i] * myYUnit);
                var y1g = (float)(height - myOffset - greenHistogram[i] * myYUnit) + fudge_factor;
                var y1b = (float)(height - myOffset - blueHistogram[i] * myYUnit) + 2 * fudge_factor;

                var x2 = (float)(myOffset + ((i + 1) * myXUnit));
                var y2r = (float)(height - myOffset - redHistogram[i + 1] * myYUnit);
                var y2g = (float)(height - myOffset - greenHistogram[i + 1] * myYUnit) + fudge_factor;
                var y2b = (float)(height - myOffset - blueHistogram[i + 1] * myYUnit) + 2 * fudge_factor;

                g.DrawLine(redPen, new System.Drawing.PointF(x1, y1r), new System.Drawing.PointF(x2, y2r));
                g.DrawLine(greenPen, new System.Drawing.PointF(x1, y1g), new System.Drawing.PointF(x2, y2g));
                g.DrawLine(bluePen, new System.Drawing.PointF(x1, y1b), new System.Drawing.PointF(x2, y2b));
            }
            return hist_img;
        }

        /// <summary>
        /// Draw RGB histogram of an image (each color as spline area)
        /// </summary>
        /// <param name="path_img">path to Image</param>
        /// <returns>image of the RGB histogram</returns>
        public static Bitmap DrawRgbHistogramChart(string path_img)
        {
            var image = Image.FromFile(path_img, true);
            return DrawRgbHistogramChart(image);
        }

        /// <summary>
        /// Draw RGB histogram of an image (each color as spline area)
        /// </summary>
        /// <param name="image">image</param>
        /// <returns>image of the RGB histogram</returns>
        public static Bitmap DrawRgbHistogramChart(Image image)
        {
            //based on https://code.msdn.microsoft.com/Professional-Image-b88e8871#content

            int width = 256;
            int height = 128;

            var hist_img = new Bitmap(width, height);

            var chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            //chart.Dock = DockStyle.Fill;

            chart.AntiAliasing = System.Windows.Forms.DataVisualization.Charting.AntiAliasingStyles.All;

            chart.Size = new Size(width, height);
            chart.BackColor = Color.LightGray;
            chart.ChartAreas.Add("Default");

            chart.Series.Add("Red");
            chart.Series.Add("Green");
            chart.Series.Add("Blue");

            // Set SplineArea chart type
            chart.Series["Red"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.SplineArea;
            chart.Series["Green"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.SplineArea;
            chart.Series["Blue"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.SplineArea;

            // set line tension
            chart.Series["Red"]["LineTension"] = "0.8";
            chart.Series["Green"]["LineTension"] = "0.8";
            chart.Series["Blue"]["LineTension"] = "0.8";

            // Set colour and transparency
            chart.Series["Red"].Color = Color.FromArgb(200, Color.DarkRed);
            chart.Series["Green"].Color = Color.FromArgb(150, Color.DarkGreen);
            chart.Series["Blue"].Color = Color.FromArgb(100, Color.DarkBlue);

            // Disable X & Y axis labels
            chart.ChartAreas["Default"].AxisX.LabelStyle.Enabled = false;
            chart.ChartAreas["Default"].AxisY.LabelStyle.Enabled = false;
            chart.ChartAreas["Default"].AxisX.MinorGrid.Enabled = false;
            chart.ChartAreas["Default"].AxisX.MajorGrid.Enabled = false;
            chart.ChartAreas["Default"].AxisY.MinorGrid.Enabled = false;
            chart.ChartAreas["Default"].AxisY.MajorGrid.Enabled = false;

            // load image to smaller size to speedup calculation
            var image_bmp = (Bitmap)CommonUtils.ImageUtils.Resize(image, smaller_size, smaller_size, true);

            var redHistogram = GetHistogram(image_bmp, 0);
            var greenHistogram = GetHistogram(image_bmp, 1);
            var blueHistogram = GetHistogram(image_bmp, 2);

            // Draw histogram

            chart.Series["Red"].Points.Clear();
            chart.Series["Green"].Points.Clear();
            chart.Series["Blue"].Points.Clear();

            float fudge_factor = 0.6f; // added to R/G to make them visible when all line overlap

            for (int i = 0; i < redHistogram.Length; i++)
            {
                chart.Series["Red"].Points.AddY(redHistogram[i] + fudge_factor);
                chart.Series["Green"].Points.AddY(greenHistogram[i] + 2 * fudge_factor);
                chart.Series["Blue"].Points.AddY(blueHistogram[i]);
            }
            chart.DrawToBitmap(hist_img, new Rectangle(0, 0, hist_img.Width - 1, hist_img.Height - 1));

            return hist_img;
        }

        /// <summary>
        /// Get the histogram of an image
        /// </summary>
        /// <param name="image">image</param>
        /// <param name="type">histogram type: 0=Red, 1=Green, 2=Blue, 3=RGB Average</param>
        /// <returns>histogram</returns>
        public static double[] GetHistogram(Bitmap image, int type = 3)
        {
            double[] histogram = new double[256];

            // for faster pixels access
            // http://csharpexamples.com/fast-image-processing-c/
            unsafe
            {
                BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                for (int y = 0; y < heightInPixels; y++)
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        //System.Drawing.Color c = image.GetPixel(x, y);
                        int R = currentLine[x + 2];
                        int G = currentLine[x + 1];
                        int B = currentLine[x];
                        switch (type)
                        {
                            case 0:
                                histogram[R]++;
                                break;

                            case 1:
                                histogram[G]++;
                                break;

                            case 2:
                                histogram[B]++;
                                break;

                            default:
                                //average
                                histogram[((R + G + B) / 3)]++;
                                break;
                        }
                    }
                }
                image.UnlockBits(bitmapData);
            } // unsafe

            return histogram;
        }

        /// <summary>
        /// Get the histogram of an image
        /// </summary>
        /// <param name="image">image</param>
        /// <param name="type">histogram type: 0=Red, 1=Green, 2=Blue, 3=RGB Average</param>
        /// <returns>histogram</returns>
        public static double[] GetHistogram(Image image)
        {
            var image_bmp = (Bitmap)image;
            return GetHistogram(image_bmp);
        }

        /// <summary>
        /// Get the histogram of an image
        /// </summary>
        /// <param name="path_image">path to image</param>
        /// <param name="type">histogram type: 0=Red, 1=Green, 2=Blue, 3=RGB Average</param>
        /// <returns>histogram</returns>
        public static double[] GetHistogram(string path_image, int type = 3)
        {
            var image = Image.FromFile(path_image, true);
            var image_bmp = (Bitmap)image;

            // load image to smaller size to speedup calculation
            image_bmp = (Bitmap)CommonUtils.ImageUtils.Resize(image_bmp, smaller_size, smaller_size, true);

            return GetHistogram(image_bmp, type);
        }
    }
}