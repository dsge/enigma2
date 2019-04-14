using UnityEngine;

public class WarpTarget {
    protected Vector3 targetCoordinates;

    public WarpTarget(Vector3 targetCoordinates) {
        this.targetCoordinates = targetCoordinates;
    }

    public Vector3 getTargetCoordinates() {
        return this.targetCoordinates;
    }
}
