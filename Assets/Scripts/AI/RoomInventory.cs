using UnityEngine;
using System.Collections;

public class RoomInventory : MonoBehaviour {

	public GameObject[] objects; //Objekte im Raum
	public RoomInventory[] neighbouringRooms; //Nachbarräume
	public GameObject stairs; //Treppen, falls welche vorliegen
	private string roomName; //Name des Raums
	public bool isMainFloor; //Ist der Raum einer der Gände in der Mitte?
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
	//Prüft ob ein Objekt im Raum liegt
	public bool containsObject(GameObject obj)
	{
		foreach(GameObject g in objects)
		{
			if(g.Equals(obj))
				return true;
		}
		
		return false;
	}
	//Name des Raums
	public string getRoomName()
	{
		return roomName;
	}
	
	//Nachbarraume
	public RoomInventory[] getNeighbouringRooms()
	{
		return neighbouringRooms;
	}
	//Findet die Tür zu einem Nachbarraum
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
