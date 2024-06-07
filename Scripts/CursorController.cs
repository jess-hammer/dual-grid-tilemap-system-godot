using Godot;
using static DualGridTilemap;

public partial class CursorController : Node2D {
    [Export] DualGridTilemap dualGridTilemap;
    public override void _Process(double delta) {
        Vector2I coords = dualGridTilemap.LocalToMap(Position);
        if (Input.IsActionPressed("left_click")) {
            dualGridTilemap.SetCell(MAIN_LAYER, coords, dualGridTilemap.dirtPlaceholderAtlasCoord);
        } else if (Input.IsActionPressed("right_click")) {
            dualGridTilemap.SetCell(MAIN_LAYER, coords, dualGridTilemap.grassPlaceholderAtlasCoord);
        }
    }
    public override void _PhysicsProcess(double delta) {
        GlobalPosition = GetWorldPosTile(GetGlobalMousePosition()) + new Vector2(8, 8);
    }

    public static Vector2I GetWorldPosTile(Vector2 worldPos) {
        int xInt = Mathf.FloorToInt(worldPos.X / 16) * 16;
        int yInt = Mathf.FloorToInt(worldPos.Y / 16) * 16;
        Vector2I result = new(xInt, yInt);
        return result;
    }
}
