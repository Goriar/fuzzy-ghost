using UnityEngine;
using System.Collections;

public struct MenuButton {
	public float x, y;
	public float tweenToX, tweenToY;
	public string name;
	public int arrayPos;
	public float offset;
}

public class ObjectInteraction : MonoBehaviour {
	
	private float posX,posY; 	//Geben die Position der Maus im Moment des Klicks an
	
	private GameObject target;	//Das ausgewählte Objekt
	
	private bool mouseDown = false;
	private float mouseDownStart;
	private bool menuOpen = false;
	
	private float count;		//Zählvariable, welche die vergangene Zeit seit dem die Maus gedrückt wurde zählt 
								//und ob diese weiterhin gedrückt ist
	
	public Texture menuTexture;	
	
	public GameObject interactionButtonPrefab; 
	private MenuButton[] buttons;
	private int activeButtonIndex;
	
	private Interactable interact;
	
	
	// Use this for initialization
	void Start (){ 
		count = 0.5f;  			//0.5 Sekunden bis das Menü gezeigt wird
		activeButtonIndex = -1;
	}
	
	
	void OnGUI () {
	
		if (menuOpen) {
			
			float spriteWidth = 2760f*0.4f;
			float spriteHeight = 552f*0.4f;
			float spriteCropWidth = spriteWidth*0.1f;
			float spriteCropHeight = spriteHeight*0.5f;
			
			for (int i = 0; i < buttons.Length; i++) {
				// Update der X und Y Position der Buttons
				buttons[i].x = iTween.FloatUpdate(buttons[i].x, buttons[i].tweenToX, 10f);
				buttons[i].y = iTween.FloatUpdate(buttons[i].y, buttons[i].tweenToY, 10f);
		
				bool insideButtonX = (Input.mousePosition.x >= buttons[i].x-spriteCropWidth*0.5f && Input.mousePosition.x <= buttons[i].x+spriteCropWidth*0.5f);
				bool insideButtonY = ((Screen.height - Input.mousePosition.y) >= buttons[i].y-spriteCropHeight*0.5f && (Screen.height - Input.mousePosition.y) <= buttons[i].y+spriteCropHeight*0.5f);
				
				float yOffset = 0f;
				
				if (insideButtonX && insideButtonY) {
					yOffset = 0.5f;
					activeButtonIndex = i;
				}
				
				// zeichnet die Buttons mit crop
				GUI.BeginGroup( new Rect( buttons[i].x-spriteCropWidth*0.5f, buttons[i].y-spriteCropHeight*0.5f, spriteCropWidth, spriteCropHeight ) );		
				GUI.DrawTexture(new Rect(-spriteWidth*buttons[i].offset, -spriteHeight*yOffset, spriteWidth, spriteHeight), menuTexture, ScaleMode.StretchToFill, true);
				GUI.EndGroup();
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		//Prüft ob die Maus gedrückt wurde und zählt dann count runter. Wenn lang genug gedrückt wurde,
		//wird ein Raycast geschickt mit dem das Objekt indetifiziert wird
		
		
		if(Input.GetMouseButtonDown(0)){
			mouseDown = true;
			mouseDownStart = Time.time;
		} else if (Input.GetMouseButtonUp(0)) {
			mouseDown = false;
			menuOpen = false;
			buttons = null;
			
			if (activeButtonIndex >= 0 && interact != null) {
				interact.doSomething(activeButtonIndex);
				
			}
			
			interact = null;
			activeButtonIndex = -1;
		}
		
		if (mouseDown && (Time.time - mouseDownStart) > count) {
			
			
			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if(Physics.Raycast(ray,out hit)){
				target = hit.collider.gameObject;
				posX = Input.mousePosition.x;
				posY = Screen.height - Input.mousePosition.y;
			}
						
			if(!menuOpen && target != null && target.GetComponent("Interactable") != null && target.GetComponent<Interactable>().enabled){
				menuOpen = true;
				
				interact = (Interactable)target.GetComponent("Interactable");
				
				string[] buttonTexts = interact.getButtonTexts();
				float[] buttonOffsets = interact.getButtonOffsets();
				buttons = new MenuButton[buttonTexts.Length];
				
				float buttonRadius = 20f*buttons.Length;
				if (buttons.Length == 1)
					buttonRadius = 0f;
				if (buttons.Length == 2)
					buttonRadius = 60f;
				else if (buttons.Length == 3)
					buttonRadius = 75f;
				for(int i=0; i<buttons.Length; i++){
					
					buttons[i] = new MenuButton();
					buttons[i].x = posX;
					buttons[i].y = posY;
					buttons[i].name = buttonTexts[i];
					buttons[i].offset = buttonOffsets[i];
					
					buttons[i].tweenToX = posX + Mathf.Cos((2*Mathf.PI/buttons.Length)*i)*buttonRadius;
					buttons[i].tweenToY = posY + Mathf.Sin((2*Mathf.PI/buttons.Length)*i)*buttonRadius;
						
				}
				
			}
		}
			
	}
}
