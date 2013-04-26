using UnityEngine;
using System.Collections;

public class Moving : MonoBehaviour {
	
	public float moveSpeed;						// holds the speed the objects moves every update in m/s
	public LayerEnum layer;						// hold the layer the object is in
	public HouseLevelEnum houseLevel;			// holds the level, the object is in
	public DirectionEnum viewDirection;			// saves the direction the object is facing			
	public bool switchBack;						// is true, when object can switch layer backward
	public bool switchFore;						// is true, when object can switch layer forward
	public DirectionEnum moveDirection;
	
	// Use this for initialization
	void Start () {
		moveSpeed = 2f;
		layer = LayerEnum.FRONT;
		houseLevel = HouseLevelEnum.LOWER;
		viewDirection = DirectionEnum.LEFT;
		moveDirection = DirectionEnum.NONE;
	}
	
	public void moveLeft () {
		moveDirection = DirectionEnum.LEFT;
		viewDirection = DirectionEnum.LEFT;
	}
	
	
	public void moveRight () {
		moveDirection = DirectionEnum.RIGHT;
		viewDirection = DirectionEnum.RIGHT;
	}
	
	public void stopMoving () {
		moveDirection = DirectionEnum.NONE;
	}
	
	public void goToPoint () {
	}
	
	public void goToObect (GameObject o) {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
				
		if (moveDirection == DirectionEnum.LEFT) {
			transform.Translate(Vector3.left * moveSpeed * Time.fixedDeltaTime);
		}
		
		if (moveDirection == DirectionEnum.RIGHT) {
			transform.Translate(Vector3.right * moveSpeed * Time.fixedDeltaTime);
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
