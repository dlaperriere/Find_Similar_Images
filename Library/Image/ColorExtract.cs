// C# port of Kepler Gelotte Image Color Extract from http://www.coolphptools.com/color_extract

/**
 * colors.inc.php (colorextract v1.1)
 * ----------------------------------
 * This class can be used to get the most common colors in an image.
 * It needs one parameter:
 * 	image - the filename of the image you want to process.
 * Optional parameters:
 *
 *	count - how many colors should be returned. 0 mmeans all. default=20
 *	reduce_brightness - reduce (not eliminate) brightness variants? default=true
 *	reduce_gradients - reduce (not eliminate) gradient variants? default=true
 *	delta - the amount of gap when quantizing color values.
 *		Lower values mean more accurate colors. default=16
 *
 * Author: 	Csongor Zalatnai
 *
 * Modified By: Kepler Gelotte - Added the gradient and brightness variation
 * 	reduction routines. Kudos to Csongor for an excellent class. The original
 * 	version can be found at:
 *
 *	http://www.phpclasses.org/browse/package/3370.html (BSD License)
 *
 */

// modifications
// - work internally with RGB colors instead of hex
//
// additions
// - Compare
// - Draw
// - Main, MainColor & TopColors
//
// Copyright (C) David Laperriere

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Images
{
    internal class ColorExtract
    {
        /// <summary>
        /// Image size used for calculations
        /// </summary>
        private static int smaller_size = Images.ImageSimilarity.SmallerSize;

        /// <summary>
        /// number of colors used by TopColors
        /// </summary>
        public static int top_colors = 20;

        /// <summary>
        /// Main used draw to dominant colors of an image
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                System.Console.WriteLine("Must provide an image");
                System.Environment.Exit(-1);
            }

            var file = args[0];

            if (!File.Exists(file))
            {
                System.Console.WriteLine("{0} does not seem to be a valid image", file);
                System.Environment.Exit(-1);
            }

            var main_color = MainColor(file);
            foreach (var color in main_color)
            {
                var name = GetColorName(color.Key);

                Console.WriteLine("({0:000} {1:000} {2:000}) : {3:00.0} {4}", color.Key.R, color.Key.G, color.Key.B, 100 * color.Value, name);
            }
            Console.WriteLine("% {0:00.0}\n", 100 * main_color.Values.Sum());

            var imagem = Draw(main_color, 12, 12);
            imagem.Save(file + ".main_color.bmp");

            var top_colors = TopColors(file);
            foreach (var color in top_colors)
            {
                var name = GetColorName(color.Key);

                Console.WriteLine("({0:000} {1:000} {2:000}) : {3:00.0} {4}", color.Key.R, color.Key.G, color.Key.B, 100 * color.Value, name);
            }
            Console.WriteLine("% {0:00.0}", 100 * top_colors.Values.Sum());

            var imaget = Draw(top_colors, 50, 12);
            imaget.Save(file + ".top_colors.bmp");
        }

        //http://stackoverflow.com/questions/11728209/how-to-get-the-name-of-color-while-having-its-rgb-value-in-c
        /// <summary>
        ///  Get name of a color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        internal static String GetColorName(Color color)
        {
            var predefined = typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static);
            var match = (from p in predefined where ((Color)p.GetValue(null, null)).ToArgb() == color.ToArgb() select (Color)p.GetValue(null, null));
            if (match.Any())
                return match.First().Name;
            return String.Empty;
        }

        /// <summary>
        /// Draw dominants colors of an image
        /// </summary>
        /// <param name="colors"></param>
        /// <returns></returns>
        public static Bitmap Draw(Dictionary<Color, double> colors, int width = 50, int height = 10)
        {
            var color_img = new Bitmap(width, height);

            Graphics g = Graphics.FromImage(color_img);

            // backgound
            g.FillRectangle(Brushes.White, 0, 0, color_img.Width, color_img.Height);

            // draw dominant colors
            int ncolors = colors.Count;
            var sumcolors = colors.Values.Sum();
            int x = 0;

            for (int i = 0; i < colors.Count; i++)
            {
                var color = colors.ElementAt(i).Key;
                var brush = new SolidBrush(color);
                var cwidth = (int)((double)width * (colors[color] / sumcolors)) + 1;

                g.FillRectangle(brush, new Rectangle(x, 0, cwidth, height));
                x += cwidth;
            }

            g.Dispose();

            return color_img;
        }

        /// <summary>
        /// Compare Top colors of 2 images
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns>similarity % [0,100]</returns>
        static public double CompareTop(Image image1, Image image2)
        {
            double sim = 0;

            var top_color1 = Images.ColorExtract.TopColors(image1);
            var top_color2 = Images.ColorExtract.TopColors(image2);

            var top_color1_img = (Image)Images.ColorExtract.Draw(top_color1);
            var top_color2_img = (Image)Images.ColorExtract.Draw(top_color2);
            double sim_top_color = Images.ImageSimilarity.CompareImages(top_color1_img, top_color2_img, ComparisonMethod.PixelsDifferenceSorted);

            top_color1_img.Dispose();
            top_color2_img.Dispose();

            sim = sim_top_color;

            return sim;
        }

        /// <summary>
        /// Compare Top colors of 2 Base64 images
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns>similarity % [0,100]</returns>
        static public double CompareTop(string image1, string image2)
        {
            double sim = 0;

            var image1_bytes = Convert.FromBase64String(image1);
            var top_color1_img = Image.FromStream(new MemoryStream(image1_bytes));

            var image2_bytes = Convert.FromBase64String(image2);
            var top_color2_img = Image.FromStream(new MemoryStream(image2_bytes));

            double sim_top_color = Images.ImageSimilarity.CompareImages(top_color1_img, top_color2_img, ComparisonMethod.PixelsDifferenceSorted);

            sim = sim_top_color;

            top_color1_img.Dispose();
            top_color2_img.Dispose();
            return sim;
        }

        /// <summary>
        /// Compare 2 images color dictionaries
        /// </summary>
        /// <param name="colors1"></param>
        /// <param name="colors2"></param>
        /// <returns></returns>
        static public double Compare(Dictionary<Color, double> colors1, Dictionary<Color, double> colors2)
        {
            double sim = 0;

            var ncolors1 = colors1.Count;
            var ncolors2 = colors2.Count;

            if (ncolors1 == 0) { return 0; }
            if (ncolors2 == 0) { return 0; }

            // case 1 color:  0 or 100%
            if (ncolors1 == 1 && ncolors2 == 1)
            {
                var color1i = colors1.ElementAt(0).Key.ToArgb();
                var color2i = colors2.ElementAt(0).Key.ToArgb();

                if (Math.Abs(color1i - color2i) <= Images.ImageSimilarity.Tolerance)
                {
                    sim = 100.0;
                }
                return sim;
            }

            // case > 1 color : % common

            /// 1 vs 2
            /// count % pixels of the same color
            double total1 = 0;
            double common1 = 0;
            double sim1v2 = 0;
            foreach (var color in colors1)
            {
                var c = color.Key;
                var p = color.Value;
                total1 += p;

                if (colors2.ContainsKey(c))
                {
                    var p2 = colors2[c];

                    if (p >= p2)
                    {
                        common1 += p2;
                    }
                    else
                    {
                        common1 += p;
                    }
                }
                else
                {
                    common1 += 0;
                }
            }
            sim1v2 = 100.0 * common1 / total1;

            /// 2 vs 1
            /// count % pixels of the same color
            double total2 = 0;
            double common2 = 0;
            double sim2v1 = 0;
            foreach (var color in colors2)
            {
                var c = color.Key;
                var p = color.Value;
                total2 += p;

                if (colors1.ContainsKey(c))
                {
                    var p2 = colors1[c];
                    if (p >= p2)
                    {
                        common2 += p2;
                    }
                    else
                    {
                        common2 += p;
                    }
                }
                else
                {
                    common2 += 0;
                }
            }
            sim2v1 = 100.0 * common2 / total2;

            sim = 0.5 * (sim1v2 + sim2v1);

            return sim;
        }

        /// <summary>
        /// Find Colors of an image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="count">nomber of colors to return</param>
        /// <param name="reduce_brightness"></param>
        /// <param name="reduce_gradients"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static Dictionary<Color, double> Colors(string image_name, int count = 5, bool reduce_brightness = true, bool reduce_gradients = false, int delta = 80)
        {
            var image = Image.FromFile(image_name, true);
            var colors = Colors(image, count, reduce_brightness, reduce_gradients, delta);
            image.Dispose();

            return colors;
        }

        /// <summary>
        /// Most frequent color of an image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Dictionary<Color, double> MainColor(string image_name)
        {
            var image = Image.FromFile(image_name, true);
            var mcolor = MainColor(image);
            image.Dispose();
            return mcolor;
        }

        /// <summary>
        /// Most frequent color of an image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Dictionary<Color, double> MainColor(Image image)
        {
            return Colors(image, count: 1, reduce_brightness: true, reduce_gradients: true, delta: 35);
        }

        /// <summary>
        /// Most frequent colors of an image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Dictionary<Color, double> TopColors(string image_name)
        {
            var image = Image.FromFile(image_name, true);
            var tcolors = TopColors(image);
            image.Dispose();
            return tcolors;
        }

        /// <summary>
        /// Most frequent colors of an image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Dictionary<Color, double> TopColors(Image image)
        {
            //return Colors(image, count: 5, reduce_brightness: true, reduce_gradients: false, delta: 80);
            return Colors(image, count: top_colors, reduce_brightness: true, reduce_gradients: true, delta: 12);
        }

        /// <summary>
        /// Find Colors of an image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="count">how many colors should be returned. 0 means all.</param>
        /// <param name="reduce_brightness">reduce (not eliminate) brightness variants?</param>
        /// <param name="reduce_gradients">reduce (not eliminate) gradient variants?</param>
        /// <param name="delta">he amount of gap when quantizing color values. Lower values mean more accurate colors.</param>
        /// <returns>Color -> % dictionary</returns>
        public static Dictionary<Color, double> Colors(Image image, int count = 5, bool reduce_brightness = true, bool reduce_gradients = false, int delta = 80)
        {
            bool image_is_readable = false;
            var colorarray = new Dictionary<Color, double>();
            var arr = new Dictionary<Color, double>();
            var im = new Bitmap(1, 1);
            try
            {
                im = (Bitmap)image;
                if (im.Width > smaller_size || im.Height > smaller_size)
                {
                    im = (Bitmap)CommonUtils.ImageUtils.Resize(im, smaller_size, smaller_size, System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor);
                }

                image_is_readable = true;
            }
            catch { }

            if (image_is_readable)
            {
                int half_delta = 0;
                if (delta > 2)
                {
                    half_delta = delta / 2 - 1;
                }
                else
                {
                    half_delta = 0;
                }

                double total_pixel_count = 0;

                // for faster pixels access
                // http://csharpexamples.com/fast-image-processing-c/
                unsafe
                {
                    BitmapData bitmapData = im.LockBits(new Rectangle(0, 0, im.Width, im.Height), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(im.PixelFormat) / 8;
                    int heightInPixels = bitmapData.Height;
                    int widthInBytes = bitmapData.Width * bytesPerPixel;
                    byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                    for (int y = 0; y < heightInPixels; y++)
                    {
                        byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                        for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                        {
                            int R = currentLine[x + 2];
                            int G = currentLine[x + 1];
                            int B = currentLine[x];
                            var A = currentLine[x + 3];
                            if (A == 0) { continue; } // skip transparent

                            total_pixel_count++;
                            // ROUND THE COLORS, TO REDUCE THE NUMBER OF DUPLICATE COLORS
                            if (delta > 1)
                            {
                                R = (((R + half_delta) / delta) * delta);
                                G = (((G + half_delta) / delta) * delta);
                                B = (((B + half_delta) / delta) * delta);

                                if (R >= 256)
                                {
                                    R = 255;
                                }
                                if (G >= 256)
                                {
                                    G = 255;
                                }
                                if (B >= 256)
                                {
                                    B = 255;
                                }
                            }

                            var color = Color.FromArgb(R, G, B);
                            if (!colorarray.ContainsKey(color))
                            {
                                colorarray[color] = 1;
                            }
                            else
                            {
                                colorarray[color]++;
                            }
                        }
                    }
                    im.UnlockBits(bitmapData);
                    im.Dispose();
                }

                // Reduce gradient colors
                if (reduce_gradients)
                {
                    var gradients = new Dictionary<Color, Color>();

                    var new_color = Color.Black;

                    //foreach (var hex in hexarray.Keys)
                    for (int i = 0; i < colorarray.Count; i++)
                    {
                        var color = colorarray.ElementAt(i);

                        if (!gradients.ContainsKey(color.Key))
                        {
                            new_color = _find_adjacent(color.Key, gradients, delta);
                            gradients[color.Key] = new_color;
                        }
                        else
                        {
                            new_color = gradients[color.Key];
                        }

                        if (color.Key != new_color)
                        {
                            colorarray[color.Key] = 0;
                            colorarray[new_color] += color.Value;
                        }
                    }
                }

                // Reduce brightness variations
                if (reduce_brightness)
                {
                    var brightness = new Dictionary<Color, Color>();

                    for (int i = 0; i < colorarray.Count; i++)
                    {
                        Color new_color = Color.Black;
                        var color = colorarray.ElementAt(i);
                        if (!brightness.ContainsKey(color.Key))
                        {
                            new_color = _normalize(color.Key, brightness, delta);
                            brightness[color.Key] = new_color;
                        }
                        else
                        {
                            new_color = brightness[color.Key];
                        }

                        if (color.Key != new_color)
                        {
                            colorarray[color.Key] = 0;
                            colorarray[new_color] += color.Value;
                        }
                    }
                }

                // convert counts to percentages

                for (int i = 0; i < colorarray.Count; i++)
                {
                    colorarray[colorarray.ElementAt(i).Key] = colorarray.ElementAt(i).Value / total_pixel_count;
                }

                if (count > 0)
                {
                    var sortedhexarray = from entry in colorarray orderby entry.Value descending select entry.Key; ;

                    foreach (var k in sortedhexarray)
                    {
                        if (count == 0)
                        {
                            break;
                        }
                        count--;
                        arr[k] = colorarray[k];
                    }
                    return arr;
                }
                else
                {
                    return colorarray;
                }
            }
            else
            {
                //System.Console.Error.WriteLine("Image {0} does not exist or is unreadable", image_name);
                return colorarray;
            }
        }

        /// <summary>
        /// Normalize color
        /// </summary>
        /// <param name="color"></param>
        /// <param name="colorarray"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        static internal Color _normalize(Color color, Dictionary<Color, Color> colorarray, int delta)
        {
            int lowest = 255;
            int highest = 0;
            var R = (int)color.R;
            var G = (int)color.G;
            var B = (int)color.B;

            if (R < lowest)
            {
                lowest = R;
            }
            if (G < lowest)
            {
                lowest = G;
            }
            if (B < lowest)
            {
                lowest = B;
            }

            if (R > highest)
            {
                highest = R;
            }
            if (G > highest)
            {
                highest = G;
            }
            if (B > highest)
            {
                highest = B;
            }

            // Do not normalize white, black, or shades of grey unless low delta
            if (lowest == highest)
            {
                if (delta <= 32)
                {
                    if (lowest == 0 || highest >= (255 - delta))
                    {
                        return color;
                    }
                }
                else
                {
                    return color;
                }
            }

            for (; lowest < 256 && highest < 256; lowest += delta, highest += delta)
            {
                var new_R = R - lowest;
                var new_G = G - lowest;
                var new_B = B - lowest;

                if (new_R < 0 || new_G < 0 || new_B < 0) { continue; }

                var new_color = Color.FromArgb(new_R, new_G, new_B);
                if (colorarray.ContainsKey(new_color))
                {
                    // same color, different brightness - use it instead
                    return new_color;
                }
            }

            return color;
        }

        /// <summary>
        /// Find adjacent color
        /// </summary>
        /// <param name="color"></param>
        /// <param name="gradients"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        static internal Color _find_adjacent(Color color, Dictionary<Color, Color> gradients, int delta)
        {
            var red = (int)color.R;
            var green = (int)color.G;
            var blue = (int)color.B;
            Color new_color = Color.Black;
            if (red > delta)
            {
                new_color = Color.FromArgb(red - delta, green, blue);

                if (gradients.ContainsKey(new_color))
                {
                    return gradients[new_color];
                }
            }
            if (green > delta)
            {
                new_color = Color.FromArgb(red, green - delta, blue);

                if (gradients.ContainsKey(new_color))
                {
                    return gradients[new_color];
                }
            }
            if (blue > delta)
            {
                new_color = Color.FromArgb(red, green, blue - delta);

                if (gradients.ContainsKey(new_color))
                {
                    return gradients[new_color];
                }
            }

            if (red < (255 - delta))
            {
                new_color = Color.FromArgb(red + delta, green, blue);

                if (gradients.ContainsKey(new_color))
                {
                    return gradients[new_color];
                }
            }
            if (green < (255 - delta))
            {
                new_color = Color.FromArgb(red, green + delta, blue);

                if (gradients.ContainsKey(new_color))
                {
                    return gradients[new_color];
                }
            }
            if (blue < (255 - delta))
            {
                new_color = Color.FromArgb(red, green, blue + delta);

                if (gradients.ContainsKey(new_color))
                {
                    return gradients[new_color];
                }
            }

            return color;
        }
    }
}