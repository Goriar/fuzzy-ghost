using UnityEngine;
using System.Collections;

public class ObjectInteraction : MonoBehaviour {
	
	private bool showMenu;		//Wird true, wenn ein Objekt ausgewählt wurde
	
	private float posX,posY; 	//Geben die Position der Maus im Moment des Klicks an
	
	private GameObject target;	//Das ausgewählte Objekt
	
	private float count;		//Zählvariable, welche die vergangene Zeit seit dem die Maus gedrückt wurde zählt 
								//und ob diese weiterhin gedrückt ist
	
	// Use this for initialization
	void Start (){ 
		showMenu = false;
		count = 0.5f;  			//0.5 Sekunden bis das Menü gezeigt wird

	}
	
	// Update is called once per frame
	void Update () {
		
		//Prüft ob die Maus gedrückt wurde und zählt dann count runter. Wenn lang genug gedrückt wurde,
		//wird ein Raycast geschickt mit dem das Objekt indetifiziert wird
		
		if(Input.GetMouseButton(0)){
			count-=Time.deltaTime;
			if(count<=0){
				if(!showMenu){
					posX = Input.mousePosition.x;
					posY = Screen.height - Input.mousePosition.y;
					
					RaycastHit hit = new RaycastHit();
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					
					if(Physics.Raycast(ray,out hit)){
						target = hit.collider.gameObject;
					}
				}
				if(target!=null)
					showMenu = true;
			}
		}
		else{
			count = 0.5f;
		}
		
	}
	
	void OnGUI(){
	
		//Erstellt das Menü und führt Aktion aus, sobald der entsprechende Button gedrückt wurde
		
		if(showMenu){
			if(target.GetComponent("Interactable") != null && target.GetComponent<Interactable>().enabled){
				Interactable interact = (Interactable)target.GetComponent("Interactable");
				string[] buttons = interact.getButtonTexts();
				GUI.Box(new Rect(posX,posY,100,150),"Test");
				for(int i=0; i<buttons.Length; i++){
					if(GUI.Button(new Rect(posX+5,posY+25+i*25,90,30),buttons[i])){
						interact.doSomething(i);
						showMenu = false;
						count = 0.5f;
					
					}
				}
				
			}
			else
				showMenu = false;
		}
	}
}
