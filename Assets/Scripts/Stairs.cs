using UnityEngine;
using System.Collections;

public class Stairs {
	
	private static float workTime;
	private static float workIndex = 0;
	
	public static void goUpstairs(ref int index,float startTime){
		
		GameObject player = GameObject.Find("Player");
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

		if(player.transform.position.Equals(up)){
			workIndex++;
			workTime = Time.time;
			if(workIndex>2){
				index = -1;
				workIndex = 0;
			}
		}
	}
	
	public static void goDownstairs(ref int index, float startTime){
		
		 GameObject player = GameObject.Find("Player");
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
	}
	
	
		
	
}
