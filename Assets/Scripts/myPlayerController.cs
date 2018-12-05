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
           
            fpsController = GetComponent<FirstPersonController>();
            playerCameraTransform = transform.Find("FirstPersonCharacter");
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

	}
    
    private void Update()
    {
        if (!isLocalPlayer) return;

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            CmdSkillOn(this.transform.gameObject);
        }
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            CmdSkillOff(this.transform.gameObject);
        }
        
        if (Input.GetKeyUp(KeyCode.R))
        {
            CmdChangeCharacter(this.transform.gameObject);
        }

    }


    [Command]
    private void CmdSkillOn(GameObject character)
    {
        character.GetComponent<myPlayerController>().RpcSkillOn();
    }
    [Command]
    private void CmdSkillOff(GameObject character)
    {
        character.GetComponent<myPlayerController>().RpcSkillOff();
    }
    [Command]
    private void CmdChangeCharacter(GameObject character)
    {
        character.GetComponent<myPlayerController>().RpcChangeCharacter();
    }



    [ClientRpc]
    public void RpcChangeCharacter()
    {
        if (isLocalPlayer)
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
        
    }

    [ClientRpc]
    public void RpcSkillOn()
    {
        if (isLocalPlayer)
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
        
    }

    [ClientRpc]
    public void RpcSkillOff()
    {
        if (isLocalPlayer)
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
}

