using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;

    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }
    [SerializeField] private Season season = Season.Fall;

    public Vector3 playerPosition;
    public List<string> itemList = new List<string>();
    [SerializeField] private GameObject itemPrefab;
    private Transform itemsParent;

    [SerializeField] bool refuseTeleport = false;
    [SerializeField] bool playerEntered = false;

    [Header("UI")]
    [SerializeField] private Canvas targetCanvas;
    [SerializeField] public Text uppertext;
    [SerializeField] public Text door;
    [SerializeField] public Text playerSpeaking;
    [SerializeField] public Text warning;

    [Header(" - Puzzle Photo - ")]
    [SerializeField] private GameObject scarecrowPuzzlePhoto;
    [SerializeField] private GameObject snowmanPuzzlePhoto;
    [SerializeField] private GameObject birdPuzzlePhoto;
    [SerializeField] private GameObject picnicPuzzlePhoto;
    [Space]
    [SerializeField] private GameObject mePuzzlePhoto;
    [SerializeField] private GameObject momPuzzlePhoto;
    [SerializeField] private GameObject dadPuzzlePhoto;
    [SerializeField] private GameObject dogPuzzlePhoto;

    public bool pickedCarrot = false;
    public bool pickedBoots = false;

    private bool firstScene = true;

    public bool birdPhotoFind = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Start()
    {
        scarecrowPuzzlePhoto.SetActive(false);
        snowmanPuzzlePhoto.SetActive(false);
        birdPuzzlePhoto.SetActive(false);
        picnicPuzzlePhoto.SetActive(false);

        mePuzzlePhoto.SetActive(false); 
        momPuzzlePhoto.SetActive(false);    
        dadPuzzlePhoto.SetActive(false);
        dogPuzzlePhoto.SetActive(false);

        playerSpeaking.enabled = false;
        warning.enabled = false;
    }
    public void SwitchSeason(Season newSeason)
    {
        switch (newSeason)
        {
            case Season.Spring:
                SceneManager.LoadScene("_Spring");
                season = Season.Spring;
                break;
            case Season.Summer:
                SceneManager.LoadScene("_Summer");
                season = Season.Summer;
                break;
            case Season.Fall:
                SceneManager.LoadScene("_Fall");
                season = Season.Fall;
                break;
            case Season.Winter:
                SceneManager.LoadScene("_Winter");
                season = Season.Winter;
                break;
            default:
                Debug.LogError("Invalid season specified.");
                break;
        }
    }

    public Season getCurSeason()
    {
        return season;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //if (!uppertext)
        //{
        //    uppertext = GameObject.FindWithTag("UpperText").GetComponent<Text>();
        //}
        //if (!targetCanvas)
        //{
        //    targetCanvas = GameObject.FindWithTag("MainUI").GetComponent<Canvas>();
        //}

        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            if (!refuseTeleport)
            {
                player.transform.position = GameManager.Instance.playerPosition;
            }
            
        }

        if(pickedCarrot)
        {
            //remove carrot
            GameObject carrot = GameObject.FindWithTag("Carrot");
            Destroy(carrot);
        }

        if (pickedBoots)
        {
            //remove boots
            GameObject boots = GameObject.FindWithTag("Boots");
            Destroy(boots);
        }

        if (birdPhotoFind)
        {
            //remove boots
            GameObject birdPhoto = GameObject.FindWithTag("BirdPhoto");
            Destroy(birdPhoto);
        }



        // UI
        if (targetCanvas != null)
        {
            itemsParent = targetCanvas.transform.Find("Items");
            if (itemsParent != null)
            {
                if (itemList.Count > 0)
                {
                    //CreateItemUI();
                }
            }
            else
            {
                Debug.LogError("Items parent not found on the target canvas.");
            }
        }

        if (!firstScene)
        {
            switch (season)
            {
                case Season.Spring:
                    playerSpeaking.color = new Color(102f / 255f, 255f / 255f, 102f / 255f);
                    playerSpeaking.text = "I like the bloom";
                    playerSpeaking.enabled = true;
                    StartCoroutine(WaitAndExecute(() => playerSpeaking.enabled = false, 2f));
                    break;
                case Season.Summer:
                    playerSpeaking.color = new Color(255f / 255f, 255f / 255f, 102f / 255f);
                    playerSpeaking.text = "Yay, it's so sunny and warm";
                    playerSpeaking.enabled = true;
                    StartCoroutine(WaitAndExecute(() => playerSpeaking.enabled = false, 2f));
                    break;
                case Season.Fall:
                    playerSpeaking.color = new Color(255f / 255f, 165f / 255f, 0f / 255f);
                    playerSpeaking.text = "Red, yellow, and orange";
                    playerSpeaking.enabled = true;
                    StartCoroutine(WaitAndExecute(() => playerSpeaking.enabled = false, 2f));
                    break;
                case Season.Winter:
                    playerSpeaking.color = new Color(0f / 255f, 128f / 255f, 255f / 255f);
                    playerSpeaking.text = "Wow, this is getting cold";
                    playerSpeaking.enabled = true;
                    StartCoroutine(WaitAndExecute(() => playerSpeaking.enabled = false, 2f));
                    break;
            }
        }

        firstScene = false;

        
    }

    IEnumerator WaitAndExecute(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (!playerEntered)
        {
            uppertext.enabled = false;
        }
    }

    // Items Logic
    public void AddItem(string newItem)
    {
        itemList.Add(newItem);
        Debug.Log(newItem + " added to the list.");

        if (targetCanvas != null)
        {
            itemsParent = targetCanvas.transform.Find("Items");
            if (itemsParent != null)
            {
                CreateItemUI();
            }
            else
            {
                Debug.LogError("Items parent not found on the target canvas.");
            }
        }
    }

    public void UseItem(string useItem)
    {
        if (!itemList.Contains(useItem))
        {
            Debug.Log("Try to use not having item: " + useItem);
            //show text to upper ui
            uppertext.text = "You don't have " + useItem;
            uppertext.enabled = true;
            StartCoroutine(DisableUpperText());
            return;
        }
        if (useItem == "Boots")
        {

        }
        else
        {
            itemList.Remove(useItem);
            Debug.Log(useItem + " used.");

            // Find the item object in the scene
            GameObject itemObject = GameObject.Find(useItem);

            if (itemObject != null)
            {
                itemObject.SendMessage("SetQuantity", 0, SendMessageOptions.RequireReceiver);
            }
        }
        

        
    }

    private IEnumerator DisableUpperText()
    {
        yield return new WaitForSeconds(2);
        uppertext.enabled = false;
    }

    private void CreateItemUI()
    {
        // Instantiate a new itemPrefab as a child of itemsParent
        GameObject newItemObject = Instantiate(itemPrefab);
        newItemObject.transform.SetParent(itemsParent, false);
        Text itemNameText = newItemObject.transform.Find("Item Name").GetComponent<Text>();
        newItemObject.name = itemList[itemList.Count - 1];

        if (itemNameText != null && itemList.Count > 0)
        {
            // Set the text of the UI element to the last item in the itemList
            itemNameText.text = itemList[itemList.Count - 1];
        }
    }

    public bool checkIfItemAlreadyCollected(string name)
    {
        for(int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] == name)
            {
                return true;
            }
        }
        return false;
    }

    public void PuzzlePhotoActive(string photoName)
    {
        if (photoName != null)
        {

            switch (photoName)
            {
                case "scarecrow":
                    scarecrowPuzzlePhoto.SetActive(true);
                    break;
                case "snowman":
                    snowmanPuzzlePhoto.SetActive(true);
                    break;
                case "bird":
                    birdPuzzlePhoto.SetActive(true);
                    break;
                case "picnic":
                    picnicPuzzlePhoto.SetActive(true);
                    break;
                default:
                    Debug.LogError("Invalid photoName specified.");
                    break;
            }          
        }
    }

    public void Dialog(string dialogName)
    {
        if (playerEntered && dialogName != null)
        {
            switch(dialogName)
            {
                case ("scarecrowNotSolved"):
                    uppertext.text = "Hi, I am Scarecrow, can you bring me a special flower?";
                    uppertext.enabled = true;
                    break;
                case ("scarecrowSolved"):
                    uppertext.text = "Thank you!";
                    uppertext.enabled = true;
                    break;
                case ("snowmanNotSolved"):
                    uppertext.text = "Hi, I am Snowman, can you bring me a carrot?";
                    uppertext.enabled = true;
                    break;
                case ("snowmanSolved"):
                    uppertext.text = "Thank you!";
                    uppertext.enabled = true;
                    break;
                case ("birdNotSolved"):
                    uppertext.text = "Hi, I am bird, can you bring me corn?";
                    uppertext.enabled = true;
                    break;
                case ("birdSolved"):
                    uppertext.text = "Thank you!";
                    uppertext.enabled = true;
                    break;
                case ("picnicNotSolved"):
                    uppertext.text = "Hi, I am picnic, can you bring me ice?";
                    uppertext.enabled = true;
                    break;
                case ("picnicSolved"):
                    uppertext.text = "Thank you!";
                    uppertext.enabled = true;
                    break;
                case ("riverNotSolved"):
                    if (season == Season.Winter)
                    {
                        uppertext.text = "You need winter boots to pass through the river";
                    }
                    else
                    {
                        uppertext.text = "The water is too deep !";
                    }
                    uppertext.enabled = true;
                    break;
                case ("riverSolved"):
                    uppertext.text = "You can pass through now!";
                    uppertext.enabled = true;
                    break;
            }
        }
    }

    public void PlayerEnter()
    {
        playerEntered = true;
    }

    public void PlayerLeave()
    {
        playerEntered = false;
    }

    public void FamilyPhotoActive(string memberName)
    {
        switch (memberName)
        {
            case ("me"):
                mePuzzlePhoto.SetActive(true);
                break;
            case ("mom"):
                momPuzzlePhoto.SetActive(true);
                break;
            case ("dad"):
                dadPuzzlePhoto.SetActive(true);
                break;
            case ("dog"):
                dogPuzzlePhoto.SetActive(true);
                break;
        }

    }
}
