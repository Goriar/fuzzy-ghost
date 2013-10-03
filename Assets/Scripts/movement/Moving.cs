using UnityEngine;
using System.Collections;

public class Moving : MonoBehaviour {
	
	public InputController controller = null;		// Controller, der Objekt bewegt (standardmäßig keiner)
	
	public float moveSpeed = 2f;					// Bewegungs-Geschwindigkeit des Objekts in m/s
	
	public LayerEnum layer;							// Beinhaltet die Ebene, in der sich das Objekt befindet
	public HouseLevelEnum houseLevel;				// Beinhaltet die Ebene, in der sich das Objekt befindet
	
	public DirectionEnum viewDirection;				// Richtung, in der das Objekt schaut			
	public DirectionEnum moveDirection;				// Bewegungsrichtung (dient zum Updaten je FixedUpdate)
	public bool execMovement;						// Bestimmt, ob der Charakter um einen Zug bewegt werden soll (Keyboard / Controller Eingabe)
	
	public Door usableDoor;							// aktuell benutzbare Tür (in greifbarer Nähe)
	
	public bool movementExecuted = true;			// gibt an, ob die Bewegung vollendet wurde
	public bool finishedAction = true; 				// für AI
	private bool currentlyMoving = false;			// gibt an, ob Objekt sich bewegt
	
	private bool locked;							// Bestimm, ob Bewegung ausgeführt werden kann
	
	private float lastIdleAnimation;				// Zeitpunkt der letzten Idle Animation
	public float minIdleAnimationInverval = 8f;		// Minimale Dauer zwischen zwei Idle Animationen  
	
	// Lerp spezifische Attribute
	public bool activeLerp;							// Wahr, wenn aktuell eine Lerp ausgeführt wird
	private float lerpStartTime;					// Startzeit des Lerps
	private Vector3 lerpFrom;						// Startposition des Lerps
	public Vector3 lerpTo;							// Endposition des Lerps
	private float lerpLength;						// Physikalische Länge des Lerps
	
	// Delegates
	public delegate void UseDelegate();
	public UseDelegate goToCallback;
	
	
	///
	/// Use this for initialization
	/// 
	void Start () {
		//moveSpeed = 2f; // 2 m/s Bewegungsgeschw.
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
		if (!locked) {
			moveDirection = DirectionEnum.LEFT;
			viewDirection = DirectionEnum.LEFT;
			BroadcastMessage("playAnimation", "move");
		}
	}
	
	///
	/// Führt Bewegung nach rechts aus
	///
	public void execMoveRight () {
		moveRight();
		currentlyMoving = true;
		execMovement = true;
		activeLerp = false; // bricht automatisierte Bewegung ab
	}	
	
	///
	/// Setzt Direction Bool Werte und Animation
	/// 
	private void moveRight () {
		if (!locked) {
			moveDirection = DirectionEnum.RIGHT;
			viewDirection = DirectionEnum.RIGHT;
			BroadcastMessage("playAnimation", "move");
		}
	}
	
	///
	/// Stopt die Bewegung
	/// 
	public void stopMoving () {
		if (!activeLerp) {
			currentlyMoving = false;
			execMovement = false;
			moveDirection = DirectionEnum.NONE;
			BroadcastMessage("stopAnimation", "move");
		}	
	}
	
	/// 
	/// aktiviert Ebenenwechsel auf switchLayer Variable
	///
	public void startLayerSwitch () {
		if (transform.position.x == usableDoor.transform.position.x) {
			this.startLayerSwitch(usableDoor.switchTo, usableDoor.switchDirection);
		} else {
			goToObject(usableDoor.gameObject, this.startLayerSwitch);
		}
	}
	
	public void startLayerSwitch (Door door) {
		usableDoor = door;
		startLayerSwitch();
	}
	
	///
	/// aktiviert Ebenewechsel
	/// @param layer Ebene, auf die gewechselt werden soll
	/// 
	public void startLayerSwitch (LayerEnum layer, DirectionEnum direction) {
		finishedAction =false;
		if (!locked) {
			// Bewegung blockieren während Objekt Ebene wechselt
			lockMovement();
			
			this.layer = layer;
			
			// Setzt die neue Z Position abhängig von layer
			float zPos = transform.localPosition.z;
			switch (layer) {
				case LayerEnum.FRONT:
					zPos = ObjectRegister.getLayer("layer_front").transform.localPosition.z;	
					break;
				case LayerEnum.MID:
					zPos = ObjectRegister.getLayer("layer_mid").transform.localPosition.z;		
					break;
				case LayerEnum.BACK:
					zPos = ObjectRegister.getLayer("layer_back").transform.localPosition.z;		
					break;
			}
			
		
			// Bewege Objekt an neue Z-Position mit Geschwindigkeit moveSpeed
			// (mal 0.5 -> ich sollte noch herausfinden, was für eine "Einheit" iTween für Speed benutzt)
			Hashtable ht = new Hashtable();
			ht.Add("speed", moveSpeed * 0.3f);
			ht.Add("z", zPos);
			ht.Add("easetype", "easeInOutSine");
			ht.Add("oncomplete", "stopLayerSwitch");
			iTween.MoveTo(gameObject, ht);
		
			// Setze Blickrichtung und spiele Animation
			viewDirection = direction;
			
			if (viewDirection == DirectionEnum.FORE) {
				BroadcastMessage("playAnimation", "moveFore");
			} else if (viewDirection == DirectionEnum.BACK) {
				BroadcastMessage("playAnimation", "moveBack");
			}
			
			// Wenn Objekt von Kamera verfolgt wird, blende ggf. Layers aus / blende sie ein
			if (Camera.main.GetComponent<CameraView>().target == transform) {
				switch (layer) {
					case LayerEnum.FRONT:
						ObjectRegister.getLayer("layer_front").GetComponent<Layer>().changeVisibilityByDependence();
						break;
					case LayerEnum.MID:
						ObjectRegister.getLayer("layer_mid").GetComponent<Layer>().changeVisibilityByDependence();
						break;
					case LayerEnum.BACK:
						ObjectRegister.getLayer("layer_back").GetComponent<Layer>().changeVisibilityByDependence();
						break;
				}
			}	
		}		
	}
	
