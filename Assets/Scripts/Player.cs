using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	int row = 0;
	int col = 0;
	int maxRow = 9;
	int maxCol = 9;

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
		row = Mathf.Clamp( row - 1, 0, maxRow);
		UpdatePosition();
	}

	void MoveDown()
	{
		row = Mathf.Clamp( row + 1, 0, maxRow);
		UpdatePosition();
	}

	void MoveRight()
	{
		col = Mathf.Clamp( col + 1, 0, maxCol);
		UpdatePosition();
	}

	void MoveLeft()
	{
		col = Mathf.Clamp( col - 1, 0, maxCol);
		UpdatePosition();
	}

	void UpdatePosition()
	{
		Vector3 pos = GameObject.Find("board").GetComponent<Board>().GetTilePosition( row, col);
		pos.z = transform.position.z;	// do not change the Z position
		transform.position = pos;
	}

	// called when the board is finished loading
	void OnBoardLoaded()
	{
		UpdatePosition();
	}

}
