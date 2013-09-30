using UnityEngine;
using System.Collections;

public class Stairs : MonoBehaviour{
	
	/*
	private static float workTime;			//Wird benötigt, wenn ein Wegpunkt erreicht wird
	
	private static float workIndex = 0;		//Wird benötigt, um zu bestimmen welcher Wegpunkt als nächstes angesteuert wird
	
	*/
	
	public RoomInventory lowerMainFloor, upperMainFloor;
	public int level = 1;

	
	void Update(){
		
		
	}
	
	//Nach Oben gehen. !!Namen der Wegpunkte sind fest vorgegeben!!
	//Wenn Aktion beendet wird index auf -1 gesetzt
	public void stairsUp(){
		
	 	Moving movingComp = GameObject.FindGameObjectWithTag("Player").GetComponent<Moving>();
		 movingComp.goToObject(GameObject.Find("StairsBottom"),goUpstairs);
		/*
		GameObject stairUp = null;
		GameObject stairBot = null;
		if(workIndex == 0){
			workTime = startTime;
			stairBot = GameObject.Find("StairsBottom");
		 	stairUp = GameObject.Find("StairsMid1");
		}
		else if(workIndex == 1){
			stairBot = GameObject.Find("StairsMid1");
			stairUp = GameObject.Find("StairsMid2");
		}
		else{
			stairBot = GameObject.Find("StairsMid2");
			stairUp = GameObject.Find("StairsTop");
		}
		Vector3 up = stairUp.transform.position;
		Vector3 bot = stairBot.transform.position;
		
		float journeyLength = Vector3.Distance(bot,up);
		float distCovered = (Time.time - workTime) * 2.0f;
		float fracJourney = distCovered/journeyLength;
		player.transform.position = Vector3.Lerp(bot,up,fracJourney);
		
		//Wenn Endposition erreicht wurde, wird die Aktion beendet, ansonsten werden
		//die Variabeln auf die nächste Position gesetzt
		if(player.transform.position.Equals(up)){
			workIndex++;
			workTime = Time.time;
			if(workIndex>2){
				index = -1;
				workIndex = 0;
			}
		}
		*/
	}

	
	public void goUpstairs(){
		
	GameObject player = GameObject.Find("Player");
		
		Player pComp = player.GetComponent<Player>();
		pComp.currentLocation = upperMainFloor;
		Transform[] path1 = {GameObject.Find("StairsBottom").transform, GameObject.Find("StairsMid1").transform};
		Transform[] path2 = {GameObject.Find("StairsMid2").transform,GameObject.Find("StairsTop").transform};
		
		iTween.MoveTo(player,iTween.Hash("path",path1,"time",1.5f,"oncomplete","execMoveLeft","easetype","easeoutsine"));

		iTween.MoveTo(player,iTween.Hash("path",path2,"time",1.5f,"delay",1.6f,"easetype","easeinoutsine"));
	}
	
	public void goUpstairs(GameObject npc){
		
		Moving movComp = npc.GetComponent<Moving>();
		movComp.finishedAction = false;
		npc.BroadcastMessage("playAnimation","move");
		Transform[] path = {GameObject.Find("StairsBottom").transform, GameObject.Find("StairsMid1").transform,
							GameObject.Find("StairsMid2").transform,GameObject.Find("StairsTop").transform};

		iTween.MoveTo(npc,iTween.Hash("path",path,"time",3.0f,"oncomplete","finishAction","oncompletetarget",gameObject,"oncompleteparams",movComp,"easetype","easeoutsine"));
	}
	
	//Selbe wie oben, nur dass die Wegpunkte anders rum sind
	//Wenn Aktion beendet wird index auf -1 gesetzt
	public void stairsDown(){
		
		 Moving movingComp = GameObject.FindGameObjectWithTag("Player").GetComponent<Moving>();
		  movingComp.goToObject(GameObject.Find("StairsTop"),goDownstairs);
		/*
				GameObject stairUp = null;
				GameObject stairBot = null;
				if(workIndex == 0){
					workTime = startTime;
					stairBot = GameObject.Find("StairsTop");
				 	stairUp = GameObject.Find("StairsMid2");
				}
				else if(workIndex == 1){
					stairBot = GameObject.Find("StairsMid2");
					stairUp = GameObject.Find("StairsMid1");
				}
				else{
					stairBot = GameObject.Find("StairsMid1");
					stairUp = GameObject.Find("StairsBottom");
				}
				Vector3 up = stairUp.transform.position;
				Vector3 bot = stairBot.transform.position;
				
				float journeyLength = Vector3.Distance(bot,up);
				float distCovered = (Time.time - workTime) * 2.0f;
				float fracJourney = distCovered/journeyLength;
				player.transform.position = Vector3.Lerp(bot,up,fracJourney);
		
				if(player.transform.position.Equals(up)){
					workIndex++;
					workTime = Time.time;
					if(workIndex>2){
						index = -1;
						workIndex = 0;
					}
				}
		*/
	}
	
	public void goDownstairs(){
		GameObject player = GameObject.Find("Player");
		Player pComp = player.GetComponent<Player>();
		pComp.currentLocation = lowerMainFloor;
		Transform[] path1 = {GameObject.Find("StairsTop").transform, GameObject.Find("StairsMid2").transform};
		Transform[] path2 = {GameObject.Find("StairsMid1").transform,GameObject.Find("StairsBottom").transform};
		
		iTween.MoveTo(player,iTween.Hash("path",path1,"time",1.5f,"oncomplete","execMoveLeft","easetype","easeoutsine"));

		iTween.MoveTo(player,iTween.Hash("path",path2,"time",1.5f,"delay",1.6f,"easetype","easeinoutsine"));
	}
	
	public void goDownstairs(GameObject npc){
		
		Moving movComp = npc.GetComponent<Moving>();
		movComp.finishedAction = false;
		npc.BroadcastMessage("playAnimation","move");
		Transform[] path = {GameObject.Find("StairsTop").transform, GameObject.Find("StairsMid2").transform,GameObject.Find("StairsMid1").transform,GameObject.Find("StairsBottom").transform};
		

		iTween.MoveTo(npc,iTween.Hash("path",path,"time",3.0f,"oncomplete","finishAction","oncompletetarget",gameObject,"oncompleteparams",movComp, "easetype","easeoutsine"));
	}
	
	public void ladderUp(){
		GameObject player = GameObject.Find("Player");
		
		Transform[] path1 = {GameObject.Find("LadderBottom").transform, GameObject.Find("LadderTop").transform};
		iTween.MoveTo(player,iTween.Hash("path",path1,"time",1.5f,"oncomplete","execMoveLeft","easetype","easeoutsine"));
	}
	
	public void ladderDown(){
		GameObject player = GameObject.Find("Player");
		
		Transform[] path1 = {GameObject.Find("LadderTop").transform, GameObject.Find("LadderBottom").transform};
		iTween.MoveTo(player,iTween.Hash("path",path1,"time",1.5f,"oncomplete","execMoveLeft","easetype","easeoutsine"));
	}

	public void finishAction(Moving movComp){
			movComp.finishedAction = true;
		}

	
	
}
