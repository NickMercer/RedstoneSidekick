# RedstoneSidekick
A rebuild of the RedstoneSidekick application using .NET 5

![](https://github.com/NickMercer/RedstoneSidekick/blob/master/RedstoneSidekick/Images/App/RSBanner.png?raw=true)
#### A Minecraft Crafting List Application

Get the Newest Release Here: [v1.0](https://github.com/NickMercer/RedstoneSidekick/releases/tag/v1.0)

Redstone Sidekick is a companion desktop application for Minecraft that speeds up your building by creating material lists for your Minecraft projects. You can add items to the list manually, or you can let Redstone Sidekick read Minecraft structure files (.nbt) and automatically generate a crafting list from them. In addition, you can save your item gathering progress and share your crafting lists with others, either through Redstone Sidekick project files or by generating project strings that can be given to other users.

## A Brief Guide to Redstone Sidekick
Below is a quick guide to the basic functionality of Redstone Sidekick. In this guide you'll see how to:
* Create a project from a Minecraft structure file
* Add/Remove Items
* View your project's "shopping list" 
* Save and load projects through .rsp files or project strings.

## Creating a Project From a Minecraft Structure File
If you need help creating a structure file in Minecraft, there is a [tutorial here.](https://www.digminecraft.com/getting_started/structure_block_corner_mode.php "tutorial here.")

In brief, a structure file, (marked by the .nbt file type) is a file used by Minecraft to save a three dimensional section of blocks. Redstone Sidekick can parse these files to give you a list of the blocks in that structure; and also break those blocks down into their constituent parts. 

In order to create a RS Project from a structure file, go to the file menu in Redstone Sidekick, select "New Project from Structure File" or "Add Structure to Project" and select your .nbt file in the file explorer.

![](https://i.imgur.com/Sd3C11Y.png)

Your structure will populate the crafting list in the main window, and look something like this:

![](https://i.imgur.com/fmkLwpj.png)

## Adding and Removing Items

Sometimes, you may have items in the structure file that you do not want to actually replicate in your crafting list, or you may want to add items to your list that were not present in the structure file. You can remove items by selecting them in the Crafting Tree tab, and pressing the "Delete Selected Item" button in the bottom left. 

To add items, use the categorized, searchable list of blocks on the right hand side of the project window, and double click the item you want to add. It will add the item to the project with a quantity of zero, and then you can edit the quantity by selecting it.

In the example project, we removed extra items from the structure like the grass blocks it was placed on, and added 10 diamonds from the item menu on the right.

![](https://i.imgur.com/RJTjVHR.png)

## The Crafting Tree vs. The Shopping List

So far, we've looked at the project through the crafting tree view. The crafting tree view shows you every block that is in the final structure. For blocks that have crafting recipes to create them, there is a plus button on the bottom right of the block display to see the component parts. 

![](https://i.imgur.com/JkgUok8.png)

The shopping list view lets you see what you will actually need to gather to create the structure. The shopping list aggregates all the material counts for each final block in the structure, and condenses them into a list of the base materials. If you don't want a block or its ingredients to appear on the shopping list, check the box on the block in the crafting tree view, and it will be ignored.

In the shopping list view, you can keep track of how many items you have gathered. When you have gathered enough, Redstone Sidekick will check that item off your list. If you go back to the Crafting Tree view and edit your structure, the program will still remember your previous quantities, and adjust accordingly.

![](https://i.imgur.com/UVBkU5k.png)

## Saving and Loading Projects
Redstone Sidekick gives you two methods for saving projects: Redstone Sidekick Project files, and Project Strings. 

#### Redstone Sidekick Project Files (.rsp)
Project files save your project to your local machine, and store:
* The Project Name
* The Crafting Tree List
* Your Shopping List Gathering Progress

This is a complete save of your project, and will allow you to continue your gathering from where you left off. 

However, because this saves your gathering progress and requires sharing a file, this isn't the ideal way to share projects with others. For that, there are Project Strings.

#### Project Strings
Project strings are designed for easy sharing of projects with other users. These are especially helpful for content creators, as a tutorial maker can include a project string in their YouTube description or Twitch chat, and easily share the item list for a project with their audience, with no download necessary.

Project strings do not include the gathering progress for the shopping list. This means any time you load a project string it will create a "fresh" project for you.

In order to create a project string for your structure, simply go to the File menu and click "Copy Project String to Clipboard".

To create a project from a project string, go to File -> New Project From Project String. You will be prompted to paste your project string into the window that pops up, and it will populate a new project with that data.

![](https://i.imgur.com/wzzWYbL.png)


## Examples

Those are the basics of Redstone Sidekick! Included in the git repository and release are a couple Minecraft structure files to demo the program, some images of what those structures create in Minecraft, and project strings for those same structures.

Release: [v1.0](https://github.com/NickMercer/RedstoneSidekick/releases/tag/v1.0)

I hope you enjoy the project, and please let me know if you have any questions or suggestions to make Redstone Sidekick even more helpful. 

Happy Building!
