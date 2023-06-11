using Godot;
using System;

public partial class Geometry : RefCounted
{
	private struct Triangle
	{
		public Vector2[] Verticies = new Vector2[3];

		public Triangle(Vector2[] verticies)
		{
			this.Verticies = verticies;
		}

		public float GetArea()
		{
			float a = Verticies[0].DistanceTo(Verticies[1]);
			float b = Verticies[1].DistanceTo(Verticies[2]);
			float c = Verticies[2].DistanceTo(Verticies[0]);

			float s = (a + b + c) / 2.0f;
			return MathF.Sqrt(s * (s - a) * (s - b) * (s - c));
		}

		public Vector2 GetRandomPoint()
		{
			Random rng = new Random();

			float r1 = MathF.Sqrt(rng.NextSingle());
			float r2 = rng.NextSingle();
			float m1 = 1.0f - r1;
			float m2 = r1 * (1.0f - r2);
			float m3 = r2 * r1;

			Vector2 p1 = this.Verticies[0];
			Vector2 p2 = this.Verticies[1];
			Vector2 p3 = this.Verticies[2];

			return (m1 * p1) + (m2 * p2) + (m3 * p3);
		}
	}

	public Vector2[] GetRandomPointsInPolygon(Vector2[] verticies, int[] indices, int amount)
	{        
		Vector2[] points = new Vector2[amount];

		Triangle[] triangles = VerticiesToTriangles(verticies, indices);
		float area = 0.0f;

		foreach (Triangle triangle in triangles)
		{
			area += triangle.GetArea();
		}

		for (int i = 0; i < amount; i++)
		{
			points[i] = PickRandomTriangle(triangles, area).GetRandomPoint();
		}

		return points;
	}

	private Triangle PickRandomTriangle(Triangle[] triangles, float area)
	{
		float rng = new Random().NextSingle() * area;

		foreach (Triangle triangle in triangles)
		{
			float triangleArea = triangle.GetArea();

			if (rng < triangleArea)
			{
				return triangle;
			}

			rng -= triangleArea;
		}

		return triangles[triangles.Length - 1];
	}
	
	private Triangle[] VerticiesToTriangles(Vector2[] verticies, int[] indices)
	{
		Triangle[] triangles = new Triangle[indices.Length / 3];

		for (int i = 0; i < indices.Length / 3; i++)
		{
			int index = i * 3;

			Vector2[] triangleVerticies = new Vector2[3];

			triangleVerticies[0] = verticies[indices[index]];
			triangleVerticies[1] = verticies[indices[index + 1]];
			triangleVerticies[2] = verticies[indices[index + 2]];

			Triangle triangle = new Triangle(triangleVerticies);

			triangles[i] = triangle;
		}

		return triangles;
	}
}
