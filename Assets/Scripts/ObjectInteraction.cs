using UnityEngine;
using System.Collections;

public class ObjectInteraction : MonoBehaviour {
	
	private bool showMenu;
	private float posX,posY;
	private GameObject target;
	private float count;
	
	// Use this for initialization
	void Start (){ 
		showMenu = false;
		count = 0.5f;
	
	}
	
	// Update is called once per frame
	void Update () {
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
				showMenu = true;
			}
		}
		else{
			count = 0.5f;
		}
		
	}
	
	void OnGUI(){
	
		if(showMenu){
			if(target.GetComponent("Interactable") != null){
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
