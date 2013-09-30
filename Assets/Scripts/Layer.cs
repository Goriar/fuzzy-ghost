using UnityEngine;
using System.Collections;

public class Layer : MonoBehaviour {
	
	// Werte für Layer Sichtbarkeits-Animation
	private float showSpeed = 1f;
	private float hideSpeed = 1f;
	private float showDelay = 0f;
	private float hideDelay = 0.5f;
	
	// Transparenz der Türen
	private float doorTransparency = 0.1f;
	
	
	// Use this for initialization
	void Start () {
		ObjectRegister.registerLayer(gameObject);
	}
	
	///
	/// Verstecke Layer
	///
	void hideLayer () {
		gameObject.GetComponent<Hide>().hide(hideSpeed, hideDelay);
	}
	
	///
	/// Zeige Hintergrund Layer
	///
	void showBackgroundLayer () {
		gameObject.GetComponent<Hide>().show(showSpeed, showDelay);
	}
	
	///
	/// Zeige aktuelle Layer
	///
	void showForegroundLayer () {
		gameObject.GetComponent<Hide>().show(showSpeed, showDelay);
		
		// Durchgeht alle Kindobjekte, die verstekct werden können
		foreach (Hide child in gameObject.GetComponentsInChildren<Hide>()){
    		// Wenn Tür in vordere Layer
    		if(child.tag == "alpha_door"){
        		// mache Tür teilweise Transparent, damit durchgesehen werden kann
        		child.makeTransparent(doorTransparency, showSpeed, hideDelay);
    		}
		}
		gameObject.GetComponent<Hide>().show(showSpeed, showDelay);
	}
	
	///
	/// Ändere Sichtbarkeit, abhängig von aktueller Layer
	///
	public void changeVisibilityByDependence() {
		showForegroundLayer();
		// wenn vordere Layer
		if (this.tag == "layer_front") {
			ObjectRegister.getLayer("layer_mid").BroadcastMessage("showBackgroundLayer");
			ObjectRegister.getLayer("layer_back").BroadcastMessage("showBackgroundLayer");
		// wenn mittlere Layer
		} else if (this.tag == "layer_mid") {
			ObjectRegister.getLayer("layer_front").BroadcastMessage("hideLayer");
			ObjectRegister.getLayer("layer_back").BroadcastMessage("showBackgroundLayer");
		// wenn hintere Layer
		} else if (this.tag == "layer_back") {
			ObjectRegister.getLayer("layer_front").BroadcastMessage("hideLayer");
			ObjectRegister.getLayer("layer_mid").BroadcastMessage("hideLayer");
		}
	}
	
}
