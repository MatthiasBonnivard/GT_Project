using UnityEngine;
using System.Collections;

public class NewCube : MonoBehaviour {
	int[,] gridArray = new int[8, 8];
	float[,] cubeArray = new float[20, 3];
	public int total = 4;
	public int winZero = 6;
	public bool playAgain = false;
	public bool betweenLevel = false;
	int xAr = 0;
	int yAr = 1;
	int zAr = 2;
	bool cubePutOk = false;
	GameObject[] cubeGObjArray;
	GameObject[] cubeGObjArrayPlayer = new GameObject[100];
	public int nbOfCubeCreated = 0;
	public int sceneNumber = 0;
	public int time = 0;
	public int[] timeLevelsArray = new int[10];
	public AudioClip Xylo_13;
	private AudioSource source;
	//var aSources = GetComponents(AudioSource);
	//newCubeSound = aSources[1];

	// Use this for initialization
	void Start () {
		for ( int i = 0; i < 8;i++ ) {
			for ( int j = 0; j < 8;j++ ) {
				gridArray[i, j] = 0;
			}
		}
		loadScene(sceneNumber, true);
		time = 1000;
		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		float x,y,z = 0;

        printTime();

        initPosition();
        //TODO why default position + 4
		int val = gridArray [(int)transform.position.x + 4, (int)transform.position.z + 4];
		x = transform.position.x;
		y = (float) (val + 0.5);
		z = transform.position.z;
		transform.position = new Vector3 (x, y, z);

		if (Input.GetKeyDown ("space")) {
            y = generateNewCube(y);
		}

		for (int i = 0; i < 20; i++) {
			if (x == (cubeArray [i, 0]) && (y-0.5) == (cubeArray [i, 1]) && z == cubeArray [i, 2]) {
				GetComponent<Renderer>().material.color = new Color (1, 255, 1);
				if(cubePutOk == true){
					winZero--;
					//time = time  - 100;
					cubePutOk = false;
				}
				break;
			}
			else{
				GetComponent<Renderer>().material.color = new Color (255, 1, 1);
			}
		}

		string levelTimeString = "LEVEL: ";

		for (int i = 0; i < sceneNumber + 1; i++) {
			levelTimeString += "\n figure " + i + " : " + (timeLevelsArray[i]/100).ToString();
		}

		GameObject.Find("TextGrid").GetComponent<TextMesh>().text = "REMAINING CUBE : " + (this.winZero-2).ToString() + "\n\n" 
			+ "LEVEL " + this.sceneNumber.ToString() + " / 7 \n";
		GameObject.Find("TextLevelDuration").GetComponent<TextMesh> ().text = levelTimeString;
			//"\n nbOfCubeCreated= " + nbOfCubeCreated.ToString();

		GameObject.Find("TextCommand").GetComponent<TextMesh> ().text = "Commands : \n - B : change view \n - SPACE : create cube \n - DIRECTIONAL ARROWS : move";

		if (playAgain) {
			System.Threading.Thread.Sleep(3000);
			playAgain = false;
			GameObject.Find("FinishText").GetComponent<TextMesh> ().text = "";
		}

		GameObject.Find("TextLevelDIsplay").GetComponent<TextMesh> ().text = "LEVEL # \n   " + this.sceneNumber.ToString();

		if (sceneNumber == 8){

			int total = 0;
			for (int i = 0; i < sceneNumber + 1; i++) {
				total += timeLevelsArray[i];
			}
			playAgain = true;
			GameObject.Find("TextLevelDIsplay").GetComponent<TextMesh> ().text = "SCORE =  " + (total/100).ToString();
			GameObject.Find("FinishText").GetComponent<TextMesh> ().text = "ALL LEVELS FINISHED";
			sceneNumber = 0;
		}

		if (winZero == 2) {
			winZero = 6;
			timeLevelsArray[sceneNumber] = time;
			sceneNumber++;
			time = 0;
//			renderer.material.color = new Color (1, 255, 1);
			for (int i = 0; i < 8; i++) {
				Destroy(cubeGObjArray[i]);
			}
			for (int i = 0; i < nbOfCubeCreated; i++) {
				Destroy(cubeGObjArrayPlayer[i]);
			}
			nbOfCubeCreated = 0;
			for ( int i = 0; i < 8;i++ ) {
				for ( int j = 0; j < 8;j++ ) {
					gridArray[i, j] = 0;
				}
			}
			loadScene(sceneNumber, false);
//			betweenLevel = true;
		}
//		if (betweenLevel == true) {
//			newLevelLighting();
//		}
	}

    void initPosition()
    {

    }

    void printTime()
    {
        time = time + 2;
        GameObject.Find("TextChrono").GetComponent<TextMesh>().text = (this.time / 100).ToString();
    }

    float generateNewCube(float y)
    {
        cubeGObjArrayPlayer[nbOfCubeCreated] = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubeGObjArrayPlayer[nbOfCubeCreated].transform.position = new Vector3(transform.position.x, 
            transform.position.y, transform.position.z);
        nbOfCubeCreated++;
        gridArray[(int)transform.position.x + 4, (int)transform.position.z + 4] += 1;
        y = transform.position.y + 1;
        cubePutOk = true;
        //audio.play()
        //source.PlayOneShot(Xylo_13,1000000);
        return y;
    }

//	void newLevelLighting(){
//		System.Threading.Thread.Sleep(2000);
////		GameObject.Find("Directional light").SetActive(true);
////		GameObject.Find("Directional lightBotoom").SetActive(true);
//		GameObject.Find("SpotlightNewLevel").SetActive(false);
//		betweenLevel = false;
//	}

