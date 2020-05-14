using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    static InteractManager instance;
    public static InteractManager Instance
    {
        get
        {
            if (instance == null)
                new InteractManager();
            return instance;
        }
    }

    public Transform PlayerTransform { get; set; }

    private LinkedList<Interactable> queue = new LinkedList<Interactable>();
    private Interactable currentItem = null;
       
    InteractManager() { instance = this; }    

    // Update is called once per frame
    void Update()
    {
        int count = queue.Count;
        if (count > 1)
        {
            float dist = Mathf.Infinity;
            foreach (Interactable item in queue)
            {
                float newDist = Vector3.Distance(item.transform.position, PlayerTransform.position);
                if (newDist < dist)
                {
                    dist = newDist;
                    currentItem = item;
                }
            }
        }
        else if (count > 0)
            currentItem = queue.First.Value;

        if(currentItem != null && Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("Interacting");
            currentItem.Interact();
        }
    }

    public void AddItemToQueue(Interactable item)
    {
        gameObject.SetActive(true);
        queue.AddLast(item);
    }

    public void RemoveItemFromQueue(Interactable item)
    {
        queue.Remove(item);
        if (queue.Count == 0)
            currentItem = null;
    }
}
