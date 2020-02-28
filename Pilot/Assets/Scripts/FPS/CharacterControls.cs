using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
 
public class CharacterControls : GravityObject {
 
	[SerializeField] private float speed = 10.0f;
	[SerializeField] private float maxVelocityChange = 10.0f;
	[SerializeField] private bool canJump = true;
	[SerializeField] private float jumpHeight = 2.0f;
	[SerializeField] private Vector2 mouseSensetivity = new Vector2(.5f, .5f);
	[SerializeField] private Vector2 mouseValue;
	private bool grounded = false;

    private new Rigidbody rigidbody;
    [SerializeField] private new Transform camera;
	[SerializeField] private Vector2 verticalClamp = new Vector2(-90, 90);

	[SerializeField] private float interactionDistance = 1f;
	[SerializeField] private IInteractable targetInteractable;
	[SerializeField] private List<PilotSeat> nearbySeats = new List<PilotSeat>();
	private bool enabledThisFrame;

	private Vector3 targetVelocity;
	
	void Awake () 
	{
        rigidbody = GetComponent<Rigidbody>();
	    rigidbody.freezeRotation = true;
	    rigidbody.useGravity = false;
	}

	public void ActivateRigidbody(bool active)
	{
		rigidbody.isKinematic = !active;
	}

    void Update()
    {
		OrientAgainstGravity();
		ProccessInput();
		FindInteractable();
		
		if(enabledThisFrame)
			enabledThisFrame = false;
    }

	void OnTriggerEnter(Collider other)
	{
		PilotSeat seat = other.GetComponent<PilotSeat>();
		if(seat)
			nearbySeats.Add(seat);
	}

	void OnTriggerExit(Collider other)
	{
		PilotSeat seat = other.GetComponent<PilotSeat>();
		if(seat)
			nearbySeats.Remove(seat);
	}

	void ProccessInput()
	{
		// Calculate how fast we should be moving
		targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		
		if(Input.GetKey(KeyCode.W))
			targetVelocity.z = 1;
		if(Input.GetKey(KeyCode.S))
			targetVelocity.z = -1;
		if(Input.GetKey(KeyCode.A))
			targetVelocity.x = -1;
		if(Input.GetKey(KeyCode.D))
			targetVelocity.x = 1;

		if(Input.GetKeyDown(KeyCode.F) && enabledThisFrame == false)
		{
			PilotSeat nearestSeat = FindNearestSeat();
			if(nearestSeat) nearestSeat.Enter(this);
		}

		if(Input.GetKeyDown(KeyCode.E))
		{
			if(targetInteractable != null)
				targetInteractable.Interact();
		}
		
		mouseValue += new Vector2(
			Input.GetAxis("Mouse X") * mouseSensetivity.x, 
			Input.GetAxis("Mouse Y") * mouseSensetivity.y);

		mouseValue.y = Mathf.Clamp(mouseValue.y, verticalClamp.x, verticalClamp.y);

		Vector3 cameraRot = camera.localRotation.eulerAngles;
		cameraRot.x = -mouseValue.y;
		camera.localRotation = Quaternion.Euler(cameraRot);

		// Debug.DrawRay(transform.position, transform.up, Color.green, 1);
		transform.Rotate(Vector3.up, mouseValue.x, Space.Self);
	}

	void FindInteractable()
	{
		RaycastHit hit;
		if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(.5f, .5f)), out hit, interactionDistance))
		{
			IInteractable interactable = hit.collider.GetComponent<IInteractable>();
			if(interactable != null)
			{
				// Update UI
				if(targetInteractable != interactable)
					Debug.Log(hit.collider.name);
					
				targetInteractable = interactable;
			}
			else
				targetInteractable = null;
		}
		else
			targetInteractable = null;
	}

	PilotSeat FindNearestSeat()
	{
		PilotSeat nearest = null;
		float distance = Mathf.Infinity;

		foreach(PilotSeat seat in nearbySeats)
		{
			float d = Vector3.Distance(transform.position, seat.transform.position);
			if(d < distance || nearest == null)
			{
				nearest = seat;
				distance = d;
			}
		}

		return nearest;
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

    void OrientAgainstGravity()
    {
		if(direction != Vector3.zero)
			transform.up = -direction;
    }
	
	void FixedUpdate () 
	{
	    if (grounded) 
		{
	        targetVelocity = transform.TransformDirection(targetVelocity);
	        targetVelocity *= speed;
			Debug.DrawRay(transform.position, targetVelocity, Color.blue, 1f);
 
	        // Apply a force that attempts to reach our target velocity
	        Vector3 velocity = rigidbody.velocity;

	        Vector3 velocityChange = transform.InverseTransformVector(targetVelocity - velocity);
	        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
	        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
	        velocityChange.y = 0;
	        rigidbody.AddRelativeForce(velocityChange, ForceMode.VelocityChange);
			Debug.DrawRay(transform.position, velocityChange, Color.red, 1f);
 
	        // Jump
	        if (canJump && Input.GetButton("Jump")) {
				Vector3 localVelocity = transform.InverseTransformVector(rigidbody.velocity);
	            rigidbody.velocity = transform.TransformVector(
					new Vector3(localVelocity.x, 0, localVelocity.z));
				rigidbody.velocity -= direction * CalculateJumpVerticalSpeed();
	        }
	    }
 
	    // We apply gravity manually for more tuning control
	    rigidbody.AddForce(direction * intensity * rigidbody.mass);
		Debug.DrawRay(transform.position, direction, Color.yellow, 1f);
		
	    grounded = false;
		//platform = null;
	}
 
	void OnCollisionStay (Collision collision) {
	    grounded = true;    
	}
 
	float CalculateJumpVerticalSpeed () {
	    // From the jump height and gravity we deduce the upwards speed 
	    // for the character to reach at the apex.
	    return Mathf.Sqrt(2 * jumpHeight * intensity);
	}
}
