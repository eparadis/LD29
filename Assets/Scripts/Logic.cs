using UnityEngine;
using System.Collections;

public class Logic : MonoBehaviour {

	Board board;
	public GameObject endMessageTextObject;
	int treasureCollected;

	// Use this for initialization
	void Start () {
		treasureCollected = 0;
	}
	
	void OnBoardLoaded()
	{
		board = GetComponent<Board>();
	}

	void OnPlayerMoved( Vector2 playerPos)
	{
		int row = (int) playerPos.x;
		int col = (int) playerPos.y;

		// did the player hit a mine?
		if( board.isMineTile(row, col) )
		{
			endMessageTextObject.GetComponent<TextMesh>().text = "Game Over";
			StartCoroutine( AnimateEndMessage() );
			BroadcastMessage("OnSinkShip");
			StartCoroutine( WaitForAnyKeyToRestart() );
		}
	}

	IEnumerator AnimateEndMessage()
	{
		float length = 1f;
		float startTime = Time.time;
		Vector3 pos = new Vector3( 0, 4.5f, -2);
		float t;
		do {
			t = Time.time - startTime;
			pos.x = Mathf.Sin(t) * 5f;
			endMessageTextObject.transform.localPosition = pos;
			yield return new WaitForEndOfFrame();
		} while( Time.time < startTime + length);
	}

	void LevelComplete()
	{
		endMessageTextObject.GetComponent<TextMesh>().text = "Level Complete";
		StartCoroutine( AnimateEndMessage() );
		BroadcastMessage("OnSinkShip");
		// TODO go to the next level
	}

	IEnumerator WaitForAnyKeyToRestart()
	{
		yield return new WaitForEndOfFrame(); // gobble any input from playing
		yield return new WaitForSeconds(0.5f);	//give time for the game over to run a little

		while( !Input.anyKey && !Input.GetMouseButton(0) )
		{
			yield return new WaitForEndOfFrame();
		}
		Application.LoadLevel (0);
	}

	void OnDiverMoved( Vector2 p)
	{
		if( board.isMineTile( (int) p.x, (int) p.y) )
			board.NeutralizeMine( (int) p.x, (int) p.y);
		if( board.isTreasureTile( (int) p.x, (int) p.y) )
		{
			board.CollectTreasure( (int) p.x, (int) p.y );
			treasureCollected += 1;
			if( board.GetTotalTreasures() == treasureCollected)
			{
				LevelComplete();
			}
		}
	}
}
