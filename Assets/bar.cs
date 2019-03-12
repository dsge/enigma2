using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bar : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Get the mouse delta. This is not in the range -1...1
        float h = 2.0f * Input.GetAxis("Mouse X");
        float v = 2.0f * Input.GetAxis("Mouse Y");

        transform.Rotate(v, h, 0);
    }
}
