using UnityEngine;
using System.Collections;

public class CameraView : MonoBehaviour {
	
	public Transform target;				// Ziel der Kamera
	public float zDistance = 10f;			// Distanz der Kamera vom Spieler
	public float yDistance = 10f;			// Distanz der Kamera vom Spieler
	public float yOffset = 0;				// Versatz der Kamera entlang der y Achse
	private float layerVisibilityTime;		// Dauer des hide/show lerps für die Layer
	
	///
	/// Use this for initialization
	///
	void Start () {
		setNewPos();
	}
	
	// Update is called once per frame
	void Update () {
		// Folgt Target Objekt und schaut auf es
		setNewPos();
		transform.LookAt(target);
		
		setYOffset();
	}
		
	// Setzt neue Position für die Kamera abhängig vom Target Objekt
	void setNewPos () {
		Vector3 pos = target.transform.position; // Aktuelle Target Position
		Vector3 camPos = new Vector3(pos.x, pos.y + yDistance, pos.z + zDistance); // neu berechnete Kamera Position
		transform.position = camPos;
	}
	
	void setYOffset () {
		transform.position = new Vector3(transform.position.x, (transform.position.y + yOffset), transform.position.z);
	}
}
