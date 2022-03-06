using UnityEngine;

[RequireComponent(typeof(NPC))]
public class OfflineNPC : MonoBehaviour
{
    private NPC _baseNPC;

    private void Start()
    {
        _baseNPC = GetComponent<NPC>();
        _baseNPC.Init();
    }

    void Update()
    {
        _baseNPC.Tick();
    }
}
