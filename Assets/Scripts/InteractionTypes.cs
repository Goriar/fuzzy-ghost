using System;
using UnityEngine;
public class InteractionTypes
{
	public enum Type{
		NONE,PLAYER,TEST,STAIRS_UP,STAIRS_DOWN
	}
		
	private Type type;
	private string[] buttons;
	private int workIndex;
	private float workTime;
		
	public InteractionTypes (Type t)
	{
		type = t;
		workIndex = 0;
			
		switch(type){
		case Type.PLAYER:
			buttons = new string[2];
			buttons[0] = "Test";
			buttons[1] = "Exit";
			break;
		case Type.TEST:
			buttons = new string[3];
			buttons[0] = "Lol";
			buttons[1] = "Trolol";
			buttons[2] = "Exit";
			break;
		case Type.STAIRS_UP:
			buttons = new string[2];
			buttons[0] = "Go up";
			buttons[1] = "Exit";
			break;
		case Type.STAIRS_DOWN:
			buttons = new string[2];
			buttons[0] = "Go Dwon";
			buttons[1] = "Exit";
			break;
		default:
			buttons = new string[2];
			buttons[0] = "MISSING";
			buttons[1] = "Exit";
			break;
		}
	}
	
	public string[] getButtonTexts(){
		return buttons;
	}
		
	public void doSomething(ref int index, float startTime){
		if(type==Type.STAIRS_UP){
			if(index == 0){
				
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
					stairUp = GameObject.Find("StairsUp");
				}
				Vector3 up = stairUp.transform.position;
				Vector3 bot = stairBot.transform.position;
				
				float journeyLength = Vector3.Distance(bot,up);
				float distCovered = (Time.time - workTime) * 10.0f;
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
		
		if(type==Type.STAIRS_DOWN){
			if(index == 0){
				
				 GameObject player = GameObject.Find("Player");
				GameObject stairUp = null;
				GameObject stairBot = null;
				if(workIndex == 0){
					workTime = startTime;
					stairBot = GameObject.Find("StairsUp");
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
				float distCovered = (Time.time - workTime) * 10.0f;
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
	}

		
}


