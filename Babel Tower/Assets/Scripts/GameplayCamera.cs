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

    // Mouvements angulaires
    private const float ANGLE = 90f;
    private const float MAX_ANGULAR_SPEED = 200f;
    private const float MIN_ANGULAR_SPEED = 100f;
    private const float ANGULAR_ACCELERATION = 1f;
    private float remainingDegrees = 0f;
    private float angularSpeed = 0f;

    // Mouvements verticaux
    public float initialHeigh = 0f;
    private const float HEIGH = 1f; // Ne marche pas
    private const float MAX_VERTICAL_SPEED = 3f;
    private const float MIN_VERTICAL_SPEED = 1.5f;
    private const float VERTICAL_ACCELERATION = 1f;
    private float verticalSpeed = 0f;
    public float remainingHeigh = 0f;
    public float lastHeigh = 0f;

    void Start () {
        initialHeigh = transform.position.y;
        target = new Vector3(0.5f, 0f, 0.5f);
        remainingDegrees = ANGLE;
        ResetSpeeds();
    }

    public void ResetSpeeds() {
        remainingDegrees = ANGLE;
        currentState = CameraState.HoldPosition;
        angularSpeed = MAX_ANGULAR_SPEED;

        verticalSpeed = MAX_VERTICAL_SPEED;
        remainingHeigh = (target.y - lastHeigh) * HEIGH;
        lastHeigh = target.y;
    }
	
	void Update () {
        switch(currentState) {
            case CameraState.MoveTo:
                float distance;
                // Rotation de la caméra
                if (remainingDegrees > 0f) {
                    angularSpeed = Mathf.Lerp(angularSpeed, MIN_ANGULAR_SPEED, ANGULAR_ACCELERATION * Time.deltaTime);
                    distance = angularSpeed * Time.deltaTime;
                    if (remainingDegrees - distance < 0) {
                        distance = remainingDegrees;
                        remainingDegrees = 0f;
                    }
                    else
                        remainingDegrees -= distance;
                    transform.RotateAround(target, Vector3.up, -distance);
                }
                // Translation verticale de la caméra
                /*if (remainingHeigh > 0f) {
                    verticalSpeed = Mathf.Lerp(verticalSpeed, MIN_VERTICAL_SPEED, VERTICAL_ACCELERATION * Time.deltaTime);
                    distance = verticalSpeed * Time.deltaTime;
                    if(remainingHeigh - distance < 0) {
                        distance = remainingHeigh;
                        remainingHeigh = 0f;
                    }
                    else
                        remainingHeigh -= distance;
                    //transform.Translate(Vector3.up * distance);
                }*/

                if (remainingDegrees <= 0 /*&& remainingHeigh <= 0f*/)
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
