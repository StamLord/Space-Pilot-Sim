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

    private new Rigidbody rigidbody;
    private new Transform camera;
	public Collider interactionCollider;

	private List<IInteractable> nearbyInteractables = new List<IInteractable>();
	private PilotSeat nearbySeat;
	
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
        //Rotate();
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
	}

    void Rotate()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y = camera.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(rot);
    }
 
	void FixedUpdate () {
	    if (grounded) {
	        // Calculate how fast we should be moving
	        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
	        targetVelocity = transform.TransformDirection(targetVelocity);
	        targetVelocity *= speed;
 
	        // Apply a force that attempts to reach our target velocity
	        Vector3 velocity = rigidbody.velocity;
	        Vector3 velocityChange = (targetVelocity - velocity);
	        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
	        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
	        velocityChange.y = 0;
	        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
 
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
 
	    grounded = false;
	}
 
	void OnCollisionStay () {
	    grounded = true;    
	}
 
	float CalculateJumpVerticalSpeed () {
	    // From the jump height and gravity we deduce the upwards speed 
	    // for the character to reach at the apex.
	    return Mathf.Sqrt(2 * jumpHeight * intensity);
	}
}
