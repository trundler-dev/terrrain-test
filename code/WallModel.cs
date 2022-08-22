using Sandbox;
using System.Collections.Generic;

namespace MyGame;

public class WallModel : ModelEntity
{
	public List<List<int>> Outlines = new();
	public List<Vector3> Vertices { get; set; } = new();
	public List<int> Triangles { get; set; } = new();

	public int Width { get; set; }
	public int Height { get; set; }

	private readonly float resolution = 5f;

	public void CreateWallMesh()
	{
		List<Vector3> wallVertices = new();
		List<int> wallTriangles = new();

		float wallHeight = 50f;

		Mesh wallMesh = new Mesh( Material.Load( "materials/dev/dev_measuregeneric01.vmat" ) )
		{
			Bounds = new BBox( 0, new Vector3( Width * resolution, wallHeight, Height * resolution ) )
		};

		foreach ( List<int> outline in Outlines )
		{
			for ( int i = 0; i < outline.Count - 1; i++ )
			{
				int startIndex = wallVertices.Count;
				wallVertices.Add( Vertices[outline[i]] );
				wallVertices.Add( Vertices[outline[i + 1]] );
				wallVertices.Add( Vertices[outline[i]] - Vector3.Right * wallHeight );
				wallVertices.Add( Vertices[outline[i + 1]] - Vector3.Right * wallHeight );

				wallTriangles.Add( startIndex + 0 );
				wallTriangles.Add( startIndex + 2 );
				wallTriangles.Add( startIndex + 3 );

				wallTriangles.Add( startIndex + 3 );
				wallTriangles.Add( startIndex + 1 );
				wallTriangles.Add( startIndex + 0 );
			}
		}

		var vertList = new List<Vert>();
		foreach ( var vert in wallVertices )
		{
			vertList.Add( new Vert( vert ) );
		}

		wallMesh.CreateVertexBuffer( vertList.Count, Vert.Layout, vertList );
		wallMesh.CreateIndexBuffer( wallTriangles.Count, wallTriangles );

		var builder = new ModelBuilder();

		if ( IsClient )
		{
			builder.AddMesh( wallMesh );
		}
		else
		{
			builder.AddCollisionMesh( wallVertices.ToArray(), wallTriangles.ToArray() );
		}

		Model = builder.Create();
	}
}
