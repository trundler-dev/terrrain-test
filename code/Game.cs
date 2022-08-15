using MyGame;
using System;
using System.Linq;

namespace Sandbox;

public partial class MyGame : Game
{
	public MarchingSquares MarchingSquares { get; set; }
	public int width = 100, height = 100;
	public int cubeSize = 5;

	[Net] public bool Lock { get; set; } = false;

	public MyGame()
	{
		MarchingSquares = new MarchingSquares();
		MarchingSquares.Width = width;
		MarchingSquares.Height = height;
		MarchingSquares.GenerateTerrainGrid();
	}

	/// <summary>
	/// A client has joined the server. Make them a pawn to play with
	/// </summary>
	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );

		// Create a pawn for this client to play with
		var pawn = new Pawn();
		client.Pawn = pawn;

		// Get all of the spawnpoints
		var spawnpoints = Entity.All.OfType<SpawnPoint>();

		// chose a random one
		var randomSpawnPoint = spawnpoints.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

		// if it exists, place the pawn there
		if ( randomSpawnPoint != null )
		{
			var tx = randomSpawnPoint.Transform;
			tx.Position = tx.Position + Vector3.Up * 50.0f; // raise it up
			pawn.Transform = tx;
		}
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		// Map.Display();
		// Map.DisplayExposed( 100 );

		/*		if ( Input.Pressed( InputButton.Jump ) )
				{
					for ( int i = 0; i < width; i++ )
					{
						for ( int j = 0; j < height; j++ )
						{
							if ( Map.TerrainGrid[i, j] )
							{
								Cube cube = new Cube();
								cube.Position = new Vector3( i * cubeSize, 0, j * cubeSize );
							}
						}
					}
				}*/



		if ( Input.Pressed( InputButton.SecondaryAttack ) && !Lock )
		{
			Lock = true;
			// marching squares draw debug lines
			MarchingSquares.Draw();
		}
	}

	public override void PostLevelLoaded()
	{
		base.PostLevelLoaded();

		/*		Map.FillWithNoise();
				// fill marching squares with data
				MarchingSquares.TerrainGrid = Map.TerrainGrid;*/
	}
}
