using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
	private InteractionTypes interaction; 		//Objekt vom Typ InteractionTypes in dem die Aktionen gespeichert sind
	
	public InteractionTypes.Type type;			//Enum mit dem das Objekt identifiziert wird
	
	private int action;							//Index der die Aktion bestimmt, -1 wenn keine Aktion
	
	private float startTime;					//Zeit zu dem der Button gedrückt wurde
	
	private bool playerArrivedAtTarget;			//Gibt an ob der Spieler beim Objekt ist
	
	// Use this for initialization
	void Start ()
	{
		interaction = new InteractionTypes(type);
		action = -1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Wenn Spieler etwas anderes macht, wird Aktion abgebrochen
		if(Input.anyKey && !playerArrivedAtTarget){
			action = -1;
		}
		
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
		}	
		if(action >= 0 && playerArrivedAtTarget){
		interaction.doSomething(ref action,startTime);
		}
		else{
			playerArrivedAtTarget = false;
		}
	}
	
	//Ermittelt den Text der Buttons
	public string[] getButtonTexts(){
		return interaction.getButtonTexts();
	}
	
	//Wird aufgerufen sobald eine Aktion ausgeführt werden soll
	public void doSomething(int index){
		action = index;
		startTime = Time.time;
	}
	
}

