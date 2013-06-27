using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {
	
	public float damage = 10f;
	
	void OnTriggerEnter(Collider other) {
		Player player = other.gameObject.GetComponent<Player>();
		
		// Wenn Objekt Spieler ist
		if(player!=null){
			player.applyDamage(damage); // ziehe schaden ab
			// TODO: Play Effect
			BroadcastMessage("fadeoutDestroy", 1f);
		}
	}
	
}
