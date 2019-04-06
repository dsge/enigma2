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
        this.map = generateMap(initMapParts(), new Vector2(5, 5));
    }

    public List<GameObject> getMap() {
        return this.map;
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

    List<GameObject> generateMap(List<GameObject> mapPartTemplates, Vector2 mapSize){
        System.Random rnd = new System.Random();
        List<GameObject> ret = new List<GameObject>();
        /**
         * a világ közepétől milyen távol legyen a generált pályarészek széle
         */
        Vector3 mapOffsetFromCenter = new Vector3(-35, 0, 0);

        for (int j = 0; j < mapSize.y; j++){
            for (int i = 0; i < mapSize.x; i++){
                GameObject prefab = mapPartTemplates[rnd.Next(mapPartTemplates.Count)];
                Vector3 prefabBoundsSize = prefab.GetComponent<Renderer>().bounds.size;
                Vector3 position = new Vector3(-i * prefabBoundsSize.x, 0, -j * prefabBoundsSize.z) + mapOffsetFromCenter;
                GameObject part = Instantiate (prefab, position, Quaternion.identity);
                ret.Add(part);
            }
        }
        return ret;
    }
}
