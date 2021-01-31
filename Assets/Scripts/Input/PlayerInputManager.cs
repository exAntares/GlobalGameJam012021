using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{ 
    public void setMousePos( Vector3 value ) {
        mousePos = value;
    }
    public GameObject playerShipGameObject;
    public Grid gameGrid;
    public GameObject ClickCatcher;
    private GameObject ghostShipContainer;
    private PlayerInputType currentInputType;
    // Start is called before the first frame update

    private Vector3 mousePos;
    public void Init() {
        currentInputType = PlayerInputType.CONTROL_PENDING;
        ghostShipContainer = new GameObject();
        ghostShipContainer.transform.parent = gameGrid.transform;
        ClickCatcher.SetActive(false);
    }

    public void Reset() {
        destroy();
        Init();
    }
    void Start()
    {
        Init();
    }

    private void destroy() {
        if(ghostShipContainer != null) {
            if(ghostShipContainer.transform.parent != null) {
                ghostShipContainer.transform.parent = null;
            }
        }
    }

    // Update is called once per frame
    private void Update() {
        if(currentInputType == PlayerInputType.CONTROL_DISABLED) {
            return;
        }

        if(currentInputType == PlayerInputType.CONTROL_PENDING) {
            checkPlayerRequestingControl();
        } else {
            checkPlayerRequestingReleaseControl();
        }

        updateGhostPreviews();
    }

    public void LockPlayerControl() {
        currentInputType = PlayerInputType.CONTROL_DISABLED;
    }

    public void UnlockPlayerControl() {
        currentInputType = PlayerInputType.CONTROL_PENDING;
    }   

    private void checkPlayerRequestingControl() {
        if (Input.GetKey(KeyCode.M)) {
            startControlMoveRaft();
        }
    }

    private void checkPlayerRequestingReleaseControl() {
        if(Input.GetKey(KeyCode.Escape) || Input.GetMouseButtonDown(1)) {
            releasePlayerControl();
        }
    }

    private void releasePlayerControl() {
        if(currentInputType == PlayerInputType.RAFT_MOVEMENT)
        {
            cancelMoveRaft();
        }
        
        currentInputType = PlayerInputType.CONTROL_PENDING;
    }


    private void startControlMoveRaft() {
        ClickCatcher.SetActive(true);
        currentInputType = PlayerInputType.RAFT_MOVEMENT;
        createGhostBoat();
    }

    private void createGhostBoat() {
        ghostShipContainer = new GameObject();
        SpriteRenderer[] sprites = playerShipGameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer spr in sprites) {
            SpriteRenderer newSpr = GameObject.Instantiate(spr);
            newSpr.color = new Color(1.0f,0f,0f,0.5f);
            newSpr.transform.parent = ghostShipContainer.transform;
            newSpr.transform.Translate(spr.transform.parent.position);
        }
        
        ghostShipContainer.transform.parent = gameGrid.transform;
    }

    private void cancelMoveRaft() {
        Destroy(ghostShipContainer);
        ClickCatcher.SetActive(false);
    }

    private void updateGhostPreviews() {
        if(currentInputType == PlayerInputType.RAFT_MOVEMENT) {
            Vector3 gridPos = new Vector3(Mathf.Round(mousePos.x),Mathf.Round(mousePos.y),Mathf.Round(mousePos.z));
            ghostShipContainer.transform.position = gridPos;
        }
    }
}

public enum PlayerInputType
{
    CONTROL_DISABLED,
    CONTROL_PENDING,
    RAFT_MOVEMENT,
    AIM_CANNON,
    ATTACH_STRUCTURE,
    DETACH_STRUCTURE,
    MOVE_COMPONENT
}