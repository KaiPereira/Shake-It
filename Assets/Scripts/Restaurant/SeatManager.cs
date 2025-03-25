using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SeatManager : MonoBehaviour
{
	private Tilemap tilemap;

	private List<Vector3> availableSeats = new List<Vector3>();

	// Start is called before the first frame update
	void Start()
	{
		tilemap = GameObject.Find("Furnishing").GetComponent<Tilemap>();

		FindAllSeats();
	}
	
	public void FindAllSeats()
	{
		availableSeats.Clear();

		BoundsInt bounds = tilemap.cellBounds;
		TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

		for (int x = bounds.xMin; x < bounds.xMax; x++) {
			for (int y = bounds.yMin; y < bounds.yMax; y++) {
				Vector3Int tilePosition = new Vector3Int(x, y, 0);

				TileBase tile = tilemap.GetTile(tilePosition);

				if (tile != null && tile.ToString().Contains("seat"))
				{
					Vector3 worldPos = tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 1f, 0);
					availableSeats.Add(worldPos);
				}
			}
		}
	}

	public Vector3 GetNextAvailableSeat()
	{
		if (availableSeats.Count == 0) return Vector3.zero;

		Vector3 seat = availableSeats[0];
		availableSeats.RemoveAt(0);

		return seat;
	}
}
