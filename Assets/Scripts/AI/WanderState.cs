using System.Collections;
using UnityEngine;


public class WanderState : AIState
{
	private Moving movingComoponent;
	private bool movingToTransition;
	private GameObject nextTarget;
	
	public WanderState (StateMachine sm) : base(sm)
	{
		movingComoponent = stateMachine.Enemy.getMovingComponent();
	}
	
	public override void enterState(){
		stateMachine.Enemy.setCharacterPath();
	
	}
	
	public override void updateAI(){
		if(stateMachine.Enemy.getCharacterPath().Length == 0){
			stateMachine.changeState(new IdleState(stateMachine));
		}
		else{
			if(movingComoponent.finishedAction)
			{
				Debug.Log ("new action");
				if(!movingToTransition){
					nextTarget = stateMachine.Enemy.popNextTarget();
					if(nextTarget.GetComponent<Door>()!=null)
					{
						Door door = nextTarget.GetComponent<Door>();
						movingToTransition = true;
					}
					movingComoponent.goToObject(nextTarget);
					
				} 
				else
				{
					Debug.Log("else case");
					Door door = nextTarget.GetComponent<Door>();
					door.use(movingComoponent);
					for(int i = 0; i<door.connectedRooms.Length; ++i)
					{
						if(!door.connectedRooms[i].Equals(stateMachine.Enemy.currentLocation)){
							stateMachine.Enemy.currentLocation = door.connectedRooms[i];
							break;
							Debug.Log("fuck");
						}
					}
					movingToTransition = false;
				}
			}
			else{
				
				
			}
		}
	
	}
	
	public override void exitState()
	{	
	}
}


