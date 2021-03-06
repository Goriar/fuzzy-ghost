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

		stateMachine.Enemy.setCharacterPath(null);
		movingComoponent = stateMachine.Enemy.getMovingComponent();
	}
	
	public override void updateAI(){
			
		stateMachine.Enemy.BroadcastMessage("playAnimation", "move");
	
		if(stateMachine.Enemy.getCharacterPath().Length == 0){
			if(movingComoponent.finishedAction){
				if(stateMachine.Enemy.cType == CharacterType.NORMAL){
					stateMachine.changeState(StateType.IDLE_STATE);
				} else {
					stateMachine.changeState(StateType.SEARCHING_ENEMY_STATE);
				}
				return;
			}
		}
		else {	
			// Wenn grade keine Bewegung mehr stattfindet ...
			if(movingComoponent.finishedAction)
			{
				// ... frage nach, ob Objekt sich zu nächstem Objekt bewegen muss, ...
				if(!movingToTransition){
					// ... erhalte das nächste Ziel, ...
					nextTarget = stateMachine.Enemy.popNextTarget();
					// ... frage nach ob nächstes Ziel eine Tür oder Treppe ist und setze dementsprechend Transitions Bool Wert.
					if(nextTarget.GetComponent<Door>()!=null || nextTarget.GetComponent<Stairs>()!=null)
					{
						movingToTransition = true;
					}
					
					// Gehe dann zum nächsten Ziel
					movingComoponent.goToObject(nextTarget);
				} 
				// Wenn nächtes Ziel Tür oder Treppe
				else
				{
					// Wenn Tür ...
					if(nextTarget.GetComponent<Door>()!=null){
						// ... setze temp. Variable Tür und benutze Tür
						Door door = nextTarget.GetComponent<Door>();
						door.use(movingComoponent);
					
						// Keine Transition mehr
						movingToTransition = false;
					}
					// Wenn Treppe ...
					else{
						// ... setze temp. Variable für die Treppe
						Stairs stairs = nextTarget.GetComponent<Stairs>();
						// Wenn Treppe zum zweiten Stockwerk ...
						if(stairs.level == 1){
							// ... benutze Treppe nach oben ...
							stairs.goUpstairs(stateMachine.Enemy.gameObject);
						}
						// Wenn Treppe zum ersten Stock ...
						else{
							// ... benutze Treppe nach unten ...
							stairs.goDownstairs(stateMachine.Enemy.gameObject);
						}
						// Keine Transition mehr
						movingToTransition = false;
					}
				}
			}
		}
	}
	
	public override void exitState()
	{	
		stateMachine.Enemy.resetCurrentValue();
	}
}


