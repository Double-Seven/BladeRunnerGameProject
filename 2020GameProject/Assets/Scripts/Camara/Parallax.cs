using UnityEngine;

public class Parallax : MonoBehaviour {
    private float length = 8f, startpos;
    public GameObject cam;
    public float parallaxEffect = 1;

    private void Start()
    {
        startpos = transform.position.x;
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr) length = mr.bounds.size.x;
        else length = 20f;
        Debug.Log(length);
    }

    private void Update()
    {
        if (!cam) cam = GameFlowManager.instance.getCamera();
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float distance = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
