using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public DirectionEnum switchDirection;														// Richtung, in die durch Tür gegangen werden kann
	public LayerEnum switchTo;																				// Layer, in die durch Tür gegangen wird
	public Door otherSide;																						// Tür auf anderen Seite (anderen Layer)
	public RoomInventory[] connectedRooms = new RoomInventory[2];			// Räume, die mittels Tür miteinander verbunden werden
	public AudioClip doorAudio;																			// Tür Geräusch
	
	///
	/// Interaktion durch GUI Eingabe
	/// @param action			Aktion, die ausgefürt werden soll
	///
	void interact (string action) {
		if (action == "Use") {
			use(); // Use Funktion
		}
	}
	
	///
	/// Öffnet die Tür
	///
	void open () {		
		// Hashtable für Animationsoptionen
		Hashtable ht = new Hashtable();
		ht.Add("y",180); // Rotation in Y Achse
		ht.Add("Time", 1f); // Zeit für Animation
		int counter = 0;
		// Rotiere alle Kindobjekte mit übergebenen Eigenschaften
		foreach (Transform child in this.transform) {
			iTween.RotateTo(child.gameObject, ht); 
		}
	}
	
	///
	/// Schließt die Tür
	///
	public void close () {
		// Funktion wie Öffnen, mit anderen Werten
		Hashtable ht = new Hashtable();
		ht.Add("y",270);
		ht.Add("Time", 1f);
		
		foreach (Transform child in this.transform) {
			iTween.RotateTo(child.gameObject, ht);
		}
	}
	
	///
	/// Use Methode mit Player Moving Componente
	///
	public void use () {
		// Moving Componente vom Spieler
		Moving movingComp = GameObject.FindGameObjectWithTag("Player").GetComponent<Moving>();
		// Fügt Callback zu Moving Componente hinzu
		movingComp.goToCallback += this.open;
		if (otherSide != null) {
			// Wenn Tür Gegenstück vorhanden, füge auch hier Callback hinzu
			movingComp.goToCallback += otherSide.open;
		}
		// spiele Audio
		AudioSource audio = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
		audio.clip = doorAudio;
		audio.Play();
		// Initiiere Layer Switch
		movingComp.startLayerSwitch(this);
	}
	
	///
	/// Use Methode mit definierter Moving Componente (z.B. NPCs)
	/// @param movComp			Moving Componente
	///
	public void use (Moving movComp) {
		Moving movingComp = movComp;
		// Öffne Tür
		this.open();
		if (otherSide != null) {
			// Öffne Tür
			otherSide.open();
		}
		// spiele Audio
		AudioSource audio = movingComp.gameObject.GetComponent<AudioSource>();
		audio.clip = doorAudio;
		audio.Play();
		// Initiiere Layer Switch
		movingComp.startLayerSwitch(this);
	}
	
	
}
