using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
	private InteractionTypes interaction;
	public InteractionTypes.Type type;
	// Use this for initialization
	void Start ()
	{
		interaction = new InteractionTypes(type);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public string[] getButtonTexts(){
		return interaction.getButtonTexts();
	}
	
	public void doSomething(int index){
		interaction.doSomething(index);
	}
	
}

