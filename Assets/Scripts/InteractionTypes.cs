using System;
using UnityEngine;
public class InteractionTypes
{
	public enum Type{
		NONE,TEST,STAIRS_UP,STAIRS_DOWN
	}
		
	private Type type;
	private string[] buttons;

		
	public InteractionTypes (Type t)
	{
		type = t;
			
		switch(type){
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
			buttons[0] = "Go Down";
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
		
		//Going Upstairs
		if(type==Type.STAIRS_UP){
			if(index == 0){
				
				Stairs.goUpstairs(ref index,startTime);			
			}	
			else{
				index = -1;
			}
		}
		
		//Going Downstairs
		if(type==Type.STAIRS_DOWN){
			if(index == 0){
				
				Stairs.goDownstairs(ref index,startTime);				
			}	
			else{
				index = -1;
			}
		}
		
		if(type==Type.TEST||type==Type.NONE){
			index = -1;
		}
	}

		
}


