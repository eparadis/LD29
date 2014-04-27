using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	int row = 0;
	int col = 0;
	int maxRow = 9;
	int maxCol = 9;
	private Board board;
	bool isSunk = false;
	bool diverDeployed = false;
	public GameObject diverPrefab;
	GameObject diver;
	int diverRow = 0;
	int diverCol = 0;

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
		if( Input.GetKeyDown(KeyCode.Space) )
			DeployDiver();
	}

	void MoveUp()
	{
		if(isSunk)
			return;

		if( diverDeployed)
		{
			if( isMovable( row - 1, col))
			{
				diverRow = row - 1;
				diverCol = col;
				SetDiverPosition();
			}
		} else
		if( isMovable( row - 1, col) )
		{
			row -= 1;
			UpdatePosition();
		}
	}

	void MoveDown()
	{
		if(isSunk)
			return;

		if( diverDeployed)
		{
			if( isMovable( row + 1, col))
			{
				diverRow = row + 1;
				diverCol = col;
				SetDiverPosition();
			}
		} else
		if( isMovable( row + 1, col) )
		{
			row += 1;
			UpdatePosition();
		}
	}

	void MoveRight()
	{
		if(isSunk)
			return;

		if( diverDeployed)
		{
			if( isMovable( row, col + 1))
			{
				diverRow = row;
				diverCol = col + 1;
				SetDiverPosition();
			}
		} else
		if( isMovable( row, col + 1) )
		{
			col += 1;
			UpdatePosition();
		}
	}

	void MoveLeft()
	{
		if(isSunk)
			return;

		if( diverDeployed)
		{
			if( isMovable( row, col - 1))
			{
				diverRow = row;
				diverCol = col - 1;
				SetDiverPosition();
			}
		} else
		if( isMovable( row, col - 1) )
		{
			col -= 1;
			UpdatePosition();
		}
	}

	void SetDiverPosition()
	{
		Vector3 p = board.GetTileLocalPosition( diverRow, diverCol);
		p.z = diver.transform.localPosition.z;
		diver.transform.localPosition = p;
		transform.parent.gameObject.BroadcastMessage( "OnDiverMoved", new Vector2( diverRow, diverCol));
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
	void OnBoardLoaded( Vector2 startPosition)
	{
		row = (int) startPosition.x;
		col = (int) startPosition.y;
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
		DeployDiver();
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

	void DeployDiver()
	{
		if( isSunk )
			return;	// don't respond to deploying diver if we're sunk

		if( !diverDeployed)
		{
			diver = (GameObject) GameObject.Instantiate( diverPrefab);
			diver.transform.parent = board.transform;
			Vector3 pos = board.GetTileLocalPosition( row, col ); // current boat position
			pos.z = - 1.5f;	// above the boat
			diver.transform.localPosition = pos;
			diverDeployed = true;
		}
		else{
			diverDeployed = false;
			Destroy (diver);
			diver = null;
		}
	}

	public Vector2 GetDiverPosition()
	{
		if( !diverDeployed)
			return new Vector2( -1, -1);	// off board, should never match

		return new Vector2( diverRow, diverCol);
	}

	void OnSharkCaughtDiver( Vector2 p)
	{
		isSunk = true;	// we're not actually sunk, but the game is over so we're EFFECTIVELY sunk. figuratively 'sunk'
		StartCoroutine( AnimateDiverDeath() ); // animate the diver dying or something
	}

	IEnumerator AnimateDiverDeath()
	{
		float length = 1f;	// one second 
		float startTime = Time.time;
		Vector3 scale = diver.transform.localScale;
		do {
			scale *= 0.9f;
			diver.transform.localScale = scale;
			yield return new WaitForEndOfFrame();
		} while (Time.time < startTime + length);
	}

}
