using DataStructure.Image;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DataStructure.Test
{
[TestClass]
public class TestBKTreeImage
{
    [TestMethod]
    public void BKTreeImage_Constructor()
    {
        var index = new Images.ImageIndex.Index();
        var image_hashs = new ConcurrentDictionary<String, List<String>>();
        var hash_algo = Images.ImageHashAlgorithm.Average;

        ulong ahash1 = 181;
        ulong ahash2 = 171;

        var hash1 = new List<String> { ahash1.ToString(), "1111111111" };
        index.Add("Test", hash1);

        var hash2 = new List<String> { ahash2.ToString(), "1111111110" };
        index.Add("Test2", hash2);

        var tree = new BKTree(hash_algo, index);
        var id = index.Id("Test");
        tree.Add(id);
        id = index.Id("Test2");
        tree.Add(id);

        // using Average tree algo
        Assert.AreEqual("Average", tree.ImageHashAlgo());

        Assert.AreEqual(ahash1.ToString(), tree.GetImageHash("Test"));
        Assert.AreEqual(ahash2.ToString(), tree.GetImageHash("Test2"));

        Assert.AreEqual(100.0, tree.GetImageSimilarity("Test", "Test"));
        Assert.AreNotEqual(100.0, tree.GetImageSimilarity("Test", "Test2"));

        // using PerceptiveColor tree algo
        hash_algo = Images.ImageHashAlgorithm.PerceptiveColor;
        image_hashs = new ConcurrentDictionary<String, List<String>>();
        index = new Images.ImageIndex.Index();
        tree = new BKTree(hash_algo, index);

        Assert.AreEqual("PerceptiveColor", tree.ImageHashAlgo());
    }

    [TestMethod]
    public void BKTreeImage_Search()
    {
        var index = new Images.ImageIndex.Index();
        var hash_algo = Images.ImageHashAlgorithm.Average;

        ulong ahash1 = 181;
        ulong ahash2 = 171;

        var hash1 = new List<String> { ahash1.ToString(), "1111111111" };
        index.Add("Test", hash1);

        var hash2 = new List<String> { ahash2.ToString(), "1111111110" };
        index.Add("Test2", hash2);

        var hash3 = new List<String> { ahash1.ToString(), "1111111111" };
        index.Add("Test3", hash3);

        var hash4 = new List<String> { ahash1.ToString(), "1111111111" };
        index.Add("Test", hash4);

        var tree = new BKTree(hash_algo, index);

        var id = index.Id("Test");
        tree.Add(id);
        id = index.Id("Test2");
        tree.Add(id);
        id = index.Id("Test3");
        tree.Add(id);

        // index= test test2 test3
        Assert.AreEqual(3, index.FileCount());

        Assert.AreEqual(ahash1.ToString(), tree.GetImageHash("Test"));
        Assert.AreEqual(ahash2.ToString(), tree.GetImageHash("Test2"));

        Assert.AreEqual(100.0, tree.GetImageSimilarity("Test", "Test"));
        Assert.AreNotEqual(100.0, tree.GetImageSimilarity("Test", "Test2"));

        // test -> test test3
        var result_d0 = tree.Search("Test", 100).Count;
        Assert.AreEqual(2, result_d0);

        // test2 block hash =  171
        Assert.AreEqual(ahash2.ToString(), tree.GetImageHash("Test2"));

        // using Average tree algo
        Assert.AreEqual("Average", tree.ImageHashAlgo());
    }

    [TestMethod]
    public void BKTreeImage_SearchBlock()
    {
        var index = new Images.ImageIndex.Index();
        var hash_algo = Images.ImageHashAlgorithm.Block;

        ulong ahash1 = 181;

        var hash1 = new List<String> { ahash1.ToString(), "1111111111" };
        index.Add("Test", hash1);

        var hash2 = new List<String> { ahash1.ToString(), "1111111110" };
        index.Add("Test2", hash2);

        var hash3 = new List<String> { ahash1.ToString(), "1111111111" };
        index.Add("Test3", hash3);

        var hash4 = new List<String> { ahash1.ToString(), "1111111111" };
        index.Add("Test", hash4);

        var tree = new BKTree(hash_algo, index);

        var id = index.Id("Test");
        tree.Add(id);
        id = index.Id("Test2");
        tree.Add(id);
        id = index.Id("Test3");
        tree.Add(id);

        // index= test test2 test3
        Assert.AreEqual(3, index.FileCount());

        Assert.AreEqual("1111111111", tree.GetImageHash("Test"));
        Assert.AreEqual("1111111110", tree.GetImageHash("Test2"));

        Assert.AreEqual(100.0, tree.GetImageSimilarity("Test", "Test"));
        Assert.AreNotEqual(100.0, tree.GetImageSimilarity("Test", "Test2"));

        // test -> test test3
        var result_d100 = tree.Search("Test", 100).Count;
        Assert.AreEqual(2, result_d100);

        // using test2 block hash
        Assert.AreEqual("1111111110", tree.GetImageHash("Test2"));

        // using block tree algo
        Assert.AreEqual("Block", tree.ImageHashAlgo());
    }

    [TestMethod]
    public void BKTreeImage_NoDuplicate()
    {
        var index = new Images.ImageIndex.Index();
        var hash_algo = Images.ImageHashAlgorithm.Block;

        ulong ahash1 = 181;
        ulong ahash2 = 281;

        var hash1 = new List<String> { ahash1.ToString(), "1111111111" };
        index.Add("Test", hash1);

        var hash2 = new List<String> { ahash2.ToString(), "1111111111" };
        index.Add("Test3", hash2);

        var hash3 = new List<String> { ahash1.ToString(), "1111110000" };
        index.Add("Test2", hash3);

        var hash4 = new List<String> { ahash2.ToString(), "000000000000000000000000" };
        index.Add("dup", hash4);

        var tree = new BKTree(hash_algo, index);

        var id = index.Id("Test");
        tree.Add(id);
        tree.Add(id);
        id = index.Id("Test2");
        tree.Add(id);
        tree.Add(id);
        id = index.Id("Test3");
        tree.Add(id);
        tree.Add(id);
        id = index.Id("dup");
        tree.Add(id);
        tree.Add(id);
        tree.Add(id);

        // index= test test2 test3 dup
        Assert.AreEqual(4, index.FileCount());

        // test -> test test3
        var result_d100 = tree.Search("Test", 100).Count;
        Assert.AreEqual(2, result_d100);

        // dup -> dup
        var result_d100_2 = tree.Search("dup", 100).Count;
        Assert.AreEqual(1, result_d100_2);

        // dup block hash
        Assert.AreEqual("000000000000000000000000", tree.GetImageHash("dup"));

        // tree algo
        Assert.AreEqual("Block", tree.ImageHashAlgo());
    }
}
}