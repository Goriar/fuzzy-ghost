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
		npc.GetComponent<Moving>().deactivateLerp();
		stateMachine.Enemy.BroadcastMessage("playAnimation", "scare");
	}
	public override void updateAI()
	{
		Moving movComp = npc.getMovingComponent();
		if(npc.enemyDetected)
		{
			if(player.transform.position.x < npc.transform.position.x){
				movComp.execMoveRight();	
				movComp.deactivateLerp();
			} else {
				movComp.execMoveLeft();
				movComp.deactivateLerp();
			}
			stateMachine.Enemy.BroadcastMessage("playAnimation", "scare");	
			player.applyDamage(2.0f*Time.deltaTime);
			Debug.Log("Enemy Detected!");
		}
		else{
			stateMachine.changeState(StateType.IDLE_STATE);
		}
	}
	
	public override void exitState()
	{
		
	}
}


