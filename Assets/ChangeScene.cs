using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class ChangeScene : MonoBehaviour
{

    Controller m_leapController;
    //Set these Textures in the Inspector
    public Texture panoramica, sala1, brujas, sala2;
    Renderer m_Renderer;

    private float dist_anterior = 0;
    private int indice_imagen = 0;
    private int numero_texturas = 4;
    private Texture[] vector_imagenes;

    // Use this for initialization
    void Start()
    {
        //Fetch the Renderer from the GameObject
        m_Renderer = GetComponent<Renderer>();

        m_leapController = new Controller();

        //Make sure to enable the Keywords
        m_Renderer.material.EnableKeyword("_NORMALMAP");
        m_Renderer.material.EnableKeyword("_METALLICGLOSSMAP");

        //Set the Texture you assign in the Inspector as the main texture (Or Albedo)
        m_Renderer.material.SetTexture("_MainTex", panoramica);

        vector_imagenes = new Texture[numero_texturas];
        vector_imagenes[0] = panoramica;
        vector_imagenes[1] = sala1;
        vector_imagenes[2] = brujas;
        vector_imagenes[3] = sala2;
    }

    // Update is called once per frame
    void Update()
    {
        Frame f = m_leapController.Frame();

        // Obtenemos la mano izquierda y derecha
        Hand left_hand = null;
        Hand right_hand = null;

        bool closed_left = false, left = false,
             closed_right = false, right = false;

        float x_d = 0, y_d = 0, z_d = 0,
              x_i = 0, y_i = 0, z_i = 0;

        for (int i = 0; i < f.Hands.Count; ++i)
        {
            if (f.Hands[i].IsLeft)
                left_hand = f.Hands[i];
            else if (f.Hands[i].IsRight)
                right_hand = f.Hands[i];
        }

        right = (right_hand != null);
        left = (left_hand != null);

        if (left && right)
        {
            x_d = right_hand.PalmPosition.x;
            y_d = right_hand.PalmPosition.y;
            z_d = right_hand.PalmPosition.z;

            x_i = left_hand.PalmPosition.x;
            y_i = left_hand.PalmPosition.y;
            z_i = left_hand.PalmPosition.z;

            if (right_hand.GrabStrength == 1 && left_hand.GrabStrength < 0.3)
            {
                float x_2 = (x_d - x_i) * (x_d - x_i),
                      y_2 = (y_d - y_i) * (y_d - y_i),
                      z_2 = (z_d - z_i) * (z_d - z_i);

                float distancia = Mathf.Sqrt(x_2 + y_2 + z_2);

                if (distancia < 50)
                {
                    if (indice_imagen < (numero_texturas-1))
                        indice_imagen += 1;
                    else
                        indice_imagen = 0;

                    m_Renderer.material.mainTexture = vector_imagenes[indice_imagen];
                    System.Threading.Thread.Sleep(150);
                }
            }
        }
    }
}
