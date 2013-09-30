using System;
using UnityEngine;
public class InteractionTypes
{
	public enum Type{
		NONE,USE,OPEN,TAKE,COMBINE,STAIRS_UP,STAIRS_DOWN,LADDER_UP,LADDER_DOWN,START_GAME,QUIT_GAME,CURSE
	}
	
	public struct TypeButton {
		public Texture texture;
		public string name;
		public string methodName;
		public string infoText;
		public int iconPos;
	}
	
	private Type type;				//Bestimmt den Typ des Objekts
	
	private TypeButton button;		//Eigenschaften des Buttons
	
	private float OffsetWidth = 1f/10f;
	
		
	public InteractionTypes (Type t)
	{
		type = t;
		
		//Je nach Typ den Text der Buttons bestimmen
		switch(type){
		case Type.USE:
			button = new TypeButton();
			button.name = "use";
			button.methodName = "use";
			button.infoText = "Benutze Objekt";
			button.iconPos = 5;
			break;
		case Type.OPEN:
			button = new TypeButton();
			button.name = "open";
			button.methodName = "use";
			button.infoText = "Öffne Objekt";
			button.iconPos = 0;
			break;
		case Type.TAKE:
			button = new TypeButton();
			button.name = "take";
			button.methodName = "take";
			button.infoText = "Nimm Objekt";
			button.iconPos = 1;
			break;
		case Type.COMBINE:
			button = new TypeButton();
			button.name = "combine";
			button.methodName = "combine";
			button.infoText = "Kombiniere Gegenstände";
			button.iconPos = 6;
			break;
		case Type.STAIRS_UP:
			button = new TypeButton();
			button.name = "stairs_up";
			button.methodName = "stairsUp";
			button.infoText = "Gehe nach oben";
			button.iconPos = 3;
			break;
		case Type.STAIRS_DOWN:
			button = new TypeButton();
			button.name = "stairs_down";
			button.methodName = "stairsDown";
			button.infoText = "Gehe nach unten";
			button.iconPos = 4;
			break;
		case Type.LADDER_UP:
			button = new TypeButton();
			button.name = "ladder_up";
			button.methodName = "ladderUp";
			button.infoText = "Kletter nach oben";
			button.iconPos = 3;
			break;
		case Type.LADDER_DOWN:
			button = new TypeButton();
			button.name = "ladder_down";
			button.methodName = "ladderDown";
			button.infoText = "Kletter nach unten";
			button.iconPos = 4;
			break;
		case Type.START_GAME:
			button = new TypeButton();
			button.name = "start_game";
			button.methodName = "startGame";
			button.infoText = "Starte ein neues Spiel";
			button.iconPos = 1;
			break;
		case Type.QUIT_GAME:
			button = new TypeButton();
			button.name = "quit_game";
			button.methodName = "quitGame";
			button.infoText = "Beende das Spiel";
			button.iconPos = 0;
			break;
		case Type.CURSE:
			button = new TypeButton();
			button.name = "curse_object";
			button.methodName = "curseObject";
			button.infoText = "Verfluche einen Gegenstand";
			button.iconPos = 2;
			break;
			
		default:
			break;
			
		}
	}
	
	public string getButtonText(){
		return button.infoText;
	}
	
	public float getButtonOffset() {
		return button.iconPos*OffsetWidth;
	}
	
	
	public string getMethod() {
		return button.methodName;
	}

	public InteractionTypes.Type getInteractionType(){
		return type;
	}
	
}


