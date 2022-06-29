using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public GameObject electrode;
    public GameObject electrodeShadow;

    private static readonly int ElectrodeMaxPositionX = 100;
    private static readonly int ElectrodeMaxPositionY = 60;
    private static readonly int ElectrodeDistanceX = 20;
    private static readonly int ElectrodedistanceY = 20;

    private bool isInitialized = false;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update()
    {

    }
}
