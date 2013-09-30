using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	private GameObject currentItem; // Item the NPC/Player currently has
	
	///
	/// Nehme Gegenstand
	/// Nimmt Gegenstand, der übergeben wurde, wenn Inventar leer ist
	/// @param item Gegenstand, der aufgehoben wird
	/// 
	public void takeItem(GameObject item) {
		if (currentItem == null) {
			BroadcastMessage("showPlayer");
			currentItem = item;
			currentItem.GetComponent<Item>().wasTaken = true;
			currentItem.SetActive(false);
		}
	}
	
	///
	/// Benutze Gegenstand
	/// Nutzt den aktuell ausgerüsteten Gegenstand, wenn er vorhanden ist
	/// 
	public void useItem() {
		if (currentItem != null) {
			currentItem.BroadcastMessage("useInInventory");
		}
	}
	
	///
	/// Gibt true zurück, wenn angelegter Gegenstand vorhanden ist
	///
	public bool hasItem() {
		return currentItem != null;
	}
	
	///
	/// Gibt aktuelles Item zurück
	///
	public Item getItem() {
		if (currentItem != null) {
			return currentItem.GetComponent<Item>();
		} else {
			return null;	
		}
	}
	
	///
	/// Lasse Gegenstand fallen
	/// Lässt aktuellen Gegenstand fallen
	/// 
	public void dropItem() {
		if (currentItem != null) {
			BroadcastMessage("hidePlayer");
			currentItem.transform.position = gameObject.transform.position;
			currentItem.GetComponent<Item>().currentLayer = gameObject.GetComponent<Moving>().layer;
			currentItem.SetActive(true);
			currentItem = null;
		}
	}
	
	public void removeItem() {
		if (currentItem != null) {
			BroadcastMessage("hidePlayer");
			currentItem = null;
			Debug.Log ("Item removed");
		}
	}
	
}
