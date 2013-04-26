using UnityEngine;
using System.Collections;

public class Moving : MonoBehaviour {
	
	public float moveSpeed;
	public LayerEnum layer;
	public HouseLevelEnum houseLevel;
	public DirectionEnum viewDirection;
	
	// Use this for initialization
	void Start () {
		moveSpeed = 2f;
		layer = LayerEnum.MID;
		houseLevel = HouseLevelEnum.LOWER;
		viewDirection = DirectionEnum.LEFT;
		
		setLayer();
		setHouseLevel();
	}
	
	private void setLayer () {
		switch (layer) {
		case LayerEnum.FRONT:
			transform.position = new Vector3(transform.position.x, transform.position.y, GameObject.FindWithTag("layer_front").transform.localPosition.z);		
			break;
		case LayerEnum.MID:
			transform.position = new Vector3(transform.position.x, transform.position.y, GameObject.FindWithTag("layer_mid").transform.localPosition.z);		
			break;
		case LayerEnum.BACK:
			transform.position = new Vector3(transform.position.x, transform.position.y, GameObject.FindWithTag("layer_back").transform.localPosition.z);		
			break;
		}
	}
	
	private void setHouseLevel () {
		switch (houseLevel) {
		case HouseLevelEnum.CELLAR:
			transform.position = new Vector3(transform.position.x, GameObject.FindWithTag("house_attic").transform.localPosition.y, transform.position.y);		
			break;
		case HouseLevelEnum.LOWER:
			transform.position = new Vector3(transform.position.x, GameObject.FindWithTag("house_lower").transform.localPosition.y, transform.position.y);		
			break;
		case HouseLevelEnum.UPPER:
			transform.position = new Vector3(transform.position.x, GameObject.FindWithTag("house_upper").transform.localPosition.y, transform.position.y);		
			break;
		case HouseLevelEnum.ATTIC:
			transform.position = new Vector3(transform.position.x, GameObject.FindWithTag("house_attic").transform.localPosition.y, transform.position.y);		
			break;
		}
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
		
		if (Input.GetKey(KeyCode.DownArrow)) {
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
