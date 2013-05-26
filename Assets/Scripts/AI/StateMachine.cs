using System;
using System.Collections.Generic;
using UnityEngine;


public class StateMachine
{
	AIState currentState;
	private GameObject player;
	private Character enemy;
	HouseInventory house;
	
	public StateMachine (GameObject player, Character enemy)
	{
		this.player = player;
		this.enemy = enemy;
		house = new HouseInventory();
		currentState = new IdleState(this);
	
	}
	
	public void stateUpdate()
	{
		currentState.updateAI();
	}
	
	public void changeState(AIState state)
	{
		currentState.exitState();
		currentState = state;
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


