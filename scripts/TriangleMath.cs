using Godot;
using System;

public partial class TriangleMath : RefCounted
{
	public struct Triangle
	{
		public Vector2 A;
		public Vector2 B;
		public Vector2 C;

		public Triangle(Vector2 a, Vector2 b, Vector2 c)
		{
			this.A = a;
			this.B = b;
			this.C = c;
		}

		public float GetArea()
		{
			float a = A.DistanceTo(B);
			float b = B.DistanceTo(C);
			float c = C.DistanceTo(A);

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

			return (m1 * A) + (m2 * B) + (m3 * C);
		}
	}

	public float GetTrianglesArea(Triangle[] triangles)
	{
		float area = 0.0f;

		foreach (Triangle triangle in triangles)
		{
			area += triangle.GetArea();
		}

		return area;
	}

	// Pick random triangle with higher probability of choosing bigger one.
	// Useful for even distribution of points.
	public Triangle PickRandomTriangle(Triangle[] triangles, float area)
	{
		float rng = new Random().NextSingle() * area;

		foreach (Triangle triangle in triangles)
		{
			float triangleArea = triangle.GetArea();

			if (rng < triangleArea)
				return triangle;

			rng -= triangleArea;
		}

		return triangles[triangles.Length - 1];
	}
	
	public Triangle[] GetTriangles(Vector2[] polygon, int[] indices)
	{
		int triangleSize = indices.Length / 3;
		Triangle[] triangles = new Triangle[triangleSize];

		for (int i = 0; i < triangleSize; i++)
		{
			int idx = i * 3;

			Vector2 a = polygon[indices[idx]];
			Vector2 b = polygon[indices[idx + 1]];
			Vector2 c = polygon[indices[idx + 2]];

			Triangle triangle = new Triangle(a, b, c);

			triangles[i] = triangle;
		}

		return triangles;
	}
}
