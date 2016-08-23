ImageHashing
============

Overview
--------

This is a very simple C# library used to generate average perceptual hashes of images and files containing image content. A similarity function is also included to compare hashes to one another and obtain a percentage on just how alike those hashes are. Various convenience methods are also provided to reduce the number of calls made by the user.

This library is free to use and based heavily on:

* Dr. Neal Krawetz's average hashing algorithm, outlined at http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html
* David Oftedal's C# implementation of Dr. Krawetz's algorithm, available at http://folk.uio.no/davidjo/computing.php

Credit goes to the both of them for the base logic and an initial implementation. My edits to Oftedal's code are mostly readability, style, and a few additional convenience functions.

Perceptual Hashing
------------------

For those not familiar with perceptual hashing, pHash.org gives a good definition:

> A perceptual hash is a fingerprint of a multimedia file derived from various features from its content. Unlike cryptographic hash functions which rely on the avalanche effect of small changes in input leading to drastic changes in output, perceptual hashes are "close" to one another if the features are similar.

So how is this useful? Well, let's say you're a forensic scientist with a lead on a case. You have an image that you are looking for in some other collection of photos. If the manager of that collection changes even one pixel of the image, then the MD5/SHA-1/whatever hash will end up being *completely* different. Herein lies a problem: traditional cryptographic hashes are either a yes or a no. While this is great for verifying very content-sensitive files (like a downloaded software package), it doesn't work well with multimedia recognition. Changing one pixel in a picture won't change the content according to the human eye; we will still recognize it as the same image.

Enter perceptual hashing. Perceptual hashes are based on the multimedia content of a file, not just the bytes contained in that file. This allows us to run comparisons on image files and determine similarity both easily and efficiently. This technology is widely used: both Google and TinEye use it for image searches by crawling the web for images, computing hashes, storing them in some database, then running your given image against those known hashes for matches.