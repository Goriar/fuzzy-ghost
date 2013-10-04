using UnityEngine;
using System.Collections;

public class Stairs : MonoBehaviour{

	
	public RoomInventory lowerMainFloor, upperMainFloor;
	public int level = 1;
	public bool isLadder = false;

	
	void Update(){
		
		
	}
	
	//Nach Oben gehen. !!Namen der Wegpunkte sind fest vorgegeben!!
	public void stairsUp(){
		
	 	Moving movingComp = GameObject.FindGameObjectWithTag("Player").GetComponent<Moving>();
		 movingComp.goToObject(GameObject.Find("StairsBottom"),goUpstairs);
	}

	// Treppe nach oben gehen für den Spieler
	public void goUpstairs(){
		
	GameObject player = GameObject.Find("Player");
		
		Player pComp = player.GetComponent<Player>();
		pComp.usesStairs = true;
		pComp.GetComponent<InputController>().lockControls();
		pComp.currentLocation = upperMainFloor;
		Transform[] path1 = {GameObject.Find("StairsBottom").transform, GameObject.Find("StairsMid1").transform};
		Transform[] path2 = {GameObject.Find("StairsMid2").transform,GameObject.Find("StairsTop").transform};
		
		iTween.MoveTo(player,iTween.Hash("path",path1,"time",1.5f,"oncomplete","execMoveLeft","easetype","easeoutsine"));

		iTween.MoveTo(player,iTween.Hash("path",path2,"time",1.5f,"oncomplete","endStairs","oncompletetarget",gameObject,"delay",1.6f,"easetype","easeinoutsine"));
	}
	
	// Treppe nach oben gehen für den NPC
	public void goUpstairs(GameObject npc){
		
		Moving movComp = npc.GetComponent<Moving>();
		movComp.finishedAction = false;
		npc.BroadcastMessage("playAnimation","move");
		Transform[] path1 = {GameObject.Find("StairsBottom").transform, GameObject.Find("StairsMid1").transform};
		Transform[] path2 =	{GameObject.Find("StairsMid2").transform,GameObject.Find("StairsTop").transform};
		
		iTween.MoveTo(npc,iTween.Hash("path",path1,"time",1.5f,"oncomplete","execMoveLeft","easetype","easeoutsine"));

		iTween.MoveTo(npc,iTween.Hash("path",path2,"time",1.5f,"delay",1.6f,"easetype","easeinoutsine","oncomplete","finishAction","oncompletetarget",gameObject,"oncompleteparams",movComp));
		
	}
	
	//Selbe wie oben, nur dass die Wegpunkte anders rum sind
	public void stairsDown(){
		
		 Moving movingComp = GameObject.FindGameObjectWithTag("Player").GetComponent<Moving>();
		  movingComp.goToObject(GameObject.Find("StairsTop"),goDownstairs);
		
	
	}
	
	//Der Spieler wird mittels ITween nach unten bewegt
	public void goDownstairs(){
		GameObject player = GameObject.Find("Player");
		Player pComp = player.GetComponent<Player>();
		pComp.GetComponent<InputController>().lockControls();
		pComp.usesStairs = true;
		pComp.currentLocation = lowerMainFloor;
		//Die Punkte die er abläuft sind fest vorgegeben
		Transform[] path1 = {GameObject.Find("StairsTop").transform, GameObject.Find("StairsMid2").transform};
		Transform[] path2 = {GameObject.Find("StairsMid1").transform,GameObject.Find("StairsBottom").transform};
		
		iTween.MoveTo(player,iTween.Hash("path",path1,"time",1.5f,"oncomplete","execMoveLeft","easetype","easeoutsine"));

		iTween.MoveTo(player,iTween.Hash("path",path2,"time",1.5f,"oncomplete","endStairs","oncompletetarget",gameObject,"delay",1.6f,"easetype","easeinoutsine"));
	}
	
