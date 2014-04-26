using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	int row = 0;
	int col = 0;
	int maxRow = 9;
	int maxCol = 9;
	private Board board;
	bool isSunk = false;

	// Use this for initialization
	void Start () {
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

	// called on a successful change in position
	void UpdatePosition()
	{
		if( isSunk )
			return;
		transform.parent.gameObject.BroadcastMessage( "OnPlayerMoved", new Vector2( row, col));
		Vector3 pos = board.GetTilePosition( row, col);
		pos.z = transform.position.z;	// do not change the Z position
		transform.position = pos;
	}

	// called when the board is finished loading
	void OnBoardLoaded()
	{
		board = transform.parent.gameObject.GetComponent<Board>(); 
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

	void OnMouseDown()
	{
		Debug.Log ("mouse down on player");
	}

	void TileClicked( Vector2 coords)
	{
		int r = (int) coords.x;
		int c = (int) coords.y;

		//straight moves only for now
		if( c == col && r < row)
			MoveUp();
		if( c == col && r > row)
			MoveDown();
		if( c < col && r == row)
			MoveLeft();
		if( c > col && r == row)
			MoveRight();
	}

	void OnSinkShip()
	{
		isSunk = true;
		StartCoroutine( AnimateSinking() );
	}


	IEnumerator AnimateSinking()
	{
		float length = 1f;	// one second to sink the ship
		float startTime = Time.time;
		Vector3 scale = gameObject.transform.localScale;
		do {
			scale *= 0.9f;
			gameObject.transform.localScale = scale;
			yield return new WaitForEndOfFrame();
		} while (Time.time < startTime + length);
	}

}
