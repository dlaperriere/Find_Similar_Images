// Average hash of an image http://www.hackerfactor.com/blog/index.php?/archives/529-Kind-of-Like-That.html
//
// from https://github.com/perivar/FindSimilar/blob/master/ImageAverageHash.cs
// Copyright (C) Per Ivar Nerseth.
//
// modifications
//  - removed the DEBUG code
//  - added Compare  method

using CommonUtils;

using System;
using System.Drawing;
using System.IO;

namespace Images
{
    // http://stackoverflow.com/questions/4240490/problems-with-dct-and-idct-algorithm-in-java
    // http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html
    // http://pastebin.com/Pj9d8jt5
    // https://github.com/jforshee/ImageHashing/blob/master/ImageHashing/ImageHashing.cs
    public class ImageAverageHash
    {
        private static BitCounter bitCounter = new BitCounter(8);

        /// <summary>
        /// Calcutate the average hash of an image according to the algorithm given by Dr. Neal Krawetz
        /// on his blog: http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html.
        /// </summary>
        /// <param name="image">The image to hash.</param>
        /// <returns>average hash</returns>
        public static string AverageHash(Image image)
        {
            int smallerSize = 8;

            Bitmap squeezedImage = new Bitmap(CommonUtils.ImageUtils.Resize(image, smallerSize, smallerSize));

            uint averageValue = 0;
            byte[] grayscaleByteArray = CommonUtils.ImageUtils.ImageToByteArray8BitGrayscale(squeezedImage, out averageValue);

            // Calcutate the hash: each bit is a pixel
            // 1 = higher than average, 0 = lower than average
            ulong hash = 0;

            for (int i = 0; i < grayscaleByteArray.Length; i++)
            {
                if (grayscaleByteArray[i] >= averageValue)
                {
                    hash |= (1UL << ((grayscaleByteArray.Length - 1) - i));
                }
            }

            squeezedImage.Dispose();

            return hash.ToString();
        }

        /// <summary>
        /// Calcutate the average hash of the image content in the given file.
        /// </summary>
        /// <param name="path">Path to the input file.</param>
        /// <returns>average hash</returns>
        public static string AverageHash(String path)
        {
            Image img = Image.FromFile(path, true);
            var hash = AverageHash(img);
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
            var hash1 = AverageHash(image1);
            var hash2 = AverageHash(image2);
            return Similarity(hash1, hash2);
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