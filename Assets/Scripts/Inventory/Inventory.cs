using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class Inventory : MonoBehaviour
{
    public class UnityIntEvent : UnityEvent<int> { }
    public int numberOfBombs=4;
    public int numberOfRopes=4;
    public int goldAmount=0;
    public UnityEvent BombEvent { get; private set; } = new UnityEvent();
    public UnityEvent RopeEvent { get; private set; } = new UnityEvent();
    public UnityIntEvent GoldEvent { get; private set; } = new UnityIntEvent();
    public bool hasGlove =false;
  

    private void Reset()
    {
        numberOfBombs = 4;
        numberOfRopes = 4;
        goldAmount = 0;
    }
    //private void Awake()
    //{
    //    BombEvent?.Invoke();
    //    RopeEvent?.Invoke();
    //    GoldEvent?.Invoke(0);
    //}
    public void UseBomb()
    {
        numberOfBombs--;
        BombEvent?.Invoke();
    }
    public void UseRope()
    {
        numberOfRopes--;
        RopeEvent?.Invoke();
    }
    public void PickupBombs(int amount)
    {
        numberOfBombs += amount;
        BombEvent?.Invoke();
    }
    public void PickupRopes(int amount)
    {
        numberOfRopes += amount;
        RopeEvent?.Invoke();
    }
    public void PickupGold(int amount)
    {
        goldAmount += amount;
        GoldEvent?.Invoke(amount);
    }
    public void PickupGlove()
    {
        hasGlove = true;
    }
}
