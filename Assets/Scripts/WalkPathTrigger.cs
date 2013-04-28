using UnityEngine;
using System.Collections;

public class WalkPathTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		other.gameObject.GetComponent<Moving>().walkPath = gameObject;
	}
	
}
