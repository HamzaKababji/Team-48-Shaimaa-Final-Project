using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{

    public Item item;

    public override void Interact()
    {
        base.Interact();

        pickUp();
    }

    void pickUp()
    {
        Debug.Log("Picking up item" + item.name);
        // add to inventory
        bool wasPickedUp = Inventory.instance.Add(item);

        if (wasPickedUp)
        {

        }
        Destroy(gameObject);
    }
}
