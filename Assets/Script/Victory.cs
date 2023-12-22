using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour

{
    public static Victory Instance;

    public Camera defaultCamera;
    public Camera winnerCamera;
    public bool isWinner = false;

    public Transform target;
    public float smoothspeed = 1.0f;

    public Transform playerRotation;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        defaultCamera.enabled = true;
        winnerCamera.enabled = false; 
    }

    // Update is called once per frame
    void Update()
    {
        if (isWinner)
        {
            defaultCamera.enabled = false;
            winnerCamera.enabled = true;
        }
    
    }

    private void LateUpdate()
    {
        if (target != null && isWinner)
        {
            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, target.position.z + 2.4f);

            Vector3 smoothedPosition = Vector3.Lerp(winnerCamera.transform.position, desiredPosition, smoothspeed*Time.deltaTime);

            winnerCamera.transform.position = smoothedPosition;

            playerRotation.LookAt(new Vector3(playerRotation.position.x, playerRotation.position.y, playerRotation.position.z));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && PlayerController.instance.groundedPlayer)
        {
            isWinner = true;
        }
    }
}
