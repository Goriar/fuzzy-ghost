using UnityEngine;
using System.Collections;
using System.Xml;

public class Character : MonoBehaviour
{
	public string name;
	
	public StateMachine stateMachine{get;set;}
	Moving movingComponent;
	public RoomInventory currentLocation;
	public GameObject[] objectsOfInterest;
	public GameObject currentObjectOfInterest;
	public float currentValue;
	public float [] objectOfInterestValues;
	public GameObject[] characterPath;
	
	
	private bool enemyDetected;
	public bool EnemyDetected{get;set;}
	
	private float scareLevel;				// Aktuelles Erschreckfortschritt
	
	public float superstitionFactor;		// Aberglaube Faktor (von 0 bis 2)
	
	public bool readyToTalk;
	public bool talking;
	public bool npcDetected;
 	string[] currentThingsToSay;
	public Character chatPartner;
	XmlDocument xml;
	
	public float dialogueTime;
	
	
	
	// Use this for initialization
	void Start ()
	{
		stateMachine = new StateMachine(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(),this);
		characterPath = new GameObject[0];
		movingComponent = this.gameObject.GetComponent<Moving>();
		
		if(objectsOfInterest.Length == 0)
			Debug.LogError("KEINE OBJEKTE ANGEGEBEN FÜR: "+this.ToString());
		
		currentValue = 0;
		currentObjectOfInterest = null;
		
		objectOfInterestValues = new float[objectsOfInterest.Length];
		for(int i = 0; i < objectOfInterestValues.Length; ++i)
		{
			objectOfInterestValues[i] = Random.value;
		}
		
		scareLevel = 0.0f;
		// Setze Aberglaubefaktor auf Grenzen, falls unter-/überschritten
		superstitionFactor = (superstitionFactor > 2f) ? 2f : superstitionFactor;
		superstitionFactor = (superstitionFactor < 0f) ? 0f : superstitionFactor;
		
		readyToTalk = true;
		talking = false;
		dialogueTime = 0.0f;
		
		xml = new XmlDocument();
		xml.Load(".\\Assets\\Scripts\\AI\\ThingsToSay.xml");
		XmlNode node;
		XmlNodeList nlist = xml.GetElementsByTagName(name);
		if(nlist.Count == 0){
			node = xml.GetElementsByTagName("TestCharacter")[0];
		} else {
			node = nlist[0];
		}
		
		currentThingsToSay = new string[node.ChildNodes.Count];
		for(int i = 0; i < currentThingsToSay.Length; ++i){
			currentThingsToSay[i] = node.ChildNodes[i].InnerText;
		}
		
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		stateMachine.stateUpdate();
		updateObjectOfInterestList();
		dialogueTime += Time.deltaTime;
		if(dialogueTime >20.0f){
			readyToTalk = true;	
		}
	}
	
	public void scare (float scareAddition) {
		scareLevel += scareAddition*superstitionFactor;
	}
	
	public string getDialogue(){
		if(scareLevel < 2.5f){
			return currentThingsToSay[0];
		} 
		if(scareLevel < 7.5f){
			return currentThingsToSay[1];
		}
		return currentThingsToSay[2];
	}
	
	public RoomInventory getCurrentLocation()
	{
		return currentLocation;
	}
	
	RoomInventory getMainFloorNeighbour()
	{
		RoomInventory[] neighbours = currentLocation.getNeighbouringRooms();
		foreach(RoomInventory ri in neighbours)
		{
			if(ri.isMainFloor)
				return ri;
		}
		
		return null;
	}
	
	
	bool roomContainsObject(RoomInventory room, GameObject obj)
	{
		foreach(GameObject g in room.objects)
		{
			if(g.Equals(obj))
				return true;
		}
		
		return false;
	}
	
	public GameObject[] getCharacterPath()
	{
		return characterPath;	
	}
	
	public GameObject popNextTarget()
	{
		GameObject obj = characterPath[0];
		GameObject[] newCharacterPath = new GameObject[characterPath.Length-1];
		for(int i = 0; i < newCharacterPath.Length; ++i)
		{
			newCharacterPath[i] = characterPath[i+1];	
		}
		characterPath = newCharacterPath;
		return obj;
	}
	
	protected void assignNextObjectOfInterest()
	{
		int changedIndex = 0;
		for(int i = 0; i < objectsOfInterest.Length; ++i)
		{
			if(currentValue < objectOfInterestValues[i])
			{
				currentValue = objectOfInterestValues[i];
				currentObjectOfInterest = objectsOfInterest[i];
				changedIndex = i;
			}
		}
		objectOfInterestValues[changedIndex] = 0;
	}
	
	protected void updateObjectOfInterestList()
	{
		for(int i = 0; i < objectOfInterestValues.Length; ++i)
		{
			objectOfInterestValues[i] += Time.deltaTime * Random.value;
		}
	}
	
	public void resetCurrentValue()
	{
		currentValue = 0;
	}
	
	private void addToPath(GameObject obj)
	{
		GameObject[] newCharacterPath = new GameObject[characterPath.Length+1];
		for(int i = 0; i < characterPath.Length; ++i)
		{
			newCharacterPath[i] = characterPath[i];	
		}
		newCharacterPath[newCharacterPath.Length-1] = obj;
		characterPath = newCharacterPath;
	}
	
	public Moving getMovingComponent()
	{
		return movingComponent;
	}
	
	public void setCharacterPath()
	{
		characterPath = new GameObject[0];
		assignNextObjectOfInterest();
		if(currentLocation.containsObject(currentObjectOfInterest))
			addToPath(currentObjectOfInterest);
		else
		{
			RoomInventory mainFloor = null;
			GameObject door = null;
			if(!currentLocation.isMainFloor){
				mainFloor = this.getMainFloorNeighbour();
				door = currentLocation.getDoorToRoom(mainFloor);
				addToPath(door);
			}
			else{
				mainFloor = currentLocation;	
			}
			
			if(mainFloor.containsObject(currentObjectOfInterest))
			{
				addToPath(currentObjectOfInterest);
				return;
			}
			
			RoomInventory nextRoom = null;
			foreach(RoomInventory room in mainFloor.getNeighbouringRooms())
			{
				if(roomContainsObject(room,currentObjectOfInterest))
					nextRoom = room;
			}
			
			if(nextRoom == null){
				addToPath(mainFloor.stairs);
				Stairs stairs = mainFloor.stairs.GetComponent<Stairs>();
				if(mainFloor.Equals(stairs.lowerMainFloor))
					mainFloor = stairs.upperMainFloor;
				else
					mainFloor = stairs.lowerMainFloor;
				
				if(mainFloor.containsObject(currentObjectOfInterest))
				{
					addToPath(currentObjectOfInterest);
					return;
				}
				
				foreach(RoomInventory room in mainFloor.getNeighbouringRooms())
				{
					if(roomContainsObject(room,currentObjectOfInterest))
						nextRoom = room;
				}
			}
			
			door = mainFloor.getDoorToRoom(nextRoom);
			addToPath(door);
			addToPath(currentObjectOfInterest);
			
			
			
			/*
			for(int i = 0; i < characterPath.Capacity; ++i)
			{
				//movingComponent.goToCallback += movingComponent.goToObject((GameObject)characterPath.ToArray()[i]);
				//GameObject g = (GameOject)characterPath.ToArray()[i];
				//Door d = g.GetComponent<Door>();
				//if(d!=null)
					//movingComponent.goToCallback += d.use;
			}
			*/
		}
	}
	
	
}

