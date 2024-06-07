using Godot;

public partial class ToggleGrid : CheckButton {
    [Export] Sprite2D gridSprite;
    public void _on_toggled(bool toggledOn) {
        gridSprite.Visible = toggledOn;
    }
}
