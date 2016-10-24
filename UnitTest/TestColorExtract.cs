using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Drawing;
using System.IO;

namespace SimilarImage.Test
{
[TestClass]
public class TestColorExtract
{
    private string test_data_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../UnitTest/images/");

    [TestMethod]
    public void ColorExtract_Colors()
    {
        var rectangleb = (Image)CommonUtils.ImageUtils.MakeSquare(Color.Black, 10, 20);
        var rectanglew = (Image)CommonUtils.ImageUtils.MakeSquare(Color.White, 10, 20);

        var colorsb = Images.ColorExtract.Colors(rectangleb);
        var colors = Images.ColorExtract.Colors(rectanglew);

        Assert.IsTrue(Images.ColorExtract.Compare(colorsb, colors) != 100);
        Assert.IsTrue(Images.ColorExtract.Compare(colorsb, colorsb) == 100);
        Assert.IsTrue(Images.ColorExtract.Compare(colors, colors) == 100);

        rectangleb.Dispose();
        rectanglew.Dispose();
    }

    [TestMethod]
    public void ColorExtract_Colors_File()
    {
        string lena = test_data_path + "lena.jpg";
        string koala = test_data_path + "koala1.jpg";

        var lena_img = Image.FromFile(lena, true);
        var koala_img = Image.FromFile(koala, true);

        var colorsl = Images.ColorExtract.Colors(lena_img);
        var colorsk = Images.ColorExtract.Colors(koala_img);

        Assert.IsTrue(Images.ColorExtract.Compare(colorsl, colorsk) != 100);
        Assert.IsTrue(Images.ColorExtract.Compare(colorsl, colorsl) == 100);
        Assert.IsTrue(Images.ColorExtract.Compare(colorsk, colorsk) == 100);

        lena_img.Dispose();
        koala_img.Dispose();
    }

    [TestMethod]
    public void ColorExtract_GetColorName()
    {
        Assert.IsTrue(Images.ColorExtract.GetColorName(Color.Black).Equals("Black"));
        Assert.IsTrue(Images.ColorExtract.GetColorName(Color.FromArgb(0, 0, 0)).Equals("Black"));

        Assert.IsTrue(Images.ColorExtract.GetColorName(Color.White).Equals("White"));
        Assert.IsTrue(Images.ColorExtract.GetColorName(Color.FromArgb(255, 255, 255)).Equals("White"));
    }

    [TestMethod]
    public void ColorExtract_MainColor()
    {
        var rectangleb = (Image)CommonUtils.ImageUtils.MakeSquare(Color.Black, 100, 200);
        var rectanglew = (Image)CommonUtils.ImageUtils.MakeSquare(Color.White, 100, 200);

        var mainb = Images.ColorExtract.MainColor(rectangleb);
        var mainw = Images.ColorExtract.MainColor(rectanglew);

        Assert.IsTrue(Images.ColorExtract.Compare(mainb, mainw) != 100);
        Assert.IsTrue(Images.ColorExtract.Compare(mainb, mainb) == 100);
        Assert.IsTrue(Images.ColorExtract.Compare(mainw, mainw) == 100);

        rectangleb.Dispose();
        rectanglew.Dispose();
    }

    [TestMethod]
    public void ColorExtract_TopColors()
    {
        var rectangleb = (Image)CommonUtils.ImageUtils.MakeSquare(Color.Black, 100, 200);
        var rectanglew = (Image)CommonUtils.ImageUtils.MakeSquare(Color.White, 100, 200);

        var topb = Images.ColorExtract.TopColors(rectangleb);
        var topw = Images.ColorExtract.TopColors(rectanglew);

        Assert.IsTrue(Images.ColorExtract.Compare(topb, topw) != 100);
        Assert.IsTrue(Images.ColorExtract.Compare(topb, topb) == 100);
        Assert.IsTrue(Images.ColorExtract.Compare(topw, topw) == 100);

        rectangleb.Dispose();
        rectanglew.Dispose();
    }
}
}