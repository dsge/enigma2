using UnityEngine;

public class Init : MonoBehaviour
{
    static bool inited = false;
    void Start()
    {
        this.init();
    }
    void init(){
        if (Init.inited) {
            return;
        }
        Init.inited = true;
        GameObject globalComponentsHandler = createGlobalComponentsHandler();
        /**
         * load the first "map"
         */
        BasicSceneSwitchHandler handler = globalComponentsHandler.GetComponent<BasicSceneSwitchHandler>();
        handler.warpToZone(new WarpTarget(handler.getZones()[0], new Vector3(0, 1.5f, 0)));
    }
    /**
     * Creates a global gameobject that will be persistent across scenes to hold global Components
     *
     * this function should not add nothing more than the SceneSwitchHandler to it,
     * every other component should be added by the SceneSwitchHandler instead
     */
    GameObject createGlobalComponentsHandler() {
        GameObject ret = new GameObject(BasicSceneSwitchHandler.GLOBAL_COMPONENTS_HANDLER_NAME, new System.Type[] {
            typeof(BasicSceneSwitchHandler)
            // read the comment above before you'd add anything here
        });
        Object.DontDestroyOnLoad(ret);
        return ret;
    }
}
