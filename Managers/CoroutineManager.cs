using UnityEngine;

namespace SharpzReborn.Managers;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance;

    private void Awake() => Instance = this;
}