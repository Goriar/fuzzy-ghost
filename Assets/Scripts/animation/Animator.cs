using UnityEngine;
using System.Collections;

public class Animator : MonoBehaviour {
	
	private string currentAnimationName;
	private SpriteAnimation currentAnimation;
	private Texture defaultTexture;
	
	//
	// Initiales Setzen der default Texture (keine Bewegung)
	void Start () {
		defaultTexture = renderer.material.mainTexture;
	}
	
	/// 
	/// spielt Animation mit gewünschtem Namen ab
	/// @param _name		Name der abzuspielenden Animation
	/// 
	public void playAnimation (string _name) {
		// Spiele nur ab, wenn nicht schon aktuelle Animation
		if (_name != currentAnimationName) {
			
			// Array mit allen verfügbaren Animationen
			SpriteAnimation[] sprites = gameObject.GetComponents<SpriteAnimation>();
		
			// Stopt aktuelle Animation
			if (currentAnimation) {
				currentAnimation.stop();
			}
			
			// Durchsucht Animationen bis richtige gefunden wird
			foreach (SpriteAnimation sprite in sprites) {
				if (sprite.name == _name) {
					// setzt aktuelle Animation und spielt sie ab
					currentAnimation = sprite;
					currentAnimationName = _name;
					currentAnimation.play();
					break; // Abbruch der Schleife, wenn gefunden
				}
			}
		}
	}
	
	/// 
	/// stoppt Animation mit gewünschtem Namen
	/// @param _name		Name der zu stoppenden Animation
	/// 
	public void stopAnimation (string _name) {
		// Abfrage der zu stoppenden Animation, damit nur die richtige/aktuelle gestoppt wird, andere werden ignoriert
		if (currentAnimationName == _name) {
			stopAnimation();
		}
	}
	
	/// 
	/// stoppt jegliche Animation
	/// 
	public void stopAnimation () {
		// Wenn aktuelle Animation vorhanden
		if (currentAnimation) {
			currentAnimation.stop();
			currentAnimation = null; // reset
			currentAnimationName = ""; // reset Name
			// Setzt default Textur
			renderer.material.mainTexture = defaultTexture;
			renderer.material.mainTextureOffset = new Vector2(0,0);
			// Orientierung des Sprites, je nach Bewegungsrichtung
			if (gameObject.GetComponent<Moving>().viewDirection == DirectionEnum.RIGHT) {
				renderer.material.mainTextureScale = new Vector2(-1,1); // Negativ Skallierung um zu spiegeln
			} else {
				renderer.material.mainTextureScale = new Vector2(1,1);
			}
		}
	}
	
	
	
}
