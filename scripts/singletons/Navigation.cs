namespace SharpGame;

public partial class Navigation : Node
{
    AStarGrid2D _aStar = new();

    public override void _Ready()
    {
        _aStar.DiagonalMode = AStarGrid2D.DiagonalModeEnum.OnlyIfNoObstacles;
        _aStar.JumpingEnabled = true;
    }

    Vector2I GetId(Vector2 position)
    {
        Vector2I cell = (Vector2I)((position - _aStar.Offset) / _aStar.CellSize).Round();
        return cell.Clamp(_aStar.Region.Position, _aStar.Region.End);
    }

    public Godot.Collections.Array<Vector2> GetPath(Vector2 from, Vector2 to)
    {
        // Return null if the path is outside the tilemaps region
        if (to.X > _aStar.Region.Size.X || to.Y > _aStar.Region.Size.Y)
            return null;

        Godot.Collections.Array<Vector2> path = new();

        foreach (Vector2I cell in _aStar.GetIdPath(GetId(from), GetId(to)))
        {
            path.Add(_aStar.GetPointPosition(cell));
        }

        return path;
    }

    public Godot.Collections.Array<Vector2I> GetIdPath(Vector2I fromId, Vector2I toId) => 
        _aStar.GetIdPath(fromId, toId);

    public void SetTileMap(TileMap tileMap)
    {
        Vector2 cellSize = new(tileMap.CellQuadrantSize, tileMap.CellQuadrantSize);

        _aStar.CellSize = cellSize;
        _aStar.Offset = cellSize / 2; // Make point the center of a cell
        _aStar.Region = tileMap.GetUsedRect();
        _aStar.Update();

        foreach (Vector2I cell in tileMap.GetUsedCells(0))
        {
            _aStar.SetPointSolid(cell, true);
        }
    }
}
