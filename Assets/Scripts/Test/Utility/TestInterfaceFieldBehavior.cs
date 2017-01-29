using UnityEngine;
using System.Collections;

public class TestInterfaceFieldBehavior : MonoBehaviour
{
    [InterfaceField]
    public Transform testTransform;

    [InterfaceField]
    public string testString;

    [InterfaceField]
    public Vector2 testVector;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
