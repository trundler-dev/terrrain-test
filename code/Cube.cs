using Sandbox;
using System.Collections.Generic;

namespace MyGame;

public class Cube : ModelEntity
{

	public override void Spawn()
	{
		base.Spawn();

		if ( IsServer )
			SetupCollision();
	}

	public override void ClientSpawn()
	{
		base.Spawn();

		CreateModel();
	}

	private void SetupCollision()
	{
		SetupPhysicsFromOBB( PhysicsMotionType.Static, Position, Position + 5f );
	}

	private void CreateModel()
	{
		var w = 5;

		var mesh = new Mesh( Material.Load( "materials/dev/dev_measuregeneric01.vmat" ) )
		{
			Bounds = new BBox( 0, 5 )
		};

		List<Vert> Vertices = new List<Vert>()
		{
			new Vert(new Vector3(0, 0, w)),
			new Vert(new Vector3(0, w, w)),
			new Vert(new Vector3(w, w, w)),
			new Vert(new Vector3(w, 0, w)),
			new Vert(new Vector3(0, 0, 0)),
			new Vert(new Vector3(0, w, 0)),
			new Vert(new Vector3(w, w, 0)),
			new Vert(new Vector3(w, 0, 0)),
		};

		mesh.CreateVertexBuffer( 8, Vert.Layout, Vertices );
		mesh.SetVertexRange( 0, Vertices.Count );
		mesh.CreateIndexBuffer( BlockIndices.Length, BlockIndices );

		Model = new ModelBuilder().AddMesh( mesh ).Create();
	}

	static readonly int[] BlockIndices = new[]
	{
		2, 1, 0, 0, 3, 2,
		5, 6, 7, 7, 4, 5,
		4, 7, 3, 3, 0, 4,
		6, 5, 1, 1, 2, 6,
		5, 4, 0, 0, 1, 5,
		7, 6, 2, 2, 3, 7,
	};
}

public struct Vert
{
	public Vector3 position;

	public Vert( Vector3 position )
	{
		this.position = position;
	}

	public static readonly VertexAttribute[] Layout = new VertexAttribute[1]
	{
		new VertexAttribute(VertexAttributeType.Position, VertexAttributeFormat.Float32),
	};
}
