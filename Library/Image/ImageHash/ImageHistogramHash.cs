// Hash RGB histogram of an image
//
// Copyright (C) David Laperriere

using OpenCvSharp; //https://github.com/shimat/opencvsharp
using OpenCvSharp.Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Images
{
    internal class ImageHistogramHash
    {
        private static int bins = 16;

        private static double scale = 9;

        /// <summary>
        /// internal parameters
        /// </summary>
        private static int smaller_size = Images.ImageSimilarity.SmallerSize;

        /// <summary>
        /// Compare 2 images
        /// </summary>
        /// <param name="image1">The first image.</param>
        /// <param name="image2">The second image.</param>
        /// <returns>similarity % [0,100]</returns>
        public static double Compare(Image image1, Image image2)
        {
            var hash1 = HistogramHash(image1);
            var hash2 = HistogramHash(image2);
            return Similarity(hash1, hash2);
        }

        /// <summary>
        /// Compare 2 images
        /// </summary>
        /// <param name="image1">The first image.</param>
        /// <param name="image2">The second image.</param>
        /// <returns>similarity % [0,100]</returns>
        public static double Compare(Mat mat_image1, Mat mat_image2)
        {
            var image1 = (Image)BitmapConverter.ToBitmap(mat_image1);
            var image2 = (Image)BitmapConverter.ToBitmap(mat_image2);

            var hash1 = HistogramHash(image1);
            var hash2 = HistogramHash(image2);

            image1.Dispose();
            image2.Dispose();

            return Similarity(hash1, hash2);
        }

        /// <summary>
        /// Calculate hash of the RGB histogram of an image
        /// </summary>
        /// <param name="img_path">path to image file</param>
        /// <returns>histogram hash</returns>
        public static string HistogramHash(string img_path)
        {
            var image = Image.FromFile(img_path, true);
            var hash = HistogramHash(image);
            image.Dispose();

            return hash;
        }

        /// <summary>
        /// Calculate hash of the RGB histogram of an image
        /// </summary>
        /// <param name="image"></param>
        /// <returns>histogram hash</returns>
        public static string HistogramHash(Image image)
        {
            string hash = String.Empty;

            var image_bmp = (Bitmap)image;
            if (image.Width > smaller_size || image.Height > smaller_size)
            {
                image_bmp = CommonUtils.ImageUtils.Resize(image, smaller_size, smaller_size);
            }

            // get image R/G/B histograms
            var hist_r = Images.ImageHistogram.GetHistogram(image_bmp, 0);
            var hist_g = Images.ImageHistogram.GetHistogram(image_bmp, 1);
            var hist_b = Images.ImageHistogram.GetHistogram(image_bmp, 2);

            // rescale histogram values to 0-9 into a smaller number of bins
            var hist_bin_r = Enumerable.Repeat(0.0, bins + 1).ToArray();
            var hist_bin_g = Enumerable.Repeat(0.0, bins + 1).ToArray();
            var hist_bin_b = Enumerable.Repeat(0.0, bins + 1).ToArray();

            for (int i = 0; i < hist_r.Length; i++)
            {
                var bin = i / (hist_r.Length / bins);

                hist_bin_r[bin] += hist_r[i];
                hist_bin_g[bin] += hist_g[i];
                hist_bin_b[bin] += hist_b[i];
            }

            double max = hist_bin_r.Max();
            max = Math.Max(max, hist_bin_g.Max());
            max = Math.Max(max, hist_bin_b.Max());

            for (int i = 0; i < hist_bin_r.Length; i++)
            {
                hist_bin_r[i] = scale * hist_bin_r[i] / max;
                hist_bin_g[i] = scale * hist_bin_g[i] / max;
                hist_bin_b[i] = scale * hist_bin_b[i] / max;
            }

            // make hash
            var hashb = new System.Text.StringBuilder(String.Empty, 3 * bins);
            for (int i = 0; i < hist_bin_r.Length; i++)
            {
                hashb.Append(((int)hist_bin_r[i]).ToString());
                hashb.Append(((int)hist_bin_g[i]).ToString());
                hashb.Append(((int)hist_bin_b[i]).ToString());
            }
            hash = hashb.ToString();

            return hash;
        }


        /// <summary>
        /// Calculate similarity of 2 RGB histograms hashes
        /// </summary>
        /// <param name="s1">binary string 1 </param>
        /// <param name="s2">binary string 2 </param>
        /// <returns>similarity % [0,100]</returns>
        public static double Similarity(string s1, string s2)
        {
            double sim = 0;

            // use shortest string
            int lengthS1 = s1.Length;
            int lengthS2 = s2.Length;
            int length = lengthS2 > lengthS1 ? lengthS1 : lengthS2;

            var rgb1 = new List<double>();
            var rgb2 = new List<double>();

            double similarity = 0.0;
            double zeros = 0.0;

            for (int k = 0; k < length; k++)
            {
                var is1 = (double)s1[k] - 48.0;
                var is2 = (double)s2[k] - 48.0;

                if (is1 == 0 && is2 == 0)
                {
                    similarity += 1.0;
                    zeros += 1.0;
                }
                else
                {
                    var max = Math.Max(is1, is2);
                    var min = Math.Min(is1, is2);
                    similarity += min / max;
                }
            }

            sim = 100.0 * (similarity - 0.5 * zeros) / ((double)length - 0.5 * zeros);
            return sim;
        }
    }
}