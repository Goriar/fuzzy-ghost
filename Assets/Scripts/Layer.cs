using UnityEngine;
using System.Collections;

public class Layer : MonoBehaviour {
	
	private float showSpeed = 1f;
	private float hideSpeed = 1f;
	private float showDelay = 0f;
	private float hideDelay = 0.5f;
	
	// Use this for initialization
	void Start () {
		ObjectRegister.registerLayer(gameObject);
	}
	
	void hideLayer () {
		gameObject.GetComponent<Hide>().hide(hideSpeed, hideDelay);
	}
	
	void showLayer () {
		gameObject.GetComponent<Hide>().show(showSpeed, showDelay);
	}
	
}
