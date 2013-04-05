using UnityEngine;
using System.Collections;

public class Moving : MonoBehaviour {
	
	public float moveSpeed;
	
	// Use this for initialization
	void Start () {
		moveSpeed = 10f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
		}
		
		if (Input.GetKey(KeyCode.RightArrow)) {
			transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
		}
		
		if (Input.GetKey(KeyCode.DownArrow)) {
			transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
		}
		
		if (Input.GetKey(KeyCode.UpArrow)) {
			transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
		}
		
	}
}
