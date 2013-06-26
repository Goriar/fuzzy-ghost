using System;
using UnityEngine;

public class TalkingState : AIState
{
	private double idleTime;
	private Character chatPartner;
	
	public TalkingState (StateMachine sm) : base(sm)
	{
		idleTime = 0.0;
	}
	
	public override void enterState(){
		idleTime = 0.0;
		stateMachine.Enemy.talking = true;
		chatPartner = stateMachine.Enemy.chatPartner;
		Moving partnerMov = chatPartner.getMovingComponent();
		Moving thisMov = stateMachine.Enemy.getMovingComponent();
		partnerMov.deactivateLerp();
		/*
		if(thisMov.viewDirection != partnerMov.viewDirection){
			if(partnerMov.viewDirection == DirectionEnum.LEFT)
				partnerMov.execMoveRight();
			else
				partnerMov.execMoveLeft();
		}
		*/
		Debug.Log("talking");
	}
		
	public override void updateAI(){
	
		idleTime+=Time.deltaTime;
		if(idleTime > 5.0){
			this.stateMachine.changeState(StateType.WANDER_STATE);
		}
	}
	
	public override void exitState(){
		stateMachine.Enemy.talking = false;
		stateMachine.Enemy.readyToTalk = false;
	}
}


