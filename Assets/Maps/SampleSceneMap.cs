using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SampleSceneMap : BasicMap{
    public override WarpTarget getDefaultWarpTarget(){
        if (this.defaultWarpTarget == null) {
            this.defaultWarpTarget = new WarpTarget(this.getProvidedZones()[0], new Vector3(0, 1.5f, 0));
        }
        return this.defaultWarpTarget;
    }

    /**
     * what Zones does this map provide? which ones it contain?
     */
    public override List<WorldZone> getProvidedZones(){
        if (this.zones == null) {
            this.zones = new List<WorldZone>();
            this.zones.Add(new WorldZone("SampleScene", this));
        }
        return this.zones;
    }

    public override void initializeScene(){
        new GameObject("local map related stuff", new System.Type[]{
            typeof(BasicEnemySpawner),
        });

        BasicMapGenerator mapGenerator = new BasicMapGenerator();
        List<GameObject> mapPartTemplates = new List<GameObject>();
        mapPartTemplates.Add(loadResource("basicMapParts/MapPart_01"));
        mapPartTemplates.Add(loadResource("basicMapParts/MapPart_02"));
        mapPartTemplates.Add(loadResource("basicMapParts/MapPart_03"));
        mapPartTemplates.Add(loadResource("basicMapParts/MapPart_04"));
        mapPartTemplates.Add(loadResource("basicMapParts/MapPart_05"));
        mapPartTemplates.Add(loadResource("basicMapParts/MapPart_13_TwoEnterance"));
        List<GameObject> map = mapGenerator.generateMap(mapPartTemplates, new Vector2(5, 5));

        BasicSceneSwitchHandler handler = GameObject.Find(BasicSceneSwitchHandler.GLOBAL_COMPONENTS_HANDLER_NAME).GetComponent<BasicSceneSwitchHandler>();

        (GameObject.Instantiate(handler.warpPadTemplate, new Vector3(-10, 0, -10), Quaternion.identity)
            .AddComponent(typeof(WarpPadWarpTargetHandler)) as WarpPadWarpTargetHandler)
            .warpTarget = new WarpTarget(handler.getMaps()[0].getProvidedZones()[0], new Vector3(0, 1.5f, 0));

        (GameObject.Instantiate(handler.warpPadTemplate, new Vector3(15, 0, -15), Quaternion.identity)
            .AddComponent(typeof(WarpPadWarpTargetHandler)) as WarpPadWarpTargetHandler)
            .warpTarget = new WarpTarget(zones[0], new Vector3(5, 0, -15));

        (GameObject.Instantiate(handler.warpPadTemplate, new Vector3(5, 0, -15), Quaternion.identity)
            .AddComponent(typeof(WarpPadWarpTargetHandler)) as WarpPadWarpTargetHandler)
            .warpTarget = new WarpTarget(zones[0], new Vector3(15, 0, -15));
    }

    protected GameObject loadResource(string resourceName) {
        GameObject ret = Resources.Load(resourceName) as GameObject;
        if (ret == null) {
            throw new System.ArgumentException(System.String.Format("unable to load resource ({0})", resourceName), "resourceName");
        }
        return ret;
    }
}
