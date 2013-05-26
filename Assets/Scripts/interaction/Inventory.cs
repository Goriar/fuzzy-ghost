using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	private GameObject currentItem;
	
	public void takeItem(GameObject item) {
		if (currentItem == null) {
			currentItem = item;
		}
	}
	
	public void useItem() {
		if (currentItem != null) {
			currentItem.BroadcastMessage("useInInventory");
		}
	}
	
	public void dropItem() {
		if (currentItem != null) {
			// Drop Code here
		}
	}
	
}
