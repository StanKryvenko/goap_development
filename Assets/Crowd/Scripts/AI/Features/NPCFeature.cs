
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class NPCFeature
{
    public static IEnumerable<NPC> GetNearestNPCs(NPC npc, float radius)
    {
        var nearestNPC = Physics.OverlapSphere(npc.Pos, radius)
            .Where(x => x.gameObject != npc.gameObject && x.GetComponent<NPC>() != null);
        
        return nearestNPC.Select(x => x.GetComponent<NPC>());
    }
    
    public static NPC GetNearestNPC(NPC npc, float radius)
    {
        var nearestNPC = Physics.OverlapSphere(npc.Pos, radius).FirstOrDefault(x => x.gameObject != npc.gameObject);
        
        return nearestNPC != null ? nearestNPC.GetComponent<NPC>() : null;
    }

    public static double FindDistanceTo(this NPC npc, NPC target)
    {
        var aPos = npc.transform.position;
        var bPos = target.transform.position;
        return (bPos.x - aPos.x) * (bPos.x - aPos.x) + 
               (bPos.y - aPos.y) * (bPos.y - aPos.y) + 
               (bPos.z - aPos.z) * (bPos.z - aPos.z);
    }
    
    public static bool IsAnybodyAround(NPC npc)
    {
        foreach (var castObj in Physics.OverlapSphere(npc.Pos, 5))
        {
            if (castObj.gameObject != npc.gameObject && 
                (castObj.CompareTag("NPC") ||
                castObj.CompareTag("Player")))
            {
                return true;
            }
        }

        return false;
    }

    public static GameObject GetNearestPlayer(NPC npc, float radius)
    {
        var nearestPlayer = Physics.OverlapSphere(npc.Pos, radius).FirstOrDefault(x => x.gameObject != npc.gameObject && x.CompareTag("Player"));
        
        return nearestPlayer != null ? nearestPlayer.gameObject : null;
    }

    public static NPC GetNearestEnemy(NPC npc, float radius)
    {
        var nearestNPC = Physics.OverlapSphere(npc.Pos, radius).FirstOrDefault(x =>
            x.GetComponent<NPC>() != null && x.GetComponent<NPC>().Home.GroupLeader != npc.Home.GroupLeader);
        
        return nearestNPC != null ? nearestNPC.GetComponent<NPC>() : null;
    }
}