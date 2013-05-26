using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	private GameObject currentItem;
	
	public void takeItem(GameObject item) {
		if (currentItem == null) {
			currentItem = item;
			currentItem.GetComponent<Item>().wasTaken = true;
			currentItem.SetActive(false);
		}
	}
	
	public void useItem() {
		if (currentItem != null) {
			currentItem.BroadcastMessage("useInInventory");
		}
	}
	
	public void dropItem() {
		if (currentItem != null) {
			currentItem.transform.position = gameObject.transform.position;
			currentItem.GetComponent<Item>().currentLayer = gameObject.GetComponent<Moving>().layer;
			currentItem.SetActive(true);
			currentItem = null;
		}
	}
	
}
