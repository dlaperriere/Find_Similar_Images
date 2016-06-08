// Difference hash of an image http://www.hackerfactor.com/blog/index.php?/archives/529-Kind-of-Like-That.html
//
// based on :
// David Oftedal python implementation http://01101001.net/DifferenceHash.py
// Per Ivar Nerseth C# AverageHash https://github.com/perivar/FindSimilar/blob/master/ImageAverageHash.cs
//
// Copyright (C) David Laperriere

using CommonUtils;
using System;
using System.Drawing;
using System.IO;

namespace Images
{
    public class ImageDifferenceHash
    {
        /// <summary>
        /// Compare 2 images
        /// </summary>
        /// <param name="image1">The first image.</param>
        /// <param name="image2">The second image.</param>
        /// <returns>similarity % [0,100]</returns>
        public static double Compare(Image image1, Image image2)
        {
            var hash1 = DifferenceHash(image1);
            var hash2 = DifferenceHash(image2);
            return Similarity(hash1, hash2);
        }

        /// <summary>
        /// Calcutate the difference hash of an image according to the algorithm given by Dr. Neal Krawetz
        /// on his blog: http://www.hackerfactor.com/blog/index.php?/archives/529-Kind-of-Like-That.html
        /// </summary>
        /// <param name="image">The image to hash.</param>
        /// <param name="hash_columns">hash columns instead of roes</param>
        /// <returns>difference hash</returns>
        public static string DifferenceHash(Image image, bool hash_columns = false)
        {
            int smallerSize = 8;
            string fileSavePrefix = "DifferenceHash (" + StringUtils.GetCurrentTimestamp() + ") ";

            if (hash_columns)
            {
                image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            Bitmap squeezedImage = new Bitmap(CommonUtils.ImageUtils.Resize(image, smallerSize, smallerSize));

            uint DifferenceValue = 0;
            byte[] differenceByteArray = CommonUtils.ImageUtils.ImageToByteArray8BitGrayscale(squeezedImage, out DifferenceValue);

            // Calcutate the hash: each bit is a pixel
            // 1 = higher than previous, 0 = lower
            ulong hash = 0; // = Unsigned 64-bit integer
            byte previousPixel = differenceByteArray[differenceByteArray.Length - 1];
            for (int i = 0; i < differenceByteArray.Length; i++)
            {
                byte pixel = differenceByteArray[i];
                hash <<= 1;
                int row = (i / squeezedImage.Width);

                if (row % 2 == 0 && pixel >= previousPixel)
                {
                    hash |= 1;
                }
                else if (row % 2 != 0 && previousPixel >= pixel)
                {
                    hash |= 1;
                }

                previousPixel = pixel;
            }
            squeezedImage.Dispose();

            return hash.ToString();
        }

        /// <summary>
        /// Calcutate the difference hash of the image content in the given file.
        /// </summary>
        /// <param name="path">Path to the input file.</param>
        /// <returns>difference hash</returns>
        public static string DifferenceHash(String path)
        {
            var image = Image.FromFile(path, true);
            var hash = DifferenceHash(image);
            image.Dispose();

            return hash;
        }


        /// <summary>
        /// Calculate the similarity of 2 hashes
        /// </summary>
        /// <param name="hash1">The first hash.</param>
        /// <param name="hash2">The second hash.</param>
        /// <returns>similarity % [0,100]</returns>
        public static double Similarity(ulong hash1, ulong hash2)
        {
            return Images.ImageHash.SimilarityUtin64(hash1, hash2);
        }

        /// <summary>
        /// Calculate the similarity of 2 hashes
        /// </summary>
        /// <param name="hash1">hash of the first image file.</param>
        /// <param name="hash2">hash of the second image file.</param>
        /// <returns>similarity % [0,100]</returns>
        public static double Similarity(String hash1, String hash2)
        {
            ulong hash1v = Convert.ToUInt64(hash1);
            ulong hash2v = Convert.ToUInt64(hash2);
            return Similarity(hash1v, hash2v);
        }
    }
}