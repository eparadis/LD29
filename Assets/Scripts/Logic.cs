using UnityEngine;
using System.Collections;

public class Logic : MonoBehaviour {

	Board board;
	public GameObject gameOverText;

	// Use this for initialization
	void Start () {

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
			StartCoroutine( AnimateGameOver() );
			BroadcastMessage("OnSinkShip");
			StartCoroutine( WaitForAnyKeyToRestart() );
		}
	}

	IEnumerator AnimateGameOver()
	{
		float length = 1f;
		float startTime = Time.time;
		Vector3 pos = new Vector3( 0, 4.5f, -2);
		float t;
		do {
			t = Time.time - startTime;
			pos.x = Mathf.Sin(t) * 5f;
			gameOverText.transform.localPosition = pos;
			yield return new WaitForEndOfFrame();
		} while( Time.time < startTime + length);
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
}
