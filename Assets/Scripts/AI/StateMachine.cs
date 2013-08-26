using System;
using System.Collections.Generic;
using UnityEngine;


public class StateMachine
{
	AIState currentState;
	StateType currentStateType;
	AIState[] stateList;
	private Player player;
	private Character enemy;
	
	public StateMachine (Player player, Character enemy)
	{
		this.player = player;
		this.enemy = enemy;
		currentState = new IdleState(this);
		currentStateType = StateType.IDLE_STATE;
		currentState.enterState();
		
		if(enemy.cType != CharacterType.GHOST_HUNTER){
			stateList = new AIState[6];
			stateList[(int)StateType.IDLE_STATE] = new IdleState(this);
			stateList[(int)StateType.WANDER_STATE] = new WanderState(this);
			stateList[(int)StateType.ENEMY_DETECTED_STATE] = new EnemyDetectedState(this);
			stateList[(int)StateType.TALKING_STATE] = new TalkingState(this);
			stateList[(int)StateType.SCARED_STATE] = new ScaredState(this);
			stateList[(int)StateType.FLEE_STATE] = new FleeState(this);
		} else {
			stateList = new AIState[4];
			stateList[(int)StateType.IDLE_STATE] = new IdleState(this);
			stateList[(int)StateType.WANDER_STATE] = new WanderState(this);
			stateList[(int)StateType.FLEE_STATE] = new FleeState(this);
			stateList[(int)StateType.HUNTING_ENEMY_STATE] = new HuntingEnemyState(this);
		}
	
	}
	
	public void stateUpdate()
	{
		currentState.updateAI();
	}
	
	public void changeState(StateType state)
	{
		currentState.exitState();
		currentState = stateList[(int)state];
		currentStateType = state;
		currentState.enterState();
	}
	
	public StateType getState(){
		return currentStateType;
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


