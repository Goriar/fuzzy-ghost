using UnityEngine;
using System.Collections;

public class FieldOfView : MonoBehaviour {
	
	public GameObject npc;
	private Moving npcMov;
	private DirectionEnum viewDirection;
	private float distance;
	private float sizeDifference;
	// Use this for initialization
	void Start () {
		npcMov = npc.GetComponent<Moving>();
		viewDirection = DirectionEnum.LEFT;
		distance = npc.transform.position.x - this.transform.position.x;
		if(distance<0)
			distance*=-1;
		
		sizeDifference = npc.transform.position.y - this.transform.position.y;
		if(sizeDifference<0)
			sizeDifference*=-1;
	}
	
	// Update is called once per frame
	void Update () {
		if(viewDirection!=npcMov.viewDirection){
		
			viewDirection = npcMov.viewDirection;
				if(viewDirection == DirectionEnum.LEFT){
					Vector3 newPos = new Vector3(npc.transform.position.x+distance,
												 npc.transform.position.y+sizeDifference,
												 npc.transform.position.z);
					this.gameObject.transform.position = newPos;
				}
				if(viewDirection == DirectionEnum.RIGHT){
					Vector3 newPos = new Vector3(npc.transform.position.x-distance,
												 npc.transform.position.y+sizeDifference,
												 npc.transform.position.z);
					this.gameObject.transform.position = newPos;
				}
			
		}
	}
	
	void OnTriggerEnter(Collider other){
		if(other.gameObject.Equals(GameObject.FindGameObjectWithTag("Player"))){
			Debug.Log("Enemy Detected!");	
		}
	}
}
