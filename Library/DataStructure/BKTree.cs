// BK-tree   https://en.wikipedia.org/wiki/BK-tree
//
// A BK-tree is a metric tree suggested by Walter Austin Burkhard and Robert M. Keller specifically adapted to discrete metric spaces.
// For simplicity, let us consider integer discrete metric d(x,y). Then, BK-tree is defined in the following way. An arbitrary element a is
// selected as root node. The root node may have zero or more subtrees. The k-th subtree is recursively built of all elements b such that
// d(a,b) = k. BK-trees can be used for approximate string matching in a dictionary.
//
// Reference
// W. Burkhard and R. Keller. Some approaches to best-match file searching, CACM, 1973
//  http://portal.acm.org/citation.cfm?doid=362003.362025
//
// Copyright (C) David Laperriere

using System;
using System.Collections.Generic;

namespace DataStructure.Text
{
    /// <summary>
    /// BKTree string metric tree
    /// </summary>
    public class BKTree
    {
        #region BKTree data

        private int distParent = 0;
        private string word = null;
        private List<BKTree> subtrees = new List<BKTree>();

        public enum DistanceMetric { Hamming, Levenshtein }

        private DistanceMetric metric;
        private Distance DistanceMethod;

        #endregion BKTree data

        #region constructor

        /// <summary>
        /// Default constructor, use Levenshtein as distance metric
        /// </summary>
        public BKTree()
        {
            metric = DistanceMetric.Levenshtein;
            DistanceMethod = LevenshteinDistance;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dm">distance metric</param>
        public BKTree(DistanceMetric dm)
        {
            if (dm == DistanceMetric.Hamming)
            {
                metric = DistanceMetric.Hamming;
                DistanceMethod = HammingDistance;
            }
            else
            {
                metric = DistanceMetric.Levenshtein;
                DistanceMethod = LevenshteinDistance;
            }
        }

        /// <summary>
        /// Constructor (Add to subtree)
        /// </summary>
        /// <param name="w">word</param>
        /// <param name="dist">distance</param>
        /// <param name="dm">distance metric</param>
        public BKTree(string w, int dist, DistanceMetric dm)
        {
            w = w.ToLower();
            metric = dm;
            if (dm == DistanceMetric.Hamming)
            {
                DistanceMethod = HammingDistance;
            }
            else
            {
                DistanceMethod = LevenshteinDistance;
            }
            word = w;
            distParent = dist;
        }

        #endregion constructor

        #region add

        /// <summary>
        /// Add word to tree
        /// </summary>
        /// <param name="w">word</param>
        public void Add(string w)
        {
            w = w.ToLower();

            if (word == null)
            {
                word = w;
                distParent = 0;
                return;
            }
            else
            {
                int inDst = DistanceMethod(word, w);

                if (subtrees.Count > 0)
                {
                    foreach (var sub in subtrees)
                    {
                        if (sub.distParent == inDst)
                        {
                            sub.Add(w);
                            return;
                        }
                    }
                }

                subtrees.Add(new BKTree(w, inDst, metric));
                return;
            }
        }

        #endregion add

        #region search

        /// <summary>
        /// Search word
        /// </summary>
        /// <param name="w">word</param>
        /// <param name="maxdist">max distance (0 == no difference)</param>
        /// <returns></returns>
        public Dictionary<string, int> Search(string w, int maxdist)
        {
            w = w.ToLower();

            var matches = new Dictionary<string, int>();

            int distance = DistanceMethod(word, w);

            if (distance <= maxdist)
            {
                matches.Add(word, distance);
            }

            foreach (var sub in subtrees)
            {
                if ((sub.distParent <= (distance + maxdist)) &&
                    (sub.distParent >= (distance - maxdist)))
                {
                    var srtn = sub.Search(w, maxdist);
                    foreach (var m in srtn)
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

        #endregion search

        #region distance metric

        /// <summary>
        ///  Distance between 2 words (delegate)
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        private delegate int Distance(string first, string second);

        /// <summary>
        /// Hamming Distance of 2 strings
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns>Number of differences (lower values equals more similar)</returns>
        private static int HammingDistance(string first, string second)
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
        /// Levenshtein Distance of 2 strings
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns>number of edits to change one into the other</returns>
        private static int LevenshteinDistance(string first, string second)
        {
            if (first.Length == 0) return second.Length;
            if (second.Length == 0) return first.Length;

            var lenFirst = first.Length;
            var lenSecond = second.Length;

            var d = new int[lenFirst + 1, lenSecond + 1];

            for (var i = 0; i <= lenFirst; i++)
                d[i, 0] = i;

            for (var i = 0; i <= lenSecond; i++)
                d[0, i] = i;

            for (var i = 1; i <= lenFirst; i++)
            {
                for (var j = 1; j <= lenSecond; j++)
                {
                    var match = (first[i - 1] == second[j - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + match);
                }
            }

            return d[lenFirst, lenSecond];
        }

        #endregion distance metric
    }
}