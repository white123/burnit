using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.Networking;

public class myPlayerController: NetworkBehaviour 
{
	private FirstPersonController fpsController;
	private Transform playerCameraTransform;
	private Camera playerCamera;
	private AudioListener playerAudioListener;

    private int lighterOrExtinguisher = 1; // 0 for lighter, 1 for Extinguisher
    
	void Start()
	{

	    if (isLocalPlayer)
        {
            gameObject.name = "ME";
            
        }

        //當角色被產生出來時，如果不是Local Player就把所有的控制項目關閉，這些角色的位置資料將由Server來同步

        if (!isLocalPlayer) {
           
            disableCharacter(transform.Find("MyLighter").gameObject);
            disableCharacter(transform.Find("MyExtinguisher").gameObject);
            
        }

	}
    
    private void disableCharacter(GameObject character)
    {
        fpsController = character.GetComponent<FirstPersonController>();
        playerCameraTransform = character.transform.Find("FirstPersonCharacter");
        playerCamera = playerCameraTransform.GetComponent<Camera>();
        playerAudioListener = playerCameraTransform.GetComponent<AudioListener>();

        if (fpsController)
        {
            fpsController.enabled = false;
        }
        if (playerCamera)
        {
            playerCamera.enabled = false;
        }
        if (playerAudioListener)
        {
            playerAudioListener.enabled = false;
        }
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            SkillOn();
        }
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            SkillOff();
        }
        
        if (Input.GetKeyUp(KeyCode.R))
        {
            ChangeCharacter();
        }

    }

    private void ChangeCharacter()
    {
        if (lighterOrExtinguisher == 0)
        {
            transform.Find("MyLighter").gameObject.SetActive(false);
            transform.Find("MyExtinguisher").gameObject.SetActive(true);
            lighterOrExtinguisher = 1;
        }
        else if (lighterOrExtinguisher == 1)
        {
            transform.Find("MyLighter").gameObject.SetActive(true);
            transform.Find("MyExtinguisher").gameObject.SetActive(false);
            lighterOrExtinguisher = 0;
        }
        else
        {
            Debug.Log("error for change;");
        }
    }

    private void SkillOn()
    {
        if (lighterOrExtinguisher == 0)
        {
            GameObject character = transform.Find("MyLighter").gameObject;
            character.transform.Find("LighterFlame").gameObject.SetActive(true);
            
           
        }
        else if (lighterOrExtinguisher == 1)
        {
            GameObject character = transform.Find("MyExtinguisher").gameObject;
            character.transform.Find("TyreBurnoutSmoke").gameObject.SetActive(true);
            
        }
        else
        {
            Debug.Log("error for skill;");
        }
    }

    private void SkillOff()
    {
        if (lighterOrExtinguisher == 0)
        {
            GameObject character = transform.Find("MyLighter").gameObject;
            character.transform.Find("LighterFlame").gameObject.SetActive(false);


        }
        else if (lighterOrExtinguisher == 1)
        {
            GameObject character = transform.Find("MyExtinguisher").gameObject;
            character.transform.Find("TyreBurnoutSmoke").gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("error for skill;");
        }
    }
}

