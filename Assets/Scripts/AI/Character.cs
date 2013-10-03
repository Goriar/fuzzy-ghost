using UnityEngine;
using System.Collections;
using System.Xml;

public class Character : MonoBehaviour
{
	public string name;
	public CharacterType cType;
	
	//Ghost Hunter Timer
	private float hunterTimer;
	private const float MAX_HUNTING_TIME = 60.0f;
	private bool trapActive;
	public GameObject trapPrefab;
	
	public StateMachine stateMachine{get;set;}
	Moving movingComponent;
	public RoomInventory currentLocation;
	public GameObject[] objectsOfInterest;
	public GameObject currentObjectOfInterest;
	public float currentValue;
	public float [] objectOfInterestValues;
	public GameObject[] characterPath;
	
	public bool enemyDetected;
	
	public float maxScareLevel = 100.0f;
	public float scareLevel;				// Aktuelles Erschreckfortschritt
	public float superstitionFactor;		// Aberglaube Faktor (von 0 bis 2)
	
	public bool readyToTalk;
	public bool talking;

 	string[] currentThingsToSay;
	public Character chatPartner;
	XmlDocument xml;
	
	public float dialogueTime;
	public AudioClip screamAudio;
	
	public Texture portrait;
	
	
	// Use this for initialization
	void Start ()
	{
		stateMachine = new StateMachine(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(),this);
		characterPath = new GameObject[0];
		movingComponent = this.gameObject.GetComponent<Moving>();
		
		if(objectsOfInterest.Length == 0)
			Debug.LogError("KEINE OBJEKTE ANGEGEBEN FÜR: "+this.ToString());
		
		currentValue = 0;
		currentObjectOfInterest = null;
		
		objectOfInterestValues = new float[objectsOfInterest.Length];
		for(int i = 0; i < objectOfInterestValues.Length; ++i)
		{
			objectOfInterestValues[i] = Random.Range(0,10);
		}
		
		scareLevel = 0.0f;
		// Setze Aberglaubefaktor auf Grenzen, falls unter-/überschritten
		superstitionFactor = (superstitionFactor > 2f) ? 2f : superstitionFactor;
		superstitionFactor = (superstitionFactor < 0f) ? 0f : superstitionFactor;
		
		readyToTalk = true;
		talking = false;
		dialogueTime = 0.0f;
		
		xml = new XmlDocument();
		xml.Load("./Assets/Scripts/AI/ThingsToSay.xml");
		XmlNode node;
		XmlNodeList nlist = xml.GetElementsByTagName(name);
		if(nlist.Count == 0){
			node = xml.GetElementsByTagName("TestCharacter")[0];
		} else {
			node = nlist[0];
		}
		
		currentThingsToSay = new string[node.ChildNodes.Count];
		for(int i = 0; i < currentThingsToSay.Length; ++i){
			currentThingsToSay[i] = node.ChildNodes[i].InnerText;
		}
		
		if(cType == CharacterType.GHOST_HUNTER){
			trapActive = false;
			hunterTimer = 0.0f;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		stateMachine.stateUpdate();
		updateObjectOfInterestList();
		if(cType == CharacterType.GHOST_HUNTER){
			hunterTimer += Time.deltaTime;
			if(hunterTimer>=MAX_HUNTING_TIME){
				stateMachine.changeState(StateType.FLEE_STATE);
				hunterTimer = 0.0f;
			}
		} else {
			if(scareLevel>=maxScareLevel && stateMachine.getState() != StateType.FLEE_STATE){
				stateMachine.changeState(StateType.FLEE_STATE);	
			}
			dialogueTime += Time.deltaTime;
			if(dialogueTime >20.0f){
				if(stateMachine.getState() != StateType.SCARED_STATE)
					readyToTalk = true;	
				else
					readyToTalk = false;
			}
		}
	}
	
	public bool isTrapActive(){
		return trapActive;
	}
	
	public void setTrapActive(bool val){
			trapActive = val;
	}
	
	public void scare (float scareAddition) {
		scareLevel += scareAddition*superstitionFactor;
	}
	
	public string getDialogue(){
		if(scareLevel < maxScareLevel*0.25){
			return currentThingsToSay[0];
		} 
		if(scareLevel < maxScareLevel*0.75){
			return currentThingsToSay[1];
		}
		return currentThingsToSay[2];
	}
	
	public RoomInventory getCurrentLocation()
	{
		return currentLocation;
	}
	
	public void setCurrentLocation (RoomInventory location) {
		currentLocation = location;
	}
	
	RoomInventory getMainFloorNeighbour()
	{
		RoomInventory[] neighbours = currentLocation.getNeighbouringRooms();
		foreach(RoomInventory ri in neighbours)
		{
			if(ri.isMainFloor)
				return ri;
		}
		
		return null;
	}
	
	
	bool roomContainsObject(RoomInventory room, GameObject obj)
	{
		foreach(GameObject g in room.objects)
		{
			if(g.Equals(obj))
				return true;
		}
		
		return false;
	}
	
	public GameObject[] getCharacterPath()
	{
		return characterPath;	
	}
	
	public GameObject popNextTarget()
	{
		GameObject obj = characterPath[0];
		GameObject[] newCharacterPath = new GameObject[characterPath.Length-1];
		for(int i = 0; i < newCharacterPath.Length; ++i)
		{
			newCharacterPath[i] = characterPath[i+1];	
		}
		characterPath = newCharacterPath;
		return obj;
	}
	
	protected void assignNextObjectOfInterest()
	{
		int changedIndex = 0;
		for(int i = 0; i < objectsOfInterest.Length; ++i)
		{
			if(currentValue < objectOfInterestValues[i])
			{
				currentValue = objectOfInterestValues[i];
				currentObjectOfInterest = objectsOfInterest[i];
				changedIndex = i;
			}
		}
		objectOfInterestValues[changedIndex] = 0;
	}
	
	protected void updateObjectOfInterestList()
	{
		for(int i = 0; i < objectOfInterestValues.Length; ++i)
		{
			objectOfInterestValues[i] += Time.deltaTime * Random.Range(0,10);
		}
	}
	
	public void resetCurrentValue()
	{
		currentValue = 0;
	}
	
	private void addToPath(GameObject obj)
	{
		GameObject[] newCharacterPath = new GameObject[characterPath.Length+1];
		for(int i = 0; i < characterPath.Length; ++i)
		{
			newCharacterPath[i] = characterPath[i];	
		}
		newCharacterPath[newCharacterPath.Length-1] = obj;
		characterPath = newCharacterPath;
	}
	
	public Moving getMovingComponent()
	{
		return movingComponent;
	}
	
	public void setCharacterPath(GameObject specificObject)
	{
		characterPath = new GameObject[0];
		// Wenn kein spezielles Objekt definiert wird, wird ein neues Objekt der Interesse hinzugefügt ...
		if(specificObject == null){
			assignNextObjectOfInterest();
		}
		// ... ansonsten wird spezifisch definiertes Objekt benutzt
		else{
			currentObjectOfInterest = specificObject;
		}
		
		// Wenn aktueller Raum das gesuchte Objekt beinhaltet, füge es zum Pfad hinzu ...
		if(currentLocation.containsObject(currentObjectOfInterest))
			addToPath(currentObjectOfInterest);
		// ... ansonsten gehe durch andere Räume
		else
		{
			RoomInventory mainFloor = null;
			GameObject door = null;
			// Wenn aktueller Raum nicht der Flur ist ...
			if(!currentLocation.isMainFloor){
				// ... setze Flur auf aktuellem Stockwerk und erhalte die Tür zum Flur.
				mainFloor = this.getMainFloorNeighbour(); // !!!
				door = currentLocation.getDoorToRoom(mainFloor);
				// Füge danach die Tür zum Pfad hinzu
				addToPath(door);
			}
			// Wenn aktueller Raum der Flur ist, füge diesen als Flur hinzu
			else{
				mainFloor = currentLocation;	
			}
			
			// Wenn Flur das gesuchte Objekt beinhaltet, füge es zum Pfad hinzu und beende Funktion
			if(mainFloor.containsObject(currentObjectOfInterest))
			{
				addToPath(currentObjectOfInterest);
				return;
			}
	
			RoomInventory nextRoom = null;
			// Durchgehe alle Räume des Flures auf dem aktuellen Stockwerk ...
			foreach(RoomInventory room in mainFloor.getNeighbouringRooms())
			{
				// ... und überprüfe, ob Objekt in Raum enthalten ist ...
				if(roomContainsObject(room,currentObjectOfInterest)) {
					// ... und setze nextRoom Variable mit diesem Raum und beende die Schleife
					nextRoom = room;
					break;
				}
			}
			
			// Wenn Objekt in keinem dieser Räume ist ...
			if(nextRoom == null){
				// ... benutze die Treppe in das andere Stockwerk
				addToPath(mainFloor.stairs);
				
				// Erhalte Stairs Komponente
				Stairs stairs = mainFloor.stairs.GetComponent<Stairs>();
				// Wenn aktueller Flur dem unteren Flur entspricht, setzte Flur um auf den oberen ...
				if(mainFloor.Equals(stairs.lowerMainFloor))
					mainFloor = stairs.upperMainFloor;
				// ... ansonsten auf den unteren
				else
					mainFloor = stairs.lowerMainFloor;
				
				// Wenn anderer Flur as Objekt der Interesse beinhaltet, füge zu Pfad hinzu und beende Funktion ...
				if(mainFloor.containsObject(currentObjectOfInterest))
				{
					addToPath(currentObjectOfInterest);
					return;
				}
				
				// ... ansonsten durchgehe alle Räume, die an Flur angehangen sind nach dem Objekt und setzte nächsten Raum
				foreach(RoomInventory room in mainFloor.getNeighbouringRooms())
				{
					if(roomContainsObject(room,currentObjectOfInterest))
						nextRoom = room;
				}
			}
			
			// Erhalte die TÜr zum nächsten Raum und füge sie zum Pfad hinzu
			door = mainFloor.getDoorToRoom(nextRoom);
			addToPath(door);
			// Anschließend füge das Objekt der Interesse zum Pfad hinzu
			addToPath(currentObjectOfInterest);
		}
	}
	
	
}

