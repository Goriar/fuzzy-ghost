using UnityEngine;
using System.Collections;

public class Respawner : MonoBehaviour
{
	private ArrayList respawnList;
	private ArrayList respawnTimes;
		
	// Use this for initialization
	void Start ()
	{
		respawnList = new ArrayList();
		respawnTimes = new ArrayList();
	}
	
	// Update is called once per frame
	void Update ()
	{
		for(int i = 0; i<respawnTimes.Count; ++i){
			float val = (float)respawnTimes[i];
			val +=Time.deltaTime;
			respawnTimes[i] = val;
			if(val>=10.0f){
				GameObject g = (GameObject)respawnList[i];
					Animation anim = g.GetComponent<Animation>();
				if(anim !=null){
					anim.wrapMode = WrapMode.Loop;
				}
				g.GetComponent<Interactable>().enabled = true;
				g.SetActive(true);
				respawnList.RemoveAt(i);
				respawnTimes.RemoveAt(i);
				
			}
		}
	}
	
	public void addToRespawnList(GameObject o){
		respawnList.Add(o);
		ArrayList temp = new ArrayList(respawnList.Count);
		for(int i = 0; i<respawnTimes.Count; ++i)
		{
			temp.Add(respawnTimes[i]);
		}
		temp.Add(0.0f);
		respawnTimes = temp;
	}
	
}

