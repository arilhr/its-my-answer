using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float maxSpeed = 5f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;

    public Transform itemPickedPos;
    
    private float turnSmoothValue;
    private float turnSmoothTime = 0.1f;

    private IPickable itemPicked = null;
    private float pickRayLength = 2f;

    private Transform playerCamera;
    private PlayerInput input;
    private Rigidbody rb;
    private PhotonView view;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
        input = new PlayerInput();
    }

    private void OnEnable()
    {
        if (view.IsMine)
        {
            input.GameControl.Jump.started += Jump;
            input.GameControl.Pick.started += PickItem;
            input.GameControl.Drop.started += DropItem;
            input.GameControl.Enable();
        }
    }

    private void OnDisable()
    {
        if (view.IsMine)
        {
            input.GameControl.Jump.started -= Jump;
            input.GameControl.Disable();
        }
    }

    private void Start()
    {
        if (view.IsMine)
        {
            CameraManager.Instance.SetObjectFollow(transform);
            playerCamera = CameraManager.Instance.mainCamera.transform;
        }
    }

    private void FixedUpdate()
    {
        if (!view.IsMine) return;

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

    private void PickItem(InputAction.CallbackContext ctx)
    {
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, Vector3.one * pickRayLength / 2f, transform.forward, out hit, Quaternion.identity, pickRayLength))
        {
            Debug.Log($"Pick item: {hit.collider.gameObject.name}");

            hit.collider.GetComponent<IPickable>()?.Picked(this);
            itemPicked = hit.collider.GetComponent<IPickable>();
        }
    }

    private void DropItem(InputAction.CallbackContext ctx)
    {
        if (itemPicked == null) return;

        itemPicked.Dropped();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
}
