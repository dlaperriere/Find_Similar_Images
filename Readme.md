# Find similar images

## Reverse image search utility based on perceptual hash algorithms

![screenshot](images/screenshot-findsimilar.png?raw=true)

## Perceptual hash algorithms

The perceptual hashes are calculated from reduced 8x8 grayscale images:

Algorithm   | Description
----------- | -----------
Average     | Use the pixel's value and the average color
Block       | Divide image in blocks and use their mean value
Difference  | Use the difference between adjacent pixels
Perceptive  | Use a discrete cosine transform (DCT) and compare frequencies rather than color values 


## Compilation

Find similar images can be built from source with the 'SimilarImages.sln' Visual Studio 2013 solution. 

Install the NuGet package 'OpenCvSharp3-AnyCPU'   https://github.com/shimat/opencvsharp

    PM> Install-Package OpenCvSharp3-AnyCPU

## Copyright

David Laperriere dlaperriere@outlook.com
