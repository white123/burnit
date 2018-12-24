using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.Networking;

public class myPlayerController: NetworkBehaviour 
{
    public GameObject defeatedCanvas;

	private FirstPersonController fpsController;
	private Transform playerCameraTransform;
	private Camera playerCamera;
	private AudioListener playerAudioListener;

    [SerializeField] private AudioClip m_LighterSound;
    [SerializeField] private AudioClip m_ExtinguisherSound;
    [SerializeField] private AudioClip m_AttackSound;
    private AudioSource m_AudioSource;

    private bool isDead;

    private int lighterOrExtinguisher; // 0 for lighter, 1 for Extinguisher
    
	void Start()
	{
        isDead = false;
        
        if (isLocalPlayer)
        {
            if (transform.Find("MyLighter").gameObject.activeSelf)
                lighterOrExtinguisher = 0;
            else if (transform.Find("MyExtinguisher").gameObject.activeSelf)
                lighterOrExtinguisher = 1;
            else
                Debug.LogError("set active lighter or extinguisher only one");
            gameObject.name = "ME";
            m_AudioSource = this.transform.Find("SoundEffect").GetComponent<AudioSource>();
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
            if (gameObject.tag != "Dead")
            {
                if (this.tag == "Lighter") { m_AudioSource.clip = m_LighterSound; m_AudioSource.loop = false; }
                else { m_AudioSource.clip = m_ExtinguisherSound; m_AudioSource.loop = true; }
                m_AudioSource.Play();
            }
        }
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            CmdSkillOff();
            m_AudioSource.Stop();
        }
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (this.tag == "Lighter") return;
            CmdAttack();
            m_AudioSource.loop = false;
            m_AudioSource.clip = m_AttackSound;
            m_AudioSource.Play();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (this.tag == "Lighter") return;
            //m_AudioSource.Stop();
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            CmdChangeCharacter(this.transform.gameObject);
        }

    }

    public bool iisLocalPlayer()
    {
        if (isLocalPlayer) return true;
        return false;
    }
    public bool Dead()
    {
        return isDead;
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
    [Command]
    public void CmdKilled(GameObject killed)
    {
        this.transform.gameObject.GetComponent<myPlayerController>().RpcKilled(killed);
    }
    [Command]
    public void CmdAttack()
    {
        if (gameObject.tag == "Lighter") return;
        this.transform.gameObject.GetComponent<myPlayerController>().RpcAttack();
    }
    [Command]
    public void CmdChange()
    {
        this.transform.gameObject.GetComponent<myPlayerController>().RpcChangeCharacter();
    }
   


    [ClientRpc]
    public void RpcKilled(GameObject killed)
    {
        if (isDead) return;
        isDead = true;
        gameObject.tag = "Dead";
        if ("ME" != killed.name) return;
        Instantiate (defeatedCanvas, Vector2.zero, Quaternion.identity);
        fpsController = killed.GetComponent<FirstPersonController>();
        if (fpsController)
        {
            fpsController.enabled = false;
        }
        var myController = killed.GetComponent<myPlayerController>();
        if (myController)
        {
            myController.enabled = false;
        }
    }
    [ClientRpc]
    public void RpcChangeCharacter()
    {
        
        if (lighterOrExtinguisher == 0)
        {
            transform.Find("MyLighter").gameObject.SetActive(false);
            transform.Find("MyExtinguisher").gameObject.SetActive(true);
            gameObject.tag = "Extinguisher";
            lighterOrExtinguisher = 1;
        }
        else if (lighterOrExtinguisher == 1)
        {
            transform.Find("MyLighter").gameObject.SetActive(true);
            transform.Find("MyExtinguisher").gameObject.SetActive(false);
            gameObject.tag = "Lighter";
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
    [ClientRpc]
    public void RpcAttack()
    {   
        
        gameObject.GetComponent<Animator>().Play("axe");
    }
}

