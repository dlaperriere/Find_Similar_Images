// pHash-like image hash
//
// from https://github.com/perivar/FindSimilar/blob/master/ImagePHash.cs
// Copyright (C) Per Ivar Nerseth.
//
// modifications
//  - removed the DEBUG code
//  - added method Compare & PerceptiveHash
//  - GetHash use Bitmap.LockBits and direct memory access in unsafe context for faster pixel access
//    http://csharpexamples.com/fast-image-processing-c/ (in case large size/smallerSize are used)

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Images
{
    // * pHash-like image hash.
    // * Author: Elliot Shepherd (elliot@jarofworms.com)
    // * Based On: http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html
    // * Converted from Java to C# by Per Ivar Nerseth (perivar@nerseth.com)
    public class ImagePHash
    {
        /// <summary>
        /// Image size used for calculations
        /// </summary>
        private int size = 32;			// 32 (64 usable?)

        private int smallerSize = 8; 	// 8  (16 usable?)

        /// <summary>
        /// default constructor
        /// </summary>
        public ImagePHash()
        {
        }

        /// <summary>
        /// Constructor with size of reduced image
        /// </summary>
        /// <param name="size"></param>
        /// <param name="smallerSize"></param>
        public ImagePHash(int size, int smallerSize)
        {
            this.size = size;
            this.smallerSize = smallerSize;
        }

        /// <summary>
        /// Compare 2 images
        /// </summary>
        /// <param name="image1">The first image.</param>
        /// <param name="image2">The second image.</param>
        /// <returns>similarity % [0,100]</returns>
        public static double Compare(Image image1, Image image2)
        {
            var hash1 = PerceptiveHash(image1);
            var hash2 = PerceptiveHash(image2);
            return Similarity(hash1, hash2);
        }


        /// <summary>
        /// Calcutate the perceptual hash of an image according to the algorithm given by Dr. Neal Krawetz
        /// on his blog: http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html.
        /// </summary>
        /// <param name="image">The image to hash</param>
        /// <returns>perceptive hash</returns>
        public static string PerceptiveHash(Image image)
        {
            var image_bmp = new Bitmap(image);
            var hash = PerceptiveHash(image_bmp);
            image_bmp.Dispose();

            return hash;
        }

        /// <summary>
        /// Calcutate the perceptual hash of an image according to the algorithm given by Dr. Neal Krawetz
        /// on his blog: http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html.
        /// </summary>
        /// <param name="image_name">The image to hash</param>
        /// <returns>perceptive hash</returns>
        public static string PerceptiveHash(string image_name)
        {
            var image = new Bitmap(image_name);
            var hash = PerceptiveHash(image);
            image.Dispose();

            return hash;
        }

        /// <summary>
        /// Calculate the similarity of 2 perceptive hashes
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns>similarity % [0,100]</returns>
        public static double Similarity(string first, string second)
        {
            return Images.ImageHash.SimilarityBitString(first, second);
        }

        /// <summary>
        /// Calcutate the perceptual hash of an image according to the algorithm given by Dr. Neal Krawetz
        /// on his blog: http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html.
        /// </summary>
        /// <param name="img">The image to hash</param>
        /// <returns>perceptive hash</returns>
        private static string PerceptiveHash(Bitmap img)
        {
            var phash = new Images.ImagePHash(Images.ImageSimilarity.SmallerSize, 8);
            string hash = phash.GetHash(img);

            return hash;
        }

        /// <summary>
        /// Calcutate the perceptual hash of an image according to the algorithm given by Dr. Neal Krawetz
        /// on his blog: http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html.
        /// </summary>
        /// <param name="image">The image to hash.</param>
        /// <returns>Returns a 'binary string' (aka bitstring) (like. 001010111011100010) which is easy to do a hamming distance on.</returns>
        private string GetHash(Bitmap img)
        {
            // 1. Reduce size.
            // Like Average Hash, pHash starts with a small image.
            // However, the image is larger than 8x8; 32x32 is a good size.
            // This is really done to simplify the DCT computation and not
            // because it is needed to reduce the high frequencies.
            var simg = CommonUtils.ImageUtils.Resize(img, size, size);

            // 2. Reduce color.
            // The image is reduced to a grayscale just to further simplify
            // the number of computations.
            var gimg = CommonUtils.ImageUtils.MakeGrayscale(simg);

            double[][] vals = new double[size][];

            // for faster pixels access
            // http://csharpexamples.com/fast-image-processing-c/

            unsafe
            {
                BitmapData bitmapData = gimg.LockBits(new Rectangle(0, 0, gimg.Width, gimg.Height), ImageLockMode.ReadWrite, gimg.PixelFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(gimg.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                {
                    vals[x / 4] = new double[size];
                    for (int y = 0; y < heightInPixels; y++)
                    {
                        byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);

                        // Console.WriteLine("x ({0})  y ({1})", x, y);

                        // when the image is grayscale RGB has the same value
                        vals[x / 4][y] = currentLine[x];
                    }
                }

                gimg.UnlockBits(bitmapData);
                simg.Dispose();
                gimg.Dispose();
            }

            //  3. Calcutate the DCT.
            //	The DCT separates the image into a collection of frequencies
            //	and scalars. While JPEG uses an 8x8 DCT, this algorithm uses
            //	a 32x32 DCT.
            double[][] dctVals = DctMethods.dct2(vals);
            //double[][] dctVals = DctMethods.idct2(vals);

            // 4. Reduce the DCT.
            // This is the magic step. While the DCT is 32x32, just keep the
            // top-left 8x8. Those represent the lowest frequencies in the
            // picture.

            // 5 a) Calcutate the average value.
            // Like the Average Hash, compute the mean DCT value (using only
            // the 8x8 DCT low-frequency values and excluding the first term
            // since the DC coefficient can be significantly different from
            // the other values and will throw off the average).
            double total = 0;
            for (int x = 0; x < smallerSize; x++)
            {
                for (int y = 0; y < smallerSize; y++)
                {
                    total += dctVals[x][y];
                }
            }
            total -= dctVals[0][0];

            // 5. b) Calcutate the average value.
            double avg = total / (double)((smallerSize * smallerSize) - 1);

            // 6. Further reduce the DCT.
            // This is the magic step. Set the 64 hash bits to 0 or 1
            // depending on whether each of the 64 DCT values is above or
            // below the average value. The result doesn't tell us the
            // actual low frequencies; it just tells us the very-rough
            // relative scale of the frequencies to the mean. The result
            // will not vary as long as the overall structure of the image
            // remains the same; this can survive gamma and color histogram
            // adjustments without a problem.
            string hash = String.Empty;
            for (int x = 0; x < smallerSize; x++)
            {
                for (int y = 0; y < smallerSize; y++)
                {
                    if (x != 0 && y != 0)
                    {
                        hash += (dctVals[x][y] > avg ? "1" : "0");
                    }
                }
            }

            return hash;
        }
    }
}