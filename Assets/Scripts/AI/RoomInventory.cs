using UnityEngine;
using System.Collections;

public class RoomInventory : MonoBehaviour {

	public GameObject[] objects;
	public RoomInventory[] neighbouringRooms;
	public GameObject stairs;
	private string roomName;
	public bool isMainFloor;
	public int level;   //Stockwerk, von 1..3
	
	// Use this for initialization
	void Start () {
		roomName = this.gameObject.name;
			
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public GameObject[] getObjects(){
		return objects;
	}
	
	public bool containsObject(GameObject obj)
	{
		foreach(GameObject g in objects)
		{
			if(g.Equals(obj))
				return true;
		}
		
		return false;
	}
	
	public string getRoomName()
	{
		return roomName;
	}
	
	public RoomInventory[] getNeighbouringRooms()
	{
		return neighbouringRooms;
	}
	
	public GameObject getDoorToRoom(RoomInventory room)
	{
		foreach(GameObject g in objects)
		{
			Door door = g.GetComponent<Door>();
			Stairs stairs =g.GetComponent<Stairs>();
			if(door != null)
			{
				for(int i = 0; i < door.connectedRooms.Length; ++i)
				{
					if(door.connectedRooms[i].Equals(room))
						return g;
				}
			}
			
		}
		
		return null;
	}
}
