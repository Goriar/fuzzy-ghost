using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
	StateMachine stateMachine;
	Moving movingComponent;
	public RoomInventory currentLocation;
	public GameObject objectOfInterest;
	private GameObject[] characterPath;
	
	// Use this for initialization
	void Start ()
	{
		stateMachine = new StateMachine(GameObject.FindGameObjectWithTag("Player"),this);
		characterPath = new GameObject[0];
		movingComponent = this.gameObject.GetComponent<Moving>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		stateMachine.stateUpdate();
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
		if(currentLocation.containsObject(objectOfInterest))
			addToPath(objectOfInterest);
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
			
			if(mainFloor.containsObject(objectOfInterest))
			{
				addToPath(objectOfInterest);
				return;
			}
			
			RoomInventory nextRoom = null;
			foreach(RoomInventory room in mainFloor.getNeighbouringRooms())
			{
				if(roomContainsObject(room,objectOfInterest))
					nextRoom = room;
			}
			
			door = mainFloor.getDoorToRoom(nextRoom);
			addToPath(door);
			addToPath(objectOfInterest);
			
		
			
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
	
}

