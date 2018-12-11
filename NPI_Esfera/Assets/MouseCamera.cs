using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class MouseCamera : MonoBehaviour
{
    Controller m_leapController;
    bool m_twoHandGrabLastFrame = false;
    public Vector3 m_offset;

    public float speed = 1.5f;
    private float X;
    private float Y;

    private void Start()
    {
        m_leapController = new Controller();
    }

    //Hand GetLeftMostHand(Frame f)
    //{
    //    float xComp = float.MaxValue;
    //    Hand candidate = null;
    //    for (int i = 0; i < f.Hands.Count; ++i)
    //    {
    //        //if (f.Hands[i].PalmPosition.ToUnityScaled().x < xComp)

    //        {
    //            candidate = f.Hands[i];
    //            //xComp = f.Hands[i].PalmPosition.ToUnityScaled().x;
    //        }
    //    }
    //    return candidate;
    //}

    //Hand GetRightMostHand(Frame f)
    //{
    //    float xComp = -float.MaxValue;
    //    Hand candidate = null;
    //    for (int i = 0; i < f.Hands.Count; ++i)
    //    {
    //        if (f.Hands[i].PalmPosition.ToUnityScaled().x > xComp)
    //        {
    //            candidate = f.Hands[i];
    //            xComp = f.Hands[i].PalmPosition.ToUnityScaled().x;
    //        }
    //    }
    //    return candidate;
    //}

    bool Pinching(Hand h)
    {
        if (h == null) return false;
        return h.PinchStrength > 0.7f;
    }

    bool Grabbing(Hand h)
    {
        if (h == null) return false;
        return h.GrabStrength > 0.45f;
    }

    void Update()
    {
        Frame f = m_leapController.Frame();

        // Obtenemos la mano izquierda y derecha
        //Hand left_hand = GetLeftMostHand(f);
        //Hand right_hand = GetRightMostHand(f);
        Hand left_hand = null;
        Hand right_hand = null;

        for (int i = 0; i < f.Hands.Count; ++i)
        {
            if (f.Hands[i].IsLeft)
            {
                left_hand = f.Hands[i];
            }
            else if (f.Hands[i].IsRight)
            {
                right_hand = f.Hands[i];
            }
        }



        if (f.Hands.Count > 0)
        {
            float pitch = right_hand.Direction.Pitch;
            float yaw = right_hand.Direction.Yaw;

            if ( right_hand.GrabStrength < 0.4)
            {
            transform.Rotate(new Vector3(- pitch * speed, yaw * speed, 0));
                X = transform.rotation.eulerAngles.x;
                Y = transform.rotation.eulerAngles.y;
                transform.rotation = Quaternion.Euler(X, Y, 0);
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, 0));
                X = transform.rotation.eulerAngles.x;
                Y = transform.rotation.eulerAngles.y;
                transform.rotation = Quaternion.Euler(X, Y, 0);
            }

        }


        //if (Input.GetMouseButton(0))
        //{
        //    transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed, -Input.GetAxis("Mouse X") * speed, 0));
        //    X = transform.rotation.eulerAngles.x;
        //    Y = transform.rotation.eulerAngles.y;
        //    transform.rotation = Quaternion.Euler(X, Y, 0);
        //}
    }
}

