using UnityEngine;
using System.Collections;

public class GameplayCamera : MonoBehaviour {

    public enum CameraState {
        Zero = 0,
        MoveTo,
        HoldPosition,
        NumberOfStates
    }

    private Vector3 target;
    public CameraState currentState = CameraState.HoldPosition;
    private Camera thisCamera;
    public float remainingDegrees = 0f;
    private const float ANGLE = 90f;
    private const float MAX_ANGULAR_SPEED = 100f;
    private const float MIN_ANGULAR_SPEED = 50f;
    public float angularSpeed = 0f;
    private const float ANGULAR_ACCELERATION = 1f;

	// Use this for initialization
	void Start () {
        target = new Vector3(0.5f, 0.5f, 0);
        remainingDegrees = ANGLE;
        thisCamera = GetComponent<Camera>();
        InitValues();
    }

    public void InitValues() {
        remainingDegrees = ANGLE;
        currentState = CameraState.HoldPosition;
        angularSpeed = MAX_ANGULAR_SPEED;
    }
	
	// Update is called once per frame
	void Update () {
        switch(currentState) {
            case CameraState.MoveTo:
                angularSpeed = Mathf.Lerp(angularSpeed, MIN_ANGULAR_SPEED, ANGULAR_ACCELERATION * Time.deltaTime);
                transform.RotateAround(target, Vector3.up, -angularSpeed * Time.deltaTime);
                remainingDegrees -= angularSpeed * Time.deltaTime;
                if (remainingDegrees <= 0)
                    InitValues();
                break;
            case CameraState.HoldPosition:
                break;
            default:
                break;
        }
    }

    public void MoveToNextTarget(Vector3 _position) {
        currentState = CameraState.MoveTo;
    }

    public Vector3 GetTarget() {
        return target;
    }
}
