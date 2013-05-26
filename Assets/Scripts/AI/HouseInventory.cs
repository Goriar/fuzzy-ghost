using System;
using UnityEngine;

public class HouseInventory
{
	RoomInventory kitchen, diningRoom;
	public HouseInventory ()
	{
		kitchen 	= GameObject.Find("Kitchen").GetComponent<RoomInventory>();
		diningRoom  = GameObject.Find("Dining Room").GetComponent<RoomInventory>();
	}
	
	GameObject[] getKitchenObjects()
	{
		return kitchen.getObjects();
	}
	
	GameObject[] getDiningRoomObjects()
	{
		return diningRoom.getObjects();
	}
}


