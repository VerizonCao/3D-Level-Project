using UnityEngine;
using UnityEngine.UI;

public class ItemStatus : MonoBehaviour
{
    [SerializeField] private Text quantityText;
    public int itemQuantity = 1;

    void Start()
    {
        itemQuantity = 1;
        UpdateQuantityText();
    }

    public void SetQuantity(int newQuantity)
    {
        itemQuantity = newQuantity;
        UpdateQuantityText();
    }

    private void UpdateQuantityText()
    {
        if (quantityText != null)
        {
            quantityText.text = $"{itemQuantity}/1";
        }
    }
}
