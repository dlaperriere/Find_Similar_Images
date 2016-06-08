// Use the main color of an image as a "hash"
// Copyright (C) David Laperriere.

using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Images
{
    public class ImageColorHash
    {
        private static double tolerance = Images.ImageSimilarity.Tolerance;
        private static double tolerance_percent = Images.ImageSimilarity.TolerancePercent;

        /// <summary>
        /// Calcutate the color hash an image
        /// </summary>
        /// <param name="image">The image to hash.</param>
        /// <returns>main color argb int value as a color hash</returns>
        public static string ColorHash(Image image)
        {
            int hash = 0;
            //var main_color = Images.ColorExtract.MainColor(image);
            var main_color = Images.ColorExtract.TopColors(image);
            hash = main_color.ElementAt(0).Key.ToArgb();

            return hash.ToString();
        }

        /// <summary>
        /// Calcutate the color hash of the image  in the given file.
        /// </summary>
        /// <param name="path">Path to the input file.</param>
        /// <returns>average hash</returns>
        public static string ColorHash(String path)
        {
            Image img = Image.FromFile(path, true);
            var hash = ColorHash(img);
            img.Dispose();

            return hash;
        }

        /// <summary>
        /// Compare 2 images
        /// </summary>
        /// <param name="image1">The first image.</param>
        /// <param name="image2">The second image.</param>
        /// <returns>similarity % [0,100]</returns>
        public static double Compare(Image image1, Image image2)
        {
            var hash1 = ColorHash(image1);
            var hash2 = ColorHash(image2);
            return Similarity(hash1, hash2);
        }


        /// <summary>
        /// Calculate the similarity of 2 color hashes
        /// </summary>
        /// <param name="hash1">The first hash.</param>
        /// <param name="hash2">The second hash.</param>
        /// <returns>similarity % (0 or 100)</returns>
        public static double Similarity(string hash1, string hash2)
        {
            double sim = 0.0;
            var c1 = Color.FromArgb(Convert.ToInt32(hash1));
            var c2 = Color.FromArgb(Convert.ToInt32(hash2));

            //sim = Images.ImageSimilarity.Compare2Pixels(c1, c2) ;

            if (c1.R == c2.R && c1.G == c2.G && c1.B == c2.B)
            {
                sim = 100.0;
            }

            return sim;
        }
    }
}