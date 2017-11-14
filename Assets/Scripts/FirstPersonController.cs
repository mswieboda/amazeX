using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour {

	public float movementSpeed = 5.0f;
	public float movementSprintSpeed = 10.0f;
	public float mouseSensitivity = 2.0f;
	public float upDownRange = 60.0f;
	public float jumpSpeed = 20.0f;

	private float verticalRotation = 0;
//	private float horizontalRotation = 0;
	private float verticalVelocity = 0;
	private float movementSpeedInAir;

	private CharacterController characterController;

	// Use this for initialization
	void Start() {
		Cursor.visible = false;
		characterController = GetComponent<CharacterController> ();
	}

	// Update is called once per frame
	void Update() {

		// Rotation
		float rotationLeftRight;
		float rotationUpDown;

		rotationLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;

		transform.Rotate(0, rotationLeftRight, 0);
//		if (characterController.isGrounded) {
//			transform.Rotate(0, rotationLeftRight, 0);
//		}
//		else {
//			Camera.main.transform.localRotation = Quaternion.Euler(0, -rotationLeftRight, 0);
//			horizontalRotation += rotationLeftRight;
//		}

		rotationUpDown = Input.GetAxis("Mouse Y") * mouseSensitivity;

		verticalRotation -= rotationUpDown;
		verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);

//		Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
		Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

		// Movement
		Vector3 speed;
		float forwardSpeed = Input.GetAxis("Vertical");
		float sideSpeed = Input.GetAxis("Horizontal");

		Debug.Log("Fire2 down: " + Input.GetButton("Fire2"));

		if(characterController.isGrounded && Input.GetButtonDown("Jump")) {
			movementSpeedInAir = getForwardSpeed(forwardSpeed);
			verticalVelocity = jumpSpeed;
		}
		else if(!characterController.isGrounded) {
			forwardSpeed = movementSpeedInAir;

			if (!Input.GetButton("Fire2")) {
				verticalVelocity += Physics.gravity.y * Time.deltaTime;
			}
		}
		else {
			forwardSpeed = getForwardSpeed(forwardSpeed);

			if (!Input.GetButton("Fire2")) {
				verticalVelocity = -1f;
			}
		}


		if(characterController.isGrounded) {
			Debug.Log("Grounded" + " v:" + verticalVelocity);
		}
		else {
			Debug.Log("NOT Grounded" + " v:" + verticalVelocity);
		}

		sideSpeed *= movementSpeed;
		speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);
		speed = transform.rotation * speed;

		characterController.Move(speed * Time.deltaTime);
	}

	float getForwardSpeed(float forwardSpeed) {
		return forwardSpeed * (Input.GetButton("Fire3") ? movementSprintSpeed : movementSpeed);
	}
}
