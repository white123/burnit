using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.Networking;

public class myPlayerController: NetworkBehaviour 
{
	private FirstPersonController fpsController;
	private Transform playerCameraTransform;
	private Camera playerCamera;
	private AudioListener playerAudioListener;

    private int lighterOrExtinguisher; // 0 for lighter, 1 for Extinguisher
    
	void Start()
	{

	    if (isLocalPlayer)
        {
            if (transform.Find("MyLighter").gameObject.activeSelf)
                lighterOrExtinguisher = 0;
            else if (transform.Find("MyExtinguisher").gameObject.activeSelf)
                lighterOrExtinguisher = 1;
            else
                Debug.LogError("set active lighter or extinguisher only one");
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
            CmdSkillOn();
        }
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            CmdSkillOff();
        }
        
        if (Input.GetKeyUp(KeyCode.R))
        {
            CmdChangeCharacter(this.transform.gameObject);
        }

    }

    public bool iisLocalPlayer()
    {
        if (isLocalPlayer) return true;
        return false;
    }


    [Command]
    private void CmdSkillOn()
    {
        this.transform.gameObject.GetComponent<myPlayerController>().RpcSkillOn();
    }
    [Command]
    private void CmdSkillOff()
    {
        this.transform.gameObject.GetComponent<myPlayerController>().RpcSkillOff();
    }
    [Command]
    private void CmdChangeCharacter(GameObject character)
    {
        character.GetComponent<myPlayerController>().RpcChangeCharacter();
    }



    [ClientRpc]
    public void RpcChangeCharacter()
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
            Debug.LogError("error for change;");
        }
        
        
    }

    [ClientRpc]
    public void RpcSkillOn()
    {
        
        if (lighterOrExtinguisher == 0)
        {
            GameObject character = transform.Find("MyLighter").gameObject;
            character.transform.Find("LighterFlame").gameObject.SetActive(true);
            character.GetComponent<BoxCollider>().enabled = true;


        }
        else if (lighterOrExtinguisher == 1)
        {
            GameObject character = transform.Find("MyExtinguisher").gameObject;
            character.transform.Find("TyreBurnoutSmoke").gameObject.SetActive(true);
            character.GetComponent<BoxCollider>().enabled = true;

        }
        else
        {
            Debug.LogError("error for skill;");
        }
        
        
    }

    [ClientRpc]
    public void RpcSkillOff()
    {
      
        if (lighterOrExtinguisher == 0)
        {
            GameObject character = transform.Find("MyLighter").gameObject;
            character.transform.Find("LighterFlame").gameObject.SetActive(false);
            character.GetComponent<BoxCollider>().enabled = false;

        }
        else if (lighterOrExtinguisher == 1)
        {
            GameObject character = transform.Find("MyExtinguisher").gameObject;
            character.transform.Find("TyreBurnoutSmoke").gameObject.SetActive(false);
            character.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            Debug.LogError("error for skill;");
        }
        
        
    }
}

