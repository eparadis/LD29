using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

	public GameObject waterTilePrefab;
	public GameObject landTilePrefab;
	public GameObject mineTilePrefab;

	private string[] levelData = {
	//   0123456789
		"0000000000",	// 0
		"0011002000",	// 1
		"0011000000",	// 2
		"0000002000",	// 3
		"0000000000",	// 4
		"0020000000",	// 5
		"0000000001",	// 6
		"0000200011",	// 7
		"0000001111",	// 8
		"0000111111"};	// 9

	// Use this for initialization
	void Start () {
		PlaceTiles();
		BroadcastMessage("OnBoardLoaded");	// tell child objects board is loaded; should include all tiles, players, enemies, etc
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PlaceTiles()
	{
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
				}
				tile.transform.parent = gameObject.transform;
				tile.transform.localPosition = pos;
				tile.name = MakeTileName( row, col);
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
}
