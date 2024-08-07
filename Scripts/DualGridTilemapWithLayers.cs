using Godot;
using System;
using System.Collections.Generic;
using static TileType;

public partial class DualGridTilemapWithLayers : TileMap {
    public static int MAIN_LAYER = 0;
    public static int DISPLAY_LAYER = 1;
    [Export] public Vector2I grassPlaceholderAtlasCoord;
    [Export] public Vector2I dirtPlaceholderAtlasCoord;
    readonly Vector2I[] NEIGHBOURS = new Vector2I[] { new(0, 0), new(1, 0), new(0, 1), new(1, 1) };

    readonly Dictionary<Tuple<TileType, TileType, TileType, TileType>, Vector2I> neighboursToAtlasCoord = new() {
        {new (Grass, Grass, Grass, Grass), new Vector2I(2, 1)}, // DEFAULT
        {new (Dirt, Dirt, Dirt, Grass), new Vector2I(1, 3)}, // OUTER_BOTTOM_RIGHT
        {new (Dirt, Dirt, Grass, Dirt), new Vector2I(0, 0)}, // OUTER_BOTTOM_LEFT
        {new (Dirt, Grass, Dirt, Dirt), new Vector2I(0, 2)}, // OUTER_TOP_RIGHT
        {new (Grass, Dirt, Dirt, Dirt), new Vector2I(3, 3)}, // OUTER_TOP_LEFT
        {new (Dirt, Grass, Dirt, Grass), new Vector2I(1, 0)}, // EDGE_RIGHT
        {new (Grass, Dirt, Grass, Dirt), new Vector2I(3, 2)}, // EDGE_LEFT
        {new (Dirt, Dirt, Grass, Grass), new Vector2I(3, 0)}, // EDGE_BOTTOM
        {new (Grass, Grass, Dirt, Dirt), new Vector2I(1, 2)}, // EDGE_TOP
        {new (Dirt, Grass, Grass, Grass), new Vector2I(1, 1)}, // INNER_BOTTOM_RIGHT
        {new (Grass, Dirt, Grass, Grass), new Vector2I(2, 0)}, // INNER_BOTTOM_LEFT
        {new (Grass, Grass, Dirt, Grass), new Vector2I(2, 2)}, // INNER_TOP_RIGHT
        {new (Grass, Grass, Grass, Dirt), new Vector2I(3, 1)}, // INNER_TOP_LEFT
        {new (Dirt, Grass, Grass, Dirt), new Vector2I(2, 3)}, // DUAL_UP_RIGHT
        {new (Grass, Dirt, Dirt, Grass), new Vector2I(0, 1)}, // DUAL_DOWN_RIGHT
		{new (Dirt, Dirt, Dirt, Dirt), new Vector2I(0, 3)},
    };

    public override void _Ready() {
        // Refresh all display tiles
        foreach (Vector2I coord in GetUsedCells(MAIN_LAYER)) {
            setDisplayTile(coord);
        }
    }

    public void SetTile(Vector2I coords, Vector2I atlasCoords) {
        SetCell(0, coords, 0, atlasCoords);
        setDisplayTile(coords);
    }

    void setDisplayTile(Vector2I pos) {
        // loop through 4 display neighbours
        for (int i = 0; i < NEIGHBOURS.Length; i++) {
            Vector2I newPos = pos + NEIGHBOURS[i];
            SetCell(DISPLAY_LAYER, newPos, 0, calculateTile(newPos));
        }
    }

    Vector2I calculateTile(Vector2I coords) {
        // get 4 world tile neighbours
        TileType botRight = getTileType(GetCellAtlasCoords(0, coords - NEIGHBOURS[0]));
        TileType botLeft = getTileType(GetCellAtlasCoords(0, coords - NEIGHBOURS[1]));
        TileType topRight = getTileType(GetCellAtlasCoords(0, coords - NEIGHBOURS[2]));
        TileType topLeft = getTileType(GetCellAtlasCoords(0, coords - NEIGHBOURS[3]));

        // return tile (atlas coord) that fits the neighbour rules
        return neighboursToAtlasCoord[new(topLeft, topRight, botLeft, botRight)];
    }

    TileType getTileType(Vector2I atlasCoord) {
        if (atlasCoord == grassPlaceholderAtlasCoord)
            return Grass;
        else
            return Dirt;
    }
}