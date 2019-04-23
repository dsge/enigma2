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
            this.zones.Add(new WorldZone("bigHouse", this));
        }
        return this.zones;
    }

    public override void initializeScene(){
        new GameObject("local map related stuff", new System.Type[]{
            typeof(BasicMapGenerator),
            typeof(BasicEnemySpawner),
        });

        /*(Instantiate(this.warpPadTemplate, new Vector3(-10, 0, -10), Quaternion.identity)
            .AddComponent(typeof(WarpPadWarpTargetHandler)) as WarpPadWarpTargetHandler)
            .warpTarget = new WarpTarget(zones[0], new Vector3(0, 1.5f, 0));

        (Instantiate(this.warpPadTemplate, new Vector3(15, 0, -15), Quaternion.identity)
            .AddComponent(typeof(WarpPadWarpTargetHandler)) as WarpPadWarpTargetHandler)
            .warpTarget = new WarpTarget(zones[1], new Vector3(5, 0, -15));

        (Instantiate(this.warpPadTemplate, new Vector3(5, 0, -15), Quaternion.identity)
            .AddComponent(typeof(WarpPadWarpTargetHandler)) as WarpPadWarpTargetHandler)
            .warpTarget = new WarpTarget(zones[1], new Vector3(15, 0, -15));*/
    }
}
