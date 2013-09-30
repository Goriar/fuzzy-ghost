using UnityEngine;
using System.Collections;

public class LayerSwitch : MonoBehaviour {
	
	public float speed = 2f;									// Geschwindigkeit der Animation
	
	private Vector3 startPos;								// Startposition für Animation
	private Vector3 endPos;									// Endposition für Animation
	private LayerEnum endLayer;							// Ebene, in die animiert werden soll
	private bool active;											// Animation aktiv
	private float startTime;									// Startzeit der Animation
	private float journeyLength;							// Dauer der Animation
	
	
	// Use this for initialization
	void Start () {
		active = false;
		startTime = 0f;
		
	}
	
	///
	/// Aktiviere Layer Wechsel
	/// @param layer Ebene, in die gewechselt werden soll
	///
	public void switchLayers (LayerEnum layer) {
		// Startzeit und -position für den Lerp
		startTime = Time.time;
		startPos = transform.position;
		
		// Erhalte Z-Position der Layer, in die gewechselt werden soll
		float zPos = 0f; 
		switch (layer) {
		case LayerEnum.FRONT:
			zPos = GameObject.FindWithTag("layer_front").transform.localPosition.z;		
			break;
		case LayerEnum.MID:
			zPos = GameObject.FindWithTag("layer_mid").transform.localPosition.z;		
			break;
		case LayerEnum.BACK:
			zPos = GameObject.FindWithTag("layer_back").transform.localPosition.z;		
			break;
		}
		
		// Setze Enposition mit aktueller X und Y Position und neu erhaltenen Z-Position der Ebene
		endPos = new Vector3(transform.position.x, transform.position.y, zPos);
		endLayer = layer;
		
		// set journey length with the 2 positions
		journeyLength = Vector3.Distance(startPos, endPos);
		active = true;
	}
	
	
	
	// Update is called once per frame
	void Update () {
		// Wenn Layer Wechsel aktiv
		if (active) {
			// berechne Lerp Parameter
			float distCovered = (Time.time - startTime) * speed;
      	float fracJourney = distCovered / journeyLength;
      	transform.position = Vector3.Lerp(startPos, endPos, fracJourney);
			
			// setze aktiv auf false, wenn Obejekt Endposition erreicht hat	
			if (transform.position.z == endPos.z) {
				Moving moving = GetComponent<Moving>();
				moving.layer = endLayer; // setze layer, des aktuellen Spielers oder NPCs
				active = false;
			}
		}
	}
}
