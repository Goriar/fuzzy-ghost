using UnityEngine;
using System.Collections;

public class Occupant : MonoBehaviour {
	
	private float scareLevel;				// Aktuelles Erschreckfortschritt
	
	public float superstitionFactor;		// Aberglaube Faktor (von 0 bis 2)
	
	// Use this for initialization
	void Start () {
		scareLevel = 0;
		// Setze Aberglaubefaktor auf Grenzen, falls unter-/überschritten
		superstitionFactor = (superstitionFactor > 2f) ? 2f : superstitionFactor;
		superstitionFactor = (superstitionFactor < 0f) ? 0f : superstitionFactor;
	}
	
	public void scare (float scareAddition) {
		scareLevel += scareAddition*superstitionFactor;
	}
}
