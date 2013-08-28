using System;
using UnityEngine;


public class HuntingEnemyState : AIState
{
	Player player;
	Character ghostHunter;
	Moving movComp;
	public HuntingEnemyState (StateMachine sm) : base(sm)
	{
		player = sm.Player;
		ghostHunter = sm.Enemy;
		movComp = ghostHunter.getMovingComponent();
	}
	
	public override void enterState ()
	{
		movComp = ghostHunter.getMovingComponent();
		movComp.deactivateLerp();
	}
	
	public override void updateAI ()
	{
		
		if(ghostHunter.enemyDetected)
		{
			movComp.goToX(player.transform.position.x);
			float extraDamage = 0.0f;
			float distance = (ghostHunter.transform.position - player.transform.position).magnitude;
			
			if(distance < 1.0f){
				extraDamage = 10.0f;
			}
			player.applyDamage((5.0f+extraDamage)*Time.deltaTime);
			
		}
		else{
			//Sollte der Jäger dem Geist durch die Tür hinterher laufen?
			stateMachine.changeState(StateType.SEARCHING_ENEMY_STATE);
		}
		
	}
	
	public override void exitState ()
	{
		
	}
}


