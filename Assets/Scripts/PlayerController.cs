using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using RollaBall;
using System;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI difficultyText;
    public GameObject winTextObject;
    public GameObject restartButtonObject;
    public GameObject pickUpPrefab;
    public GameObject pickUpPrefabParent;
    public GameObject playGround;
    public GameObject levelLabelText;
    public TMP_Dropdown difficultyDropdown;


    private RollaBall.Types.Level level;
    private int maxCount; //actual level depending maxCount of PickUps
    private const string pickupTagName = "PickUp";
    private Vector3 playfieldSize;

    private Rigidbody rb; //Game Ball = Player Ball
    private int count;
    private float movementX;
    private float movementY;
    private bool stop;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        level = new RollaBall.Types.Level();
        //Disable old fixed PickUps
        foreach (GameObject go in GetAllGameObjectsInCurrentScene())
          if (go.CompareTag(pickupTagName))
            go.SetActive(false);
        playfieldSize = transform.InverseTransformVector(playGround.GetComponent<Renderer>().bounds.size);  //TODO: Changes from start to restart to restart ...! Should be fixed all the time!
        //substract the walls from the outer limit and give a few more ticks to prevent object from being partialy embedded in the wall
        playfieldSize.x -= 2.5f;
        playfieldSize.z -= 2.5f;
        PopulateDifficultyDropdown();
        ResetAllComponents();
        ShowChooseDifficulty();
    }
    private void PopulateDifficultyDropdown()
    {
        RollaBall.Types.Levels levels = new RollaBall.Types.Levels();
        difficultyDropdown.ClearOptions();
        difficultyDropdown.options.Clear();
        difficultyDropdown.AddOptions(levels.levels);
    }

    private void ShowChooseDifficulty()
    {
        difficultyDropdown.GetComponent<GameObject>().SetActive(true);
        levelLabelText.SetActive(true);
        difficultyDropdown.Show();
        restartButtonObject.SetActive(true);
    }
    private void HideChooseDifficulty()
    {
        levelLabelText.SetActive(false);
        difficultyDropdown.Hide();
        restartButtonObject.SetActive(false);
        if (difficultyDropdown != null)
        {
            difficultyDropdown.GetComponent<GameObject>().SetActive(false);
        }
    }
    public void difficultyDropDown_onValueChanged()
    {
        level.ChangeDifficulty(Convert.ToInt32(difficultyDropdown.captionText));
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString() + "/" + maxCount;
        if(count >= maxCount)
        {
            winTextObject.SetActive(true);
            restartButtonObject.SetActive(true);
            ShowChooseDifficulty();
            StopMovement(rb);
        }
    }
    private void SetDifficultyText()
    {
        difficultyText.text = "Difficulty: " + level.difficulty;
    }

    private void StopMovement(Rigidbody rb)
    {
        stop = true;
        movementX = 0;
        movementY = 0;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void RestartClick()
    {
        ResetAllComponents();
    }

    private void ResetAllComponents()
    {
        maxCount = UnityEngine.Random.Range(level.minCount, level.maxCount + 1);
        //Vector3 playfieldSize = transform.InverseTransformVector(playGround.GetComponent<Renderer>().bounds.size);  //TODO: Changes from start to restart to restart ...! Should be fixed all the time!
        for (int i = 1; i <= maxCount; i++)
        {
            float x = UnityEngine.Random.Range((-playfieldSize.x/2), (playfieldSize.x/2));
            float z = UnityEngine.Random.Range((-playfieldSize.z/2), (playfieldSize.z/2));
            Debug.Log(playfieldSize);
            GameObject go = Instantiate(pickUpPrefab, new Vector3(x, 0.5f, z), Quaternion.identity, pickUpPrefabParent.transform);
            go.SetActive(true);
        }

        //foreach (GameObject go in GetAllGameObjectsInCurrentScene())
          //  if (go.CompareTag(pickupTagName))
            //    go.SetActive(true);

        count = 0;

        HideChooseDifficulty();

        SetCountText();
        SetDifficultyText();        
        winTextObject.SetActive(false);
        restartButtonObject.SetActive(false);
        levelLabelText.SetActive(false);        
        stop = false;
    }

    private List<GameObject> GetAllGameObjectsInCurrentScene()
    {
        List<GameObject> allGOs = new List<GameObject>();
        GameObject[] rootGOs = gameObject.scene.GetRootGameObjects();
        for (int i = 0; i < rootGOs.Length; i++)
            GetAllChildren(rootGOs[i].transform, allGOs);
        return allGOs;
    }

    private void GetAllChildren(Transform current, List<GameObject> arrayToFill)
    {
        arrayToFill.Add(current.gameObject);

        for (int i = 0; i < current.childCount; i++)
            GetAllChildren(current.GetChild(i), arrayToFill);
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        if (!stop)
            rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(pickupTagName))
        {
            //other.gameObject.SetActive(false);
            Destroy(other.gameObject);

            count++;

            SetCountText();
        }
    }
}
