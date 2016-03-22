using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.ImageEffects
{
    [RequireComponent(typeof(Camera))]
    public class Draw2DMetaballs : PostEffectsBase
    {
        public float radius;
        public float testCenter;
        // resources
        public Shader shader;
        public Color waterColor;
        public List<Vector3> positions;
        private Material metaballMaterial = null;
        private Camera _camera;
        private Texture2D positionsTexture;
        new void Start()
        {
            CheckResources();
            testCenter = Screen.width / 2;
            if (_camera == null)
                _camera = GetComponent<Camera>();
        }

        void OnEnable()
        {

            if (_camera == null)
                _camera = GetComponent<Camera>();
        }

        void OnDisable()
        {
            
            if (null != metaballMaterial)
            {
                DestroyImmediate(metaballMaterial);
                metaballMaterial = null;
            }
        }
        void CreatePositionsTexture()
        {
            positionsTexture = new Texture2D(positions.Count + 1, 1);
            //Color[] positionsAsColors = new Color[positions.Count];
            //for(int i = 0)
            for (int i = 0; i < positions.Count; i++)
            {
                Vector3 pos = _camera.WorldToScreenPoint(positions[i]);
                Color positionAsColor = new Color(pos.x, pos.y, pos.z);
                positionsTexture.SetPixel(i + 1, 1, positionAsColor);
            }
            positionsTexture.Apply();
        }
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            CreatePositionsTexture();
         
            metaballMaterial.SetColor("_Color", waterColor);
            metaballMaterial.SetFloat("_Radius", radius);
            metaballMaterial.SetFloat("_TestX0", testCenter);
            metaballMaterial.SetTexture("_PositionsTex", positionsTexture);
            metaballMaterial.SetInt("_width", positionsTexture.width);


            Graphics.Blit(source, destination, metaballMaterial);
        }
        public override bool CheckResources()
        {
            CheckSupport(true, true); // depth & hdr needed
            metaballMaterial = CheckShaderAndCreateMaterial(shader, metaballMaterial);

            return isSupported;
        }


    }
}
