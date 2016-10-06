using UnityEngine;
using System.Collections;

public class CrafterControllerFREE : MonoBehaviour {

	private Animator animator;
	private GameObject box;
    private GameObject scaffolding;
    private GameObject boxFuturePosition;
    private Transform target;

	float rotationSpeed = 10;
	Vector3 inputVec;
	bool isMoving;
	bool isPaused;

	public enum CharacterState {Idle, Box}
    public enum ActualObject {None, Cube, Scaffolding}
	public CharacterState charState;
    public ActualObject actualObj;

	void Awake()
	{
		animator = this.GetComponent<Animator>();
		box = GameObject.Find("Carry");
        scaffolding = GameObject.Find("Scaffolding");
	}

	void Start()
	{
		StartCoroutine (COShowItem("none", 0f));
		charState = CharacterState.Idle;
        actualObj = ActualObject.None;

        GameObject.Find("ScaffoldingBoard").GetComponent<Renderer>().material.color = new Color(0.8f, 0.6f, 0.3f);
        GameObject.Find("ScaffoldingCube").GetComponent<Renderer>().material.color = new Color(0.8f, 0.55f, 0.2f);
	}

	void Update()
	{
		//Get input from controls
		float z = Input.GetAxisRaw("Horizontal");
		float x = -(Input.GetAxisRaw("Vertical"));
		inputVec = new Vector3(x, 0, z);
		animator.SetFloat("VelocityX", -x);
		animator.SetFloat("VelocityY", z);

        //display rotation
        target = GameObject.Find("Player").transform;

        //GameObject.Find("TextLevelDIsplay").GetComponent<TextMesh>().text =
          //  "Rotation Y  # \n   " + target.localRotation.y.ToString();

		if (x != 0 || z != 0 )  //if there is some input
		{
			//set that character is moving
			animator.SetBool("Moving", true);
			isMoving = true;

			//if we are running, set the animator
			if (Input.GetButton("Jump"))
				animator.SetBool("Running", true);
			else
				animator.SetBool("Running", false);
		}
		else
		{
			//character is not moving
			animator.SetBool("Moving", false);
			isMoving = false;
		}

		UpdateMovement();  //update character position and facing

        if (charState == CharacterState.Idle && !isMoving)
        {
            if (Input.GetKeyDown("space"))
            {
                //If the player is in the cube dispenser zone
                if (target.position.x < -9 && target.position.x > -10 &&
                    target.position.z > -3 && target.position.z < -1)
                {
                    receiveObject("box");
                    simulateDropPosition();
                }
                else if (target.position.x < -9 && target.position.x > -10 &&
                    target.position.z > 1 && target.position.z < 3) 
                //else if (target.position.x > (GameObject.Find("ScaffoldingDispenser").transform.position.x - 1) &&
                //    target.position.x < (GameObject.Find("ScaffoldingDispenser").transform.position.x + 1) &&
                //    target.position.z < (GameObject.Find("ScaffoldingDispenser").transform.position.z - 1) &&
                //    target.position.z > (GameObject.Find("ScaffoldingDispenser").transform.position.z + 1))
                {
                    receiveObject("scaffolding");
                    simulateDropPosition();
                }
            }
        }
        if (charState == CharacterState.Box)
        {
            if (!isMoving)
            {
                if (Input.GetKeyUp("space"))
                {
                    boxFuturePosition.SetActive(false);
                    generateNewObject();
                    putDownObject();
                }
                if (Input.GetKey("space"))
                {
                    boxFuturePosition.SetActive(true);
                    updateDropPosition();
                }
            }
            else
            {
                if (Input.GetKey("space"))
                    boxFuturePosition.SetActive(false);
            }
        }

		if(Input.GetKey(KeyCode.R))
			this.gameObject.transform.position = new Vector3(0,0,0);

		animator.SetFloat("Velocity", UpdateMovement());  //sent velocity to animator
	}

