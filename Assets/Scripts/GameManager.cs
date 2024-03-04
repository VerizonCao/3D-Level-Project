using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;

    [SerializeField] private Vector3 playerPosition;
    public List<string> itemList = new List<string>();

    [SerializeField] private Canvas targetCanvas;
    [SerializeField] private GameObject itemPrefab;
    private Transform itemsParent;

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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.transform.position = GameManager.Instance.playerPosition;
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
        itemList.Remove(useItem);
        Debug.Log(useItem + " used.");

        // Find the item object in the scene
        GameObject itemObject = GameObject.Find(useItem + "(Clone)");

        if (itemObject != null)
        {
            itemObject.SendMessage("SetQuantity", 0, SendMessageOptions.RequireReceiver);
        }
    }

    private void CreateItemUI()
    {
        // Instantiate a new itemPrefab as a child of itemsParent
        GameObject newItemObject = Instantiate(itemPrefab, itemsParent);
        Text itemNameText = newItemObject.transform.Find("Item Name").GetComponent<Text>();

        if (itemNameText != null && itemList.Count > 0)
        {
            // Set the text of the UI element to the last item in the itemList
            itemNameText.text = itemList[itemList.Count - 1];
        }
    }
}