	void loadScene(int sceneNumber, bool start){
		float xBase = (float)-15;
		float yBase = (float)0.5;
		float zBase = (float)0;

		for ( int i = 0; i < 20;i++ ) {
			for ( int j = 0; j < 3;j++ ) {
				// the i = the cube number and the
				// j is the axis number (0==x 1==y 2==z) 
				cubeArray[i, j] = 0;
			}
		}

		int numberOfObj = 8;

		//Cube of the shape to reproduce
		if (sceneNumber == 1) {
			cubeArray [(int)1, xAr] = (float)-1;
			cubeArray [(int)2, xAr] = (float)-2;
			cubeArray [(int)3, xAr] = (float)-3;
			cubeArray [(int)4, xAr] = (float)1;
			cubeArray [(int)5, xAr] = (float)2;
		} 
		else if (sceneNumber == 2) {
			cubeArray [(int)1, zAr] = (float)-1;
			cubeArray [(int)2, zAr] = (float)-2;
			cubeArray [(int)3, zAr] = (float)-3;
			cubeArray [(int)4, zAr] = (float)1;
			cubeArray [(int)5, zAr] = (float)2;
		}
		else if (sceneNumber == 3) {
			cubeArray [(int)1, xAr] = (float)1;
			cubeArray [(int)2, xAr] = (float)2;
			cubeArray [(int)3, zAr] = (float)1;
			cubeArray [(int)4, zAr] = (float)-1;
			cubeArray [(int)5, zAr] = (float)-2;
		}
		else if (sceneNumber == 4) {
			cubeArray [(int)1, zAr] = (float)1;
			cubeArray [(int)2, zAr] = (float)2;
			cubeArray [(int)3, xAr] = (float)1;
			cubeArray [(int)4, xAr] = (float)2;
			cubeArray [(int)5, xAr] = (float)3;
		}
		else if (sceneNumber == 5) {
			cubeArray [(int)1, zAr] = (float)-1;
			cubeArray [(int)2, zAr] = (float)-2;
			cubeArray [(int)3, xAr] = (float)-1;
			cubeArray [(int)4, xAr] = (float)-2;
			cubeArray [(int)5, xAr] = (float)-3;
		}
		else if (sceneNumber == 6) {
			cubeArray [(int)5, xAr] = (float)-1;
			cubeArray [(int)2, xAr] = (float)-2;
			cubeArray [(int)3, xAr] = (float)-1;
			cubeArray [(int)3, yAr] = (float)1;
			cubeArray [(int)4, xAr] = (float)-2;
			cubeArray [(int)4, yAr] = (float)1;
			cubeArray [(int)1, xAr] = (float)-2;
			cubeArray [(int)1, yAr] = (float)2;
		}
		else if (sceneNumber == 7) {
			cubeArray [(int)5, xAr] = (float)-1;
			cubeArray [(int)5, zAr] = (float)1;
			cubeArray [(int)2, xAr] = (float)-2;
			cubeArray [(int)2, zAr] = (float)2;
			cubeArray [(int)3, xAr] = (float)-1;
			cubeArray [(int)3, zAr] = (float)1;
			cubeArray [(int)3, yAr] = (float)1;
			cubeArray [(int)4, xAr] = (float)-2;
			cubeArray [(int)4, zAr] = (float)2;
			cubeArray [(int)4, yAr] = (float)1;
			cubeArray [(int)1, xAr] = (float)-2;
			cubeArray [(int)1, yAr] = (float)2;
			cubeArray [(int)1, zAr] = (float)2;
		}
		else {
			cubeArray [(int)1, yAr] = (float)1;
			cubeArray [(int)2, xAr] = (float)1;
			cubeArray [(int)3, xAr] = (float)-1;
			cubeArray [(int)4, zAr] = (float)1;
			cubeArray [(int)5, zAr] = (float)-1;
		}

		cubeGObjArray = new GameObject[numberOfObj];

		for (int i = 0; i < numberOfObj - 2; i++) {
			cubeGObjArray[i] = GameObject.CreatePrimitive (PrimitiveType.Cube);
			cubeGObjArray[i].transform.position = new Vector3 ((float) xBase + cubeArray[i,xAr],(float) yBase + cubeArray[i,yAr], (float) zBase + cubeArray[i,zAr]);
		}

		//cube indice on both side colored in yellow
		cubeGObjArray[numberOfObj -3].GetComponent<Renderer>().material.color = new Color (255, 200, 1);
		cubeGObjArray[numberOfObj -2] = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cubeGObjArray[numberOfObj -2].transform.position = new Vector3 ((float)cubeArray[(numberOfObj - 3),xAr],(float) yBase + cubeArray[(numberOfObj - 3),yAr], (float) zBase + cubeArray[(numberOfObj - 3),zAr]);
		cubeGObjArray[numberOfObj -2].GetComponent<Renderer>().material.color = new Color (255, 200, 1);
		//add the indice cube to the grid
		gridArray[(int) cubeArray[(numberOfObj -3), xAr] + 4,(int) cubeArray[(numberOfObj -3), zAr] + 4] += 1;

//		if (start == false) {
//			betweenLevel = true;
////			GameObject.Find ("Directional light").SetActive (false);
////			GameObject.Find ("Directional lightBotoom").SetActive (false);
//			GameObject.Find ("SpotlightNewLevel").SetActive (true);
//		}
	}
}