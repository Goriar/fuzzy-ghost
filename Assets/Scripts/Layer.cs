using UnityEngine;
using System.Collections;

public class Layer : MonoBehaviour {
	
	private float showSpeed = 1f;
	private float hideSpeed = 1f;
	private float showDelay = 0f;
	private float hideDelay = 0.5f;
	
	private float doorTransparency = 0.2f;
	
	
	// Use this for initialization
	void Start () {
		ObjectRegister.registerLayer(gameObject);
	}
	
	void hideLayer () {
		gameObject.GetComponent<Hide>().hide(hideSpeed, hideDelay);
	}
	
	void showBackgroundLayer () {
		gameObject.GetComponent<Hide>().show(showSpeed, showDelay);
	}
	
	void showForegroundLayer () {
		gameObject.GetComponent<Hide>().show(showSpeed, showDelay);
		
		foreach (Hide child in gameObject.GetComponentsInChildren<Hide>()){
    		if(child.tag == "alpha_door"){
        		child.makeTransparent(0.1f, showSpeed, hideDelay);
    		}
		}
		gameObject.GetComponent<Hide>().show(showSpeed, showDelay);
	}
	
	public void changeVisibilityByDependence() {
		showForegroundLayer();
		if (this.tag == "layer_front") {
			ObjectRegister.getLayer("layer_mid").BroadcastMessage("showBackgroundLayer");
			ObjectRegister.getLayer("layer_back").BroadcastMessage("showBackgroundLayer");
		} else if (this.tag == "layer_mid") {
			ObjectRegister.getLayer("layer_front").BroadcastMessage("hideLayer");
			ObjectRegister.getLayer("layer_back").BroadcastMessage("showBackgroundLayer");
		} else if (this.tag == "layer_back") {
			ObjectRegister.getLayer("layer_front").BroadcastMessage("hideLayer");
			ObjectRegister.getLayer("layer_mid").BroadcastMessage("hideLayer");
		}
	}
	
}
