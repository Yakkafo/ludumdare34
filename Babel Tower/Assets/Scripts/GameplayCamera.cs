using UnityEngine;
using System.Collections;

public class GameplayCamera : MonoBehaviour {

    public enum CameraState {
        Zero = 0,
        MoveTo,
        HoldPosition,
        NumberOfStates
    }

    public Vector3 target;
    public CameraState currentState = CameraState.HoldPosition;
    private Camera thisCamera;

    // Mouvements angulaires
    private const float ANGLE = 90f;
    private const float MAX_ANGULAR_SPEED = 200f;
    private const float MIN_ANGULAR_SPEED = 100f;
    private const float ANGULAR_ACCELERATION = 1f;
    private float remainingDegrees = 0f;
    private float angularSpeed = 0f;

    // Mouvements verticaux
    public float initialHeigh = 0f;
    private const float MAX_VERTICAL_SPEED = 3f;
    private const float MIN_VERTICAL_SPEED = 1.5f;
    private const float VERTICAL_ACCELERATION = 1f;
    private float verticalSpeed = 0f;
    public float remainingHeigh = 0f;

    void Start () {
        initialHeigh = transform.position.y;
        target = new Vector3(0.5f, initialHeigh, 0.5f);
        remainingDegrees = ANGLE;
        thisCamera = GetComponent<Camera>();
        ResetSpeeds();
    }

    public void ResetSpeeds() {
        remainingDegrees = ANGLE;
        currentState = CameraState.HoldPosition;
        angularSpeed = MAX_ANGULAR_SPEED;

        verticalSpeed = MAX_VERTICAL_SPEED;
        remainingHeigh = target.y - transform.position.y;
    }
	
	void Update () {
        switch(currentState) {
            case CameraState.MoveTo:
                // Rotation de la caméra
                if (remainingDegrees > 0f) {
                    angularSpeed = Mathf.Lerp(angularSpeed, MIN_ANGULAR_SPEED, ANGULAR_ACCELERATION * Time.deltaTime);
                    transform.RotateAround(target, Vector3.up, -angularSpeed * Time.deltaTime);
                    remainingDegrees -= angularSpeed * Time.deltaTime;
                    remainingDegrees = remainingDegrees < 0f ? 0f : remainingDegrees;
                }
                // Translation verticale de la caméra
                if (remainingHeigh > 0f) {
                    verticalSpeed = Mathf.Lerp(verticalSpeed, MIN_VERTICAL_SPEED, VERTICAL_ACCELERATION * Time.deltaTime);
                    float distance = verticalSpeed * Time.deltaTime;
                    transform.Translate(Vector3.up * distance);
                    remainingHeigh -= distance;
                    remainingHeigh = remainingHeigh < 0f ? 0f : remainingHeigh;
                }
                if (remainingDegrees <= 0 && remainingHeigh <= 0f)
                    ResetSpeeds();
                break;
            case CameraState.HoldPosition:
                break;
            default:
                break;
        }
    }

    public void MoveToNextTarget() {
        currentState = CameraState.MoveTo;
    }
}
