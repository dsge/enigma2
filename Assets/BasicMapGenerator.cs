using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMapGenerator : MonoBehaviour
{
    /**
     * the generated map (a collection of map parts)
     */
    List<GameObject> map;

    void Start()
    {
        this.map = generateMap(initMapParts());
    }

    List<GameObject> initMapParts() {
        List<GameObject> ret = new List<GameObject>();
        ret.Add(initMapPart("MapPart_01"));
        ret.Add(initMapPart("MapPart_02"));
        ret.Add(initMapPart("MapPart_03"));
        ret.Add(initMapPart("MapPart_04"));
        ret.Add(initMapPart("MapPart_05"));
        return ret;
    }

    GameObject initMapPart(string resourceName){
        GameObject ret = Resources.Load(resourceName) as GameObject;
        if (ret == null) {
            throw new System.ArgumentException(System.String.Format("unable to load resource ({0})", resourceName), "resourceName");
        }
        return ret;
    }

    List<GameObject> generateMap(List<GameObject> mapPartTemplates){
        System.Random rnd = new System.Random();
        List<GameObject> ret = new List<GameObject>();

        for (int i = 0; i < 5; i++){
            GameObject prefab = mapPartTemplates[rnd.Next(mapPartTemplates.Count)];
            GameObject part = Instantiate (
                prefab,
                new Vector3(-i * prefab.GetComponent<Renderer>().bounds.size.x - 35, 0, 0),
                Quaternion.identity
            );
            ret.Add(part);
        }
        return ret;
    }
}
