namespace Compare2Images
{
partial class Compare2ImagesForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            {
                components.Dispose();
            }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.label_image1 = new System.Windows.Forms.Label();
        this.pictureBox_image2 = new System.Windows.Forms.PictureBox();
        this.pictureBox_image1 = new System.Windows.Forms.PictureBox();
        this.label_image2 = new System.Windows.Forms.Label();
        this.GroupBox_Similarity = new System.Windows.Forms.GroupBox();
        this.layout_similarity = new System.Windows.Forms.TableLayoutPanel();
        this.label_AvgHash = new System.Windows.Forms.Label();
        this.label_DistPixelsRgb_sorted_similarity = new System.Windows.Forms.Label();
        this.label_DistPixelsRgb_sorted = new System.Windows.Forms.Label();
        this.label_AvgHash_similarity = new System.Windows.Forms.Label();
        this.tableLayoutPanel_RGB_image = new System.Windows.Forms.TableLayoutPanel();
        this.pictureBox_RGB2 = new System.Windows.Forms.PictureBox();
        this.pictureBox_RGB1 = new System.Windows.Forms.PictureBox();
        this.tableLayoutPanel_Feature_similarity = new System.Windows.Forms.TableLayoutPanel();
        this.label_FeatureStats = new System.Windows.Forms.Label();
        this.label_Feature_similarity = new System.Windows.Forms.Label();
        this.pictureBox_Feature = new System.Windows.Forms.PictureBox();
        this.tableLayoutPanel_right_hist = new System.Windows.Forms.TableLayoutPanel();
        this.label_DistPixelsRgb = new System.Windows.Forms.Label();
        this.label_DiffPixelsRgb_sorted = new System.Windows.Forms.Label();
        this.label_DiffPixelsRgb = new System.Windows.Forms.Label();
        this.tableLayoutPanel_right_hist_similarity = new System.Windows.Forms.TableLayoutPanel();
        this.label_DiffPixelsRgb_similarity = new System.Windows.Forms.Label();
        this.label_DistPixelsRgb_similarity = new System.Windows.Forms.Label();
        this.label_DiffPixelsRgb_sorted_similarity = new System.Windows.Forms.Label();
        this.label_DiffHash_similarity = new System.Windows.Forms.Label();
        this.label_DiffHash = new System.Windows.Forms.Label();
        this.label_BlockHash = new System.Windows.Forms.Label();
        this.label_BlockHash_similarity = new System.Windows.Forms.Label();
        this.tableLayoutPanel_DominantColors = new System.Windows.Forms.TableLayoutPanel();
        this.labelDominantColors_similarity = new System.Windows.Forms.Label();
        this.tableLayoutPanel_DominantColorsImages = new System.Windows.Forms.TableLayoutPanel();
        this.pictureBox_DominantColor1 = new System.Windows.Forms.PictureBox();
        this.pictureBox_DominantColor2 = new System.Windows.Forms.PictureBox();
        this.pictureBox_Color1 = new System.Windows.Forms.PictureBox();
        this.pictureBox_Color2 = new System.Windows.Forms.PictureBox();
        this.label_DominantColors = new System.Windows.Forms.Label();
        this.tableLayoutPanel_small_images = new System.Windows.Forms.TableLayoutPanel();
        this.pictureBox_small2_gray = new System.Windows.Forms.PictureBox();
        this.pictureBox_small1_gray = new System.Windows.Forms.PictureBox();
        this.pictureBox_small2 = new System.Windows.Forms.PictureBox();
        this.pictureBox_small1 = new System.Windows.Forms.PictureBox();
        this.label_Feature = new System.Windows.Forms.Label();
        this.label_RGBHistogramAverage = new System.Windows.Forms.Label();
        this.label_RGBHistogramAverage_similarity = new System.Windows.Forms.Label();
        this.label_RGBHistogram_similarity = new System.Windows.Forms.Label();
        this.label_RGBHistogram = new System.Windows.Forms.Label();
        this.label_MD5Hash = new System.Windows.Forms.Label();
        this.label_MD5Hash_similarity = new System.Windows.Forms.Label();
        this.label_PHash = new System.Windows.Forms.Label();
        this.label_PHash_similarity = new System.Windows.Forms.Label();
        this.label_HistogramHash = new System.Windows.Forms.Label();
        this.label_HistogramHash_similarity = new System.Windows.Forms.Label();
        this.label_message = new System.Windows.Forms.Label();
        this.tableLayoutPanel_PictureBox = new System.Windows.Forms.TableLayoutPanel();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_image2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_image1)).BeginInit();
        this.GroupBox_Similarity.SuspendLayout();
        this.layout_similarity.SuspendLayout();
        this.tableLayoutPanel_RGB_image.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_RGB2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_RGB1)).BeginInit();
        this.tableLayoutPanel_Feature_similarity.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Feature)).BeginInit();
        this.tableLayoutPanel_right_hist.SuspendLayout();
        this.tableLayoutPanel_right_hist_similarity.SuspendLayout();
        this.tableLayoutPanel_DominantColors.SuspendLayout();
        this.tableLayoutPanel_DominantColorsImages.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_DominantColor1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_DominantColor2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Color1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Color2)).BeginInit();
        this.tableLayoutPanel_small_images.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_small2_gray)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_small1_gray)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_small2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_small1)).BeginInit();
        this.tableLayoutPanel_PictureBox.SuspendLayout();
        this.SuspendLayout();
        //
        // label_image1
        //
        this.label_image1.AutoSize = true;
        this.label_image1.BackColor = System.Drawing.SystemColors.ButtonFace;
        this.label_image1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label_image1.Location = new System.Drawing.Point(4, 0);
        this.label_image1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        this.label_image1.Name = "label_image1";
        this.label_image1.Size = new System.Drawing.Size(54, 21);
        this.label_image1.TabIndex = 3;
        this.label_image1.Text = "label1";
        //
        // pictureBox_image2
        //
        this.pictureBox_image2.AllowDrop = true;
        this.pictureBox_image2.BackColor = System.Drawing.SystemColors.ControlDark;
        this.pictureBox_image2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.pictureBox_image2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        this.pictureBox_image2.Cursor = System.Windows.Forms.Cursors.Arrow;
        this.pictureBox_image2.Dock = System.Windows.Forms.DockStyle.Top;
        this.pictureBox_image2.Location = new System.Drawing.Point(356, 26);
        this.pictureBox_image2.Margin = new System.Windows.Forms.Padding(5);
        this.pictureBox_image2.MinimumSize = new System.Drawing.Size(200, 100);
        this.pictureBox_image2.Name = "pictureBox_image2";
        this.pictureBox_image2.Size = new System.Drawing.Size(341, 121);
        this.pictureBox_image2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
        this.pictureBox_image2.TabIndex = 2;
        this.pictureBox_image2.TabStop = false;
        this.pictureBox_image2.DragDrop += new System.Windows.Forms.DragEventHandler(this.PictureBoxDragDrop);
        this.pictureBox_image2.DragEnter += new System.Windows.Forms.DragEventHandler(this.PictureBoxDragEnter);
        this.pictureBox_image2.DragLeave += new System.EventHandler(this.PictureBoxDragLeave);
        //
        // pictureBox_image1
        //
        this.pictureBox_image1.AllowDrop = true;
        this.pictureBox_image1.BackColor = System.Drawing.SystemColors.ControlDark;
        this.pictureBox_image1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.pictureBox_image1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        this.pictureBox_image1.Cursor = System.Windows.Forms.Cursors.Arrow;
        this.pictureBox_image1.Dock = System.Windows.Forms.DockStyle.Top;
        this.pictureBox_image1.Location = new System.Drawing.Point(5, 26);
        this.pictureBox_image1.Margin = new System.Windows.Forms.Padding(5);
        this.pictureBox_image1.MinimumSize = new System.Drawing.Size(200, 100);
        this.pictureBox_image1.Name = "pictureBox_image1";
        this.pictureBox_image1.Size = new System.Drawing.Size(341, 121);
        this.pictureBox_image1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
        this.pictureBox_image1.TabIndex = 1;
        this.pictureBox_image1.TabStop = false;
        this.pictureBox_image1.DragDrop += new System.Windows.Forms.DragEventHandler(this.PictureBoxDragDrop);
        this.pictureBox_image1.DragEnter += new System.Windows.Forms.DragEventHandler(this.PictureBoxDragEnter);
        this.pictureBox_image1.DragLeave += new System.EventHandler(this.PictureBoxDragLeave);
        //
        // label_image2
        //
        this.label_image2.AutoSize = true;
        this.label_image2.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label_image2.Location = new System.Drawing.Point(354, 0);
        this.label_image2.Name = "label_image2";
        this.label_image2.Size = new System.Drawing.Size(54, 21);
        this.label_image2.TabIndex = 4;
        this.label_image2.Text = "label2";
        //
        // GroupBox_Similarity
        //
        this.GroupBox_Similarity.AutoSize = true;
        this.GroupBox_Similarity.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.GroupBox_Similarity.Controls.Add(this.layout_similarity);
        this.GroupBox_Similarity.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.GroupBox_Similarity.Location = new System.Drawing.Point(0, 152);
        this.GroupBox_Similarity.Name = "GroupBox_Similarity";
        this.GroupBox_Similarity.Size = new System.Drawing.Size(702, 254);
        this.GroupBox_Similarity.TabIndex = 5;
        this.GroupBox_Similarity.TabStop = false;
        this.GroupBox_Similarity.Text = "Similarity";
        //
        // layout_similarity
        //
        this.layout_similarity.AutoSize = true;
        this.layout_similarity.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.layout_similarity.ColumnCount = 6;
        this.layout_similarity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.layout_similarity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.layout_similarity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.layout_similarity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.layout_similarity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.layout_similarity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 68F));
        this.layout_similarity.Controls.Add(this.label_AvgHash, 0, 0);
        this.layout_similarity.Controls.Add(this.label_DistPixelsRgb_sorted_similarity, 3, 7);
        this.layout_similarity.Controls.Add(this.label_DistPixelsRgb_sorted, 2, 7);
        this.layout_similarity.Controls.Add(this.label_AvgHash_similarity, 1, 0);
        this.layout_similarity.Controls.Add(this.tableLayoutPanel_RGB_image, 0, 6);
        this.layout_similarity.Controls.Add(this.tableLayoutPanel_Feature_similarity, 4, 7);
        this.layout_similarity.Controls.Add(this.pictureBox_Feature, 4, 6);
        this.layout_similarity.Controls.Add(this.tableLayoutPanel_right_hist, 2, 6);
        this.layout_similarity.Controls.Add(this.tableLayoutPanel_right_hist_similarity, 3, 6);
        this.layout_similarity.Controls.Add(this.label_DiffHash_similarity, 1, 2);
        this.layout_similarity.Controls.Add(this.label_DiffHash, 0, 2);
        this.layout_similarity.Controls.Add(this.label_BlockHash, 0, 1);
        this.layout_similarity.Controls.Add(this.label_BlockHash_similarity, 1, 1);
        this.layout_similarity.Controls.Add(this.tableLayoutPanel_DominantColors, 4, 1);
        this.layout_similarity.Controls.Add(this.label_DominantColors, 4, 0);
        this.layout_similarity.Controls.Add(this.tableLayoutPanel_small_images, 2, 0);
        this.layout_similarity.Controls.Add(this.label_Feature, 4, 3);
        this.layout_similarity.Controls.Add(this.label_RGBHistogramAverage, 2, 3);
        this.layout_similarity.Controls.Add(this.label_RGBHistogramAverage_similarity, 3, 3);
        this.layout_similarity.Controls.Add(this.label_RGBHistogram_similarity, 3, 2);
        this.layout_similarity.Controls.Add(this.label_RGBHistogram, 2, 2);
        this.layout_similarity.Controls.Add(this.label_MD5Hash, 2, 1);
        this.layout_similarity.Controls.Add(this.label_MD5Hash_similarity, 3, 1);
        this.layout_similarity.Controls.Add(this.label_PHash, 0, 7);
        this.layout_similarity.Controls.Add(this.label_PHash_similarity, 1, 7);
        this.layout_similarity.Controls.Add(this.label_HistogramHash, 0, 3);
        this.layout_similarity.Controls.Add(this.label_HistogramHash_similarity, 1, 3);
        this.layout_similarity.Controls.Add(this.label_message, 0, 8);
        this.layout_similarity.Dock = System.Windows.Forms.DockStyle.Fill;
        this.layout_similarity.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
        this.layout_similarity.Location = new System.Drawing.Point(3, 28);
        this.layout_similarity.Name = "layout_similarity";
        this.layout_similarity.Padding = new System.Windows.Forms.Padding(3);
        this.layout_similarity.RowCount = 9;
        this.layout_similarity.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.layout_similarity.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.layout_similarity.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.layout_similarity.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.layout_similarity.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.layout_similarity.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.layout_similarity.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.layout_similarity.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.layout_similarity.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        this.layout_similarity.Size = new System.Drawing.Size(696, 223);
        this.layout_similarity.TabIndex = 6;
        //
        // label_AvgHash
        //
        this.label_AvgHash.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_AvgHash.AutoSize = true;
        this.label_AvgHash.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label_AvgHash.Location = new System.Drawing.Point(6, 4);
        this.label_AvgHash.Name = "label_AvgHash";
        this.label_AvgHash.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_AvgHash.Size = new System.Drawing.Size(122, 21);
        this.label_AvgHash.TabIndex = 6;
        this.label_AvgHash.Text = "Average hash ";
        //
        // label_DistPixelsRgb_sorted_similarity
        //
        this.label_DistPixelsRgb_sorted_similarity.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_DistPixelsRgb_sorted_similarity.AutoSize = true;
        this.label_DistPixelsRgb_sorted_similarity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.label_DistPixelsRgb_sorted_similarity.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_DistPixelsRgb_sorted_similarity.Location = new System.Drawing.Point(400, 177);
        this.label_DistPixelsRgb_sorted_similarity.Name = "label_DistPixelsRgb_sorted_similarity";
        this.label_DistPixelsRgb_sorted_similarity.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_DistPixelsRgb_sorted_similarity.Size = new System.Drawing.Size(36, 23);
        this.label_DistPixelsRgb_sorted_similarity.TabIndex = 36;
        this.label_DistPixelsRgb_sorted_similarity.Text = "---";
        //
        // label_DistPixelsRgb_sorted
        //
        this.label_DistPixelsRgb_sorted.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_DistPixelsRgb_sorted.AutoSize = true;
        this.label_DistPixelsRgb_sorted.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_DistPixelsRgb_sorted.Location = new System.Drawing.Point(203, 178);
        this.label_DistPixelsRgb_sorted.Name = "label_DistPixelsRgb_sorted";
        this.label_DistPixelsRgb_sorted.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_DistPixelsRgb_sorted.Size = new System.Drawing.Size(191, 21);
        this.label_DistPixelsRgb_sorted.TabIndex = 35;
        this.label_DistPixelsRgb_sorted.Text = "Pixels distance (sorted)";
        //
        // label_AvgHash_similarity
        //
        this.label_AvgHash_similarity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.label_AvgHash_similarity.AutoSize = true;
        this.label_AvgHash_similarity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.label_AvgHash_similarity.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label_AvgHash_similarity.ForeColor = System.Drawing.Color.Black;
        this.label_AvgHash_similarity.Location = new System.Drawing.Point(145, 3);
        this.label_AvgHash_similarity.Name = "label_AvgHash_similarity";
        this.label_AvgHash_similarity.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_AvgHash_similarity.Size = new System.Drawing.Size(36, 23);
        this.label_AvgHash_similarity.TabIndex = 2;
        this.label_AvgHash_similarity.Text = "---";
        //
        // tableLayoutPanel_RGB_image
        //
        this.tableLayoutPanel_RGB_image.Anchor = System.Windows.Forms.AnchorStyles.Right;
        this.tableLayoutPanel_RGB_image.AutoSize = true;
        this.tableLayoutPanel_RGB_image.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.tableLayoutPanel_RGB_image.BackColor = System.Drawing.SystemColors.ControlDark;
        this.tableLayoutPanel_RGB_image.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
        this.tableLayoutPanel_RGB_image.ColumnCount = 2;
        this.layout_similarity.SetColumnSpan(this.tableLayoutPanel_RGB_image, 2);
        this.tableLayoutPanel_RGB_image.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel_RGB_image.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel_RGB_image.Controls.Add(this.pictureBox_RGB2, 1, 0);
        this.tableLayoutPanel_RGB_image.Controls.Add(this.pictureBox_RGB1, 0, 0);
        this.tableLayoutPanel_RGB_image.Location = new System.Drawing.Point(6, 98);
        this.tableLayoutPanel_RGB_image.Name = "tableLayoutPanel_RGB_image";
        this.tableLayoutPanel_RGB_image.RowCount = 1;
        this.tableLayoutPanel_RGB_image.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel_RGB_image.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel_RGB_image.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel_RGB_image.Size = new System.Drawing.Size(191, 76);
        this.tableLayoutPanel_RGB_image.TabIndex = 16;
        //
        // pictureBox_RGB2
        //
        this.pictureBox_RGB2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
        this.pictureBox_RGB2.Location = new System.Drawing.Point(96, 9);
        this.pictureBox_RGB2.Margin = new System.Windows.Forms.Padding(0);
        this.pictureBox_RGB2.Name = "pictureBox_RGB2";
        this.tableLayoutPanel_RGB_image.SetRowSpan(this.pictureBox_RGB2, 2);
        this.pictureBox_RGB2.Size = new System.Drawing.Size(94, 66);
        this.pictureBox_RGB2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
        this.pictureBox_RGB2.TabIndex = 1;
        this.pictureBox_RGB2.TabStop = false;
        //
        // pictureBox_RGB1
        //
        this.pictureBox_RGB1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
        this.pictureBox_RGB1.Location = new System.Drawing.Point(1, 1);
        this.pictureBox_RGB1.Margin = new System.Windows.Forms.Padding(0);
        this.pictureBox_RGB1.Name = "pictureBox_RGB1";
        this.tableLayoutPanel_RGB_image.SetRowSpan(this.pictureBox_RGB1, 2);
        this.pictureBox_RGB1.Size = new System.Drawing.Size(94, 74);
        this.pictureBox_RGB1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
        this.pictureBox_RGB1.TabIndex = 0;
        this.pictureBox_RGB1.TabStop = false;
        //
        // tableLayoutPanel_Feature_similarity
        //
        this.tableLayoutPanel_Feature_similarity.AutoSize = true;
        this.tableLayoutPanel_Feature_similarity.ColumnCount = 2;
        this.tableLayoutPanel_Feature_similarity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel_Feature_similarity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel_Feature_similarity.Controls.Add(this.label_FeatureStats, 0, 0);
        this.tableLayoutPanel_Feature_similarity.Controls.Add(this.label_Feature_similarity, 1, 0);
        this.tableLayoutPanel_Feature_similarity.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.tableLayoutPanel_Feature_similarity.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
        this.tableLayoutPanel_Feature_similarity.Location = new System.Drawing.Point(439, 177);
        this.tableLayoutPanel_Feature_similarity.Margin = new System.Windows.Forms.Padding(0);
        this.tableLayoutPanel_Feature_similarity.MinimumSize = new System.Drawing.Size(150, 23);
        this.tableLayoutPanel_Feature_similarity.Name = "tableLayoutPanel_Feature_similarity";
        this.tableLayoutPanel_Feature_similarity.RowCount = 1;
        this.tableLayoutPanel_Feature_similarity.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel_Feature_similarity.Size = new System.Drawing.Size(150, 23);
        this.tableLayoutPanel_Feature_similarity.TabIndex = 37;
        //
        // label_FeatureStats
        //
        this.label_FeatureStats.AutoSize = true;
        this.label_FeatureStats.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_FeatureStats.Location = new System.Drawing.Point(3, 0);
        this.label_FeatureStats.Name = "label_FeatureStats";
        this.label_FeatureStats.Size = new System.Drawing.Size(28, 21);
        this.label_FeatureStats.TabIndex = 0;
        this.label_FeatureStats.Text = "---";
        this.label_FeatureStats.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
        //
        // label_Feature_similarity
        //
        this.label_Feature_similarity.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_Feature_similarity.AutoSize = true;
        this.label_Feature_similarity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.label_Feature_similarity.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_Feature_similarity.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
        this.label_Feature_similarity.Location = new System.Drawing.Point(37, 0);
        this.label_Feature_similarity.Name = "label_Feature_similarity";
        this.label_Feature_similarity.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_Feature_similarity.Size = new System.Drawing.Size(36, 23);
        this.label_Feature_similarity.TabIndex = 1;
        this.label_Feature_similarity.Text = "---";
        //
        // pictureBox_Feature
        //
        this.pictureBox_Feature.BackColor = System.Drawing.SystemColors.ControlDark;
        this.pictureBox_Feature.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.pictureBox_Feature.Location = new System.Drawing.Point(442, 98);
        this.pictureBox_Feature.MinimumSize = new System.Drawing.Size(150, 60);
        this.pictureBox_Feature.Name = "pictureBox_Feature";
        this.pictureBox_Feature.Size = new System.Drawing.Size(188, 60);
        this.pictureBox_Feature.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.pictureBox_Feature.TabIndex = 39;
        this.pictureBox_Feature.TabStop = false;
        //
        // tableLayoutPanel_right_hist
        //
        this.tableLayoutPanel_right_hist.AutoSize = true;
        this.tableLayoutPanel_right_hist.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.tableLayoutPanel_right_hist.ColumnCount = 1;
        this.tableLayoutPanel_right_hist.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel_right_hist.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        this.tableLayoutPanel_right_hist.Controls.Add(this.label_DistPixelsRgb, 0, 2);
        this.tableLayoutPanel_right_hist.Controls.Add(this.label_DiffPixelsRgb_sorted, 0, 1);
        this.tableLayoutPanel_right_hist.Controls.Add(this.label_DiffPixelsRgb, 0, 0);
        this.tableLayoutPanel_right_hist.Location = new System.Drawing.Point(200, 95);
        this.tableLayoutPanel_right_hist.Margin = new System.Windows.Forms.Padding(0);
        this.tableLayoutPanel_right_hist.Name = "tableLayoutPanel_right_hist";
        this.tableLayoutPanel_right_hist.RowCount = 3;
        this.tableLayoutPanel_right_hist.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
        this.tableLayoutPanel_right_hist.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
        this.tableLayoutPanel_right_hist.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
        this.tableLayoutPanel_right_hist.Size = new System.Drawing.Size(131, 69);
        this.tableLayoutPanel_right_hist.TabIndex = 33;
        //
        // label_DistPixelsRgb
        //
        this.label_DistPixelsRgb.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_DistPixelsRgb.AutoSize = true;
        this.label_DistPixelsRgb.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_DistPixelsRgb.Location = new System.Drawing.Point(3, 47);
        this.label_DistPixelsRgb.Name = "label_DistPixelsRgb";
        this.label_DistPixelsRgb.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_DistPixelsRgb.Size = new System.Drawing.Size(125, 21);
        this.label_DistPixelsRgb.TabIndex = 30;
        this.label_DistPixelsRgb.Text = "Pixels distance";
        //
        // label_DiffPixelsRgb_sorted
        //
        this.label_DiffPixelsRgb_sorted.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_DiffPixelsRgb_sorted.AutoSize = true;
        this.label_DiffPixelsRgb_sorted.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_DiffPixelsRgb_sorted.Location = new System.Drawing.Point(3, 23);
        this.label_DiffPixelsRgb_sorted.MinimumSize = new System.Drawing.Size(124, 23);
        this.label_DiffPixelsRgb_sorted.Name = "label_DiffPixelsRgb_sorted";
        this.label_DiffPixelsRgb_sorted.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_DiffPixelsRgb_sorted.Size = new System.Drawing.Size(124, 23);
        this.label_DiffPixelsRgb_sorted.TabIndex = 29;
        this.label_DiffPixelsRgb_sorted.Text = "Pixels (sorted)";
        //
        // label_DiffPixelsRgb
        //
        this.label_DiffPixelsRgb.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_DiffPixelsRgb.AutoSize = true;
        this.label_DiffPixelsRgb.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_DiffPixelsRgb.Location = new System.Drawing.Point(3, 1);
        this.label_DiffPixelsRgb.Name = "label_DiffPixelsRgb";
        this.label_DiffPixelsRgb.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_DiffPixelsRgb.Size = new System.Drawing.Size(63, 21);
        this.label_DiffPixelsRgb.TabIndex = 27;
        this.label_DiffPixelsRgb.Text = "Pixels ";
        //
        // tableLayoutPanel_right_hist_similarity
        //
        this.tableLayoutPanel_right_hist_similarity.AutoSize = true;
        this.tableLayoutPanel_right_hist_similarity.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.tableLayoutPanel_right_hist_similarity.ColumnCount = 1;
        this.tableLayoutPanel_right_hist_similarity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel_right_hist_similarity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        this.tableLayoutPanel_right_hist_similarity.Controls.Add(this.label_DiffPixelsRgb_similarity, 0, 0);
        this.tableLayoutPanel_right_hist_similarity.Controls.Add(this.label_DistPixelsRgb_similarity, 0, 2);
        this.tableLayoutPanel_right_hist_similarity.Controls.Add(this.label_DiffPixelsRgb_sorted_similarity, 0, 1);
        this.tableLayoutPanel_right_hist_similarity.Location = new System.Drawing.Point(397, 95);
        this.tableLayoutPanel_right_hist_similarity.Margin = new System.Windows.Forms.Padding(0);
        this.tableLayoutPanel_right_hist_similarity.Name = "tableLayoutPanel_right_hist_similarity";
        this.tableLayoutPanel_right_hist_similarity.RowCount = 3;
        this.tableLayoutPanel_right_hist_similarity.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
        this.tableLayoutPanel_right_hist_similarity.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
        this.tableLayoutPanel_right_hist_similarity.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
        this.tableLayoutPanel_right_hist_similarity.Size = new System.Drawing.Size(42, 69);
        this.tableLayoutPanel_right_hist_similarity.TabIndex = 34;
        //
        // label_DiffPixelsRgb_similarity
        //
        this.label_DiffPixelsRgb_similarity.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_DiffPixelsRgb_similarity.AutoSize = true;
        this.label_DiffPixelsRgb_similarity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.label_DiffPixelsRgb_similarity.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_DiffPixelsRgb_similarity.Location = new System.Drawing.Point(3, 0);
        this.label_DiffPixelsRgb_similarity.Name = "label_DiffPixelsRgb_similarity";
        this.label_DiffPixelsRgb_similarity.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_DiffPixelsRgb_similarity.Size = new System.Drawing.Size(36, 23);
        this.label_DiffPixelsRgb_similarity.TabIndex = 28;
        this.label_DiffPixelsRgb_similarity.Text = "---";
        //
        // label_DistPixelsRgb_similarity
        //
        this.label_DistPixelsRgb_similarity.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_DistPixelsRgb_similarity.AutoSize = true;
        this.label_DistPixelsRgb_similarity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.label_DistPixelsRgb_similarity.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_DistPixelsRgb_similarity.Location = new System.Drawing.Point(3, 46);
        this.label_DistPixelsRgb_similarity.Name = "label_DistPixelsRgb_similarity";
        this.label_DistPixelsRgb_similarity.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_DistPixelsRgb_similarity.Size = new System.Drawing.Size(36, 23);
        this.label_DistPixelsRgb_similarity.TabIndex = 31;
        this.label_DistPixelsRgb_similarity.Text = "---";
        //
        // label_DiffPixelsRgb_sorted_similarity
        //
        this.label_DiffPixelsRgb_sorted_similarity.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_DiffPixelsRgb_sorted_similarity.AutoSize = true;
        this.label_DiffPixelsRgb_sorted_similarity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.label_DiffPixelsRgb_sorted_similarity.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_DiffPixelsRgb_sorted_similarity.Location = new System.Drawing.Point(3, 23);
        this.label_DiffPixelsRgb_sorted_similarity.MinimumSize = new System.Drawing.Size(36, 23);
        this.label_DiffPixelsRgb_sorted_similarity.Name = "label_DiffPixelsRgb_sorted_similarity";
        this.label_DiffPixelsRgb_sorted_similarity.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_DiffPixelsRgb_sorted_similarity.Size = new System.Drawing.Size(36, 23);
        this.label_DiffPixelsRgb_sorted_similarity.TabIndex = 30;
        this.label_DiffPixelsRgb_sorted_similarity.Text = "---";
        //
        // label_DiffHash_similarity
        //
        this.label_DiffHash_similarity.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_DiffHash_similarity.AutoSize = true;
        this.label_DiffHash_similarity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.label_DiffHash_similarity.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_DiffHash_similarity.Location = new System.Drawing.Point(145, 49);
        this.label_DiffHash_similarity.Name = "label_DiffHash_similarity";
        this.label_DiffHash_similarity.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_DiffHash_similarity.Size = new System.Drawing.Size(36, 23);
        this.label_DiffHash_similarity.TabIndex = 18;
        this.label_DiffHash_similarity.Text = "---";
        //
        // label_DiffHash
        //
        this.label_DiffHash.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_DiffHash.AutoSize = true;
        this.label_DiffHash.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_DiffHash.Location = new System.Drawing.Point(6, 50);
        this.label_DiffHash.Name = "label_DiffHash";
        this.label_DiffHash.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_DiffHash.Size = new System.Drawing.Size(132, 21);
        this.label_DiffHash.TabIndex = 17;
        this.label_DiffHash.Text = "Difference hash";
        //
        // label_BlockHash
        //
        this.label_BlockHash.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_BlockHash.AutoSize = true;
        this.label_BlockHash.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_BlockHash.Location = new System.Drawing.Point(6, 27);
        this.label_BlockHash.Name = "label_BlockHash";
        this.label_BlockHash.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_BlockHash.Size = new System.Drawing.Size(95, 21);
        this.label_BlockHash.TabIndex = 40;
        this.label_BlockHash.Text = "Block hash";
        //
        // label_BlockHash_similarity
        //
        this.label_BlockHash_similarity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.label_BlockHash_similarity.AutoSize = true;
        this.label_BlockHash_similarity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.label_BlockHash_similarity.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_BlockHash_similarity.Location = new System.Drawing.Point(145, 26);
        this.label_BlockHash_similarity.Name = "label_BlockHash_similarity";
        this.label_BlockHash_similarity.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_BlockHash_similarity.Size = new System.Drawing.Size(36, 23);
        this.label_BlockHash_similarity.TabIndex = 41;
        this.label_BlockHash_similarity.Text = "---";
        //
        // tableLayoutPanel_DominantColors
        //
        this.tableLayoutPanel_DominantColors.AutoSize = true;
        this.tableLayoutPanel_DominantColors.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.tableLayoutPanel_DominantColors.ColumnCount = 2;
        this.tableLayoutPanel_DominantColors.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel_DominantColors.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel_DominantColors.Controls.Add(this.labelDominantColors_similarity, 1, 0);
        this.tableLayoutPanel_DominantColors.Controls.Add(this.tableLayoutPanel_DominantColorsImages, 0, 0);
        this.tableLayoutPanel_DominantColors.Dock = System.Windows.Forms.DockStyle.Top;
        this.tableLayoutPanel_DominantColors.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
        this.tableLayoutPanel_DominantColors.Location = new System.Drawing.Point(439, 26);
        this.tableLayoutPanel_DominantColors.Margin = new System.Windows.Forms.Padding(0);
        this.tableLayoutPanel_DominantColors.MinimumSize = new System.Drawing.Size(150, 20);
        this.tableLayoutPanel_DominantColors.Name = "tableLayoutPanel_DominantColors";
        this.tableLayoutPanel_DominantColors.RowCount = 1;
        this.tableLayoutPanel_DominantColors.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel_DominantColors.Size = new System.Drawing.Size(194, 23);
        this.tableLayoutPanel_DominantColors.TabIndex = 44;
        //
        // labelDominantColors_similarity
        //
        this.labelDominantColors_similarity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.labelDominantColors_similarity.AutoSize = true;
        this.labelDominantColors_similarity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.labelDominantColors_similarity.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.labelDominantColors_similarity.Location = new System.Drawing.Point(88, 0);
        this.labelDominantColors_similarity.Margin = new System.Windows.Forms.Padding(0);
        this.labelDominantColors_similarity.Name = "labelDominantColors_similarity";
        this.labelDominantColors_similarity.Size = new System.Drawing.Size(30, 23);
        this.labelDominantColors_similarity.TabIndex = 45;
        this.labelDominantColors_similarity.Text = "---";
        //
        // tableLayoutPanel_DominantColorsImages
        //
        this.tableLayoutPanel_DominantColorsImages.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
        this.tableLayoutPanel_DominantColorsImages.AutoSize = true;
        this.tableLayoutPanel_DominantColorsImages.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.tableLayoutPanel_DominantColorsImages.ColumnCount = 2;
        this.tableLayoutPanel_DominantColorsImages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.31F));
        this.tableLayoutPanel_DominantColorsImages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 89.69F));
        this.tableLayoutPanel_DominantColorsImages.Controls.Add(this.pictureBox_DominantColor1, 1, 0);
        this.tableLayoutPanel_DominantColorsImages.Controls.Add(this.pictureBox_DominantColor2, 1, 1);
        this.tableLayoutPanel_DominantColorsImages.Controls.Add(this.pictureBox_Color1, 0, 0);
        this.tableLayoutPanel_DominantColorsImages.Controls.Add(this.pictureBox_Color2, 0, 1);
        this.tableLayoutPanel_DominantColorsImages.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
        this.tableLayoutPanel_DominantColorsImages.Location = new System.Drawing.Point(0, 1);
        this.tableLayoutPanel_DominantColorsImages.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
        this.tableLayoutPanel_DominantColorsImages.MinimumSize = new System.Drawing.Size(75, 18);
        this.tableLayoutPanel_DominantColorsImages.Name = "tableLayoutPanel_DominantColorsImages";
        this.tableLayoutPanel_DominantColorsImages.RowCount = 2;
        this.tableLayoutPanel_DominantColorsImages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.tableLayoutPanel_DominantColorsImages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.tableLayoutPanel_DominantColorsImages.Size = new System.Drawing.Size(84, 22);
        this.tableLayoutPanel_DominantColorsImages.TabIndex = 1;
        //
        // pictureBox_DominantColor1
        //
        this.pictureBox_DominantColor1.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.pictureBox_DominantColor1.BackColor = System.Drawing.SystemColors.ControlDark;
        this.pictureBox_DominantColor1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.pictureBox_DominantColor1.Location = new System.Drawing.Point(8, 0);
        this.pictureBox_DominantColor1.Margin = new System.Windows.Forms.Padding(0);
        this.pictureBox_DominantColor1.MinimumSize = new System.Drawing.Size(75, 11);
        this.pictureBox_DominantColor1.Name = "pictureBox_DominantColor1";
        this.pictureBox_DominantColor1.Size = new System.Drawing.Size(75, 11);
        this.pictureBox_DominantColor1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.pictureBox_DominantColor1.TabIndex = 0;
        this.pictureBox_DominantColor1.TabStop = false;
        //
        // pictureBox_DominantColor2
        //
        this.pictureBox_DominantColor2.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.pictureBox_DominantColor2.BackColor = System.Drawing.SystemColors.ControlDark;
        this.pictureBox_DominantColor2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.pictureBox_DominantColor2.Location = new System.Drawing.Point(8, 11);
        this.pictureBox_DominantColor2.Margin = new System.Windows.Forms.Padding(0);
        this.pictureBox_DominantColor2.MinimumSize = new System.Drawing.Size(75, 11);
        this.pictureBox_DominantColor2.Name = "pictureBox_DominantColor2";
        this.pictureBox_DominantColor2.Size = new System.Drawing.Size(75, 11);
        this.pictureBox_DominantColor2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.pictureBox_DominantColor2.TabIndex = 1;
        this.pictureBox_DominantColor2.TabStop = false;
        //
        // pictureBox_Color1
        //
        this.pictureBox_Color1.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.pictureBox_Color1.BackColor = System.Drawing.SystemColors.ControlDark;
        this.pictureBox_Color1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.pictureBox_Color1.Location = new System.Drawing.Point(0, 0);
        this.pictureBox_Color1.Margin = new System.Windows.Forms.Padding(0);
        this.pictureBox_Color1.MinimumSize = new System.Drawing.Size(5, 5);
        this.pictureBox_Color1.Name = "pictureBox_Color1";
        this.pictureBox_Color1.Size = new System.Drawing.Size(5, 11);
        this.pictureBox_Color1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.pictureBox_Color1.TabIndex = 2;
        this.pictureBox_Color1.TabStop = false;
        //
        // pictureBox_Color2
        //
        this.pictureBox_Color2.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.pictureBox_Color2.BackColor = System.Drawing.SystemColors.ControlDark;
        this.pictureBox_Color2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.pictureBox_Color2.Location = new System.Drawing.Point(0, 11);
        this.pictureBox_Color2.Margin = new System.Windows.Forms.Padding(0);
        this.pictureBox_Color2.MinimumSize = new System.Drawing.Size(5, 5);
        this.pictureBox_Color2.Name = "pictureBox_Color2";
        this.pictureBox_Color2.Size = new System.Drawing.Size(5, 11);
        this.pictureBox_Color2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.pictureBox_Color2.TabIndex = 3;
        this.pictureBox_Color2.TabStop = false;
        //
        // label_DominantColors
        //
        this.label_DominantColors.AutoSize = true;
        this.label_DominantColors.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_DominantColors.Location = new System.Drawing.Point(439, 3);
        this.label_DominantColors.Margin = new System.Windows.Forms.Padding(0);
        this.label_DominantColors.Name = "label_DominantColors";
        this.label_DominantColors.Size = new System.Drawing.Size(56, 21);
        this.label_DominantColors.TabIndex = 0;
        this.label_DominantColors.Text = "Colors";
        //
        // tableLayoutPanel_small_images
        //
        this.tableLayoutPanel_small_images.Anchor = System.Windows.Forms.AnchorStyles.Top;
        this.tableLayoutPanel_small_images.AutoSize = true;
        this.tableLayoutPanel_small_images.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.tableLayoutPanel_small_images.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
        this.tableLayoutPanel_small_images.ColumnCount = 4;
        this.tableLayoutPanel_small_images.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel_small_images.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel_small_images.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel_small_images.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel_small_images.Controls.Add(this.pictureBox_small2_gray, 3, 0);
        this.tableLayoutPanel_small_images.Controls.Add(this.pictureBox_small1_gray, 1, 0);
        this.tableLayoutPanel_small_images.Controls.Add(this.pictureBox_small2, 2, 0);
        this.tableLayoutPanel_small_images.Controls.Add(this.pictureBox_small1, 0, 0);
        this.tableLayoutPanel_small_images.Location = new System.Drawing.Point(264, 3);
        this.tableLayoutPanel_small_images.Margin = new System.Windows.Forms.Padding(0);
        this.tableLayoutPanel_small_images.MinimumSize = new System.Drawing.Size(69, 16);
        this.tableLayoutPanel_small_images.Name = "tableLayoutPanel_small_images";
        this.tableLayoutPanel_small_images.RowCount = 1;
        this.tableLayoutPanel_small_images.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel_small_images.Size = new System.Drawing.Size(69, 18);
        this.tableLayoutPanel_small_images.TabIndex = 26;
        //
        // pictureBox_small2_gray
        //
        this.pictureBox_small2_gray.Location = new System.Drawing.Point(52, 1);
        this.pictureBox_small2_gray.Margin = new System.Windows.Forms.Padding(0);
        this.pictureBox_small2_gray.MinimumSize = new System.Drawing.Size(16, 16);
        this.pictureBox_small2_gray.Name = "pictureBox_small2_gray";
        this.pictureBox_small2_gray.Size = new System.Drawing.Size(16, 16);
        this.pictureBox_small2_gray.TabIndex = 25;
        this.pictureBox_small2_gray.TabStop = false;
        //
        // pictureBox_small1_gray
        //
        this.pictureBox_small1_gray.Location = new System.Drawing.Point(18, 1);
        this.pictureBox_small1_gray.Margin = new System.Windows.Forms.Padding(0);
        this.pictureBox_small1_gray.MinimumSize = new System.Drawing.Size(16, 16);
        this.pictureBox_small1_gray.Name = "pictureBox_small1_gray";
        this.pictureBox_small1_gray.Size = new System.Drawing.Size(16, 16);
        this.pictureBox_small1_gray.TabIndex = 24;
        this.pictureBox_small1_gray.TabStop = false;
        //
        // pictureBox_small2
        //
        this.pictureBox_small2.Location = new System.Drawing.Point(35, 1);
        this.pictureBox_small2.Margin = new System.Windows.Forms.Padding(0);
        this.pictureBox_small2.MinimumSize = new System.Drawing.Size(16, 16);
        this.pictureBox_small2.Name = "pictureBox_small2";
        this.pictureBox_small2.Size = new System.Drawing.Size(16, 16);
        this.pictureBox_small2.TabIndex = 26;
        this.pictureBox_small2.TabStop = false;
        //
        // pictureBox_small1
        //
        this.pictureBox_small1.Location = new System.Drawing.Point(1, 1);
        this.pictureBox_small1.Margin = new System.Windows.Forms.Padding(0);
        this.pictureBox_small1.MinimumSize = new System.Drawing.Size(16, 16);
        this.pictureBox_small1.Name = "pictureBox_small1";
        this.pictureBox_small1.Size = new System.Drawing.Size(16, 16);
        this.pictureBox_small1.TabIndex = 27;
        this.pictureBox_small1.TabStop = false;
        //
        // label_Feature
        //
        this.label_Feature.AutoSize = true;
        this.label_Feature.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_Feature.Location = new System.Drawing.Point(439, 72);
        this.label_Feature.Margin = new System.Windows.Forms.Padding(0);
        this.label_Feature.Name = "label_Feature";
        this.label_Feature.Size = new System.Drawing.Size(144, 21);
        this.label_Feature.TabIndex = 38;
        this.label_Feature.Text = "Feature Detection";
        //
        // label_RGBHistogramAverage
        //
        this.label_RGBHistogramAverage.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_RGBHistogramAverage.AutoSize = true;
        this.label_RGBHistogramAverage.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_RGBHistogramAverage.Location = new System.Drawing.Point(203, 73);
        this.label_RGBHistogramAverage.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
        this.label_RGBHistogramAverage.Name = "label_RGBHistogramAverage";
        this.label_RGBHistogramAverage.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_RGBHistogramAverage.Size = new System.Drawing.Size(170, 21);
        this.label_RGBHistogramAverage.TabIndex = 22;
        this.label_RGBHistogramAverage.Text = "Histogram (average)";
        //
        // label_RGBHistogramAverage_similarity
        //
        this.label_RGBHistogramAverage_similarity.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_RGBHistogramAverage_similarity.AutoSize = true;
        this.label_RGBHistogramAverage_similarity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.label_RGBHistogramAverage_similarity.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_RGBHistogramAverage_similarity.Location = new System.Drawing.Point(400, 72);
        this.label_RGBHistogramAverage_similarity.Name = "label_RGBHistogramAverage_similarity";
        this.label_RGBHistogramAverage_similarity.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_RGBHistogramAverage_similarity.Size = new System.Drawing.Size(36, 23);
        this.label_RGBHistogramAverage_similarity.TabIndex = 23;
        this.label_RGBHistogramAverage_similarity.Text = "---";
        //
        // label_RGBHistogram_similarity
        //
        this.label_RGBHistogram_similarity.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_RGBHistogram_similarity.AutoSize = true;
        this.label_RGBHistogram_similarity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.label_RGBHistogram_similarity.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_RGBHistogram_similarity.Location = new System.Drawing.Point(400, 49);
        this.label_RGBHistogram_similarity.Name = "label_RGBHistogram_similarity";
        this.label_RGBHistogram_similarity.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_RGBHistogram_similarity.Size = new System.Drawing.Size(36, 23);
        this.label_RGBHistogram_similarity.TabIndex = 31;
        this.label_RGBHistogram_similarity.Text = "---";
        //
        // label_RGBHistogram
        //
        this.label_RGBHistogram.AutoSize = true;
        this.label_RGBHistogram.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_RGBHistogram.Location = new System.Drawing.Point(203, 49);
        this.label_RGBHistogram.Name = "label_RGBHistogram";
        this.label_RGBHistogram.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_RGBHistogram.Size = new System.Drawing.Size(92, 21);
        this.label_RGBHistogram.TabIndex = 32;
        this.label_RGBHistogram.Text = "Histogram";
        //
        // label_MD5Hash
        //
        this.label_MD5Hash.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_MD5Hash.AutoSize = true;
        this.label_MD5Hash.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_MD5Hash.Location = new System.Drawing.Point(203, 27);
        this.label_MD5Hash.Name = "label_MD5Hash";
        this.label_MD5Hash.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_MD5Hash.Size = new System.Drawing.Size(95, 21);
        this.label_MD5Hash.TabIndex = 5;
        this.label_MD5Hash.Text = "MD5 hash ";
        //
        // label_MD5Hash_similarity
        //
        this.label_MD5Hash_similarity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.label_MD5Hash_similarity.AutoSize = true;
        this.label_MD5Hash_similarity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.label_MD5Hash_similarity.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_MD5Hash_similarity.ForeColor = System.Drawing.Color.DarkCyan;
        this.label_MD5Hash_similarity.Location = new System.Drawing.Point(400, 26);
        this.label_MD5Hash_similarity.Name = "label_MD5Hash_similarity";
        this.label_MD5Hash_similarity.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_MD5Hash_similarity.Size = new System.Drawing.Size(36, 23);
        this.label_MD5Hash_similarity.TabIndex = 10;
        this.label_MD5Hash_similarity.Text = "---";
        //
        // label_PHash
        //
        this.label_PHash.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_PHash.AutoSize = true;
        this.label_PHash.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_PHash.Location = new System.Drawing.Point(6, 178);
        this.label_PHash.Name = "label_PHash";
        this.label_PHash.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_PHash.Size = new System.Drawing.Size(133, 21);
        this.label_PHash.TabIndex = 20;
        this.label_PHash.Text = "Perceptive hash";
        //
        // label_PHash_similarity
        //
        this.label_PHash_similarity.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_PHash_similarity.AutoSize = true;
        this.label_PHash_similarity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.label_PHash_similarity.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_PHash_similarity.Location = new System.Drawing.Point(145, 177);
        this.label_PHash_similarity.Name = "label_PHash_similarity";
        this.label_PHash_similarity.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_PHash_similarity.Size = new System.Drawing.Size(36, 23);
        this.label_PHash_similarity.TabIndex = 21;
        this.label_PHash_similarity.Text = "---";
        //
        // label_HistogramHash
        //
        this.label_HistogramHash.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_HistogramHash.AutoSize = true;
        this.label_HistogramHash.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_HistogramHash.Location = new System.Drawing.Point(6, 73);
        this.label_HistogramHash.Name = "label_HistogramHash";
        this.label_HistogramHash.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_HistogramHash.Size = new System.Drawing.Size(132, 21);
        this.label_HistogramHash.TabIndex = 42;
        this.label_HistogramHash.Text = "Histogram hash";
        //
        // label_HistogramHash_similarity
        //
        this.label_HistogramHash_similarity.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.label_HistogramHash_similarity.AutoSize = true;
        this.label_HistogramHash_similarity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.label_HistogramHash_similarity.Font = new System.Drawing.Font("Tahoma", 10.2F);
        this.label_HistogramHash_similarity.Location = new System.Drawing.Point(145, 72);
        this.label_HistogramHash_similarity.Name = "label_HistogramHash_similarity";
        this.label_HistogramHash_similarity.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
        this.label_HistogramHash_similarity.Size = new System.Drawing.Size(36, 23);
        this.label_HistogramHash_similarity.TabIndex = 43;
        this.label_HistogramHash_similarity.Text = "---";
        //
        // label_message
        //
        this.label_message.AutoSize = true;
        this.layout_similarity.SetColumnSpan(this.label_message, 5);
        this.label_message.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label_message.ForeColor = System.Drawing.SystemColors.HotTrack;
        this.label_message.Location = new System.Drawing.Point(6, 200);
        this.label_message.Name = "label_message";
        this.label_message.Size = new System.Drawing.Size(93, 18);
        this.label_message.TabIndex = 45;
        this.label_message.Text = "                 ";
        //
        // tableLayoutPanel_PictureBox
        //
        this.tableLayoutPanel_PictureBox.AutoSize = true;
        this.tableLayoutPanel_PictureBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.tableLayoutPanel_PictureBox.ColumnCount = 2;
        this.tableLayoutPanel_PictureBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.tableLayoutPanel_PictureBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.tableLayoutPanel_PictureBox.Controls.Add(this.label_image1, 0, 0);
        this.tableLayoutPanel_PictureBox.Controls.Add(this.label_image2, 1, 0);
        this.tableLayoutPanel_PictureBox.Controls.Add(this.pictureBox_image1, 0, 1);
        this.tableLayoutPanel_PictureBox.Controls.Add(this.pictureBox_image2, 1, 1);
        this.tableLayoutPanel_PictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tableLayoutPanel_PictureBox.Location = new System.Drawing.Point(0, 0);
        this.tableLayoutPanel_PictureBox.Name = "tableLayoutPanel_PictureBox";
        this.tableLayoutPanel_PictureBox.RowCount = 2;
        this.tableLayoutPanel_PictureBox.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel_PictureBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel_PictureBox.Size = new System.Drawing.Size(702, 152);
        this.tableLayoutPanel_PictureBox.TabIndex = 6;
        //
        // Compare2ImagesForm
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.ClientSize = new System.Drawing.Size(702, 406);
        this.Controls.Add(this.tableLayoutPanel_PictureBox);
        this.Controls.Add(this.GroupBox_Similarity);
        this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
        this.MinimumSize = new System.Drawing.Size(680, 450);
        this.Name = "Compare2ImagesForm";
        this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
        this.Text = "Compare2Images";
        this.Load += new System.EventHandler(this.LoadForm);
        this.SizeChanged += new System.EventHandler(this.LoadForm);
        this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormDragEnter);
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_image2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_image1)).EndInit();
        this.GroupBox_Similarity.ResumeLayout(false);
        this.GroupBox_Similarity.PerformLayout();
        this.layout_similarity.ResumeLayout(false);
        this.layout_similarity.PerformLayout();
        this.tableLayoutPanel_RGB_image.ResumeLayout(false);
        this.tableLayoutPanel_RGB_image.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_RGB2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_RGB1)).EndInit();
        this.tableLayoutPanel_Feature_similarity.ResumeLayout(false);
        this.tableLayoutPanel_Feature_similarity.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Feature)).EndInit();
        this.tableLayoutPanel_right_hist.ResumeLayout(false);
        this.tableLayoutPanel_right_hist.PerformLayout();
        this.tableLayoutPanel_right_hist_similarity.ResumeLayout(false);
        this.tableLayoutPanel_right_hist_similarity.PerformLayout();
        this.tableLayoutPanel_DominantColors.ResumeLayout(false);
        this.tableLayoutPanel_DominantColors.PerformLayout();
        this.tableLayoutPanel_DominantColorsImages.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_DominantColor1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_DominantColor2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Color1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Color2)).EndInit();
        this.tableLayoutPanel_small_images.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_small2_gray)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_small1_gray)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_small2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_small1)).EndInit();
        this.tableLayoutPanel_PictureBox.ResumeLayout(false);
        this.tableLayoutPanel_PictureBox.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBox_image1;
    private System.Windows.Forms.PictureBox pictureBox_image2;
    private System.Windows.Forms.Label label_image1;
    private System.Windows.Forms.Label label_image2;
    private System.Windows.Forms.GroupBox GroupBox_Similarity;
    private System.Windows.Forms.TableLayoutPanel layout_similarity;
    private System.Windows.Forms.Label label_AvgHash_similarity;
    private System.Windows.Forms.Label label_MD5Hash;
    private System.Windows.Forms.Label label_AvgHash;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_PictureBox;
    private System.Windows.Forms.Label label_MD5Hash_similarity;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_RGB_image;
    private System.Windows.Forms.PictureBox pictureBox_RGB1;
    private System.Windows.Forms.PictureBox pictureBox_RGB2;
    private System.Windows.Forms.Label label_DiffHash;
    private System.Windows.Forms.Label label_DiffHash_similarity;
    private System.Windows.Forms.Label label_PHash;
    private System.Windows.Forms.Label label_PHash_similarity;
    private System.Windows.Forms.Label label_RGBHistogramAverage;
    private System.Windows.Forms.Label label_RGBHistogramAverage_similarity;
    private System.Windows.Forms.PictureBox pictureBox_small1_gray;
    private System.Windows.Forms.PictureBox pictureBox_small2_gray;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_small_images;
    private System.Windows.Forms.PictureBox pictureBox_small2;
    private System.Windows.Forms.PictureBox pictureBox_small1;
    private System.Windows.Forms.Label label_DiffPixelsRgb;
    private System.Windows.Forms.Label label_DiffPixelsRgb_similarity;
    private System.Windows.Forms.Label label_DiffPixelsRgb_sorted;
    private System.Windows.Forms.Label label_DiffPixelsRgb_sorted_similarity;
    private System.Windows.Forms.Label label_RGBHistogram_similarity;
    private System.Windows.Forms.Label label_RGBHistogram;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_right_hist;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_right_hist_similarity;
    private System.Windows.Forms.Label label_DistPixelsRgb;
    private System.Windows.Forms.Label label_DistPixelsRgb_similarity;
    private System.Windows.Forms.Label label_DistPixelsRgb_sorted;
    private System.Windows.Forms.Label label_DistPixelsRgb_sorted_similarity;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Feature_similarity;
    private System.Windows.Forms.Label label_FeatureStats;
    private System.Windows.Forms.Label label_Feature_similarity;
    private System.Windows.Forms.Label label_Feature;
    private System.Windows.Forms.PictureBox pictureBox_Feature;
    private System.Windows.Forms.Label label_BlockHash;
    private System.Windows.Forms.Label label_BlockHash_similarity;
    private System.Windows.Forms.Label label_HistogramHash;
    private System.Windows.Forms.Label label_HistogramHash_similarity;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_DominantColors;
    private System.Windows.Forms.Label labelDominantColors_similarity;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_DominantColorsImages;
    private System.Windows.Forms.PictureBox pictureBox_DominantColor1;
    private System.Windows.Forms.PictureBox pictureBox_DominantColor2;
    private System.Windows.Forms.Label label_DominantColors;
    private System.Windows.Forms.PictureBox pictureBox_Color1;
    private System.Windows.Forms.PictureBox pictureBox_Color2;
    private System.Windows.Forms.Label label_message;

}
}

