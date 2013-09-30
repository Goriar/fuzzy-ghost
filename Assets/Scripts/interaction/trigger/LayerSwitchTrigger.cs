using UnityEngine;
using System.Collections;

public class LayerSwitchTrigger : MonoBehaviour {

	public Door door;
	
	///
	/// Wenn Charakter oder Spieler Trigger betritt...
	/// @param other anderes Collider Objekt
	///
	void OnTriggerEnter(Collider other) {
		// Ändert Wechselrichtung und Ebene des Objekts
		if(other.gameObject.GetComponent<Moving>()!=null && other.gameObject.tag == "Player"){
			other.gameObject.GetComponent<Moving>().usableDoor = door;
		}
	}
	
	///
	/// Wenn Charakter oder Spieler Trigger verlässt...
	/// @param other anderes Collider Objekt
	///
	void OnTriggerExit(Collider other) {
		// Ändert Wechselrichtung auf NONE, wenn Wechselrichtung immer noch die selbe ist (beim Wechsel betritt Objekt automatisch neuen Trigger, der wohl in andere Richtung geht)
		if(other.gameObject.GetComponent<Moving>()!=null && other.gameObject.tag == "Player"){
			if (other.gameObject.GetComponent<Moving>().usableDoor == door) {
				other.gameObject.GetComponent<Moving>().usableDoor = null;
			}
		}
	}
}
