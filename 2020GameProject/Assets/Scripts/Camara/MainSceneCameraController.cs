using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneCameraController : MonoBehaviour
{

    public Character player;

    [Header("Motion values")]
    public float horizontaLevel = 2;  // the fixed y-level of camera
    public float movementSmoothFactor = 0.3f;  // the smooth factor for the camera movement

    private float depthLevel = -10;  // the z-level of camera
    private Vector3 camera_velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // only make this camera follow the change of x-position of player in the main scene
        if (!player.isFacingRight || player.transform.position.x < transform.position.x) return;
        Vector3 cameraPos = new Vector3(player.transform.position.x, horizontaLevel, depthLevel);

        // Smoothly move the camera towards that target position
        // camera_velocity will be passed as reference and gradually change by the function
        this.transform.position = Vector3.SmoothDamp(this.transform.position, cameraPos, ref camera_velocity, movementSmoothFactor);

    }
}
