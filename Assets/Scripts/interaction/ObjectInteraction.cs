using UnityEngine;
using System.Collections;

// Menübutton Stucture
public struct MenuButton {
	public float x, y; // Startpos
	public float tweenToX, tweenToY; // Endpos der Animation
	public string name;
	public int arrayPos; // Array pos in Aktionsliste
	public float offset; // X-Offset des Sprites
}

public class ObjectInteraction : MonoBehaviour {
	
	private float posX,posY; 											// Geben die Position der Maus im Moment des Klicks an
	
	private GameObject target;											// Das ausgewählte Objekt
	
	private bool mouseDown = false;								// Maus wurde gedrückt
	private float mouseDownStart;									// Startzeitpunkt, an dem die Maus gedrückt wurde
	private bool menuOpen = false;									// Menü wurde geöffnet
	
	private float count;														// Zählvariable, welche die vergangene Zeit seit dem die Maus gedrückt wurde zählt 
																								// und ob diese weiterhin gedrückt ist
	
	public Texture menuTexture;										// Textur des Ringmenüs
	
	private MenuButton[] buttons;									// Menü Button Liste			
	private int activeButtonIndex;									// aktuell aktivierter Button
	
	private Interactable interact;									// Interaktion
	
	
	// Use this for initialization
	void Start (){ 
		count = 0.5f;  			//0.5 Sekunden bis das Menü gezeigt wird
		activeButtonIndex = -1;
	}
	
	
	void OnGUI () {
	
		if (menuOpen) {
			
			// Sprite Dimensionen
			float spriteWidth = 2760f*0.4f;
			float spriteHeight = 552f*0.4f;
			
			// Dimensionen eines einzelnen Buttons
			float spriteCropWidth = spriteWidth*0.1f;
			float spriteCropHeight = spriteHeight*0.5f;
			
			// Temporäre Variable um zu schauen, ob überhaupt ein Element aktiv ist (zum späteren Reset des aktivIndex)
			bool oneActive = false;
			
			// Durchgeht alle Buttons
			for (int i = 0; i < buttons.Length; i++) {
				// Update der X und Y Position der Buttons (Animation auf finale Position)
				buttons[i].x = iTween.FloatUpdate(buttons[i].x, buttons[i].tweenToX, 10f);
				buttons[i].y = iTween.FloatUpdate(buttons[i].y, buttons[i].tweenToY, 10f);
			
				// AABB Kollision mit Mauszeiger
				bool insideButtonX = (Input.mousePosition.x >= buttons[i].x-spriteCropWidth*0.5f && Input.mousePosition.x <= buttons[i].x+spriteCropWidth*0.5f);
				bool insideButtonY = ((Screen.height - Input.mousePosition.y) >= buttons[i].y-spriteCropHeight*0.5f && (Screen.height - Input.mousePosition.y) <= buttons[i].y+spriteCropHeight*0.5f);
				
				// yOffset = 0 			=> Button inaktiv
				// yOffset = 0.5f 		=> Button aktiv
				float yOffset = 0f;
				
				// Wenn Maus in Button, dann setze aktiv
				if (insideButtonX && insideButtonY) {
					yOffset = 0.5f;
					oneActive = true;
					activeButtonIndex = i; // setze aktiven Button (für Aktion bei Loslassen) 
				}
				
				// zeichnet die Buttons mit crop
				GUI.BeginGroup( new Rect( buttons[i].x-spriteCropWidth*0.5f, buttons[i].y-spriteCropHeight*0.5f, spriteCropWidth, spriteCropHeight ) );		
				GUI.DrawTexture(new Rect(-spriteWidth*buttons[i].offset, -spriteHeight*yOffset, spriteWidth, spriteHeight), menuTexture, ScaleMode.StretchToFill, true);
				GUI.EndGroup();
			}
			
			// Reset auf -1, wenn kein Element aktiv ist (um Bug zu beheben, der eintritt, wenn ein Element aktiv war, bei mousebuttonup aber nicht mehr aktiv ist)
			if (oneActive == false)
				activeButtonIndex = -1;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		//Prüft ob die Maus gedrückt wurde und zählt dann count runter. Wenn lang genug gedrückt wurde,
		//wird ein Raycast geschickt mit dem das Objekt indetifiziert wird
		
		// Wenn Maus gedrückt wird, setze Werte
		if(Input.GetMouseButtonDown(0)){
			mouseDown = true;
			mouseDownStart = Time.time;
		// Wenn Maus losgelassen wird, initiiere Button aktion (wenn ein Button aktiv)
		} else if (Input.GetMouseButtonUp(0)) {
			// schließe Menü
			mouseDown = false;
			menuOpen = false;
			buttons = null;
			
			// Wenn aktiver Button und aktion vorhanden, führe diese aus
			if (activeButtonIndex >= 0 && interact != null) {
				interact.doSomething(activeButtonIndex);
				
			}
			
			// Setze Interaktion und aktiven Button wieder auf null
			interact = null;
			activeButtonIndex = -1;
		}
		
		// Wenn Maus gedrückt und Zeit für Menüerscheinen überschritten...
		if (mouseDown && (Time.time - mouseDownStart) > count) {
			
			// Führe Raycast auf Mausposition aus
			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			// Wenn Objekt getroffen, setze Werte von Objekt in Variable
			if(Physics.Raycast(ray,out hit)){
				target = hit.collider.gameObject;
				posX = Input.mousePosition.x;
				posY = Screen.height - Input.mousePosition.y;
			}
			
			// wenn Menü noch nich geöffnet und target vorhanden und Interagierbar
			if(!menuOpen && target != null && target.GetComponent("Interactable") != null && target.GetComponent<Interactable>().enabled){
				// Setze Menü geöffnet
				menuOpen = true;
				
				// Setze interagierbares Objekt
				interact = (Interactable)target.GetComponent("Interactable");
				
				// Erhalte Button Werte (Texte, Sprite Offsets, etc.)
				string[] buttonTexts = interact.getButtonTexts();
				float[] buttonOffsets = interact.getButtonOffsets();
				// erstelle Array mit Anzahl der zu rendernden Buttons
				buttons = new MenuButton[buttonTexts.Length];
				
				// Berechne Button Radius anhand der Anzahl der Buttons
				float buttonRadius = 20f*buttons.Length;
				// Sonderfälle für 1-3 Buttons
				if (buttons.Length == 1)
					buttonRadius = 0f;
				if (buttons.Length == 2)
					buttonRadius = 60f;
				else if (buttons.Length == 3)
					buttonRadius = 75f;
				
				// Durchgehe alle Buttons und setze deren Werte
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
