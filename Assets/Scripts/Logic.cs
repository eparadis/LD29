using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		StartCoroutine( WaitForAnyKeyToGoToNextLevel() );
	}

	IEnumerator WaitForAnyKeyToGoToNextLevel()
	{
		yield return new WaitForEndOfFrame(); // gobble any input from playing
		yield return new WaitForSeconds(0.5f);	//give time for the game over to run a little

		float time = Time.time;
		while( !Input.anyKey && !Input.GetMouseButton(0) && Time.time < time + 0.5f )
		{
			yield return new WaitForEndOfFrame();
		}

		// TODO this is REALLY a hack-job >:O
		string currentLevelName = PlayerPrefs.GetString("selected_level", "sample_level");
		int nextLevel = 0;
		TextAsset[] levels = Resources.LoadAll<TextAsset>("");
		for( int i = 0; i < levels.Length; i += 1)
		{
			if( levels[i].name == currentLevelName)
				nextLevel = i + 1;
		}
		if( nextLevel == levels.Length )	// we've finished the last level, so go back to the menu
		{
			Debug.Log("Could not find another level, going back to title screen.");
			Application.LoadLevel( 0 );
		}
		else
		{
			// store the next level in the player prefs so it'll get loaded when we jump back
			PlayerPrefs.SetString( "selected_level", levels[nextLevel].name);
			Application.LoadLevel( Application.loadedLevel);
		}
	}

	IEnumerator WaitForAnyKeyToRestart()
	{
		yield return new WaitForEndOfFrame(); // gobble any input from playing
		yield return new WaitForSeconds(0.5f);	//give time for the game over to run a little

		float time = Time.time;
		while( !Input.anyKey && !Input.GetMouseButton(0) && Time.time < time + 0.5f)
		{
			yield return new WaitForEndOfFrame();
		}
		Application.LoadLevel( Application.loadedLevel);
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

	void OnSharkCaughtDiver( Vector2 p)
	{
		endMessageTextObject.GetComponent<TextMesh>().text = "Game Over";
		StartCoroutine( AnimateEndMessage() );
		StartCoroutine( WaitForAnyKeyToRestart() );
	}
}
