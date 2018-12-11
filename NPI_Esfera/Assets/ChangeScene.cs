using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class ChangeScene : MonoBehaviour {

    Controller m_leapController;
    //Set these Textures in the Inspector
    public Texture m_MainTexture, m_Normal, m_Metal;
    Renderer m_Renderer;

    Random rnd;

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
        m_Renderer.material.SetTexture("_MainTex", m_MainTexture);
    }

    // Update is called once per frame
    void Update()
    {
        Frame f = m_leapController.Frame();

        // Obtenemos la mano izquierda y derecha
        Hand left_hand = null;

        for (int i = 0; i < f.Hands.Count; ++i)
            if (f.Hands[i].IsLeft)
                left_hand = f.Hands[i];


        if (left_hand != null)
        {
            if (left_hand.GrabStrength > 0.9)
            {

                if (Random.Range(0, 2) == 0)
                    m_Renderer.material.mainTexture = m_MainTexture;
                else
                    m_Renderer.material.mainTexture = m_Metal;

            }

        }   
    }
}
