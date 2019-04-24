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
    public GameObject warpPadTemplate;

    List<BasicMap> maps = new List<BasicMap>();

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

    public BasicSceneSwitchHandler init() {
        if (this.inited) {
            return this;
        }
        this.inited = true;
        this.globalComponentsHandler = this.attachGlobalComponents(this.gameObject);
        this.player = createPlayer();

        this
            .registerMap(new BigHouseMap())
            .registerMap(new SampleSceneMap());

        this.warpPadTemplate = this.loadGameObjectFromResource("warppad/warppad");
        this.warpPadTemplate.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        return this;
    }

    protected BasicSceneSwitchHandler registerMap(BasicMap map) {
        this.maps.Add(map);
        return this;
    }

    protected GameObject attachGlobalComponents(GameObject globalComponentsHandler) {
        globalComponentsHandler.AddComponent(typeof(FPSDisplay));
        globalComponentsHandler.AddComponent(typeof(EnemyHpTopbarDisplay));
        return globalComponentsHandler;
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
        GameObject ret = Instantiate(loadGameObjectFromResource("player/Player"), new Vector3(0, 0, 0), Quaternion.identity);
        Object.DontDestroyOnLoad(ret);

        ret.AddComponent(typeof(PlayerInteractions));

        GameObject lampObj = new GameObject("Lamp");
        lampObj.transform.parent = ret.transform;
        lampObj.transform.localPosition = new Vector3(0,1.5f,0);
        Light light = lampObj.AddComponent<Light>();
        light.type = LightType.Point;
        light.range = 30.0f;
        light.cullingMask &= ~Layers.CURRENT_PLAYER; //everything, except current_player
        return ret;
    }

    public List<BasicMap> getMaps() {
        return this.maps;
    }

    protected List<WorldZone> getZones() {
        List<WorldZone> ret = new List<WorldZone>();
        foreach (var map in this.maps) {
            ret.AddRange(map.getProvidedZones());
        }
        return ret;
    }

    public void onWarpPadClick(GameObject warpPad) {
        Scene currentScene = SceneManager.GetActiveScene();

        WarpPadWarpTargetHandler handler = warpPad.GetComponent<WarpPadWarpTargetHandler>();

        if (handler != null && handler.warpTarget != null) {
            this.warpToZone(handler.warpTarget);
        }

    }

    public void warpToZone(WarpTarget target){
        if (!this.getZones().Contains(target.getZone())) {
            throw new System.ArgumentException(System.String.Format("invalid zoneName ({0})", target.getZone().sceneName), "zoneName");
        }
        this.init();
        Scene currentScene = SceneManager.GetActiveScene();
        StartCoroutine(createOrLoadSceneByName(target.getZone().sceneName, delegate(Scene scene) {
            Scene targetScene = scene;
            if (!currentScene.Equals(targetScene)){
                this.switchScenes(currentScene, targetScene, target);
            } else {

            }
            this.movePlayerToTarget(this.player, target);
        }));
    }

    protected void movePlayerToTarget(GameObject player, WarpTarget target) {
        player.transform.position = target.getTargetCoordinates();
    }

    protected void switchScenes(Scene currentScene, Scene targetScene, WarpTarget target) {
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
        this.loadSceneObjects(targetScene, target);
    }
    /**
     * load the custom stuff that we need for a given scene, like gameobjects or components
     */
    protected void loadSceneObjects(Scene scene, WarpTarget target) {
        target.getZone().map.initializeScene();
    }

    GameObject loadGameObjectFromResource(string resourceName){
        GameObject ret = Resources.Load(resourceName) as GameObject;
        if (ret == null) {
            throw new System.ArgumentException(System.String.Format("unable to load resource ({0})", resourceName), "resourceName");
        }
        return ret;
    }
}
