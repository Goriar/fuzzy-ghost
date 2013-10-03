using System;
using UnityEngine;

public class SearchingEnemyState : AIState
{
	Character ghostHunter;
	Player player;
	Moving movComp;
	float searchTimer;
	float turnTimer;
	public SearchingEnemyState (StateMachine sm) : base(sm) 
	{
		ghostHunter = sm.Enemy;
		player = sm.Player;
		movComp = ghostHunter.getMovingComponent();
		searchTimer = 0.0f;
	}
	
	public override void enterState ()
	{
		movComp = ghostHunter.getMovingComponent();
		movComp.stopLerp();
		searchTimer = 0.0f;
		turnTimer = 1.0f;
		stateMachine.Enemy.gameObject.BroadcastMessage("playAnimation","work");
	}
	
	public override void updateAI ()
	{
		//Bleibt an einer Stelle stehen und dreht sich mehrmals um, wechselt dann wieder in
		//die Wander State und legt eine Falle, falls es noch keine gibt
		stateMachine.Enemy.gameObject.BroadcastMessage("playAnimation","work");
		if(searchTimer>=5.0f){
			stateMachine.changeState(StateType.WANDER_STATE);
			if(!ghostHunter.isTrapActive()){
				GameObject trap = GameObject.Instantiate(ghostHunter.trapPrefab,
														ghostHunter.transform.position,
														ghostHunter.transform.rotation) as GameObject;
				trap.SetActive(true);
				ghostHunter.setTrapActive(true);
				Debug.Log("trap");
			}
		} else {
			if(turnTimer > 1.0f){
				if(movComp.viewDirection == DirectionEnum.RIGHT){
					movComp.execMoveLeft();	
					movComp.stopLerp();
				} else {
					movComp.execMoveRight();
					movComp.stopLerp();
				}
				turnTimer = 0.0f;
			}
			turnTimer+=Time.deltaTime;
		}
		searchTimer+=Time.deltaTime;
	}
	
	public override void exitState ()
	{
		turnTimer = 0.0f;
		searchTimer = 0.0f;
	}
}


