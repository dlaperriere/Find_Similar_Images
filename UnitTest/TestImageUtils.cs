using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Drawing;
using System.IO;

namespace CommonUtils.Test
{
[TestClass]
public class TestImageUtils
{
    public static double allowed_difference = 0.000000001;
    private string test_data_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../UnitTest/images/");

    /// <summary>
    /// Test image format from file
    /// </summary>
    [TestMethod]
    public void ImageUtils_GetFormat()
    {
        string dir = test_data_path;
        var images_index = new Images.ImageIndex(dir);

        var jpg = dir + "lena.jpg";
        var format = CommonUtils.ImageUtils.GetImageFormat(jpg);
        Assert.IsTrue(format == System.Drawing.Imaging.ImageFormat.Jpeg);

        var bmp = dir + "koala1.bmp";
        format = CommonUtils.ImageUtils.GetImageFormat(bmp);
        Assert.IsTrue(format == System.Drawing.Imaging.ImageFormat.Bmp);

        var png = dir + "koala1.png";
        format = CommonUtils.ImageUtils.GetImageFormat(png);
        Assert.IsTrue(format == System.Drawing.Imaging.ImageFormat.Png);

        var gif = dir + "koala1.gif";
        format = CommonUtils.ImageUtils.GetImageFormat(gif);
        Assert.IsTrue(format == System.Drawing.Imaging.ImageFormat.Gif);

        var tiff = dir + "koala1.tif";
        format = CommonUtils.ImageUtils.GetImageFormat(tiff);
        Assert.IsTrue(format == System.Drawing.Imaging.ImageFormat.Tiff);
    }

    /// <summary>
    /// Test image format from image
    /// </summary>
    [TestMethod]
    public void ImageUtils_GetFormat_Image()
    {
        string dir = test_data_path;
        var images_index = new Images.ImageIndex(dir);

        var jpg = dir + "lena.jpg";
        var img = Image.FromFile(jpg, true);
        var format = CommonUtils.ImageUtils.GetImageFormat(img);
        Assert.IsTrue(format == System.Drawing.Imaging.ImageFormat.Jpeg);
        img.Dispose();

        var bmp = dir + "koala1.bmp";
        img = Image.FromFile(bmp, true);
        format = CommonUtils.ImageUtils.GetImageFormat(img);
        Assert.IsTrue(format == System.Drawing.Imaging.ImageFormat.Bmp);
        img.Dispose();

        var png = dir + "koala1.png";
        img = Image.FromFile(png, true);
        format = CommonUtils.ImageUtils.GetImageFormat(img);
        Assert.IsTrue(format == System.Drawing.Imaging.ImageFormat.Png);
        img.Dispose();

        var gif = dir + "koala1.gif";
        img = Image.FromFile(gif, true);
        format = CommonUtils.ImageUtils.GetImageFormat(img);
        Assert.IsTrue(format == System.Drawing.Imaging.ImageFormat.Gif);
        img.Dispose();

        var tiff = dir + "koala1.tif";
        img = Image.FromFile(tiff, true);
        format = CommonUtils.ImageUtils.GetImageFormat(img);
        Assert.IsTrue(format == System.Drawing.Imaging.ImageFormat.Tiff);
        img.Dispose();
    }

    /// <summary>
    /// Test merge 2 images
    /// </summary>
    [TestMethod]
    public void ImageUtils_Merge()
    {
        var rectangleb = CommonUtils.ImageUtils.MakeSquare(Color.Black, 100, 200);
        var rectanglew = CommonUtils.ImageUtils.MakeSquare(Color.White, 100, 200);

        var merged = CommonUtils.ImageUtils.MergeImages(rectangleb, rectanglew);

        var pixelb = merged.GetPixel(50, 50);
        Assert.AreEqual(0, pixelb.R);
        Assert.AreEqual(0, pixelb.G);
        Assert.AreEqual(0, pixelb.B);

        var pixelw = merged.GetPixel(150, 50);
        Assert.AreEqual(255, pixelw.R);
        Assert.AreEqual(255, pixelw.G);
        Assert.AreEqual(255, pixelw.B);
    }

    /// <summary>
    /// Test image resize
    /// </summary>
    [TestMethod]
    public void ImageUtils_Resize()
    {
        var rectangle = CommonUtils.ImageUtils.MakeSquare(Color.Black, 100, 200);

        var rectangle_smaller = CommonUtils.ImageUtils.Resize(rectangle, 50, 50, false);
        Assert.AreEqual(50, rectangle_smaller.Width);
        Assert.AreEqual(50, rectangle_smaller.Height);
    }

    /// <summary>
    /// Test image resize with aspect ratio
    /// </summary>
    [TestMethod]
    public void ImageUtils_ResizeAspectRatio()
    {
        var rectangle = CommonUtils.ImageUtils.MakeSquare(Color.Black, 100, 200);

        var rectangle_smaller_ratio = CommonUtils.ImageUtils.Resize(rectangle, 50, 50, true);
        Assert.AreEqual(25, rectangle_smaller_ratio.Width);
        Assert.AreEqual(50, rectangle_smaller_ratio.Height);

        var rectangle_smaller_ratio2 = CommonUtils.ImageUtils.Resize(rectangle, 50, 50, System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor);
        Assert.AreEqual(25, rectangle_smaller_ratio2.Width);
        Assert.AreEqual(50, rectangle_smaller_ratio2.Height);
    }
}
}