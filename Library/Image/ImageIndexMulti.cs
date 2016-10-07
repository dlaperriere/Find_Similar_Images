// Image files search index
//
// Copyright (C) David Laperriere

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Images
{
    /// <summary>
    /// Use multiple image indexes to search and filter with more than one cpu/core
    ///
    /// Each index contains:
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
    ///  var images_index = new Images.ImageIndexMulti(image_directory, Environment.ProcessorCount);
    ///  var var similar_images = images_index.SearchSimilarImages(image_of_interest, option_image_hash,option_filter_colors, option_similarity_cutoff);
    /// </example>
    ///
    /// <remarks>
    /// Not safe to use within Parallel.ForEach/Parallel.For loops
    /// </remarks>
    public class ImageIndexMulti : IImageIndex
    {
        #region Index data

        private int number_of_index = 2;
        private string image_folder = String.Empty;

        private List<Images.ImageIndex> index_list = new List<Images.ImageIndex>();
        private Dictionary<string, int>[] similar_images_found;

        #endregion Index data

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ImageIndexMulti()
        {
            number_of_index = Environment.ProcessorCount;

            Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dir">image directory</param>
        /// <param name="nindex"> number of index to use</param>
        public ImageIndexMulti(string dir, int nindex = 2)
        {
            if (nindex > 1)
            {
                number_of_index = nindex;
            }

            Initialize();

            IndexImageDirectory(dir);
        }

        /// <summary>
        /// Initialize index data
        /// </summary>
        private void Initialize()
        {
            index_list = new List<Images.ImageIndex>();
            for (int i = 0; i < number_of_index; i++)
            {
                var index = new Images.ImageIndex();
                index.SetIndexName(String.Format("images{0}.idx", i));
                index_list.Add(index);
            }
        }

        #endregion Constructors

        #region Images files

        /// <summary>
        /// List of images files in index
        /// </summary>
        /// <returns></returns>
        public List<String> ImageFilesIndexed()
        {
            var files = new List<String>();

            foreach (var index in index_list)
            {
                files = files.Union(index.ImageFilesIndexed()).ToList();
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

            foreach (var index in index_list)
            {
                var iinfo = index.ImageInfo(image_name);
                if (iinfo.Count > 0)
                {
                    info = iinfo;
                    break;
                }
            }

            return info;
        }

        #endregion Image size & color info

        #region Index images

        /// <summary>
        /// Index all image files in given directory
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

            // read existing index files
            Parallel.ForEach(index_list, (index) => { index.LoadIndex(dir); });

            // index new files
            var image_files = index_list[0].ImageFilesInDir(dir);
            foreach (var index in index_list)
            {
                image_files = image_files.Except(index.ImageFilesIndexed()).ToList();
            }
            image_files.Sort();
            var file_lists = Partition<string>(image_files, number_of_index);

            Parallel.For(0, number_of_index, i =>
            {
                index_list[i].IndexImageFiles(dir, file_lists[i]);
            });

            // read new index files
            Parallel.ForEach(index_list, (index) => { index.LoadIndex(dir); });

            image_folder = dir;

            // create shared index data
            var all_images = new ConcurrentDictionary<String, String>();
            var all_ids = new ConcurrentDictionary<String, String>();
            var all_image_hashes_infos = new ConcurrentDictionary<String, List<String>>();
            foreach (var index in index_list)
            {
                foreach (var image in index.IndexData.Images)
                {
                    all_images.TryAdd(image.Key, image.Value);
                }

                foreach (var id in index.IndexData.Ids)
                {
                    all_ids.TryAdd(id.Key, id.Value);
                }
                foreach (var image_hash in index.IndexData.ImageHashesInfos)
                {
                    all_image_hashes_infos.TryAdd(image_hash.Key, image_hash.Value);
                }
            }

            foreach (var index in index_list)
            {
                index.IndexData.Images = all_images;
                index.IndexData.Ids = all_ids;
                index.IndexData.ImageHashesInfos = all_image_hashes_infos;
            }

            GC.Collect(); // force GC to free memory
            GC.WaitForPendingFinalizers();
        }

        #endregion Index images

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
            similar_images_found = new Dictionary<string, int>[number_of_index];

            Parallel.For(0, number_of_index, i =>
            {
                var found = new Dictionary<string, int>();
                found.Add(image, 100);

                if (index_list[i].Count > 0)
                {
                    found = index_list[i].SearchSimilarImages(image, imagehash, filter_method, similarity_cutoff);
                }
                similar_images_found[i] = found;
               
            });

            if (similar_images_found == null) { return matches; }
            if (similar_images_found.Length == 0) { return matches; }

            foreach (var images_found in similar_images_found)
            {
                matches = matches.Union(images_found).ToDictionary(k => k.Key, v => v.Value);
            }

            return matches;
        }

        #endregion Search similar images

        #region utility method

        //http://www.vcskicks.com/partition-list.php
        /// <summary>
        /// Split a list of elements into smaller lists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="totalPartitions"></param>
        /// <returns></returns>
        private List<T>[] Partition<T>(List<T> list, int totalPartitions)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            if (totalPartitions < 1)
                throw new ArgumentOutOfRangeException("totalPartitions");

            List<T>[] partitions = new List<T>[totalPartitions];

            int maxSize = (int)Math.Ceiling(list.Count / (double)totalPartitions);
            int k = 0;

            for (int i = 0; i < partitions.Length; i++)
            {
                partitions[i] = new List<T>();
                for (int j = k; j < k + maxSize; j++)
                {
                    if (j >= list.Count)
                        break;
                    partitions[i].Add(list[j]);
                }
                k += maxSize;
            }

            return partitions;
        }

        #endregion utility method
    }
}