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
    public const int WARPPADS_ID = 11;
    /**
     * bitmask for the player that we are currently playing as (in the middle of the screen) - do NOT use this for other players who
     * just happen to be on screen
     */
    public const int CURRENT_PLAYER = 1 << 12;
}
