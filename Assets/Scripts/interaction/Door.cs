using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public DirectionEnum switchDirection;
	public LayerEnum switchTo;
	public Door otherSide;
	public RoomInventory[] connectedRooms = new RoomInventory[2];
	
	void interact (string action) {
		if (action == "Use") {
			use();
		}
	}
	
	void open () {		
		Hashtable ht = new Hashtable();
		ht.Add("y",180);
		ht.Add("Time", 1f);
		int counter = 0;
		foreach (Transform child in this.transform) {
			iTween.RotateTo(child.gameObject, ht);
		}
	}
	
	public void close () {
		Hashtable ht = new Hashtable();
		ht.Add("y",270);
		ht.Add("Time", 1f);
		
		foreach (Transform child in this.transform) {
			iTween.RotateTo(child.gameObject, ht);
		}
	}
	
	public void use () {
		Moving movingComp = GameObject.FindGameObjectWithTag("Player").GetComponent<Moving>();
		movingComp.goToCallback += this.open;
		if (otherSide != null) {
			movingComp.goToCallback += otherSide.open;
		}
		movingComp.startLayerSwitch(this);
	}
	
	public void use (Moving movComp) {
		Moving movingComp = movComp;
		this.open();
		if (otherSide != null) {
			otherSide.open();
		}
		movingComp.startLayerSwitch(this);
	}
	
	
}
