using UnityEngine;
using static GameManager;
using static InteractItem;

public class InteractItem : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject interactUI;
    [SerializeField] private Transform UILocation;
    public DetectionCircle detectionCircle;
    private Camera mainCamera;
    public enum ItemType
    {
        ObtainableItem,
        ConsumeItem,
        SeasonChangingItem
    }

    [SerializeField] private ItemType itemType;
    [Header("ObtainableItem")]
    public string itemName;

    [Header("ConsumeItem")]
    [SerializeField] private string requiredItemName;
    [SerializeField] private GameObject puzzleItem;


    public enum SwitchtoSeason
    {
        Spring,
        Summer,
        Fall,
        Winter
    }
    [Header("SeasonChangingItem")]
    [SerializeField] private SwitchtoSeason switchtoSeason;



    private void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;

            if (mainCamera == null)
            {
                Debug.LogError("Main camera not found.");
                return;
            }
        }

        if (detectionCircle != null && detectionCircle.isTriggered)
        {
            switch (itemType)
            {
                case ItemType.ObtainableItem:
                    UISetActive();
                    break;
                case ItemType.ConsumeItem:
                    if (GameManager.Instance.itemList.Contains(requiredItemName))
                    {
                        UISetActive();
                    }
                    break;
                case ItemType.SeasonChangingItem:
                    UISetActive();
                    break;
                default:
                    break;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                switch (itemType)
                {
                    case ItemType.ObtainableItem:
                        GameManager.Instance.AddItem(itemName);
                        if(itemName == "Carrot")
                        {
                            GameManager.Instance.pickedCarrot = true;
                        }
                        else if(itemName == "Boots")
                        {
                            GameManager.Instance.pickedBoots = true;
                        }
                        Destroy(gameObject);
                        break;
                    case ItemType.ConsumeItem:
                        if (GameManager.Instance.itemList.Contains(requiredItemName))
                        {             
                            puzzleItem.SendMessage("SolvePuzzle", SendMessageOptions.DontRequireReceiver);
                        }
                        GameManager.Instance.UseItem(requiredItemName);
                        break;
                    case ItemType.SeasonChangingItem:
                        Season newSeason = ConvertSwitchtoSeason(switchtoSeason);
                        GameManager.Instance.SwitchSeason(newSeason);
                        break;
                    default:
                        break;
                }


            }
        }
        else
        {
            if (interactUI != null)
            {
                interactUI.SetActive(false);
            }
        }
    }
    private Season ConvertSwitchtoSeason(SwitchtoSeason switchSeason)
    {
        switch (switchSeason)
        {
            case SwitchtoSeason.Spring:
                return Season.Spring;
            case SwitchtoSeason.Summer:
                return Season.Summer;
            case SwitchtoSeason.Fall:
                return Season.Fall;
            case SwitchtoSeason.Winter:
                return Season.Winter;
            default:
                return Season.Spring; 
        }
    }
    private void UISetActive()
    {
        if (interactUI != null)
        {
            interactUI.SetActive(true);

            if (UILocation != null)
            {
                Vector3 screenPos = mainCamera.WorldToScreenPoint(UILocation.position);
                interactUI.transform.position = screenPos;
            }
        }
    }
}
