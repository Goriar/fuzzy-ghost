using UnityEngine;
using System.Collections;

public class ObjectInteraction : MonoBehaviour {
	
	private float posX,posY,posZ; 	//Geben die Position der Maus im Moment des Klicks an
	
	private GameObject target;	//Das ausgewählte Objekt
	
	private bool mouseDown = false;
	private float mouseDownStart;
	private bool menuOpen = false;
	
	private float count;		//Zählvariable, welche die vergangene Zeit seit dem die Maus gedrückt wurde zählt 
								//und ob diese weiterhin gedrückt ist
	
	public GameObject interactionButtonPrefab; 
	private GameObject[] buttons;
	
	// Use this for initialization
	void Start (){ 
		count = 0.5f;  			//0.5 Sekunden bis das Menü gezeigt wird
		buttons = new GameObject[0];
	}
	
	// Update is called once per frame
	void Update () {
		
		//Prüft ob die Maus gedrückt wurde und zählt dann count runter. Wenn lang genug gedrückt wurde,
		//wird ein Raycast geschickt mit dem das Objekt indetifiziert wird
		
		if(Input.GetMouseButtonDown(0)){
			Debug.Log ("Mouse Down");
			mouseDown = true;
			mouseDownStart = Time.time;
		} else if (Input.GetMouseButtonUp(0)) {
			mouseDown = false;
			menuOpen = false;
			for (int i = 0; i < buttons.Length; i++) {
				Destroy(buttons[i]);
			}
		}
		
		if (mouseDown && (Time.time - mouseDownStart) > count) {
			
			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if(Physics.Raycast(ray,out hit)){
				target = hit.collider.gameObject;
				posX = hit.point.x;
				posY = hit.point.y;
				posZ = hit.point.z;
			}
			
			if(!menuOpen && target != null && target.GetComponent("Interactable") != null && target.GetComponent<Interactable>().enabled){
				menuOpen = true;
				Interactable interact = (Interactable)target.GetComponent("Interactable");
				Debug.Log("opened menu");
				Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;
					
				string[] buttonTexts = interact.getButtonTexts();
				float[] buttonOffsets = interact.getButtonOffsets();
				buttons = new GameObject[buttonTexts.Length];
				float buttonRadius = 0.06f*buttons.Length;
				if (buttons.Length == 2)
					buttonRadius = 0.18f;
				else if (buttons.Length == 3)
					buttonRadius = 0.2f;
				for(int i=0; i<buttons.Length; i++){
					Debug.Log (buttonOffsets[i]);
					buttons[i] = GameObject.Instantiate (interactionButtonPrefab, new Vector3(posX, posY, posZ), playerPos.rotation) as GameObject;
					buttons[i].renderer.material.mainTextureOffset = new Vector2 (buttonOffsets[i],0.5f);
					Debug.Log(buttons[i].renderer.material.mainTextureOffset);
					
					if (buttons.Length > 1) {
						float tweenPosX = posX + Mathf.Cos((2*Mathf.PI/buttons.Length)*i)*buttonRadius;
						float tweenPosY = posY + Mathf.Sin((2*Mathf.PI/buttons.Length)*i)*buttonRadius;
						
						
						Hashtable ht = new Hashtable();
						ht.Add("time", 0.3f);
						ht.Add("x", tweenPosX);
						ht.Add ("y", tweenPosY);
						ht.Add("easetype", "easeInOutSine");
						ht.Add("oncomplete", "stopLayerSwitch");
						iTween.MoveTo(buttons[i].gameObject, ht);
					}
				}
				/*
				string[] buttons = interact.getButtonTexts();
				GUI.Box(new Rect(posX,posY,100,150),"Test");
				for(int i=0; i<buttons.Length; i++){
					if(GUI.Button(new Rect(posX+5,posY+25+i*25,90,30),buttons[i])){
						interact.doSomething(i);
						showMenu = false;
						count = 0.5f;
					
					}
				}*/
				
			}
		}
				
	}
}
