using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        ret.Add(initMapPart("basicMapParts/MapPart_01"));
        ret.Add(initMapPart("basicMapParts/MapPart_02"));
        ret.Add(initMapPart("basicMapParts/MapPart_03"));
        ret.Add(initMapPart("basicMapParts/MapPart_04"));
        ret.Add(initMapPart("basicMapParts/MapPart_05"));
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
         * the edge of the map from the center of the world
         */
        Vector3 mapOffsetFromCenter = new Vector3(1, 0, 0);

        List<NavMeshBuildSource> allNavMeshSources = new List<NavMeshBuildSource>();

        for (int j = 0; j < mapSize.y; j++){
            for (int i = 0; i < mapSize.x; i++){
                /**
                 * randomize a mapPart that we will use
                 */
                GameObject prefab = mapPartTemplates[rnd.Next(mapPartTemplates.Count)];
                /**
                 * the estimated size of the mapPart
                 */
                Vector3 prefabBoundsSize = prefab.GetComponent<Renderer>().bounds.size;
                /**
                 * we will place it at the next spot in our map
                 */
                Vector3 position = new Vector3(-i * prefabBoundsSize.x, 0, -j * prefabBoundsSize.z) + mapOffsetFromCenter;
                GameObject part = Instantiate (prefab, position, Quaternion.identity);
                /**
                 * collect the navmesh sources from this mapPart
                 */
                List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
                NavMeshBuilder.CollectSources(part.GetComponent<Renderer>().bounds, Layers.GROUND, NavMeshCollectGeometry.PhysicsColliders, 0, new List<NavMeshBuildMarkup>(), sources);
                allNavMeshSources.AddRange(sources);
                /**
                 * we will store a reference to the generated part we could work with it later
                 */
                ret.Add(part);
            }
        }
        /**
         * create the navmeshdata for all the surfaces that we're going to use
         */
        allNavMeshSources.AddRange(this.getAdditionalNavMeshSources());
        NavMeshData data = this.createNavMeshData(allNavMeshSources);
        if (data != null) {
            NavMesh.AddNavMeshData(data);
        }
        return ret;
    }

    List<NavMeshBuildSource> getAdditionalNavMeshSources() {
        /**
         * since all navmeshes have to be generated at once, we need to add any extra surfaces that we want enemies to walk on
         */
        List<NavMeshBuildSource> ret = new List<NavMeshBuildSource>();

        GameObject plane = GameObject.Find("Plane");
        if (plane != null) {
            /**
             * this is the basic floor of the SampleScene (the one that isn't generated by BasicMapGenerator)
             */
            List<NavMeshBuildSource> s = new List<NavMeshBuildSource>();
            NavMeshBuilder.CollectSources(plane.GetComponent<Renderer>().bounds, Layers.GROUND, NavMeshCollectGeometry.PhysicsColliders, 0, new List<NavMeshBuildMarkup>(), s);
            ret.AddRange(s);
        }

        return ret;
    }

    NavMeshData createNavMeshData(List<NavMeshBuildSource> sources){
        Bounds b = new Bounds(Vector3.zero, Vector3.zero);
        foreach (Renderer r in FindObjectsOfType(typeof(Renderer))) {
            b.Encapsulate(r.bounds);
        }
        return NavMeshBuilder.BuildNavMeshData(NavMesh.GetSettingsByID(0), sources, b, Vector3.zero, Quaternion.identity);
    }


}
