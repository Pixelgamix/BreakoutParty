# BreakoutParty
A simple pixel art style breakout clone made by [Pixelgamix](https://github.com/Pixelgamix) using [FNA](https://github.com/FNA-XNA/FNA) and [Farseer Physics](https://farseerphysics.codeplex.com/).

## System requirements
* [.NET Framework 4.5.2](https://www.microsoft.com/en-US/download/details.aspx?id=42642)
* [VC++ 2010 redistributable](https://www.microsoft.com/en-us/download/details.aspx?id=5555) (Windows only)
* For development: Windows 7 and Visual Studio 2015 Community Edition
* For playing: Graphics card with basic shader support, Windows, Linux or Mac (Linux and Mac being untested)

## Downloading native libraries
FNA depends on some native libraries that are **not** included in this repository in order to keep the repository size small. Currently there are 2 ways to obtain the native libraries:

1. Download them from the official [FNA website](http://fna-xna.github.io/download/). Please note that these files get updated by the FNA dev team without regards of backwards compatibility so they might not work with the FNA version BreakoutParty is using.
2. Download a BreakoutParty [Release](https://github.com/Pixelgamix/BreakoutParty/releases) and grab the native libraries from there.

After downloading them just copy the native libraries for your operating system into the folder you built BreakoutParty to. Copy all of them if you want to make sure the game runs on every operating system.
