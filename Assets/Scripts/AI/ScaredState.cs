using System;
using UnityEngine;

public class ScaredState : AIState
{
	Moving movingComponent;
	private float idleTime;
	bool movingToTransition;
	private GameObject nextTarget;
	private GameObject[] panicSpots;
	private GameObject finalTarget;
	
	
	public ScaredState (StateMachine sm) : base(sm)
	{
		movingComponent = stateMachine.Enemy.getMovingComponent();
		nextTarget = null;
		panicSpots = GameObject.FindGameObjectsWithTag("panic_spot");
	} 
	
	public override void enterState(){
		finalTarget = null;
		float bestDistance = 0f;
		stateMachine.Enemy.getMovingComponent().deactivateLerp();
		foreach(GameObject g in panicSpots){
			Vector3 v = g.transform.position - stateMachine.Enemy.gameObject.transform.position;
			if(v.magnitude > bestDistance){
				finalTarget = g;
				bestDistance = v.magnitude;
			}
		}
		stateMachine.Enemy.setCharacterPath(finalTarget);
		
		movingComponent = stateMachine.Enemy.getMovingComponent();
		idleTime = 0.0f;
		movingToTransition = false;
		stateMachine.Enemy.readyToTalk = false;
		stateMachine.Enemy.dialogueTime = 0.0f;
		AudioSource audio = stateMachine.Enemy.gameObject.GetComponent<AudioSource>();
		audio.clip = stateMachine.Enemy.screamAudio;
		audio.Play();
		stateMachine.Enemy.BroadcastMessage("playAnimation", "scare");
	}
	
	public override void updateAI(){
		
		//Charakter hat Ort erreicht
		if(stateMachine.Enemy.getCharacterPath().Length == 0){
			Vector3 distance = stateMachine.Enemy.transform.position-finalTarget.transform.position;
			if(distance.magnitude<=0.2f){
				stateMachine.changeState(StateType.WANDER_STATE);
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
	
	public override void exitState(){
		stateMachine.Enemy.resetCurrentValue();
		idleTime =0.0f;
	}
}


