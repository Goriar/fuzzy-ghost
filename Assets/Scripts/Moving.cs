using UnityEngine;
using System.Collections;

public class Moving : MonoBehaviour {
	
	public float moveSpeed;						// Bewegungs-Geschwindigkeit des Objekts in m/s
	
	public LayerEnum layer;						// Beinhaltet die Ebene, in der sich das Objekt befindet
	public HouseLevelEnum houseLevel;			// Beinhaltet die Ebene, in der sich das Objekt befindet
	
	public DirectionEnum viewDirection;			// Richtung, in der das Objekt schaut			
	public DirectionEnum moveDirection;			// Bewegungsrichtung (dient zum Updaten je FixedUpdate)
	public bool execMovement;					// Bestimmt, ob der Charakter um einen Zug bewegt werden soll (Keyboard / Controller Eingabe)
	
	public bool switchBack;						// wahr, wenn Objekt Ebene nach hinten wechseln kann (bei Tür)
	public bool switchFore;						// wahr wenn Objekt Ebene nach vorne wechseln kann (bei Tür)
	
	// Lerp spezifische Attribute
	public bool activeLerp;						// Wahr, wenn aktuell eine Lerp ausgeführt wird
	private float lerpStartTime;				// Startzeit des Lerps
	private Vector3 lerpFrom;					// Startposition des Lerps
	public Vector3 lerpTo;						// Endposition des Lerps
	private float lerpLength;					// Physikalische Länge des Lerps
	
	///
	/// Use this for initialization
	/// 
	void Start () {
		moveSpeed = 2f; // 2 m/s Bewegungsgeschw.
		layer = LayerEnum.FRONT; // ist in vorderster Ebene
		houseLevel = HouseLevelEnum.LOWER; // Erdgeschoss
		viewDirection = DirectionEnum.LEFT; // schaut nach links
		moveDirection = DirectionEnum.NONE; // keine Bewegung am Anfang
	}
	
	///
	/// Führt Bewegung nach links aus
	///
	public void execMoveLeft () {
		moveLeft();
		execMovement = true;
		activeLerp = false; // bricht automatisierte Bewerung ab
	}	
	
	///
	/// Setzt Direction Bool Werte und Animation
	/// 
	private void moveLeft () {
		moveDirection = DirectionEnum.LEFT;
		viewDirection = DirectionEnum.LEFT;
		BroadcastMessage("playAnimation", "move");
	}
	
	///
	/// Führt Bewegung nach rechts aus
	///
	public void execMoveRight () {
		moveRight();
		execMovement = true;
		activeLerp = false; // bricht automatisierte Bewegung ab
	}	
	
	///
	/// Setzt Direction Bool Werte und Animation
	/// 
	private void moveRight () {
		moveDirection = DirectionEnum.RIGHT;
		viewDirection = DirectionEnum.RIGHT;
		BroadcastMessage("playAnimation", "move");
	}
	
	
	
	///
	/// Stopt die Bewegung
	/// 
	public void stopMoving () {
		if (!activeLerp) {
			execMovement = false;
			moveDirection = DirectionEnum.NONE;
			BroadcastMessage("stopAnimation");
		}	
	}
		
	/// 
	/// Bewegung zu Punkt x
	/// 
	public void goToX (float x) {
		
		// Wenn x links von Objekt, blicke links, wenn rechts von Objekt, rechts
		if (x > transform.position.x) {
			viewDirection = DirectionEnum.LEFT;
			moveDirection = DirectionEnum.LEFT;
			moveLeft();
		}
		if (x < transform.position.x) {
			viewDirection = DirectionEnum.RIGHT;
			moveDirection = DirectionEnum.RIGHT;
			moveRight();
		}
		
		// Setze notwendige Werte für Lerp
		lerpFrom = new Vector3(transform.position.x, transform.position.y, transform.position.z); // Lerp von aktueller Position
		lerpTo = new Vector3(x, transform.position.y, transform.position.z); // Lerp zu neuer X Pos, andere Achsen bleiben gleich
		lerpStartTime = Time.time; // Startzeit = aktuelle Zeit
		lerpLength = Vector3.Distance(lerpFrom, lerpTo);
		activeLerp = true; // aktiviert Lerp
	}
	
	///
	/// Obsolet oder hauen wir das Ganze hier rein, damit Bewegung immer zusammen gehört?
	/// @param o GameObjekt, zu dem bewegt werden soll
	/// 
	public void goToObject (GameObject o) {
	}
	
	/// 
	/// führt Lerp aus, wenn gerade ein Lerp aktiv ist (activeLerp == true)
	/// Wird dauerhaft von FixedUpdate aufgerufen
	/// 
	private void doLerp () {
		if (activeLerp) {
			float distCovered = (Time.time - lerpStartTime) * moveSpeed; // schon absolvierte Distanz
        	float fracJourney = distCovered / lerpLength; // Fortschritt des Lerp von 0 bis 1
        	transform.position = Vector3.Lerp(lerpFrom, lerpTo, fracJourney); // setze Position neu mittels Lerp
			
			// Deaktiviere Lerp, wenn Startpos = endpos	
			if (transform.position.x == lerpTo.x && transform.position.y == lerpTo.y && transform.position.z == lerpTo.z) {
				activeLerp = false;
				stopMoving();
			}
		}
	}
	
	/// 
	/// Fixed Update für Eingaben bzw. Physik
	/// 
	void FixedUpdate () {
		
		// Wenn Bewegung nach links update nach links mit Geschwindigkeit
		if (execMovement && moveDirection == DirectionEnum.LEFT) {
			transform.Translate(Vector3.left * moveSpeed * Time.fixedDeltaTime);
		}
		
		// Wenn Bewegung nach rechts, update nach rechts mit Geschwindigkeit
		if (execMovement && moveDirection == DirectionEnum.RIGHT) {
			transform.Translate(Vector3.right * moveSpeed * Time.fixedDeltaTime);
		}
		
		// Muss noch geändert werden!
		// Layer Switch bei Pfeil nach oben
		if (Input.GetKey(KeyCode.UpArrow)) {
			// Wenn Objekt nach hinten gehen kann, dann ...
			if (switchBack) {
				LayerSwitch layerSwitch = GetComponent<LayerSwitch>();
				// Layer switch je nach aktueller Layer
				switch (layer) {
				case LayerEnum.FRONT:
					layerSwitch.switchLayers(LayerEnum.MID);
					break;
				case LayerEnum.MID:
					layerSwitch.switchLayers(LayerEnum.BACK);
					break;
				}
			}
		}
		
		
		// Muss noch geändert werden!
		// Layer Switch bei Pfeil nach 
		if (Input.GetKey(KeyCode.DownArrow)) {
			// Wenn Objekt nach vorne gehen kann, dann ...
			if (switchFore) {
				LayerSwitch layerSwitch = GetComponent<LayerSwitch>();
				// Layer switch je nach aktueller Layer
				switch (layer) {
				case LayerEnum.BACK:
					layerSwitch.switchLayers(LayerEnum.MID);
					break;
				case LayerEnum.MID:
					layerSwitch.switchLayers(LayerEnum.FRONT);
					break;
				}
			}
		}
		
		
		doLerp(); // Führt Lerp aus, falls aktiv
		
	}
}
