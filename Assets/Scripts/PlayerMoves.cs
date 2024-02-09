using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoves : MonoBehaviour
{
	[SerializeField] float 		base_speed;
	[SerializeField] float		sprint_speed;
	float						actual_speed;
	[SerializeField] Rigidbody	rb;
	[SerializeField] float 		jumpForce;
	[SerializeField] float grapplingForce = 10f;
	[SerializeField] Camera playerCamera;
	bool						isGrounded = true;
	bool						isGrapplined = false;

	public float maxGrappleDistance = 100f;

    // Update is called once per frame
    void Update()
    {
		actual_speed = Input.GetKey(KeyCode.LeftShift) ? sprint_speed : base_speed;
		if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
     
            rb.AddForce(new Vector3(0.0f, 2.0f, 0.0f) * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
		if (Input.GetKeyDown(KeyCode.B) && !isGrapplined)
			StartCoroutine(FireGrappin());
		float x = Input.GetAxis("Horizontal");
  		float z = Input.GetAxis("Vertical");
   		Vector3 movement = new Vector3(x, 0, z);
		transform.Translate(movement * actual_speed * Time.deltaTime);
    }

	IEnumerator FireGrappin() {
		RaycastHit hit;
		isGrapplined = true;
		if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maxGrappleDistance)) {
			Vector3 dir = hit.point - transform.position;
		    rb.AddForce(dir.normalized * grapplingForce, ForceMode.Impulse);
		}
		yield return new WaitForSeconds(0.2f);
		isGrapplined = false;
	}

	void	OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Ground")
			isGrounded = true;
	}
}