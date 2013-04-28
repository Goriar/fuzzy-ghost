using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	
	private Moving moving;
	
	// Use this for initialization
	void Start () {
		// Starte mit Player Objekt, das steuerbar ist
		moving = GameObject.FindGameObjectWithTag("Player").GetComponent<Moving>();
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
				GameObject target = hit.collider.gameObject; // getroffenes Objekt
				// Wenn Objekt aktuell begehter Pfad ist
				if (target == moving.walkPath) {
					moving.goToX(hit.point.x);
				}	
			}
		}
	}
	
	void FixedUpdate () {
		// Bewegung links bei Pfeil links
		if (Input.GetKey(KeyCode.LeftArrow)) {
			moving.moveLeft();
		}		
		// Bewegung rechts bei Pfeil rechts
		else if (Input.GetKey(KeyCode.RightArrow)) {
			moving.moveRight();
		}
		// Stopt Bewegung, wenn keiner der beiden Pfeile gedrückt wird
		else {
			moving.stopMoving();
		}
		
		goToMouseClick(); // Checkt Klick zum Bewegen
	}
}
