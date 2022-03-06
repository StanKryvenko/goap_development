using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GroupTypeEnum
{
    Soldier,
    Partisan,
    Protector
}

public class NPCSpawner : MonoBehaviour
{
    public NPC GroupLeader { get; set; }
    
    public Dictionary<NPC, int> VoteBook = new Dictionary<NPC, int>();
    public GroupTypeEnum GroupType;

    public Guid VotingId = Guid.NewGuid();

    private float _votingTime = 5;
    private float _lastVoteTime;
    private bool _voteRuns = true;
    
    private void Update()
    {
        if (Time.time > _lastVoteTime + _votingTime)
        {
            if (_voteRuns)
            {
                _voteRuns = false;
                var potentialLeader = VoteBook.FirstOrDefault(a => a.Value == VoteBook.Max(b => b.Value));
                GroupLeader = potentialLeader.Key;
                
                var leaderMaterial = GroupLeader.GetComponent<MeshRenderer>().material;
                leaderMaterial.SetColor("_Color", leaderMaterial.color / 4);
            }

            if (GroupLeader == null && !_voteRuns)
            {
                _lastVoteTime = Time.time;
                _voteRuns = true;
                VoteBook.Clear();
                VotingId = new Guid();
            }

        }
    }
}
