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
    public float moveSpeed = 1f;
    public float maxSpeed = 5f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;

    [Header("Reference")]
    public Transform itemPickedPos;
    public TMP_Text currentAnswerPickedText;

    private float turnSmoothValue;
    private float turnSmoothTime = 0.1f;

    public int currentAnswer { private set; get; }
    public AnswerItem currentItemPicked { private set; get; }
    private float pickRayLength = 2f;

    private Transform playerCamera;
    public PlayerInput input { private set; get; }
    private Rigidbody rb;
    private PhotonView pv;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        input = new PlayerInput();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        if (pv.IsMine)
        {
            input.GameControl.Jump.started += Jump;
            input.GameControl.Pick.started += PickItem;
            input.GameControl.Drop.started += DropItem;
            input.GameControl.Enable();
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();

        if (pv.IsMine)
        {
            input.GameControl.Jump.started -= Jump;
            input.GameControl.Disable();
        }
    }

    private void Start()
    {
        if (pv.IsMine)
        {
            CameraManager.Instance.SetObjectFollow(transform);
            playerCamera = CameraManager.Instance.mainCamera.transform;
        }
    }

    private void Update()
    {
        UpdateAnswerUI();
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine) return;

        Move();
    }

    private void Move()
    {
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

        if (rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
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
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        return (Physics.Raycast(ray, out RaycastHit hit, 0.3f));
    }

    #region Pick and Drop Item
    private void PickItem(InputAction.CallbackContext ctx)
    {
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, Vector3.one * pickRayLength / 2f, transform.forward, out hit, Quaternion.identity, pickRayLength))
        {
            if (hit.collider.GetComponent<AnswerItem>().isPicked) return;

            Debug.Log($"Pick item: {hit.collider.gameObject.name}");

            currentItemPicked = hit.collider.GetComponent<AnswerItem>();
            currentItemPicked.Picked(this);

            int updatedAnswer = hit.collider.GetComponent<AnswerItem>().answer;

            Hashtable currentProps = new Hashtable();
            currentProps["CurrentAnswer"] = updatedAnswer;
            PhotonNetwork.LocalPlayer.SetCustomProperties(currentProps);
        }
    }

    private void DropItem(InputAction.CallbackContext ctx)
    {
        if (currentItemPicked == null) return;

        currentItemPicked.Dropped();

        Hashtable currentProps = new Hashtable();
        currentProps["CurrentAnswer"] = -1;
        PhotonNetwork.LocalPlayer.SetCustomProperties(currentProps);
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
            }
        }
    }

    #endregion


    private void UpdateAnswerUI()
    {
        if (currentAnswer == -1)
            currentAnswerPickedText.text = string.Empty;
        else
            currentAnswerPickedText.text = $"{currentAnswer}";
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
}
