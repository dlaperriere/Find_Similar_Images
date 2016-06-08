// Similar image informations
//
// Copyright (C) David Laperriere

using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Images.DataBinding
{
    [Serializable]
    public class SimilarImage
    {
        #region Accessors

        public double Distance { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public BitmapImage Colors { get; set; }

        #endregion Accessors

        #region constructor

        /// <summary>
        /// Similar image informations
        /// </summary>
        /// <param name="image_path"></param>
        /// <param name="colors"></param>
        /// <param name="similarity"></param>
        public SimilarImage(string image_path, System.Drawing.Image colors, double similarity)
        {
            this.ImageName = Path.GetFileName(image_path);
            this.ImagePath = image_path;
            BitmapImage image = new BitmapImage();
            MemoryStream ms = new MemoryStream();
            colors.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            image.BeginInit();
            image.StreamSource = ms;
            image.EndInit();
            this.Colors = image;
            this.Distance = similarity;

            
        }

        #endregion constructor

        #region Clone

        public SimilarImage Clone()
        {
            System.IO.MemoryStream m = new System.IO.MemoryStream();
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter b =
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            b.Serialize(m, this);
            m.Position = 0;
            return (SimilarImage)b.Deserialize(m);
        }

        #endregion Clone
    }
}