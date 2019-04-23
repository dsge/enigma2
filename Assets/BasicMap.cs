using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BasicMap{
    /**
     * the generated map (a collection of map parts)
     */
    protected List<GameObject> parts;
    /**
     * the worldzones provided by this map
     */
    protected List<WorldZone> zones;
    protected WarpTarget defaultWarpTarget;

    public abstract WarpTarget getDefaultWarpTarget();

    /**
     * what Zones does this map provide? which ones it contain?
     */
    public abstract List<WorldZone> getProvidedZones();
    /**
     * load and start the contents of the scene
     */
    public abstract void initializeScene();

    protected GameObject loadGameObjectFromResource(string resourceName){
        GameObject ret = Resources.Load(resourceName) as GameObject;
        if (ret == null) {
            throw new System.ArgumentException(System.String.Format("unable to load resource ({0})", resourceName), "resourceName");
        }
        return ret;
    }

}
