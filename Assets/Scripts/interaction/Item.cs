using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	
	public LayerEnum currentLayer;					// gibt aktuelle Layer des Objekts an
	public Vector3 originalPosition;				// Ursprüngliche Position des Gegenstands
	public bool wasTaken;							// gibt an, ob der Gegenstand genommen wurde, oder ob er an original Position liegt
	
	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
		wasTaken = false;
	}
	
	void take () {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		// Führe Aktion aus, wenn am Objekt
		if (transform.position.x == player.transform.position.x) {
			player.GetComponent<Inventory>().takeItem(this.gameObject);
		// Ansonsten gehe zum Objekt und führe Aktion dann aus
		} else {
			Moving movingComp = player.GetComponent<Moving>();
			movingComp.goToObject(this.gameObject,this.take);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
