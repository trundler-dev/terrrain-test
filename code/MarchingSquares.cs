using Sandbox;
using System.Collections.Generic;

namespace MyGame;

public class MarchingSquares : ModelEntity
{
	public bool[,] TerrainGrid { get; set; }

	public List<Vector3> Vertices { get; set; } = new();
	public List<int> Triangles { get; set; } = new();

	private readonly float localY = 0f;
	private readonly float resolution = 5f;

	public int Width { get; set; }
	public int Height { get; set; }

	public float surfaceLevel { get; set; } = 0.5f;

	public override void Spawn()
	{
		base.Spawn();

		GenerateTerrainGrid();
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();

		GenerateTerrainGrid();
		Draw();
	}

	public void GenerateTerrainGrid()
	{
		TerrainGrid = new bool[Width, Height];

		for ( int x = 0; x < Width; x++ )
			for ( int y = 0; y < Height; y++ )
			{
				TerrainGrid[x, y] = Noise.Simplex( x, y ) > surfaceLevel;
			}
	}

	public void Draw()
	{
		for ( int x = 0; x < TerrainGrid.GetLength( 0 ) - 1; x++ )
			for ( int z = 0; z < TerrainGrid.GetLength( 1 ) - 1; z++ )
			{
				float xRes = x * resolution;
				float zRes = z * resolution;

				var middleTop = new Node( new Vector3( xRes + resolution * 0.5f, localY, zRes ) );
				var middleRight = new Node( new Vector3( xRes + resolution, localY, zRes + resolution * 0.5f ) );
				var middleBottom = new Node( new Vector3( xRes + resolution * 0.5f, localY, zRes + resolution ) );
				var middleLeft = new Node( new Vector3( xRes, localY, zRes + resolution * 0.5f ) );

				var topLeft = new Node( new Vector3( xRes, localY, zRes ) );
				var topRight = new Node( new Vector3( xRes + resolution, localY, zRes ) );
				var bottomRight = new Node( new Vector3( xRes + resolution, localY, zRes + resolution ) );
				var bottomLeft = new Node( new Vector3( xRes, localY, zRes + resolution ) );

				bool c1 = TerrainGrid[x, z];
				bool c2 = TerrainGrid[x + 1, z];
				bool c3 = TerrainGrid[x + 1, z + 1];
				bool c4 = TerrainGrid[x, z + 1];

				int marchCase = GetCase( c1, c2, c3, c4 );

				switch ( marchCase )
				{
					case 1:
						MeshFromPoints( middleBottom, bottomLeft, middleLeft );
						break;
					case 2:
						MeshFromPoints( middleRight, bottomRight, middleBottom );
						break;
					case 4:
						MeshFromPoints( middleTop, topRight, middleRight );
						break;
					case 8:
						MeshFromPoints( topLeft, middleTop, middleLeft );
						break;

					// 2 points:
					case 3:
						MeshFromPoints( middleRight, bottomRight, bottomLeft, middleLeft );
						break;
					case 6:
						MeshFromPoints( middleTop, topRight, bottomRight, middleBottom );
						break;
					case 9:
						MeshFromPoints( topLeft, middleTop, middleBottom, bottomLeft );
						break;
					case 12:
						MeshFromPoints( topLeft, topRight, middleRight, middleLeft );
						break;
					case 5:
						MeshFromPoints( middleTop, topRight, middleRight, middleBottom, bottomLeft, middleLeft );
						break;
					case 10:
						MeshFromPoints( topLeft, middleTop, middleRight, bottomRight, middleBottom, middleLeft );
						break;

					// 3 point:
					case 7:
						MeshFromPoints( middleTop, topRight, bottomRight, bottomLeft, middleLeft );
						break;
					case 11:
						MeshFromPoints( topLeft, middleTop, middleRight, bottomRight, bottomLeft );
						break;
					case 13:
						MeshFromPoints( topLeft, topRight, middleRight, middleBottom, bottomLeft );
						break;
					case 14:
						MeshFromPoints( topLeft, topRight, bottomRight, middleBottom, middleLeft );
						break;

					// 4 point:
					case 15:
						MeshFromPoints( topLeft, topRight, bottomRight, bottomLeft );
						break;
				}
			}

		Log.Info( "Made it past cases" );

		// Convert to Vert struct
		List<Vert> vertList = new List<Vert>();
		foreach ( var vert in Vertices )
		{
			vertList.Add( new Vert( vert ) );
		}

		Log.Info( "Created vertList" );
		Log.Info( $"{Host.Name} :: {vertList.Count}" );

		var mesh = new Mesh( Material.Load( "materials/dev/dev_measuregeneric01.vmat" ) )
		{
			Bounds = new BBox( 0, 5 )
		};
		mesh.CreateVertexBuffer( vertList.Count, Vert.Layout, vertList );
		mesh.CreateIndexBuffer( Triangles.Count, Triangles );

		Log.Info( "Created vertex buffer and index buffer" );

		Model = new ModelBuilder().AddMesh( mesh ).Create();

		Log.Info( "Created and set model" );
	}

	private int GetCase( bool a, bool b, bool c, bool d )
	{
		var num = 0;
		if ( a )
			num += 8;
		if ( b )
			num += 4;
		if ( c )
			num += 2;
		if ( d )
			num += 1;

		return num;
	}

	private void MeshFromPoints( params Node[] points )
	{
		AssignVertices( points );

		if ( points.Length >= 3 )
			CreateTriangle( points[0], points[1], points[2] );
		if ( points.Length >= 4 )
			CreateTriangle( points[0], points[2], points[3] );
		if ( points.Length >= 5 )
			CreateTriangle( points[0], points[3], points[4] );
		if ( points.Length >= 6 )
			CreateTriangle( points[0], points[4], points[5] );
	}

	private void AssignVertices( Node[] points )
	{
		for ( int i = 0; i < points.Length; i++ )
		{
			if ( points[i].VertexIndex == -1 )
			{
				points[i].VertexIndex = Vertices.Count;
				Vertices.Add( points[i].Position );
			}
		}
	}

	private void CreateTriangle( Node a, Node b, Node c )
	{
		Triangles.Add( a.VertexIndex );
		Triangles.Add( b.VertexIndex );
		Triangles.Add( c.VertexIndex );
	}

	private void DrawLine( Node a, Node b )
	{
		DebugOverlay.Line( a.Position, b.Position, 1000f );
	}

	public class Node
	{
		public Vector3 Position { get; set; }
		public int VertexIndex { get; set; } = -1;

		public Node( Vector3 position )
		{
			Position = position;
		}
	}
}
