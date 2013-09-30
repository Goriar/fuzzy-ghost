using UnityEngine;
using System.Collections;

public class FieldOfView : MonoBehaviour {
	
	public GameObject npc;
	private Moving npcMov;
	private DirectionEnum viewDirection;
	private float distance;
	private float sizeDifference;
	private Vector3 scale;
	
	private float timer;
	private GameObject despawnObject;
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
		
		timer = 0.0f;
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
		if(despawnObject!=null){
				timer+=Time.deltaTime;
				if(timer>=15.0f){
					Respawner respawn = GameObject.FindWithTag("MainCamera").GetComponent<Respawner>();
					respawn.addToRespawnList(despawnObject);
					despawnObject.GetComponent<Interactable>().enabled=true;
					despawnObject.GetComponent<Item>().used = false;
					despawnObject.SetActive(false);
					despawnObject = null;
					timer = 0.0f;
				}
			}
	}
	
	void OnTriggerEnter(Collider other){
		Item item = other.GetComponent<Item>();
		Character ch = npc.GetComponent<Character>();
		Item testForRoom = null;
		for(int i = 0; i<ch.currentLocation.objects.Length; ++i){
			if(item == ch.currentLocation.objects[i].GetComponent<Item>()){
				testForRoom = item;
			}
		}
		Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		if(item != null && testForRoom !=null){
			if(item.getScaryness()>0 && !item.used){
				ch = npc.GetComponent<Character>();
				ch.stateMachine.changeState(StateType.SCARED_STATE);
				ch.scare(item.getScaryness());
				item.curse(false);
				item.used = true;
				player.raiseAttention(item.getAttentionFactor());
				despawnObject = other.gameObject;
				Debug.Log("SCAAAAARED!");
				return;
			}
		}
		if(other.gameObject.Equals(GameObject.FindGameObjectWithTag("Player"))
			&& ch.gameObject.GetComponent<Character>().currentLocation == player.currentLocation){
			ch = npc.GetComponent<Character>();
			if(ch.cType == CharacterType.NORMAL && player.canBeSeen()){
				ch.enemyDetected = true;
				ch.stateMachine.changeState(StateType.ENEMY_DETECTED_STATE);
			}
			if(ch.cType == CharacterType.GHOST_HUNTER){
				ch.enemyDetected = true;
				ch.stateMachine.changeState(StateType.HUNTING_ENEMY_STATE);
			}
		}
		if(other.gameObject.GetComponent<Character>()!=null 
			&& other.gameObject.GetComponent<Character>().currentLocation == ch.currentLocation){
			Character thisNpc = npc.GetComponent<Character>();
			Character otherNpc = other.gameObject.GetComponent<Character>();
			if(thisNpc.readyToTalk && otherNpc.readyToTalk){
				thisNpc.chatPartner = otherNpc;
				otherNpc.chatPartner = thisNpc;
				thisNpc.stateMachine.changeState(StateType.TALKING_STATE);
				otherNpc.stateMachine.changeState(StateType.TALKING_STATE);
			}
		}
		
	}
	
	void OnTriggerExit(Collider other){
		if(other.gameObject.Equals(GameObject.FindGameObjectWithTag("Player"))){
			Character ch = npc.GetComponent<Character>();
			ch.enemyDetected = false;
		}
		if(other.gameObject.GetComponent<Character>()!=null){
			Character thisNpc = npc.GetComponent<Character>();
			Character otherNpc = other.gameObject.GetComponent<Character>();
			thisNpc.chatPartner = null;
			otherNpc.chatPartner = null;
		}
	}
	
	
}
