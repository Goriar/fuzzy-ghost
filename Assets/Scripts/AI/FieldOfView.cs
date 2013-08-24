using UnityEngine;
using System.Collections;

public class FieldOfView : MonoBehaviour {
	
	public GameObject npc;
	private Moving npcMov;
	private DirectionEnum viewDirection;
	private float distance;
	private float sizeDifference;
	private Vector3 scale;
	// Use this for initialization
	void Start () {
		scale = this.gameObject.transform.localScale;
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
				this.gameObject.transform.localScale = scale;
				Vector3 newPos = new Vector3(npc.transform.position.x+distance,
											 npc.transform.position.y+sizeDifference,
											 npc.transform.position.z);
				this.gameObject.transform.position = newPos;
			}
			if(viewDirection == DirectionEnum.RIGHT){
				this.gameObject.transform.localScale = scale;
				Vector3 newPos = new Vector3(npc.transform.position.x-distance,
											 npc.transform.position.y+sizeDifference,
											 npc.transform.position.z);
				this.gameObject.transform.position = newPos;
			}
			if(viewDirection == DirectionEnum.FORE || 
				viewDirection == DirectionEnum.BACK || 
				viewDirection == DirectionEnum.NONE){
			
				this.gameObject.transform.localScale = new Vector3(0,0,0);
			}
			
		}
	}
	
	void OnTriggerEnter(Collider other){
		Item item = other.GetComponent<Item>();
		if(item != null){
			if(item.scareFactor>0){
				Character ch = npc.GetComponent<Character>();
				ch.stateMachine.changeState(StateType.SCARED_STATE);
				Debug.Log("SCAAAAARED!");
				return;
			}
		}
		Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		if(other.gameObject.Equals(GameObject.FindGameObjectWithTag("Player"))&& player.canBeSeen()){
			Character ch = npc.GetComponent<Character>();
			ch.EnemyDetected = true;
		}
		if(other.gameObject.GetComponent<Character>()!=null){
			Character thisNpc = npc.GetComponent<Character>();
			Character otherNpc = other.gameObject.GetComponent<Character>();
			thisNpc.npcDetected =true;
			otherNpc.npcDetected = true;
			thisNpc.chatPartner = otherNpc;
			otherNpc.chatPartner = thisNpc;
		}
		
	}
	
	void OnTriggerExit(Collider other){
		if(other.gameObject.Equals(GameObject.FindGameObjectWithTag("Player"))){
			Character ch = npc.GetComponent<Character>();
			ch.EnemyDetected = false;
		}
		if(other.gameObject.GetComponent<Character>()!=null){
			Character thisNpc = npc.GetComponent<Character>();
			Character otherNpc = other.gameObject.GetComponent<Character>();
			thisNpc.npcDetected =false;
			otherNpc.npcDetected = false;
			thisNpc.chatPartner = null;
			otherNpc.chatPartner = null;
		}
	}
	
	
}
