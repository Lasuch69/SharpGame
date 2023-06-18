using Godot;
using System;

public partial class Navigation : Node
{
	private AStarGrid2D _aStar = new ();
	private Vector2I _offset = new ();

	public override void _Ready()
	{
		_aStar.DiagonalMode = AStarGrid2D.DiagonalModeEnum.OnlyIfNoObstacles;
		_aStar.JumpingEnabled = true;
	}

	public Vector2 GetPosition(Vector2I id)
	{
		return (id * _aStar.CellSize) + _aStar.CellSize / 2;
	}

	public Vector2I GetId(Vector2 position)
	{
		return (Vector2I)((position - _aStar.CellSize / 2) / _aStar.CellSize).Round();
	}

	public Godot.Collections.Array<Vector2> GetPath(Vector2 from, Vector2 to)
	{
		var pathId = GetIdPath(GetId(from), GetId(to));
		Godot.Collections.Array<Vector2> path = new ();
		path.Resize(pathId.Count);

		for (int i = 0; i < path.Count; i++)
		{
			path[i] = GetPosition(pathId[i]);
		}

		return path;
	}

	public Godot.Collections.Array<Vector2I> GetIdPath(Vector2I fromId, Vector2I toId)
	{
		var path = _aStar.GetIdPath(ToLocal(fromId), ToLocal(toId));

		for (int i = 0; i < path.Count; i++)
		{
			path[i] = ToGlobal(path[i]);
		}

		return path;
	}

	public void SetTileMap(TileMap tileMap)
	{
		Rect2I map = tileMap.GetUsedRect();	
		int cellSize = tileMap.CellQuadrantSize;

		_aStar.CellSize = new Vector2(cellSize, cellSize);
		_aStar.Size = map.Size;
		_aStar.Update();
		
		_offset = map.Position;

		Godot.Collections.Array<Vector2I> usedCells = tileMap.GetUsedCells(0);

		foreach (Vector2I cell in usedCells)
		{
			_aStar.SetPointSolid(ToLocal(cell), true);
		}
	}

	private Vector2I ToLocal(Vector2I id)
	{
		return id - _offset;
	}

	private Vector2I ToGlobal(Vector2I id)
	{
		return id + _offset;
	}
}
