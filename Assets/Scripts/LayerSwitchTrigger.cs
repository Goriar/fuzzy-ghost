using UnityEngine;
using System.Collections;

public class LayerSwitchTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if(tag == "trigger_switch_back") {
			other.gameObject.GetComponent<Moving>().switchBack = true; // if tag is 'trigger_switch_back', set collider switchBack to true -> enter
		} else if (tag == "trigger_switch_fore") {
			other.gameObject.GetComponent<Moving>().switchFore = true; // if tag is 'trigger_switch_fore', set collider switchBack to true -> enter
		}
	}
	
	void OnTriggerExit(Collider other) {
		if(tag == "trigger_switch_back") {
			other.gameObject.GetComponent<Moving>().switchBack = false;  // if tag is 'trigger_switch_back', set collider switchBack to false -> exit
		} else if (tag == "trigger_switch_fore") {
			other.gameObject.GetComponent<Moving>().switchFore = false;  // if tag is 'trigger_switch_back', set collider switchBack to false -> exit
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
