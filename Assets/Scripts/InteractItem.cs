using UnityEngine;
using static GameManager;
using static InteractItem;

public class InteractItem : MonoBehaviour
{
    [SerializeField] private GameObject interactUI;
    [SerializeField] private Transform UILocation;
    [SerializeField] private string requiredItemName;
    public enum ItemType
    {
        ObtainableItem,
        ConsumeItem,
        SeasonChangingItem
    }

    [SerializeField] private ItemType itemType;
    public string itemName;

    public enum SwitchtoSeason
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    [SerializeField] private SwitchtoSeason switchtoSeason;

    public DetectionCircle detectionCircle;
    private Camera mainCamera;

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
                        Destroy(gameObject);
                        break;
                    case ItemType.ConsumeItem:
                        GameManager.Instance.UseItem(itemName);
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
