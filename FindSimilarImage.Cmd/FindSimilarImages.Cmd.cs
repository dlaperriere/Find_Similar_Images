// Find Similar Images
// - find images similar to an image of interest in a directory
// - find groups of similar images in a directory
//
// Copyright (C) David Laperriere

using Mono.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FindSimilarImages
{
/// <summary>
/// Command line version of FindSimilarImages
///
/// Usage
/// </summary>
internal class FindSimilarImagesCmd
{
    private static string version = "1.0";

    #region Search parameters

    /// <summary>
    /// Search parameters
    /// </summary>

    private class Parameters
    {
        public string image_of_interest;
        public string image_directory = String.Empty;
        public string search_mode = "?";

        public Images.ImageHashAlgorithm image_hash_algorithm = Images.ImageHashAlgorithm.BlockHistogram;
        public Images.ComparisonMethod color_filter = Images.ComparisonMethod.MainColor;
        public int min_similarity = 80;
    }

    #endregion Search parameters

    #region Usage

    /// <summary>
    /// Show program parameters and usage information
    /// </summary>
    public static void Usage()
    {
        Usage(message: String.Empty, verbose: true);
    }

    /// <summary>
    /// Show program parameters and usage information
    /// </summary>
    /// <param name="message">optional error message</param>
    /// <param name="verbose">show parameters description</param>
    public static void Usage(string message = "", bool verbose = true)
    {
        int exit_code = 0;
        if (message.Length > 0)
            {
                System.Console.WriteLine("{0}\n", message);
                exit_code = -1;
            }
        System.Console.WriteLine("Find Similar Images v{0}\n", version);
        System.Console.WriteLine("Usage");
        System.Console.WriteLine("-----");
        System.Console.WriteLine(" Find images similar to an image of interest in a directory\n");
        System.Console.WriteLine("   FindSimilarImages.Cmd  -dir name -image name -algo block  -filter colors  -similarity 80\n");
        System.Console.WriteLine(" Find groups of similar images in a directory\n");
        System.Console.WriteLine("   FindSimilarImages.Cmd  -dir name -algo difference  -filter colors  -similarity 80\n");
        System.Console.WriteLine(" Index images in a directory\n");
        System.Console.WriteLine("   FindSimilarImages.Cmd  -index dir_name\n");
        System.Console.WriteLine(" Show program parameters and usage information\n");
        System.Console.WriteLine("   FindSimilarImages.Cmd  -help\n");
        if (verbose)
            {
                System.Console.WriteLine("Parameters");
                System.Console.WriteLine("----------");
                System.Console.WriteLine("image directory");
                System.Console.WriteLine("  -dir name\n");
                System.Console.WriteLine("image of interest");
                System.Console.WriteLine("  -image name\n");
                System.Console.WriteLine("perceptual hash algorithm ");
                System.Console.WriteLine("  -algo average|block|difference|perceptive    ");
                System.Console.WriteLine("  -algo caverage|cblock|cdifference|cperceptive   (hash + main color filter)");
                System.Console.WriteLine("  -algo haverage|hblock|hdifference|hperceptive   (hash + RGB histogram filter)\n");
                System.Console.WriteLine("color filter");
                System.Console.WriteLine("  -filter color          main color ");
                System.Console.WriteLine("  -filter colors         dominant colors (up to 20)");
                System.Console.WriteLine("  -filter difference     compare pixels RGB values");
                System.Console.WriteLine("  -filter sdifference    compare pixels sorted by luminance");
                System.Console.WriteLine("  -filter distance       compare pixels RGB distance");
                System.Console.WriteLine("  -filter sdistance      compare pixels sorted by luminance\n");
                System.Console.WriteLine("minimum similarity %");
                System.Console.WriteLine("  -similarity percentage (70 = 70%)\n");
            }

        System.Environment.Exit(exit_code);
    }

    #endregion Usage

    #region Read Command Line Parameters

    /// <summary>
    /// Read search parameters from the command line
    /// </summary>
    /// <param name="args"></param>
    private static Parameters ReadCommandLineParameters(string[] args)
    {
        var search_parameters = new Parameters();

        string algo = String.Empty;
        string color_filter = String.Empty;
        string min_similarity = String.Empty;
        string index_directory = String.Empty;
        search_parameters.image_directory = String.Empty;
        search_parameters.image_of_interest = String.Empty;

        var p = new OptionSet()
        {
            { "h|?|help", v => Usage () },
            { "i=|image=", v => search_parameters.image_of_interest = v },
            { "d=|dir=|directory=", v => search_parameters.image_directory = v },
            { "index=", v => index_directory = v },
            { "a=|algo=|algorithm=", v => algo = v.ToLower() },
            { "f=|filter=", v => color_filter = v.ToLower() },
            { "s=|sim=|similarity=", v => min_similarity = v },
            { "v|version", v => Usage (String.Empty,false) },
        };
        try
            {
                p.Parse(args);
            }
        catch (Mono.Options.OptionException oe)
            {
                Usage(message: oe.Message, verbose: false);
            }

        // Check parameters for errors

        //* -dir
        if (!index_directory.Equals(String.Empty))
            {
                search_parameters.image_directory = index_directory;
            }

        if (search_parameters.image_directory.Length == 0)
            {
                Usage("must provide an image directory (-dir name or -index name)...");
            }

        if (search_parameters.image_directory.Equals(@".\") || search_parameters.image_directory.Equals(@"./"))
            {
                search_parameters.image_directory = Directory.GetCurrentDirectory();
            }

        if (!Directory.Exists(search_parameters.image_directory))
            {
                string message = String.Format("image directory \"{0}\" does not exist (-dir or -index)...", search_parameters.image_directory);
                Usage(message, verbose: false);
            }

        //* -image
        if (Directory.Exists(search_parameters.image_of_interest))
            {
                string message = String.Format("image name \"{0}\" is a directory (-image)...", search_parameters.image_of_interest);
                Usage(message, verbose: false);
            }

        if (!File.Exists(search_parameters.image_of_interest))
            {
                search_parameters.image_of_interest = " ";
                search_parameters.search_mode = String.Format("find groups of similar images in \'{0}\'", search_parameters.image_directory);
            }
        else
            {
                search_parameters.search_mode = String.Format("find images similar to \'{0}\' in \'{1}\'", search_parameters.image_of_interest, search_parameters.image_directory);
            }

        // -index dir
        if (!index_directory.Equals(String.Empty))
            {
                search_parameters.search_mode = "index";
                return search_parameters;
            }

        //* -algo
        Dictionary<string, Images.ImageHashAlgorithm> algos = new Dictionary<string, Images.ImageHashAlgorithm>
        {
            {"average",Images.ImageHashAlgorithm.Average},
            {"block",Images.ImageHashAlgorithm.Block},
            {"color",Images.ImageHashAlgorithm.Color},
            {"histogram",Images.ImageHashAlgorithm.Histogram},
            {"difference",Images.ImageHashAlgorithm.Difference},
            {"perceptive",Images.ImageHashAlgorithm.Perceptive},

            {"haverage",Images.ImageHashAlgorithm.AverageHistogram},
            {"hblock",Images.ImageHashAlgorithm.BlockHistogram},
            {"hdifference",Images.ImageHashAlgorithm.DifferenceHistogram},
            {"hperceptive",Images.ImageHashAlgorithm.PerceptiveHistogram},

            {"caverage",Images.ImageHashAlgorithm.AverageColor},
            {"cblock",Images.ImageHashAlgorithm.BlockColor},
            {"cdifference",Images.ImageHashAlgorithm.DifferenceColor},
            {"cperceptive",Images.ImageHashAlgorithm.PerceptiveColor}
        };

        if (algos.ContainsKey(algo))
            {
                search_parameters.image_hash_algorithm = algos[algo];
            }
        else
            {
                string message = String.Format("unknown perceptual algorithm name \"{0}\" (-algo)...", algo);
                Usage(message, verbose: true);
            }

        //* -filter
        Dictionary<string, Images.ComparisonMethod> color_filters = new Dictionary<string, Images.ComparisonMethod>
        {
            {String.Empty,Images.ComparisonMethod.None},
            {"none",Images.ComparisonMethod.None},
            {"difference",Images.ComparisonMethod.PixelsDifference},
            {"sdifference",Images.ComparisonMethod.PixelsDifferenceSorted},
            {"distance",Images.ComparisonMethod.PixelsDistance},
            {"sdistance",Images.ComparisonMethod.PixelsDistanceSorted},
            {"colors",Images.ComparisonMethod.TopColors},
            {"color",Images.ComparisonMethod.MainColor},
            {"features",Images.ComparisonMethod.Feature},
            {"feature",Images.ComparisonMethod.Feature}
        };

        if (color_filters.ContainsKey(color_filter))
            {
                search_parameters.color_filter = color_filters[color_filter];
            }
        else
            {
                string message = String.Format("unknown color filter method \"{0}\" (-filter)...", color_filter);
                Usage(message, verbose: true);
            }

        //* -similarity

        if (min_similarity.Length == 0)
            {
                Usage("must provide similarity cutoff (--similarity percent)...");
            }

        int similarity = 0;
        try
            {
                similarity = Convert.ToInt32(min_similarity);
            }
        catch (Exception e)
            {
                Console.WriteLine("Could not convert {0} to a number (-similarity):\n{1} ", min_similarity, e.Message);
            }
        if (similarity <= 0 || similarity > 100)
            {
                string message = String.Format("min. similarity \"{0}\" must be between ]0-100] (-similarity)...", similarity);
                Usage(message, verbose: false);
            }
        else
            {
                search_parameters.min_similarity = similarity;
            }

        return search_parameters;
    }

    #endregion Read Command Line Parameters

    #region Main

    /// <summary>
    /// Find images similar to an image of interest in a directory
    /// FindSimilarImages.Cmd  -dir name -image name -algo block  -filter colors  -similarity 80
    ///
    /// Find groups of similar images in a directory
    /// FindSimilarImages.Cmd  -dir name -algo block  -filter colors  -similarity 80
    /// </summary>
    /// <param name="args"></param>
    [STAThread]
    public static void Main(string[] args)
    {
        var parameters = ReadCommandLineParameters(args);

        Stopwatch stopWatch = new Stopwatch();

        Console.WriteLine("Find Similar Images");
        Console.WriteLine("-------------------");
        Console.WriteLine("  image directory : {0}", parameters.image_directory);
        Console.WriteLine("image of interest : {0}", parameters.image_of_interest);
        Console.WriteLine("      search mode : {0}", parameters.search_mode);
        if (!parameters.search_mode.Equals("index"))
            {
                Console.WriteLine("       image hash : {0}", parameters.image_hash_algorithm.ToString());
                Console.WriteLine(" pixel comparison : {0}", parameters.color_filter.ToString());
                Console.WriteLine("       similarity : {0}%", parameters.min_similarity);
            }
        Console.WriteLine("-------------------");

        // Index images files in directory
        ////////////////////////////////////////////////

        stopWatch.Reset();
        stopWatch.Start();

        //var images_index = new Images.ImageIndex(parameters.image_directory);
        var images_index = new Images.ImageIndexMulti();
        images_index.IndexImageDirectory(parameters.image_directory);
        Console.WriteLine("");

        stopWatch.Stop();
        TimeSpan ts_index = stopWatch.Elapsed;
        PrintElapsedTime(ts_index);

        var image_files = images_index.ImageFilesIndexed();
        Console.WriteLine("* {1} images in directory \"{0}\"", parameters.image_directory, image_files.Count().ToString());

        if (parameters.search_mode.Equals("index"))
            {
                System.Environment.Exit(0);
            }

        // find similar images and export as html page
        ////////////////////////////////////////////////

        Console.WriteLine("* Search for similar images");
        stopWatch.Reset();
        stopWatch.Start();

        if (File.Exists(parameters.image_of_interest))
            {
                // Case :find images similar to an image of interest in a directory
                try
                    {
                        if (!Path.IsPathRooted(parameters.image_of_interest))
                        {
                            parameters.image_of_interest = Path.Combine(Directory.GetCurrentDirectory(), parameters.image_of_interest);
                        }

                        var similar_images = images_index.SearchSimilarImages(parameters.image_of_interest, parameters.image_hash_algorithm, parameters.color_filter, parameters.min_similarity);
                        SaveSimilarHtml(parameters.image_of_interest, parameters.image_of_interest, parameters.min_similarity, similar_images, images_index);
                    }
                catch (Exception e)
                    {
                        Console.Error.WriteLine("Error  => Search similar images {0}: \n{1}", parameters.image_of_interest, e.Message);
                    }
            }
        else
            {
                // Case :find groups of similar images in a directory

                var seen = new Dictionary<string, byte>();

                foreach (var image in image_files)
                    {
                        if (!seen.ContainsKey(image))
                            {
                                try
                                    {
                                        seen.Add(image, 1);

                                        var similar_images = images_index.SearchSimilarImages(image, parameters.image_hash_algorithm, parameters.color_filter, parameters.min_similarity);
                                        SaveSimilarHtml(parameters.image_of_interest, image, parameters.min_similarity, similar_images, images_index);
                                        foreach (var simage in similar_images.Keys)
                                            {
                                                if (!seen.ContainsKey(simage))
                                                    {
                                                        seen.Add(simage, 1);
                                                    }
                                            }
                                    }
                                catch (Exception e)
                                    {
                                        Console.Error.WriteLine("Error  => Search similar images {0}: \n{1}", image, e);
                                    }
                            }
                    }
            }

        stopWatch.Stop();
        TimeSpan ts_search = stopWatch.Elapsed;
        PrintElapsedTime(ts_search);
    }

    #endregion Main

    #region Print elapsed time

    /// <summary>
    ///  Utility method used to print the time taken by a step of the search
    /// </summary>
    /// <param name="ts"></param>
    private static void PrintElapsedTime(TimeSpan ts)
    {
        string elapsedTime = String.Format("{0:00} h: {1:00} m: {2:00}.{3:00} s",
                                           ts.Hours, ts.Minutes, ts.Seconds,
                                           ts.Milliseconds / 10);
        Console.WriteLine("  RunTime: " + elapsedTime);
    }

    #endregion Print elapsed time

    #region Save similar html

    /// <summary>
    /// Save similar images as a web page
    /// </summary>
    /// <param name="image_searched">image name</param>
    /// <param name="min_similarity">similarity cutoff</param>
    /// <param name="similar_images_found">similar images (name -> similarity)</param>
    private static void SaveSimilarHtml(string image_of_interest, string image_searched, int min_similarity, Dictionary<string, int> similar_images_found, IImageIndex images_index, bool link_images = false)
    {
        string max_image_size = "256";
        if (similar_images_found.Count() > 1)
            {
                var image_name = Path.GetFileName(image_searched);
                Console.WriteLine("  - {0} is similar to {1} others", image_name, similar_images_found.Count() - 1);
                var image_info = images_index.ImageInfo(image_searched);
                var isimilarity = similar_images_found[image_searched];

                try
                    {
                        uint count = 0;
                        using (var shtml = new System.IO.StreamWriter(image_name + "_similar.html"))
                        {
                            count++;
                            shtml.WriteLine("<!DOCTYPE html>");
                            shtml.WriteLine("<html>");

                            shtml.WriteLine("<head><style>img{ max-width: " + max_image_size + "px; max-height: " + max_image_size + "px; }</style></head>");
                            shtml.WriteLine("<body style=\"background-color:#E6E6FA\">");

                            shtml.WriteLine(" <script language=\"JavaScript\">                                         ");
                            shtml.WriteLine("    function AddResolution(image, td){            ");
                            shtml.WriteLine("     var img = document.getElementById(image);    ");
                            shtml.WriteLine("     var w = img.naturalWidth;                    ");
                            shtml.WriteLine("     var h = img.naturalHeight;                   ");
                            shtml.WriteLine("     var td = document.getElementById(td);        ");
                            shtml.WriteLine("     var content = document.createTextNode(\" resolution \"+w+\"x\"+h+\" \");  ");
                            shtml.WriteLine("        td.appendChild(content);                  ");
                            shtml.WriteLine("    }                                             ");
                            shtml.WriteLine("                                                  ");
                            shtml.WriteLine("   </script>                                      ");

                            shtml.WriteLine("<table>");
                            shtml.WriteLine("<tr><td id=\"td{5}\" >sim. {0}% {1} <img  src=\"data:image/png;base64,{3}\" width=6 height=6 /> &nbsp; &nbsp; <img  src=\"data:image/png;base64,{4}\" width=50 height=6 />   &nbsp; size {2}  &nbsp;    </td></tr>", isimilarity, image_searched, image_info[0], image_info[2], image_info[3], count);

                            if (link_images)
                                {
                                    shtml.WriteLine("<tr><td> <a href=\"{1}\"><img src=\"file:///{0}\" id=\"img{2}\" onload=\"AddResolution('img{2}','td{2}')\" ></a> </td></tr>", image_searched, image_name + "_similar.html", count);
                                }
                            else
                                {
                                    shtml.WriteLine("<tr><td> <img src=\"file:///{0}\" id=\"img{1}\" onload=\"AddResolution('img{1}','td{1}')\" > </td></tr>", image_searched, count);
                                }

                            var sorted_images = similar_images_found.OrderByDescending(i => i.Value).ToDictionary(i => i.Key, i => i.Value).Keys.ToList();
                            foreach (var simage in sorted_images)
                                {
                                    count++;
                                    if (simage.Equals(image_searched))
                                        {
                                            continue;
                                        }

                                    var similarity = similar_images_found[simage];

                                    var simage_info = images_index.ImageInfo(simage);

                                    shtml.WriteLine("<tr><td  id=\"td{5}\">sim. {0}% {1}   <img  src=\"data:image/png;base64,{3}\" width=6 height=6 /> &nbsp;  <img  src=\"data:image/png;base64,{4}\" width=50 height=6 /> &nbsp; size {2}  &nbsp; </td></tr>", similarity, simage, simage_info[0], simage_info[2], simage_info[3], count);
                                    if (link_images)
                                        {
                                            shtml.WriteLine("<tr><td> <a href=\"{1}\"><img src=\"file:///{0}\" alt=\"{2}\" id=\"img{3}\" onload=\"AddResolution('img{3}','td{3}')\" ></a> </td></tr>", simage, Path.GetFileName(simage) + "_similar.html", similarity, count);
                                        }
                                    else
                                        {
                                            shtml.WriteLine("<tr><td> <img src=\"file:///{0}\" alt=\"{1}\"  id=\"img{2}\" onload=\"AddResolution('img{2}','td{2}')\" > </td></tr>", simage, similarity, count);
                                        }
                                }
                            shtml.WriteLine("</table>");
                            shtml.WriteLine("</body></html>");
                        }
                    }
                catch (Exception ex)
                    {
                        Console.WriteLine("Error  => Could not export {0} as html : {1}", image_name, ex.Message);
                    }
            }
        else
            {
                if (File.Exists(image_of_interest))
                    {
                        Console.WriteLine("\t - image {0} is not {1}% similar to other images", image_of_interest, min_similarity);
                    }
            }
    }

    #endregion Save similar html
}
}