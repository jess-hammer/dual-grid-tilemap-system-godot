using Godot;

public partial class CursorController : Node2D
{
    [Export] DualGridTilemap dualGridTilemap;
    public override void _Process(double delta)
    {
        Vector2I coords = dualGridTilemap.LocalToMap(Position);
        if (Input.IsActionPressed("left_click"))
        {
            dualGridTilemap.SetTile(coords, TerrainType.Dirt);
        }
        else if (Input.IsActionPressed("right_click"))
        {
            dualGridTilemap.SetTile(coords, TerrainType.Grass);
        }
        else if (Input.IsActionPressed("middle_click"))
        {
            dualGridTilemap.SetTile(coords, TerrainType.Sand);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition = GetWorldPosTile(GetGlobalMousePosition()) + new Vector2(8, 8);
    }

    public static Vector2I GetWorldPosTile(Vector2 worldPos)
    {
        int xInt = Mathf.FloorToInt(worldPos.X / DualGridTilemap.TILE_SIZE) * DualGridTilemap.TILE_SIZE;
        int yInt = Mathf.FloorToInt(worldPos.Y / DualGridTilemap.TILE_SIZE) * DualGridTilemap.TILE_SIZE;
        Vector2I result = new(xInt, yInt);
        return result;
    }
}
