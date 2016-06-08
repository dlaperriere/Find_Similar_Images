// Compare images similarity with hashing algorithms
//
// Copyright (C) David Laperriere

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Images
{
    [Flags]
    public enum ImageHashAlgorithm { Average = 1, AverageHistogram, AverageColor, Block, BlockHistogram, BlockColor, Color, Difference, DifferenceHistogram, DifferenceColor, Histogram, MD5, Perceptive, PerceptiveHistogram, PerceptiveColor };

    /// <summary>
    /// Compare images similarity with hashing algorithms
    ///
    /// - Average and Perceptive hash are from https://github.com/perivar/FindSimilar/
    /// - Block Mean Value hash is based on https://github.com/commonsmachinery/blockhash/blob/master/blockhash.c
    /// - Difference hash is based on http://01101001.net/DifferenceHash.py
    /// - MD5 hash
    /// - RGB histogram hash
    ///
    /// </summary>
    ///
    /// <seealso cref="http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html"/>
    /// <seealso cref="http://www.hackerfactor.com/blog/index.php?/archives/529-Kind-of-Like-That.html"/>
    public class ImageHash
    {
        /// <summary>
        ///  internal variables
        /// </summary>
        private static CommonUtils.BitCounter bitCounter = new CommonUtils.BitCounter(8);

        #region main

        /// <summary>
        ///  Main used to compare the hashes of 2 images
        /// </summary>
        /// <param name="args">image1 image2</param>
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                System.Console.WriteLine("Must provide 2 images");
                System.Environment.Exit(-1);
            }

            var file1 = args[0];
            var file2 = args[1];

            if (!File.Exists(file1))
            {
                System.Console.WriteLine("{0} does not seem to be a valid image", file1);
                System.Environment.Exit(-1);
            }
            if (!File.Exists(file2))
            {
                System.Console.WriteLine("{0} does not seem to be a valid image", file2);
                System.Environment.Exit(-1);
            }

            foreach (ImageHashAlgorithm algo in Enum.GetValues(typeof(ImageHashAlgorithm)))
            {
                if (algo == ImageHashAlgorithm.Average ||
                    algo == ImageHashAlgorithm.Block ||
                    algo == ImageHashAlgorithm.Color ||
                    algo == ImageHashAlgorithm.Difference ||
                    algo == ImageHashAlgorithm.Histogram ||
                    algo == ImageHashAlgorithm.MD5 ||
                    algo == ImageHashAlgorithm.Perceptive)
                {
                    Console.WriteLine("{0} image hash algorithm ", algo);

                    var hash1 = HashImage(file1, algo);
                    Console.WriteLine("{0}\n Hash:{1}", file1, hash1);

                    var hash2 = HashImage(file2, algo);
                    Console.WriteLine("{0}\n Hash:{1}", file2, hash2);

                    var sim = HashSimilarity(hash1, hash2, algo);
                    Console.WriteLine(" similarity = {0:00.0}\n\n", sim);

                    // draw hash
                    var hashimg1 = DrawHash(hash1, algo);
                    hashimg1.Save(file1 + ".hash" + algo + ".png", ImageFormat.Png);
                    hashimg1.Dispose();

                    var hashimg2 = DrawHash(hash2, algo);
                    hashimg2.Save(file2 + ".hash" + algo + ".png", ImageFormat.Png);
                    hashimg2.Dispose();
                }
            }

            // draw smaller
            var image1 = Image.FromFile(file1, true);
            var smaller1 = CommonUtils.ImageUtils.Resize(image1, 8, 8);
            smaller1.Save(file1 + ".small.png", ImageFormat.Png);
            image1.Dispose();
            smaller1.Dispose();

            var image2 = Image.FromFile(file2, true);
            var smaller2 = CommonUtils.ImageUtils.Resize(image2, 8, 8);
            smaller2.Save(file2 + ".small.png", ImageFormat.Png);
            image2.Dispose();
            smaller2.Dispose();
        }

        #endregion main

        #region compare images hashes

        /// <summary>
        /// Compare 2 images hashes
        /// </summary>
        /// <param name="image_name1">path to 1st Image</param>
        /// <param name="image_name2">path to 2nd Image</param>
        /// <param name="hash">hash algorithm</param>
        /// <returns>similarity % [0,100]</returns>
        public static double Compare(String image_name1, String image_name2, Images.ImageHashAlgorithm hash)
        {
            Image image1 = Image.FromFile(image_name1, true);
            Image image2 = Image.FromFile(image_name2, true);

            var sim = Compare(image1, image2, hash);

            image1.Dispose();
            image2.Dispose();

            return sim;
        }

        /// <summary>
        /// Compare 2 images hashes
        /// </summary>
        /// <param name="image1">1st Image</param>
        /// <param name="image2">2nd Image</param>
        /// <param name="hash">hash algorithm</param>
        /// <returns>similarity % [0,100]</returns>
        public static double Compare(Image image1, Image image2, Images.ImageHashAlgorithm hash)
        {
            double cmp = 0;
            switch (hash)
            {
                case ImageHashAlgorithm.Average:
                    cmp = Images.ImageAverageHash.Compare(image1, image2);
                    break;

                case ImageHashAlgorithm.Block:
                    cmp = Images.ImageBlockHash.Compare(image1, image2);
                    break;

                case ImageHashAlgorithm.Color:
                    cmp = Images.ImageColorHash.Compare(image1, image2);
                    break;

                case ImageHashAlgorithm.Difference:
                    cmp = Images.ImageDifferenceHash.Compare(image1, image2);
                    break;

                case ImageHashAlgorithm.MD5:
                    cmp = Images.ImageMD5Hash.Compare(image1, image2);
                    break;

                case ImageHashAlgorithm.Perceptive:
                    cmp = Images.ImagePHash.Compare(image1, image2);
                    break;

                case ImageHashAlgorithm.Histogram:
                    cmp = Images.ImageHistogramHash.Compare(image1, image2);
                    break;

                default:
                    break;
            }
            return cmp;
        }

        #endregion compare images hashes

        #region draw

        /// <summary>
        /// Draw image representation of an image hash
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="hash_algo"></param>
        /// <returns></returns>
        public static Image DrawHash(string hash, Images.ImageHashAlgorithm hash_algo)
        {
            var img = (Image)new Bitmap(1, 1);
            switch (hash_algo)
            {
                case ImageHashAlgorithm.Average:
                    var ahashl = Convert.ToUInt64(hash);
                    var ahashb = CommonUtils.StringUtils.LongToBinaryString((long)ahashl);
                    img = HashBitStringToImage(ahashb);
                    break;

                case ImageHashAlgorithm.Block:
                    img = HashBitStringToImage(hash);
                    break;

                case ImageHashAlgorithm.Color:
                    img = (Image)CommonUtils.ImageUtils.MakeSquare(Color.FromArgb(Convert.ToInt32(hash)));
                    break;

                case ImageHashAlgorithm.Difference:
                    var dhashl = Convert.ToUInt64(hash);
                    var dhashb = CommonUtils.StringUtils.LongToBinaryString((long)dhashl);
                    img = HashBitStringToImage(dhashb);
                    break;

                case ImageHashAlgorithm.Histogram:
                    img = HashBitStringToImage(hash,9);
                    break;

                case ImageHashAlgorithm.MD5:
                    break;

                case ImageHashAlgorithm.Perceptive:
                    img = HashBitStringToImage(hash);
                    break;

                default:
                    break;
            }

            return img;
        }

        /// <summary>
        /// draw hash bitstring (0/1 string or 0-9 string)
        /// </summary>
        /// <param name="hash"></param>
        /// <returns>image representation of an image hash </returns>
        private static Image HashBitStringToImage(string hash, int max_value = 1)
        {
            double[] rawImage = new double[hash.Length];
            for (int i = 0; i < hash.Length; i++)
            {
                int value = Convert.ToInt32(hash[i]) - 48;
                rawImage[i] = value;
            }

            var image = new Bitmap(hash.Length, 1);

            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image))
            {
                g.Clear(Color.DarkGray);
                for (int i = 0; i < hash.Length; i++)
                {
                    double value = rawImage[i];

                    Color color;
                    if (max_value == 9)
                    {
                        // histogram hash (0-9 string)
                        if (value == 0) { color = Color.LightBlue; }
                        else if (value == 1) { color = Color.LightSteelBlue; }
                        else if (value == 2) { color = Color.SkyBlue; }
                        else if (value == 3) { color = Color.Turquoise; }
                        else if (value == 4) { color = Color.DeepSkyBlue; }
                        else if (value == 5) { color = Color.DodgerBlue; }
                        else if (value == 6) { color = Color.CadetBlue; }
                        else if (value == 7) { color = Color.MediumBlue; }
                        else if (value == 8) { color = Color.Navy; }
                        else { color = Color.MidnightBlue; }
                    }
                    else {
                        if (value == 1) { color = Color.MidnightBlue; }
                        else { color = Color.LightSteelBlue; }
                    }
                    

                    image.SetPixel(i, 0, color);
                }
            }

            return CommonUtils.ImageUtils.Resize(image, hash.Length, 8);
        }


        #endregion draw

        #region hash image

        /// <summary>
        /// Calculate image hash
        /// </summary>
        /// <param name="image_path"></param>
        /// <param name="hash_algo">hash algorithm</param>
        /// <returns>image hash</returns>
        public static string HashImage(string image_path, Images.ImageHashAlgorithm hash_algo)
        {
            Image image = Image.FromFile(image_path, true);
            var hash = HashImage(image, hash_algo);
            image.Dispose();

            return hash;
        }

        /// <summary>
        /// Calculate image hash
        /// </summary>
        /// <param name="image"></param>
        /// <param name="hash_algo">hash algorithm</param>
        /// <returns>image hash</returns>
        public static string HashImage(Image image, Images.ImageHashAlgorithm hash_algo)
        {
            string hash = String.Empty;
            switch (hash_algo)
            {
                case ImageHashAlgorithm.Average:
                    hash = Images.ImageAverageHash.AverageHash(image);
                    break;

                case ImageHashAlgorithm.Block:
                    hash = Images.ImageBlockHash.BlockHash(image);
                    break;

                case ImageHashAlgorithm.Color:
                    hash = Images.ImageColorHash.ColorHash(image);
                    break;

                case ImageHashAlgorithm.Difference:
                    hash = Images.ImageDifferenceHash.DifferenceHash(image);
                    break;

                case ImageHashAlgorithm.MD5:
                    hash = Images.ImageMD5Hash.MD5Hash(image);
                    break;

                case ImageHashAlgorithm.Perceptive:
                    hash = Images.ImagePHash.PerceptiveHash(image);
                    break;

                case ImageHashAlgorithm.Histogram:
                    hash = Images.ImageHistogramHash.HistogramHash(image);
                    break;

                default:
                    break;
            }
            return hash;
        }

        #endregion hash image

        #region image hash similarity

        /// <summary>
        ///  Calculate the similarity of 2 images hashes
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="hash_algo">hash algorithm</param>
        /// <returns>similarity % [0,100]</returns>
        public static double HashSimilarity(string first, string second, Images.ImageHashAlgorithm hash_algo)
        {
            double sim = 0;
           
            try
            {
                switch (hash_algo)
                {
                    case ImageHashAlgorithm.Average:
                        sim = Images.ImageAverageHash.Similarity(first, second);
                        break;

                    case ImageHashAlgorithm.Block:
                        sim = Images.ImageBlockHash.Similarity(first, second);
                        break;

                    case ImageHashAlgorithm.Color:
                        sim = Images.ImageColorHash.Similarity(first, second);
                        break;

                    case ImageHashAlgorithm.Difference:
                        sim = Images.ImageDifferenceHash.Similarity(first, second);
                        break;

                    case ImageHashAlgorithm.MD5:
                        sim = Images.ImageMD5Hash.Similarity(first, second);
                        break;

                    case ImageHashAlgorithm.Perceptive:
                        sim = Images.ImagePHash.Similarity(first, second);
                        break;

                    case ImageHashAlgorithm.Histogram:
                        sim = Images.ImageHistogramHash.Similarity(first, second);
                        break;

                    default:
                        break;
                }
            }
            catch(Exception e) {
                var msg = e.Message;
                //Console.WriteLine("error hash sim {0} vs {1} : {2}", first, second,msg);
            }
            return sim;
        }

        /// <summary>
        /// Hamming Distance of 2 strings
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns>Number of differences (lower values equals more similar)</returns>
        public static int HammingDistance(string first, string second)
        {
            // use shortest string
            int lengthS1 = first.Length;
            int lengthS2 = second.Length;
            int length = lengthS2 > lengthS1 ? lengthS1 : lengthS2;

            int counter = 0;
            for (int k = 0; k < length; k++)
            {
                if (first[k] != second[k])
                {
                    counter++;
                }
            }
            return counter;
        }

        /// <summary>
        /// Calculate the similarity of 2 bitstring hashes
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns>similarity % [0,100]</returns>
        public static double SimilarityBitString(string first, string second)
        {
            int different = HammingDistance(first, second);
            double perc = (double)different / (double)first.Length;
            return 100 * (1 - perc);
        }

        /// <summary>
        /// Calculate the similarity of 2 Uint64/ulong hashes
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns>similarity % [0,100]</returns>
        public static double SimilarityUtin64(ulong first, ulong second)
        {
            int s1 = bitCounter.CountOnesWithPrecomputation(first ^ second);
            return ((64 - s1) * 100) / 64.0;
        }

        #endregion image hash similarity
    } // class
} /// namespace