using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	int row = 0;
	int col = 0;
	int maxRow = 9;
	int maxCol = 9;
	private Board board;

	// Use this for initialization
	void Start () {
		board = GameObject.Find("board").GetComponent<Board>();
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKeyDown(KeyCode.RightArrow) )
			MoveRight();
		if( Input.GetKeyDown(KeyCode.LeftArrow) )
			MoveLeft();
		if( Input.GetKeyDown(KeyCode.UpArrow) )
			MoveUp();
		if( Input.GetKeyDown(KeyCode.DownArrow) )
			MoveDown();
	}

	void MoveUp()
	{
		if( isMovable( row - 1, col) )
		{
			row -= 1;
			UpdatePosition();
		}
	}

	void MoveDown()
	{
		if( isMovable( row + 1, col) )
		{
			row += 1;
			UpdatePosition();
		}
	}

	void MoveRight()
	{
		if( isMovable( row, col + 1) )
		{
			col += 1;
			UpdatePosition();
		}
	}

	void MoveLeft()
	{
		if( isMovable( row, col - 1) )
		{
			col -= 1;
			UpdatePosition();
		}
	}

	void UpdatePosition()
	{
		Vector3 pos = board.GetTilePosition( row, col);
		pos.z = transform.position.z;	// do not change the Z position
		transform.position = pos;
	}

	// called when the board is finished loading
	void OnBoardLoaded()
	{
		UpdatePosition();
	}

	bool isOffBoard( int r, int c)
	{
		return (r < 0) || (r > maxRow) || (c < 0) || (c > maxCol);
	}

	bool isMovable( int r, int c)
	{
		return !isOffBoard(r,c) && board.isWaterTile(r,c);
	}

}
