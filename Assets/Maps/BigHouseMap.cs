using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigHouseMap : BasicMap{
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
        GameObject bigHouse = GameObject.Instantiate(this.loadGameObjectFromResource("big_house/big_house"), new Vector3(0, 0, 0), Quaternion.identity);
        bigHouse.transform.eulerAngles = new Vector3(0, 90, 0);

        BasicSceneSwitchHandler handler = GameObject.Find(BasicSceneSwitchHandler.GLOBAL_COMPONENTS_HANDLER_NAME).GetComponent<BasicSceneSwitchHandler>();

        WorldZone targetZone = handler.getMaps()[1].getProvidedZones()[0];

        (GameObject.Instantiate(handler.warpPadTemplate, new Vector3(-10, 0, -10), Quaternion.identity)
            .AddComponent(typeof(WarpPadWarpTargetHandler)) as WarpPadWarpTargetHandler)
            .warpTarget = new WarpTarget(targetZone, new Vector3(0, 1.5f, 0));
    }
}
