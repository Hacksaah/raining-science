using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    public static InteractManager Instance;
    private Transform player;

    private LinkedList<Interactable> queue = new LinkedList<Interactable>();
    private Interactable currentItem = null;
    

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObjectPoolManager.PlayerTarget;
    }

    // Update is called once per frame
    void Update()
    {
        int count = queue.Count;
        if (count > 1)
        {
            float dist = Mathf.Infinity;
            foreach (Interactable item in queue)
            {
                float newDist = Vector3.Distance(item.transform.position, player.position);
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
