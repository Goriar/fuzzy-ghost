using UnityEngine;
using System.Collections;

public class SpriteAnimation : MonoBehaviour {
	
	private bool active = false;								// mache nichts, bevor aktiv
	private bool startedRoutine;								// Gibt an, ob die CoRoutine gestartet wurde
	
	public string name = "Animation";
	
	private Material blockMaterial;
	public Texture blockTexture;
	
	public bool isPlaying;											// gibt an, ob die Animation abgespielt wird
	public bool loop;													// gibt an, ob die Animation geloopt werden soll
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
			
	/// 
	/// spielt Animation  ab
	///
	public void play () {
		// Wenn Animation noch nicht aktiv
		if (!active) {
			active = true;
			
			// Wenn Animation noch nicht spielt
			if (!isPlaying) {
				isPlaying = true;
				animationComplete = false;
				blockMaterial.mainTexture = blockTexture;
								
				if (!startedRoutine) {
					StopCoroutine("Draw"); // Stoppt Co-Routine
					StartCoroutine("Draw"); // Startet neue Co-Routine
					startedRoutine = true;
				}
			}
		}			
	}
	
	/// 
	/// stoppt Animation
	/// 
	public void stop () {
		isPlaying = false;
		active = false;
		startedRoutine = false;
		_currentFrame = 1;
	}
	
	/// 
	/// zeichnet Animation immer neu, bis gestoppt wird
	/// 
	public IEnumerator Draw () {
		// Schleife, solange Animation gespielt werden soll (als Co-Routine => Parallelisierug)
		while (isPlaying) {
			// führe nur aus, wenn aktiv
			if (active) {
				
				// Wenn aktuelle Frame über max. Anzahl Frames schreitet, setze auf letzte Frame
				if (_currentFrame > _totalFrames) {
					_currentFrame -= _totalFrames;
				}
				
				// Berechne Offset (X-Y-Achse)
				// x mit Modulo => Umbruch auf aktuelle Spalte (X-Achse) in Zeile (Y-Achse)
				// y mit Bruch => Umbruch auf aktuelle Zeile
				int _offsetX = (_currentFrame - 1) % blockWidth; 
				int _offsetY = (_currentFrame - 1) / blockWidth;
				
				float directionSwitch = 1f;
				
				// Wenn Objekt nach links bewegt, spiegel die Textur mit negativem X Scale
				if (gameObject.GetComponent<Moving>().viewDirection == DirectionEnum.RIGHT) {
					directionSwitch = -1f;
					_offsetX = (_currentFrame) % blockWidth;
				}
				
				// Setze Offsets der Textur
				blockMaterial.mainTextureOffset = new Vector2 (_offsetX / (float) blockWidth, 1f - ((_offsetY + 1) / (float) blockHeight)); 
				// Ändere Skallierung (Spiegelung) der Textur 
				blockMaterial.mainTextureScale = new Vector2 ( 1f / (float) blockWidth * directionSwitch, 1f / (float) blockHeight); 
			}

			// next steps! 
			_currentFrame++; 
			if (_currentFrame > _totalFrames) {
				// Loop
				if (loop) {
					_currentFrame = 1; // Fange wieder bei Frame 1 an
				// Einmaliges Abspielen
				} else {
					_currentFrame = _totalFrames; // Stop an letzteer Frame
					animationComplete = true;
					BroadcastMessage("stopAnimation", name); // melde Stop der Animation
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
	
	/// 
	/// Gibt Dauer der Anmation zurück
	/// 
	public float getDuration () {
		return _totalFrames * AnimationDeltaTime;
	}
	
	/// 
	/// Setzt blockMaterial auf Material des Objekts
	/// 
	void Awake () {
		blockMaterial = renderer.material;
	}
	
}
