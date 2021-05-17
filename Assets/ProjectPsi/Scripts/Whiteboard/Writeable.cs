using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Writeable : MonoBehaviour
{
    public Camera captureCam;
    public new MeshRenderer renderer;
    public MeshRenderer oldRenderer;
    //public TrailRenderer[] markers;
    public Texture2D inputTexture;

    private RenderTexture captureTexture;
    private Texture2D saveTexture;
    private Texture2D inputTextureCopy;//if you want to save the texture out, dont use this
    
    private Material mat;
    private Material old;

    //private int currentMarker = 0;

    // Start is called before the first frame update
    private void Start()
    {
        captureTexture = new RenderTexture(1024, 512, 24);
        saveTexture = new Texture2D(1024, 512, TextureFormat.ARGB32,false);
        captureTexture.useDynamicScale = true;
        mat = new Material(Shader.Find("Standard"));
        old = new Material(Shader.Find("Unlit/Texture"));
        
        captureCam.targetTexture = captureTexture;
        renderer.material = mat;
        if (inputTexture == null)
        {
            Color[] pixels = saveTexture.GetPixels();

            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = Color.white;
            }

            saveTexture.SetPixels(pixels);
            saveTexture.Apply();
        }
        else
        {
            inputTextureCopy = new Texture2D(inputTexture.width, inputTexture.height, inputTexture.format, false);
            Graphics.CopyTexture(inputTexture, inputTextureCopy);
            saveTexture = inputTextureCopy;
        }
        oldRenderer.material = old;
        mat.mainTexture = saveTexture;
    }

    TrailRenderer marker;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Marker") || collision.collider.CompareTag("Eraser"))
        {
            //Debug.Log("entered");
            marker = collision.gameObject.GetComponentInChildren<TrailRenderer>();
            marker.emitting = true;
            mat.mainTexture = captureTexture;
            old.mainTexture = saveTexture;
            //markers[marker.markerNumber].emitting = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if(collision.collider.CompareTag("Marker") || collision.collider.CompareTag("Eraser"))
        {
            //Debug.Log("left");
            
            Graphics.CopyTexture(captureTexture, saveTexture);
            mat.mainTexture = saveTexture;

            marker = collision.gameObject.GetComponentInChildren<TrailRenderer>();
            marker.emitting = false;
            marker.Clear();
            //markers[currentMarker].emitting = false;
            //markers[currentMarker].Clear();
        }
    }
}
