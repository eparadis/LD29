using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TitleMenu : MonoBehaviour {

	private string instructionText =
		"You are an undersea explorer searching for treasure!\n" +
		"Pilot your boat and deploy your trusty diver to recover the riches of the deep.\n" +
		"Beware: Explosive mines and hungry sharks lurk beneath the waves!";
	private string creditsText =
		"A Ludum Dare 29 48-Hour Competition Entry\n" +
		"Programming: Ed Paradis\n" +
		"Special Thanks to Katharine C. <3";

	private List<string> levelNames;
	int selection = 0;
	Vector2 scrollPos;

	// Use this for initialization
	void Start () {
		TextAsset[] levels = Resources.LoadAll<TextAsset>("");
		levelNames = new List<string>();
		foreach( TextAsset ta in levels)
		{
			levelNames.Add( ta.name);
		}
	}
	
	void OnGUI()
	{
		Rect menuRect = new Rect( Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/1.7f);

		GUILayout.BeginArea( menuRect, GUI.skin.box);
		GUILayout.Space(15);
		GUILayout.Label( instructionText, GUI.skin.box, GUILayout.ExpandHeight(true) );
		GUILayout.Space(15);
		GUILayout.Label("Select a level:");
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		selection = GUILayout.SelectionGrid(selection, levelNames.ToArray(), 1);
		GUILayout.EndScrollView();

		GUILayout.BeginHorizontal();
		if( GUILayout.Button( "Start Game", GUILayout.Height(50)) )
		{
			Debug.Log ("selected level is " + levelNames[selection]);
			PlayerPrefs.SetString( "selected_level", levelNames[selection]);
			Application.LoadLevel( Application.loadedLevel + 1);	// go to whatever the next level in the build is
		}
		if( GUILayout.Button( "Level Editor", GUILayout.Height(50)) )
			Application.LoadLevel( "LevelEditor");
		GUILayout.EndHorizontal();

		GUILayout.Space(15);
		GUILayout.Label ( creditsText, GUI.skin.box );
		GUILayout.EndArea();

	}
}
