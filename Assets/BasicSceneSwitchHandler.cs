using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSceneSwitchHandler : MonoBehaviour
{
    GameObject globalComponentsHandler;
    List<GameObject> warpPads = new List<GameObject>();
    GameObject warpPadTemplate;
    void Start()
    {
        globalComponentsHandler = createGlobalComponentsHandler();
        this.warpPadTemplate = loadGameObjectFromResource("warppad/warppad");
        this.warpPadTemplate.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        placeWarppad();
    }

    GameObject createGlobalComponentsHandler() {
        return new GameObject("global components handler", new System.Type[] {
            typeof(FPSDisplay),
            typeof(BasicEnemySpawner),
            typeof(EnemyHpTopbarDisplay),
            typeof(BasicMapGenerator)
        });

    }

    void placeWarppad() {
        warpPads.Add(Instantiate (warpPadTemplate, new Vector3(-10, 0, -10), Quaternion.identity));
    }

    Scene createOrFindSceneByName(string name) {
        Scene ret = SceneManager.GetSceneByName(name);
        if (!ret.IsValid()){
            ret = SceneManager.CreateScene(name);
        }
        return ret;
    }

    public void teleportToBigHouse() {
        Scene scene = createOrFindSceneByName("bigHouse");

        GameObject player = GameObject.Find("Player");
        GameObject global = GameObject.Find("global components handler");
        global.GetComponent<BasicEnemySpawner>().removeAllEnemies();

        Object.DontDestroyOnLoad(player);
        Object.DontDestroyOnLoad(global);
        SceneManager.MoveGameObjectToScene(global, scene);
        SceneManager.MoveGameObjectToScene(player, scene);
        Scene previousScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(scene);
        SceneManager.UnloadSceneAsync(previousScene);


        GameObject bigHouse = Instantiate(loadGameObjectFromResource("big_house/big_house"), new Vector3(0, 0, 0), Quaternion.identity);
        bigHouse.transform.eulerAngles = new Vector3(0, 90, 0);

        GameObject lampObj = new GameObject("Lamp");
        lampObj.transform.parent = player.transform;
        lampObj.transform.localPosition = new Vector3(0,0,0);
        Light light = lampObj.AddComponent<Light>();
        light.type = LightType.Spot;
        light.range = 30.0f;
    }

    public List<string> getZones() {
        List<string> ret = new List<string>();
        ret.Add("bigHouse");
        ret.Add("SampleScene");
        return ret;
    }

    public void onWarpPadClick(GameObject warpPad) {
        teleportToBigHouse();
    }

    public void warpToZone(string zoneName, WarpTarget target = null){
        if (!getZones().Contains(zoneName)) {
            throw new System.ArgumentException(System.String.Format("invalid zoneName ({0})", zoneName), "zoneName");
        }
        Scene currentScene = SceneManager.GetActiveScene();
        Scene targetScene = createOrFindSceneByName(zoneName);
        if (!currentScene.Equals(targetScene)){

        }
    }

    protected void switchScenes(Scene currentScene, Scene targetScene) {
        GameObject player = GameObject.Find("Player");
        GameObject global = GameObject.Find("global components handler");

        Object.DontDestroyOnLoad(player);
        Object.DontDestroyOnLoad(global);
        SceneManager.MoveGameObjectToScene(global, targetScene);
        SceneManager.MoveGameObjectToScene(player, targetScene);
    }

    GameObject loadGameObjectFromResource(string resourceName){
        GameObject ret = Resources.Load(resourceName) as GameObject;
        if (ret == null) {
            throw new System.ArgumentException(System.String.Format("unable to load resource ({0})", resourceName), "resourceName");
        }
        return ret;
    }
}
