using UnityEngine;
using System.Collections;

public class MovementLimiter
{
    public Transform pos1, pos2;
    public float length;
    public bool pos1CanMove, pos2CanMove;
    public MovementLimiter(Transform p1, Transform p2, float lnth, bool pos1CM, bool pos2CM)
    {
        pos1 = p1;
        pos2 = p2;
        length = lnth;
        pos1CanMove = pos1CM;
        pos2CanMove = pos2CM;
    }

    bool BeyondLimit()
    {
        return (Vector2.Distance(pos1.position.XY(), pos2.position.XY()) > length);
    }


    DoubleVector2 LimitedPositions()
    {
        DoubleVector2 dv2;
        dv2.pos1 = pos1.position.XY();
        dv2.pos2 = pos2.position.XY();
            if(BeyondLimit())
            {

                Vector2 dir = (dv2.pos1 - dv2.pos2).normalized; // pointing toward pos1 
                if (pos1CanMove && !pos2CanMove)
                {
                    Vector2 nPos1 = dv2.pos2 + dir * length;
                    dv2.pos1 = nPos1;
                }

                if (pos2CanMove && !pos1CanMove)
                {
                    Vector2 nPos2 = dv2.pos1 - dir * length;
                    dv2.pos2 = nPos2;
                }

                if(pos2CanMove && pos1CanMove)
                {
                    Vector2 mid = (dv2.pos1 - dv2.pos2) * .5f + dv2.pos2;
                    Vector2 nPos1 = mid + dir * (length / 2);
                    Vector2 nPos2 = mid - dir * (length / 2);
                    dv2.pos1 = nPos1;
                    dv2.pos2 = nPos2;
                }

        }
        return dv2;
    }

    public void ApplyLimits()
    {
        DoubleVector2 v2l = LimitedPositions();
        pos1.position = v2l.pos1.XYZ(pos1.position.z);
        pos2.position = v2l.pos2.XYZ(pos2.position.z);
    }
	
}

public struct DoubleVector2
{
    public Vector2 pos1, pos2;
}
