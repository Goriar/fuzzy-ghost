using UnityEngine;
using System.Collections;

public class SpriteAnimation : MonoBehaviour {
	
	private bool active = false;				// mache nichts, bevor aktiv
	private bool startedRoutine;				// Gibt an, ob die CoRoutine gestartet wurde
	
	public string name = "Animation";
	
	private Material blockMaterial;
	public Texture blockTexture;
	
	public bool isPlaying;						// gibt an, ob die Animation abgespielt wird
	public bool loop;							// gibt an, ob die Animation geloopt werden soll
	private bool animationComplete = false;		// gibt an, ob die Animation fertig angespielt wurde
	
	public int _totalFrames;
	public int _currentFrame = 1;
	
	// Anzahl der Tiles im Block
	public int blockWidth = 8;
	public int blockHeight = 2;
	
	// Breite und Höhe eines Tiles in Px
	public int tileWidth = 500;
	public int tileHeight = 1000;
	
	public float AnimationDeltaTime = 0.3f;		// Dauer eines Animationsschrittes in Sek
			
	public void play () {
		if (!active) {
			active = true;
			
			if (!isPlaying) {
				isPlaying = true;
				animationComplete = false;
				blockMaterial.mainTexture = blockTexture;
								
				if (!startedRoutine) {
					StopCoroutine("Draw");
					StartCoroutine("Draw");
					startedRoutine = true;
				}
			}
		}			
	}
	
	public void stop () {
		isPlaying = false;
		active = false;
		startedRoutine = false;
		_currentFrame = 1;
	}
	
	public IEnumerator Draw () {
		while (isPlaying) {
			if (active) {

				if (_currentFrame > _totalFrames) {
					_currentFrame -= _totalFrames;
				}
												
				int _offsetX = (_currentFrame - 1) % blockWidth; 
				int _offsetY = (_currentFrame - 1) / blockWidth;
				
				float directionSwitch = 1f;
				
				// wenn Objekt nach links bewegt, spiegel die Textur mit negativem X Scale
				if (gameObject.GetComponent<Moving>().viewDirection == DirectionEnum.RIGHT) {
					directionSwitch = -1f;
					_offsetX = (_currentFrame) % blockWidth;
				}
				
				//Set the texture to the indicated offset 
				blockMaterial.mainTextureOffset = new Vector2 (_offsetX / (float) blockWidth, 1f - ((_offsetY + 1) / (float) blockHeight)); 
				//Change the scale of the texture 
				blockMaterial.mainTextureScale = new Vector2 ( 1f / (float) blockWidth * directionSwitch, 1f / (float) blockHeight); 
			}

			// next steps! 
			_currentFrame++; 
			if (_currentFrame > _totalFrames) {
				if (loop) {
					_currentFrame = 1;
				} else {
					_currentFrame = _totalFrames; // stop at the last frame
					animationComplete = true;
					BroadcastMessage("stopAnimation", name);
					break; // stopt Schleife
				}
			}	
			
			// Warte für AnimationDeltaTime Sekunden (falls Animation noch spielt)
			if (isPlaying) {
				yield return new WaitForSeconds(AnimationDeltaTime);
			} else {
				yield return null;
				break;
			}	
		}
	}
	
	public float getDuration () {
		return _totalFrames * AnimationDeltaTime;
	}
	
	void Awake () {
		blockMaterial = renderer.material;
	}
	
}
