//Switches between pointed and flattop system. (By commenting this line out)
//#define POINTED

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// HexCoordinate manages the necessary mathmatics for a Hexagon based coordinate system.
/// </summary>
[System.Serializable]
public class HexCoordinate {

#region Values
	#region Orientation
	/// <summary>
	/// The number of directions of a Hexagon
	/// </summary>
	public const int directionCount = 6;
#if POINTED
	/// <summary>
	/// A boolean that tracks if the hexagon is Pointy
	/// </summary>
	public static readonly bool Pointed = true;

	/// <summary>
	/// An enumeration of the neighboring Hexagon Directions
	/// </summary>
	public enum Direction { UpperRight = 0, Right = 1, LowerRight = 2, LowerLeft = 3, Left = 4, UpperLeft = 5 }

	/// <summary>
	/// The offsets for neighboring hexagons
	/// </summary>
	public static readonly Vector2Int[] offset = new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(-1, 1) };

	/// <summary>
	/// The string names of each Hexagon
	/// </summary>
	public static readonly string[] directionNames = new string[] { "UpperRight", "Right", "LowerRight", "LowerLeft", "Left", "UpperLeft"};

	/// <summary>
	/// The inverse Direction for each Hexagonal Direction
	/// </summary>
	public static readonly Direction[] inverseDirection = new Direction[] { Direction.LowerLeft, Direction.Left, Direction.UpperLeft, Direction.UpperRight, Direction.Right, Direction.LowerRight};

	/// <summary>
	/// An enumeration of Directions that when traversed in sequence result in returning to the beginning in a circular pattern. Offset but circularTraversal[4] to create a circle around a specific point.
	/// </summary>
	public static readonly Direction[] circularTraversal = new Direction[] { Direction.LowerRight, Direction.LowerLeft, Direction.Left, Direction.UpperLeft, Direction.UpperRight, Direction.Right };

#else
	/// <summary>
	/// A boolean that tracks if the hexagon is Pointy
	/// </summary>
	public static readonly bool Pointed = false;
	
	/// <summary>
	/// An enumeration of the neighboring Hexagon Directions
	/// </summary>
	public enum Direction { UpperRight = 0, LowerRight = 1, Bottom = 2, LowerLeft = 3, UpperLeft = 4, Top = 5}
	
	/// <summary>
	/// The offsets for neighboring hexagons
	/// </summary>
	public static readonly Vector2Int[] offset = new Vector2Int[] { new Vector2Int(1,0), new Vector2Int(1,-1), new Vector2Int(0,-1), new Vector2Int(-1,0), new Vector2Int(-1,1), new Vector2Int(0, 1) };
	
	/// <summary>
	/// The string names of each Hexagon
	/// </summary>
	public static readonly string[] directionNames = new string[] { "UpperRight", "LowerRight", "Bottom", "LowerLeft", "UpperLeft", "Top"};
	
	/// <summary>
	/// The inverse Direction for each Hexagonal Direction
	/// </summary>
	public static readonly Direction[] inverseDirection = new Direction[] { Direction.LowerLeft, Direction.UpperLeft, Direction.Top, Direction.UpperRight, Direction.LowerRight, Direction.Bottom};

	/// <summary>
	/// An enumeration of Directions that when traversed in sequence result in returning to the beginning in a circular pattern. Offset but circularTraversal[4] to create a circle around a specific point.
	/// </summary>
	public static readonly Direction[] circularTraversal = new Direction[] { Direction.Bottom, Direction.LowerLeft, Direction.UpperLeft, Direction.Top, Direction.UpperRight, Direction.LowerRight };
