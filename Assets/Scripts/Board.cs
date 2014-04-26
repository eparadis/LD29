using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

	public GameObject waterTilePrefab;

	// Use this for initialization
	void Start () {
		PlaceTiles();
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
				GameObject tile = (GameObject) GameObject.Instantiate( waterTilePrefab);
				tile.transform.parent = gameObject.transform;
				tile.transform.localPosition = pos;
				tile.name = waterTilePrefab.name + string.Format ("_r{0}_c{1}", row, col);
			}
		}
	}
}
