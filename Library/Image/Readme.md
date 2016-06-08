## Images hashing algorithms

### Perceptual hash from reduced 8x8 grayscale images

- ImageAverageHash.cs:  Use the pixel's value and the average color.
- ImageBlockHash.cs: Divide image in blocks and use their mean value.
- ImageDifferenceHash.cs: Use the difference between adjacent pixels.
- ImagePHash.cs:  Use a discrete cosine transform (DCT) and compare frequencies rather than color values.

### Image color

- ImageColorHash.cs : Main color of an image
- ImageHistogramHash.cs : RGB histogram 

### references

- http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html
- http://bertolami.com/index.php?engine=blog&content=posts&detail=perceptual-hashing
- https://en.wikipedia.org/wiki/Perceptual_hashing
- http://www.coolphptools.com/color_extract
- Implementation and benchmarking of perceptual image hash functions http://phash.org/docs/pubs/thesis_zauner.pdf
