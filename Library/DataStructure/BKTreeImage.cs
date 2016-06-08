// BK-tree that use image hash index for distance calculations
//
// Copyright (C) David Laperriere

using System;
using System.Collections.Generic;

namespace DataStructure.Image
{
    /// <summary>
    /// Image hash BKTree
    /// </summary>
    public class BKTree
    {
        #region BKTree data

        private int distParent = 0;
        private string image_path = null;
        private List<BKTree> subtrees = new List<BKTree>();

        private Images.ImageHashAlgorithm imghash_algo_used;
        private Images.ImageIndex.Index image_index = null;
        private Distance DistanceMethod;

        #endregion BKTree data

        #region constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="img_index">image hashes</param>
        /// <param name="imghash_algo">image hash algo</param>
        public BKTree(Images.ImageHashAlgorithm imghash_algo, Images.ImageIndex.Index img_index)
        {
            ImageHashAlgo(imghash_algo);
            image_index = img_index;
            ImageHashAlgo(imghash_algo);
        }

        /// <summary>
        /// Constructor (Add to subtree)
        /// </summary>
        /// <param name="image"></param>
        /// <param name="distance"></param>
        /// <param name="imghash_algo">image hash algo</param>
        /// <param name="img_index">image hashes</param>
        public BKTree(string image, int distance, Images.ImageHashAlgorithm imghash_algo, Images.ImageIndex.Index img_index)
        {
            image_path = image;
            distParent = distance;
            ImageHashAlgo(imghash_algo);
            image_index = img_index;
        }

        #endregion constructor

        #region Add image

        /// <summary>
        /// Add image to BK Tree
        /// </summary>
        /// <param name="image">image_name</param>
        public void Add(string image)
        {
            if (image_path == null)
            {
                image_path = image;
                distParent = 0;
                return;
            }
            else
            {
                int distance = DistanceMethod(image_path, image);

                if (subtrees.Count > 0)
                {
                    foreach (var sub in subtrees)
                    {
                        if (sub.distParent == distance)
                        {
                            sub.Add(image);
                            return;
                        }
                    }
                }

                subtrees.Add(new BKTree(image, distance, imghash_algo_used, image_index));
                return;
            }
        }

        #endregion Add image

        #region Search

        /// <summary>
        /// Search BK Tree
        /// </summary>
        /// <param name="image">image path</param>
        /// <param name="maxdist">max. distance</param>
        public Dictionary<string, int> Search(string image, int maxdist)
        {
            var results = new Dictionary<string, int>();

            maxdist = 100 - maxdist;

            var image_id = image_index.Id(image);
          
            var matches = RecursiveSearch(image_id, maxdist);
        
            foreach (var match in matches)
            {
                var image_name = image_index.Id(match.Key);
                results.Add(image_name, 100 - match.Value);
            }
            return results;
        }

        /// <summary>
        /// Search BK Tree recursively
        /// </summary>
        /// <param name="image">image path</param>
        /// <param name="maxdist">max. distance</param>
        /// <returns></returns>
        private Dictionary<string, int> RecursiveSearch(string image, int maxdist)
        {
            var matches = new Dictionary<string, int>();
            int distance = DistanceMethod(image_path, image);

            if (distance <= maxdist)
            {
                matches.Add(image_path, distance);
            }

            if (subtrees == null) { return matches; }
            if (subtrees.Count == 0) { return matches; }

            foreach (var sub in subtrees)
            {
                if ((sub.distParent <= (distance + maxdist)) &&
                    (sub.distParent >= (distance - maxdist)))
                {
                    var subtree_matches = sub.RecursiveSearch(image, maxdist);
                    foreach (var m in subtree_matches)
                    {
                        if (!matches.ContainsKey(m.Key))
                        {
                            matches.Add(m.Key, m.Value);
                        }
                    }
                }
            }

            return matches;
        }

        #endregion Search

        #region Image hash distance metric

        /// <summary>
        /// Distance between 2 images (delegate)
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        private delegate int Distance(string first, string second);

        /// <summary>
        ///  Distance between 2 images
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns>100 - similary</returns>
        private int ImageHashDistance(string first, string second)
        {
            double hash_sim = GetImageSimilarity(first, second);

            return (int)(100 - hash_sim);
        }

        /// <summary>
        /// Calculate similarity of 2 images in index
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns>[0-100%]</returns>
        public double GetImageSimilarity(string image1, string image2)
        {
            double sim = 0;

            sim = Images.ImageHash.HashSimilarity(GetImageHash(image1), GetImageHash(image2), imghash_algo_used);
	    if (sim > 100){sim = 100.0;}	
            return sim; 
        }

        #endregion Image hash distance metric

        #region Image hash methods

        /// <summary>
        /// Set image hash algorithm used and distance metric
        /// </summary>
        /// <param name="imghash_algo">image hash algo</param>
        private void ImageHashAlgo(Images.ImageHashAlgorithm imghash_algo)
        {
            imghash_algo_used = imghash_algo;
            DistanceMethod = ImageHashDistance;
        }

        /// <summary>
        /// Get image hash algorithm used
        /// </summary>
        /// <returns></returns>
        public string ImageHashAlgo()
        {
            return imghash_algo_used.ToString();
        }

        /// <summary>
        /// Get image hash from index
        /// </summary>
        /// <param name="image_name"></param>
        /// <returns></returns>
        public string GetImageHash(string image_name)
        {
            string hash = String.Empty;

            hash = image_index.ImageHash(image_name, imghash_algo_used);

            return hash;
        }

        #endregion Image hash methods

    } // BKTree
} //namespace