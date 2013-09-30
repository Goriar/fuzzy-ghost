using UnityEngine;
using System.Collections;

public class NPCDialogue : MonoBehaviour {
	
	Character character;
	Player player;
	// Use this for initialization
	void Start () {
		character = this.GetComponent<Character>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		if(character.talking && player.currentLocation == character.currentLocation)
			GUI.Label(new Rect(Camera.mainCamera.WorldToScreenPoint(this.transform.position).x,
				Camera.mainCamera.WorldToScreenPoint(this.transform.position).y,
				200,
				200)
				,character.getDialogue());
	}
}
