using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Leap;

/**
 * Controla el movimiento de la camara con la mano y detecta los gestos principales.
 */
public class MouseCamera : MonoBehaviour
{
    // Atributos
    Controller m_leapController;
    bool m_twoHandGrabLastFrame = false;
    public Vector3 m_offset;
    Bone index_finger;

    public float speed = 0f;
    private float X;
    private float Y;
    private float Z;
    private float dist_anterior = 0;
    private float camara_ini;
    private float tope_camara;
    private bool zurdo = false;

    private float pos_sec_y = 0;
    private int contador_anterior = 0;

    // Inizializacion del leap y la camara.
    private void Start()
    {
        contador_anterior = 0;
        zurdo = PlayerPrefs.GetInt("ManoPrincipal") == 1;
        pos_sec_y = 0;

        // Leap init
        m_leapController = new Controller();

        camara_ini = GetComponent<Camera>().fieldOfView;
        tope_camara = camara_ini + 10;
    }

    bool Pinching(Hand h)
    {
        if (h == null) return false;
        return h.PinchStrength == 1.0f;
    }

    void Update()
    {
        Frame f = m_leapController.Frame();

        // Obtenemos la mano izquierda y derecha
        Hand left_hand = null;
        Hand right_hand = null;

        float x_d = 0, y_d = 0, z_d = 0,
              x_i = 0, y_i = 0, z_i = 0;
        float deviation = 0.5F;

        bool closed_left = false, left = false,
             closed_right = false, right = false;

        for (int i = 0; i < f.Hands.Count; ++i)
        {
            if (f.Hands[i].IsLeft)
                left_hand = f.Hands[i];
            else if (f.Hands[i].IsRight)
                right_hand = f.Hands[i];
        }

        // Si la mano principal no es la derecha, intercambiamos el objeto
        // asignado a las variables left_hand y right_hand
        if (zurdo)
        {
            Hand aux = right_hand;
            right_hand = left_hand;
            left_hand = aux;
            deviation = -deviation;
        }

        if (right = (right_hand != null))
        {
            float pitch = right_hand.Direction.Pitch,
                  yaw = right_hand.Direction.Yaw + deviation;

            x_d = right_hand.PalmPosition.x;
            y_d = right_hand.PalmPosition.y;
            z_d = right_hand.PalmPosition.z;
            //Debug.Log("POS_DER: (" + x_d + ", " + y_d + ", " + z_d + ")");

            if (Mathf.Abs(yaw) < 0.25F)
                yaw = 0.0F;
            if (Mathf.Abs(pitch) < 0.25F)
                pitch = 0.0F;

            //Debug.Log("GRAB: " + right_hand.GrabStrength);

            if (right_hand.GrabStrength < 1)
            {
                transform.Rotate(new Vector3(-pitch * speed, yaw * speed, 0));
                X = transform.rotation.eulerAngles.x;
                Y = transform.rotation.eulerAngles.y;
                transform.rotation = Quaternion.Euler(X, Y, 0);
            }
            else
                closed_right = true;
        }
        if (left = (left_hand != null))
        {
            x_i = left_hand.PalmPosition.x;
            y_i = left_hand.PalmPosition.y;
            z_i = left_hand.PalmPosition.z;
            //Debug.Log("POS_IZQ: (" + x_i + ", " + y_i + ", " + z_i + ")");

            if (left_hand.GrabStrength >= 1)
                closed_left = true;
        }
        if (left && right)
        {
            float x_2 = (x_d - x_i) * (x_d - x_i),
                  y_2 = (y_d - y_i) * (y_d - y_i),
                  z_2 = (z_d - z_i) * (z_d - z_i);

            float distancia = Mathf.Sqrt(x_2 + y_2 + z_2);
            float diff_distancia = distancia - dist_anterior;
            dist_anterior = distancia;
            //Debug.Log("DIST: " + diff_distancia);
            //Debug.Log("CAMARA_INI: " + camara_ini);

            Vector normal = left_hand.PalmNormal;

            if (normal[1] > 0.85)
            {
                int extendedFingers = 0;
                float y_var = y_i - pos_sec_y;
                pos_sec_y = y_i;

                for (int i = 0; i < left_hand.Fingers.Count; i++)
                {
                    Finger digit = left_hand.Fingers[i];
                    if (digit.IsExtended)
                        extendedFingers++;
                }

                //Debug.Log("DEDOS: " + extendedFingers);

                switch (extendedFingers)
                {
                    case 1:
                        break;
                    case 2:
                        GetComponent<Camera>().fieldOfView = camara_ini;
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }

                if (y_var > 4.0F)
                    contador_anterior++;
                else
                    contador_anterior = 0;

                Debug.Log("CONTADOR: " + contador_anterior);

                if (contador_anterior >= 8)
                    SceneManager.LoadScene(0);
            }
            else if (closed_left && closed_right)
            {
                float camara_actual = GetComponent<Camera>().fieldOfView - diff_distancia / 10;

                if (diff_distancia != 0 && tope_camara >= camara_actual)
                    GetComponent<Camera>().fieldOfView = camara_actual;
            }
            //else if (closed_right && Pinching(left_hand))
            //    GetComponent<Camera>().fieldOfView = camara_ini;
        }
    }
}


