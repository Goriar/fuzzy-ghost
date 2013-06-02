using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
	StateMachine stateMachine;
	Moving movingComponent;
	public RoomInventory currentLocation;
	public GameObject[] objectsOfInterest;
	public GameObject currentObjectOfInterest;
	public float currentValue;
	public float [] objectOfInterestValues;
	public GameObject[] characterPath;
	
	// Use this for initialization
	void Start ()
	{
		stateMachine = new StateMachine(GameObject.FindGameObjectWithTag("Player"),this);
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
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		stateMachine.stateUpdate();
		updateObjectOfInterestList();
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
				movingComponent.goToCallback += movingComponent.goToObject((GameObject)characterPath.ToArray()[i]);
				GameObject g = (GameOject)characterPath.ToArray()[i];
				Door d = g.GetComponent<Door>();
				if(d!=null)
					movingComponent.goToCallback += d.use;
			}
			*/
		}
	}
	
	public void playerDetected(){
		
		/*
		RaycastHit hit = new RaycastHit();
		Vector3 pos1 = this.gameObject.transform.position;
		pos1.x+=2.0f;
		Vector3 pos2 = this.gameObject.transform.position;
		pos2.x-=2.0f;
		Ray ray1 = new Ray(gameObject.transform.position,pos1);
		Ray ray2 = new Ray(gameObject.transform.position,pos2);
		
		if(Physics.Raycast(ray1, out hit))
		{
			if(hit.collider.gameObject.CompareTag("Player"))
				return true;
		}
		
		if(Physics.Raycast(ray2, out hit))
		{
			if(hit.collider.gameObject.CompareTag("Player"))
				return true;
		}
		
		return false;
		
		*/
		
		

	}
	
}

