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

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("CanvasManager").GetComponent<MenuScript>();

        currentSelection = MenuSelection.NULL;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 stickPos;
        stickPos.x = Input.GetAxis("LeftStickX");
        stickPos.y = -Input.GetAxis("LeftStickY");

        if (stickPos.x > 0.1f && stickPos.y > stickPos.x * 0.5f)
        {
            currentSelection = MenuSelection.LEVELSELCT;
        }
        else if (stickPos.x > 0.1f && stickPos.y < stickPos.x * 0.5f && stickPos.y > -stickPos.x)
        {
            currentSelection = MenuSelection.OPTIONS;
        }
        else if (stickPos.y < -0.1f && stickPos.x > stickPos.y && stickPos.x < -stickPos.y)
        {
            currentSelection = MenuSelection.EXIT;
        }
        else if (stickPos.x < -0.1f && stickPos.y > stickPos.x && stickPos.y < stickPos.x * 0.5f)
        {
            currentSelection = MenuSelection.CREDITS;
        }
        else if (stickPos.x < -0.1f && stickPos.y > stickPos.x * 0.5f)
        {
            currentSelection = MenuSelection.DATABASE;
        }


        if (Input.GetButton("A"))
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
        if (Input.GetButton("B"))
        {
            manager.Load("TitleScreen");
            gameObject.SetActive(false);
        }
    }
}