#endif

	//Due to the ordering of the enums, this remains consistent 
	#endregion

	#region Dimensions
	/// <summary>
	/// The Square root of three, stored to allow for its use in other constants.
	/// </summary>
	private const float sqrt3 = 1.732050807568877293527446341505872366942805253810380628f;

	/// <summary>
	/// The size of a hexgon.
	/// </summary>
	public static readonly float hexSize = 1.0f;

	/// <summary>
	/// The Width of a hexagon of hexSize
	/// </summary>
	public static float Width {
		get {
#if POINTED
			return sqrt3 * hexSize;
#else
			return 2 * hexSize;
#endif
		}
	}

	/// <summary>
	/// The Height of a hexagon hexSize
	/// </summary>
	public static float Height {
		get {
#if POINTED
			return 2 * hexSize;
#else
			return sqrt3 * hexSize;
#endif
		}
	}


	/// <summary>
	/// An enumeration of the points of a hexagon, arranged so that each point is oriented to the left of the associated Direction
	/// </summary>
	public static readonly Vector2[] points = new Vector2[] {
		#if POINTED
		new Vector2(0.0f, hexSize),
		new Vector2((hexSize * sqrt3) / 2.0f, hexSize * 0.5f),
		new Vector2((hexSize * sqrt3) / 2.0f, -hexSize * 0.5f),
		new Vector2(0.0f, -hexSize),
		new Vector2(-(hexSize * sqrt3) / 2.0f, -hexSize * 0.5f),
		new Vector2(-(hexSize * sqrt3) / 2.0f, hexSize * 0.5f)
		#else
		new Vector2(hexSize * 0.5f, (hexSize * sqrt3) / 2.0f),
		new Vector2(hexSize, 0.0f),
		new Vector2(hexSize * 0.5f, -(hexSize * sqrt3) / 2.0f),
		new Vector2(-hexSize * -0.5f, (hexSize * sqrt3) / 2.0f),
		new Vector2(-hexSize, 0.0f),
		new Vector2(-hexSize * 0.5f, (hexSize * sqrt3) / 2.0f)
		#endif
	};

	/// <summary>
	/// The faces on a hexagon shape, arranged so that each face is oriented along the associated Direction
	/// </summary>
	public static readonly Vector2[] faces = new Vector2[] {
		#if POINTED
		new Vector2((hexSize * sqrt3) / 4.0f, hexSize * 0.75f),
		new Vector2(hexSize * sqrt3 * 0.5f, 0.0f),
		new Vector2((hexSize * sqrt3) / 4.0f, -hexSize * 0.75f),
		new Vector2(-(hexSize * sqrt3) / 4.0f, -hexSize * 0.75f),
		new Vector2(-hexSize * sqrt3 * 0.5f, 0.0f),
		new Vector2(-(hexSize * sqrt3) / 4.0f, hexSize * 0.75f)

		#else
		new Vector2(hexSize * 0.75f, (hexSize * sqrt3) / 4.0f),
		new Vector2(hexSize * 0.75f, -(hexSize * sqrt3) / 4.0f),
		new Vector2(0.0f, -hexSize * sqrt3),
		new Vector2(-hexSize * 0.75f, (hexSize * sqrt3) / 4.0f),
		new Vector2(-hexSize * 0.75f, -(hexSize * sqrt3) / 4.0f),
		new Vector2(0.0f, hexSize * sqrt3)
		#endif
	};

#endregion

	#region Cooridnates
	/// <summary>
	/// The position on the hexagonal x-axis
	/// </summary>
	public int X;

	/// <summary>
	/// The position on the hexagonal y-axis
	/// </summary>
	public int Y;

	/// <summary>
	/// The position on the hexagonal x-axis (Calculated)
	/// </summary>
	public int Z {
		get {
			return -X - Y;
		}
	}
	#endregion
#endregion

#region Constructors
	/// <summary>
	/// Creates a HexCoordinate at position (0,0)
	/// </summary>
	public HexCoordinate() {
		this.X = 0;
		this.Y = 0;
	}

	/// <summary>
	/// Creates a HexCoordinate at position (x,y)
	/// </summary>
	public HexCoordinate(int X, int Y) {
		this.X = X;
		this.Y = Y;
	}

	/// <summary>
	/// Create a HexCoordinate at the position specified by a Vector2
	/// </summary>
	/// <param name="vector"></param>
	public HexCoordinate(Vector2Int vector) {
		this.X = vector.x;
		this.Y = vector.y;
	}
#endregion

