namespace MyGame;

public class Map
{
	public float[,] NoiseGrid { get; }
	public bool[,] TerrainGrid { get; }

	int Width;
	int Height;

	float surfaceLevel = 0.5f;

	public Map( int width, int height )
	{
		NoiseGrid = new float[width, height];
		TerrainGrid = new bool[width, height];
		Width = width;
		Height = height;
	}

	public void FillWithNoise()
	{
		for ( int x = 0; x < Width; x++ )
			for ( int y = 0; y < Height; y++ )
			{
				NoiseGrid[x, y] = Noise.Simplex( x, y );
				TerrainGrid[x, y] = NoiseGrid[x, y] > surfaceLevel;
			}
	}

	public void Display()
	{
		if ( NoiseGrid.Length == 0 )
			return;

		for ( int x = 0; x < Width; x++ )
			for ( int y = 0; y < Height; y++ )
			{
				if ( NoiseGrid[x, y] > surfaceLevel )
				{
					DebugOverlay.Line( new Vector3( x, y ), new Vector3( x, y ) + Vector3.Up * 20f );
				}
			}
	}

	public void DisplayExposed( int offset )
	{
		if ( NoiseGrid.Length == 0 )
			return;

		for ( int x = 0; x < Width; x++ )
			for ( int y = 0; y < Height; y++ )
			{
				if ( NoiseGrid[x, y] > 0.5f )
				{
					bool exposed = CheckNeighbours( x, y );
					if ( exposed )
					{
						DebugOverlay.Line( new Vector3( x + offset, y + offset ), new Vector3( x + offset, y + offset ) + Vector3.Up * 20f );
					}
					TerrainGrid[x, y] = exposed;
				}
			}
	}

	// Returns true if neighbouring a point under the threshold
	private bool CheckNeighbours( int x, int y )
	{
		bool checkLeft = false, checkRight = false, checkTop = false, checkBottom = false;

		if ( x - 1 >= 0 )
		{
			checkLeft = true;
		}

		if ( x + 1 <= Width - 1 )
		{
			checkRight = true;
		}

		if ( y - 1 >= 0 )
		{
			checkTop = true;
		}

		if ( y + 1 <= Height - 1 )
		{
			checkBottom = true;
		}

		if ( checkLeft && NoiseGrid[x - 1, y] <= surfaceLevel )
		{
			return true;
		}

		if ( checkRight && NoiseGrid[x + 1, y] <= surfaceLevel )
		{
			return true;
		}

		if ( checkTop && NoiseGrid[x, y - 1] <= surfaceLevel )
		{
			return true;
		}

		if ( checkBottom && NoiseGrid[x, y + 1] <= surfaceLevel )
		{
			return true;
		}

		return false;
	}
}
