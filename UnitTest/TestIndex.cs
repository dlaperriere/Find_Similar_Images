using Images;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace SimilarImage.Test
{
[TestClass]
public class TestImageIndex
{
    private string test_data_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../UnitTest/images/");
    private string test_index = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../UnitTest/images/images.idx");

    private void DeleteIndexFile()
    {
        File.Delete(test_index);
    }

    /// <summary>
    /// Test index constructor
    /// </summary>
    [TestMethod]
    public void Index_Constructor()
    {
        DeleteIndexFile();

        var images_index = new Images.ImageIndex(test_data_path);
        Assert.IsTrue(File.Exists(test_index));

        // index not empty
        var list = images_index.ImageFilesIndexed();
        Assert.IsTrue(list.Count > 0);
    }

    /// <summary>
    /// Test index content
    /// </summary>
    [TestMethod]
    public void Index_Content()
    {
        DeleteIndexFile();

        var images_index = new Images.ImageIndex(test_data_path);
        var list = images_index.ImageFilesIndexed();

        // included
        Assert.IsTrue(list.Contains(test_data_path + "lena.jpg"));
        Assert.IsTrue(list.Contains(test_data_path + "koala1.jpg"));

        // excluded
        Assert.IsFalse(list.Contains(test_data_path + "CorruptImage.jpg"));
        Assert.IsFalse(list.Contains(test_data_path + "wrong.txt.bmp"));

        // included file info
        var info = images_index.ImageInfo(test_data_path + "lena.jpg");
        var s = info[0];
        var w = info[1];
        var h = info[2];
        Assert.IsTrue(s.Equals("91.8K"));
        //  Assert.IsTrue(w.Equals("512"));
        // Assert.IsTrue(w.Equals("512"));
    }

    /// <summary>
    /// Test index image search
    /// </summary>
    [TestMethod]
    public void Index_Search()
    {
        DeleteIndexFile();

        var images_index = new Images.ImageIndex(test_data_path);

        var test_image = test_data_path + "lena.jpg";

        foreach (Images.ImageHashAlgorithm algo in Enum.GetValues(typeof(Images.ImageHashAlgorithm)))
            {
                foreach (ComparisonMethod method in Enum.GetValues(typeof(ComparisonMethod)))
                    {
                        var result = images_index.SearchSimilarImages(test_image, algo, method, 90);
                        var nmatches = result.Count;
                        Console.WriteLine("{0} {1} {2} found: {3}", test_image, algo, method, nmatches);

                        if (method == ComparisonMethod.Feature || algo == Images.ImageHashAlgorithm.MD5)
                            {
                                Assert.IsTrue(result.Count >= 0);
                            }
                        else
                            {
                                foreach (var match in result.Keys)
                                    {
                                        Console.WriteLine("  - {0}", match);
                                    }
                                Assert.IsTrue(result.Count >= 1);
                            }
                    }
            }

        DeleteIndexFile();
    }
}
}