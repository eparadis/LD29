using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	public GameObject waterTilePrefab;
	public GameObject landTilePrefab;
	public GameObject mineTilePrefab;
	public GameObject sharkPrefab;
	public GameObject treasurePrefab;

	private List<char[]> levelData;
	private int totalTreasures;
	private Vector2 playerStartPosition;

	public void InitalizeBoard ( List<char[]> level ) {
		levelData = level;	// should we clone our own copy?
		playerStartPosition = Vector2.zero;
		PlaceTiles();
		BroadcastMessage("OnBoardLoaded", playerStartPosition);	// tell child objects board is loaded; should include all tiles, players, enemies, etc
	}

	void PlaceTiles()
	{
		totalTreasures = 0;
		int numRows = 10;
		int numCols = 10;

		for( int row = 0; row < numRows; row += 1)
		{
			for( int col = 0; col < numCols; col += 1)
			{
				Vector3 pos = new Vector3( row, col, 0);
				GameObject tile;
				switch( levelData[row][col])
				{
				default:
				case '0':
					tile = (GameObject) GameObject.Instantiate( waterTilePrefab);
					break;
				case '1':
					tile = (GameObject) GameObject.Instantiate( landTilePrefab);
					break;
				case '2':
					tile = (GameObject) GameObject.Instantiate( mineTilePrefab);
					break;
				case '3':
					tile = (GameObject) GameObject.Instantiate( waterTilePrefab);

					GameObject sharkGO = (GameObject) GameObject.Instantiate( sharkPrefab);
					sharkGO.transform.parent = gameObject.transform;
					Vector3 sharkPos = pos;
					sharkPos.z = -0.5f;	// start it on top of the given tile in the map, but below the ship
					sharkGO.transform.localPosition = sharkPos;	
					Shark shark = sharkGO.GetComponent<Shark>();
					shark.row = row;
					shark.col = col;
					shark.board = this;
					break;
				case '4':
					tile = (GameObject) GameObject.Instantiate( waterTilePrefab);

					GameObject treasureGO = (GameObject) GameObject.Instantiate( treasurePrefab);
					treasureGO.transform.parent = gameObject.transform;
					Vector3 treasurePos = pos;
					treasurePos.z = -0.75f;	// above tiles and sharks, below ship
					treasureGO.transform.localPosition = treasurePos;
					treasureGO.name = "treasure_" + MakeTileName(row,col);

					totalTreasures += 1;
					break;
				case '5':
					tile = (GameObject) GameObject.Instantiate( waterTilePrefab);
					playerStartPosition = new Vector2( row, col);

					break;
				}
				tile.transform.parent = gameObject.transform;
				tile.transform.localPosition = pos;
				tile.name = MakeTileName( row, col);
				ClickReporter cr = tile.AddComponent<ClickReporter>();
				cr.row = row;
				cr.col = col;
				cr.reportTarget = gameObject;
			}
		}
	}

	string MakeTileName( int row, int col)
	{
		return string.Format ("board_tile_r{0}_c{1}", row, col);
	}

	public Vector3 GetTilePosition( int row, int col)
	{
		return transform.FindChild( MakeTileName( row, col)).position ;
	}

	public Vector3 GetTileLocalPosition( int row, int col)
	{
		return transform.FindChild( MakeTileName( row, col)).localPosition ;
	}

	public bool isWaterTile( int row, int col)
	{
		char t = levelData[row][col];
		return t == '0' |		// water
				t == '2' |		// mine
				t == '3'|		// water where a shark starts
				t == '4'|		// water where some treasure starts
				t == '5';		// water where the player starts
	}

	public bool isMineTile( int row, int col)
	{
		return levelData[row][col] == '2';
	}

	public void NeutralizeMine( int row, int col)
	{
		if( levelData[row][col] == '2')
		{
			// change the map data
			char[] temp = levelData[row];
			temp[col] = '0';
			levelData[row] = temp;
			// redraw the single changed tile
			MakeWaterTile( row, col);
		}
	}

	void MakeWaterTile( int row, int col)
	{
		Destroy(GameObject.Find( MakeTileName(row, col)));
		GameObject tile;
		Vector3 pos = new Vector3( row, col, 0);
		tile = (GameObject) GameObject.Instantiate( waterTilePrefab);

		tile.transform.parent = gameObject.transform;
		tile.transform.localPosition = pos;
		tile.name = MakeTileName( row, col);
		ClickReporter cr = tile.AddComponent<ClickReporter>();
		cr.row = row;
		cr.col = col;
		cr.reportTarget = gameObject;
	}

	public void CollectTreasure( int row, int col)
	{
		GameObject t = GameObject.Find ("treasure_" + MakeTileName( row,col));
		if(t != null)
		{
			Destroy(t);
		}
	}

	public bool isTreasureTile( int row, int col)
	{
		GameObject t = GameObject.Find ("treasure_" + MakeTileName( row,col));
		if( t != null)
			return true;
		else
			return false;
	}

	public int GetTotalTreasures()
	{
		return totalTreasures;
	}

	/*private void EraseTiles()
	{
		foreach( Transform child in transform)
		{
			Destroy(child.gameObject);
		}
	}*/
}
