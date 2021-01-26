using UnityEngine;

public class Parallax : MonoBehaviour {
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect = 1;

    private void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<MeshRenderer>().bounds.size.x;
        if (Mathf.Approximately(length, 0f)) length = 10f;
        Debug.Log(gameObject.name + length);
    }

    private void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float distance = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
