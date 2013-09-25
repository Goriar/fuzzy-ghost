using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	
	public Texture icon;
	public string internalName;						// Name des Gegenstands (für interne Verwendung)
	public string name;								// sichtbarer Name des Gegenstands
	public LayerEnum currentLayer;					// gibt aktuelle Layer des Objekts an
	public Vector3 originalPosition;				// Ursprüngliche Position des Gegenstands
	public bool wasTaken;							// gibt an, ob der Gegenstand genommen wurde, oder ob er an original Position liegt
	public float scareFactor = 0;					// Erschreckfaktor als eigentlicher Gegenstand
	public float combineScareFactor = 0;			// Erschrekfaktor, wenn er an anderen Gegenstand hängt
	public float attentionFactor = 0;
	public float combineAttentionFactor = 0;
	public Item[] combinableItems;
	private Item[] combinedItems;
	private bool cursed;
	public bool used;
	
	private Vector3 spawnCoordinates;
	
	// Use this for initialization
	void Start () {
		combinedItems = new Item[combinableItems.Length];
		originalPosition = transform.position;
		wasTaken = false;
		spawnCoordinates = this.transform.position;
		used = false;
		cursed =false;
	}
	
	public Vector3 getSpawnCoordinates(){
		return spawnCoordinates;
	}
	
	public bool isCursed(){
		return cursed;
	}
	
	public void curse(bool b){
		cursed = b;	
	}
	
	void take () {
		this.take (GameObject.FindGameObjectWithTag("Player"));
	}
	
	public float getScaryness(){
		float val = 0.0f;
		foreach(Item i in combinedItems){
			if(i != null){
				val += i.combineScareFactor;	
			}
		}
		if(val > 0.0f){
			val+=combineScareFactor;	
		} else {
			val+=scareFactor;
		}
		return val;
		
	}
	
	public float getAttentionFactor(){
		float val = 0.0f;
		foreach(Item i in combinedItems){
			if(i != null){
				val += i.combineAttentionFactor;	
			}
		}
		if(val > 0.0f){
			val+=combineAttentionFactor;	
		} else {
			val+=attentionFactor;
		}
		return val;
		
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
	
	void combine () {
		Inventory playerInv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
		if(playerInv.hasItem()) {
			int combineSlot = -1;
			// Iteriere durch alle kombinierbaren Gegenstände und schaue, ob getragener Gegenstand kombinierbar ist
			for (int i = 0; i < combinableItems.Length; i++) {
				if (combinableItems[i].internalName == playerInv.getItem().internalName) {
					// Wenn slot noch leer ist, füge Gegenstand hinzu
					if (combinedItems[i] == null) {
						combinedItems[i] = playerInv.getItem(); // legt Item in den Slot
						foreach(CombinationSlot slot in this.GetComponentsInChildren<CombinationSlot>()) {
							if (slot.slotNumber == i) {
								playerInv.getItem().gameObject.GetComponent<Interactable>().enabled = false;
								playerInv.getItem().transform.parent = this.transform;
								playerInv.getItem().transform.position = slot.transform.position;
								playerInv.getItem().transform.rotation = slot.transform.rotation;
								playerInv.getItem().gameObject.SetActive(true);
							}
						}
						playerInv.removeItem(); // Lösche Item aus Inventar
					}
					break;
				}
			}
		}
	}
	
	public Texture getIcon () {
		return icon;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
