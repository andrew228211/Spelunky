using TMPro;
using UnityEngine;
public class ItemUI : MonoBehaviour
{
    private Player player;
    private TextMeshProUGUI bombAmountText;
    private TextMeshProUGUI ropeAmountText;
    private TextMeshProUGUI currentGoldAmountText;
    
    private void Awake()
    {
        player = GetComponent<Player>();
        bombAmountText = GameObject.Find("bombAmountText").GetComponent<TextMeshProUGUI>();
        ropeAmountText = GameObject.Find("ropeAmountText").GetComponent<TextMeshProUGUI>();
        currentGoldAmountText = GameObject.Find("dollarAmountText").GetComponent<TextMeshProUGUI>();      
    }
    private void Start()
    {
        player.inventory.BombEvent.AddListener(BombChanged);
        player.inventory.RopeEvent.AddListener(RopeChanged);
        player.inventory.GoldEvent.AddListener(GoldChanged);
    }
    private void Update()
    {
        
    }
    private void BombChanged()
    {
        bombAmountText.text = player.inventory.numberOfBombs.ToString();
    }
    private void RopeChanged()
    {
        ropeAmountText.text = player.inventory.numberOfRopes.ToString();
    }
    private void GoldChanged(int newGold)
    {
        currentGoldAmountText.text = player.inventory.goldAmount.ToString();
    }
}
