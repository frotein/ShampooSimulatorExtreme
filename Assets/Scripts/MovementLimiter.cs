using UnityEngine;
using System.Collections;

//[System.Serializable]
public class MovementLimiter
{
    public Transform pos1, pos2; // the hands or hand to be limited;
    public float length;
    public Transform[] chain; // used to get the exact length of a chain of objects being limited
    public bool pos1CanMove, pos2CanMove;
    float give;
    float lastFrameLength;
    public MovementLimiter(Transform p1, Transform p2, float lnth, bool pos1CM, bool pos2CM, Transform[] chane)
    {
        pos1 = p1;
        pos2 = p2;
        pos1CanMove = pos1CM;
        pos2CanMove = pos2CM;
        chain = chane;
        give = 0.05f * chain.Length;
        length = lnth + give;
    }
    float getChainLength()
    {
        float tLength = 0;
        for (int i = 0; i < chain.Length - 1; i++)
        {
            tLength += Vector2.Distance(chain[i].position.XY(), chain[i + 1].position.XY());
        }
        return tLength;
    }
    bool BeyondLimit()
    {
        float tLength = getChainLength();
       
       
        return (tLength > length && tLength > lastFrameLength);
    }


    DoubleVector2 LimitedPositionMovements()
    {
        DoubleVector2 dv2;
        dv2.pos1 = chain[0].position.XY(); // pos1.position.XY();
        dv2.pos2 = chain[chain.Length - 1].position.XY();// pos2.position.XY();
        if (BeyondLimit())
        {
            float nLength = length;
            Vector2 dir = (dv2.pos1 - dv2.pos2).normalized; // pointing toward pos1 
            if (pos1CanMove && !pos2CanMove)
            {
                Vector2 nPos1 = dv2.pos2 + dir * nLength;
                dv2.pos1 = nPos1;
            }

            if (pos2CanMove && !pos1CanMove)
            {
                Vector2 nPos2 = dv2.pos1 - dir * nLength;
                dv2.pos2 = nPos2;
            }

            if (pos2CanMove && pos1CanMove)
            {
                Vector2 mid = (dv2.pos1 - dv2.pos2) * .5f + dv2.pos2;
                Vector2 nPos1 = mid + dir * (nLength / 2);
                Vector2 nPos2 = mid - dir * (nLength / 2);
                dv2.pos1 = nPos1;
                dv2.pos2 = nPos2;
            }
            dv2.pos1 = dv2.pos1 - chain[0].position.XY();
            dv2.pos2 = dv2.pos2 - chain[chain.Length - 1].position.XY();
        }
        else
        { dv2.pos1 = Vector2.zero; dv2.pos2 = Vector2.zero; }

            return dv2;
    }

    public void ApplyLimits()
    {
        DoubleVector2 v2l = LimitedPositionMovements();
        pos1.position -= v2l.pos1.XYZ(0);
        pos2.position -= v2l.pos2.XYZ(0);
        lastFrameLength = getChainLength();
    }
	
}

public struct DoubleVector2
{
    public Vector2 pos1, pos2;
}
