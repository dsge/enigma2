using UnityEngine;
/**
 * this class will hold all the info that is needed to tell where a player should be warped inside a map
 *
 * it should be able to be used with raw coordinates
 *
 * @todo it should also support warping to GameObjects (like other players or in front of doors or to a warppad)
 *       without having to manually extract their coordinates before the warp
 *
 * @todo it should also hold information about the target map (rather than just coordinates inside a map)
 */
public class WarpTarget {
    protected Vector3 targetCoordinates;

    public WarpTarget(Vector3 targetCoordinates) {
        this.targetCoordinates = targetCoordinates;
    }

    public Vector3 getTargetCoordinates() {
        return this.targetCoordinates;
    }
}
