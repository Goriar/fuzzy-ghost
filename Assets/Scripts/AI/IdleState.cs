using System.Collections;
using UnityEngine;


	public class IdleState : AIState
	{
		private float idleTime;
		
		public IdleState (StateMachine stateMachine) : base(stateMachine)
		{
			idleTime = 0.0f;
		}
		
		public override void enterState(){
			idleTime = 0.0f;
		}
		
		public override void updateAI(){
		
		
			if(!stateMachine.Enemy.EnemyDetected){
				idleTime+=Time.deltaTime;
				if(idleTime > 5.0f){
					this.stateMachine.changeState(StateType.WANDER_STATE);
				}
			}
			else{
				stateMachine.changeState(StateType.ENEMY_DETECTED_STATE);
			}
		}
		
		public override void exitState(){
			idleTime = 0.0f;
		}
	}


