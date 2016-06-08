// Compare images : colors, pixels, features 
//
// Copyright (C) David Laperriere

using OpenCvSharp; //https://github.com/shimat/opencvsharp
using OpenCvSharp.Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Windows.Media.Media3D;

namespace Images
{
    /// <summary>
    ///  Image comparison methods
    /// </summary>
    [Flags]
    public enum ComparisonMethod { None, TopColors, MainColor, PixelsDifference, PixelsDifferenceSorted, PixelsDistance, PixelsDistanceSorted, Feature, RGBHistogramHash };

    /// <summary>
    /// Compare the RGB pixels of images
    /// </summary>
    internal class ImageSimilarity
    {
        /// <summary>
        /// Image size used for calculations
        /// </summary>
        public static int SmallerSize = 36;

        /// <summary>
        /// Difference tolerated for pixel comparison
        /// </summary>
        public static double Tolerance = 25;
        public static double TolerancePercent = Tolerance / 255.0;


        #region main
        /// <summary>
        /// Main used to compare the 2 images
        /// </summary>
        /// <param name="args">file1 file2</param>
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
                System.Console.WriteLine("file {0} does not appear to be a valid file...", file1);
                System.Environment.Exit(-1);
            }
            if (!File.Exists(file1))
            {
                System.Console.WriteLine("file {0} does not appear to be a valid file...", file1);
                System.Environment.Exit(-1);
            }

