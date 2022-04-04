using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform playerCamera;
    public float speed = 6f;
    public Transform pickedItemPos;

    private CharacterController controller;
    private float horizontalDirection = 0f;
    private float verticalDirection = 0f;

    private float turnSmoothValue;
    private float turnSmoothTime = 0.1f;

    private IPickable itemPicked = null;
    private float pickRayLength = 2f;

    private PhotonView view;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (view.IsMine)
        {
            CameraManager.Instance.SetObjectFollow(transform);
            playerCamera = CameraManager.Instance.mainCamera.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!view.IsMine) return;

        InputPlayer();
        Move();
    }

    private void InputPlayer()
    {
        horizontalDirection = Input.GetAxisRaw("Horizontal");
        verticalDirection = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            PickItem();
        }

        if (Input.GetMouseButtonDown(1))
        {
            DropItem();
        }
    }

    private void Move()
    {
        Vector3 direction = new Vector3(horizontalDirection, 0f, verticalDirection);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothValue, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    private void PickItem()
    {
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, Vector3.one * pickRayLength / 2f, transform.forward, out hit, Quaternion.identity, pickRayLength))
        {
            Debug.Log($"Pick item: {hit.collider.gameObject.name}");

            hit.collider.GetComponent<IPickable>()?.Picked(this);
            itemPicked = hit.collider.GetComponent<IPickable>();
        }
    }

    private void DropItem()
    {
        if (itemPicked == null) return;

        itemPicked.Dropped();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
}
