//  Block Mean Value Based Image Perceptual Hash
// Converted blockhash_quick from C to C#  https://github.com/commonsmachinery/blockhash/blob/master/blockhash.c
//
// Copyright (C) David Laperriere

using OpenCvSharp; //https://github.com/shimat/opencvsharp
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Images
{
    /// <summary>
    ///  Perceptual image hash calculation tool based on algorithm descibed in
    ///  Block Mean Value Based Image Perceptual Hashing by Bian Yang, Fan Gu and Xiamu Niu
    ///
    ///  Copyright 2014 Commons Machinery http://commonsmachinery.se/
    ///  Distributed under an MIT license, please see LICENSE in the top dir.
    ///
    ///  C# port of blockhash_quick https://github.com/commonsmachinery/blockhash/blob/master/blockhash.c
    ///
    ///  reference
    ///  Bian Yang et al. Block Mean Value Based Image Perceptual Hashing
    ///  Intelligent Information Hiding and Multimedia Signal Processing, 2006. IIH-MSP '06.
    ///   http://dx.doi.org/10.1109/IIH-MSP.2006.265125
    /// </summary>
    internal class ImageBlockHash
    {
        /// <summary>
        /// Calculate the Block Mean Value Based Image Perceptual Hash
        /// </summary>
        /// <param name="image_name">image to hash</param>
        /// <param name="bits">number of blocks to divide the image by horizontally and vertically</param>
        /// <returns>hash bitstring</returns>
        /// <remarks>
        /// based on blockhash_quick https://github.com/commonsmachinery/blockhash/blob/master/blockhash.c
        /// </remarks>
        /// <seealso cref="http://dx.doi.org/10.1109/IIH-MSP.2006.265125"/>
        public static string BlockHash(string image_name, int bits = 8)
        {
            var image = Image.FromFile(image_name, true); //new Mat(image_name, ImreadModes.Unchanged);
            var hash = BlockHash(image, bits);
            image.Dispose();
            return hash;
        }

        /// <summary>
        /// Calculate the Block Mean Value Based Image Perceptual Hash
        /// </summary>
        /// <param name="image">image to hash</param>
        /// <param name="bits">number of blocks to divide the image by horizontally and vertically</param>
        /// <returns>hash bitstring</returns>
        /// <remarks>
        /// based on blockhash_quick https://github.com/commonsmachinery/blockhash/blob/master/blockhash.c
        /// </remarks>
        /// <seealso cref="http://dx.doi.org/10.1109/IIH-MSP.2006.265125"/>
        public static string BlockHash(Image image, int bits = 8)
        {
            // use smaller image to speedup calculation (default 64x64)
            var bmp_image = new Bitmap(image, 8 * bits, 8 * bits);
            var mat_image = BitmapConverter.ToMat(bmp_image);
            var hash = BlockHash(mat_image, bits);

            bmp_image.Dispose();
            mat_image.Dispose();

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
            var hash1 = BlockHash(image1);
            var hash2 = BlockHash(image2);
            return Similarity(hash1, hash2);
        }


        /// <summary>
        /// Calculate the similarity of 2 block hashes
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns>similarity % [0,100]</returns>
        public static double Similarity(string first, string second)
        {
            return Images.ImageHash.SimilarityBitString(first, second);
        }

        /// <summary>
        /// Calculate the Block Mean Value Based Image Perceptual Hash
        /// </summary>
        /// <param name="image">image to hash (OpenCV Mat)</param>
        /// <param name="bits">number of blocks to divide the image by horizontally and vertically</param>
        /// <returns>hash bitstring</returns>
        /// <remarks>
        /// based on blockhash_quick https://github.com/commonsmachinery/blockhash/blob/master/blockhash.c
        /// </remarks>
        /// <seealso cref="http://dx.doi.org/10.1109/IIH-MSP.2006.265125"/>
        private static string BlockHash(Mat image, int bits = 8)
        {
            int x, y, ix, iy;
            int value;
            int block_width;
            int block_height;
            int[] blocks;


            // use smaller image to speedup calculation (default 64x64)
            if (image.Width != 8 * bits || image.Height != 8 * bits)
            {
                var smaller = new OpenCvSharp.Size(8 * bits, 8 * bits);
                var simage = image.Resize(smaller, 0, 0, InterpolationFlags.Linear);
                simage.CopyTo(image);
                simage.Dispose();
            }


            var image_byte3 = new MatOfByte3(image);
            var indexer = image_byte3.GetIndexer();

            block_width = (image.Width) / bits;
            block_height = (image.Height) / bits;

            blocks = new int[bits * bits];

            for (y = 0; y < bits; y++)
            {
                for (x = 0; x < bits; x++)
                {
                    value = 0;

                    for (iy = 0; iy < block_height; iy++)
                    {
                        value = 0;
                        for (ix = 0; ix < block_width; ix++)
                        {
                            var iix = (x * block_width + ix);
                            var iiy = (y * block_height + iy);

                            value += (int)((double)indexer[iiy, iix].Item2 + (double)indexer[iiy, iix].Item1 + (double)indexer[iiy, iix].Item0);
                        }
                    }

                    blocks[y * bits + x] = value;
                }
            }

            var hash = translate_blocks_to_bits(blocks, bits * bits, block_width * block_height);

            //return bits_to_hexhash(hash);
            return hash;
        }

        /// <summary>
        /// Translate blocks to bits using medians across four horizontal bands
        /// </summary>
        ///
        /// <param name="blocks">array of block values</param>
        /// <param name="nblocks"> # of blocks</param>
        /// <param name="pixels_per_block"></param>
        /// <returns>block mean value hash bitstring</returns>
        ///
        /// <remarks>
        /// Output a 1 if the block is brighter than the median.
        /// With images dominated by black or white, the median may
        /// end up being 0 or the max value, and thus having a lot
        /// of blocks of value equal to the median.  To avoid
        /// generating hashes of all zeros or ones, in that case output
        /// 0 if the median is in the lower value space, 1 otherwise
        /// </remarks>
        ///
        private static string translate_blocks_to_bits(int[] blocks, int nblocks, int pixels_per_block)
        {
            string hash = String.Empty;

            double half_block_value;
            int bandsize, i, j, v;
            double m;

            half_block_value = pixels_per_block * 255 * 3 / 2;
            bandsize = nblocks / 4;

            for (i = 0; i < 4; i++)
            {
                var block_subset = blocks.Skip(i * bandsize).Take(bandsize).ToArray();
                Array.Sort(block_subset);
                m = CommonUtils.MathUtils.GetMedian(block_subset);

                for (j = i * bandsize; j < (i + 1) * bandsize; j++)
                {
                    v = blocks[j];
                    if (v > m || (Math.Abs(v - m) < 1 && m > half_block_value))
                    {
                        hash += "1";
                    }
                    else
                    {
                        hash += "0";
                    }
                }
            }

            return hash;
        }


        /// <summary>
        /// Convert  bits to hexadecimal string representation.
        /// </summary>
        ///
        /// <param name="bits">hash bitstring</param>
        /// 
        ///
        /// <remarks>
        /// Hash length should be a multiple of 4.
        /// </remarks>
        ///
        private static string bits_to_hexhash(string bits)
        {
           
            var hex = new StringBuilder();
            var len = bits.Length / 4;

            int tmp, b;
            for (int i = 0; i < len; i++)
            {
                tmp = 0;
                for (int j = 0; j < 4; j++)
                {
                    b = i * 4 + j;
                    tmp = tmp | ( (bits[b]-48) << 3 >> j);
                }

                var stmp = tmp.ToString("X");
                hex.Append(stmp);

            }

            return hex.ToString();
        }


    }
}