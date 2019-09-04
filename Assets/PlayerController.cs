using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
 	[SerializeField] [Range(0,20)] public float walkSpeed;
	[SerializeField] [Range(0,20)] public float runSpeed;
	[SerializeField] [Range(0,-20)] public float gravity;
	[SerializeField] [Range(0,50)] public float jumpHeight;
	[SerializeField] [Range(0,50)] public float WallJumpHeight;
	[SerializeField] [Range(0,1)] public float airControlPercent;

	[SerializeField] [Range(0,1)] public float turnSmoothTime;
	float turnSmoothVelocity;

	[SerializeField] [Range(0,1)] public float speedSmoothTime;
	[SerializeField] public bool canJump; 
	[SerializeField] public bool isDead;
	[SerializeField] public bool isPaused;
	float speedSmoothVelocity;
	float currentSpeed;
	float velocityY;

	[SerializeField] public GameObject PauseMenu;
	[SerializeField] public GameObject DeathMenu;

	Animator animator;
	Transform cameraT;
	CharacterController controller;


	void Start () {
		animator = GetComponent<Animator> ();
		cameraT = Camera.main.transform;
		controller = GetComponent<CharacterController> ();
	}

	void Update () {
		// input
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		Vector2 inputDir = input.normalized;
		bool running = Input.GetKey (KeyCode.LeftShift);

		Move (inputDir, running);

		if(controller.isGrounded){
			canJump = true;
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			Jump ();
			WallJump();
		}

		if(Input.GetButtonDown("Cancel") && isDead == false){
			pauseGame();
		}

		// animator
		/*/float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
		animator.SetFloat ("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
        /*/ 
	}

	public void Move(Vector2 inputDir, bool running) {
		if (inputDir != Vector2.zero) {
			float targetRotation = Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
		}
			
		float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

		velocityY += Time.deltaTime * gravity;
		Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

		controller.Move (velocity * Time.deltaTime);
		currentSpeed = new Vector2 (controller.velocity.x, controller.velocity.z).magnitude;

		if (controller.isGrounded) {
			velocityY = 0;
		}

	}

	public void Jump() {
		if (controller.isGrounded && canJump) {
			float jumpVelocity = Mathf.Sqrt (-2 * gravity * jumpHeight);
			velocityY = jumpVelocity;
			canJump = false;
		}
	}

	public void WallJump(){
		if (canJump && controller.isGrounded == false) {
			float jumpVelocity = Mathf.Sqrt (-2 * gravity * WallJumpHeight);
			velocityY = jumpVelocity;
			canJump = false;
		}
	}

	float GetModifiedSmoothTime(float smoothTime) {
		if (controller.isGrounded) {
			return smoothTime;
		}

		if (airControlPercent == 0) {
			return float.MaxValue;
		}
		return smoothTime / airControlPercent;
	}

	void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Jumpable")){
            Debug.Log("can walljump");
			if(canJump == false){
            canJump = true;
			}
        }

		if(other.gameObject.CompareTag("Death")){
			DeathEXE();
		}
    }

	void OnTriggerExit(Collider other){
		if(other.gameObject.CompareTag("Jumpable")){
            Debug.Log("can't walljump");
			if(canJump == true){
            canJump = false;
			}
		}
	}

	void DeathEXE(){
	Destroy(gameObject);
	isDead = true;
	DeathMenu.SetActive(true);
	Time.timeScale = 0;
	cameraT.GetComponent<CameraController>().setCursorLock();
	}

	public void pauseGame(){
		if(isPaused == false){
			isPaused = true;
			PauseMenu.SetActive(true);
			Time.timeScale = 0;
			cameraT.GetComponent<CameraController>().setCursorLock();
		} else if(isPaused){
			isPaused = false;
			PauseMenu.SetActive(false);
			Time.timeScale = 1;
			cameraT.GetComponent<CameraController>().setCursorLock();
		}
	}

}