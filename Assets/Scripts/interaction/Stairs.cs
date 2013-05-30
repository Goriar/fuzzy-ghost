using UnityEngine;
using System.Collections;

public class Stairs : MonoBehaviour{
	
	/*
	private static float workTime;			//Wird benötigt, wenn ein Wegpunkt erreicht wird
	
	private static float workIndex = 0;		//Wird benötigt, um zu bestimmen welcher Wegpunkt als nächstes angesteuert wird
	
	*/
	
	public RoomInventory lowerMainFloor, upperMainFloor;
	public int level = 1;
	
	//Nach Oben gehen. !!Namen der Wegpunkte sind fest vorgegeben!!
	//Wenn Aktion beendet wird index auf -1 gesetzt
	public void stairsUp(){
		
	 	Moving movingComp = GameObject.FindGameObjectWithTag("Player").GetComponent<Moving>();
		 movingComp.goToCallback+= this.goUpstairs;
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
		
		Transform path1 = GameObject.Find("StairsBottom").transform;
		Transform[] path2 = {GameObject.Find("StairsMid1").transform,GameObject.Find("StairsMid2").transform,GameObject.Find("StairsTop").transform};
		Hashtable table = new Hashtable();
		table.Add("position",path1);
		table.Add("time",2.0f);
		table.Add("easetype", "easeOutSine");
		table.Add("oncomplete","execMoveLeft");
		
		iTween.MoveTo(player,table);
		table.Remove("position");
		table.Remove("oncomplete");
		table.Add("path",path2);
		iTween.MoveTo(player,table);
	}
	
	public void goUpstairs(GameObject npc){
		
		
		
		Transform path1 = GameObject.Find("StairsBottom").transform;
		Transform[] path2 = {GameObject.Find("StairsMid1").transform,GameObject.Find("StairsMid2").transform,GameObject.Find("StairsTop").transform};
		Hashtable table = new Hashtable();
		table.Add("position",path1);
		table.Add("time",2.0f);
		table.Add("easetype", "easeOutSine");
		table.Add("oncomplete","execMoveLeft");
		
		iTween.MoveTo(npc,table);
		table.Remove("position");
		table.Remove("oncomplete");
		table.Add("path",path2);
		iTween.MoveTo(npc,table);
	}
	
	//Selbe wie oben, nur dass die Wegpunkte anders rum sind
	//Wenn Aktion beendet wird index auf -1 gesetzt
	public void stairsDown(){
		
		 Moving movingComp = GameObject.FindGameObjectWithTag("Player").GetComponent<Moving>();
		 movingComp.goToCallback+= this.goDownstairs;
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
		Transform path1 = GameObject.Find("StairsTop").transform;
		Transform[] path2 = {GameObject.Find("StairsMid2").transform,GameObject.Find("StairsMid1").transform,
							GameObject.Find("StairsBottom").transform};
		 Hashtable table = new Hashtable();
		 table.Add("position",path1);
		 table.Add("time",3.0f);
		 table.Add("easetype", "easeOutSine");
		 
		iTween.MoveTo(player,table);
		table.Remove("position");
		table.Remove("oncomplete");
		table.Add("path",path2);
		iTween.MoveTo(player,table);
	}
	
	public void goDownstairs(GameObject npc){

		Transform path1 = GameObject.Find("StairsTop").transform;
		Transform[] path2 = {GameObject.Find("StairsMid2").transform,GameObject.Find("StairsMid1").transform,
							GameObject.Find("StairsBottom").transform};
		 Hashtable table = new Hashtable();
		 table.Add("position",path1);
		 table.Add("time",3.0f);
		 table.Add("easetype", "easeOutSine");
		 
		iTween.MoveTo(npc,table);
		table.Remove("position");
		table.Remove("oncomplete");
		table.Add("path",path2);
		iTween.MoveTo(npc,table);
	}
	
	
		
	
}
