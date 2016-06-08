// image index interface

using System;
using System.Collections.Generic;

internal interface IImageIndex
{
    void IndexImageDirectory(string dir);

    List<String> ImageFilesIndexed();

    List<string> ImageInfo(string image_name);

    Dictionary<string, int> SearchSimilarImages(string image, Images.ImageHashAlgorithm imagehash, Images.ComparisonMethod filter_method, int similarity_cutoff);
}