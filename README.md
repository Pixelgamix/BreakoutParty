# BreakoutParty
A simple pixel art style breakout clone made by [Pixelgamix](https://github.com/Pixelgamix) using [FNA](https://github.com/FNA-XNA/FNA) and [Farseer Physics](https://farseerphysics.codeplex.com/).

## Work in progress
BreakoutParty is in very early development stages. Weirdness, bugs and even crashes are to be expected. Since the source code changes frequently it does not make much sense to contribute at the moment. Feel free to fork and play around, but we most likely won't accept pull requests until the game is much more stable and feature complete.

## System requirements
* For development: Windows 7 and Visual Studio 2015 Community Edition
* For playing: Graphics card with basic shader support, Windows, Linux or Mac (Linux and Mac being untested)

## Downloading native libraries
FNA depends on some native libraries that are **not** included in this repository in order to keep the repository size small. You can either get and compile the native libraries yourself or download them from the official [FNA website](http://fna-xna.github.io/download/). After downloading them just copy the native libraries for your operating system into the folder you built BreakoutParty to.

## Custom build Farseer Physics
Our Farseer Physics Dll is like the official MonoGame version with 2 exceptions:
* We replaced references to MonoGame with references to FNA
* We lowered the VelocityThreshold setting to work around reflections sometimes not being calculated as such (see http://box2d.org/forum/viewtopic.php?f=4&t=7392 for information on this one)
