using UnityEngine;
using System.Collections;

public enum DifficultySelection
{
    EASY = 0,
    MEDIUM = 1,
    HARD = 2,
    NULL = 3,
}
public enum LevelSelection
{
    LEVEL1 = 0,
    LEVEL2 = 1,
    LEVEL3 = 2,
    NULL = 3,
}

public class LevelSelectScript : MonoBehaviour {

    public MenuScript manager;

    private LevelSelection currentSelectedLevel;
    private DifficultySelection currentSelectedDifficulty;

    private LevelSelection hoveredOverLevel;
    private DifficultySelection hoveredOverDifficulty;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("CanvasManager").GetComponent<MenuScript>();

        hoveredOverLevel = LevelSelection.LEVEL1;
        hoveredOverDifficulty = DifficultySelection.EASY;

        currentSelectedLevel = LevelSelection.NULL;
        currentSelectedDifficulty = DifficultySelection.NULL;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 stickPos;
        stickPos.x = Input.GetAxis("LeftStickX");
        stickPos.y = -Input.GetAxis("LeftStickY");

        if (stickPos.y > 0.1f)
        {
            FlickUp();
        }
        if (stickPos.y < -0.1f)
        {
            FlickDown();
        }

        if (Input.GetButtonDown("A"))
        {
            if (currentSelectedLevel == LevelSelection.NULL)
            {
                switch (hoveredOverLevel)
                {
                    case LevelSelection.LEVEL1:
                        currentSelectedLevel = LevelSelection.LEVEL1;
                        break;

                    case LevelSelection.LEVEL2:
                        currentSelectedLevel = LevelSelection.LEVEL2;
                        break;

                    case LevelSelection.LEVEL3:
                        currentSelectedLevel = LevelSelection.LEVEL3;
                        break;

                    case LevelSelection.NULL:
                        break;

                    default:
                        break;
                }
            }
            else if (currentSelectedLevel != LevelSelection.NULL)
            {
                switch (hoveredOverDifficulty)
                {
                    case DifficultySelection.EASY:
                        currentSelectedDifficulty = DifficultySelection.EASY;
                        break;

                    case DifficultySelection.MEDIUM:
                        currentSelectedDifficulty = DifficultySelection.MEDIUM;
                        break;

                    case DifficultySelection.HARD:
                        currentSelectedDifficulty = DifficultySelection.HARD;
                        break;

                    case DifficultySelection.NULL:
                        break;

                    default:
                        break;
                }
            }

            if (currentSelectedDifficulty != DifficultySelection.NULL)
            {
                //start level with chosen attributes
                Application.LoadLevel("TestScene");
            }
        }

        if (Input.GetButtonDown("B"))
        {
            if (currentSelectedLevel == LevelSelection.NULL)
            {
                manager.Load("MainMenu");
                gameObject.SetActive(false);
            }
            else
            {
                currentSelectedLevel = LevelSelection.NULL;
            }
        }
    }

    void FlickUp()
    {
        if (currentSelectedLevel == LevelSelection.NULL)
        {
            if (hoveredOverLevel == LevelSelection.LEVEL1)
            {
                hoveredOverLevel = LevelSelection.LEVEL3;
            }
            else if (hoveredOverLevel == LevelSelection.LEVEL2)
            {
                hoveredOverLevel = LevelSelection.LEVEL1;
            }
            else if (hoveredOverLevel == LevelSelection.LEVEL3)
            {
                hoveredOverLevel = LevelSelection.LEVEL2;
            }
        }
        else
        {
            if (hoveredOverDifficulty == DifficultySelection.EASY)
            {
                hoveredOverDifficulty = DifficultySelection.HARD;
            }
            else if (hoveredOverDifficulty == DifficultySelection.MEDIUM)
            {
                hoveredOverDifficulty = DifficultySelection.EASY;
            }
            else if (hoveredOverDifficulty == DifficultySelection.HARD)
            {
                hoveredOverDifficulty = DifficultySelection.MEDIUM;
            }
        }
    }

    void FlickDown()
    {
        if (currentSelectedLevel == LevelSelection.NULL)
        {
            if (hoveredOverLevel == LevelSelection.LEVEL1)
            {
                hoveredOverLevel = LevelSelection.LEVEL2;
            }
            else if (hoveredOverLevel == LevelSelection.LEVEL2)
            {
                hoveredOverLevel = LevelSelection.LEVEL3;
            }
            else if (hoveredOverLevel == LevelSelection.LEVEL3)
            {
                hoveredOverLevel = LevelSelection.LEVEL1;
            }
        }
        else
        {
            if (hoveredOverDifficulty == DifficultySelection.EASY)
            {
                hoveredOverDifficulty = DifficultySelection.MEDIUM;
            }
            else if (hoveredOverDifficulty == DifficultySelection.MEDIUM)
            {
                hoveredOverDifficulty = DifficultySelection.HARD;
            }
            else if (hoveredOverDifficulty == DifficultySelection.HARD)
            {
                hoveredOverDifficulty = DifficultySelection.EASY;
            }
        }
    }
}
