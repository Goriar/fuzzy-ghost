using System.Collections;
using UnityEngine;


	public class IdleState : AIState
	{
		private double idleTime;
		
		public IdleState (StateMachine stateMachine) : base(stateMachine)
		{
			idleTime = 0.0;
		}
		
		public override void enterState(){
			idleTime = 0.0;
		}
		
		public override void updateAI(){
		
			if(!stateMachine.Enemy.EnemyDetected){
				idleTime+=Time.deltaTime;
				if(idleTime > 5.0){
					this.stateMachine.changeState(StateType.WANDER_STATE);
				}
			}
			else{
				stateMachine.changeState(StateType.ENEMY_DETECTED_STATE);
			}
		}
		
		public override void exitState(){
			idleTime = 0.0;
		}
	}