	//Ein NPC wird mittels ITween nach unten bewegt
	public void goDownstairs(GameObject npc){
		
		Moving movComp = npc.GetComponent<Moving>();
		movComp.finishedAction = false;
		npc.BroadcastMessage("playAnimation","move");
		//Die Punkte die er abläuft sind fest vorgegeben
		Transform[] path1 = {GameObject.Find("StairsTop").transform, GameObject.Find("StairsMid2").transform};
		Transform[] path2 = {GameObject.Find("StairsMid1").transform,GameObject.Find("StairsBottom").transform};
		iTween.MoveTo(npc,iTween.Hash("path",path1,"time",1.5f,"oncomplete","execMoveLeft","easetype","easeoutsine"));

		iTween.MoveTo(npc,iTween.Hash("path",path2,"time",1.5f,"delay",1.6f,"easetype","easeinoutsine","oncomplete","finishAction","oncompletetarget",gameObject,"oncompleteparams",movComp));

		
	}
	//Itween zum Leiter hinauf klettern
	public void ladderUp(){
		GameObject player = GameObject.Find("Player");
		player.GetComponent<InputController>().lockControls();
		player.GetComponent<Player>().usesStairs = true;
		Transform[] path1 = {GameObject.Find("LadderBottom").transform, GameObject.Find("LadderTop").transform};
		iTween.MoveTo(player,iTween.Hash("path",path1,"time",1.5f,"oncomplete","endStairs","oncompletetarget",gameObject,"easetype","easeoutsine"));
	}
	//Itween zum Leiter hinab klettern
	public void ladderDown(){
		GameObject player = GameObject.Find("Player");
		player.GetComponent<InputController>().lockControls();
		player.GetComponent<Player>().usesStairs = true;
		Transform[] path1 = {GameObject.Find("LadderTop").transform, GameObject.Find("LadderBottom").transform};
		iTween.MoveTo(player,iTween.Hash("path",path1,"time",1.5f,"oncomplete","endStairs","oncompletetarget",gameObject,"easetype","easeoutsine"));
	}

	public void finishAction(Moving movComp){
		movComp.finishedAction = true;
		// Erhalte Raumkomponente
		RoomInventory roomComp = (level == 1) ? upperMainFloor : lowerMainFloor;
		
		// Wenn Objekt NPC ...
		Character chara = movComp.GetComponent<Character>();
		Player player = movComp.GetComponent<Player>();
		RoomInventory otherFloor = null;
		if (chara != null) {
			Debug.Log("Char " + chara.name + " wechselt durch Treppe von Raum " + chara.getCurrentLocation() + " in Raum " + roomComp);
			chara.setCurrentLocation(roomComp);
			chara.setCharacterPath(chara.currentObjectOfInterest);
		// Wenn Objekt Player ...
		} else if (player != null) {
			player.setCurrentLocation(roomComp);
		}
	}
	
	
	///
	/// Methode zum beenden der Treppensteig Animation
	/// 
	void endStairs () {
		Player player = GameObject.Find("Player").GetComponent<Player>();
		Moving movComp = player.GetComponent<Moving>();
		if (isLadder) {
			movComp.execMoveLeft();
			movComp.goToCallback += unlockStairsForPlayer;
			movComp.goToX(player.transform.position.x-0.4f);
		} else {
			movComp.goToCallback += unlockStairsForPlayer;
			movComp.goToX(player.transform.position.x+0.4f);
		}
	}
	
	void unlockStairsForPlayer () {
		Debug.Log ("blubi");
		GameObject.Find("Player").GetComponent<Player>().usesStairs = false;
		GameObject.Find("Player").GetComponent<InputController>().unlockControls();
	}
	
	///
	/// Trigger für automatische Benutzung der Treppe beim Spieler
	/// @param other anderer Collider
	/// 
	void OnTriggerStay(Collider other){
		Player player = other.GetComponent<Player>();
		if (player != null && !player.usesStairs) {
			if (player != null) {
				if (!isLadder) {
					if (level == 1) {
						goUpstairs();
					} else if (level == 2) {
						goDownstairs();
					}
				} else {
					if (level == 2) {
						ladderUp();
					} else if (level == 3) {
						ladderDown();
					}
				}
			}
		}
	}
	
	
}
