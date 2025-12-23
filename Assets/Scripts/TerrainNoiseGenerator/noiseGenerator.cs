using System.Linq;
using UnityEngine;
using System.IO;

public class WhiteTextureGenerator : MonoBehaviour
{
    void Start()
    {
        int width = 1024;
        int height = 1024;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);

        Color32[] pixels = new Color32[width * height];

        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = new Color32(255, 255, 255, 255);

        tex.SetPixelData(pixels, 0);
        tex.Apply();
        MeshRenderer rend = GetComponent<MeshRenderer>();
        rend.material.mainTexture = tex;
    }
}
