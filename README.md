# MLE-Slot-Usage-Tool
This is a tool to look up different teams player/slot usage, displaying their uses throughout the season, and showing their salary and scrim points

## Downloading the tool
There is a single executable application that you need to download, targeting windows X86 (If you're on Windows PC, this will work)
* in the future, I would like to provide a platform independent option, but this will require .NET 6 to be installed. Currently the framework is packaged with the application

## Running the tool
Run the application as you would any other project, and a console window will appear. Some text will display, possibly including failed processing data, this typically will still allow the program to run just fine, it's just good to keep in mind if you are looking at that players team/league and notice something weird. Will create further updates to fix these
To query a teams usage you must enter the name of their team followed by the specific league you are looking at. For example, "Foxes AL", or "Blizzard ML". This will then display the current players on their team in that league, in order of their slot they are in. Following that will be their salary and eligibility points, along with the games played in doubles, standard, and combined. 
