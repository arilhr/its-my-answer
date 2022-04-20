using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [Header("Player Properties")]
    public float moveSpeed = 1f;
    public float maxSpeed = 5f;
    public float jumpForce = 5f;
    public float punchedForce = 20f;
    public LayerMask groundLayer;
    private bool isStunned = false;
    private bool isMoved = true;

    private float turnSmoothValue;
    private float turnSmoothTime = 0.1f;

    [Header("Reference")]
    public PlayerAnswer playerAnswer;
    public Transform droppedItemPos;
    public TMP_Text currentAnswerPickedText;
    private Animator animator;
    private Rigidbody rb;
    private PhotonView pv;
    private Transform playerCamera;
    public PlayerInput input { private set; get; }

    [Header("Item Pick Properties")]
    public bool itemPickGizmos = true;
    public Vector3 itemPickArea;
    public Vector3 itemPickOffset;
    public float itemPickMaxDistance;
    public LayerMask itemPickLayer;

    public int currentAnswer { private set; get; }
    public AnswerItem currentItemPicked { private set; get; }

    [Header("Punch Cast")]
    public bool punchGizmos = true;
    public Vector3 punchArea;
    public Vector3 punchOffset;
    public float punchMaxDistance;
    public LayerMask punchLayerMask;
    private float timeBetweenPunch = 1f;
    private bool isPunching = false;

    [Header("Debug")]
    public bool showPickAnswerArea = false;
    public bool showPunchArea = false;
    public GameObject playerModelTest;


    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        input = new PlayerInput();

        // set default 
        Hashtable currentProps = new Hashtable();
        currentProps["CurrentAnswer"] = -1;
        PhotonNetwork.LocalPlayer.SetCustomProperties(currentProps);
    }

    public override void OnEnable()
    {
        base.OnEnable();

        if (pv.IsMine)
        {
            input.GameControl.Jump.started += Jump;
            input.GameControl.Pick.started += PickItemInput;
            input.GameControl.Punch.started += PunchInput;
            input.GameControl.Enable();
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();

        if (pv.IsMine)
        {
            input.GameControl.Jump.started -= Jump;
            input.GameControl.Pick.started -= PickItemInput;
            input.GameControl.Punch.started -= PunchInput;
            input.GameControl.Disable();
        }
    }

    private void Start()
    {
        if (pv.IsMine)
        {
            CameraManager.Instance.SetObjectFollow(transform);
            playerCamera = CameraManager.Instance.mainCamera.transform;

            // spawn player model skin
            GameObject playerModel = PhotonNetwork.Instantiate("Player Models/" + InventoryManager.Instance.GetUsedSkin().modelInGame.name, transform.position, Quaternion.identity);
            animator = playerModel.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        UpdateAnswerUI();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine) return;

        Move();
    }

    private void Move()
    {
        if (isStunned) return;

        Vector2 moveInput = input.GameControl.Move.ReadValue<Vector2>();
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);

        if (direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothValue, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            rb.AddForce(moveDir * moveSpeed, ForceMode.Impulse);
        }

        CheckMaxSpeed();

        if (rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;
    }

    private void CheckMaxSpeed()
    {
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (isStunned) return;

        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void PunchInput(InputAction.CallbackContext ctx)
    {
        if (isStunned) return;

        Punch();
    }

    private void Punch()
    {
        if (isPunching) return;

        // animation
        animator.SetTrigger("Punch");

        Vector3 centerPos = transform.position + transform.right * punchOffset.x + transform.up * punchOffset.y + transform.forward * punchOffset.z;
        RaycastHit hit;
        if (Physics.BoxCast(centerPos, punchArea / 2, transform.forward, out hit, transform.rotation, punchMaxDistance, punchLayerMask))
        {
            Debug.Log($"Collided {hit.collider.gameObject.name}");

            PlayerController player = hit.collider.gameObject.GetComponent<PlayerController>();

            if (player == this) return;
            if (player == null) return;

            player.photonView.RPC("RpcPunch", player.photonView.Owner, transform.forward);
        }

        StartCoroutine(PunchCooldown());
    }

    private IEnumerator PunchCooldown()
    {
        isPunching = true;
        yield return new WaitForSeconds(timeBetweenPunch);
        isPunching = false;
    }

    [PunRPC]
    private void RpcPunch(Vector3 forceDir)
    {
        Punched(forceDir * punchedForce);
    }

    public void Punched(Vector3 force)
    {
        Debug.Log($"Punched");

        DropItem();

        rb.AddForce(force * 10);

        StartCoroutine(GetStun());
    }

    private IEnumerator GetStun()
    {
        isStunned = true;
        
        yield return new WaitForSeconds(1f);

        isStunned = false;
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        return (Physics.Raycast(ray, out RaycastHit hit, 0.3f));
    }

    #region Pick and Drop Item
    private void PickItemInput(InputAction.CallbackContext ctx)
    {
        if (currentItemPicked == null)
        {
            PickItem();
        }
        else
        {
            DropItem();
        }
    }

    public void PickItem()
    {
        RaycastHit hit;
        Vector3 centerPos = transform.position + transform.right * itemPickOffset.x + transform.up * itemPickOffset.y + transform.forward * itemPickOffset.z;
        if (Physics.BoxCast(centerPos, itemPickArea / 2, transform.forward, out hit, transform.rotation, itemPickMaxDistance, itemPickLayer))
        {
            if (hit.collider.GetComponent<AnswerItem>() == null) return;
            if (hit.collider.GetComponent<AnswerItem>().isPicked) return;

            Debug.Log($"Pick item: {hit.collider.gameObject.name}");

            currentItemPicked = hit.collider.GetComponent<AnswerItem>();
            currentItemPicked.Picked(this);

            int pickedAnswer = hit.collider.GetComponent<AnswerItem>().answer;

            // set player answer object
            pv.RPC("RpcPickAnswer", RpcTarget.All, pickedAnswer);

            Hashtable currentProps = new Hashtable();
            currentProps["CurrentAnswer"] = pickedAnswer;
            PhotonNetwork.LocalPlayer.SetCustomProperties(currentProps);

            // animation
            animator.SetBool("isPickBox", true);
        }
    }

    [PunRPC]
    public void RpcPickAnswer(int answer)
    {
        playerAnswer.SetAnswerPicked(answer);
    }

    public void DropItem()
    {
        if (currentItemPicked == null) return;

        Debug.Log($"Dropped item!");

        currentItemPicked.Dropped();
        currentItemPicked = null;

        // set player answer dropped
        pv.RPC("RpcDropAnswer", RpcTarget.All);

        Hashtable currentProps = new Hashtable();
        currentProps["CurrentAnswer"] = -1;
        PhotonNetwork.LocalPlayer.SetCustomProperties(currentProps);

        // animation
        animator.SetBool("isPickBox", false);
    }

    [PunRPC]
    public void RpcDropAnswer()
    {
        playerAnswer.SetAnswerDropped();
    }

    #endregion

    #region Callback
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (targetPlayer != null && targetPlayer == pv.Owner)
        {
            Debug.Log($"{targetPlayer.NickName} props changed");
        
            if (targetPlayer.CustomProperties.ContainsKey("CurrentAnswer"))
            {
                currentAnswer = (int)targetPlayer.CustomProperties["CurrentAnswer"];
                Debug.Log($"{targetPlayer.NickName} current answer changed to {currentAnswer}");
            }
        }
    }

    #endregion

    #region UI
    private void UpdateAnswerUI()
    {
        if (currentAnswer == -1)
            currentAnswerPickedText.text = string.Empty;
        else
            currentAnswerPickedText.text = $"{currentAnswer}";
    }
    #endregion

    #region Animation
    private void UpdateAnimation() 
    {
        if (!pv.IsMine) return;
        if (pv.CreatorActorNr != pv.OwnerActorNr) return;

        // run state
        Vector2 moveInput = input.GameControl.Move.ReadValue<Vector2>();
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);
        if (direction.magnitude > 0.1f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    public void WinAnimate()
    {
        if (!pv.IsMine) return;

        animator.SetBool("isWin", true);
    }
    #endregion


    #region Static Function
    public static PlayerController GetLocalPlayer()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in playerObjects)
        {
            PlayerController player = p.GetComponent<PlayerController>();

            if (player != null)
            {
                if (player.photonView.Owner == PhotonNetwork.LocalPlayer)
                {
                    return player;
                }
            }
        }

        return null;
    }

    public static GameObject GetPlayerObject(Player owner)
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in playerObjects)
        {
            PlayerController player = p.GetComponent<PlayerController>();

            if (player != null)
            {
                if (player.photonView.Owner == owner)
                {
                    return p;
                }
            }
        }

        return null;
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (showPunchArea)
        {
            Gizmos.color = Color.yellow;
            Vector3 centerPos = transform.position + transform.right * punchOffset.x + transform.up * punchOffset.y + transform.forward * punchOffset.z;
            Gizmos.DrawWireCube(centerPos + transform.forward * punchMaxDistance, punchArea);
        }

        if (punchGizmos)
        {
            RaycastHit hit;
            Vector3 centerPos = transform.position + transform.right * punchOffset.x + transform.up * punchOffset.y + transform.forward * punchOffset.z;
            bool isHit = Physics.BoxCast(centerPos, punchArea / 2, transform.forward, out hit, transform.rotation, punchMaxDistance, punchLayerMask);
            
            if (isHit)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(centerPos, transform.forward * hit.distance);
            }
        }

        if (showPickAnswerArea)
        {
            Gizmos.color = Color.red;
            Vector3 centerPos = transform.position + transform.right * itemPickOffset.x + transform.up * itemPickOffset.y + transform.forward * itemPickOffset.z;
            Gizmos.DrawWireCube(centerPos + transform.forward * itemPickMaxDistance, itemPickArea);
        }

        if (itemPickGizmos)
        {
            RaycastHit hit;
            Vector3 centerPos = transform.position + transform.right * itemPickOffset.x + transform.up * itemPickOffset.y + transform.forward * itemPickOffset.z;
            bool isHit = (Physics.BoxCast(centerPos, itemPickArea / 2, transform.forward, out hit, transform.rotation, itemPickMaxDistance, itemPickLayer));

            if (isHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(centerPos, transform.forward * hit.distance);
            }
        }
    }
}