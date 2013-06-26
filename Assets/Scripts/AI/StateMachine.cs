using System;
using System.Collections.Generic;
using UnityEngine;


public class StateMachine
{
	AIState currentState;
	AIState[] stateList;
	private Player player;
	private Character enemy;
	
	public StateMachine (Player player, Character enemy)
	{
		this.player = player;
		this.enemy = enemy;
		currentState = new IdleState(this);
		currentState.enterState();
		
		stateList = new AIState[4];
		stateList[(int)StateType.IDLE_STATE] = new IdleState(this);
		stateList[(int)StateType.WANDER_STATE] = new WanderState(this);
		stateList[(int)StateType.ENEMY_DETECTED_STATE] = new EnemyDetectedState(this);
		stateList[(int)StateType.TALKING_STATE] = new TalkingState(this);
	
	}
	
	public void stateUpdate()
	{
		currentState.updateAI();
	}
	
	public void changeState(StateType state)
	{
		currentState.exitState();
		currentState = stateList[(int)state];
		currentState.enterState();
	}
	
	public Player Player{
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