	void RotateTowardsMovementDir()  //face character along input direction
	{
		if (!isPaused)
		{
			if (inputVec != Vector3.zero)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVec), 
                    Time.deltaTime * rotationSpeed);
			}
		}
	}

	float UpdateMovement()  //movement of character
	{
		Vector3 motion = inputVec;  //get movement input from controls

		//reduce input for diagonal movement
		motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1)?.7f:1;
		
		if(!isPaused)
			RotateTowardsMovementDir();  //if not paused, face character along input direction

		return inputVec.magnitude;
	}

    void generateNewObject()
    {
        float dropObjectPositionX = (float)System.Math.Round((decimal)target.position.x, 0) + 0.5f;
        float dropObjectPositionZ = (float)System.Math.Round((decimal)target.position.z, 0) + 0.5f;
        if (actualObj == ActualObject.Cube)
        {
            GameObject newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newCube.transform.position =
                new Vector3(dropObjectPositionX, transform.position.y + 0.5f, dropObjectPositionZ);
            newCube.GetComponent<Renderer>().material.color = new Color(0.88f, 0.88f, 0.88f);
        }
        else if (actualObj == ActualObject.Scaffolding)
        {
            GameObject newScaffolding = (GameObject)Instantiate(GameObject.Find("Scaffolding"), 
                new Vector3(dropObjectPositionX, transform.position.y + 0.5f, dropObjectPositionZ), 
                Quaternion.identity);
            newScaffolding.transform.position -= new Vector3(-1, 0, -1);
        }
    }

    void simulateDropPosition()
    {
        //generate a square position for the future cube position
        boxFuturePosition = GameObject.CreatePrimitive(PrimitiveType.Cube);
        boxFuturePosition.transform.position = new Vector3(
            (float)System.Math.Round((decimal)target.position.x, 0),
            (float)System.Math.Round((decimal)target.position.y, 0),
            (float)System.Math.Round((decimal)target.position.z, 0));
        boxFuturePosition.transform.localScale -= new Vector3(0, 0.95F, 0);
        boxFuturePosition.GetComponent<Renderer>().material.color = new Color(1, 60, 1);
    }

    void updateDropPosition()
    {
        float futurePositionX = (float)System.Math.Round((decimal)target.position.x, 0) + 0.5f;
        float futurePositionZ = (float)System.Math.Round((decimal)target.position.z, 0) + 0.5f;
        boxFuturePosition.transform.position =
            new Vector3(futurePositionX, transform.position.y, futurePositionZ);
    }

    void putDownObject() {
        animator.SetTrigger("CarryPutdownTrigger");
        StartCoroutine(COMovePause(1.2f));
        StartCoroutine(COShowItem("none", .7f));
        charState = CharacterState.Idle;
        actualObj = ActualObject.None;
    }

    void receiveObject(string name)
    {
        animator.SetTrigger("CarryRecieveTrigger");
        StartCoroutine(COMovePause(1.2f));
        StartCoroutine(COShowItem(name, .5f));
        charState = CharacterState.Box;
        if (name == "box")
            actualObj = ActualObject.Cube;
        else if (name == "scaffolding")
            actualObj = ActualObject.Scaffolding;
    }

	void OnGUI () 
	{
        //TODO print the needed values
        //GUI.TextField (new Textfi)

		if (charState == CharacterState.Idle && !isMoving)
		{
			isPaused = false;

			if (GUI.Button (new Rect (25, 25, 150, 30), "Pickup Box")) 
			{
				animator.SetTrigger("CarryPickupTrigger");
				StartCoroutine (COMovePause(1.2f));
				StartCoroutine (COShowItem("box", .5f));
				charState = CharacterState.Box;
			}

			if (GUI.Button (new Rect (25, 65, 150, 30), "Recieve Box")) 
			{
				animator.SetTrigger("CarryRecieveTrigger");
				StartCoroutine (COMovePause(1.2f));
				StartCoroutine (COShowItem("box", .5f));
				charState = CharacterState.Box;
			}
		}

		if (charState == CharacterState.Box && !isMoving)
		{
			if (GUI.Button (new Rect (25, 25, 150, 30), "Put Down Box")) 
			{
				animator.SetTrigger("CarryPutdownTrigger");
				StartCoroutine (COMovePause(1.2f));
				StartCoroutine (COShowItem("none", .7f));
				charState = CharacterState.Idle;
			}
			
			if (GUI.Button (new Rect (25, 65, 150, 30), "Give Box")) 
			{
				animator.SetTrigger("CarryHandoffTrigger");
				StartCoroutine (COMovePause(1.2f));
				StartCoroutine (COShowItem("none", .6f));
				charState = CharacterState.Idle;
			}
		}
	}

	public IEnumerator COMovePause(float pauseTime)
	{
		isPaused = true;
		yield return new WaitForSeconds(pauseTime);
		isPaused = false;
	}

	public IEnumerator COChangeCharacterState(float waitTime, CharacterState state)
	{
		yield return new WaitForSeconds(waitTime);
		charState = state;
	}
	
	public IEnumerator COShowItem(string item, float waittime)
	{
		yield return new WaitForSeconds (waittime);
		
		if(item == "none")
		{
			box.SetActive(false);
            scaffolding.SetActive(false);
		}

		else if(item == "box")
		{
			box.SetActive(true);
		}

        else if (item == "scaffolding")
        {
            scaffolding.SetActive(true);
        }

		yield return null;
	}
}