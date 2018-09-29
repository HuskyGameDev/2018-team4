using System.Collections.Generic;
using UnityEngine;

public static class Extensions  {

	#region LIST
	private static System.Random rng = new System.Random();
	public static void Shuffle<T>(this List<T> list) {
		int n = list.Count;
		while (n > 1) {
			n--;
			int k = rng.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
	#endregion

	#region HexCoordinate
	public static HexCoordinate.Direction InverseDirection(this HexCoordinate.Direction dir) {
		return HexCoordinate.inverseDirection[(int)dir];
	}

	public static Vector2Int Offset(this HexCoordinate.Direction dir) {
		return HexCoordinate.offset[(int)dir];
	}

	public static string GetName(this HexCoordinate.Direction dir) {
		return HexCoordinate.directionNames[(int)dir];
	}
	#endregion
}
