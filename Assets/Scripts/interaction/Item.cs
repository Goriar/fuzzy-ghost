using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	
	public Texture icon;
	public string internalName;									// Name des Gegenstands (für interne Verwendung)
	public string name;													// sichtbarer Name des Gegenstands
	public LayerEnum currentLayer;								// gibt aktuelle Layer des Objekts an
	public Vector3 originalPosition;							// Ursprüngliche Position des Gegenstands
	public bool wasTaken;												// gibt an, ob der Gegenstand genommen wurde, oder ob er an original Position liegt
	public float scareFactor = 0;								// Erschreckfaktor als einzelner Gegenstand
	public float combineScareFactor = 0;					// Erschreckfaktor, wenn er an anderen Gegenstand hängt
	public float attentionFactor = 0;						// Aufmerksamkeitsfaktor	als einzelnder Gegenstand
	public float combineAttentionFactor = 0;			// Aufmerksamkeitsfaktor, wenn er an anderem Gegenstand hängt
	public Item[] combinableItems;								// Liste mit kombinierbaren Gegenstanden
	public Item[] combinedItems;								// Liste mit bereits kombinierten Gegenständen
	public bool slotBased = true; 								// gibt an, ob Items in Slots hinzugefügt werden sollen, oder Mesh geändert werden soll
	public GameObject[] combineObjects;					// Liste mit Kombinierbaren Objekten, für nicht slotbasiertes Kombinieren
	// COMMENT TODO: CHRIS
	private int combinationsApplied;
	private bool cursed;
	public bool used;
	public AudioClip takeAudio;
	
	
	// Use this for initialization
	void Start () {
		combinedItems = new Item[combinableItems.Length];
		originalPosition = transform.position;
		wasTaken = false;
		used = false;
		cursed =false;
		combinationsApplied = 0;
	}
	
	///
	/// Gibt Spawnkoordinaten zurück
	///
	public Vector3 getSpawnCoordinates(){
		return originalPosition;
	}
	
	///
	/// Gibt zurück, ob verflucht ist
	///
	public bool isCursed(){
		return cursed;
	}
	
	///
	/// Verfluche mit angegeben Wert
	/// @param b wert, um den verflucht werden soll
	///
	public void curse(bool b){
		cursed = b;	
	}
	
	///
	/// Nimmt Objekt auf
	///
	void take () {
		this.take (GameObject.FindGameObjectWithTag("Player"));
		AudioSource audio = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
		audio.clip = takeAudio;
		audio.Play();
	}
	
	///
	/// Gibt Erschreckwert zurück
	///
	public float getScaryness(){
		float val = 0.0f;
		/// addiert Erschreckfaktoren der hinzugefügten Gegenstände
		foreach(Item i in combinedItems){
			Debug.Log("Adding " + i.combineScareFactor + "additional Scarefactor");
			if(i != null){
				Debug.Log("Adding " + i.combineScareFactor + "additional Scarefactor");
				val += i.combineScareFactor;	
			}
		}
		
		// Wenn kombinierte Objekte angehanden, addiere eigenen Kombinier-erschreck-faktor
		if(val > 0.0f){
			val+=combineScareFactor;	
		// Oder addiere Einzel-erschreckfaktor
		} else {
			val+=scareFactor;
		}
		
		Debug.Log ("Scariness: "+val);
		return val;
		
	}
	
	///
	/// Gibt Aufmerksamkeitsfaktor zurück
	///
	public float getAttentionFactor(){
		
		float val = 0.0f;
		/// addiert Aufmerksamkeitsfaktoren der hinzugefügten Gegenstände
		foreach(Item i in combinedItems){
			if(i != null){
				
				val += i.combineAttentionFactor;	
			}
		}
		
		// Wenn kombinierte Objekte angehanden, addiere eigenen Kombinier-aufmerksamkeits-faktor
		if(val > 0.0f){
			val+=combineAttentionFactor;	
		// Oder addiere Einzel-aufmerksamkeitsfaktor
		} else {
			val+=attentionFactor;
		}
		return val;
		
	}
	
	///
	/// Gegenstand wird von angegebenen Charakter aufgenommen
	/// @param character Charakter, der Gegenstand aufheben soll
	///
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
	
	public bool isCombinable () {
		Inventory playerInv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
		if(playerInv.hasItem()) {
			for (int i = 0; i < combinableItems.Length; i++){
				if (combinableItems[i].internalName == playerInv.getItem().internalName)
					return true;
			}
		}
		return false;
	}
	
	///
	/// Kombiniert Objekte miteinander
	///
	void combine () {
		/// Inventory des Spielers
		Inventory playerInv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
		// Wenn Item in Spieler Inventar
		if(playerInv.hasItem()) {
			// Durchgehe alle Items
			for (int i = 0; i < combinableItems.Length; i++){
				// Wenn nicht Slot basiert und Item stimmt mit kombinierbarem Gegenstand überein
				if(!slotBased && combinableItems[i].internalName == playerInv.getItem().internalName){
					// COMMENT TODO: CHRIS
					combineObjects[combinationsApplied].SetActive(true);
					if(combinationsApplied>0)
						combineObjects[combinationsApplied-1].SetActive(false);
					if(combinedItems[combinationsApplied] == null) {
						Debug.Log("Slot Nummer"+combinationsApplied);
						combinedItems[combinationsApplied] = playerInv.getItem();
					}
					combinationsApplied++;
					if (gameObject.renderer != null) {
						gameObject.renderer.enabled = false;
					} else {
						foreach (Renderer childRenderer in gameObject.GetComponentsInChildren<Renderer>()) {
							childRenderer.enabled = false;
						}
					}
					playerInv.removeItem();
					gameObject.GetComponent<Interactable>().enabled = false;
					return;
				}
			}
			int combineSlot = -1;
			// Iteriere durch alle kombinierbaren Gegenstände und schaue, ob getragener Gegenstand kombinierbar ist
			for (int i = 0; i < combinableItems.Length; i++) {
				if (combinableItems[i].internalName == playerInv.getItem().internalName) {
					// Wenn slot noch leer ist, füge Gegenstand hinzu
					if (combinedItems[i] == null) {
						combinedItems[i] = playerInv.getItem(); // legt Item in den Slot
						// Iteration durch alle Kombinationsslots
						foreach(CombinationSlot slot in this.GetComponentsInChildren<CombinationSlot>()) {
							// Wenn Slotnummern übereinstimmen...
							if (slot.slotNumber == i) {
								// Objekt ist nicht mehr interagierbar
								playerInv.getItem().gameObject.GetComponent<Interactable>().enabled = false;
								// setze Position
								playerInv.getItem().transform.parent = this.transform;
								playerInv.getItem().transform.position = slot.transform.position;
								playerInv.getItem().transform.rotation = slot.transform.rotation;
								// Akviere Rendering des Objekts
								playerInv.getItem().gameObject.SetActive(true);
							}
						}
						playerInv.removeItem(); // Lösche Item aus Inventar
					}
					gameObject.GetComponent<Interactable>().enabled = false;
					break;
				}
			}
		}
	}
	
	///
	/// Gibt Icon des Gegenstands zurück
	///	
	public Texture getIcon () {
		return icon;
	}
	
}
