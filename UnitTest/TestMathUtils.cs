using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace CommonUtils.Test
{
[TestClass]
public class TestMathUtils
{
    public static double allowed_difference = 0.000000001;

    /// <summary>
    /// Test GetMedian
    /// </summary>
    [TestMethod]
    public void MathUtils_Median()
    {
        // array needs to be sorted
        int[] x = { 1, 2, 4, 3, 5, 6, 7 };
        int[] sx = new int[x.Length];
        x.CopyTo(sx, 0);
        Array.Sort(sx);

        var med_x = CommonUtils.MathUtils.GetMedian(x);
        var med_sx = CommonUtils.MathUtils.GetMedian(sx);

        Assert.AreNotEqual(med_x, med_sx);
        Assert.AreEqual(3, med_x);
        Assert.AreEqual(4, med_sx);

        // odd # of elements
        int[] y = { 7, 6, 5 };
        var med_y = CommonUtils.MathUtils.GetMedian(y);

        Assert.AreEqual(6, med_y);

        // even # of elements
        int[] z = { 1, 2, 3, 4 };
        var med_z = CommonUtils.MathUtils.GetMedian(z);

        Assert.AreEqual(2.5, med_z, allowed_difference);
    }

    /// <summary>
    /// Test pearson correlation
    /// </summary>
    [TestMethod]
    public void MathUtils_PearsonCorrelation()
    {
        double[] x = { 1, 2, 3, 4, 5, 6, 7 };
        double[] y = { 7, 6, 5, 4, 3, 2, 1 };
        double[] z = { 12, 45, 3, 2, 3, 2, 1 };

        var x_vs_x = CommonUtils.MathUtils.PearsonCorrelation(x, x);
        Assert.AreEqual(1, x_vs_x);

        var y_vs_y = CommonUtils.MathUtils.PearsonCorrelation(y, y);
        Assert.AreEqual(1, y_vs_y);

        var x_vs_y = CommonUtils.MathUtils.PearsonCorrelation(x, y);
        Assert.AreEqual(-1, x_vs_y);

        // compare results with excel correl method
        var x_vs_z = CommonUtils.MathUtils.PearsonCorrelation(x, z);
        Assert.AreEqual(-0.573922349, x_vs_z, allowed_difference);

        var y_vs_z = CommonUtils.MathUtils.PearsonCorrelation(z, y);
        Assert.AreEqual(0.573922349, y_vs_z, allowed_difference);
    }
}
}