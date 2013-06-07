using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
	private InteractionTypes[] interaction; 	//Objekt vom Typ InteractionTypes in dem die Aktionen gespeichert sind
	
	public InteractionTypes.Type[] types;		// Interkationstypen des Objekts
	
	private int action;							//Index der die Aktion bestimmt, -1 wenn keine Aktion
	
	private float startTime;					//Zeit zu dem der Button gedrückt wurde
	
	private bool playerArrivedAtTarget;			//Gibt an ob der Spieler beim Objekt ist
	
	// Use this for initialization
	void Start ()
	{
		interaction = new InteractionTypes[types.Length];
		for (int i = 0; i < types.Length; i++) {
			interaction[i] = new InteractionTypes(types[i]);
		}
		action = -1;
		playerArrivedAtTarget = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Wenn Spieler etwas anderes macht, wird Aktion abgebrochen
		if(Input.anyKey && !playerArrivedAtTarget){
			action = -1;
		}
		/*
		//Falls der Spieler nicht beim Objekt ist, wird er zu ihm bewegt. (Gegebenenfalls in Moving verschieben)
		if(!playerArrivedAtTarget && action>=0){
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			Vector3 playerPos = player.transform.position;
			Vector3 ObjectPos = this.transform.position;
			
			if(playerPos.x > ObjectPos.x+0.4 || playerPos.x < ObjectPos.x-0.4){
				if(ObjectPos.x > playerPos.x){
					player.transform.Translate(Vector3.right * 2f *Time.deltaTime);
				}
				else{
					player.transform.Translate(Vector3.left * 2f *Time.deltaTime);
				}
			}
			else{
				playerArrivedAtTarget = true;
				startTime = Time.time;
			}
		}	*/
		if(action >= 0 && playerArrivedAtTarget){
		//interaction.doSomething(ref action,startTime);
		}
		else{
			playerArrivedAtTarget = false;
		}
	}
	
	//Ermittelt den Text der Buttons
	public string[] getButtonTexts(){
		
		string[] output = new string[types.Length+1];
		for (int i = 0; i < types.Length; i++) {
			output[i] = interaction[i].getButtonText();
		}
		output[types.Length] = "Exit";
		return output;
	}
	
	//Wird aufgerufen sobald eine Aktion ausgeführt werden soll
	public void doSomething(int index){
		if (index == types.Length) {
			return;
		}
		this.BroadcastMessage(interaction[index].getMethod(), GameObject.FindGameObjectWithTag("Player"));
	}
	
}