#region Methods
	#region Public

	/// <summary>
	/// Calculates the world position of the center of a hexagon at a HexCoordinate
	/// </summary>
	/// <param name="hex"></param>
	/// <returns></returns>
	public static Vector3 GetWorldPositionFromHex(HexCoordinate hex) {
		Vector3 returnPos;
	#if POINTED
		float x = hexSize * (sqrt3 * hex.X + sqrt3 / 2.0f * hex.Y);
		float y = hexSize * (1.5f * hex.Y);
	#else
		float x = hexSize * (1.5f * hex.X);
		float y = hexSize * (sqrt3 * hex.Y + sqrt3 / 2.0f * hex.X);
	#endif
		returnPos = new Vector3(x, y, 0.0f);
		return returnPos;
	}

	/// <summary>
	/// Calculates the position of a hexagon containing a specified World coordinate
	/// </summary>
	/// <param name="point"></param>
	/// <returns></returns>
	public static HexCoordinate GetHexPositionFromWorld(Vector2 point) {
	#if POINTED
		float x = (sqrt3 / 3.0f * point.x - 1.0f / 3 * point.y) / hexSize;
		float y = (2.0f / 3 * point.y) / hexSize;
	#else
		float x = (2.0f / 3 * point.x) / hexSize;
		float y = (-1.0f/3 * point.x + sqrt3/3.0f * point.y) / hexSize;
	#endif
		return HexRound(x, y);
	}

	/// <summary>
	/// Calculates the distance between two HexCoordinates
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static int Distance(HexCoordinate a, HexCoordinate b) {
		return (a-b).Magnitude();
	}

	/// <summary>
	/// Calculates the Vector Magnitude of a HexCoordinate;
	/// </summary>
	/// <returns></returns>
	public int Magnitude() {
		return (Mathf.Abs(X)+ Mathf.Abs(Y) + Mathf.Abs(Z)) / 2;
	}

	public static Direction? IsNeighbor(HexCoordinate a, HexCoordinate b) {
		for (int i = 0; i < offset.Length; i++) {
			if (a + offset[i] == b) {
				return (Direction)i;
			}
		}
		return null;
	}

	/// <summary>
	/// Creats an ordered list of coordinates for a Line between a pair of HexCorrdinates.
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static List<HexCoordinate> FindLine(HexCoordinate a, HexCoordinate b) {
		List<HexCoordinate> orderedReturnCoordinates = new List<HexCoordinate>();
		int N = Distance(a,b);

		Vector2 a_Nudge = new Vector2(a.X + 1e-6f, a.Y + 1e-6f);
		Vector2 b_Nudge = new Vector2(b.X + 1e-6f, b.Y + 1e-6f);

		float step = 1.0f / (float)Mathf.Max(N, 1);

		for (int i = 0; i <= N; i++) {
			orderedReturnCoordinates.Add(HexRound(Mathf.Lerp(a_Nudge.x, b_Nudge.x, step * i), Mathf.Lerp(a_Nudge.y, b_Nudge.y, step * i)));
		}

		return orderedReturnCoordinates;
	}

	/// <summary>
	/// Creates an unordered list of coordinates for a Ring around the specified HexCoordinate at the specified distance
	/// </summary>
	/// <param name="center"></param>
	/// <param name="distance"></param>
	public static List<HexCoordinate> GenerateRing(HexCoordinate center, int distance) {
		List<HexCoordinate> returnCoordinates = new List<HexCoordinate>();
		//If distance is 0, the 'ring' is just the center
		if (distance == 0) {
			returnCoordinates.Add(center);
			return returnCoordinates;
		}
		HexCoordinate pos = center + (circularTraversal[4].Offset() * distance);

		for (int i = 0; i < directionCount; i++) {
			for (int r = 0; r < distance; r++) {
				returnCoordinates.Add(pos);
				pos = pos + (circularTraversal[i]).Offset();
			}
		}

		return returnCoordinates;
	}

	/// <summary>
	/// Creates an unordered list of coordinates for a Circle shape around a center point
	/// </summary>
	/// <param name="radius"></param>
	/// <returns></returns>
	public static List<HexCoordinate> GenerateCircle(HexCoordinate center, int radius) {
		List<HexCoordinate> returnCoordinates = new List<HexCoordinate>();
		//Loop through all possible coordinats in the circle.
		for (int x = -radius; x <= radius; x++) {
			for (int y = -radius; y <= radius; y++) {
				//Ignore any coordinate that has a linear distance to far away from the center
				if (HexCoordinate.Distance(new HexCoordinate(0, 0), new HexCoordinate(x, y)) > radius) continue;
				returnCoordinates.Add(center + new HexCoordinate(x, y));
			}
		}
		return returnCoordinates;
	}

	/// <summary>
	/// Creates an unordered list of coordinates for a square shape around a center point
	/// </summary>
	/// <param name="xDim"></param>
	/// <param name="yDim"></param>
	/// <returns></returns>
	public static List<HexCoordinate> GenerateSquare(HexCoordinate center, int xDim, int yDim) {
		List<HexCoordinate> returnCoordinates = new List<HexCoordinate>();

	#if POINTED
		for (int y = -yDim; y <= yDim; y++) {
			int y_offset = Mathf.FloorToInt(y / 2.0f);
			for (int x = -y_offset - xDim; x <= xDim - y_offset; x++) {
				returnCoordinates.Add(center + new HexCoordinate(x, y));
			}
		}
	#else
		for (int x = -xDim; x <= xDim; x++) {
			int x_offset = Mathf.FloorToInt(x / 2.0f);
			for (int y = -x_offset - yDim; y <= yDim - x_offset; y++) {
				returnCoordinates.Add(center + new HexCoordinate(x,y));
			}
		}
	#endif

		return returnCoordinates;
	}

    /// <summary>
    /// return the direction of the offset
    /// </summary>
    /// <param name="_offset"></param>
    /// <returns></returns>
    public static Direction GetDirectionFromOffset(Vector2Int _offset)
    {
        for(int i = 0; i < offset.Length; i++)
        {
            if (offset[i] == _offset)
            {
                return (Direction)i;
            }
        }
        Debug.Log("Invalid Offset");
        return (Direction)0;
    }
	#endregion

	#region Private
	/// <summary>
	/// Rounds to an int Hex Coordinate. Used for returning HexCoordinates from algroithms that work on a non integer basis.
	/// </summary>
	/// <param name="q"></param>
	/// <param name="r"></param>
	/// <returns></returns>
	private static HexCoordinate HexRound(float x, float y) {
		//The following operates on Cube coordinates, so create the third coordinate.
		float z = -x - y;

		int rx = Mathf.RoundToInt(x);
		int ry = Mathf.RoundToInt(y);
		int rz = Mathf.RoundToInt(z);

		float q_diff = Mathf.Abs(rx - x);
		float r_diff = Mathf.Abs(ry - y);
		float s_diff = Mathf.Abs(rz - z);

		if (q_diff > r_diff && q_diff > s_diff) {
			rx = -ry - rz;
		}
		else if (r_diff > s_diff) {
			ry = -rx - rz;
		}
		else {
			rz = -rx - ry;
		}

		return new HexCoordinate(rx, ry);
	}
	#endregion

	#region Override
		#region Operators

	/// <summary>
	/// Override the Addition Operator
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static HexCoordinate operator +(HexCoordinate a, HexCoordinate b) {
		return new HexCoordinate(a.X + b.X, a.Y + b.Y);
	}

	/// <summary>
	/// Override the Subtaction Operator
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static HexCoordinate operator -(HexCoordinate a, HexCoordinate b) {
		return new HexCoordinate(a.X - b.X, a.Y - b.Y);
	}

	/// <summary>
	/// Override the Addition operator with Vector2Int
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static HexCoordinate operator +(HexCoordinate a, Vector2Int b) {
		return new HexCoordinate(a.X + b.x, a.Y + b.y);
	}

	/// <summary>
	/// Override the Subtraction operator with Vector2int
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static HexCoordinate operator -(HexCoordinate a, Vector2Int b) {
		return new HexCoordinate(a.X - b.x, a.Y - b.y);
	}

	/// <summary>
	/// Override the equality operator
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static bool operator ==(HexCoordinate a, HexCoordinate b) {
		if (object.ReferenceEquals(a, null)) {
			return object.ReferenceEquals(b, null);
		}

		return a.Equals(b);
	}

	/// <summary>
	/// Override the inequality operator
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static bool operator !=(HexCoordinate a, HexCoordinate b) {
		return !(a == b);
	}

		#endregion
		#region Object

	/// <summary>
	/// Rough hascode generated by converting to a string based on the coordinates and then using its hash code.
	/// </summary>
	/// <returns></returns>
	public override int GetHashCode() {
		return string.Format("{0}-{1}-{2}", X, Y, Z).GetHashCode();
	}

	/// <summary>
	/// Override the Equals method.
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	public override bool Equals(object obj) {
		if (obj == null)
			return false;
		else if (obj.GetType() == typeof(HexCoordinate)) {
			HexCoordinate b = (HexCoordinate)obj;
			return (X == b.X && Y == b.Y && Z == b.Z);
		}
		else
			return false;
	}

	/// <summary>
	/// Convert the HexCoordinate into a readable string representation.
	/// </summary>
	/// <returns></returns>
	public override string ToString() {
		return string.Format("({0},{1})", X, Y);
	}

		#endregion
	#endregion

#endregion

}
