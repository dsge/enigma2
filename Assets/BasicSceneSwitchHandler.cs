using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSceneSwitchHandler : MonoBehaviour
{
    public static string GLOBAL_COMPONENTS_HANDLER_NAME = "global components handler";
    GameObject globalComponentsHandler;
    GameObject player;
    List<GameObject> warpPads = new List<GameObject>();
    GameObject warpPadTemplate;

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
        this.warpPadTemplate = this.loadGameObjectFromResource("warppad/warppad");
        this.warpPadTemplate.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        this.placeWarppad();
    }

    void init() {
        if (this.inited) {
            return;
        }
        this.inited = true;
        this.globalComponentsHandler = this.attachGlobalComponents(this.gameObject);
        this.player = createPlayer();
    }

    protected GameObject attachGlobalComponents(GameObject globalComponentsHandler) {
        globalComponentsHandler.AddComponent(typeof(FPSDisplay));
        globalComponentsHandler.AddComponent(typeof(EnemyHpTopbarDisplay));
        return globalComponentsHandler;
    }

    protected void tmpCreateBigHouseScene() {
        GameObject bigHouse = Instantiate(loadGameObjectFromResource("big_house/big_house"), new Vector3(0, 0, 0), Quaternion.identity);
        bigHouse.transform.eulerAngles = new Vector3(0, 90, 0);
    }

    protected void tmpCreateSampleScene(){
        new GameObject("local map related stuff", new System.Type[]{
            typeof(BasicMapGenerator),
            typeof(BasicEnemySpawner),
        });
    }

    public GameObject getPlayer() {
        return this.player;
    }

    void placeWarppad() {
        warpPads.Add(Instantiate (warpPadTemplate, new Vector3(-10, 0, -10), Quaternion.identity));
    }

    IEnumerator createOrLoadSceneByName(string name, System.Action<Scene> resultCallback) {
        AsyncOperation asyncLoad = null;
        try {
            asyncLoad = SceneManager.LoadSceneAsync(name);
            asyncLoad.allowSceneActivation = false;
        } catch (System.Exception e) {
            Debug.Log("foo");
        }
        if (asyncLoad != null) {
            while (!asyncLoad.isDone && asyncLoad.progress < 0.9f)
            {
                yield return null;
            }
        }
        Scene ret = SceneManager.GetSceneByName(name);
        if (!ret.IsValid()){
            ret = SceneManager.CreateScene(name);
        }
        resultCallback(ret);
        yield return null;
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

    public List<string> getZones() {
        List<string> ret = new List<string>();
        ret.Add("bigHouse");
        ret.Add("SampleScene");
        return ret;
    }

    public void onWarpPadClick(GameObject warpPad) {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name != "bigHouse") {
            warpToZone("bigHouse");
        } else {
            warpToZone("SampleScene");
        }
    }

    public void warpToZone(string zoneName, WarpTarget target = null){
        if (!getZones().Contains(zoneName)) {
            throw new System.ArgumentException(System.String.Format("invalid zoneName ({0})", zoneName), "zoneName");
        }
        this.init();
        Scene currentScene = SceneManager.GetActiveScene();
        StartCoroutine(createOrLoadSceneByName(zoneName, delegate(Scene scene) {
            Scene targetScene = scene;
            if (!currentScene.Equals(targetScene)){
                this.switchScenes(currentScene, targetScene);
            } else {

            }
            if (target == null) {
                target = new WarpTarget(new Vector3(0, 3, 0));
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
        this.loadScene(targetScene);
    }
    /**
     * load the custom stuff that we need for a given scene, like gameobjects or components
     */
    protected void loadScene(Scene scene) {
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
