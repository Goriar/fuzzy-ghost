using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	
	public LayerEnum currentLayer;					// gibt aktuelle Layer des Objekts an
	public Vector3 originalPosition;				// Ursprüngliche Position des Gegenstands
	public bool wasTaken;							// gibt an, ob der Gegenstand genommen wurde, oder ob er an original Position liegt
	
	public Item[] combinableItems;
	private Item[] combinedItems;
	
	// Use this for initialization
	void Start () {
		combinedItems = new Item[combinableItems.Length];
		originalPosition = transform.position;
		wasTaken = false;
	}
	
	void take () {
		this.take (GameObject.FindGameObjectWithTag("Player"));
	}
	
	void take (GameObject character) {
		// Führe Aktion aus, wenn am Objekt
		if (transform.position.x == character.transform.position.x) {
			character.GetComponent<Inventory>().takeItem(this.gameObject);
		// Ansonsten gehe zum Objekt und führe Aktion dann aus
		} else {
			Moving movingComp = character.GetComponent<Moving>();
			movingComp.goToObject(this.gameObject,this.take);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
