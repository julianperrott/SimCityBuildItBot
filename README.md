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

----------

### Bot Detail

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

----------

### The things I learn't writing the bot:

- You can't use User32.dll methods to click on the Memu application, you need to use ADB.
- ADB Input commands for tap and swipe don't work reliably. Instead I used ADB 'sendevent' to send a series of commands codes to do each tap and swipe.
- A seeming complex workflow can be simplified into a working automated process with surprisingly few building blocks.
- I need to extend the bot to sell the items on the global trade as this is a pain after the bot has been running for a few hours.
