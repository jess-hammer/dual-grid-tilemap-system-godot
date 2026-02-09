using Godot;
using System.Collections.Generic;
using static TileType;
using static TerrainType;

[Tool]
public partial class DualGridTilemap : Node2D
{
    public static readonly int TILE_SIZE = 16;
    public static readonly int HALF_TILE_SIZE = TILE_SIZE / 2;
    readonly Vector2I[] NEIGHBOURS = [new(0, 0), new(1, 0), new(0, 1), new(1, 1)];

    private static readonly Dictionary<(TileType, TileType, TileType, TileType), Vector2I> neighboursToAtlasCoord = new() {
        { ( Some, Some, Some, Some ), new Vector2I(2, 1) }, // All corners
        { ( None, None, None, Some ), new Vector2I(1, 3) }, // Outer bottom-right corner
        { ( None, None, Some, None ), new Vector2I(0, 0) }, // Outer bottom-left corner
        { ( None, Some, None, None ), new Vector2I(0, 2) }, // Outer top-right corner
        { ( Some, None, None, None ), new Vector2I(3, 3) }, // Outer top-left corner
        { ( None, Some, None, Some ), new Vector2I(1, 0) }, // Right edge
        { ( Some, None, Some, None ), new Vector2I(3, 2) }, // Left edge
        { ( None, None, Some, Some ), new Vector2I(3, 0) }, // Bottom edge
        { ( Some, Some, None, None ), new Vector2I(1, 2) }, // Top edge
        { ( None, Some, Some, Some ), new Vector2I(1, 1) }, // Inner bottom-right corner
        { ( Some, None, Some, Some ), new Vector2I(2, 0) }, // Inner bottom-left corner
        { ( Some, Some, None, Some ), new Vector2I(2, 2) }, // Inner top-right corner
        { ( Some, Some, Some, None ), new Vector2I(3, 1) }, // Inner top-left corner
        { ( None, Some, Some, None ), new Vector2I(2, 3) }, // Bottom-left top-right corners
        { ( Some, None, None, Some ), new Vector2I(0, 1) }, // Top-left down-right corners
        { ( None, None, None, None ), new Vector2I(-1, -1) } // No corners, will be treated as empty tile
    };

    [Export] TileMapLayer worldMapLayer; // should be set in the editor
    [Export] TileMapLayer grassDisplayMapLayer; // should be set in the editor
    [Export] TileMapLayer dirtDisplayMapLayer; // should be set in the editor
    [Export] TileMapLayer sandDisplayMapLayer; // should be set in the editor
    [Export] public Vector2I grassPlaceholderAtlasCoords; // should be set in the editor
    [Export] public Vector2I dirtPlaceholderAtlasCoords; // should be set in the editor
    [Export] public Vector2I sandPlaceholderAtlasCoords; // should be set in the editor
    int placeholderSourceId = 0;

    public override void _Ready()
    {
        RefreshAllTiles();
    }

    public void RefreshAllTiles()
    {
        foreach (Vector2I cellPos in worldMapLayer.GetUsedCells())
        {
            // Refresh each display layer with its own terrain type
            // This ensures each layer recalculates based on matching neighbors
            RefreshDisplayTile(cellPos, grassDisplayMapLayer, Grass);
            RefreshDisplayTile(cellPos, dirtDisplayMapLayer, Dirt);
            RefreshDisplayTile(cellPos, sandDisplayMapLayer, Sand);
        }
    }

    public override void _Process(double delta)
    {
        // optional - allows you to see changes in editor but may be costly
        if (Engine.IsEditorHint())
        {
            RefreshAllTiles();
        }
    }

    public Vector2I LocalToMap(Vector2 pos)
    {
        return worldMapLayer.LocalToMap(pos);
    }

    public void SetTile(Vector2I coords, TerrainType terrainType)
    {
        TerrainType oldTerrainType = GetTerrainType(coords);

        worldMapLayer.SetCell(coords, placeholderSourceId, GetPlaceholderAtlasCoords(terrainType));
        RefreshDisplayTile(coords, GetDisplayLayer(terrainType), terrainType);

        // Refresh old display tiles if changing terrain type
        if (oldTerrainType != terrainType && oldTerrainType != Empty)
        {
            RefreshDisplayTile(coords, GetDisplayLayer(oldTerrainType), oldTerrainType);
        }
    }

    void RefreshDisplayTile(Vector2I pos, TileMapLayer displayLayer, TerrainType terrainType)
    {
        // loop through 4 display neighbours
        for (int i = 0; i < NEIGHBOURS.Length; i++)
        {
            Vector2I newPos = pos + NEIGHBOURS[i];
            Vector2I atlasCoords = CalculateDisplayTileAtlasCoords(newPos, terrainType);
            displayLayer.SetCell(newPos, (int)terrainType, atlasCoords);
        }
    }

    Vector2I CalculateDisplayTileAtlasCoords(Vector2I coords, TerrainType terrainType)
    {
        // get 4 world tile neighbours
        TileType botRight = GetMatchingTileType(coords - NEIGHBOURS[0], terrainType);
        TileType botLeft = GetMatchingTileType(coords - NEIGHBOURS[1], terrainType);
        TileType topRight = GetMatchingTileType(coords - NEIGHBOURS[2], terrainType);
        TileType topLeft = GetMatchingTileType(coords - NEIGHBOURS[3], terrainType);

        // return tile (atlas coord) that fits the neighbour rules
        return neighboursToAtlasCoord[new(topLeft, topRight, botLeft, botRight)];
    }

    TileType GetMatchingTileType(Vector2I coords, TerrainType terrainType)
    {
        Vector2I targetPlaceholderAtlasCoords = GetPlaceholderAtlasCoords(terrainType);
        Vector2I atlasCoord = worldMapLayer.GetCellAtlasCoords(coords);
        if (atlasCoord != targetPlaceholderAtlasCoords)
            return None;
        else
            return Some;
    }

    Vector2I GetPlaceholderAtlasCoords(TerrainType terrainType)
    {
        return terrainType switch
        {
            Grass => grassPlaceholderAtlasCoords,
            Dirt => dirtPlaceholderAtlasCoords,
            Sand => sandPlaceholderAtlasCoords,
            _ => -Vector2I.One
        };
    }

    TileMapLayer GetDisplayLayer(TerrainType terrainType)
    {
        return terrainType switch
        {
            Grass => grassDisplayMapLayer,
            Dirt => dirtDisplayMapLayer,
            Sand => sandDisplayMapLayer,
            _ => null
        };
    }

    TerrainType GetTerrainType(Vector2I cellPos)
    {
        Vector2I placeholderAtlasCoords = worldMapLayer.GetCellAtlasCoords(cellPos);
        switch (placeholderAtlasCoords)
        {
            case var coords when coords == grassPlaceholderAtlasCoords:
                return Grass;
            case var coords when coords == dirtPlaceholderAtlasCoords:
                return Dirt;
            case var coords when coords == sandPlaceholderAtlasCoords:
                return Sand;
            case var coords when coords == -Vector2I.One:
                return Empty;
            default:
                GD.PrintErr($"Unknown placeholder atlas coords: {placeholderAtlasCoords}. Defaulting to Grass.");
                return Grass;
        }
    }
}

public enum TileType
{
    None,
    Some,
}

// The number should correspond to the sourceId in the TileSet
public enum TerrainType
{
    Empty = -1,
    Grass = 1,
    Dirt = 2,
    Sand = 3,
}