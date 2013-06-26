using System;
using UnityEngine;
public class InteractionTypes
{
	public enum Type{
		NONE,USE,OPEN,TAKE,COMBINE,STAIRS_UP,STAIRS_DOWN
	}
	
	public struct TypeButton {
		public Texture texture;
		public string name;
		public string methodName;
		public string infoText;
	}
	
	private Type type;				//Bestimmt den Typ des Objekts
	
	private TypeButton button;		//Eigenschaften des Buttons

		
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
			break;
		case Type.OPEN:
			button = new TypeButton();
			button.name = "open";
			button.methodName = "use";
			button.infoText = "Öffne Objekt";
			break;
		case Type.TAKE:
			button = new TypeButton();
			button.name = "take";
			button.methodName = "take";
			button.infoText = "Nimm Objekt";
			break;
		case Type.COMBINE:
			button = new TypeButton();
			button.name = "combine";
			button.methodName = "combine";
			button.infoText = "Kombiniere Gegenstände";
			break;
		case Type.STAIRS_UP:
			button = new TypeButton();
			button.name = "stairs_up";
			button.methodName = "stairsUp";
			button.infoText = "Gehe nach oben";
			break;
		case Type.STAIRS_DOWN:
			button = new TypeButton();
			button.name = "stairs_down";
			button.methodName = "stairsDown";
			button.infoText = "Gehe nach unten";
			break;
		default:
			break;
		}
	}
	
	public string getButtonText(){
		return button.infoText;
	}
	
	public string getMethod() {
		return button.methodName;
	}

		
}


