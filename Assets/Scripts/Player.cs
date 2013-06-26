using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

	public float health;
	private bool visible;

	public RoomInventory currentLocation;


	// Use this for initialization
	void Start ()
	{
		health = 100f;
	}
	
	/// 
	/// Macht Spieler sichtbar
	///
	public void showPlayer () {
		BroadcastMessage("show", 1f);
		visible = true;
	}
	
	/// 
	/// Macht Spieler unsichtbar
	///
	public void hidePlayer () {
		BroadcastMessage("makeTransparent", 1f);
		visible = false;
	}
	
	/// 
	/// Gibt true zurück, wenn Spieler sichtbar ist
	///
	public bool canBeSeen () {
		return visible;
	}
	
}

