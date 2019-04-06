using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSceneSwitchHandler : MonoBehaviour
{
    List<GameObject> warpPads;
    GameObject warpPadTemplate;
    void Start()
    {
        this.warpPadTemplate = loadGameObjectFromResource("warppad/warppad");
        this.warpPadTemplate.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        placeWarppad();
    }

    void placeWarppad() {
        warpPads.Add(Instantiate (warpPadTemplate, new Vector3(-10, 0, -10), Quaternion.identity));
    }

    GameObject loadGameObjectFromResource(string resourceName){
        GameObject ret = Resources.Load(resourceName) as GameObject;
        if (ret == null) {
            throw new System.ArgumentException(System.String.Format("unable to load resource ({0})", resourceName), "resourceName");
        }
        return ret;
    }
}
