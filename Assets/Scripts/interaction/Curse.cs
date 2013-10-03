using UnityEngine;
using System.Collections;

public class Curse : MonoBehaviour
{

	public float cursedScareFactor;  //Maximaler Erschreckwert des Fluchs
	public float cursedAttentionFactor; // Maximale Aufmerksamkeit des Fluchs
	public Font font;  //Font der für die Anzeige benutzt wird
	bool isCursing;    //Verflucht der Spieler gerade?
	float completion;  // Wie weit ist der Flcuh vorangeschritten
	GameObject player;  //Gamobjekt des Spielers
	Moving movComp;		//Die Moving Component
	public AudioClip[] curseAudio = new AudioClip[3];  //Audioclips für die Animationen
	public AudioClip curseSound;  //Audioclip der beim verfluchen spielt
	// Use this for initialization
	void Start ()
	{
		isCursing = false;
		completion = 0.0f;
		player = GameObject.FindGameObjectWithTag("Player");
		movComp = GameObject.FindGameObjectWithTag("Player").GetComponent<Moving>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(isCursing){  //Wenn der Spieler das Objekt verflucht...
			player.GetComponent<Player>().showPlayer();
			//wird der Fluch abgebrochen, sobald er sich bewegt und die Erschreck Werte berechnet
			if(movComp.isMoving()){ 
				isCursing = false;
				Item item = this.gameObject.GetComponent<Item>();
				item.scareFactor = cursedScareFactor*completion/100.0f;
				item.attentionFactor = cursedAttentionFactor*completion/100.0f;
				item.curse(true);
				//Je nachdem wie weit der Fluch kam, wird eine Animation abgespielt
				Animation anim = this.GetComponent<Animation>();
				if(completion<40.0f){
					anim.Play("Curse1");
					if(curseAudio[0]!=null){
						AudioSource audio = GetComponent<AudioSource>();
						audio.clip = curseAudio[0];
						audio.Play();
					}
				}
				if(completion>=40.0f && completion <100.0f){
					anim.Play("Curse2");
					if(curseAudio[1]!=null){
						AudioSource audio = GetComponent<AudioSource>();
						audio.clip = curseAudio[1];
						audio.Play();
					}
				}
				player.GetComponent<Player>().hidePlayer();
				completion = 0.0f;
				return;
			}
			//Das gleiche passiert wenn der Gegenstand zu 100% verflucht wurde, mit der letzen Animation
			completion+=Time.deltaTime*7.0f;
			if(completion>=100.0f){
				Item item = this.gameObject.GetComponent<Item>();
				item.scareFactor = cursedScareFactor;
				item.attentionFactor = cursedAttentionFactor;
				item.curse(true);
				Animation anim = this.GetComponent<Animation>();
				anim.Play("Curse3");
				if(curseAudio[2]!=null){
						AudioSource audio = GetComponent<AudioSource>();
						audio.clip = curseAudio[2];
						audio.Play();
					}
				isCursing = false;
				completion = 0.0f;
				player.GetComponent<Player>().hidePlayer();
			}
		}
	}
	
	void curseObject()
	{
		movComp.goToObject(this.gameObject,curse);
	}
	
	void curse(){
		//Leitet den Fluch ein und spielt passende Audio Datei ab
		player.BroadcastMessage("playAnimation", "curse");
		player.GetComponent<AudioSource>().clip = curseSound;
		player.GetComponent<AudioSource>().Play();
		isCursing = true;
		Interactable inter = this.gameObject.GetComponent<Interactable>();
		inter.enabled = false;
		
	}
	
	void OnGUI(){
		if(isCursing){
			//Grafische Textrepräsentation als Prozentzahl
			GUI.skin.font = font;
			GUI.color = Color.red;
			GUI.Label(new Rect(Camera.mainCamera.WorldToScreenPoint(player.transform.position).x,
				Camera.mainCamera.WorldToScreenPoint(player.transform.position).y+player.collider.bounds.size.y,
				200,
				200)
				,(int)completion+"%");
		}
	}
}

