class Layers {
    /**
     * bitmask for objects that are considered as part of the "ground" that the player can walk on
     */
    public const int GROUND = 1 << 9;
    /**
     * bitmask for objects that are considered enemies that the player can attack
     */
    public const int ENEMIES = 1 << 10;
    /**
     * bitmask for objects that are considered warppads or warp points that the user can click on to switch zones/areas
     */
    public const int WARPPADS = 1 << 11;
}
