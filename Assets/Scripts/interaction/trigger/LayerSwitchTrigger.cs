using UnityEngine;
using System.Collections;

public class LayerSwitchTrigger : MonoBehaviour {

	public Door door;
	
	void OnTriggerEnter(Collider other) {
		// Ändert Wechselrichtung und Ebene des Objekts
		other.gameObject.GetComponent<Moving>().usableDoor = door;
	}
	
	void OnTriggerExit(Collider other) {
		// Ändert Wechselrichtung auf NONE, wenn Wechselrichtung immer noch die selbe ist (beim Wechsel betritt Objekt automatisch neuen Trigger, der wohl in andere Richtung geht)
		if (other.gameObject.GetComponent<Moving>().usableDoor == door) {
			other.gameObject.GetComponent<Moving>().usableDoor = null;
		}
	}
}
