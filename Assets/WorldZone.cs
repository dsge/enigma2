using UnityEngine;
/**
 * WorldZones are big named areas in the world.
 *
 * Multiple WorldZones can belong to a single Scene. Travelling to any of these
 * WorldZones should trigget the loading of that single Scene (if it's not loaded already).
 */
public class WorldZone{
    /**
     * Which Scene should be loaded (if necessary) when the player enters this zone?
     */
    public string sceneName;
    /**
     * the map that this zone resides in
     */
    public BasicMap map;
    public WorldZone(string sceneName, BasicMap map){
        this.sceneName = sceneName;
        this.map = map;
    }
}
