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
			idleTime+=Time.deltaTime;
			if(idleTime > 10.0){
				this.stateMachine.changeState(new WanderState(stateMachine));
			}
		}
		
		public override void exitState(){
			idleTime = 0.0;
		}
	}


