# ✨👯 Dual-grid tilemap system in Godot 👯✨

#### This is a demo of how I implemented a dual-grid tilemap system in Godot 4.2. Hope it helps!


https://github.com/jess-hammer/dual-grid-tilemap-system-godot/assets/59108399/74928b50-8bc5-4ace-bb97-4f400e86d65a

Long story short, it does NOT use terrain tiles but rather, a custom C# script that inherits from `TileMap`!

The main reasons I love the dual-grid approach as opposed to regular terrain tiles/auto tiles 
is because:
- it allows the tiles to have perfectly rounded corners
- a maximum of only 16 tiles are required in the tileset (you could cut that number down to 6 if your tiles have symmetry)
- the tiles align with the world grid

One could also argue it's more effecient since each tile only checks 4 neighbours as opposed to 9 🤯. But haven't done any performance testing so no promises 😅.

Just like Oskar, I don't understand why this isn't more popular!

### References

See Oskar Stålberg's original proposition of a dual-grid system: https://x.com/OskSta/status/1448248658865049605

Regular tile cut:

<img src="https://github.com/jess-hammer/dual-grid-tilemap-system-godot/assets/59108399/ac3c9ab6-b399-4142-8425-3de6d67249a0" width="350" title="Inward blob cut">

On-the-dual tile cut:

<img src="https://github.com/jess-hammer/dual-grid-tilemap-system-godot/assets/59108399/5399d1b6-7169-4ff8-8a17-1ba8e483fce3" width="350" title="Inward blob cut">


Dual-grid system is also mentioned in ThinMatrix's video, 
which is what inspired me in the first place: https://youtu.be/buKQjkad2I0?si=9xot1uUw3PvNWvT9&t=234

If you have any questions or ideas for improvements feel free to contact me at jesscodesyt@gmail.com
