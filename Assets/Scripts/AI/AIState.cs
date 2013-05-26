using System;


	public abstract class AIState
	{
		protected StateMachine stateMachine;
		
		public AIState (StateMachine stateMachine)
		{
			this.stateMachine = stateMachine;
		}
		
		public abstract void enterState();
		public abstract void updateAI();
		public abstract void exitState();
	}


