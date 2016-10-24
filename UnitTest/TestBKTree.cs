using DataStructure.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DataStructure.Test
{
[TestClass]
public class TestBKTree
{
    /// <summary>
    /// Test Levenshtein distance
    /// </summary>
    [TestMethod]
    public void BKTree_SearchLevenshtein()
    {
        var tree = new BKTree();
        tree.Add("Test");
        tree.Add("Test");
        tree.Add("Test2");
        tree.Add("taxi");
        tree.Add("Text");
        tree.Add("tttt");

        // test -> test
        var result_d0 = tree.Search("Test", 0).Count;
        Assert.AreEqual(1, result_d0);

        // te-t -> teSt teXt
        var result_d1 = tree.Search("te-t", 1).Count;
        Assert.AreEqual(2, result_d1);

        // test -> test test2 teXt tTTt
        var result_d2 = tree.Search("test", 2).Count;
        Assert.AreEqual(4, result_d2);
    }

    /// <summary>
    /// Test Hamming distance
    /// </summary>
    [TestMethod]
    public void BKTree_SearchHamming()
    {
        var tree = new BKTree(BKTree.DistanceMetric.Hamming);

        tree.Add("taxi");
        tree.Add("Test2");
        tree.Add("Text");
        tree.Add("Test");
        tree.Add("tttt");

        // Hamming distance Test = Test2 (compare the length of the shorter word)
        var results = tree.Search("Test", 0);
        var result_d0 = results.Count;
        Assert.AreEqual(2, result_d0);

        // te-t max. 1 diff :  teSt teSt2 teXt
        var result_d1 = tree.Search("te-t", 1).Count;
        Assert.AreEqual(3, result_d1);

        // test max. 2 diff : test test2 teXt tTTt
        var result_d2 = tree.Search("test", 2).Count;
        Assert.AreEqual(4, result_d2);
    }

    /// <summary>
    /// Test that multiple insertion of the same word does not give duplicate search results
    /// </summary>
    [TestMethod]
    public void BKTree_NoDuplicate()
    {
        var tree = new BKTree();
        tree.Add("AaaA");
        tree.Add("TaaT");
        tree.Add("TTTT");

        tree.Add("Test");
        tree.Add("Test");
        tree.Add("TEST");
        tree.Add("TeSt");
        tree.Add("TeST");

        tree.Add("Text");
        tree.Add("Text");
        tree.Add("TEXt");

        var result_d0 = tree.Search("Test", 0).Count;
        Assert.AreEqual(1, result_d0);

        result_d0 = tree.Search("Text", 0).Count;
        Assert.AreEqual(1, result_d0);

        result_d0 = tree.Search("aaaa", 0).Count;
        Assert.AreEqual(1, result_d0);

        result_d0 = tree.Search("taat", 0).Count;
        Assert.AreEqual(1, result_d0);

        result_d0 = tree.Search("tttt", 0).Count;
        Assert.AreEqual(1, result_d0);

        result_d0 = tree.Search("zzzz", 0).Count;
        Assert.AreEqual(0, result_d0);
    }

    //http://www.dotnetperls.com/fisher-yates-shuffle
    /// <summary>
    /// Shuffle array with Fisher-Yates alogirithm
    /// </summary>
    /// <typeparam name="T">Array element type.</typeparam>
    /// <param name="array">Array to shuffle.</param>
    public void Shuffle<T>(T[] array)
    {
        Random _random = new Random();
        int n = array.Length;
        for (int i = 0; i < n; i++)
            {
                int r = i + (int)(_random.NextDouble() * (n - i));
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
    }

    /// <summary>
    /// Test that the order insertion does not influence the search results
    /// </summary>
    [TestMethod]
    public void BKTree_InsertionOrder()
    {
        int iterations = 10;
        var tree = new BKTree();

        string[] array = { "Test", "TeSt", "AaaA", "TaaT", "TTTT", "Text", "TEXt", " ", "--", ":-)" };
        for (int i = 1; i <= iterations; i++)
            {
                Shuffle(array);
                Console.WriteLine("BKTree insertion: v{0}", i);
                foreach (string value in array)
                    {
                        tree.Add(value);
                        Console.WriteLine(value);
                    }
                Console.WriteLine(" ");

                var result_d0 = tree.Search("Test", 0).Count;
                Assert.AreEqual(1, result_d0);

                result_d0 = tree.Search("Text", 0).Count;
                Assert.AreEqual(1, result_d0);

                result_d0 = tree.Search("aaaa", 0).Count;
                Assert.AreEqual(1, result_d0);

                result_d0 = tree.Search("taat", 0).Count;
                Assert.AreEqual(1, result_d0);

                result_d0 = tree.Search("tttt", 0).Count;
                Assert.AreEqual(1, result_d0);

                result_d0 = tree.Search("no match", 0).Count;
                Assert.AreEqual(0, result_d0);
            }
    }
}
}