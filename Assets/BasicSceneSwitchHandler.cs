using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class BasicSceneSwitchHandler : MonoBehaviour
{
    public static string GLOBAL_COMPONENTS_HANDLER_NAME = "global components handler";
    GameObject globalComponentsHandler;
    GameObject player;
    GameObject warpPadTemplate;

    List<WorldZone> zones;

    /**
     * did the init() function run already?
     */
    bool inited = false;
    void Start()
    {
        /**
         * this was probably already ran by warpToZone() but it doesn't hurt to run it here, just in case
         */
        this.init();
    }

    void init() {
        if (this.inited) {
            return;
        }
        this.inited = true;
        this.globalComponentsHandler = this.attachGlobalComponents(this.gameObject);
        this.player = createPlayer();

        this.warpPadTemplate = this.loadGameObjectFromResource("warppad/warppad");
        this.warpPadTemplate.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
    }

    protected GameObject attachGlobalComponents(GameObject globalComponentsHandler) {
        globalComponentsHandler.AddComponent(typeof(FPSDisplay));
        globalComponentsHandler.AddComponent(typeof(EnemyHpTopbarDisplay));
        return globalComponentsHandler;
    }

    protected void tmpCreateBigHouseScene() {
        GameObject bigHouse = Instantiate(loadGameObjectFromResource("big_house/big_house"), new Vector3(0, 0, 0), Quaternion.identity);
        bigHouse.transform.eulerAngles = new Vector3(0, 90, 0);

        List<WorldZone> zones = this.getZones();

        (Instantiate(this.warpPadTemplate, new Vector3(-10, 0, -10), Quaternion.identity)
            .AddComponent(typeof(WarpPadWarpTargetHandler)) as WarpPadWarpTargetHandler)
            .warpTarget = new WarpTarget(zones[1], new Vector3(0, 1.5f, 0));
    }

    protected void tmpCreateSampleScene(){
        new GameObject("local map related stuff", new System.Type[]{
            typeof(BasicMapGenerator),
            typeof(BasicEnemySpawner),
        });

        List<WorldZone> zones = this.getZones();

        (Instantiate(this.warpPadTemplate, new Vector3(-10, 0, -10), Quaternion.identity)
            .AddComponent(typeof(WarpPadWarpTargetHandler)) as WarpPadWarpTargetHandler)
            .warpTarget = new WarpTarget(zones[0], new Vector3(0, 1.5f, 0));

        (Instantiate(this.warpPadTemplate, new Vector3(15, 0, -15), Quaternion.identity)
            .AddComponent(typeof(WarpPadWarpTargetHandler)) as WarpPadWarpTargetHandler)
            .warpTarget = new WarpTarget(zones[1], new Vector3(5, 0, -15));

        (Instantiate(this.warpPadTemplate, new Vector3(5, 0, -15), Quaternion.identity)
            .AddComponent(typeof(WarpPadWarpTargetHandler)) as WarpPadWarpTargetHandler)
            .warpTarget = new WarpTarget(zones[1], new Vector3(15, 0, -15));
    }

    public GameObject getPlayer() {
        return this.player;
    }

    IEnumerator createOrLoadSceneByName(string name, System.Action<Scene> resultCallback) {
        AsyncOperation asyncLoad = this.loadSceneByNameIfNecessary(name);
        if (asyncLoad != null) {
            while (!asyncLoad.isDone /*&& asyncLoad.progress < 0.9f*/)
            {
                yield return null;
            }
            // asyncLoad.allowSceneActivation = true;
        }
        Scene ret = SceneManager.GetSceneByName(name);
        if (!ret.IsValid()){
            ret = SceneManager.CreateScene(name);
        }
        resultCallback(ret);
        yield return null;
    }

    protected AsyncOperation loadSceneByNameIfNecessary(string name) {
        AsyncOperation ret = null;
        Scene scene = SceneManager.GetSceneByName(name);
        if (scene.IsValid()){
            /**
             * we already have that scene loaded, no need to load it again
             */
            return null;
        }
        try {
            ret = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            // ret.allowSceneActivation = false;
        } catch (System.Exception e) {
            Debug.LogException(e);
        }
        return ret;
    }
    /**
     * create the player GameObject which (for now) will be persistent in all scenes
     */
    GameObject createPlayer() {
        GameObject ret = Instantiate(loadGameObjectFromResource("player"), new Vector3(0, 0, 0), Quaternion.identity);
        Object.DontDestroyOnLoad(ret);

        ret.AddComponent(typeof(PlayerInteractions));

        GameObject lampObj = new GameObject("Lamp");
        lampObj.transform.parent = ret.transform;
        lampObj.transform.localPosition = new Vector3(0,0,0);
        Light light = lampObj.AddComponent<Light>();
        light.type = LightType.Spot;
        light.range = 30.0f;
        return ret;
    }

    public List<WorldZone> getZones() {
        if (this.zones == null) {
            this.zones = new List<WorldZone>();
            this.zones.Add(new WorldZone("bigHouse"));
            this.zones.Add(new WorldZone("SampleScene"));
        }
        return this.zones;
    }

    public void onWarpPadClick(GameObject warpPad) {
        Scene currentScene = SceneManager.GetActiveScene();

        WarpPadWarpTargetHandler handler = warpPad.GetComponent<WarpPadWarpTargetHandler>();

        if (handler != null && handler.warpTarget != null) {
            this.warpToZone(handler.warpTarget);
        }

    }

    public void warpToZone(WarpTarget target){
        if (!getZones().Contains(target.getZone())) {
            throw new System.ArgumentException(System.String.Format("invalid zoneName ({0})", target.getZone().sceneName), "zoneName");
        }
        this.init();
        Scene currentScene = SceneManager.GetActiveScene();
        StartCoroutine(createOrLoadSceneByName(target.getZone().sceneName, delegate(Scene scene) {
            Scene targetScene = scene;
            if (!currentScene.Equals(targetScene)){
                this.switchScenes(currentScene, targetScene);
            } else {

            }
            this.movePlayerToTarget(this.player, target);
        }));
    }

    protected void movePlayerToTarget(GameObject player, WarpTarget target) {
        player.transform.position = target.getTargetCoordinates();
    }

    protected void switchScenes(Scene currentScene, Scene targetScene) {
        if (globalComponentsHandler != null) {
            BasicEnemySpawner spawner = globalComponentsHandler.GetComponent<BasicEnemySpawner>();
            if (spawner != null) {
                spawner.removeAllEnemies();
            }
            SceneManager.MoveGameObjectToScene(globalComponentsHandler, targetScene);
        }
        if (player != null) {
            SceneManager.MoveGameObjectToScene(player, targetScene);
        }
        SceneManager.SetActiveScene(targetScene);
        SceneManager.UnloadSceneAsync(currentScene);
        NavMesh.RemoveAllNavMeshData();
        this.loadSceneObjects(targetScene);
    }
    /**
     * load the custom stuff that we need for a given scene, like gameobjects or components
     */
    protected void loadSceneObjects(Scene scene) {
        if (scene.name == "bigHouse") {
            this.tmpCreateBigHouseScene();
        }
        if (scene.name == "SampleScene") {
            this.tmpCreateSampleScene();
        }
    }

    GameObject loadGameObjectFromResource(string resourceName){
        GameObject ret = Resources.Load(resourceName) as GameObject;
        if (ret == null) {
            throw new System.ArgumentException(System.String.Format("unable to load resource ({0})", resourceName), "resourceName");
        }
        return ret;
    }
}
