using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSceneSwitchHandler : MonoBehaviour
{
    List<GameObject> warpPads = new List<GameObject>();
    GameObject warpPadTemplate;
    void Start()
    {
        this.warpPadTemplate = loadGameObjectFromResource("warppad/warppad");
        this.warpPadTemplate.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        placeWarppad();
    }

    void placeWarppad() {
        warpPads.Add(Instantiate (warpPadTemplate, new Vector3(-10, 0, -10), Quaternion.identity));
    }

    public void onWarpPadClick(GameObject warpPad) {
        Scene scene = SceneManager.CreateScene("foo");
        bool asd = scene.IsValid();
        Scene previousScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(scene);
        GameObject player = GameObject.Find("Player");
        GameObject global = GameObject.Find("global components handler");
        global.GetComponent<BasicEnemySpawner>().removeAllEnemies();
        Object.DontDestroyOnLoad(player);
        Object.DontDestroyOnLoad(global);
        SceneManager.MoveGameObjectToScene(global, scene);
        SceneManager.MoveGameObjectToScene(player, scene);
        SceneManager.UnloadSceneAsync(previousScene);

        GameObject bigHousePrefab = Resources.Load("big_house/big_house") as GameObject;

        GameObject bigHouse = Instantiate(bigHousePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        bigHouse.transform.eulerAngles = new Vector3(0, 90, 0);

        GameObject lampObj = new GameObject("Lamp");
        lampObj.transform.parent = player.transform;
        lampObj.transform.localPosition = new Vector3(0,0,0);
        Light light = lampObj.AddComponent<Light>();
        light.type = LightType.Spot;
        light.range = 30.0f;
    }

    GameObject loadGameObjectFromResource(string resourceName){
        GameObject ret = Resources.Load(resourceName) as GameObject;
        if (ret == null) {
            throw new System.ArgumentException(System.String.Format("unable to load resource ({0})", resourceName), "resourceName");
        }
        return ret;
    }
}
