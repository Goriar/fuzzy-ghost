using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	private bool locked;	
	
	private GameObject player;
	private Moving moving;
	
	private float lastMouseDown;
	public bool mousePressed;
	
	// Use this for initialization
	void Start () {
		// Starte mit Player Objekt, das steuerbar ist
		player = GameObject.FindGameObjectWithTag("Player");
		moving = player.GetComponent<Moving>();
		moving.controller = this;
		BroadcastMessage("makeTransparent", 1);
	}
	
	///
	/// Sperrt alle weiteren Eingaben
	///
	public void lockControls () {
		locked = true;
	}
	
	///
	/// Entsperrt alle Eingaben wieder
	////
	public void unlockControls () {
		locked = false;
	}
	
	///
	/// Wechsel des zu steuernden Objekts
	/// @param o GameObject, das gesteuert werden soll (benötigt Moving Komponente)
	/// 
	void changeControllable (GameObject o) {
		moving = o.GetComponent<Moving>();
	}
	
	///
	/// Bewegung zu Mausklick
	/// 
	public void goToMouseClick () {
		// Linksklick
		
		if (Input.GetMouseButtonDown(0)) {
			lastMouseDown = Time.time;
			mousePressed = true;
		}
		
		float mousePressedTime = 0;
		if (mousePressed) {
			mousePressedTime = Time.time - lastMouseDown;
		}
		
		if(mousePressed && mousePressedTime > 0.5f) {
			//Debug.Log("maus lange gedrückt");
		}
		
		if (Input.GetMouseButtonUp(0)) {
			mousePressed = false;
			// Gehe nur, wenn Klick (down+up unter 1 Sek.)
			if (mousePressedTime < 0.5f) {
				// Ray Cast von Kamera aus zur Mausposition
				RaycastHit hit = new RaycastHit();
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				// Wenn Ray Objekt getroffen hat
				if(Physics.Raycast(ray,out hit)){
					Vector3 target = hit.point; // getroffenes Objekt
					moving.goToX(target.x);
				}
			}
		}		
	}
	
	void FixedUpdate () {
		// Updates nur wenn nicht Eingabe nicht gesperrt!
		if (!locked && player != null && moving != null) {
			// Bewegung links bei Pfeil links
			if (Input.GetKey(KeyCode.LeftArrow)) {
				moving.execMoveLeft();
			}		
			// Bewegung rechts bei Pfeil rechts
			else if (Input.GetKey(KeyCode.RightArrow)) {
				moving.execMoveRight();
			} 
			// Stopt Bewegung, wenn keine Pfeiltasten gedrückt wurden
			else {
				moving.stopMoving();
			}
			
			// Layer Switch nur, wenn Tür in Nähe ist
			if (moving.usableDoor != null) {
				// Layer Switch bei Pfeil nach oben
				if (Input.GetKey(KeyCode.UpArrow) && moving.usableDoor.switchDirection == DirectionEnum.BACK) {
					moving.startLayerSwitch();
				}
				
				// Layer Switch bei Pfeil nach unten
				if (Input.GetKey(KeyCode.DownArrow) && moving.usableDoor.switchDirection == DirectionEnum.FORE) {
					moving.startLayerSwitch();
				}
			}
			
					
			goToMouseClick(); // Checkt Klick zum Bewegen
		}
	}
	
}
