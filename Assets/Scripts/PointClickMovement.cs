using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PointClickMovement : MonoBehaviour
{
	[SerializeField] private Transform target;

	public float moveSpeed = 6.0f;
	public float rotSpeed = 15.0f;
	public float jumpSpeed = 15.0f;
	public float gravity = -9.8f;
	public float terminalVelocity = -20.0f;
	public float minFall = -1.5f;
	public float pushForce = 3.0f;

	public float deceleration = 25.0f;
	public float targetBuffer = 1.5f;

	private float curSpeed = 0f;
	private Vector3 targetPos = Vector3.one;

	private float vertSpeed;
	private ControllerColliderHit contact;

	private CharacterController charController;
	private Animator animator;

	void Start()
	{
		vertSpeed = minFall;

		charController = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
	}

	void Update()
	{

		Vector3 movement = Vector3.zero;

		if (Input.GetMouseButton(0))
        {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit mouseHit;
			if (Physics.Raycast(ray, out mouseHit))
            {
				GameObject hitObject = mouseHit.transform.gameObject;
				if (hitObject.layer == LayerMask.NameToLayer("Ground"))
				{
					targetPos = mouseHit.point;
					curSpeed = moveSpeed;
                }
            }
        }

		if (targetPos != Vector3.one)
        {
			if (curSpeed > moveSpeed * .5f)
            {
				Vector3 adjustedPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
				Quaternion targetRot = Quaternion.LookRotation(adjustedPos - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
            }

			movement = curSpeed * Vector3.forward;
			movement = transform.TransformDirection(movement);

			if (Vector3.Distance(targetPos, transform.position) < targetBuffer)
            {
				curSpeed -= deceleration * Time.deltaTime;
				if (curSpeed <= 0)
                {
					targetPos = Vector3.one;
                }					
            }				
        }

		animator.SetFloat("Speed", movement.sqrMagnitude);

		bool hitGround = false;
		RaycastHit hit;
		if (vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit))
		{
			float check = (charController.height + charController.radius) / 1.9f;
			hitGround = hit.distance <= check;
		}

        if (hitGround)
        {
            //if (Input.GetButtonDown("Jump"))
            //{
            //   vertSpeed = jumpSpeed;
            //}
            //else
            //{
                vertSpeed = minFall;
                animator.SetBool("Jumping", false);
            //}
        }
        else
        {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }
            if (contact != null)
            {
                animator.SetBool("Jumping", true);
            }


            if (charController.isGrounded)
            {
                if (Vector3.Dot(movement, contact.normal) < 0)
                {
                    movement = contact.normal * moveSpeed;
                }
                else
                {
                    movement += contact.normal * moveSpeed;
                }
            }
        }
        movement.y = vertSpeed;

        movement *= Time.deltaTime;
		charController.Move(movement);
	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		contact = hit;

		Rigidbody body = hit.collider.attachedRigidbody;
		if (body != null && !body.isKinematic)
		{
			body.velocity = hit.moveDirection * pushForce;
		}
	}
}
