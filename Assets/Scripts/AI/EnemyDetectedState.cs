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
	}
	
	public override void updateAI()
	{
		if(npc.EnemyDetected)
		{
			player.Health -= 5 * Time.deltaTime;
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


