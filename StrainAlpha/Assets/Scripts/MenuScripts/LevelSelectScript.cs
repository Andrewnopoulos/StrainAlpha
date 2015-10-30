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
	public GameObject backSound;
    public GameObject acceptSound;
    public GameObject switchSound;
    public MenuScript manager;

    public GameObject levelData;

    private LevelSelection currentSelectedLevel;
    private DifficultySelection currentSelectedDifficulty;

    private LevelSelection hoveredOverLevel;
    private DifficultySelection hoveredOverDifficulty;

	private Vector3[] levelPanelPositions;

	private GameObject LevelBackLight;

	private float selectionCooldown = 0.0f;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("CanvasManager").GetComponent<MenuScript>();

        hoveredOverLevel = LevelSelection.LEVEL1;
        hoveredOverDifficulty = DifficultySelection.EASY;

        currentSelectedLevel = LevelSelection.NULL;
        currentSelectedDifficulty = DifficultySelection.NULL;

		levelPanelPositions = new Vector3[6];

		levelPanelPositions[0] = new Vector3(-270, 250, 0);
		levelPanelPositions[1] = new Vector3(270, 250, 0);
		levelPanelPositions[2] = new Vector3(-430, 0, 0);
		levelPanelPositions[3] = new Vector3(430, 0, 0);
		levelPanelPositions[4] = new Vector3(-270, -250, 0);
		levelPanelPositions[5] = new Vector3(270, -250, 0);

		LevelBackLight = GameObject.Find ("LevelBackLight");
    }

    // Update is called once per frame
    void Update()
    {
		selectionCooldown -= Time.deltaTime;

        Vector2 stickPos;
        stickPos.x = Input.GetAxis("LeftStickX");
        stickPos.y = -Input.GetAxis("LeftStickY");

		if ((stickPos.y > 0.1f || Input.GetKey(KeyCode.W)) && selectionCooldown <= 0.0f)
        {
            Instantiate(switchSound);
            FlickUp();
			selectionCooldown = 0.2f;
        }
		if ((stickPos.y < -0.1f || Input.GetKey(KeyCode.S)) && selectionCooldown <= 0.0f)
        {
            Instantiate(switchSound);
            FlickDown();
			selectionCooldown = 0.2f;
        }

        if (Input.GetButtonDown("A") || Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(acceptSound);

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
				if (hoveredOverDifficulty == DifficultySelection.EASY)
				{
					LevelBackLight.transform.localPosition = levelPanelPositions[1];
				}
				else if (hoveredOverDifficulty == DifficultySelection.MEDIUM)
				{
					LevelBackLight.transform.localPosition = levelPanelPositions[3];
				}
				else
				{
					LevelBackLight.transform.localPosition = levelPanelPositions[5];
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
                GameObject persistentObject = (GameObject)Instantiate(levelData);
                PersistentData data = persistentObject.GetComponent<PersistentData>();
                data.level = currentSelectedLevel;
                data.difficulty = currentSelectedDifficulty;

                if (currentSelectedLevel == LevelSelection.LEVEL1)
                    Application.LoadLevel("TestScene");
                else if (currentSelectedLevel == LevelSelection.LEVEL2)
                    Application.LoadLevel("Level 1");
                else if (currentSelectedLevel == LevelSelection.LEVEL3)
                    Application.LoadLevel("Level 2");
            }
        }

        if (Input.GetButtonDown("B") || Input.GetKeyDown(KeyCode.Backspace))
        {
			Instantiate(backSound);

            if (currentSelectedLevel == LevelSelection.NULL)
            {
                manager.Load("MainMenu");
                gameObject.SetActive(false);
            }
            else
            {
                currentSelectedLevel = LevelSelection.NULL;
				if (hoveredOverLevel == LevelSelection.LEVEL1)
				{
					LevelBackLight.transform.localPosition = levelPanelPositions[0];
				}
				else if (hoveredOverLevel == LevelSelection.LEVEL2)
				{
					LevelBackLight.transform.localPosition = levelPanelPositions[2];
				}
				else
				{
					LevelBackLight.transform.localPosition = levelPanelPositions[4];
				}
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
				LevelBackLight.transform.localPosition = levelPanelPositions[4];
            }
            else if (hoveredOverLevel == LevelSelection.LEVEL2)
            {
                hoveredOverLevel = LevelSelection.LEVEL1;
				LevelBackLight.transform.localPosition = levelPanelPositions[0];
            }
            else if (hoveredOverLevel == LevelSelection.LEVEL3)
            {
                hoveredOverLevel = LevelSelection.LEVEL2;
				LevelBackLight.transform.localPosition = levelPanelPositions[2];
            }
        }
        else
        {
            if (hoveredOverDifficulty == DifficultySelection.EASY)
            {
                hoveredOverDifficulty = DifficultySelection.HARD;
				LevelBackLight.transform.localPosition = levelPanelPositions[5];
            }
            else if (hoveredOverDifficulty == DifficultySelection.MEDIUM)
            {
                hoveredOverDifficulty = DifficultySelection.EASY;
				LevelBackLight.transform.localPosition = levelPanelPositions[1];
            }
            else if (hoveredOverDifficulty == DifficultySelection.HARD)
            {
                hoveredOverDifficulty = DifficultySelection.MEDIUM;
				LevelBackLight.transform.localPosition = levelPanelPositions[3];
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
				LevelBackLight.transform.localPosition = levelPanelPositions[2];
            }
            else if (hoveredOverLevel == LevelSelection.LEVEL2)
            {
                hoveredOverLevel = LevelSelection.LEVEL3;
				LevelBackLight.transform.localPosition = levelPanelPositions[4];
            }
            else if (hoveredOverLevel == LevelSelection.LEVEL3)
            {
                hoveredOverLevel = LevelSelection.LEVEL1;
				LevelBackLight.transform.localPosition = levelPanelPositions[0];
            }
        }
        else
        {
            if (hoveredOverDifficulty == DifficultySelection.EASY)
            {
                hoveredOverDifficulty = DifficultySelection.MEDIUM;
				LevelBackLight.transform.localPosition = levelPanelPositions[3];
            }
            else if (hoveredOverDifficulty == DifficultySelection.MEDIUM)
            {
                hoveredOverDifficulty = DifficultySelection.HARD;
				LevelBackLight.transform.localPosition = levelPanelPositions[5];
            }
            else if (hoveredOverDifficulty == DifficultySelection.HARD)
            {
                hoveredOverDifficulty = DifficultySelection.EASY;
				LevelBackLight.transform.localPosition = levelPanelPositions[1];
            }
        }
    }
}
