using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
	private InteractionTypes interaction;
	public InteractionTypes.Type type;
	private int action;
	private float startTime;
	private bool playerArrivedAtTarget;
	// Use this for initialization
	void Start ()
	{
		interaction = new InteractionTypes(type);
		action = -1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.anyKey && !playerArrivedAtTarget){
			action = -1;
		}
		if(!playerArrivedAtTarget && action>=0){
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			Vector3 playerPos = player.transform.position;
			Vector3 ObjectPos = this.transform.position;
			
			if(playerPos.z > ObjectPos.z+1 || playerPos.z < ObjectPos.z-1){
				if(ObjectPos.z > playerPos.z){
					player.transform.Translate(Vector3.back * 10f *Time.deltaTime);
				}
				else{
					player.transform.Translate(Vector3.forward * 10f *Time.deltaTime);
				}
			}
			else{
				playerArrivedAtTarget = true;
				startTime = Time.time;
			}
		}
			
		if(action >= 0 && playerArrivedAtTarget){
		interaction.doSomething(ref action,startTime);
		}
		else{
			playerArrivedAtTarget = false;
		}
	}
	
	public string[] getButtonTexts(){
		return interaction.getButtonTexts();
	}
	
	public void doSomething(int index){
		action = index;
		startTime = Time.time;
	}
	
}

