using UnityEngine;
using System.Collections;

public class GameEnd : MonoBehaviour
{
	public Character[] characters;
	Player player;
	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		bool win = true;
		for(int i = 0; i<characters.Length; ++i){
			if(characters[i].scareLevel < characters[i].maxScareLevel){
				win = false;
				break;
			}
		}
		if(win)
			Application.LoadLevel("GameWon");
		
		if(player.health <= 0.0f)
			Application.LoadLevel("GameLost");
	}
}

