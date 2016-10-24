using Images;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SimilarImage.Test
{
[TestClass]
public class TestImageSimilarity
{
    private string test_data_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../UnitTest/images/");

    /// <summary>
    /// Test image comparison from images
    /// </summary>
    [TestMethod]
    public void ImageSimilarity_Compare()
    {
        var rectangleb = (Image)CommonUtils.ImageUtils.MakeSquare(Color.Black, 100, 200);
        var rectanglew = (Image)CommonUtils.ImageUtils.MakeSquare(Color.White, 100, 200);

        // same
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(rectangleb, rectangleb, ComparisonMethod.MainColor) == 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(rectangleb, rectangleb, ComparisonMethod.TopColors) == 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(rectangleb, rectangleb, ComparisonMethod.PixelsDifference) == 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(rectangleb, rectangleb, ComparisonMethod.PixelsDifferenceSorted) == 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(rectangleb, rectangleb, ComparisonMethod.PixelsDistance) == 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(rectangleb, rectangleb, ComparisonMethod.PixelsDistanceSorted) == 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(rectangleb, rectangleb, ComparisonMethod.RGBHistogramHash) == 100);

        // different
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(rectangleb, rectanglew, ComparisonMethod.MainColor) != 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(rectangleb, rectanglew, ComparisonMethod.TopColors) != 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(rectangleb, rectanglew, ComparisonMethod.PixelsDifference) != 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(rectangleb, rectanglew, ComparisonMethod.PixelsDifferenceSorted) != 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(rectangleb, rectanglew, ComparisonMethod.PixelsDistance) != 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(rectangleb, rectanglew, ComparisonMethod.PixelsDistanceSorted) != 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(rectangleb, rectanglew, ComparisonMethod.RGBHistogramHash) != 100);

        rectangleb.Dispose();
        rectanglew.Dispose();
    }

    /// <summary>
    /// Test image comparison from files
    /// </summary>
    [TestMethod]
    public void ImageSimilarity_Compare_File()
    {
        var lena = test_data_path + "lena.jpg";
        var koala = test_data_path + "koala1.jpg";

        // same
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(lena, lena, ComparisonMethod.TopColors) == 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(lena, lena, ComparisonMethod.MainColor) == 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(lena, lena, ComparisonMethod.PixelsDifference) == 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(lena, lena, ComparisonMethod.PixelsDifferenceSorted) == 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(lena, lena, ComparisonMethod.PixelsDistance) == 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(lena, lena, ComparisonMethod.PixelsDistanceSorted) == 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(lena, lena, ComparisonMethod.RGBHistogramHash) == 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(lena, lena, ComparisonMethod.Feature) == 100);

        // different
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(lena, koala, ComparisonMethod.TopColors) != 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(lena, koala, ComparisonMethod.MainColor) != 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(lena, koala, ComparisonMethod.PixelsDifference) != 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(lena, koala, ComparisonMethod.PixelsDifferenceSorted) != 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(lena, koala, ComparisonMethod.PixelsDistance) != 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(lena, koala, ComparisonMethod.PixelsDistanceSorted) != 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(lena, koala, ComparisonMethod.RGBHistogramHash) != 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(lena, koala, ComparisonMethod.Feature) != 100);
    }

    /// <summary>
    /// Test sorted pixel difference
    /// </summary>
    [TestMethod]
    public void ImageSimilarity_PixelsDifference_Sort()
    {
        var bars = test_data_path + "Symcamera1.jpg"; // black gray white bars
        var bars_img = Image.FromFile(bars, true);

        var bars_img_180 = Image.FromFile(bars, true);
        bars_img_180.RotateFlip(RotateFlipType.Rotate180FlipNone);

        Assert.IsTrue(Images.ImageSimilarity.CompareImages(bars_img, bars_img_180, ComparisonMethod.PixelsDifference) != 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(bars_img, bars_img_180, ComparisonMethod.PixelsDifferenceSorted) == 100);

        bars_img_180.RotateFlip(RotateFlipType.Rotate180FlipNone);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(bars_img, bars_img_180, ComparisonMethod.PixelsDifference) == 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(bars_img, bars_img_180, ComparisonMethod.PixelsDifferenceSorted) == 100);

        bars_img.Dispose();
        bars_img_180.Dispose();
    }

    /// <summary>
    /// Test pixel difference range [0-100%]
    /// </summary>
    [TestMethod]
    public void ImageSimilarity_PixelsDifference_Range()
    {
        // build list of colors from KnownColor
        List<string> colors = new List<string>();
        foreach (string colorName in Enum.GetNames(typeof(KnownColor)))
            {
                KnownColor knownColor = (KnownColor)Enum.Parse(typeof(KnownColor), colorName);
                colors.Add(colorName);
            }

        var c1 = Color.Firebrick;
        foreach (var color in colors)
            {
                var c2 = Color.FromName(color);
                Assert.IsTrue(Images.ImageSimilarity.Compare2Pixels(c1, c2) >= 0);
                Assert.IsTrue(Images.ImageSimilarity.Compare2Pixels(c1, c2) <= 100);

                var image1 = (Image)CommonUtils.ImageUtils.MakeSquare(c1, 2, 5);
                var image2 = (Image)CommonUtils.ImageUtils.MakeSquare(c2, 2, 5);
                var pixeldiff = Images.ImageSimilarity.CompareImages(image1, image2, ComparisonMethod.PixelsDifference);
                var spixeldiff = Images.ImageSimilarity.CompareImages(image1, image2, ComparisonMethod.PixelsDifferenceSorted);
                Assert.IsTrue(pixeldiff >= 0);
                Assert.IsTrue(spixeldiff >= 0);
                Assert.IsTrue(pixeldiff <= 100);
                Assert.IsTrue(spixeldiff <= 100);

                image1.Dispose();
                image2.Dispose();
            }
    }

    /// <summary>
    /// Test pixel distance range [0-100%]
    /// </summary>
    [TestMethod]
    public void ImageSimilarity_PixelsDistance_Range()
    {
        // build list of colors from KnownColor
        List<string> colors = new List<string>();
        foreach (string colorName in Enum.GetNames(typeof(KnownColor)))
            {
                KnownColor knownColor = (KnownColor)Enum.Parse(typeof(KnownColor), colorName);
                colors.Add(colorName);
            }

        var c1 = Color.Blue;
        foreach (var color in colors)
            {
                var c2 = Color.FromName(color);

                //var dist = Images.ImageSimilarity.Compare2Pixels_distance(c1, c2);
                //Console.WriteLine("{0} vs {1} dist = {2}",c1,c2,dist);

                Assert.IsTrue(Images.ImageSimilarity.Compare2Pixels_distance(c1, c2) >= 0);
                Assert.IsTrue(Images.ImageSimilarity.Compare2Pixels_distance(c1, c2) <= 100);

                var image1 = (Image)CommonUtils.ImageUtils.MakeSquare(c1, 2, 5);
                var image2 = (Image)CommonUtils.ImageUtils.MakeSquare(c2, 2, 5);

                var pixeldiff = Images.ImageSimilarity.CompareImages(image1, image2, Images.ComparisonMethod.PixelsDistance);
                var spixeldiff = Images.ImageSimilarity.CompareImages(image1, image2, Images.ComparisonMethod.PixelsDistanceSorted);

                //Console.WriteLine("{0} vs {1} dist = {2}", c1, c2, pixeldiff);

                Assert.IsTrue(pixeldiff >= 0);
                Assert.IsTrue(spixeldiff >= 0);
                Assert.IsTrue(pixeldiff <= 100);
                Assert.IsTrue(spixeldiff <= 100);

                image1.Dispose();
                image2.Dispose();
            }
    }

    /// <summary>
    /// Test sorted pixel distance
    /// </summary>
    [TestMethod]
    public void ImageSimilarity_PixelsDistance_Sort()
    {
        var bars = test_data_path + "Symcamera1.jpg"; // black gray white bars
        var bars_img = Image.FromFile(bars, true);

        var bars_img_180 = Image.FromFile(bars, true);
        bars_img_180.RotateFlip(RotateFlipType.Rotate180FlipNone);

        Assert.IsTrue(Images.ImageSimilarity.CompareImages(bars_img, bars_img_180, ComparisonMethod.PixelsDistance) != 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(bars_img, bars_img_180, ComparisonMethod.PixelsDistanceSorted) == 100);

        bars_img_180.RotateFlip(RotateFlipType.Rotate180FlipNone);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(bars_img, bars_img_180, ComparisonMethod.PixelsDistance) == 100);
        Assert.IsTrue(Images.ImageSimilarity.CompareImages(bars_img, bars_img_180, ComparisonMethod.PixelsDistanceSorted) == 100);

        bars_img.Dispose();
        bars_img_180.Dispose();
    }

    /// <summary>
    /// Test pixel tolerated difference
    /// </summary>
    [TestMethod]
    public void ImageSimilarity_2PixelsTolerance()
    {
        var color1 = Color.FromArgb(255, 0, 0);
        var color2 = Color.FromArgb(255, (int)Images.ImageSimilarity.Tolerance, (int)Images.ImageSimilarity.Tolerance);
        var color3 = Color.FromArgb(255, (int)Images.ImageSimilarity.Tolerance + 1, (int)Images.ImageSimilarity.Tolerance + 1);

        // within tolerated difference
        var test_1vs2 = Images.ImageSimilarity.Compare2Pixels(color1, color2);
        Assert.IsTrue(test_1vs2 == 100);

        var test_2vs3 = Images.ImageSimilarity.Compare2Pixels(color2, color3);
        Assert.IsTrue(test_2vs3 == 100);

        // exceed tolerated difference
        var test_1vs3 = Images.ImageSimilarity.Compare2Pixels(color1, color3);
        Assert.IsFalse(test_1vs3 == 100);
    }
}
}