using UnityEngine;
using System.Collections;

public class Curse : MonoBehaviour
{

	public float cursedScareFactor;
	public float cursedAttentionFactor;
	public Font font;
	bool isCursing;
	float completion;
	GameObject player;
	Moving movComp;
	public AudioClip[] curseAudio = new AudioClip[3];
	public AudioClip curseSound;
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
		if(isCursing){
			player.GetComponent<Player>().showPlayer();
			if(movComp.execMovement){
				isCursing = false;
				Item item = this.gameObject.GetComponent<Item>();
				item.scareFactor = cursedScareFactor*completion/100.0f;
				item.attentionFactor = cursedAttentionFactor*completion/100.0f;
				item.curse(true);
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
		BroadcastMessage("playAnimation", "curse");
		player.GetComponent<AudioSource>().clip = curseSound;
		player.GetComponent<AudioSource>().Play();
		isCursing = true;
		Interactable inter = this.gameObject.GetComponent<Interactable>();
		inter.enabled = false;
		
	}
	
	void OnGUI(){
		if(isCursing){
			GUI.skin.font = font;
			GUI.color = Color.red;
			GUI.Label(new Rect(Camera.mainCamera.WorldToScreenPoint(player.transform.position).x,
				Camera.mainCamera.WorldToScreenPoint(player.transform.position).y+player.collider.bounds.size.y,
				200,
				200)
				,(int)completion+"%");
			Debug.Log(Camera.mainCamera.WorldToScreenPoint(this.transform.position).x+" "+Camera.mainCamera.WorldToScreenPoint(this.transform.position).y);

		}
	}
}

