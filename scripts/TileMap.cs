using Godot;
using System;

public partial class TileMap : Godot.TileMap
{
    private Navigation _navigation;

    public override void _Ready()
    {
        _navigation = GetNode<Navigation>("/root/Navigation");
        _navigation.SetTileMap(this);
    }
}
