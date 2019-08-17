When a task is repetitive, why would we want to do it ourselves, if someone or something else could do it equally well or better? Computers are really good at doing repetitive tasks because they are accurate, don’t get bored, complain or need to be paid. Tasks which run within a system are the easiest and cheapest to automate, others can be cost effective if there is enough benefit. For example robots on a factory production line, automated driving or flying an aircraft. 

----------

### SimCity BuildIt

Certain computer games are a time sink, requiring constant attention to make progress and maintain attention.

![alt text](/post/img/simcity_screenshot.jpg "Sim City BuildIt Screenshot")

SimCity BuildIt (a mobile game which runs on the Android or IOS platform) is one such game and has 2 obvious repetitive areas:

*  Crafting: It requires you to manufacture raw materials and then use them to create other items which are more valuable. These items have an in-game value and can be sold or used to complete tasks.
*  Global Trade: Buying non craft able and time expensive craft-able items. Selling items I have crafted.
 
Because I want to do the fun things in the game without suffering the pain, I wrote a bot in C# to do the crafting. 

Rather than writing the code native on the platform I use an android emulator (Memu) running on my PC and I send commands to it from my C# code to automate the play. 

The basic iterations between the bot and the game are:

- Touch and Swipe on the screen.
- Read text from the display.
- Read the colour of text on the display.

----------


### Bot Demo

[YouTube Video](https://www.youtube.com/watch?v=OOk6HWBdy6U)
[YouTube Video 2](https://www.youtube.com/watch?v=JFwONo9b-RE)

----------

### 1. Crafting Bot Detail

To build a crafting bot I needed to be able to do the following:


#### Select a building
This can be done by touching the centre of the screen using ADB (Android Debug Bridge), a building must be located at the centre of the screen when the bot is started. It helps to turn on the developer options for 'pointer location' and 'showing touches'.

![alt text](/post/img/simcity_options.jpg "Developer Options")


#### Know which building I am in
The name of the building is shown at the top of the screen, this is read using OCR (Optical Character Recognition) using Tesseract Open Source OCR Engine. A screen shot of part of the MEMU application is taken and OCR'd.

![alt text](/post/img/simcity_title.jpg "Building Title")


#### Navigate to another building
There are buttons on the side of the building name which will cycle around the factories or commercial buildings.

#### Move between a factory and a Commercial building
This is done by positioning selected buildings (one commercial and one factory) in a known configuration and then clicking above or below to switch. For example the Hardware store has a Mass Production factory above it and both of these are unique buildings, so when we are at either in the building cycle we can move to the other.

![alt text](/post/img/simcity_hardware_store.jpg "Building Positions")


#### Build an Item in either a factory or commercial building

The craft-able items are laid out on buttons in specific places. So we can swipe from the button to the crafting location to build the item. Determining if we need to craft raw materials for craft-able items is found out by clicking on the item's button and looking to see the colour of the required materials. Red means we don't have enough.

![alt text](/post/img/simcity_raw_materials.jpg "Building Title")


#### Collect an item
Collecting a crafted item requires touching the building.


Next I tackle buying from "Global Trade" and Selling crafted items to generate in game currency.

The main new problem I had to solve was recognising images from the game screen. This was solved using perceptual hashing with a dictionary of categorised image hashes.

<br/>

### 2. Buying Items

Assuming we have a shopping list of items we need, then the goal is to buy those items.

Clicking on the Global trade building brings up the trade window. The window contains a number of panels, each representing an item for sale. Dragging within the list from right to left reveals more items.

![Global Trade HQ](/post/img/SB2_1_Global_Trade.png)

Usefully each panel is the same size, so If we can find where a panel starts, then the item will always be in the same place and all we need to do is to recognise the item in the panel. The way I solved this is described below:

<br/>

#### Finding Panels

The tops of the panels are always the same distance from the top of the screen. 
Finding the tops of the panels required looking along this line for pixels close to the colour of the bounding line around the panels (See indicator red lines at the top of each panel below).

Each continuous block of matching pixels is one panel. The first pixel in each line represents the top left of the panel. 

![Panels](/post/img/SB2_2_Global_Trade.png)

<br/>

#### Recognising the item in the panel

If we image capture a rectangle a fixed distance from the top left of the panel of a set size, we now have an image which can be processed and recognised using Image hashing.

![Sale Item](/post/img/SB2_3_Panel.png)

We recognise it using a library of pre-captured images. We create this library by storing unmatched images and then classify them by hand into named folders. Images which are almost the same (90%) can be automatically classified.

As each image is processed  a hash is created e.g. 70088502915571468. The file is then renamed to the hash so that this never needs to be re-computed.

![Image Hashes](/post/img/SB2_4_Hashes.png)

Once we've recognised an item that we want to buy, we then click on the panel and are transported to the trade depot of the city who is selling the item.

<br/>

#### Buying the item

Once you are at the city selling the item, you usually find many other items for sale.

The trade depot is similar to the global trade except that it has boxes instead of panels. We can use the same approach we used earlier to find the contents of the boxes and click on the desired item to buy it

![Trade Depot](/post/img/SB2_5_TradeDepot.png)

<br/>

### 3. Selling items

We have a trade depot where we can sell items. It contains a number of boxes each of which can contain a single item for sale. Once we click on an empty box a "Create Sale" dialog is shown.
 
![Trade Depot](/post/img/SB2_7_HomeDepot.png)

To sell an item, we need to:

 1. Open up our trade depot.
 2. Collect any sold items.
 3. Pick an empty box, we may need to scroll horizontally to see more boxes.
 4. Click on the box to open the "Create Sale" dialog.
 5. Pick an item to sell and click on it.
 6. Press the quantity increase button until it turns grey.
 7. Press the price increase button until it turns grey.
 8. Click the 'Put on sale' button.

Most of these steps simply require clicking at predetermined locations on the screen.

Steps 2, 3 & 5 use the same image matching technique used when buying items.



<br/>

### 4. What is Perceptual Hashing

> A perceptual hash is a fingerprint of a multimedia file derived from various features from its content. Unlike cryptographic hash functions which rely on the avalanche effect of small changes in input leading to drastic changes in output, 
perceptual hashes are "close" to one another if the features are similar.

The bot uses https://github.com/jforshee/ImageHashing which is an implementation of the algorithm found here: http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html

A long number hash is created in the following steps:

 1. Reduce the image to 8x8 pixels
 2. Reduce the color of each pixel from RGB (24 bits) to grayscale (6 bits).
 3. Compute the mean of all the 6-bit values
 4. Translate each pixel into a bit. 1 if the greyscale is >= to the mean.
 5. Turn the binary number into a long decimal.

The following shows the processes as it took place on 2 similar images. The images turned out to be only 73% similar. 

![Hashing Examples](/post/img/SB2_6_Compared.png)

----------

### 5. The things I learn't writing the bot:

- You can't use User32.dll methods to click on the Memu application, you need to use ADB.
- ADB Input commands for tap and swipe don't work reliably. Instead I used ADB 'sendevent' to send a series of commands codes to do each tap and swipe.
- A seeming complex workflow can be simplified into a working automated process with surprisingly few building blocks.
