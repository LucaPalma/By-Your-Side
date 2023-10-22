using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveableObject : MonoBehaviour
{
    Vector3 startingLocation;
    Vector3 targetLocation;
    public GameObject targetObject;
    public int switchesRequired;
    int currentSwitches;

    public bool activated = false;
    [Header("Sounds")]
    [SerializeField] private string openName = "DoorOpen";
    private AudioSource doorSound;

    private void Awake()
    {
        doorSound = GameObject.Find(openName).GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (currentSwitches == 0 && !activated)
        {
            activated = true;
            moveOpen();
        }
        if (currentSwitches > 0 && activated)
        {
            moveClose();
        }
    }

    void Start()
    {
        currentSwitches = switchesRequired;
        startingLocation = this.gameObject.transform.position;
        targetLocation = targetObject.transform.position;
    }

    public void reduceRequired()
    {
        currentSwitches -= 1;
    }
    public void increaseRequired()
    {
        currentSwitches += 1;
    }

    void moveOpen()
    {
        doorSound.Play();
        this.transform.position = targetLocation;
    }

    void moveClose()
    {
        doorSound.Play();
        activated = false;
        this.transform.position = startingLocation;
    }


}
