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
			stateMachine.Enemy.BroadcastMessage("playAnimation", "work");
		}
		
		public override void updateAI(){
		
				idleTime+=Time.deltaTime;
				if(idleTime > 5.0f){
					this.stateMachine.changeState(StateType.WANDER_STATE);
				}
		}
		
		public override void exitState(){
			idleTime = 0.0f;
		}
	}


