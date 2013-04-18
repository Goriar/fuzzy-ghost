using System;
using UnityEngine;
public class InteractionTypes
{
	public enum Type{
		NONE,PLAYER,TEST
	}
		
	private Type type;
	private string[] buttons;
		
	public InteractionTypes (Type t)
	{
		type = t;
			
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
		
	public void doSomething(int index){
		Debug.Log(buttons[index]);
	}

		
}


