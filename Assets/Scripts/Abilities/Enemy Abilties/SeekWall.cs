// ## - JV
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeekWall : Ability
{
    [SerializeField] int raycastPrecision = 100; // How many times does the raycast check per frame for walls around the player?
    [SerializeField] LayerMask walls = 64;
    [SerializeField] Transform targetPointer;
    private Enemy thisEnemy; // An Enemy class specific version of base.thisEntity.

    private void Start()
    {
        thisEnemy = GetComponent<Enemy>();
        targetPointer = new GameObject("targetPointer").transform;
    }
    public override void OnActivate()
    {
        StartCoroutine(BeginCooldown());
        RaycastHit2D[] hits = new RaycastHit2D[raycastPrecision];
        int filledIndex = 0;

        for (int i = 0; i < raycastPrecision; i++)
        {
            float x = Mathf.Sin(360 / raycastPrecision * i);
            float y = Mathf.Cos(360 / raycastPrecision * i);
            RaycastHit2D tempHit = Physics2D.Raycast(thisEnemy.target.transform.position, new Vector2(x, y), range, walls);
            if (tempHit) 
            { 
                //Debug.DrawLine(tempHit.point, thisEnemy.target.transform.position, Color.yellow, 1);
                hits[filledIndex] = tempHit; filledIndex++; 
            }
            else
            {
                //Debug.DrawLine(thisEnemy.target.position + new Vector3(x, y) * range, thisEnemy.target.position, Color.red, 1);
            }
        }

        if (hits.Length > 0)
        {
            targetPointer.position = hits[Random.Range(0, filledIndex)].point;
            thisEnemy.target = targetPointer;
            thisEnemy.state = 2; // Manually set the enemy state to override the StateChange Cooldown
        }
    }

    public override void PurgeDependantObjects()
    {
        //Destroy(targetPointer.gameObject);
        targetPointer.gameObject.SetActive(false);
    }


}