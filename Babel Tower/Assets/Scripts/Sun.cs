using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour {

    private Vector3 rotationSpeed;

	// Use this for initialization
	void Start () {
        rotationSpeed = new Vector3(0f, 5f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.World);
	}
}
