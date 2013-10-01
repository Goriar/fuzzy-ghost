using System;
using UnityEngine;

public class TalkingState : AIState
{
	private float idleTime;
	private Character chatPartner;
	
	public TalkingState (StateMachine sm) : base(sm)
	{
		idleTime = 0.0f;
	}
	
	public override void enterState(){
		idleTime = 0.0f;
		stateMachine.Enemy.talking = true;
		chatPartner = stateMachine.Enemy.chatPartner;
		Moving partnerMov = chatPartner.getMovingComponent();
		Moving thisMov = stateMachine.Enemy.getMovingComponent();
		thisMov.stopLerp();
		
		if(thisMov.viewDirection == partnerMov.viewDirection){
			if(thisMov.viewDirection == DirectionEnum.LEFT)
				thisMov.faceLeft();
			else
				thisMov.faceRight();
		}
		stateMachine.Enemy.BroadcastMessage("playAnimation", "work");
	
	}
		
	public override void updateAI(){
	
		idleTime+=Time.deltaTime;
		if(idleTime > 5.0f){
			this.stateMachine.changeState(StateType.WANDER_STATE);
		}
	}
	
	public override void exitState(){
		stateMachine.Enemy.talking = false;
		stateMachine.Enemy.readyToTalk = false;
		stateMachine.Enemy.dialogueTime = 0;
	}
}


