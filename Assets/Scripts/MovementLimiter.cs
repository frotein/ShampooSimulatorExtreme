using UnityEngine;
using System.Collections;

//[System.Serializable]
public class MovementLimiter
{
    public Transform pos1, pos2; // the hands or hand to be limited;
    public float length;
    public Transform[] chain; // used to get the exact length of a chain of objects being limited
    public float maxChainLength;
    public bool pos1CanMove, pos2CanMove;

    float give;
    float lastFrameLength;
    Vector2 center;
    public MovementLimiter(Transform p1, Transform p2, float lnth, bool pos1CM, bool pos2CM, Transform[] chane, float maxChaneLength)
    {
        pos1 = p1;
        pos2 = p2;
        pos1CanMove = pos1CM;
        pos2CanMove = pos2CM;
        chain = chane;
        give = .25f;
        length = lnth;
        maxChainLength = maxChaneLength;
        float[] temps = getChainDistances();
       // foreach (float f in temps)
        //    Debug.Log(f);
    }
    float[] getChainDistances()
    {
        float[] temp = new float[chain.Length - 1];
        for(int i = 0; i < temp.Length - 1; i++)
        {
            
            temp[i] = Vector2.Distance(chain[i].position.XY(), chain[i + 1].position.XY());
            Debug.Log(chain[i].name + " to " + chain[i + 1].name + " is " + temp[i]);
        }

        return temp;
    }

    Transform chainSegmentBreakingLimit()
    {
        float[] lengths = getChainDistances();
        int i = 0;
        foreach (float lnth in lengths)
        {
            if (lnth > maxChainLength + give)
                return chain[i];
          i++; 
        }


        return null;
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
        Transform chainSegment = chainSegmentBreakingLimit();
        if (chainSegment!= null)
        {
            HingeJoint2D[] hj = chainSegment.GetComponents<HingeJoint2D>();
            Transform connectedRB;
            HingeJoint2D joint1, joint2;
            joint1 = hj[0];
            joint2 = joint1.gameObject.GetComponents<HingeJoint2D>()[1];
            joint1.enabled = false;
            joint2.enabled = false;
            //chainSegment.GetComponent<Rigidbody2D>().AddForce(10 * chainSegment.up);
            
            /* float nLength = length;
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
                center = mid;
                Vector2 nPos1 = mid + dir * (nLength / 2);
                Vector2 nPos2 = mid - dir * (nLength / 2);
                dv2.pos1 = nPos1;
                dv2.pos2 = nPos2;
            }
            dv2.pos1 = dv2.pos1 - chain[0].position.XY();
            dv2.pos2 = dv2.pos2 - chain[chain.Length - 1].position.XY();*/
        }
       // else
       // { dv2.pos1 = Vector2.zero; dv2.pos2 = Vector2.zero; }

            return dv2;
    }

    public void ApplyLimits()
    {
        DoubleVector2 v2l = LimitedPositionMovements();
       /* float dist = Vector2.Distance(pos1.position.XY() - v2l.pos1, center);
        if (dist < Vector2.Distance(pos1.position.XY(), center))
        {
            pos1.position -= v2l.pos1.XYZ(0);
            pos2.position -= v2l.pos2.XYZ(0);
        }
        else
        {
            pos1.position += v2l.pos1.XYZ(0);
            pos2.position += v2l.pos2.XYZ(0);
        }
            lastFrameLength = getChainLength();*/
    }
	
}

public struct DoubleVector2
{
    public Vector2 pos1, pos2;
}
