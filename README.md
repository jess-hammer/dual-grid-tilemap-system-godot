# ✨👯 Dual-grid tilemap system in Godot 👯✨

#### This is a demo of how I implemented a dual-grid tilemap system in Godot 4.2. Hope it helps!


https://github.com/jess-hammer/dual-grid-tilemap-system-godot/assets/59108399/74928b50-8bc5-4ace-bb97-4f400e86d65a

Long story short, it does NOT use terrain tiles but rather, a custom script that inherits from `TileMap`! Also worth noting, instead of two seperate TileMaps, only one TileMap with 2 layers is used. Since you can't offset the layers individually, the tiles themselves are offset (which you can find in the TileSet settings).

The main reasons I love the dual-grid approach as opposed to regular terrain tiles/auto tiles 
is because:
- it allows the tiles to have perfectly rounded corners
- a maximum of only 16 tiles are required in the tileset (you could cut that number down to 6 if your tiles have symmetry)
- the tiles align with the world grid

One could also argue it's more efficient, since each tile only checks 4 neighbours as opposed to 8. But I haven't done any performance testing so no promises.

I also made a version for Unity which you can find [here](https://github.com/jess-hammer/dual-grid-tilemap-system-unity)

### References

See Oskar Stålberg's original proposition of a dual-grid system: https://x.com/OskSta/status/1448248658865049605

Just like Oskar...I don't understand why the dual-grid method isn't more popular! 

Regular tile cut:

<img src="https://github.com/jess-hammer/dual-grid-tilemap-system-godot/assets/59108399/ac3c9ab6-b399-4142-8425-3de6d67249a0" width="350" title="Inward blob cut">

On-the-dual tile cut:

<img src="https://github.com/jess-hammer/dual-grid-tilemap-system-godot/assets/59108399/5399d1b6-7169-4ff8-8a17-1ba8e483fce3" width="350" title="Inward blob cut">


Dual-grid system is also mentioned in ThinMatrix's video, 
which is what inspired me in the first place: https://youtu.be/buKQjkad2I0?si=9xot1uUw3PvNWvT9&t=234

If you have any questions or ideas for improvements feel free to contact me at jesscodesyt@gmail.com
