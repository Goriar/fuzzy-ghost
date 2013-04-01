using UnityEngine;
using System.Collections;

public class CameraView : MonoBehaviour {
	
	public Transform target;
	public float distance = 10f;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(target);
		
		Vector3 pos = target.transform.position;
		Vector3 camPos = new Vector3(pos.x, pos.y, pos.z + distance);
		transform.position = camPos;
	}
}
