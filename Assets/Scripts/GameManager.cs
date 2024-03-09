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
    [SerializeField] private Season season;

    [SerializeField] private Vector3 playerPosition;
    public List<string> itemList = new List<string>();

    [SerializeField] private Canvas targetCanvas;
    [SerializeField] TextMeshProUGUI uppertext;
    [SerializeField] private GameObject itemPrefab;
    private Transform itemsParent;

    [SerializeField] bool refuseTeleport = false;

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
    public void SwitchSeason(Season newSeason)
    {
        switch (newSeason)
        {
            case Season.Spring:
                SceneManager.LoadScene("SpringScene");
                break;
            case Season.Summer:
                SceneManager.LoadScene("SummerScene");
                break;
            case Season.Fall:
                SceneManager.LoadScene("FallScene");
                break;
            case Season.Winter:
                SceneManager.LoadScene("WinterScene");
                break;
            default:
                Debug.LogError("Invalid season specified.");
                break;
        }
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
}
