using UnityEngine;
using System.Collections;

public class LayerSwitch : MonoBehaviour {
	
	public float speed = 2f;
	
	private Vector3 startPos;
	private Vector3 endPos;
	private LayerEnum endLayer;
	private bool active;
	private float startTime;
	private float journeyLength;
	
	
	// Use this for initialization
	void Start () {
		active = false;
		startTime = 0f;
		
	}
	
	public void switchLayers (LayerEnum layer) {
		startTime = Time.time;
		startPos = transform.position;
		
		// Get Z position of the layer to switch to
		float zPos = 0f; 
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
		
		// set end position with current x,y position and new z position of layer
		endPos = new Vector3(transform.position.x, transform.position.y, zPos);
		endLayer = layer;
		
		// set journey length with the 2 positions
		journeyLength = Vector3.Distance(startPos, endPos);
		
		// Wenn Objekt von Kamera verfolgt wird, blende ggf. Layers aus / blende sie ein
		if (Camera.main.GetComponent<CameraView>().target == transform) {
			switch (layer) {
			case LayerEnum.FRONT:
				ObjectRegister.getLayer("layer_front").SetActive(true);
				ObjectRegister.getLayer("layer_front").BroadcastMessage("showLayer");		
				break;
			case LayerEnum.MID:
				ObjectRegister.getLayer("layer_mid").SetActive(true);
				ObjectRegister.getLayer("layer_mid").BroadcastMessage("showLayer");
				ObjectRegister.getLayer("layer_front").BroadcastMessage("hideLayer");
				break;
			case LayerEnum.BACK:
				ObjectRegister.getLayer("layer_back").SetActive(true);
				ObjectRegister.getLayer("layer_back").BroadcastMessage("show");
				ObjectRegister.getLayer("layer_mid").BroadcastMessage("hide");
				ObjectRegister.getLayer("layer_front").BroadcastMessage("hide");
				break;
			}
		}
		
		active = true;
	}
	
	
	
	// Update is called once per frame
	void Update () {
		if (active) {
			float distCovered = (Time.time - startTime) * speed;
        	float fracJourney = distCovered / journeyLength;
        	transform.position = Vector3.Lerp(startPos, endPos, fracJourney);
			
			// set active to false, if object reached end position	
			if (transform.position.z == endPos.z) {
				Moving moving = GetComponent<Moving>();
				moving.layer = endLayer;
				active = false; 
			}
		}
	}
}
