using UnityEngine;
using System.Collections;

public enum MenuSelection
{
    LEVELSELCT = 0,
    OPTIONS = 1,
    EXIT = 2,
    CREDITS = 3,
    DATABASE = 4,
    NULL = 5,
}

public class MainMenuScript : MonoBehaviour {

    public MenuScript manager;

    private MenuSelection currentSelection;

    private Quaternion[] rotations;

    private GameObject bg;

    private Quaternion targetRotation;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("CanvasManager").GetComponent<MenuScript>();

        currentSelection = MenuSelection.NULL;

        rotations = new Quaternion[5];

        bg = GameObject.Find("bg");

        rotations[0] = Quaternion.Euler(0, 0, -110.0f);
        rotations[1] = Quaternion.Euler(0, 0, -157.0f);
        rotations[2] = Quaternion.Euler(0, 0, -247.0f);
        rotations[3] = Quaternion.Euler(0, 0, 380.0f);
        rotations[4] = Quaternion.Euler(0, 0, -20.0f);

        bg.transform.localRotation = rotations[0];
        targetRotation = rotations[0];
    }

    // Update is called once per frame
    void Update()
    {

        bg.transform.localRotation = Quaternion.Lerp(bg.transform.localRotation, targetRotation, 0.18f);

        Vector2 stickPos;
        stickPos.x = Input.GetAxis("LeftStickX");
        stickPos.y = -Input.GetAxis("LeftStickY");

        if (stickPos.x > 0.1f && stickPos.y > stickPos.x * 0.5f)
        {
            targetRotation = rotations[0];
            currentSelection = MenuSelection.LEVELSELCT;
        }
        else if (stickPos.x > 0.1f && stickPos.y < stickPos.x * 0.5f && stickPos.y > -stickPos.x)
        {
            targetRotation = rotations[1];
            currentSelection = MenuSelection.OPTIONS;
        }
        else if (stickPos.y < -0.1f && stickPos.x > stickPos.y && stickPos.x < -stickPos.y)
        {
            targetRotation = rotations[2];
            currentSelection = MenuSelection.EXIT;
        }
        else if (stickPos.x < -0.1f && stickPos.y > stickPos.x && stickPos.y < -stickPos.x * 0.5f)
        {
            targetRotation = rotations[3];
            currentSelection = MenuSelection.CREDITS;
        }
        else if (stickPos.x < -0.1f && stickPos.y > stickPos.x * 0.5f)
        {
            targetRotation = rotations[4];
            currentSelection = MenuSelection.DATABASE;
        }


        if (Input.GetButtonDown("A"))
        {
            if (currentSelection != MenuSelection.NULL)
            {
                switch (currentSelection)
                {
                    case MenuSelection.LEVELSELCT:
                        manager.Load("LevelSelect");
                        gameObject.SetActive(false);
                        break;
                        
                    case MenuSelection.EXIT:
                        Application.Quit();
                        break;

                    default:
                        break;
                }
            }
        }
        if (Input.GetButtonDown("B"))
        {
            manager.Load("TitleScreen");
            gameObject.SetActive(false);
        }
    }
}
