using System.Collections;
using UnityEngine;


	public class WanderState : AIState
	{
		public WanderState (StateMachine sm) : base(sm)
		{
		}
		
		public override void enterState(){
			stateMachine.Enemy.setCharacterPath();
		}
		
		public override void updateAI(){
			
		}
		
		public override void exitState(){
		
		}
	}


