using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    public static InteractManager Instance;

    private Queue<Interactable> queue = new Queue<Interactable>();
    private List<int> oldId = new List<int>();
    private Interactable currentItem = null;

    private int uniqueId = -1;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentItem == null && queue.Count > 0)
            currentItem = queue.Dequeue();
        else if(currentItem != null && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interacting");
            currentItem.Interact(); 
        }
    }

    public int AddItemToQueue(Interactable item)
    {
        gameObject.SetActive(true);
        queue.Enqueue(item);
        uniqueId++;
        return uniqueId;
    }

    public void RemoveItemFromQueue(int id)
    {
        oldId.Add(id);
    }
}
