using UnityEngine;
using System.Collections;

public class WalkPathTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		Debug.Log("blub");
		other.gameObject.GetComponent<Moving>().walkPath = gameObject;
	}
	
}
