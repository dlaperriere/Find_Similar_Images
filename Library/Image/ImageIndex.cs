// Image files search index
//
// Copyright (C) David Laperriere

using DataStructure.Image;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Images
{
    /// <summary>
    /// Image files search index
    ///    - image perceptual hashes
    ///    - color informations
    ///    - image thumbnail & size
    ///    - BK-trees used for image search
    /// </summary>
    ///
    /// <example>
    /// ImageHash.Algorithm option_image_hash = ImageHash.Algorithm.BlockHistogram;
    /// Images.ComparisonMethod option_filter_colors = Images.ComparisonMethod.MainColor;
    /// int option_similarity_cutoff = 80;
    ///
    ///  var images_index = new Images.ImageIndex(image_directory);
    ///  var var similar_images = images_index.SearchSimilarImages(image_of_interest, option_image_hash,option_filter_colors, option_similarity_cutoff);
    /// </example>
    public class ImageIndex : IImageIndex
    {
        /// <summary>
        /// Index colums
        /// </summary>
        public enum IndexColumns { FileName, AverageHash = 0, BlockHash, DifferenceHash, PerceptiveHash, HistogramHash, Thumbnail, MainColor, MainColorRGB, MainColorPercent, TopColors, Size };

        #region Index data

        /// <summary>
        /// smaller image size used
        /// </summary>
        private static System.Drawing.Size smaller = new System.Drawing.Size(smaller_size, smaller_size);

        private static int smaller_size = Images.ImageSimilarity.SmallerSize;

        /// <summary>
        /// internal parameters
        /// </summary>
        private char delimiter = '\t';

        private string image_folder = String.Empty;
        private string index_filename = "images.idx";

        /// <summary>
        ///  Image index data
        ///    - image path --> internal id  (used by BKtree internally)
        ///    - image hashes & image infos
        /// </summary>
        private Index index = new Index();

        public Index IndexData
        {
            get { return index; }
            set { index = value; }
        }

        private uint image_count = 0;

        public uint Count
        {
            get { return image_count; }
            set { image_count = value; }
        }

        /// <summary>
        ///  BKTrees used for image hash lookups
        /// </summary>
        private BKTree averagehash_bktree;

        private BKTree differencehash_bktree;
        private BKTree blockhash_bktree;
        private BKTree perceptivehash_bktree;
        private BKTree histogramhash_bktree;
        private BKTree colorhash_bktree;

        #endregion Index data

        #region Constructors

        /// <summary>
        ///  Default constructor
        /// </summary>
        public ImageIndex()
        {
            Initialize();
            image_folder = String.Empty;
        }

        /// <summary>
        /// Constructor with directory
        /// </summary>
        /// <param name="dir"></param>
        public ImageIndex(string dir, string index_name = "images.idx")
        {
            Initialize();
            index_filename = index_name;
            IndexImageDirectory(dir);
            image_folder = dir;
        }

        /// <summary>
        /// Initialize index data & bktrees
        /// </summary>
        private void Initialize()
        {
            index = new Index();
            averagehash_bktree = new BKTree(Images.ImageHashAlgorithm.Average, index);
            differencehash_bktree = new BKTree(Images.ImageHashAlgorithm.Difference, index);
            blockhash_bktree = new BKTree(Images.ImageHashAlgorithm.Block, index);
            perceptivehash_bktree = new BKTree(Images.ImageHashAlgorithm.Perceptive, index);
            histogramhash_bktree = new BKTree(Images.ImageHashAlgorithm.Histogram, index);
            colorhash_bktree = new BKTree(Images.ImageHashAlgorithm.Color, index);
            image_folder = String.Empty;
        }

        /// <summary>
        /// Add image id to BKTrees
        /// </summary>
        /// <param name="image_id"></param>
        private void UpdateBKTrees(string image_id)
        {
            averagehash_bktree.Add(image_id);
            blockhash_bktree.Add(image_id);
            differencehash_bktree.Add(image_id);
            perceptivehash_bktree.Add(image_id);
            histogramhash_bktree.Add(image_id);
            colorhash_bktree.Add(image_id);
            image_count++;
        }

        /// <summary>
        /// Set index filename
        /// </summary>
        /// <param name="name"></param>
        public void SetIndexName(string name)
        {
            index_filename = name;
        }

        #endregion Constructors

        #region Add image to index

        /// <summary>
        ///  Add an image to the index
        /// </summary>
        /// <param name="image_name"></param>
        [HandleProcessCorruptedStateExceptions]
        public void AddImage(string image_name)
        {
            if (image_name == null) { return; }
            if (index.Contains(image_name)) { return; }

            try
            {
                Image image = null;
                //image = Image.FromFile(image_name, false);
                /**/
                BitmapImage imagebitmap = new BitmapImage();
                using (var ms = new MemoryStream())
                {
                    // ~ 40-50% faster than Image.FromFile
                    // image size in index not accurate
                    
                    imagebitmap.BeginInit();
                    imagebitmap.DecodePixelWidth = 4*Images.ImageSimilarity.SmallerSize;
                    imagebitmap.UriSource = new Uri(image_name);
                    imagebitmap.EndInit();
                     
                    BitmapEncoder enc = new BmpBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(imagebitmap));
                    enc.Save(ms);
                    image = Image.FromStream(ms);
                  }
                
                if (image == null) { return; }

                var file_size = new System.IO.FileInfo(image_name).Length.ToString();

                var dominant_colors = Images.ColorExtract.TopColors(image);
                var dominant_colors_img = Images.ColorExtract.Draw(dominant_colors, Images.ColorExtract.top_colors * 2, 1);
                var dominant_colors_thumbnail = ImageToString(dominant_colors_img);

                var main_colorv = dominant_colors.ElementAt(0);
                var main_color_rgb = main_colorv.Key.ToArgb();
                var main_color_percent = main_colorv.Value;
                var main_color_img = Images.ColorExtract.Draw(dominant_colors, 1, 1);
                var main_color_thumbnail = ImageToString(main_color_img);

                var bhash = Images.ImageBlockHash.BlockHash(image);

                var simage = CommonUtils.ImageUtils.Resize(image, smaller_size, smaller_size, false);

                var ahash = Images.ImageHash.HashImage(simage, ImageHashAlgorithm.Average);

                var dhash = Images.ImageHash.HashImage(simage, ImageHashAlgorithm.Difference);
                var phash = Images.ImageHash.HashImage(simage, ImageHashAlgorithm.Perceptive);

                var histhash = Images.ImageHash.HashImage(simage, ImageHashAlgorithm.Histogram);

                var thumbnail = ImageToString(simage);

                var hlist = new List<String>();
                hlist.Add(ahash);
                hlist.Add(bhash);
                hlist.Add(dhash);
                hlist.Add(phash);
                hlist.Add(histhash);
                hlist.Add(thumbnail);
                hlist.Add(main_color_thumbnail);
                hlist.Add(main_color_rgb.ToString());
                hlist.Add(main_color_percent.ToString());
                hlist.Add(dominant_colors_thumbnail);
                hlist.Add(file_size);

                var ok = index.Add(image_name, hlist);
                //if (ok) { Console.WriteLine(" added = {0}", image_name); }
                //else { Console.WriteLine(" failed to add = {0}", image_name);  }

                image.Dispose();
                dominant_colors_img.Dispose();
                main_color_img.Dispose();
                simage.Dispose();
            }
            catch (OutOfMemoryException e)
            {
                //The file does not have a valid image format.
                //-or- GDI+ does not support the pixel format of the file

                System.Console.WriteLine("\n  exclude \"{0}\" from index (invalid image format: {1})", image_name, e.Message);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("\n  exclude \"{0}\" from index (Error: {1})", image_name, e.Message);
            }
        }

        #endregion Add image to index

        #region Images files

        /// <summary>
        /// List of images files in index
        /// </summary>
        /// <returns></returns>
        public List<String> ImageFilesIndexed()
        {
            return index.Files();
        }

        /// <summary>
        /// Find images files in a given directory (bmp,gif,jpg,png,tiff)
        /// </summary>
        /// <param name="dir"></param>
        ///
        /// <returns>List of images file found</returns>
        public List<String> ImageFilesInDir(String dir)
        {
            var files = FilesInDir(dir);

            var image_files = new List<String>();
            foreach (var file in files)
            {
                if (IsValidImageExtension(Path.GetExtension(file)))
                {
                    image_files.Add(file);
                }
            }

            return image_files;
        }

        //How to: Iterate Through a Directory Tree (C# Programming Guide)
        //https://msdn.microsoft.com/en-us/library/bb513869.aspx
        /// <summary>
        /// Find files in a given directory 
        /// </summary>
        /// <param name="dir"></param>
        ///
        /// <returns>List of files found</returns>
        public List<String> FilesInDir(String dir)
        {
            var files = new List<String>();

            Stack<string> dirs = new Stack<string>(20);

            if (!System.IO.Directory.Exists(dir))
            {
                throw new ArgumentException();
            }
            dirs.Push(dir);

            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                string[] subDirs;
                try
                {
                    subDirs = System.IO.Directory.GetDirectories(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

   
                foreach (var sdir in subDirs) {

                    FileInfo pathInfo = new FileInfo(sdir);
                    if (!pathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint)){  // skip symlink or junction point
                        dirs.Push(sdir); 
                    }
                }
                   

                string[] cfiles = null;
                try
                {
                    cfiles = System.IO.Directory.GetFiles(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                foreach (string file in cfiles)
                {
                    try
                    {
                        files.Add(file);
                    }
                    catch (System.IO.FileNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                        continue;
                    }
                }

            }
            
            return files;
        }
        #endregion Images files

        #region Image size & color info

        /// <summary>
        ///  Image info from index
        ///   - file size
        ///   - thumbnail (Base64)
        ///   - main color image (Base64)
        ///   - top color image (Base64)
        /// </summary>
        /// <param name="image_name"></param>
        /// <returns></returns>
        public List<string> ImageInfo(string image_name)
        {
            var info = new List<string>();

            if (!index.Contains(image_name)) { return info; }

            var size = index.ImageInfo(image_name, IndexColumns.Size);

            var size_formated = CommonUtils.MathUtils.FormatNumber(Convert.ToUInt32(size));
            var thumbnail = index.ImageInfo(image_name, IndexColumns.Thumbnail);
            var dominantcolors = index.ImageInfo(image_name, IndexColumns.TopColors);
            var maincolor = index.ImageInfo(image_name, IndexColumns.MainColor);

            info.Add(size_formated);

            info.Add(thumbnail);
            info.Add(maincolor);
            info.Add(dominantcolors);

            return info;
        }

        #endregion Image size & color info

        #region Image extension

        /// <summary>
        /// Checks if the extension of a file is a known image format.
        /// </summary>
        /// <param name="extension">The extension of a file.</param>
        /// <returns>True if it's an image format, false if not.</returns>
        public static bool IsValidImageExtension(string extension)
        {
            // Valid extensions
            HashSet<string> image_extensions = new HashSet<string> { ".bmp", ".gif", ".jpg", ".jpeg", ".png", ".tif", ".tiff" };

            return image_extensions.Contains(extension.ToLower());
        }

        #endregion Image extension

        #region Image thumbnail

        /// <summary>
        ///  Get an image stored as a Base64 string from the index
        /// </summary>
        /// <param name="image_name"></param>
        /// <returns>image version of the Base64String in the index</returns>
        public Image ImageFromIndex(string image_name, IndexColumns col = IndexColumns.Thumbnail)
        {
            var thumbnail = index.ImageInfo(image_name, col);
            var thumbnail_bytes = Convert.FromBase64String(thumbnail);

            return Image.FromStream(new MemoryStream(thumbnail_bytes));
        }

        //https://msdn.microsoft.com/en-us/library/dhx0d524.aspx
        /// <summary>
        ///  Convert an image to a Base64String
        /// </summary>
        /// <param name="image_name"></param>
        /// <returns>Base64String version of the image</returns>
        private string ImageToString(Image image)
        {
            string base64String = String.Empty;

            using (MemoryStream ms = new MemoryStream())
            {
                var format = System.Drawing.Imaging.ImageFormat.Bmp;

                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                base64String = Convert.ToBase64String(imageBytes);
            }

            return base64String;
        }

        #endregion Image thumbnail

        #region Index images

        /// <summary>
        ///  Index all image files in given directory
        /// </summary>
        /// <param name="dir"></param>
        public void IndexImageDirectory(string dir)
        {
            // invalid dir
            if (!Directory.Exists(dir)) { return; }

            // dir already indexed
            if (image_folder.Equals(dir)) { return; }

            // reset index data structures
            Initialize();

            // load previous index
            string index_file = System.IO.Path.Combine(dir, index_filename);
            var image_files = ImageFilesInDir(dir);

            Console.WriteLine("* Indexing images files: ");

            if (File.Exists(index_file))
            {
                LoadIndex(dir);
            }

            // index new files
            var index_images_files = index.Files();
            image_files = image_files.Except(index_images_files).ToList();

            IndexImageFiles(dir, image_files);

            if (File.Exists(index_file))
            {
                LoadIndex(dir);
            }

            GC.Collect(); // force GC to free memory
            GC.WaitForPendingFinalizers();

            image_folder = dir;

            Console.WriteLine(" done");
        }

        /// <summary>
        /// Index a list image files
        /// </summary>
        /// <param name="dir"> directory where to save the index file</param>
        /// <param name="image_files"> list of images to index</param>
        public void IndexImageFiles(string dir, List<string> image_files)
        {
            // invalid dir
            if (!Directory.Exists(dir)) { return; }

            // dir already indexed
            if (image_folder.Equals(dir)) { return; }

            string index_file = System.IO.Path.Combine(dir, index_filename);

            image_files.Sort();
            Parallel.ForEach(image_files, image_name =>
            {
                //foreach (var image_name in image_files)
                //{
                this.AddImage(image_name);

                int count = index.FileCount();
                if (count % 200 == 0)
                {
                    Console.Write(".");
                    //GC.Collect(); // force GC to free memory
                    //GC.WaitForPendingFinalizers();
                }
                //}
            });

            // save index
            SaveIndex(index_file);
            image_folder = dir;
        }

        /// <summary>
        /// Save image index to file (tab delimited columns)
        /// </summary>
        /// <param name="index_file"></param>
        public void SaveIndex(string index_file)
        {
            using (var indexwriter = new System.IO.StreamWriter(index_file))
            {
                foreach (var image in index.Files())
                {
                    var image_id = index.Id(image);

                    indexwriter.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}",
                        image, index.ImageInfo(image, IndexColumns.AverageHash),
                        index.ImageInfo(image, IndexColumns.BlockHash),
                        index.ImageInfo(image, IndexColumns.DifferenceHash),
                        index.ImageInfo(image, IndexColumns.PerceptiveHash),
                        index.ImageInfo(image, IndexColumns.HistogramHash),
                        index.ImageInfo(image, IndexColumns.Thumbnail),
                        index.ImageInfo(image, IndexColumns.MainColor),
                        index.ImageInfo(image, IndexColumns.MainColorRGB),
                        index.ImageInfo(image, IndexColumns.MainColorPercent),
                        index.ImageInfo(image, IndexColumns.TopColors),
                        index.ImageInfo(image, IndexColumns.Size) 
                        );

                    // update BKTrees
                    UpdateBKTrees(image_id);
                }
            }
        }

        #endregion Index images

        #region Read index

        /// <summary>
        /// Read existing index file from a directory
        /// </summary>
        /// <param name="dir"></param>
        public void LoadIndex(string dir)
        {
            string index_file = System.IO.Path.Combine(dir, index_filename);

            if (!File.Exists(index_file)) { return; }

            using (StreamReader reader = new StreamReader(index_file))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(delimiter);

                    if (parts.Length < 12) { continue; }
                    if (File.Exists(parts[0]))
                    {
                        var hashs = new List<string>();
                        hashs.Add(parts[1]);  // avg
                        hashs.Add(parts[2]);  // block
                        hashs.Add(parts[3]);  // diff
                        hashs.Add(parts[4]);  // phash
                        hashs.Add(parts[5]);  // rgb
                        hashs.Add(parts[6]);  // thumbnail
                        hashs.Add(parts[7]);  // main color
                        hashs.Add(parts[8]);  // main color rgb
                        hashs.Add(parts[9]);  // main color %
                        hashs.Add(parts[10]); // dominant colors
                        hashs.Add(parts[11]); // file size

                        // keep index data when index & disk file size are the same
                        var disk_file_size = new System.IO.FileInfo(parts[0]).Length.ToString();

                        if (disk_file_size.Equals(parts[11]))
                        {
                            index.Add(parts[0], hashs);
                        }
                    }
                }
            }
        }

        #endregion Read index

        #region Search similar images

        /// <summary>
        ///  Search similar images
        /// </summary>
        /// <param name="image"> path of image of interest </param>
        /// <param name="imagehash"> Image hash algorithm  </param>
        /// <param name="filter_method"> Color filtering method </param>
        /// <param name="similarity_cutoff"> min. similarity % </param>
        /// <returns></returns>
        public Dictionary<string, int> SearchSimilarImages(string image, Images.ImageHashAlgorithm imagehash, Images.ComparisonMethod filter_method, int similarity_cutoff)
        {
            var matches = new Dictionary<string, int>();

            if (image == null) { return matches; }

            matches.Add(image, 100);

            if (filter_method == ComparisonMethod.Feature || imagehash == Images.ImageHashAlgorithm.MD5)
            {
                // skip diabled methods
                return matches;
            }

            if (filter_method == ComparisonMethod.None)
            {
                // search only with hash
                matches = FindSimilarImages(image, imagehash, similarity_cutoff);
            }
            else
            {
                // search and filter
                matches = FindSimilarImages(image, imagehash, similarity_cutoff);
                matches = FilterSimilarImages(image, filter_method, similarity_cutoff, matches);
            }
            return matches;
        }

        #region Find similar images

        /// <summary>
        /// Find similar image using an image hash BKTree
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imagehash"></param>
        /// <param name="similarity_cutoff"></param>
        /// <returns></returns>
        private Dictionary<string, int> FindSimilarImages(String image, Images.ImageHashAlgorithm imagehash, int similarity_cutoff)
        {
            //System.Console.WriteLine("  search simlar to {0} hash {1} {2}%",image,imagehash,similarity_cutoff);
            var similar_images = new Dictionary<string, int>();
            var rgbkept_images = new Dictionary<string, int>();

            if (image == null) { return similar_images; }

            // add image to index
            if (!index.Contains(image))
            {
                this.AddImage(image);
            }

            // check that the image was successfully added to the index search trees
            if (!index.Contains(image))
            {
                System.Console.WriteLine(" file {0} does not seem to be a valid image...", image);
                return similar_images;
            }

            // index empty
            if (index.FileCount() <= 1)
            {
                return similar_images;
            }

            // look for image in search trees
            switch (imagehash)
            {
                /// image hash + main color filter
                case Images.ImageHashAlgorithm.AverageColor:
                    similar_images = averagehash_bktree.Search(image, similarity_cutoff);

                    rgbkept_images = FilterSimilarImages(image, ComparisonMethod.MainColor, similarity_cutoff, similar_images);
                    similar_images = rgbkept_images;
                    break;

                case Images.ImageHashAlgorithm.BlockColor:
                    similar_images = blockhash_bktree.Search(image, similarity_cutoff);

                    rgbkept_images = FilterSimilarImages(image, ComparisonMethod.MainColor, similarity_cutoff, similar_images);
                    similar_images = rgbkept_images;
                    break;

                case Images.ImageHashAlgorithm.DifferenceColor:
                    similar_images = differencehash_bktree.Search(image, similarity_cutoff);

                    rgbkept_images = FilterSimilarImages(image, ComparisonMethod.MainColor, similarity_cutoff, similar_images);
                    similar_images = rgbkept_images;
                    break;

                case Images.ImageHashAlgorithm.PerceptiveColor:
                    similar_images = perceptivehash_bktree.Search(image, similarity_cutoff);

                    rgbkept_images = FilterSimilarImages(image, ComparisonMethod.MainColor, similarity_cutoff, similar_images);
                    similar_images = rgbkept_images;
                    break;

                /// image hash + rgb histogram hash filter
                case Images.ImageHashAlgorithm.AverageHistogram:
                    similar_images = averagehash_bktree.Search(image, similarity_cutoff);

                    rgbkept_images = FilterSimilarImages(image, ComparisonMethod.RGBHistogramHash, similarity_cutoff, similar_images);
                    similar_images = rgbkept_images;
                    break;

                case Images.ImageHashAlgorithm.BlockHistogram:
                    similar_images = blockhash_bktree.Search(image, similarity_cutoff);

                    rgbkept_images = FilterSimilarImages(image, ComparisonMethod.RGBHistogramHash, similarity_cutoff, similar_images);
                    similar_images = rgbkept_images;
                    break;

                case Images.ImageHashAlgorithm.DifferenceHistogram:
                    similar_images = differencehash_bktree.Search(image, similarity_cutoff);

                    rgbkept_images = FilterSimilarImages(image, ComparisonMethod.RGBHistogramHash, similarity_cutoff, similar_images);
                    similar_images = rgbkept_images;
                    break;

                case Images.ImageHashAlgorithm.PerceptiveHistogram:
                    similar_images = perceptivehash_bktree.Search(image, similarity_cutoff);

                    rgbkept_images = FilterSimilarImages(image, ComparisonMethod.RGBHistogramHash, similarity_cutoff, similar_images);
                    similar_images = rgbkept_images;
                    break;

                /// just image hash
                case Images.ImageHashAlgorithm.Average:
                    similar_images = averagehash_bktree.Search(image, similarity_cutoff);
                    break;

                case Images.ImageHashAlgorithm.Block:
                    similar_images = blockhash_bktree.Search(image, similarity_cutoff);
                    break;

                case Images.ImageHashAlgorithm.Color:
                    similar_images = colorhash_bktree.Search(image, similarity_cutoff);
                    break;

                case Images.ImageHashAlgorithm.Difference:
                    similar_images = differencehash_bktree.Search(image, similarity_cutoff);
                    break;

                case Images.ImageHashAlgorithm.Histogram:
                    similar_images = histogramhash_bktree.Search(image, similarity_cutoff);
                    break;

                case Images.ImageHashAlgorithm.Perceptive:
                    similar_images = perceptivehash_bktree.Search(image, similarity_cutoff);
                    break;

                case Images.ImageHashAlgorithm.MD5:
                    Console.WriteLine("md5 hash is not available ...");
                    System.Environment.Exit(1);
                    break;

                default:
                    //throw new Exception("unknown hash algo: " + imagehash);
                    break;
            }

            // GC.Collect(); // force GC to free memory
            // GC.WaitForPendingFinalizers();

            if (!similar_images.ContainsKey(image))
            {
                similar_images.Add(image, 100);
            }

            //Console.WriteLine("  {0} matches {1} ", image,similar_images.Count);

            return similar_images;
        }

        #endregion Find similar images

        #region Filter similar images

        /// <summary>
        /// Filter similar images found by a color filtering method
        /// </summary>
        /// <param name="image_name"></param>
        /// <param name="method"> Color filtering method </param>
        /// <param name="similarity_cutoff"> min. similarity % </param>
        /// <param name="similar_images"> similar images to filter </param>
        /// <returns></returns>
        private Dictionary<string, int> FilterSimilarImages(String image_name, Images.ComparisonMethod method, int similarity_cutoff, Dictionary<string, int> similar_images)
        {
            var kept_image = new Dictionary<string, int>();
            kept_image.Add(image_name, 100);

            // quit early when :
            //  - the image does not seem to be valid
            //  - there is no match to filter
            if (!index.Contains(image_name))
            {
                return kept_image;
            }
            if (similar_images.Count == 0)
            {
                return kept_image;
            }

            // filter out images that are bellow the similarity cutoff for the color/pixel comparison method

            int before = similar_images.Count;

            try
            {
                /// load image thumbnail & info
                Image image1 = new Bitmap(1, 1);
                Image top_colors_img1 = new Bitmap(1, 1);

                if (method != Images.ComparisonMethod.None)
                {
                    image1 = ImageFromIndex(image_name);
                }
                if (method == Images.ComparisonMethod.TopColors)
                {
                    top_colors_img1 = ImageFromIndex(image_name, IndexColumns.TopColors);
                }

                foreach (var cimage in similar_images.Keys.ToList())
                {
                    if (cimage.Equals(image_name)) { continue; }

                    var similarity_image_hash = similar_images[cimage];
                    //Console.WriteLine("  - {0} vs {1} hash sim {2}", image_name, cimage, similarity_image_hash);

                    if (method == Images.ComparisonMethod.None)
                    {
                        /// filter only with image hash
                        if (similarity_image_hash >= similarity_cutoff)
                        {
                            kept_image.Add(cimage, similarity_image_hash);
                            //Console.WriteLine("  {0}  KEEP {2} match {3}", similarity_image_hash, cimage, image_name);
                        }
                        else
                        {
                            //Console.WriteLine("  {0} REM {1} match {2}",similarity_image_hash, cimage, image_name);
                        }
                    }
                    else
                    {
                        /// also filter with color/pixel comparison method
                        var image2 = ImageFromIndex(cimage);
                        double similarity_filter = 0;

                        if (method == ComparisonMethod.Feature)
                        {
                            Console.WriteLine("feature comparison is disabled ...");
                            System.Environment.Exit(1);
                            /*
                            // use original images
                            var oimage1 = Image.FromFile(image_name,true);
                            var oimage2 = Image.FromFile(cimage, true);
                            similarity_filter = Images.ImageSimilarity.CompareImages(oimage1, oimage2, Images.ComparisonMethod.Feature);
                            oimage1.Dispose();
                            oimage2.Dispose();
                            GC.Collect(); // force GC to free memory
                            GC.WaitForPendingFinalizers();
                             */
                        }
                        else if (method == ComparisonMethod.RGBHistogramHash)
                        {
                            var rgbhash1 = index.ImageInfo(image_name, IndexColumns.HistogramHash);
                            var rgbhash2 = index.ImageInfo(cimage, IndexColumns.HistogramHash);
                            similarity_filter = Images.ImageHistogramHash.Similarity(rgbhash1, rgbhash2);
                        }
                        else if (method == ComparisonMethod.MainColor)
                        {
                            similarity_filter = 0.0;

                            var mcolor1 = index.ImageInfo(image_name, IndexColumns.MainColorRGB);
                            var mcolor2 = index.ImageInfo(cimage, IndexColumns.MainColorRGB);

                            var c1 = Color.FromArgb(Convert.ToInt32(mcolor1));
                            var c2 = Color.FromArgb(Convert.ToInt32(mcolor2));

                            similarity_filter = Images.ImageSimilarity.Compare2Pixels(c1, c2);
                        }
                        else if (method == ComparisonMethod.TopColors)
                        {
                            /// top  colors
                            var top_colors_img2 = ImageFromIndex(cimage, IndexColumns.TopColors);
                            double sim_top_color = Images.ImageSimilarity.CompareImages(top_colors_img1, top_colors_img2, method: ComparisonMethod.PixelsDifferenceSorted);

                            similarity_filter = sim_top_color;

                            top_colors_img2.Dispose();
                        }
                        else
                        {
                            // use image thumbnail from the index
                            similarity_filter = Images.ImageSimilarity.CompareImages(image1, image2, method);
                        }

                        if (similarity_image_hash >= similarity_cutoff &&
                            similarity_filter >= similarity_cutoff)
                        {
                            kept_image.Add(cimage, (int)((similarity_image_hash + similarity_filter) / 2.0));
                            //Console.WriteLine("  {0} {1} KEEP {2} match {3}", similarity_image_hash, similarity_filter, cimage, image_name);
                        }
                        else
                        {
                            //Console.WriteLine("  {0} {1} FILTER {2} match {3}", similarity_image_hash, similarity_filter, cimage, image_name);
                        }

                        image2.Dispose();
                    }
                } // foreach

                image1.Dispose();
                top_colors_img1.Dispose();

                //GC.Collect(); // force GC to free memory
                //GC.WaitForPendingFinalizers();
            }
            catch (OutOfMemoryException e)
            {
                //The file does not have a valid image format
                Console.Error.WriteLine("Error  => Filter similar images {0} {1}: \n{2}", image_name, method, e.Message);
                kept_image = similar_images;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error  => Filter similar images {0} {1}: \n{2}\n{3}", image_name, method, e.Message, e.StackTrace);
                kept_image = similar_images;
            }

            return kept_image;
        }

        #endregion Filter similar images

        #endregion Search similar images

        #region Index data class

        /// <summary>
        ///  Image index data
        ///    - image path --> internal id  (used by BKtree to save memory)
        ///    - image hashes & image infos
        /// </summary>
        public class Index
        {
            /// <summary>
            /// file id : "count|"
            /// </summary>
            private static ulong count = 0;

            private static readonly object _countlock = new object();

            private ulong nextid()
            {
                lock (_countlock)
                {
                    count++;
                }
                return count;
            }

            /// <summary>
            ///  File name -> id, id -> File name
            /// </summary>
            private ConcurrentDictionary<string, string> images = new ConcurrentDictionary<String, String>();

            public ConcurrentDictionary<string, string> Images
            {
                get { return images; }
                set { images = value; }
            }

            private ConcurrentDictionary<string, string> ids = new ConcurrentDictionary<String, String>();

            public ConcurrentDictionary<string, string> Ids
            {
                get { return ids; }
                set { ids = value; }
            }

            /// <summary>
            ///  Image hashes (File id -> image hashes & infos )
            /// </summary>
            private ConcurrentDictionary<String, List<String>> image_hashes_infos = new ConcurrentDictionary<String, List<String>>();

            public ConcurrentDictionary<String, List<String>> ImageHashesInfos
            {
                get { return image_hashes_infos; }
                set { image_hashes_infos = value; }
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public Index()
            {
                images = new ConcurrentDictionary<String, String>();
                image_hashes_infos = new ConcurrentDictionary<String, List<String>>();
                count = 0;
                nextid();
            }

            /// <summary>
            /// Test if index contains an image
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public bool Contains(string name)
            {
                bool contains = false;
                if (images.ContainsKey(name))
                {
                    contains = true;
                }

                return contains;
            }

            /// <summary>
            ///  Add image
            /// </summary>
            /// <param name="name"></param>
            /// <param name="hashes_info"> image hash & infos</param>
            /// <returns></returns>
            public bool Add(string image_name, List<string> hashes_info)
            {
                bool ok = false;

                if (image_name == null) { return false; }

                if (!images.ContainsKey(image_name))
                {
                    string id = String.Empty;

                    id = String.Format("{0}|", nextid());
                    //id = image_name.GetHashCode().ToString();

                    ok = images.TryAdd(image_name, id);
                    ok = ids.TryAdd(id, image_name);
                    ok = image_hashes_infos.TryAdd(id, hashes_info);
                }
                else
                {
                    ok = true;
                }

                return ok;
            }

            /// <summary>
            ///  Lookup info from index
            /// </summary>
            /// <param name="image"></param>
            /// <param name="column"></param>
            /// <returns></returns>
            public string ImageInfo(string image, IndexColumns column)
            {
                if (image == null) { return String.Empty; }

                var id = image;
                if (images.ContainsKey(image))
                {
                    id = images[image];
                }
                return (image_hashes_infos[id])[(int)column];
            }

            /// <summary>
            ///  Lookup image hash from index
            /// </summary>
            /// <param name="image"></param>
            /// <param name="hash_algo"></param>
            /// <returns></returns>
            public string ImageHash(string image, Images.ImageHashAlgorithm hash_algo)
            {
                if (image == null) { return String.Empty; }

                var hash = String.Empty;

                switch (hash_algo)
                {
                    case ImageHashAlgorithm.Average:
                        hash = ImageInfo(image, IndexColumns.AverageHash);
                        break;

                    case ImageHashAlgorithm.Block:
                        hash = ImageInfo(image, IndexColumns.BlockHash);
                        break;

                    case ImageHashAlgorithm.Color:
                        hash = ImageInfo(image, IndexColumns.MainColorRGB);
                        break;

                    case ImageHashAlgorithm.Difference:
                        hash = ImageInfo(image, IndexColumns.DifferenceHash);
                        break;

                    case ImageHashAlgorithm.Histogram:
                        hash = ImageInfo(image, IndexColumns.HistogramHash);
                        break;

                    case ImageHashAlgorithm.MD5:
                        break;

                    case ImageHashAlgorithm.Perceptive:
                        hash = ImageInfo(image, IndexColumns.PerceptiveHash);
                        break;

                    default:
                        break;
                }

                return hash;
            }

            /// <summary>
            /// List of images in the index
            /// </summary>
            /// <returns></returns>
            public List<string> Files()
            {
                return images.Keys.ToList();
            }

            public int FileCount()
            {
                return images.Keys.Count;
            }

            public string Id(string name)
            {
                string id = String.Empty;

                if (images.ContainsKey(name))
                {
                    id = images[name];
                }
                else
                {
                    if (ids.ContainsKey(name))
                    {
                        id = ids[name];
                    }
                }

                return id;
            }
        }

        #endregion Index data class
    }
}