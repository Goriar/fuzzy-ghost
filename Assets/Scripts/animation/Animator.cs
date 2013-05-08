using UnityEngine;
using System.Collections;

public class Animator : MonoBehaviour {
	
	private string currentAnimationName;
	private SpriteAnimation currentAnimation;
	private Texture defaultTexture;
	
	void Start () {
		defaultTexture = renderer.material.mainTexture;
	}
	
	public void playAnimation (string _name) {
		
		if (_name != currentAnimationName) {
			
			SpriteAnimation[] sprites = gameObject.GetComponents<SpriteAnimation>();
		
			// Stopt aktuelle Animation
			if (currentAnimation) {
				currentAnimation.stop();
			}
			
			foreach (SpriteAnimation sprite in sprites) {
				if (sprite.name == _name) {
					currentAnimation = sprite;
					currentAnimationName = _name;
					currentAnimation.play();
					break;
				}
			}
		}
	}
	
	public void stopAnimation (string _name) {
		if (currentAnimationName == _name) {
			stopAnimation();
		}
	}
	
	public void stopAnimation () {
		if (currentAnimation) {
			currentAnimation.stop();
			currentAnimation = null;
			currentAnimationName = "";
			renderer.material.mainTexture = defaultTexture;
			renderer.material.mainTextureOffset = new Vector2(0,0);
			if (gameObject.GetComponent<Moving>().viewDirection == DirectionEnum.RIGHT) {
				renderer.material.mainTextureScale = new Vector2(-1,1);
			} else {
				renderer.material.mainTextureScale = new Vector2(1,1);
			}
		}
	}
	
	
	
}
