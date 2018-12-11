using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class MouseCamera : MonoBehaviour
{
    Controller m_leapController;
    bool m_twoHandGrabLastFrame = false;
    public Vector3 m_offset;
    Bone index_finger;

    public float speed = 0.1f;
    private float X;
    private float Y;

    //Set these Textures in the Inspector
    public Texture m_MainTexture, m_Normal, m_Metal;
    Renderer m_Renderer;

    private void Start()
    {
        // Leap init
        m_leapController = new Controller();
    }

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

        if (right_hand != null)
        {
            index_finger = right_hand.Fingers[1].Bone(Bone.BoneType.TYPE_DISTAL);


            float pitch = right_hand.Direction.Pitch;
            float yaw = right_hand.Direction.Yaw + 0.5F;

            //float pitch = index_finger.Direction.Pitch;
            //float yaw = index_finger.Direction.Yaw;

            Debug.Log("Pitch: " + pitch);
            Debug.Log("Yaw: " + yaw);

            if (Mathf.Abs(yaw) < 0.25F)
            {
                yaw = 0.0F;
            }
            if (Mathf.Abs(pitch) < 0.25F){
                pitch = 0.0F;
            }

            if ( right_hand.GrabStrength < 0.4)
            {
                transform.Rotate(new Vector3(- pitch * speed, yaw * speed, 0));
                X = transform.rotation.eulerAngles.x;
                Y = transform.rotation.eulerAngles.y;
                transform.rotation = Quaternion.Euler(X, Y, 0);
                //ChangeScene prueba;

                //prueba.cambia();
            }

            //else
            //{
            //    transform.Rotate(new Vector3(0, 0, 0));
            //    X = transform.rotation.eulerAngles.x;
            //    Y = transform.rotation.eulerAngles.y;
            //    transform.rotation = Quaternion.Euler(X, Y, 0);
            //}
        }
    }
}

