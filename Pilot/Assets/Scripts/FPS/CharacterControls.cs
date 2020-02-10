using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
 
public class CharacterControls : GravityObject {
 
	public float speed = 10.0f;
	public float maxVelocityChange = 10.0f;
	public bool canJump = true;
	public float jumpHeight = 2.0f;
	private bool grounded = false;
	private Rigidbody platform;

    private new Rigidbody rigidbody;
    private new Transform camera;
	public Collider interactionCollider;

	private List<IInteractable> nearbyInteractables = new List<IInteractable>();
	private PilotSeat nearbySeat;
	private bool enabledThisFrame;
	
	void Awake () {
        rigidbody = GetComponent<Rigidbody>();
	    rigidbody.freezeRotation = true;
	    rigidbody.useGravity = false;
		
		camera = Camera.main.transform;
	}

	public void ActivateRigidbody(bool active)
	{
		rigidbody.isKinematic = !active;
	}

    void Update()
    {
		ProccessInput();
		
		if(enabledThisFrame)
			enabledThisFrame = false;
    }

	void OnTriggerEnter(Collider other)
	{
		PilotSeat seat = other.GetComponent<PilotSeat>();
		if(seat)
			nearbySeat = seat;

		IInteractable interactable = other.GetComponent<IInteractable>();
		if(interactable != null)
			nearbyInteractables.Add(interactable);
	}

	void OnTriggerExit(Collider other)
	{
		PilotSeat seat = other.GetComponent<PilotSeat>();
		if(seat == nearbySeat)
			nearbySeat = null;

		IInteractable interactable = other.GetComponent<IInteractable>();
		if(interactable != null)
			nearbyInteractables.Remove(interactable);
	}

	void ProccessInput()
	{
		if(enabledThisFrame)
			return;
			
		if(Input.GetKeyDown(KeyCode.F))
		{
			if(nearbySeat)
				nearbySeat.Enter(this);
		}

		if(Input.GetKeyDown(KeyCode.E))
		{

			if(nearbyInteractables.Count != 0)
				nearbyInteractables[0].Interact();
		}
	}

	void OnDisable()
	{
		GetComponent<CapsuleCollider>().enabled = false;
	}

	void OnEnable()
	{
		GetComponent<CapsuleCollider>().enabled = true;
		enabledThisFrame = true;
	}

    void Rotate()
    {
		transform.up = -direction;
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y = camera.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(rot);
    }
 
	void FixedUpdate () 
	{
		Rotate();

	    if (grounded) {
	        // Calculate how fast we should be moving
	        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			
			if(Input.GetKey(KeyCode.W))
				targetVelocity.z = 1;
			if(Input.GetKey(KeyCode.S))
				targetVelocity.z = -1;
			if(Input.GetKey(KeyCode.A))
				targetVelocity.x = -1;
			if(Input.GetKey(KeyCode.D))
				targetVelocity.x = 1;

	        targetVelocity = transform.TransformDirection(targetVelocity);
	        targetVelocity *= speed;
			Debug.DrawRay(transform.position, targetVelocity, Color.blue, 1f);
 
	        // Apply a force that attempts to reach our target velocity
	        Vector3 velocity = rigidbody.velocity;
			velocity -= platform.velocity;
	        Vector3 velocityChange = (targetVelocity - velocity);
	        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
	        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
	        velocityChange.y = 0;
	        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
			Debug.DrawRay(transform.position, velocityChange, Color.red, 1f);
 
	        // Jump
	        if (canJump && Input.GetButton("Jump")) {
				Vector3 localVelocity = transform.TransformDirection(rigidbody.velocity);
	            rigidbody.velocity = transform.TransformVector(
					new Vector3(localVelocity.x, 0, localVelocity.z));
				rigidbody.velocity -= direction * CalculateJumpVerticalSpeed();
	        }
	    }
 
	    // We apply gravity manually for more tuning control
	    rigidbody.AddForce(direction * intensity * rigidbody.mass);
		Debug.DrawRay(transform.position, direction, Color.yellow, 1f);
		
 
	    grounded = false;
		platform = null;
	}
 
	void OnCollisionStay (Collision collision) {
	    grounded = true;    
		if(collision.rigidbody)
			platform = collision.rigidbody;
	}
 
	float CalculateJumpVerticalSpeed () {
	    // From the jump height and gravity we deduce the upwards speed 
	    // for the character to reach at the apex.
	    return Mathf.Sqrt(2 * jumpHeight * intensity);
	}
}
