using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCOfflineManager : MonoBehaviour
{
    public int NPCSpawnAmount = 10;
    // NPC pending for initial spawn or respawn after destroying
    public Queue<GameObject> NpcToBorn = new Queue<GameObject>();
    public GameObject NPCPrefab;

    private const string SpawnPointTag = "NPCSpawnPoint";
    
    private List<GameObject> _spawnPoints = new List<GameObject>();
    private bool _initialSpawnDummy = true;

    private Dictionary<GroupTypeEnum, Color> SpawnColorSet;
    
    private static readonly int ColorProperty = Shader.PropertyToID("_Color");

    private void Start()
    {
        SpawnColorSet = new Dictionary<GroupTypeEnum, Color>
        {
            [GroupTypeEnum.Partisan] = Color.blue,
            [GroupTypeEnum.Protector] = Color.green,
            [GroupTypeEnum.Soldier] = Color.red
        };
    }
    
    private void Update()
    {
        if (_initialSpawnDummy)
            InitialSpawn();

        if (NpcToBorn.Any())
        {
            var born = NpcToBorn.Dequeue();
            born.SetActive(true);
        }
    }

    private void InitialSpawn()
    {
        _spawnPoints = GameObject.FindGameObjectsWithTag(SpawnPointTag).ToList();
        if (!_spawnPoints.Any()) return;
        
        for (var i = 0; i < NPCSpawnAmount; i++)
        {
            var pickedSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
            var spawner = pickedSpawnPoint.GetComponent<NPCSpawner>();
            var dummyPlayer = Instantiate(NPCPrefab, pickedSpawnPoint.transform.position, Quaternion.identity);
            dummyPlayer.GetComponent<MeshRenderer>().material.SetColor(ColorProperty, SpawnColorSet[spawner.GroupType]);
            dummyPlayer.SetActive(false);
            dummyPlayer.GetComponent<NPC>().Home = spawner;
            NpcToBorn.Enqueue(dummyPlayer);
        }
        _initialSpawnDummy = false;
    }
}
