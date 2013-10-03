using UnityEngine;
using System.Collections;

public class Stairs : MonoBehaviour{

	
	public RoomInventory lowerMainFloor, upperMainFloor;
	public int level = 1;

	
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
		pComp.currentLocation = upperMainFloor;
		Transform[] path1 = {GameObject.Find("StairsBottom").transform, GameObject.Find("StairsMid1").transform};
		Transform[] path2 = {GameObject.Find("StairsMid2").transform,GameObject.Find("StairsTop").transform};
		
		iTween.MoveTo(player,iTween.Hash("path",path1,"time",1.5f,"oncomplete","execMoveLeft","easetype","easeoutsine"));

		iTween.MoveTo(player,iTween.Hash("path",path2,"time",1.5f,"delay",1.6f,"easetype","easeinoutsine"));
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
		pComp.currentLocation = lowerMainFloor;
		//Die Punkte die er abläuft sind fest vorgegeben
		Transform[] path1 = {GameObject.Find("StairsTop").transform, GameObject.Find("StairsMid2").transform};
		Transform[] path2 = {GameObject.Find("StairsMid1").transform,GameObject.Find("StairsBottom").transform};
		
		iTween.MoveTo(player,iTween.Hash("path",path1,"time",1.5f,"oncomplete","execMoveLeft","easetype","easeoutsine"));

		iTween.MoveTo(player,iTween.Hash("path",path2,"time",1.5f,"delay",1.6f,"easetype","easeinoutsine"));
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
		
		Transform[] path1 = {GameObject.Find("LadderBottom").transform, GameObject.Find("LadderTop").transform};
		iTween.MoveTo(player,iTween.Hash("path",path1,"time",1.5f,"oncomplete","execMoveLeft","easetype","easeoutsine"));
	}
	//Itween zum Leiter hinab klettern
	public void ladderDown(){
		GameObject player = GameObject.Find("Player");
		
		Transform[] path1 = {GameObject.Find("LadderTop").transform, GameObject.Find("LadderBottom").transform};
		iTween.MoveTo(player,iTween.Hash("path",path1,"time",1.5f,"oncomplete","execMoveLeft","easetype","easeoutsine"));
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
			chara.setCurrentLocation(roomComp);
			chara.setCharacterPath(chara.currentObjectOfInterest);
		// Wenn Objekt Player ...
		} else if (player != null) {
			player.setCurrentLocation(roomComp);
		}
	}

	
	
}
