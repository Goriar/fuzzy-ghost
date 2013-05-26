using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	private bool locked;	
	
	private GameObject player;
	private Moving moving;
	
	private float lastMouseDown;
	private float mousePressedTime;
	
	// Use this for initialization
	void Start () {
		// Starte mit Player Objekt, das steuerbar ist
		player = GameObject.FindGameObjectWithTag("Player");
		moving = player.GetComponent<Moving>();
		moving.controller = this;
		player.BroadcastMessage("makeTransparent", 1);
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
		}
		
		if (Input.GetMouseButton(0)) {
			mousePressedTime = Time.time - lastMouseDown;
		}
		
		if(Input.GetMouseButton(0) && mousePressedTime > 0.5f) {
			// Debug.Log("maus lange gedrückt");
		}
		
		if (Input.GetMouseButtonUp(0)) {
			// Gehe nur, wenn Klick (down+up unter 0.5 Sek.)
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
			
			// Drop Item
			if (Input.GetKey(KeyCode.I)) {
				moving.GetComponent<Inventory>().dropItem();
			}		
			
			// TESTCODE START
			if (Input.GetKeyDown(KeyCode.Space)) {
				player.BroadcastMessage("show", 1);
			} else if (Input.GetKeyUp  (KeyCode.Space)) {
				player.BroadcastMessage("makeTransparent", 1);
			}
			// TESTCODE END
			
			// Layer Switch nur, wenn Tür in Nähe ist
			if (moving.usableDoor != null) {
				// Layer Switch bei Pfeil nach oben
				if (Input.GetKeyDown(KeyCode.UpArrow) && moving.usableDoor.switchDirection == DirectionEnum.BACK) {
					moving.usableDoor.use();
				}
				
				// Layer Switch bei Pfeil nach unten
				if (Input.GetKeyDown(KeyCode.DownArrow) && moving.usableDoor.switchDirection == DirectionEnum.FORE) {
					moving.usableDoor.use();
				}
			}
			
					
			goToMouseClick(); // Checkt Klick zum Bewegen
		}
	}
	
}
