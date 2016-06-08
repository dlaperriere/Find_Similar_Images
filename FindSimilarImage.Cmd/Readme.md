## Command line version of FindSimilarImages
 - Find images similar to an image of interest in a directory
 - Find groups of similar images in a directory
 - Index images in a directory
  
 ***Note :***
 Similar images are saved in "\*_similar.html" files and index in "images\*.idx" files.

-----
## Usage

 Find images similar to an image of interest in a directory

    FindSimilarImages.Cmd  -dir name -image name -algo block  -filter colors  -similarity 80

 Find groups of similar images in a directory

    FindSimilarImages.Cmd  -dir name -algo difference  -filter color  -similarity 80

 Index images in a directory

    FindSimilarImages.Cmd  -index dir_name

 Show program parameters and usage information

    FindSimilarImages.Cmd  -help

## Parameters

image directory

    -dir name

image of interest

    -image name

perceptual hash algorithm

    -algo average|block|difference|perceptive
    -algo caverage|cblock|cdifference|cperceptive   (hash + main color filter)
    -algo haverage|hblock|hdifference|hperceptive   (hash + RGB histogram filter)

color filter

    -filter color          main color
    -filter colors         dominant colors (up to 20)
    -filter difference     compare pixels RGB values
    -filter sdifference    compare pixels sorted by luminance
    -filter distance       compare pixels RGB distance
    -filter sdistance      compare pixels sorted by luminance

minimum similarity %

    -similarity percentage (70 = 70%)