            try
            {
                var image1 = Image.FromFile(file1, true);
                var image2 = Image.FromFile(file2, true);
                image1 = CommonUtils.ImageUtils.Resize(image1, SmallerSize, SmallerSize, false);
                image2 = CommonUtils.ImageUtils.Resize(image2, SmallerSize, SmallerSize, false);

                foreach (ComparisonMethod method in Enum.GetValues(typeof(ComparisonMethod)))
                {
                    if (method == ComparisonMethod.None) { continue; }
                    if (method == ComparisonMethod.Feature) { continue; }

                    var sim = CompareImages(image1, image2, method);
                    Console.WriteLine("{0} similarity = {1:00.0}", method, sim);
                }

                Console.WriteLine(String.Empty);
                Console.WriteLine("Compare images by Feature");
                double simf = -1;

                double feature;
                double matchs;
                Bitmap view;
                simf = CompareFeatures(image1, image2, out feature, out matchs, out view);
                Console.WriteLine("similarity = {0:00.0} (feature {1} matches {2} good matches)", simf, feature, matchs, matchs * simf / 100.0);

                image1.Dispose();
                image2.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine("CompareImages error :\n{0}", e);
            }
        }

        #endregion

        #region compare images
        /// <summary>
        ///  Compare images
        /// </summary>
        /// <param name="image_name1"> 1st image</param>
        /// <param name="image_name2"> 2nd image</param>
        /// <param name="method"> method</param>
        /// <returns>Similarity % )</returns>
        public static double CompareImages(string image_name1, string image_name2, ComparisonMethod method)
        {
            var image1 = Image.FromFile(image_name1, true);
            var image2 = Image.FromFile(image_name2, true);
            var sim = CompareImages(image1, image2, method);

            image1.Dispose();
            image2.Dispose();

            return sim;
        }

        /// <summary>
        ///  Compare images
        /// </summary>
        /// <param name="image1"> 1st image</param>
        /// <param name="image2"> 2nd image</param>
        /// <param name="method"> method</param>
        /// <returns>Similarity % )</returns>
        public static double CompareImages(Image image1, Image image2, ComparisonMethod method)
        {
            double sim = 0;
            try
            {
                var mat1 = BitmapConverter.ToMat(new Bitmap(image1));
                var mat2 = BitmapConverter.ToMat(new Bitmap(image2));

                sim = CompareImages(mat1, mat2, method);

                mat1.Dispose();
                mat2.Dispose();
            }
            catch (System.AccessViolationException)
            {
                //Console.Error.WriteLine("Access Error  => CompareImages {0} : \n{1}", method, e);
            }
            catch (OutOfMemoryException)
            {
                //The file does not have a valid image format.
                //-or- GDI+ does not support the pixel format of the file
                //Console.Error.WriteLine("Memory Error  => CompareImages {0} : \n{1}", method, e);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error  => CompareImages {0} : \n{1}", method, e);
            }
            return sim;
        }
        /// <summary>
        ///  Compare images
        /// </summary>
        /// <param name="image1"> 1st image</param>
        /// <param name="image2"> 2nd image</param>
        /// <param name="method"> method</param>
        /// <returns>Similarity % )</returns>
        public static double CompareImages(Mat image1, Mat image2, ComparisonMethod method)
        {
            double sim = 0;

            switch (method)
            {
                case ComparisonMethod.None:
                    break;

                case ComparisonMethod.TopColors:
                    var bmp1 = (Image)BitmapConverter.ToBitmap(image1);
                    var bmp2 = (Image)BitmapConverter.ToBitmap(image2);
                    sim = Images.ColorExtract.CompareTop(bmp1, bmp2);
                    bmp1.Dispose();
                    bmp2.Dispose();
                    break;

                case ComparisonMethod.MainColor:
                    bmp1 = (Image)BitmapConverter.ToBitmap(image1);
                    bmp2 = (Image)BitmapConverter.ToBitmap(image2);
                    var main1 = Images.ColorExtract.MainColor(bmp1);
                    var main2 = Images.ColorExtract.MainColor(bmp2);
                    sim = Images.ColorExtract.Compare(main1, main2);
                    bmp1.Dispose();
                    bmp2.Dispose();
                    break;

                case ComparisonMethod.PixelsDifference:
                    sim = ComparePixels(image1, image2);
                    break;

                case ComparisonMethod.PixelsDifferenceSorted:
                    sim = ComparePixelsSorted(image1, image2);
                    break;

                case ComparisonMethod.PixelsDistance:
                    sim = ComparePixelsDistance(image1, image2);
                    break;

                case ComparisonMethod.PixelsDistanceSorted:
                    sim = ComparePixelsDistanceSorted(image1, image2);
                    break;

                case ComparisonMethod.Feature:
                    double feature;
                    double matchs;
                    Bitmap view;
                    try
                    {
                        sim = CompareFeatures(image1, image2, out feature, out matchs, out view);
                    }
                    catch (System.AccessViolationException e)
                    {
                        Console.Error.WriteLine("Access Error  => CompareImages {0} : \n{1}", method, e);
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine("Error  => CompareImages {0} : \n{1}", method, e);
                    }
                    break;

                case ComparisonMethod.RGBHistogramHash:
                    sim = Images.ImageHistogramHash.Compare(image1, image2);
                    break;

                default:
                    break;
            }

            return sim;
        }

        #endregion

        #region compare 2 pixels

        /// <summary>
        /// Calculate similarity of 2 pixels RGB values
        /// </summary>
        /// <param name="pixel1"></param>
        /// <param name="pixel2"></param>
        /// <returns> 0 or 100% (same RGB value +/- tolerance) </returns>
        public static double Compare2Pixels(Color pixel1, Color pixel2)
        {
            double sim = 0;

            if (Math.Abs((double)pixel1.R - (double)pixel2.R) <= Tolerance &&
                Math.Abs((double)pixel1.G - (double)pixel2.G) <= Tolerance &&
                Math.Abs((double)pixel1.B - (double)pixel2.B) <= Tolerance)
            {
                sim = 100.0;
            }

            return sim;
        }

        /// <summary>
        /// Calculate similarity of 2 pixels based on the distance between RGB values
        /// </summary>
        /// <param name="pixel1"></param>
        /// <param name="pixel2"></param>
        /// <returns> 0 or 100% (close colors) </returns>
        public static double Compare2Pixels_distance(Color pixel1, Color pixel2)
        {
            // Distance formula from http://www.compuphase.com/cmetric.htm
            // weighted Euclidean distance that approximate CIE L*u*v*
            //
            // Other distance methods are available at https://github.com/THEjoezack/ColorMine

            double sim = 0;

            long rmean = ((long)pixel1.R + (long)pixel2.R) / 2;
            long r = (long)pixel1.R - (long)pixel2.R;
            long g = (long)pixel1.G - (long)pixel2.G;
            long b = (long)pixel1.B - (long)pixel2.B;
            var dist = Math.Sqrt((((512 + rmean) * r * r) >> 8) + 4 * g * g + (((767 - rmean) * b * b) >> 8));

            double dist_bw = 764.833315173967; // distance black vs white

            sim = 100 - (100 * dist / dist_bw);

            return sim;
        }

        #endregion

        #region images features

        // http://docs.opencv.org/3.0-beta/modules/features2d/doc/features2d.html
        // http://docs.opencv.org/3.0-beta/modules/features2d/doc/feature_detection_and_description.html
        // http://docs.opencv.org/3.0-beta/doc/tutorials/features2d/akaze_matching/akaze_matching.html
        /// <summary>
        ///  Compare images with a feature detection algorithm
        /// </summary>
        /// <param name="mat_image1"> 1st image (OpenCv Mat)</param>
        /// <param name="mat_image2"> 2nd image (OpenCv Mat)</param>
        /// <param name="feature_count">number of feature keypoints found</param>
        /// <param name="match_count">number of matches founds</param>
        /// <param name="view">image of the feature and good matches</param>
        /// <returns>Similarity % (#good matches/ # matches)</returns>
        [HandleProcessCorruptedStateExceptions]
        public static double CompareFeatures(Image image1, Image image2, out double feature_count, out double match_count, out Bitmap view)
        {
            int image_max_size = 300;
            double img2v1 = 0;
            double img1v2 = 0;

            feature_count = 0;
            match_count = 0;
            view = new Bitmap(1, 1);

            try
            {
                // resize image to speedup calculation and limit memory usage
                var bmp1 = new Bitmap(image1);
                if (image1.Width > image_max_size || image1.Height > image_max_size)
                {
                    bmp1 = (Bitmap)CommonUtils.ImageUtils.Resize(bmp1, image_max_size, image_max_size, System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor);
                }

                var bmp2 = new Bitmap(image2);
                if (image2.Width > image_max_size || image2.Height > image_max_size)
                {
                    bmp2 = (Bitmap)CommonUtils.ImageUtils.Resize(bmp2, image_max_size, image_max_size, System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor);
                }

                var mat_image1 = BitmapConverter.ToMat(bmp1);
                var mat_image2 = BitmapConverter.ToMat(bmp2);

                // stop here if one of the image does not seem to be valid
                if (mat_image1 == null) { return 0; }
                if (mat_image1.Empty()) { return 0; }
                if (mat_image2 == null) { return 0; }
                if (mat_image2.Empty()) { return 0; }

                // remove alpha channel
                if (mat_image1.Channels() == 4)
                {
                    Cv2.CvtColor(mat_image1, mat_image1, ColorConversionCodes.BGRA2BGR);
                }
                if (mat_image2.Channels() == 4)
                {
                    Cv2.CvtColor(mat_image2, mat_image2, ColorConversionCodes.BGRA2BGR);
                }

                double feature_count2 = 0;
                double match_count2 = 0;
                var view2 = new Bitmap(1, 1);
                img1v2 = CompareFeatures(mat_image1, mat_image2, out feature_count2, out match_count2, out view2);

                if (img1v2 <= 0)
                {
                    // no matchs : show images side by side
                    view = CommonUtils.ImageUtils.MergeImages(image1, image2);
                    return 0;
                }
                img2v1 = CompareFeatures(mat_image2, mat_image1, out feature_count, out match_count, out view);

                mat_image1.Dispose();
                mat_image2.Dispose();
                bmp1.Dispose();
                bmp2.Dispose();
            }
            catch (System.AccessViolationException e)
            {
                System.Console.WriteLine("AccessError  => Feature compare:\n{0}", e);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error  => Feature compare:\n{0}", e);
            }

            return (img1v2 + img2v1) / 2.0;
        }

        // http://docs.opencv.org/3.0-beta/modules/features2d/doc/features2d.html
        // http://docs.opencv.org/3.0-beta/modules/features2d/doc/feature_detection_and_description.html
        // http://docs.opencv.org/3.0-beta/doc/tutorials/features2d/akaze_matching/akaze_matching.html
        /// <summary>
        ///  Compare images with a feature detection algorithm
        /// </summary>
        /// <param name="mat_image1"> 1st image (OpenCv Mat)</param>
        /// <param name="mat_image2"> 2nd image (OpenCv Mat)</param>
        /// <param name="feature_count">number of feature keypoints found</param>
        /// <param name="match_count">number of matches founds</param>
        /// <param name="view">image of the feature and good matches</param>
        /// <returns>Similarity % (#good matches/ # matches)</returns>
        private static double CompareFeatures(Mat mat_image1, Mat mat_image2, out double feature_count, out double match_count, out Bitmap view)
        {
            match_count = 0;
            feature_count = 0;

            int nmatch = 0;
            int ngmatch = 0;

            view = new Bitmap(1, 1);

            // stop here if one of the image does not seem to be valid
            if (mat_image1 == null) { return 0; }
            if (mat_image1.Empty()) { return 0; }
            if (mat_image2 == null) { return 0; }
            if (mat_image2.Empty()) { return 0; }

            try
            {
                // Detect the keypoints and generate their descriptors

                var detector = AKAZE.Create();
                //var detector = BRISK.Create();
                //var detector = ORB.Create(); // require grayscale

                /*
                // grayscale
                Cv2.CvtColor(mat_image1, mat_image1, ColorConversionCodes.BGR2GRAY);
                Cv2.CvtColor(mat_image2, mat_image2, ColorConversionCodes.BGR2GRAY);
                mat_image1.EqualizeHist();
                mat_image2.EqualizeHist();
                */

                var descriptors1 = new MatOfFloat();
                var descriptors2 = new MatOfFloat();
                var keypoints1 = new KeyPoint[1];
                var keypoints2 = new KeyPoint[1];
                try
                {
                    keypoints1 = detector.Detect(mat_image1);
                    keypoints2 = detector.Detect(mat_image2);
                    if (keypoints1 != null)
                    {
                        detector.Compute(mat_image1, ref keypoints1, descriptors1);
                        if (descriptors1 == null) { return 0; }
                    }
                    if (keypoints2 != null)
                    {
                        detector.Compute(mat_image2, ref keypoints2, descriptors2);
                        if (descriptors2 == null) { return 0; }
                    }
                }
                catch (System.AccessViolationException) { }
                catch (Exception) { }

                // Find good matches  (Nearest neighbor matching ratio)
                float nn_match_ratio = 0.95f;

                var matcher = new BFMatcher(NormTypes.Hamming);
                var nn_matches = new DMatch[1][];
                try
                {
                    nn_matches = matcher.KnnMatch(descriptors1, descriptors2, 2);
                }
                catch (System.AccessViolationException) { }
                catch (Exception) { }

                var good_matches = new List<DMatch>();
                var matched1 = new List<KeyPoint>();
                var matched2 = new List<KeyPoint>();
                var inliers1 = new List<KeyPoint>();
                var inliers2 = new List<KeyPoint>();

                if (nn_matches != null && nn_matches.Length > 0)
                {
                    for (int i = 0; i < nn_matches.GetLength(0); i++)
                    {
                        if (nn_matches[i].Length >= 2)
                        {
                            DMatch first = nn_matches[i][0];
                            float dist1 = nn_matches[i][0].Distance;
                            float dist2 = nn_matches[i][1].Distance;

                            if (dist1 < nn_match_ratio * dist2)
                            {
                                good_matches.Add(first);
                                matched1.Add(keypoints1[first.QueryIdx]);
                                matched2.Add(keypoints2[first.TrainIdx]);
                            }
                        }
                    }
                }

                // Count matches & features
                feature_count = keypoints1.Length + keypoints2.Length;
                nmatch = nn_matches.Length;
                match_count = nmatch;
                ngmatch = good_matches.Count;

                // Draw matches view
                var mview = new Mat();

                // show images + good matchs
                if (keypoints1.Length > 0 && keypoints2.Length > 0)
                {
                    Cv2.DrawMatches(mat_image1, keypoints1, mat_image2, keypoints2, good_matches.ToArray(), mview);
                    view = BitmapConverter.ToBitmap(mview);
                }
                else
                {
                    // no matchs
                    view = new Bitmap(1, 1);
                }
            }
            catch (System.AccessViolationException e)
            {
                Console.Error.WriteLine("Access Error  => CompareFeatures : \n{0}", e.Message);
            }
            catch (Exception)
            {
                // Console.Error.WriteLine("Error  => CompareFeatures : \n{0}", e.Message);
            }

            // similarity = 0  when there was no feature  or no match
            if (feature_count <= 0)
            {
                return 0;
            }
            if (nmatch <= 0)
            {
                return 0;
            }

            // similarity = ratio of good matches/ # matches
            var similarity = 100.0 * ngmatch / nmatch;
            return similarity;
        }


        #endregion

        #region images pixels difference
        /// <summary>
        /// Compare the pixels of two images
        /// </summary>
        /// <param name="image1">1st image OpenCv matrix</param>
        /// <param name="image2">2nd image OpenCv matrix</param>
        /// <returns>Similarity [0-100]</returns>
        private static double ComparePixels(Mat image1, Mat image2)
        {
            double sim = 0;

            // resize images to smaller size to speedup calculations
            image1 = SmallerImage(image1);
            image2 = SmallerImage(image2);

            if (image1 == null) { return 0; }
            if (image1.Empty()) { return 0; }
            if (image2 == null) { return 0; }
            if (image2.Empty()) { return 0; }

            double total = (double)image1.Width * image1.Height;
            double same = 0;

            var image1_indexer = GetByteVectorIndexer(image1);
            var image2_indexer = GetByteVectorIndexer(image2);

            for (int i = 0; i < image1.Width; i++)
            {
                for (int j = 0; j < image1.Height; j++)
                {
                    var rgb1 = image1_indexer[i, j];
                    var rgb2 = image2_indexer[i, j];
                    same += 0;

                    Color c1 = Color.FromArgb((int)rgb1.Item2, (int)rgb1.Item1, (int)rgb1.Item0);
                    Color c2 = Color.FromArgb((int)rgb2.Item2, (int)rgb2.Item1, (int)rgb2.Item0);

                    if (Compare2Pixels(c1, c2) >= 100.0)
                    {
                        same += 1;
                    }
                }
            }

            image1.Dispose();
            image2.Dispose();

            sim = 100.0 * (same / total);

            return sim;
        }

        /// <summary>
        /// Compare the pixels of two images sorted by luminance
        /// </summary>
        /// <param name="image1">1st image OpenCv matrix</param>
        /// <param name="image2">2nd image OpenCv matrix</param>
        /// <returns>Similarity [0-100]</returns>
        private static double ComparePixelsSorted(Mat image1, Mat image2)
        {
            double sim = 0;

            // resize images to smaller size to speedup calculations
            image1 = SmallerImage(image1);
            image2 = SmallerImage(image2);

            if (image1 == null) { return 0; }
            if (image1.Empty()) { return 0; }
            if (image2 == null) { return 0; }
            if (image2.Empty()) { return 0; }

            double same = 0;

            // Sort RGB pixels by luminance
            var rgb1_list = SortPixelsByLuminance(image1);
            var rgb2_list = SortPixelsByLuminance(image2);
            double total = (double)rgb1_list.Count;
            for (int i = 0; i < rgb1_list.Count; i++)
            {
                same += 0;

                Color c1 = Color.FromArgb((int)rgb1_list.ElementAt(i).X, (int)rgb1_list.ElementAt(i).Y, (int)rgb1_list.ElementAt(i).Z);
                Color c2 = Color.FromArgb((int)rgb2_list.ElementAt(i).X, (int)rgb2_list.ElementAt(i).Y, (int)rgb2_list.ElementAt(i).Z);

                if (Compare2Pixels(c1, c2) >= 100.0)
                {
                    same += 1;
                }
            }

            sim = 100.0 * (same / total);

            image1.Dispose();
            image2.Dispose();

            return sim;
        }
        #endregion

        #region images pixels distance
        /// <summary>
        /// Compare the distance between the pixels of two images
        /// </summary>
        /// <param name="image1">1st image OpenCv matrix</param>
        /// <param name="image2">2nd image OpenCv matrix</param>
        /// <returns>The distance converted to a % similarity [0-100] </returns>
        private static double ComparePixelsDistance(Mat image1, Mat image2)
        {
            double sim = 0;

            // resize images to smaller size to speedup calculations
            image1 = SmallerImage(image1);
            image2 = SmallerImage(image2);

            if (image1 == null) { return 0; }
            if (image1.Empty()) { return 0; }
            if (image2 == null) { return 0; }
            if (image2.Empty()) { return 0; }

            var image1_indexer = GetByteVectorIndexer(image1);
            var image2_indexer = GetByteVectorIndexer(image2);

            double distance = 0;

            for (int i = 0; i < image1.Width; i++)
            {
                for (int j = 0; j < image1.Height; j++)
                {
                    var rgb1 = image1_indexer[i, j];
                    var rgb2 = image2_indexer[i, j];

                    var c1 = Color.FromArgb((int)rgb1.Item2, (int)rgb1.Item1, (int)rgb1.Item0);
                    var c2 = Color.FromArgb((int)rgb2.Item2, (int)rgb2.Item1, (int)rgb2.Item0);

                    distance += Compare2Pixels_distance(c1, c2);
                }
            }

            sim = distance / (image1.Width * image1.Height);

            image1.Dispose();
            image2.Dispose();

            return sim;
        }

        /// <summary>
        /// Compare the distance between the pixels of two images sorted by luminance
        /// </summary>
        /// <param name="image1">1st image OpenCv matrix</param>
        /// <param name="image2">2nd image OpenCv matrix</param>
        /// <returns>The distance converted to a % similarity [0-100] </returns>
        private static double ComparePixelsDistanceSorted(Mat image1, Mat image2)
        {
            double sim = 0;

            // resize images to smaller size to speedup calculations
            image1 = SmallerImage(image1);
            image2 = SmallerImage(image2);

            if (image1 == null) { return 0; }
            if (image1.Empty()) { return 0; }
            if (image2 == null) { return 0; }
            if (image2.Empty()) { return 0; }

            // Sort RGB pixels by luminance
            var srgb1 = SortPixelsByLuminance(image1);
            var srgb2 = SortPixelsByLuminance(image2);

            double distance = 0;
            for (int i = 0; i < srgb1.Count; i++)
            {
                var c1 = Color.FromArgb((int)srgb1.ElementAt(i).X, (int)srgb1.ElementAt(i).Y, (int)srgb1.ElementAt(i).Z);
                var c2 = Color.FromArgb((int)srgb2.ElementAt(i).X, (int)srgb2.ElementAt(i).Y, (int)srgb2.ElementAt(i).Z);

                distance += Compare2Pixels_distance(c1, c2);
            }

            sim = distance / srgb1.Count;

            image1.Dispose();
            image2.Dispose();

            return sim;
        }
        #endregion


        #region utility methods
        /// <summary>
        /// Convert Mat to byte vector index for faster access to pixels https://github.com/shimat/opencvsharp/wiki/%5BCpp%5D-Accessing-Pixel
        /// </summary>
        /// <param name="image">image OpenCv matrix</param>
        /// <returns>byte vector index</returns>
        private static MatIndexer<Vec3b> GetByteVectorIndexer(Mat image)
        {
            var image_byte3 = new MatOfByte3(image);

            return image_byte3.GetIndexer();
        }
        /// <summary>
        ///  Resize image Mat to smaller size if necessary
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [HandleProcessCorruptedStateExceptions]
        private static Mat SmallerImage(Mat image)
        {
            var smaller = new OpenCvSharp.Size(SmallerSize, SmallerSize);
            Mat simage = image;
            try
            {
                if (image.Width > SmallerSize || image.Height > SmallerSize || image.Width != image.Height)
                {
                    simage = image.Resize(smaller, 0, 0, InterpolationFlags.Linear);
                }
            }
            catch (Exception)
            {
                //Console.Error.WriteLine(" Error => SmallerSize : \n{0}\n{1}", e.Message, e.TargetSite);
                simage = new Mat();
                return simage;
            }

            return simage;
        }

        // http://www.w3.org/TR/AERT#color-contrast
        /// <summary>
        /// Sort RGB pixels by luminance (0.299 Red  0.587 Green 0.114 Blue)
        /// </summary>
        /// <param name="image">image OpenCv matrix</param>
        /// <returns>Sorted list</returns>
        private static List<Point3D> SortPixelsByLuminance(Mat image)
        {
            var image_indexer = GetByteVectorIndexer(image);

            //convert RGB to Point3D
            var rgb_points = new Point3DCollection();
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    var rgb1 = image_indexer[i, j];
                    rgb_points.Add(new Point3D((double)rgb1.Item2, (double)rgb1.Item1, (double)rgb1.Item0));
                }
            }

            // Sort RGB pixels by luminance 0.299 Red  0.587 Green  0.114 Blue
            // http://www.w3.org/TR/AERT#color-contrast
            var rgb_sorted_list = rgb_points.OrderBy(p => 0.299 * p.X + 0.587 * p.Y + 0.114 * p.Z).ToList();

            return rgb_sorted_list;
        }
    }
    #endregion

}