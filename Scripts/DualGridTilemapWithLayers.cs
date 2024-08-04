using Godot;
using System;
using System.Collections.Generic;
using static TileType;

public partial class DualGridTilemapWithLayers : TileMap {
    public static int MAIN_LAYER = 0;
    public static int DISPLAY_LAYER = 1;
    protected static Vector2I[] NEIGHBOURS = new Vector2I[] {
        new Vector2I(0, 0),
        new Vector2I(1, 0),
        new Vector2I(0, 1),
        new Vector2I(1, 1)
    };

    protected static readonly Dictionary<Tuple<TileType, TileType, TileType, TileType>, Vector2I> neighbourTupleToAtlasCoord = new() {
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

    [Export] public Vector2I grassPlaceholderAtlasCoord;
    [Export] public Vector2I dirtPlaceholderAtlasCoord;

    public override void _Ready() {
        base._Ready();
        RefreshDisplayLayer();
    }

    public void SetTile(Vector2I coords, Vector2I atlasCoords) {
        SetCell(0, coords, 0, atlasCoords);
        setDisplayTile(coords);
    }

    protected void setDisplayTile(Vector2I pos) {
        for (int i = 0; i < NEIGHBOURS.Length; i++) {
            Vector2I newPos = pos + NEIGHBOURS[i];
            base.SetCell(DISPLAY_LAYER, newPos, 0, calculateTile(newPos));
        }
    }

    protected Vector2I calculateTile(Vector2I coords) {
        // 4 neighbours
        TileType topLeft = getTileType(GetCellAtlasCoords(0, coords + new Vector2I(-1, -1)));
        TileType topRight = getTileType(GetCellAtlasCoords(0, coords + new Vector2I(0, -1)));
        TileType botLeft = getTileType(GetCellAtlasCoords(0, coords + new Vector2I(-1, 0)));
        TileType botRight = getTileType(GetCellAtlasCoords(0, coords + new Vector2I(0, 0)));

        Tuple<TileType, TileType, TileType, TileType> neighbourTuple = new(topLeft, topRight, botLeft, botRight);

        return neighbourTupleToAtlasCoord[neighbourTuple];
    }

    private TileType getTileType(Vector2I atlasCoord) {
        if (atlasCoord == grassPlaceholderAtlasCoord)
            return Grass;
        else
            return Dirt;
    }

    public void RefreshDisplayLayer() {
        foreach (Vector2I coord in GetUsedCells(MAIN_LAYER)) {
            setDisplayTile(coord);
        }
    }
}