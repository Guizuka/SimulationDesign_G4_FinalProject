using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Size
{
    Small,
    Medium,
    Large
}

public enum Base
{
    Coffee,
    Juice,
    Tea
}

public enum Additions
{
    Caramel,
    Chocolate,
    Strawberry,
    Vanilla,
    Maple,
    Peppermint
}

public class Order
{
    private Size order_size;
    private Base order_base;
    private List<Additions> order_additions;

    public Order(Size _size = Size.Small, Base _base = Base.Coffee)
    {
        order_size = _size;
        order_base = _base;
    }
    public Order(List<Additions> additions, Size _size = Size.Small, Base _base = Base.Coffee)
    {
        order_size = _size;
        order_base = _base;
        order_additions = additions;
    }
    public Size GetSize() { return order_size; }
    public void SetSize(Size size) { order_size = size; }

    public Base GetBase() { return order_base; }
    public void SetBase(Base _base) { order_base = _base; }

    public List<Additions> GetAdditions() { return order_additions; }
    public void AddOneAddition(Additions addition) {
        if(order_additions.Count < 5)
            order_additions.Add(addition);
        else
            Debug.Log("You can add up to 5 additions in a drink");
    }
    public void AddAdditions(List<Additions> additions)
    {
        if(order_additions.Count + additions.Count >= 5)
            order_additions.AddRange(additions);
        else
            Debug.Log("You can add up to 5 additions in a drink");
    }
}
