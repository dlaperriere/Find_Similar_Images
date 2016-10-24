using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Drawing;
using System.IO;

namespace SimilarImage.Test
{
[TestClass]
public class TestImagePHash
{
    private string test_data_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../UnitTest/images/");

    /// <summary>
    /// Test image hash from image & file name
    /// </summary>
    [TestMethod]
    public void PHash()
    {
        string lena = test_data_path + "lena.jpg";
        string koala = test_data_path + "koala1.jpg";

        var lena_img = Image.FromFile(lena, true);
        var koala_img = Image.FromFile(koala, true);

        var hash_lena = Images.ImagePHash.PerceptiveHash(lena_img);
        var hash_koala = Images.ImagePHash.PerceptiveHash(koala_img);

        var hash_lena2 = Images.ImagePHash.PerceptiveHash(lena);
        var hash_koala2 = Images.ImagePHash.PerceptiveHash(koala);

        // hash from image == hash from file name
        Assert.IsTrue(hash_lena.Equals(hash_lena2));
        Assert.IsTrue(hash_koala.Equals(hash_koala2));

        // similarity range
        Assert.IsTrue(Images.ImagePHash.Similarity(hash_lena, hash_koala) >= 0);
        Assert.IsTrue(Images.ImagePHash.Similarity(hash_lena, hash_koala) <= 100);

        // test same
        Assert.IsTrue(Images.ImagePHash.Compare(lena_img, lena_img) == 100);

        // test different
        Assert.IsTrue(Images.ImagePHash.Compare(lena_img, koala_img) != 100);

        lena_img.Dispose();
        koala_img.Dispose();
    }

    /// <summary>
    /// Test image hash similarity range [0-100%]
    /// </summary>
    [TestMethod]
    public void PHash_Range()
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
                var similarity = Images.ImagePHash.Compare(test_image, image);

                Console.WriteLine("{0} vs {1} similarity = {2}", test_image_name, file, similarity);
                Assert.IsTrue(similarity >= 0);
                Assert.IsTrue(similarity <= 100);
                image.Dispose();
            }
    }
}
}