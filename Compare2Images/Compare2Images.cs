// Copyright (C) David Laperriere

using System;
using System.Windows.Forms;

namespace Compare2Images
{
internal static class Compare2Images
{
    /// <summary>
    /// GUI to compare 2 images based on color histograms andperceptual hashes.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Compare2ImagesForm());
    }
}
}