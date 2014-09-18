using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelDataLoader : MonoBehaviour {

	bool showGUI = false;
	string enteredText;
	public GameObject boardPrefab;
	private GameObject boardGO;
	private Vector2 scrollPos;
	bool sure = false;
	string levelName;

	// Use this for initialization
	IEnumerator Start () {
		if( Application.loadedLevelName == "LevelEditor")
		{
			levelName = "asdf";

			if(PlayerPrefs.HasKey("user_level"))
			{
				enteredText = PlayerPrefs.GetString("user_level");
			} else
			{
				TextAsset textData=(TextAsset)Resources.Load( "sample_level");
				enteredText = textData.text;
			}
			showGUI = true;
		} else
		{
			if( PlayerPrefs.GetInt( "load_from_web", 0) == 0)
				ReadLevelFromFile( PlayerPrefs.GetString("selected_level", "sample_level") );
			else // 1
				StartCoroutine( ReadLevelFromURL ( PlayerPrefs.GetString("selected_level")) );
		}
		return null;
	}

	void ReadLevelFromFile( string textResourceName)
	{
		TextAsset textData=(TextAsset)Resources.Load( textResourceName);
		if(textData == null)
		{
			Debug.LogError( "Could not load level from " + textResourceName);
			return;
		}

		ReadLevelFromString( textData.text);
	}

	IEnumerator ReadLevelFromURL( string url )
	{
		WWW www = new WWW( url);
		yield return www;
		ReadLevelFromString( www.text);
	}

	void ReadLevelFromString( string level)
	{
		List<char[]> output = new List<char[]>();
		foreach( string line in level.Split ('\n') )
		{
			if( line.StartsWith("#") )
				continue;	// ignore lines that start with #
			if( line.Length < 10 )
				continue;	// ignore lines that aren't 10 characters
			output.Add( line.ToCharArray() );
		}

		boardGO = (GameObject) GameObject.Instantiate(boardPrefab);
		Board board = boardGO.GetComponent<Board>();
		board.InitalizeBoard( output);
	}

	void OnGUI()
	{
		if( GUI.Button( new Rect(5,5,100,50), "<- Menu") )
			Application.LoadLevel (0);

		if(!showGUI)
			return;

		GUILayout.BeginArea( new Rect(170, 200, 200, 360), GUI.skin.box);
		GUILayout.Label ("Level Name:");
		levelName = GUILayout.TextField( levelName);
		GUILayout.Label( "Level Data:");
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		enteredText = GUILayout.TextArea( enteredText, GUILayout.ExpandHeight(true) );
		GUILayout.EndScrollView();
		if( GUILayout.Button ("Build Level"))
		{
			Destroy(boardGO);
			PlayerPrefs.SetString("user_level", enteredText);
			ReadLevelFromString( enteredText);
		}
		GUILayout.Space(20);
		if( !sure)
		{
			if( GUILayout.Button("Erase user level!"))
				sure = true;
		} else 
		{
			if( GUILayout.Button("Are you sure?") )
			{
				sure = false;
				PlayerPrefs.DeleteKey("user_level");
				Application.LoadLevel( Application.loadedLevel);
			}
		}
		GUILayout.Space (20);
		if(GUILayout.Button ("Store to server"))
		{
			WWWForm form = new WWWForm();
			form.AddField( "levelName", levelName);
			form.AddField( "levelData", enteredText);
			WWW www = new WWW( "http://localhost:5000/add/", form );
			// spinning isn't great (and especially in the UI task...) but here we go
			while( !www.isDone)
			{ }
		}
		GUILayout.EndArea();
	}

	

}
