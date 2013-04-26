using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	
	private Moving moving;
	
	// Use this for initialization
	void Start () {
		moving = GameObject.FindGameObjectWithTag("Player").GetComponent<Moving>();
	}
	
	void changeControllable (GameObject o) {
		moving = o.GetComponent<Moving>();
	}
	
	void FixedUpdate () {
		// Move left
		if (Input.GetKey(KeyCode.LeftArrow)) {
			moving.moveLeft();
		}		
		// Move right
		else if (Input.GetKey(KeyCode.RightArrow)) {
			moving.moveRight();
		}
		// Stop Moving if keys are released
		else {
			moving.stopMoving();
		}
	}
}
