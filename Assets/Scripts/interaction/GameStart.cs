using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour
{
	
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void startGame(){
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFade>().StartFade(new Color(0,0,0,1),2f, this.loadLevel);
	}
	
	void loadLevel() {
		Application.LoadLevel("CFinish");
	}
	
	
	void quitGame(){
		Application.Quit();
	}
	
	void loadGame(){
		//Nope?
	}
}

