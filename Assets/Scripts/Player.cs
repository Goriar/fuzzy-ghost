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
		Health = 100f;
	}
	
	public void showPlayer () {
		BroadcastMessage("show", 1f);
		visible = true;
	}
	
	public void hidePlayer () {
		BroadcastMessage("makeTransparent", 1f);
		visible = false;
	}
	
	public bool canBeSeen () {
		return visible;
	}
		
	// Update is called once per frame
	void Update ()
	{
	
	}
}

