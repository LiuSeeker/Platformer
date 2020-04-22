
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    // https://github.com/mixandjam/Celeste-Movement/blob/master/Assets/Scripts/Collision.cs
    private LayerMask groundLayer;
    private LayerMask owplatformLayer;

    public bool onGroundEdge;
    public bool onGroundCenter;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public bool onSlope;
    public bool onOWPlatform;
    public int wallSide;


    public Vector2 bottomOffset1, bottomOffset2, bottomOffset3, bottomOffset4, rightOffset, leftOffset;

    // Start is called before the first frame update
    void Start()
    {
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        owplatformLayer = 1 << LayerMask.NameToLayer("OWPlatform");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D qd = Physics2D.Raycast((Vector2)transform.position + bottomOffset1, Vector2.down, 0.1f, groundLayer);
        RaycastHit2D qe = Physics2D.Raycast((Vector2)transform.position + bottomOffset2, Vector2.down, 0.1f, groundLayer);
        RaycastHit2D md = Physics2D.Raycast((Vector2)transform.position + bottomOffset3, Vector2.down, 0.1f, groundLayer);
        RaycastHit2D me = Physics2D.Raycast((Vector2)transform.position + bottomOffset4, Vector2.down, 0.1f, groundLayer);
        RaycastHit2D ld = Physics2D.Raycast((Vector2)transform.position + rightOffset, Vector2.right, -0.1f, groundLayer);
        RaycastHit2D le = Physics2D.Raycast((Vector2)transform.position + leftOffset, Vector2.left, -0.1f, groundLayer);

        RaycastHit2D qdp = Physics2D.Raycast((Vector2)transform.position + bottomOffset1, Vector2.down, 0.1f, owplatformLayer);
        RaycastHit2D qep = Physics2D.Raycast((Vector2)transform.position + bottomOffset2, Vector2.down, 0.1f, owplatformLayer);
        RaycastHit2D mdp = Physics2D.Raycast((Vector2)transform.position + bottomOffset3, Vector2.down, 0.1f, owplatformLayer);
        RaycastHit2D mep = Physics2D.Raycast((Vector2)transform.position + bottomOffset4, Vector2.down, 0.1f, owplatformLayer);

        if ( (qd.collider != null || qe.collider != null) || (qdp.collider != null || qep.collider != null) )
        {
            onGroundEdge = true;
            if (( (qd.collider != null || qdp.collider != null) && (Mathf.Abs(qd.normal.x) > 0.1f || Mathf.Abs(qdp.normal.x) > 0.1f)) ||
                ( (qe.collider != null || qep.collider != null) && (Mathf.Abs(qe.normal.x) > 0.1f || Mathf.Abs(qep.normal.x) > 0.1f)))
            {
                onSlope = true;
            } else
            {
                onSlope = false;
            }
            if (qdp.collider != null || qep.collider != null)
            {
                onOWPlatform = true;
            }
            else
            {
                onOWPlatform = false;
            }
        } else
        {
            onGroundEdge = false;
            onSlope = false;
        }
            

        onGroundCenter = (md.collider != null || me.collider != null || mdp.collider != null || mep.collider != null) ? true : false;
        onWall = (ld.collider != null || le.collider != null) ? true : false;

        //onSlope = ((onGroundEdge ) && Mathf.Abs(hit.normal.x) > 0.1f)

        onRightWall = ld.collider != null ? true : false;
        onLeftWall = le.collider != null ? true : false;

        wallSide = onRightWall ? -1 : 1;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset1, bottomOffset2, bottomOffset3, bottomOffset4, rightOffset, leftOffset };
        Gizmos.DrawRay((Vector2)transform.position + bottomOffset1, Vector2.down * 0.1f);
        Gizmos.DrawRay((Vector2)transform.position + bottomOffset2, Vector2.down * 0.1f);
        Gizmos.DrawRay((Vector2)transform.position + bottomOffset3, Vector2.down * 0.1f);
        Gizmos.DrawRay((Vector2)transform.position + bottomOffset4, Vector2.down * 0.1f);
        Gizmos.DrawRay((Vector2)transform.position + rightOffset, Vector2.right * 0.1f);
        Gizmos.DrawRay((Vector2)transform.position + leftOffset, Vector2.left * 0.1f);
    }
}
