using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

    public GameObject TitleScreen;
    public GameObject MainMenu;
    public GameObject LevelSelect;
    public GameObject LevelSelect_List;

    public GameObject camera;
    public Transform targetCameraPos;

    public Transform[] cameraPositions;

    void Start()
    {
        TitleScreen = GameObject.Find("Cnv_TitleScreen");
        MainMenu = GameObject.Find("Cnv_MainMenu");
        LevelSelect = GameObject.Find("Cnv_LevelSelect");
        LevelSelect_List = GameObject.Find("Cnv_LevelSelect_List");

        camera = GameObject.Find("Main Camera");

        cameraPositions = new Transform[2];

        cameraPositions[0] = GameObject.Find("CamPos_LevelSelect").GetComponent<Transform>();
        cameraPositions[1] = GameObject.Find("CamPos_MainMenu").GetComponent<Transform>();

        MainMenu.SetActive(false);
        LevelSelect.SetActive(false);
        LevelSelect_List.SetActive(false);

        camera.transform.position = cameraPositions[1].position;
        camera.transform.rotation = cameraPositions[1].rotation;
        targetCameraPos = cameraPositions[1];
    }

    void Update()
    {
        camera.transform.position = Vector3.Lerp(camera.transform.position, targetCameraPos.position, 0.07f);
        camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, targetCameraPos.transform.rotation, 0.07f);
    }

    public void Load(string scene)
    {
        switch (scene)
        {
            case "TitleScreen":
                targetCameraPos = cameraPositions[1];
                TitleScreen.SetActive(true);
                break;

            case "MainMenu":
                targetCameraPos = cameraPositions[1];
                MainMenu.SetActive(true);
                break;

            case "LevelSelect":
                targetCameraPos = cameraPositions[0];
                LevelSelect.SetActive(true);
                break;

            case "LevelSelect_List":
                targetCameraPos = cameraPositions[0];
                LevelSelect_List.SetActive(true);
                break;

            default:
                Debug.LogError("invalid level name encountered");
                break;
        }
    }
}
