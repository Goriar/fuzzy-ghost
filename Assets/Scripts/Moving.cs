using UnityEngine;
using System.Collections;

public class Moving : MonoBehaviour {
	
	public float moveSpeed;						// holds the speed the objects moves every update in m/s
	public LayerEnum layer;						// hold the layer the object is in
	public HouseLevelEnum houseLevel;			// holds the level, the object is in
	public DirectionEnum viewDirection;			// saves the direction the object is facing			
	public bool switchBack;						// is true, when object can switch layer backward
	public bool switchFore;						// is true, when object can switch layer forward
	
	// Use this for initialization
	void Start () {
		moveSpeed = 2f;
		layer = LayerEnum.FRONT;
		houseLevel = HouseLevelEnum.LOWER;
		viewDirection = DirectionEnum.LEFT;
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
				
		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.Translate(Vector3.left * moveSpeed * Time.fixedDeltaTime);
			viewDirection = DirectionEnum.LEFT;
		}
		
		if (Input.GetKey(KeyCode.RightArrow)) {
			transform.Translate(Vector3.right * moveSpeed * Time.fixedDeltaTime);
			viewDirection = DirectionEnum.RIGHT;
		}
		
		if (Input.GetKey(KeyCode.UpArrow)) {
			if (switchBack) {
				LayerSwitch layerSwitch = GetComponent<LayerSwitch>();
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
		
		if (Input.GetKey(KeyCode.DownArrow)) {
			if (switchFore) {
				LayerSwitch layerSwitch = GetComponent<LayerSwitch>();
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
		
	}
}
