/*namespace MyGame;

public class QuadTree
{
	public Node Root;
	public int Count;

	public void Insert( Vector2 point, bool value )
	{
		Root = insert( Root, point, value );
		Count++;
	}

	private Node insert( Node current, Vector2 point, bool value )
	{
		if ( current == null )
		{
			return new Node( point, value );
		}

		// SW
		if ( point.x < current.Point.x && point.y < current.Point.y )
		{
			current.SW = insert( current.SW, point, value );
		}

		// NW
		if ( point.x < current.Point.x && point.y >= current.Point.y )
		{
			current.NW = insert( current.NW, point, value );
		}

		// NE
		if ( point.x > current.Point.x && point.y >= current.Point.y )
		{
			current.NE = insert( current.NE, point, value );
		}

		// SE
		if ( point.x > current.Point.x && point.y < current.Point.y )
		{
			current.SE = insert( current.SE, point, value );
		}

		return current;
	}
}

public class Node
{
	public Vector2 Point;
	public Node NW, NE, SW, SE;
	public bool Value;
	public bool IsDeleted;

	public Node( Vector2 point, bool value )
	{
		Point = point;
		Value = value;
	}
}
*/
