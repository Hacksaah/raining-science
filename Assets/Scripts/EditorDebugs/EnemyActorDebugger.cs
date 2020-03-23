
using UnityEngine;
using UnityEditor;

public class EnemyActorDebugger : MonoBehaviour
{
    EnemyActor enemyActor;

    public bool isSelected = true;
    public bool seeViewDistance = false;


    private void Awake()
    {
        enemyActor = GetComponent<EnemyActor>();
    }


    private void OnDrawGizmos()
    {
        if (isSelected)
        {
            if (Selection.Contains(gameObject))
            {
                if (seeViewDistance)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(transform.position, enemyActor.stats.GetSightDistance());
                }

                if (enemyActor.movePath.Length > 0)
                {
                    Gizmos.color = Color.yellow;
                    for (int i = 0; i < enemyActor.movePath.Length; i++)
                    {
                        Gizmos.DrawCube(enemyActor.movePath[i], Vector3.one * 2);
                        if (i > 0)
                            Gizmos.DrawLine(enemyActor.movePath[i], enemyActor.movePath[i - 1]);
                    }
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireCube(enemyActor.currTarget, Vector3.one);
                    Gizmos.DrawLine(transform.position, enemyActor.currTarget);
                }
            }
        }
        else
        {
            if (seeViewDistance)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, enemyActor.stats.GetSightDistance());
            }

            if (enemyActor.movePath.Length > 0)
            {
                Gizmos.color = Color.yellow;
                for (int i = 0; i < enemyActor.movePath.Length; i++)
                {
                    Gizmos.DrawCube(enemyActor.movePath[i], Vector3.one * 2);
                    if (i > 0)
                        Gizmos.DrawLine(enemyActor.movePath[i], enemyActor.movePath[i - 1]);
                }
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(enemyActor.currTarget, Vector3.one);
                Gizmos.DrawLine(transform.position, enemyActor.currTarget);
            }
        }
    }
}
