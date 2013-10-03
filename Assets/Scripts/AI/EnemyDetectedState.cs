using System;
using UnityEngine;

public class EnemyDetectedState : AIState
{
	
	Character npc;
	Player player;
	
	public EnemyDetectedState (StateMachine sm) : base(sm)
	{
		npc = sm.Enemy;
		player = sm.Player;
	}
	
	public override void enterState()
	{
		npc.GetComponent<Moving>().stopLerp();
		stateMachine.Enemy.BroadcastMessage("playAnimation", "scare");
	}
	public override void updateAI()
	{
		Moving movComp = npc.getMovingComponent();
		if(npc.enemyDetected && player.canBeSeen())
		{
			if(player.transform.position.x < npc.transform.position.x){
				movComp.execMoveRight();	
				movComp.stopLerp();
			} else {
				movComp.execMoveLeft();
				movComp.stopLerp();
			}
			stateMachine.Enemy.BroadcastMessage("playAnimation", "scare");	
			player.applyDamage(10.0f*Time.deltaTime);
		}
		else{
			stateMachine.changeState(StateType.IDLE_STATE);
		}
	}
	
	public override void exitState()
	{
		
	}
}


