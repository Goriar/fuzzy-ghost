using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
	StateMachine stateMachine;
	Moving movingComponent;
	public RoomInventory currentLocation;
	public GameObject objectOfInterest;
	private ArrayList characterPath;
	
	// Use this for initialization
	void Start ()
	{
		stateMachine = new StateMachine(GameObject.FindGameObjectWithTag("Player"),this);
		characterPath = new ArrayList();
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
	
	public void setCharacterPath()
	{
		characterPath = new ArrayList();
		if(currentLocation.containsObject(objectOfInterest))
			characterPath.Add(objectOfInterest);
		else
		{
			RoomInventory mainFloor = this.getMainFloorNeighbour();
			GameObject door = currentLocation.getDoorToRoom(mainFloor);
			characterPath.Add(door);
			
			if(mainFloor.containsObject(objectOfInterest))
			{
				characterPath.Add(objectOfInterest);
				return;
			}
			
			RoomInventory nextRoom = null;
			foreach(RoomInventory room in mainFloor.getNeighbouringRooms())
			{
				if(roomContainsObject(room,objectOfInterest))
					nextRoom = room;
			}
			
			door = mainFloor.getDoorToRoom(nextRoom);
			characterPath.Add(door);
			characterPath.Add(objectOfInterest);
			
			for(int i = 0; i < characterPath.Capacity; ++i)
			{
				//movingComponent.goToCallback += movingComponent.goToObject((GameObject)characterPath.ToArray()[i]);
				//GameObject g = (GameOject)characterPath.ToArray()[i];
				//Door d = g.GetComponent<Door>();
				//if(d!=null)
					//movingComponent.goToCallback += d.use;
			}
		}
	}
	
}

