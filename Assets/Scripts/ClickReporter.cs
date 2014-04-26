using UnityEngine;
using System.Collections;

public class ClickReporter : MonoBehaviour {

	public int row;
	public int col;
	public GameObject reportTarget;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		reportTarget.BroadcastMessage( "TileClicked", new Vector2( row, col));
	}
}
