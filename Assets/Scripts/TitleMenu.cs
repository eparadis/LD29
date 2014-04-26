using UnityEngine;
using System.Collections;

public class TitleMenu : MonoBehaviour {

	private string instructionText =
		"You are an undersea explorer searching for treasure!\n" +
		"Pilot your boat and deploy your trusty diver to recover the riches of the deep.\n" +
		"Beware: Explosive mines and hungry sharks lurk beneath the waves!";
	private string creditsText =
		"A Ludum Dare 29 48-Hour Competition Entry\n" +
		"Programming: Ed Paradis\n" +
		"Special Thanks to Katharine C. <3";

	// Use this for initialization
	void Start () {
	
	}
	
	void OnGUI()
	{
		Rect menuRect = new Rect( Screen.width/4, Screen.height/3, Screen.width/2, Screen.height/2);

		GUILayout.BeginArea( menuRect, GUI.skin.box);
		GUILayout.Space(15);
		GUILayout.Label( instructionText, GUI.skin.box, GUILayout.ExpandHeight(true) );
		GUILayout.Space(15);
		if( GUILayout.Button( "Start Game", GUILayout.Height(100)) )
			Application.LoadLevel( Application.loadedLevel + 1);	// go to whatever the next level in the build is
		GUILayout.Space(15);
		GUILayout.Label ( creditsText, GUI.skin.box, GUILayout.ExpandHeight(true) );
		GUILayout.EndArea();

	}
}
