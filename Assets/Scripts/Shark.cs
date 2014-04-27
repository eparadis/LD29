using UnityEngine;
using System.Collections;

public class Shark : MonoBehaviour {

	public int row;
	public int col;
	public Board board;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void UpdatePosition()
	{
		Vector3 pos = board.GetTilePosition( row, col);
		pos.z = transform.position.z;	// do not change the Z position
		transform.position = pos;

		Vector2 diverPos = GameObject.Find("player").GetComponent<Player>().GetDiverPosition();
		if( (int)diverPos.x == row && (int)diverPos.y == col)
			transform.parent.gameObject.BroadcastMessage("OnSharkCaughtDiver", diverPos);
	}

	void OnDiverMoved( Vector2 playerPos)
	{
		// TODO this isn't the best behavior since it allows diagonal moves; a better way would be to find target moves, rule out the bad ones, then pick the best amongst the remaining
		int pRow = (int) playerPos.x;
		int pCol = (int) playerPos.y;
		int nRow = row;
		int nCol = col;

		if( pRow < row)
			nRow -= 1;
		if( pRow > row)
			nRow += 1;
		if( !board.isWaterTile(nRow, col) )
			nRow = row;	// don't change the row if it isn't a good place to move

		if( pCol < col)
			nCol -= 1;
		if( pCol > col)
			nCol += 1;
		if( !board.isWaterTile(row, nCol) )
			nCol = col;

		// check that we've not messed up on a diagonal, 
		if( board.isWaterTile( nRow, nCol)  )
		{
			row = nRow;
			col = nCol;
			UpdatePosition();
		}
	}
}
