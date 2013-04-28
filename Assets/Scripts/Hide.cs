using UnityEngine;
using System.Collections;

public class Hide : MonoBehaviour {
	
	// Attributes
	private Component[] renderers; // Renderer (Texturen)
	
	// Layer Hide Optionen
	private bool lerpActive;				// aktiv, wenn Änderung in Sichtbarkeit der Layer
	private float lerpTo;					// neue Sichtbarkeit
	private float lerpStartTime;			// Startzeit des Lerps
	private float lerpDuration;				// Dauer des Lerps
	private bool deactivateObject;			// Wenn true, wird Objekt deaktiviert, wenn Visibility = 0
		
	/// 
	/// Zeigt das Objekt komplett
	/// Ist von aussen per Message benutzbar
	/// @param duration		Dauer des Sichtbarkeitsvorgangs
	/// @param delay		Verzögerung des Sichtbarmachens
	/// 
	public void show (float duration, float delay) {
		Hashtable ht = new Hashtable();
		ht.Add("alpha", 1);
		ht.Add("time", duration);
		ht.Add("delay", delay);
		iTween.FadeTo(gameObject, ht);
	}
	
	public void show (float duration) {
		show(duration, 0);
	}
	
	/// 
	/// Versteckt das Objekt komplett mit deaktivierung
	/// Ist von aussen per Message benutzbar
	/// @param duration		Dauer des Versteckvorgangs
	/// @param delay		Verzögerung des Versteckens
	/// 
	public void hide (float duration, float delay) {
		Hashtable ht = new Hashtable();
		ht.Add("alpha", 0);
		ht.Add("time", duration);
		ht.Add("delay", delay);
		ht.Add("onComplete", "deactivate");
		iTween.FadeTo(gameObject, ht);
	}
	
	public void hide (float duration) {
		hide(duration, 0f);
	}
	
	
	/// 
	/// Versteckt das Objekt komplett ohne deaktivierung
	/// Ist von aussen per Message benutzbar
	/// @param duration				Dauer des Versteckvorgangs
	/// 
	public void makeInvisible (float duration) {
		Hashtable ht = new Hashtable();
		ht.Add("alpha", 0);
		ht.Add ("time", duration);
		iTween.FadeTo(gameObject, ht);
	}
	
	void deactivate () {
		gameObject.SetActive(false);
	}
	
	
}
