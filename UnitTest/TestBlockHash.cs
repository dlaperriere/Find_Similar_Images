using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Drawing;
using System.IO;

//https://github.com/shimat/opencvsharp

namespace SimilarImage.Test
{
[TestClass]
public class TestImageBlockHash
{
    private string test_data_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../UnitTest/images/");

    /// <summary>
    /// Test image hash from image & file name
    /// </summary>
    [TestMethod]
    public void BlockHash()
    {
        string lena = test_data_path + "lena.jpg";
        string koala = test_data_path + "koala1.jpg";

        var lena_img = Image.FromFile(lena, true);
        var koala_img = Image.FromFile(koala, true);

        // error when testing with Keep Test Execution Engine Running enabled?
        // System.TypeInitializationException: The type initializer for 'OpenCvSharp.NativeMethods' threw an exception. --->
        // System.NotSupportedException: Delegates cannot be marshaled from native code into a domain other than their home domain.
        // https://connect.microsoft.com/VisualStudio/feedback/details/771994/vstest-executionengine-x86-exe-32-bit-not-closing-vs2012-11-0-50727-1-rtmrel

        var hash_lena = Images.ImageBlockHash.BlockHash(lena_img);
        var hash_koala = Images.ImageBlockHash.BlockHash(koala_img);

        var hash_lena2 = Images.ImageBlockHash.BlockHash(lena);
        var hash_koala2 = Images.ImageBlockHash.BlockHash(koala);

        // hash from image == hash from file name
        //Console.WriteLine("f: {0}\ni: {1}", hash_koala2, hash_koala);
        Assert.IsTrue(hash_lena.Equals(hash_lena2));
        Assert.IsTrue(hash_koala.Equals(hash_koala2));

        // similarity range
        Assert.IsTrue(Images.ImageBlockHash.Similarity(hash_lena, hash_koala) >= 0);
        Assert.IsTrue(Images.ImageBlockHash.Similarity(hash_lena, hash_koala) <= 100);

        // test same
        Assert.IsTrue(Images.ImageBlockHash.Compare(lena_img, lena_img) == 100);

        // test different
        Assert.IsTrue(Images.ImageBlockHash.Compare(lena_img, koala_img) != 100);

        lena_img.Dispose();
        koala_img.Dispose();
    }

    /// <summary>
    /// Test image hash similarity range [0-100%]
    /// </summary>
    [TestMethod]
    public void BlockHash_Range()
    {
        string dir = test_data_path;
        var images_index = new Images.ImageIndex(dir);

        var test_index = dir + "images.idx";
        File.Delete(test_index);

        var test_image_name = dir + "lena.jpg";
        var test_image = Image.FromFile(test_image_name, true);

        var files = images_index.ImageFilesIndexed();

        foreach (var file in files)
            {
                var image = Image.FromFile(file, true);
                var similarity = Images.ImageBlockHash.Compare(test_image, image);

                Console.WriteLine("{0} vs {1} similarity = {2}", test_image_name, file, similarity);
                Assert.IsTrue(similarity >= 0);
                Assert.IsTrue(similarity <= 100);
                image.Dispose();
            }
    }
}
}