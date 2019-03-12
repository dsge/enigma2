using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foo : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Vertical") * 10.0f;
        float rotation = Input.GetAxis("Horizontal") * 100.0f;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);
    }
}
