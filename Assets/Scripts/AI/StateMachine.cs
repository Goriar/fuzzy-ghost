using System;
using System.Collections.Generic;
using UnityEngine;


public class StateMachine
{
	AIState currentState;
	private GameObject player;
	private Character enemy;
	
	public StateMachine (GameObject player, Character enemy)
	{
		this.player = player;
		this.enemy = enemy;
		currentState = new IdleState(this);
		currentState.enterState();
	
	}
	
	public void stateUpdate()
	{
		currentState.updateAI();
	}
	
	public void changeState(AIState state)
	{
		currentState.exitState();
		currentState = state;
		currentState.enterState();
	}
	
	public GameObject Player{
		get{ return player;}
		set{ player = value;}
	}
	
	public Character Enemy{
		get{ return enemy;}
		set{ enemy = value;}
	}

	public GameObject[] getPathObjects()
	{
		return null;
	}	
}


