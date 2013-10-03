using System;
using UnityEngine;


public class FleeState : AIState
{
	 Moving movingComponent;
	private bool movingToTransition;
	private GameObject nextTarget;
	private GameObject fleeSpot;
	
	public FleeState (StateMachine sm) : base(sm)
	{
		nextTarget = null;
		fleeSpot = GameObject.FindGameObjectWithTag("flee_spot");
	}
	
	public override void enterState ()
	{
		movingComponent = stateMachine.Enemy.getMovingComponent();
		movingComponent.stopLerp();
		stateMachine.Enemy.setCharacterPath(fleeSpot);
		stateMachine.Enemy.readyToTalk = false;
		stateMachine.Enemy.dialogueTime = 0.0f;
	}
	
	public override void updateAI ()
	{
		//Das Selbe wie in der Wander State
		//Charakter hat Ort erreicht
		if(stateMachine.Enemy.getCharacterPath().Length == 0){
			Vector3 distance = stateMachine.Enemy.transform.position-fleeSpot.transform.position;
			if(distance.magnitude<=0.5f){
				stateMachine.changeState(StateType.IDLE_STATE);
			}
		} 
		//Charakter bewegt sich zu einem Ort
		else {
		if(movingComponent.finishedAction)
		{
			if(!movingToTransition){
				nextTarget = stateMachine.Enemy.popNextTarget();
				if(nextTarget.GetComponent<Door>()!=null || nextTarget.GetComponent<Stairs>()!=null)
				{
					movingToTransition = true;
				}
				
				movingComponent.goToObject(nextTarget);
			} 
			else
			{
				if(nextTarget.GetComponent<Door>()!=null){
					Door door = nextTarget.GetComponent<Door>();
					door.use(movingComponent);
					for(int i = 0; i<door.connectedRooms.Length; ++i)
					{
						if(!door.connectedRooms[i].Equals(stateMachine.Enemy.currentLocation)){
							stateMachine.Enemy.currentLocation = door.connectedRooms[i];
							break;
							
						}
					}
					movingToTransition = false;
				}
				else{
					Stairs stairs = nextTarget.GetComponent<Stairs>();
					if(stairs.level == 1){
						stairs.goUpstairs(stateMachine.Enemy.gameObject);
						stateMachine.Enemy.currentLocation = stairs.upperMainFloor;
					}
					else{
						stairs.goDownstairs(stateMachine.Enemy.gameObject);
						stateMachine.Enemy.currentLocation = stairs.lowerMainFloor;
					}
					movingToTransition = false;
				}
			}
		}
	}
}
	
	public override void exitState ()
	{
		if(stateMachine.Enemy.cType == CharacterType.GHOST_HUNTER){
			stateMachine.Player.resetAttention();
		}
		stateMachine.Enemy.gameObject.SetActive(false);
	}
}


