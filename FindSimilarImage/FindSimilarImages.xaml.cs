// Find Similar Images UI
//
// Copyright (C) David Laperriere

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FindSimilarImages
{
/// <summary>
/// Interaction logic for FindSimilarImages.xaml
/// </summary>
public partial class MainWindow : Window
{
    #region UI data

    /// <summary>
    /// Image index
    /// </summary>
    //private Images.ImageIndex images_index = new Images.ImageIndex();
    private Images.ImageIndexMulti images_index = new Images.ImageIndexMulti();

    /// <summary>
    /// Search parameters
    /// </summary>
    private string image_of_interest = String.Empty;

    private Images.ImageHashAlgorithm option_image_hash = Images.ImageHashAlgorithm.Average;
    private Images.ComparisonMethod option_filter_colors = Images.ComparisonMethod.TopColors;
    private int option_similarity_cutoff = 70;

    /// <summary>
    /// similar images found
    /// </summary>
    private List<Images.DataBinding.SimilarImage> similar_images = new List<Images.DataBinding.SimilarImage>();

    private ObservableCollection<Images.DataBinding.SimilarImage> info_images_found = new ObservableCollection<Images.DataBinding.SimilarImage>();

    /// <summary>
    /// max. # images displayed
    /// </summary>
    private int max_images_displayed = 25;

    private enum ResultPage { First = 1, Next = 2, Previous = 3, Last = 4 };

    private int current_page = 1;

    /// <summary>
    /// Search task progress
    /// </summary>
    private Progress<string> progressManager = new Progress<string>();

    private CancellationTokenSource cancellationTokenSource;
    private CancellationToken cancellationToken;

    #endregion UI data

    #region Constructor

    public MainWindow()
    {
        InitializeComponent();

        Style = (Style)FindResource(typeof(Window));
        /*
        Timeline.DesiredFrameRateProperty.OverrideMetadata(
            typeof(Timeline),
            new FrameworkPropertyMetadata { DefaultValue = 30 }
           );
        */
        this.SimilarImagesList.ItemsSource = info_images_found;
        progressManager.ProgressChanged += UpdateSearchProgress;
    }

    #endregion Constructor

    #region Keyboard shortcuts

    /// <summary>
    ///  Keyboard shortcuts
    ///  - Alt + F = Set image folder
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_KeyboardShortcut_folder(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
        ImageFolderDialog();
    }

    /// <summary>
    ///  Keyboard shortcuts
    ///  - Alt + I = Set image
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void KeyboardShortcut_Image(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
        ImageDialog();
    }

    /// <summary>
    ///  Keyboard shortcuts
    ///  - Alt + S | Enter = Search similar images
    //
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void KeyboardShortcut_Search(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
        SearchSimilar();
    }

    /// <summary>
    ///  Keyboard shortcuts
    ///  - Ctrl + S = Save similar images
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void KeyboardShortcut_Save(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
        SaveImages();
    }

    /// <summary>
    ///  Keyboard shortcuts
    ///   - Alt + Right Arrow = Next page of results
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void KeyboardShortcut_Next(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
        ShowResultPage((int)ResultPage.Next);
    }

    /// <summary>
    ///  Keyboard shortcuts
    ///   - Alt + Left Arrow = Previous page of results
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void KeyboardShortcut_Previous(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
        ShowResultPage((int)ResultPage.Previous);
    }

    #endregion Keyboard shortcuts

    #region Image of interest

    /// <summary>
    /// Set image of interest with Drag & Drop
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ImageDrop(object sender, DragEventArgs e)
    {
        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
        if (files.Length < 1)
            {
                return;
            }
        ImageSourceConverter converter = new ImageSourceConverter();

        var dropped_file = files[0];
        if (Path.GetExtension(dropped_file).Equals(".ico"))
            {
                return;
            }

        if (converter.IsValid(dropped_file))
            {
                image_of_interest = dropped_file;
                var dropped_image_folder = Path.GetDirectoryName(dropped_file);

                if (this.SearchFolder.Text.Equals(String.Empty))
                    {
                        this.SearchFolder.Text = dropped_image_folder;
                    }

                UpdateImage(dropped_file);
            }
    }

    /// <summary>
    /// Set image of interest after mouse click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ImageClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.ClickCount >= 2)
            {
                ImageDialog();
            }
    }

    /// <summary>
    /// Set image of interest with open file dialog
    /// </summary>
    private void ImageDialog()
    {
        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
        dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        dlg.DefaultExt = ".jpeg";
        dlg.Filter = "Image files |*.bmp;*.gif;*.jpg;*.jpeg;*.png;*.tif|BMP (*.bmp)|*.bmp|GIF (*.gif)|*.gif|JPEG (*.jpeg)|*.jpeg;*.jpg|PNG (*.png)|*.png|TIFF (*.tif)|*.tif";

        var ok = dlg.ShowDialog();

        if (ok == true)
            {
                string selected_filename = dlg.FileName;
                image_of_interest = selected_filename;
                try
                    {
                        UpdateImage(image_of_interest);

                        if (this.SearchFolder.Text.Equals(String.Empty))
                            {
                                this.SearchFolder.Text = Path.GetDirectoryName(selected_filename);
                            }
                    }
                catch (Exception) { }
            }
    }

    /// <summary>
    /// Update image of interest
    /// </summary>
    /// <param name="image_path"></param>
    private void UpdateImage(string image_path)
    {
        var image = System.Drawing.Image.FromFile(image_path, true);

        var max_size = Math.Min(this.Width / 3, this.Height / 3);
        this.SearchImage.Height = max_size;

        BitmapImage imagebitmap = new BitmapImage();
        imagebitmap.BeginInit();
        imagebitmap.DecodePixelWidth = 300;
        imagebitmap.UriSource = new Uri(image_path);
        imagebitmap.EndInit();

        this.SearchImage.Source = imagebitmap;

        var top_color = Images.ColorExtract.TopColors(image);
        var top_color_img = (System.Drawing.Image)Images.ColorExtract.Draw(top_color, 100, 10);
        this.ImageColors.Source = ToImageSource(top_color_img);

        var rgb_hist_bmp = Images.ImageHistogram.DrawRgbHistogramChart(image);
        var rgb_hist_img = (System.Drawing.Image)rgb_hist_bmp;
        this.ImageHistogram.Source = ToImageSource(rgb_hist_img);

        info_images_found.Clear();
        similar_images.Clear();
        current_page = 1;
        this.LResultsPageNumber.Content = "  ";

        this.SBStatusText.Content = String.Empty;
        this.SearchImage.ToolTip = image_of_interest;

        image.Dispose();
        top_color_img.Dispose();
        rgb_hist_bmp.Dispose();
        rgb_hist_img.Dispose();
    }

    /// <summary>
    ///  Convert System.Drawing.Image to BitmapImage
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    private BitmapImage ToImageSource(System.Drawing.Image image)
    {
        BitmapImage bi = new BitmapImage();
        MemoryStream ms = new MemoryStream();

        image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        ms.Position = 0;

        bi.BeginInit();
        bi.StreamSource = ms;
        bi.EndInit();

        return bi;
    }

    #endregion Image of interest

    #region Image folder

    /// <summary>
    /// Set Image Folder
    /// </summary>
    private void ImageFolderDialog()
    {
        using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
        {
            dlg.Description = "Select image folder";
            dlg.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (!this.SearchFolder.Text.Equals(String.Empty))
                {
                    dlg.SelectedPath = this.SearchFolder.Text;
                }

            dlg.ShowNewFolderButton = false;
            var result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                {
                    this.SearchFolder.Text = dlg.SelectedPath;
                }
        }
    }

    /// <summary>
    /// Set Image Folder (Button click)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ImageFolderClick(object sender, RoutedEventArgs e)
    {
        ImageFolderDialog();
    }

    #endregion Image folder

    #region Search options

    /// <summary>
    /// Set search options from the UI (method, filter and similarity cutoff)
    /// </summary>
    private void ReadSearchOptions()
    {
        // method

        switch (this.CBMethod.SelectedIndex)
            {
            case 0:
                option_image_hash = Images.ImageHashAlgorithm.Average;
                break;

            case 1:
                option_image_hash = Images.ImageHashAlgorithm.AverageHistogram;
                break;

            case 2:
                option_image_hash = Images.ImageHashAlgorithm.AverageColor;
                break;

            case 3:
                option_image_hash = Images.ImageHashAlgorithm.Block;
                break;

            case 4:
                option_image_hash = Images.ImageHashAlgorithm.BlockHistogram;
                break;

            case 5:
                option_image_hash = Images.ImageHashAlgorithm.BlockColor;
                break;

            case 6:
                option_image_hash = Images.ImageHashAlgorithm.Color;
                break;

            case 7:
                option_image_hash = Images.ImageHashAlgorithm.Difference;
                break;

            case 8:
                option_image_hash = Images.ImageHashAlgorithm.DifferenceHistogram;
                break;

            case 9:
                option_image_hash = Images.ImageHashAlgorithm.DifferenceColor;
                break;

            case 10:
                option_image_hash = Images.ImageHashAlgorithm.Histogram;
                break;

            case 11:
                option_image_hash = Images.ImageHashAlgorithm.Perceptive;
                break;

            case 12:
                option_image_hash = Images.ImageHashAlgorithm.PerceptiveHistogram;
                break;

            case 13:
                option_image_hash = Images.ImageHashAlgorithm.PerceptiveColor;
                break;

            default:
                option_image_hash = Images.ImageHashAlgorithm.PerceptiveHistogram;
                break;
            }

        // filter

        switch (this.CBFilter.SelectedIndex)
            {
            case 0:
                option_filter_colors = Images.ComparisonMethod.None;
                break;

            case 1:
                option_filter_colors = Images.ComparisonMethod.MainColor;
                break;

            case 2:
                option_filter_colors = Images.ComparisonMethod.TopColors;
                break;

            case 3:
                option_filter_colors = Images.ComparisonMethod.PixelsDifference;
                break;

            case 4:
                option_filter_colors = Images.ComparisonMethod.PixelsDifferenceSorted;
                break;

            case 5:
                option_filter_colors = Images.ComparisonMethod.PixelsDistance;
                break;

            case 6:
                option_filter_colors = Images.ComparisonMethod.PixelsDistanceSorted;
                break;

            default:
                option_filter_colors = Images.ComparisonMethod.TopColors;
                break;
            }
        // similarity

        switch (this.CBSimilarity.SelectedIndex)
            {
            case 0:
                option_similarity_cutoff = 65;
                break;

            case 1:
                option_similarity_cutoff = 70;
                break;

            case 2:
                option_similarity_cutoff = 75;
                break;

            case 3:
                option_similarity_cutoff = 80;
                break;

            case 4:
                option_similarity_cutoff = 85;
                break;

            case 5:
                option_similarity_cutoff = 90;
                break;

            case 6:
                option_similarity_cutoff = 95;
                break;

            case 7:
                option_similarity_cutoff = 100;
                break;

            default:
                option_similarity_cutoff = 90;
                break;
            }
    }

    #endregion Search options

    #region Search Similar Images

    /// <summary>
    /// Search similar images (Button click)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SearchSimilar(object sender, RoutedEventArgs e)
    {
        SearchSimilar();
    }

    /// <summary>
    /// Search similar images
    /// </summary>
    private void SearchSimilar()
    {
        if (!this.SearchFolder.Text.Equals(String.Empty) && !image_of_interest.Equals(String.Empty))
            {
                if (Directory.Exists(this.SearchFolder.Text))
                    {
                        // re-create index
                        if ( this.CheckRecreateIndex.IsChecked == true)
                            {

                                var index_files = Directory.EnumerateFiles(this.SearchFolder.Text, "image*.idx", SearchOption.TopDirectoryOnly).ToList();
                                foreach (var index_file in index_files)
                                    {
                                        File.Delete(index_file);
                                        var fs = File.Create(index_file);
                                        fs.Close();
                                        Thread.Sleep(20);
                                    }
                                images_index = new Images.ImageIndexMulti();
                                this.CheckRecreateIndex.IsChecked = false;
                            }

                        info_images_found.Clear();
                        similar_images.Clear();
                        current_page = 1;
                        this.LResultsPageNumber.Content = "  ";

                        try
                            {
                                ReadSearchOptions();
                                RunSearchSimilarTask();

                                GC.Collect(); // force GC to free memory
                                GC.WaitForPendingFinalizers();
                            }
                        catch (Exception e)
                            {
                                this.SBStatusText.Content = String.Format("Error: {0} ", e.Message);
                            }
                    }
            }
    }

    /// <summary>
    /// Search similar images & display results
    /// </summary>
    private async void RunSearchSimilarTask()
    {
        string folder = this.SearchFolder.Text;

        cancellationTokenSource = new CancellationTokenSource();
        cancellationToken = cancellationTokenSource.Token;

        object[] task_parameters = new object[] { folder, image_of_interest };

        using (Task<Dictionary<string, int>> task = new Task<Dictionary<string, int>>(new Func<object, Dictionary<string, int>>(SearchSimilarTask), task_parameters, cancellationToken))
        {
            this.SBStatusText.Content = "Indexing images";

            this.BSearch.IsEnabled = false;
            this.BSave.IsEnabled = false;

            SBProgressBar.Width = 100;
            SBProgressBar.Height = 12;
            SBProgressBar.Visibility = Visibility.Visible;

            task.Start();
            await task;

            if (!task.IsFaulted)
                {
                    this.SBStatusText.Content = "Processing results";

                    var found = task.Result;

                    info_images_found.Clear();
                    similar_images.Clear();

                    int count = 0;
                    var similar_sorted = found.OrderByDescending(i => i.Value).ToDictionary(i => i.Key, i => i.Value).Keys.ToList();
                    foreach (var image in similar_sorted)
                        {
                            if (image.Equals(image_of_interest))
                                {
                                    continue;
                                }
                            var image_index_info = images_index.ImageInfo(image);

                            var colors_b64 = image_index_info[3];
                            var colors_bytes = Convert.FromBase64String(colors_b64);
                            var colors_img = System.Drawing.Image.FromStream(new MemoryStream(colors_bytes));
                            colors_img = (System.Drawing.Image)CommonUtils.ImageUtils.Resize(colors_img, 80, 10);

                            Images.DataBinding.SimilarImage rec = new Images.DataBinding.SimilarImage(image, colors_img, found[image]);

                            if (File.Exists(image))
                                {
                                    count++;
                                    similar_images.Add(rec);

                                    if (count % 20 == 0)
                                        {
                                            this.SBStatusText.Content = "Processing results";
                                            Thread.Sleep(5);
                                        }

                                    if (count <= max_images_displayed)
                                        {
                                            info_images_found.Add(rec);
                                        }
                                }
                        }
                    found.Clear();

                    //this.SBStatusText.Content = String.Format("Found {0} [{1} {2} {3}]", count,option_image_hash, option_filter_colors, option_similarity_cutoff);
                    this.SBStatusText.Content = String.Format("Found {0} ", count);

                    if (count >= 1)
                        {
                            this.BSave.IsEnabled = true;
                        }

                    ShowResultPage((int)ResultPage.First);
                    SBProgressBar.Visibility = Visibility.Hidden;
                }

            this.BSearch.IsEnabled = true;
        }
    }

    /// <summary>
    /// Index image folder and search for similar images
    /// </summary>
    /// <param name="task_parameters"></param>
    /// <returns>Similar images : image -> similarity % </returns>
    private Dictionary<string, int> SearchSimilarTask(object task_parameters)
    {
        object[] parameters = (object[])task_parameters;
        var folder = (string)parameters[0];

        var similar_images = new Dictionary<string, int>();

        if (!cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(20);
                ((IProgress<string>)progressManager).Report("Indexing");
                images_index.IndexImageDirectory(folder);
                Thread.Sleep(20);

                ((IProgress<string>)progressManager).Report("Searching");
                similar_images = images_index.SearchSimilarImages(image_of_interest, option_image_hash, option_filter_colors, option_similarity_cutoff);

                ((IProgress<string>)progressManager).Report("Processing matches");
                Thread.Sleep(20);
            }

        return similar_images;
    }

    /// <summary>
    /// Update status bar progress
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="progress"></param>
    private void UpdateSearchProgress(object sender, string progress)
    {
        this.SBStatusText.Content = progress;
    }

    /// <summary>
    /// Cancel search (not implemented)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CancelSearchSimilar(object sender, RoutedEventArgs e)
    {
        cancellationTokenSource.Cancel();
    }

    #endregion Search Similar Images

    #region Similar images context menu

    /// <summary>
    ///  Open similar image context menu
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="menu_event"></param>
    private void SimilarContextMenu_Open(object sender, ContextMenuEventArgs menu_event)
    {
        FrameworkElement fe = menu_event.Source as FrameworkElement;
        ContextMenu cm = fe.ContextMenu;
        while (cm == null)
            {
                fe = (FrameworkElement)fe.Parent;
                if (fe == null) break;
                cm = fe.ContextMenu;
            }
    }

    /// <summary>
    ///  Open similar image with default viewer
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="click_event"></param>
    private void SimilarMenuOpenImage_Click(object sender, RoutedEventArgs click_event)
    {
        string selectedImage = string.Empty;
        MenuItem selectedMenuItem = sender as MenuItem;
        Border selectedBorder = null;
        if (selectedMenuItem != null)
            {
                selectedBorder = ((ContextMenu)selectedMenuItem.Parent).PlacementTarget as Border;
                selectedImage = (string)selectedBorder.Tag;
            }

        if (File.Exists(selectedImage))
            {
                System.Diagnostics.Process.Start(selectedImage);
            }
    }

    /// <summary>
    /// Set similar image as search image
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="click_event"></param>
    private void SimilarMenuSetAsSearchImage_Click(object sender, RoutedEventArgs click_event)
    {
        string selectedImage = string.Empty;
        MenuItem selectedMenuItem = sender as MenuItem;
        Border selectedBorder = null;
        if (selectedMenuItem != null)
            {
                selectedBorder = ((ContextMenu)selectedMenuItem.Parent).PlacementTarget as Border;
                selectedImage = (string)selectedBorder.Tag;
            }

        if (File.Exists(selectedImage))
            {
                this.image_of_interest = selectedImage;
                try
                    {
                        UpdateImage(image_of_interest);
                        this.LResultsPageNumber.Content = "  ";
                    }
                catch (Exception) { }
            }
    }

    #endregion Similar images context menu

    #region Similar image results paging

    /// <summary>
    /// First page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BResultsFirst_Click(object sender, System.EventArgs e)
    {
        ShowResultPage((int)ResultPage.First);
    }

    /// <summary>
    /// Next page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BResultsNext_Click(object sender, System.EventArgs e)
    {
        ShowResultPage((int)ResultPage.Next);
    }

    /// <summary>
    /// Previous page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BResultsPrev_Click(object sender, System.EventArgs e)
    {
        ShowResultPage((int)ResultPage.Previous);
    }

    /// <summary>
    /// Last page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BResultsLast_Click(object sender, System.EventArgs e)
    {
        ShowResultPage((int)ResultPage.Last);
    }

    /// <summary>
    /// Show similar images found
    /// </summary>
    /// <param name="page"> First = 1, Next = 2, Previous = 3, Last = 4</param>
    private void ShowResultPage(int page)
    {
        int num_results = similar_images.Count;
        int results_per_page = max_images_displayed;

        int num_pages = num_results / results_per_page;
        if (num_results % results_per_page > 0)
            {
                num_pages++;
            }

        // 1 page is enough for all records
        if (num_results <= results_per_page)
            {
                num_pages = 1;
                current_page = 1;
                this.LResultsPageNumber.Content = String.Format(" {0}/{1} ", current_page, num_pages);
                return;
            }

        // make sure page is in valid range
        // First = 1, Next = 2, Previous = 3, Last = 4
        if (page < 1)
            {
                page = 1;
            }
        if (page > 4)
            {
                page = 4;
            }

        switch (page)
            {
            case (int)ResultPage.Next:
                if (num_results > (current_page * results_per_page))
                    {
                        if (num_results >= ((current_page * results_per_page) + results_per_page))
                            {
                                info_images_found.Clear();
                                for (int i = current_page * results_per_page; i < ((current_page * results_per_page) + results_per_page); i++)
                                    {
                                        info_images_found.Add(similar_images[i]);
                                    }
                            }
                        else
                            {
                                info_images_found.Clear();
                                for (int i = current_page * results_per_page; i < num_results; i++)
                                    {
                                        info_images_found.Add(similar_images[i]);
                                    }
                            }
                        current_page += 1;
                    }
                break;

            case (int)ResultPage.Previous:
                if (current_page > 1)
                    {
                        current_page -= 1;

                        info_images_found.Clear();

                        for (int i = ((current_page * results_per_page) - results_per_page); i < (current_page * results_per_page); i++)
                            {
                                info_images_found.Add(similar_images[i]);
                            }
                    }
                break;

            case (int)ResultPage.First:
                current_page = 2;
                ShowResultPage((int)ResultPage.Previous);
                break;

            case (int)ResultPage.Last:
                current_page = (num_results / results_per_page);
                ShowResultPage((int)ResultPage.Next);
                break;
            }

        // make sure current page is in valid range
        if (current_page < 1)
            {
                current_page = 1;
            }
        if (current_page > num_pages)
            {
                current_page = num_pages;
            }

        this.LResultsPageNumber.Content = String.Format(" {0}/{1} ", current_page, num_pages);
    }

    #endregion Similar image results paging

    #region Save similar images

    /// <summary>
    /// Save a copy of similar images to a folder (Mouse Click/KB shortcut)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SaveImages(object sender, RoutedEventArgs e)
    {
        SaveImages();
    }

    /// <summary>
    /// Save a copy of similar images to a folder
    /// </summary>
    private void SaveImages()
    {
        if (similar_images.Count < 1)
            {
                return;
            }

        using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
        {
            dlg.Description = "Select folder where the images will be saved";
            dlg.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            dlg.ShowNewFolderButton = true;
            var result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                {
                    //save image of interest
                    var image_name = Path.GetFileName(image_of_interest);
                    var image_path = image_of_interest;
                    var copy = Path.Combine(dlg.SelectedPath, image_name);

                    if (File.Exists(image_path) && !File.Exists(copy))
                        {
                            File.Copy(image_path, copy);
                        }

                    //save similar images
                    foreach (var image in similar_images)
                        {
                            if (image == null)
                                {
                                    continue;
                                }
                            image_name = image.ImageName;
                            image_path = image.ImagePath;
                            copy = Path.Combine(dlg.SelectedPath, image_name);
                            if (File.Exists(image_path) && !File.Exists(copy))
                                {
                                    File.Copy(image_path, copy);
                                }
                        }
                }
        }
    }

    #endregion Save similar images
} //class
} //namespace