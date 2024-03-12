using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] Text uppertext;

    [Header(" - Puzzle Photo - ")]
    [SerializeField] private GameObject scarecrowPuzzlePhoto;
    [SerializeField] private GameObject snowmanPuzzlePhoto;
    [SerializeField] private GameObject birdPuzzlePhoto;
    [SerializeField] private GameObject picnicPuzzlePhoto;

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
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            if (!refuseTeleport)
            {
                player.transform.position = GameManager.Instance.playerPosition;
            }
            
        }

        // UI
        if (targetCanvas != null)
        {
            itemsParent = targetCanvas.transform.Find("Items");
            if (itemsParent != null)
            {
                if (itemList.Count > 0)
                {
                    CreateItemUI();
                }
            }
            else
            {
                Debug.LogError("Items parent not found on the target canvas.");
            }
        }
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
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                playerPosition = player.transform.position;
            }

            SceneManager.LoadScene("BaseScene2");
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
        itemList.Remove(useItem);
        Debug.Log(useItem + " used.");

        // Find the item object in the scene
        GameObject itemObject = GameObject.Find(useItem);

        if (itemObject != null)
        {
            itemObject.SendMessage("SetQuantity", 0, SendMessageOptions.RequireReceiver);
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
        GameObject newItemObject = Instantiate(itemPrefab, itemsParent);
        Text itemNameText = newItemObject.transform.Find("Item Name").GetComponent<Text>();
        newItemObject.name = itemList[itemList.Count - 1];

        if (itemNameText != null && itemList.Count > 0)
        {
            // Set the text of the UI element to the last item in the itemList
            itemNameText.text = itemList[itemList.Count - 1];
        }
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
                    uppertext.text = "Hi, I am Scarecrow, can you bring me a flower?";
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
                    uppertext.text = "You need winter boots to pass through the river";
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
}
