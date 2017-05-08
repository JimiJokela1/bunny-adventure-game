using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
	public string tileName;
	public Vector3 tilePosition; //Pos in world coordinates
	public int tileSizeX; //tile sizes
	public int tileSizeZ;
	public int tileSizeY;
	public Material tileMaterial; //tile material

	/// <summary>
	/// Initializes a new instance of the Tile class.
	/// </summary>
	/// <param name="tileName">Tile name.</param>
	/// <param name="tileSizeX">Tile size x.</param>
	/// <param name="tileSizeZ">Tile size z.</param>
	/// <param name="tileSizeY">Tile size y.</param>
	/// <param name="tilePosition">Tile position.</param>
	/// <param name="tileMaterial">Tile material.</param>
	public Tile(string tileName, int tileSizeX, int tileSizeZ, int tileSizeY, Vector3 tilePosition, Material tileMaterial) {
		this.tileName = tileName;
		this.tilePosition = tilePosition;
		this.tileSizeX = tileSizeX;
		this.tileSizeZ = tileSizeZ;
		this.tileSizeY = tileSizeY;
		this.tileMaterial = tileMaterial;
	}

	/// <summary>
	/// Instantiates the tile to the gameworld.
	/// </summary>
	/// <param name="tile">Tile.</param>
	public void instantiateTile(Tile tile) {
		GameObject tileTemp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		tileTemp.name = tile.tileName;
		tileTemp.transform.position = tile.tilePosition;
		tileTemp.transform.localScale = new Vector3 (tile.tileSizeX, tile.tileSizeZ, tile.tileSizeY);
		tileTemp.GetComponent<Renderer> ().material = tile.tileMaterial;
	}





}
