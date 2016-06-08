// MD5 hash of an image
//
// Copyright (C) David Laperriere.

using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Images
{
    public class ImageMD5Hash
    {
        /// <summary>
        /// Compare 2 images MD5 hashes
        /// </summary>
        /// <param name="image1">The first image.</param>
        /// <param name="image2">The second image.</param>
        /// <returns>similarity % [0,100]</returns>
        public static double Compare(Image image1, Image image2)
        {
            bool same = false;

            if (image1 != null && image2 != null)
            {
                String hash1 = MD5Hash(image1);
                String hash2 = MD5Hash(image2);

                same = hash1.Equals(hash2);
            }

            return same ? 100.0 : 0.0;
        }


        /// <summary>
        ///  Calculate the MD5 hash an image
        /// </summary>
        /// <param name="image">The image to hash.</param>
        /// <returns>MD5 hash</returns>
        public static string MD5Hash(Image image)
        {
            ImageConverter converter = new ImageConverter();
            byte[] rawImageData = converter.ConvertTo(image, typeof(byte[])) as byte[];

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] md5Hash = md5.ComputeHash(rawImageData);

            // format as a hexadecimal string.
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < md5Hash.Length; i++)
            {
                sBuilder.Append(md5Hash[i].ToString("x2"));
            }

            return sBuilder.ToString();

        }

        /// <summary>
        /// Calculate the MD5 hash of the image  in the given file.
        /// </summary>
        /// <param name="path">Path to the input file.</param>
        /// <returns>average hash</returns>
        public static string MD5Hash(String path)
        {
            Image img = Image.FromFile(path, true);
            var hash = MD5Hash(img);

            return hash;
        }

        /// <summary>
        /// Calculate the similarity of 2 hashes
        /// </summary>
        /// <param name="hash1">The first hash.</param>
        /// <param name="hash2">The second hash.</param>
        /// <returns>similarity %(0 or 100)</returns>
        public static double Similarity(string hash1, string hash2)
        {
            double sim = 0.0;

            if (hash1.Equals(hash2))
            {
                sim = 100.0;
            }

            return sim;
        }
    }
}