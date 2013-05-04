using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	
	private Moving moving;
	
	// Use this for initialization
	void Start () {
		// Starte mit Player Objekt, das steuerbar ist
		moving = GameObject.FindGameObjectWithTag("Player").GetComponent<Moving>();
		BroadcastMessage("makeTransparent", 1);
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
		if(Input.GetMouseButton(0)) {
			// Ray Cast von Kamera aus zur Mausposition
			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			// Wenn Ray Objekt getroffen hat
			if(Physics.Raycast(ray,out hit)){
				Vector3 target = hit.point; // getroffenes Objekt
				// Wenn Objekt aktuell begehter Pfad ist
				//if (target == moving.walkPath) {
					moving.goToX(target.x);
				//}
			}
		}
	}
	
	void FixedUpdate () {
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
		
				
		goToMouseClick(); // Checkt Klick zum Bewegen
	}
}
