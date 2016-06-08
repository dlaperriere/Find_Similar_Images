// Compare 2 images
// Copyright (C) David Laperriere

using Images;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Compare2Images
{
public partial class Compare2ImagesForm : System.Windows.Forms.Form
{
    #region form data

    private String image1_path;
    private String image2_path;

    private Image image1;
    private Image image2;

    private Image display_image1;
    private Image display_image2;

    private bool image_changed;

    #endregion form data

    #region form options

    private double similarity_threshold = 75;

    private Color color_same = Color.DarkBlue;
    private Color color_similar = Color.DarkCyan;
    private Color color_different = Color.DarkRed;

    private System.Drawing.Size smaller_img_size = new System.Drawing.Size(Images.ImageSimilarity.SmallerSize, Images.ImageSimilarity.SmallerSize);
    private System.Drawing.Size rgb_img_size = new System.Drawing.Size(100, 60);
    private System.Drawing.Size feature_img_size = new System.Drawing.Size(200, 60);

    #endregion form options

    #region constructor

    /// <summary>
    /// Constructor
    /// </summary>
    public Compare2ImagesForm()
    {
        InitializeComponent();
        AllowDrop = true;
        this.AllowDrop = true;
        image_changed = false;

        string text_dd = "Drag and drop an image in the box bellow";

        label_image1.Text = text_dd;
        label_image1.Refresh();
        label_image2.Text = text_dd;
        label_image2.Refresh();

        double similarity = 0.0;
        UpdateSimilarityLabel(this.label_AvgHash_similarity, similarity);
        UpdateSimilarityLabel(this.label_BlockHash_similarity, similarity);
        UpdateSimilarityLabel(this.label_DiffHash_similarity, similarity);
        UpdateSimilarityLabel(this.label_MD5Hash_similarity, similarity);
        UpdateSimilarityLabel(this.label_PHash_similarity, similarity);
        UpdateSimilarityLabel(this.label_HistogramHash_similarity, similarity);

        UpdateSimilarityLabel(this.label_DiffPixelsRgb_similarity, similarity);
        UpdateSimilarityLabel(this.label_DiffPixelsRgb_sorted_similarity, similarity);
        UpdateSimilarityLabel(this.label_DistPixelsRgb_similarity, similarity);
        UpdateSimilarityLabel(this.label_DistPixelsRgb_sorted_similarity, similarity);

        UpdateSimilarityLabel(this.label_RGBHistogram_similarity, similarity);
        UpdateSimilarityLabel(this.label_RGBHistogramAverage_similarity, similarity);

        UpdateSimilarityLabel(this.label_Feature_similarity, similarity);
        UpdateSimilarityLabel(this.labelDominantColors_similarity, similarity);

        GroupBox_Similarity.Refresh();
        CenterToScreen();
    }

    #endregion constructor

    #region form methods

    #region similarity threshold

    /// <summary>
    ///  access similarity threshold
    /// </summary>
    public double SimilarityThreshold
    {
        get
            {
                return similarity_threshold;
            }
        set
            {
                similarity_threshold = value;
            }
    }

    #endregion similarity threshold

    #region form events (size change, drag&drop)

    private void LoadForm(object sender, EventArgs e)
    {
        UpdateForm();
    }

    #region picture box drag & drop

    /// <summary>
    ///  Update Image after Drap&Drop
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="devent"></param>
    private void PictureBoxDragDrop(object sender, DragEventArgs devent)
    {
        string filename = DroppedImageName(devent);

        try
            {
                if (sender.Equals(this.pictureBox_image1))
                    {
                        devent.Effect = DragDropEffects.Copy;

                        image1_path = Path.GetFileName(filename);
                        if (image1_path.Length > 30)
                            {
                                image1_path = image1_path.Substring(0, 10) + ".." + image1_path.Substring(image1_path.Length - 10, 10);
                            }

                        image1 = Image.FromFile(@filename, true);

                        string fileinfo = String.Format("{1}x{2} {0}", image1_path, image1.Width, image1.Height);
                        label_image1.Text = fileinfo;
                        label_image1.Refresh();

                        display_image1 = CommonUtils.ImageUtils.Resize(image1, pictureBox_image1.Size);
                        pictureBox_image1.Image = display_image1;
                        pictureBox_image1.Refresh();

                        var display_small1 = CommonUtils.ImageUtils.Resize(pictureBox_image1.Image, pictureBox_small1_gray.Width, pictureBox_small1_gray.Height);
                        pictureBox_small1.Image = display_small1;
                        pictureBox_small1.Refresh();
                        pictureBox_small1_gray.Image = CommonUtils.ImageUtils.MakeGrayscaleFastest(display_small1);
                        pictureBox_small1_gray.Refresh();

                        label_message.Text = " ";
                        label_message.Refresh();
                    }

                if (sender.Equals(this.pictureBox_image2))
                    {
                        devent.Effect = DragDropEffects.Copy;

                        image2_path = Path.GetFileName(filename);
                        if (image2_path.Length > 30)
                            {
                                image2_path = image2_path.Substring(0, 10) + ".." + image2_path.Substring(image2_path.Length - 10, 10);
                            }

                        image2 = Image.FromFile(@filename, true);
                        string fileinfo = String.Format("{1}x{2} {0}", Path.GetFileName(image2_path), image2.Width, image2.Height);
                        label_image2.Text = fileinfo;
                        label_image2.Refresh();

                        display_image2 = CommonUtils.ImageUtils.Resize(image2, pictureBox_image2.Size);
                        pictureBox_image2.Image = display_image2;
                        pictureBox_image2.Refresh();

                        var display_small2 = CommonUtils.ImageUtils.Resize(pictureBox_image2.Image, pictureBox_small2_gray.Width, pictureBox_small2_gray.Height);
                        pictureBox_small2.Image = display_small2;
                        pictureBox_small2.Refresh();
                        pictureBox_small2_gray.Image = CommonUtils.ImageUtils.MakeGrayscaleFastest(display_small2);
                        pictureBox_small2_gray.Refresh();

                        label_message.Text = " ";
                        label_message.Refresh();
                    }

                image_changed = true;
                ImageSimilarityStats();
                GroupBox_Similarity.Refresh();
            }
        catch (System.IO.FileNotFoundException)
            {
                string msg = "could not read the image.";
                label_message.Text = msg;
                label_message.Refresh();
            }
        catch (System.OutOfMemoryException)
            {
#if DEBUG
                string msg = "The file does not have a valid image format (BMP ,GIF , JPEG, PNG or TIFF ).";
                string caption = "Error: wrong format";
                MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#endif
            }
        catch (System.ArgumentException e)
            {
                var error = e.ToString();
                string name = filename;
                if (filename.Length > 30)
                    {
                        name = filename.Substring(0, 10) + ".." + filename.Substring(filename.Length - 10, 10);
                    }
                string msg = "could not read file " + name;
#if DEBUG
                msg = "could not read file :" + error;
                string caption = String.Empty;
                MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#endif
                label_message.Text = msg;
                label_message.Refresh();
            }
    }

    /// <summary>
    ///  Get Drag&Drop file name
    /// </summary>
    /// <param name="devent"></param>
    /// <returns></returns>
    private static String DroppedImageName(DragEventArgs devent)
    {
        string filename = String.Empty;
        Array data = ((IDataObject)devent.Data).GetData("FileNameW") as Array;
        if (data != null)
            {
                if ((data.Length == 1) && (data.GetValue(0) is String))
                    {
                        filename = ((string[])data)[0];
                    }
            }

        return filename;
    }

    /// <summary>
    ///  Handle form DragEnter event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="devent"></param>
    private void FormDragEnter(object sender, DragEventArgs devent)
    {
        devent.Effect = DragDropEffects.None;
    }

    /// <summary>
    ///  Handle picturebox DragEnter event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="devent"></param>
    private void PictureBoxDragEnter(object sender, DragEventArgs devent)
    {
        devent.Effect = DragDropEffects.Copy;
    }

    /// <summary>
    /// Handle picturebox DragLeave event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="devent"></param>
    private void PictureBoxDragLeave(object sender, EventArgs devent)
    {
        ((DragEventArgs)devent).Effect = DragDropEffects.None;
    }

    #endregion picture box drag & drop

    #region update form

    /// <summary>
    /// Update images & dimensions
    /// </summary>
    private void UpdateForm()
    {
        int fh = this.Size.Height;
        int fw = this.Size.Width;

        // Images
        UpdateImages();

        pictureBox_small1_gray.Size = new System.Drawing.Size(16, 16);
        pictureBox_small1_gray.Refresh();
        pictureBox_small2_gray.Size = new System.Drawing.Size(16, 16);
        pictureBox_small2_gray.Refresh();

        // RGB histogram
        pictureBox_RGB1.Size = new System.Drawing.Size(fw / 8, pictureBox_RGB1.Height);
        pictureBox_RGB1.Refresh();

        pictureBox_RGB2.Size = new System.Drawing.Size(fw / 8, pictureBox_RGB2.Height);
        pictureBox_RGB2.Refresh();

        pictureBox_Feature.Size = new System.Drawing.Size(pictureBox_RGB1.Width + pictureBox_RGB2.Width / 2, pictureBox_Feature.Height);
        pictureBox_Feature.Refresh();

        GroupBox_Similarity.Size = new System.Drawing.Size(fw / 2, (fh * 20) / 100);

        // image 1 & 2
        try
            {
                ImageSimilarityStats();
            }
        catch (System.ArgumentException e)
            {
                string msg = String.Format("{0}: {1}", e.ParamName, e);
                string caption = "image update error";
                MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        this.GroupBox_Similarity.Refresh();
    }

    #endregion update form

    #endregion form events (size change, drag&drop)

    #region Calculate & update similarity stats

    #region image similarity stats

    /// <summary>
    /// Calculate imilarity stats when images are changed
    /// </summary>
    private void ImageSimilarityStats()
    {
        // update image similarity stats
        if (image1 != null && image2 != null)
            {
                if (image_changed)
                    {
                        UpdateImages();
                        image_changed = false;

                        // reduce image size for calculation
                        var smaller_image1 = CommonUtils.ImageUtils.Resize(image1, smaller_img_size.Width, smaller_img_size.Height, false);
                        var smaller_image2 = CommonUtils.ImageUtils.Resize(image2, smaller_img_size.Width, smaller_img_size.Height, false);

                        // compare dominant colors
                        var top_color1 = Images.ColorExtract.TopColors(image1);
                        var top_color1_img = (Image)Images.ColorExtract.Draw(top_color1, Images.ColorExtract.top_colors * 2, 4);

                        var top_color2 = Images.ColorExtract.TopColors(image2);
                        var top_color2_img = (Image)Images.ColorExtract.Draw(top_color2, Images.ColorExtract.top_colors * 2, 4);

                        var main_color1_img = Images.ColorExtract.Draw(top_color1, 1, 2);
                        var main_color2_img = Images.ColorExtract.Draw(top_color2, 1, 2);

                        double dcolor_similarity = Images.ImageSimilarity.CompareImages(top_color1_img, top_color2_img, method: ComparisonMethod.PixelsDifferenceSorted);

                        pictureBox_DominantColor1.Image = top_color1_img;
                        pictureBox_DominantColor1.Refresh();
                        pictureBox_Color1.Image = main_color1_img;
                        pictureBox_Color1.Refresh();

                        pictureBox_DominantColor2.Image = top_color2_img;
                        pictureBox_DominantColor2.Refresh();
                        pictureBox_Color2.Image = main_color2_img;
                        pictureBox_Color2.Refresh();

                        UpdateSimilarityLabel(labelDominantColors_similarity, dcolor_similarity);

                        // compare average hash
                        double avghash_similarity = Images.ImageHash.Compare(smaller_image1, smaller_image2, Images.ImageHashAlgorithm.Average);
                        UpdateSimilarityLabel(label_AvgHash_similarity, avghash_similarity);

                        // compare block hash
                        double blockhash_similarity = Images.ImageHash.Compare(image1, image2, Images.ImageHashAlgorithm.Block);
                        UpdateSimilarityLabel(label_BlockHash_similarity, blockhash_similarity);

                        // compare difference hash
                        double diffhash_similarity = Images.ImageHash.Compare(smaller_image1, smaller_image2, Images.ImageHashAlgorithm.Difference);
                        UpdateSimilarityLabel(label_DiffHash_similarity, diffhash_similarity);

                        // compare perceptual hash
                        double phash_similarity = Images.ImageHash.Compare(smaller_image1, smaller_image2, Images.ImageHashAlgorithm.Perceptive);
                        UpdateSimilarityLabel(label_PHash_similarity, phash_similarity);

                        // compare MD5 hash
                        double md5hash_similarity = Images.ImageHash.Compare(smaller_image1, smaller_image2, Images.ImageHashAlgorithm.MD5);
                        UpdateSimilarityLabel(label_MD5Hash_similarity, md5hash_similarity);

                        // RGB histograms

                        var rgb1_bmp = ImageHistogram.DrawRgbHistogramChart(smaller_image1);

                        pictureBox_RGB1.Image = CommonUtils.ImageUtils.Resize(rgb1_bmp, rgb_img_size);
                        pictureBox_RGB1.Refresh();

                        var rgb2_bmp = ImageHistogram.DrawRgbHistogramChart(smaller_image2);

                        pictureBox_RGB2.Image = CommonUtils.ImageUtils.Resize(rgb2_bmp, rgb_img_size);
                        pictureBox_RGB2.Refresh();

                        rgb1_bmp.Dispose();
                        rgb2_bmp.Dispose();

                        // RGB avg histogram
                        double rgb_avg_similarity = ImageHistogram.CompareAvgRgbHistogram(smaller_image1, smaller_image2);

                        UpdateSimilarityLabel(label_RGBHistogramAverage_similarity, rgb_avg_similarity);

                        // RGB histogram
                        double rgb_similarity = ImageHistogram.CompareRgbHistogram(smaller_image1, smaller_image2);
                        UpdateSimilarityLabel(label_RGBHistogram_similarity, rgb_similarity);

                        // RGB hash
                        double rgb_hash_similarity = Images.ImageHash.Compare(smaller_image1, smaller_image2, Images.ImageHashAlgorithm.Histogram); ;
                        UpdateSimilarityLabel(label_HistogramHash_similarity, rgb_hash_similarity);

                        // RGB pixels similarity
                        double rgb_pixels_similarity = ImageSimilarity.CompareImages(smaller_image1, smaller_image2, ComparisonMethod.PixelsDifference);
                        UpdateSimilarityLabel(label_DiffPixelsRgb_similarity, rgb_pixels_similarity);

                        double rgb_pixels_similarity_sorted = ImageSimilarity.CompareImages(smaller_image1, smaller_image2, ComparisonMethod.PixelsDifferenceSorted);
                        UpdateSimilarityLabel(label_DiffPixelsRgb_sorted_similarity, rgb_pixels_similarity_sorted);

                        double dist_rgb_pixels_similarity = ImageSimilarity.CompareImages(smaller_image1, smaller_image2, ComparisonMethod.PixelsDistance);
                        UpdateSimilarityLabel(label_DistPixelsRgb_similarity, dist_rgb_pixels_similarity);

                        double dist_rgb_pixels_similarity_sorted = ImageSimilarity.CompareImages(smaller_image1, smaller_image2, ComparisonMethod.PixelsDistanceSorted);
                        UpdateSimilarityLabel(label_DistPixelsRgb_sorted_similarity, dist_rgb_pixels_similarity_sorted);

                        // feature detection
                        double nfeature = 0;
                        double nmatch = 0;
                        Bitmap view;

                        double feature_similarity = ImageSimilarity.CompareFeatures(image1, image2, out nfeature, out nmatch, out view);
                        //pictureBox_Feature.Image = CommonUtils.ImageUtils.Resize(view, feature_img_size,true);
                        pictureBox_Feature.Image = CommonUtils.ImageUtils.Resize(view, feature_img_size.Width, feature_img_size.Height);
                        pictureBox_Feature.Refresh();
                        label_FeatureStats.Text = String.Format("{0} match", ((int)nmatch));
                        //label_FeatureStats.Text = ((int)nmatch).ToString();
                        label_FeatureStats.Refresh();
                        UpdateSimilarityLabel(this.label_Feature_similarity, feature_similarity);

                        // GC.Collect(); // force GC to free memory
                        // GC.WaitForPendingFinalizers();
                    }
            } // have 0 or 1 images
        else
            {
                //update images
                UpdateImages();
            }
    }

    #endregion image similarity stats

    #region update image

    /// <summary>
    /// Update picture box images
    /// </summary>
    private void UpdateImages()
    {
        int fh = this.Size.Height;
        int fw = this.Size.Width;

        pictureBox_image1.Size = new System.Drawing.Size(fw / 2, (fh * 50) / 100);
        pictureBox_image2.Size = new System.Drawing.Size(fw / 2, (fh * 50) / 100);

        if (image1 != null)
            {
                display_image1 = CommonUtils.ImageUtils.Resize(image1, pictureBox_image1.Size);
                pictureBox_image1.Image = display_image1;
            }
        if (image2 != null)
            {
                display_image2 = CommonUtils.ImageUtils.Resize(image2, pictureBox_image2.Size);
                pictureBox_image2.Image = display_image2;
            }

        pictureBox_image1.Refresh();
        pictureBox_image2.Refresh();
        GroupBox_Similarity.Refresh();
    }

    #endregion update image

    #region update similarity % labels

    /// <summary>
    /// Update similarity percentage label
    /// </summary>
    /// <param name="label"></param>
    /// <param name="similarity"></param>
    private void UpdateSimilarityLabel(Label label, double similarity)
    {
        String similar_string = String.Format("{0:00.0}", similarity);
        if (similarity >= 99.9)
            {
                label.ForeColor = color_same;
                label.Text = "same   ";
                //label.Text = similar_string + " %";
                label.Refresh();
            }
        else if (similarity == 0.0)
            {
                label.ForeColor = color_different;
                //label.Text = "different";
                label.Text = similar_string + " %";
                label.Refresh();
            }
        else if (similarity >= similarity_threshold)
            {
                label.ForeColor = color_similar;
                label.Text = similar_string + " %";
                label.Refresh();
            }
        else
            {
                label.ForeColor = color_different;
                label.Text = similar_string + " %";
                label.Refresh();
            }
    }

    #endregion update similarity % labels

    #endregion Calculate & update similarity stats

    #endregion form methods
}
}