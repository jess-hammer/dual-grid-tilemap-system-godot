using Godot;

public partial class FollowCursor : Node2D {
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
