using UnityEngine;
using System.Collections;

public class FieldOfView : MonoBehaviour {
	
	public GameObject npc;
	private Moving npcMov;
	private DirectionEnum viewDirection;
	private float distance;
	private float sizeDifference;
	private Vector3 scale;
	
	private float[] timer;
	private GameObject[] despawnObjects;
	private int index;
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
		
		timer = new float[8];
		despawnObjects = new GameObject[8];
		index = 0;
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
		for(int i = 0; i<index; ++i){
				timer[i]+=Time.deltaTime;
				if(timer[i]>=15.0f){
					Respawner respawn = GameObject.FindWithTag("MainCamera").GetComponent<Respawner>();
					respawn.addToRespawnList(despawnObjects[i]);
					despawnObjects[i].GetComponent<Interactable>().enabled=true;
					Item item = despawnObjects[i].GetComponent<Item>();
					item.used = false;
					for(int j = 0; j<item.combineObjects.Length; ++j){
						item.combineObjects[j].SetActive(false);
					}
					despawnObjects[i].GetComponent<Item>().originalObject.SetActive(true);
					despawnObjects[i].SetActive(false);
					despawnObjects[i] = null;
					timer[i] = 0.0f;
				}
			if(despawnObjects[index-1] == null)
				index--;
			}
		
		Character ch = npc.GetComponent<Character>();
		Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		if (ch.enemyDetected && player.canBeSeen() && ch.cType == CharacterType.NORMAL) {
			ch.stateMachine.changeState(StateType.ENEMY_DETECTED_STATE);
		} else if(ch.enemyDetected && ch.cType == CharacterType.GHOST_HUNTER) {
			ch.stateMachine.changeState(StateType.HUNTING_ENEMY_STATE);	
		}	


	}
	
	void OnTriggerEnter(Collider other){
		Item item = other.GetComponent<Item>();
		Character ch = npc.GetComponent<Character>();
		Item testForRoom = null;
		for(int i = 0; i<ch.currentLocation.objects.Length; ++i){ 
			if(item == ch.currentLocation.objects[i].GetComponent<Item>()
				|| item == ch.currentLocation.objects[i].GetComponentInChildren<Item>()){
				testForRoom = item;
				break;
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
				Curse curse = other.GetComponent<Curse>();
				if(curse!=null){
					despawnObjects[index] = other.gameObject;
					timer[index] = 0.0f;
					index = index+1<= despawnObjects.Length ? index+1 : index;
				}
				return;
			}
		}
		if(other.gameObject.Equals(GameObject.FindGameObjectWithTag("Player"))
			&& ch.gameObject.GetComponent<Character>().currentLocation == player.currentLocation){
			ch = npc.GetComponent<Character>();
			if(player.currentLocation.Equals(ch.getCurrentLocation())){
				ch.enemyDetected = true;
				
			} else {
				ch.enemyDetected = false;
			}
			
		}
		if(other.gameObject.GetComponent<Character>()!=null 
			&& other.gameObject.GetComponent<Character>().currentLocation == ch.currentLocation
			&& !ch.enemyDetected){
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
