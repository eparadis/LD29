using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

	public GameObject waterTilePrefab;
	public GameObject landTilePrefab;
	public GameObject mineTilePrefab;
	public GameObject sharkPrefab;

	private string[] levelData = {
	//   0123456789
		"0000000000",	// 0
		"0011002000",	// 1
		"0011000000",	// 2
		"0000002000",	// 3
		"0000000000",	// 4
		"0020000300",	// 5
		"0030000001",	// 6
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

	public bool isWaterTile( int row, int col)
	{
		char t = levelData[row][col];
		return t == '0' |		// water
				t == '2' |		// mine
				t == '3';		// water where a shark starts
	}

	public bool isMineTile( int row, int col)
	{
		return levelData[row][col] == '2';
	}
}
