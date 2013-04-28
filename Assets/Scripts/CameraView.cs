using UnityEngine;
using System.Collections;

public class CameraView : MonoBehaviour {
	
	public Transform target;				// Ziel der Kamera
	public float distance = 10f;			// Distanz der Kamera vom Spieler
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
	}
		
	// Setzt neue Position für die Kamera abhängig vom Target Objekt
	void setNewPos () {
		Vector3 pos = target.transform.position; // Aktuelle Target Position
		Vector3 camPos = new Vector3(pos.x, pos.y + distance*0.075f, pos.z + distance*0.2f); // neu berechnete Kamera Position
		transform.position = camPos;
	}
}