	public void stopLayerSwitch () {
		if (gameObject.GetComponent<Player>() == null) {
			usableDoor.close();
			if (usableDoor.otherSide != null) {
				usableDoor.otherSide.close();
			}
		}
		unlockMovement();
		viewDirection = DirectionEnum.LEFT;
		BroadcastMessage("stopAnimation", "moveFore");
		BroadcastMessage("stopAnimation", "moveBack");
		finishedAction = true;
		
		
		// Erhalte Raumkomponente
		Transform tempRoom = usableDoor.otherSide.transform;
		RoomInventory roomComp = null;
		while (true) {
			// Oberstes Element in der Hierarchie erreicht => beende Loop
			if (tempRoom.parent == null) {
				break;
			} else if (tempRoom.parent.GetComponent<RoomInventory>() != null) {
				roomComp = tempRoom.parent.GetComponent<RoomInventory>();
				break;
			} else {
				tempRoom = tempRoom.parent;
			}
		}
		
		// Wenn Objekt NPC ...
		Character chara = gameObject.GetComponent<Character>();
		Player player = gameObject.GetComponent<Player>();
		if (chara != null) {
			chara.setCurrentLocation(roomComp);
			chara.setCharacterPath(chara.currentObjectOfInterest);
		// Wenn Objekt Player ...
		} else if (player != null) {
			player.setCurrentLocation(roomComp);
		}
	}
		
	///
	/// sperrt Bewerung
	/// 
	public void lockMovement () {
		locked = true;
	}
	
	///
	/// entsperrt Bewegung
	/// 
	public void unlockMovement () {
		locked = false;
	}
	
	public bool isMovementLocked() {
		return locked;
	}
	
	public bool isMoving() {
		return currentlyMoving;
	}
		
	
	public void deactivateLerp(){
		activeLerp = false;
				stopMoving();
				finishedAction = true;
				if (goToCallback != null) {
					goToCallback();
					goToCallback = null;
					movementExecuted = true;
				}
	}
	
	public void stopLerp(){
		activeLerp = false;
		stopMoving();
		finishedAction = true;
		goToCallback = null;
		movementExecuted = true;
	}
	/// 
	/// Bewegung zu Punkt x
	/// @param x X-Achsen Position, auf die gegangen werden soll
	/// 
	public void goToX (float x) {
			currentlyMoving = true;
			movementExecuted = false;
			finishedAction = false;
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
		this.goToObject(o, null);
	}
	public void goToObject (GameObject o, UseDelegate callback) {
		goToX(o.transform.position.x);
		goToCallback += callback;
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
				currentlyMoving = false;
				stopMoving();
				finishedAction = true;
				if (goToCallback != null) {
					goToCallback();
					goToCallback = null;
					movementExecuted = true;
				}
			}
		}
	}
		
	public void faceLeft(){
		viewDirection = DirectionEnum.LEFT;
		moveDirection = DirectionEnum.LEFT;
		BroadcastMessage("playAnimation", "move");	
		BroadcastMessage("playAnimation", "idle");
	}
	
	public void faceRight(){
		viewDirection = DirectionEnum.RIGHT;
		moveDirection = DirectionEnum.RIGHT;
		BroadcastMessage("playAnimation", "move");
		BroadcastMessage("playAnimation", "idle");
	}
	
	/// 
	/// Fixed Update für Eingaben bzw. Physik
	/// 
	void FixedUpdate () {
		if (!locked) {
			// Wenn Bewegung nach links update nach links mit Geschwindigkeit
			if (execMovement && moveDirection == DirectionEnum.LEFT) {
				transform.Translate(Vector3.left * moveSpeed * Time.fixedDeltaTime);
			}
			
			// Wenn Bewegung nach rechts, update nach rechts mit Geschwindigkeit
			if (execMovement && moveDirection == DirectionEnum.RIGHT) {
				transform.Translate(Vector3.right * moveSpeed * Time.fixedDeltaTime);
			}
					
			doLerp(); // Führt Lerp aus, falls aktiv
			
			// Idle Animation ausführen, wenn Objekt steht und Animation noch nicht so lange her war (Min Idle Interval)
			if (moveDirection == DirectionEnum.NONE && Time.time > (lastIdleAnimation + minIdleAnimationInverval)) {
				int idleRand = Random.Range(0,200); // Random Abspielen der Idle Animation
				if (idleRand == 0) {
					lastIdleAnimation = Time.time;
					BroadcastMessage("playAnimation", "idle");
				}
			}
		}
		
	}
}
