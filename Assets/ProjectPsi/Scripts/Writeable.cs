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
        captureTexture = new RenderTexture(512, 256, 24);
        saveTexture = new Texture2D(512, 256, TextureFormat.ARGB32,false);
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
        //foreach (TrailRenderer marker in markers)
        //{
        //    marker.Clear();
        //    marker.emitting = false;
        //}
    }

    //private Ray ray;
    //private RaycastHit hit;

    // Update is called once per frame
    private void Update()
    {
        //ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(ray, out hit, LayerMask.NameToLayer("WB Colliders")))
        //{
        //    markers[currentMarker].transform.parent.transform.position = hit.point;
        //}

        //if (Input.GetMouseButtonDown(0))
        //{
        //    mat.mainTexture = captureTexture;
        //    old.mainTexture = saveTexture;
        //    markers[currentMarker].emitting = true;
        //}
        //if(Input.GetMouseButtonUp(0))
        //{
        //    Graphics.CopyTexture(captureTexture, saveTexture);
        //    mat.mainTexture = saveTexture;
        //    markers[currentMarker].emitting = false;
        //    markers[currentMarker].Clear();
        //}

        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    if(currentMarker == markers.Length - 1)
        //    {
        //        currentMarker = 0;
        //    }
        //    else
        //    {
        //        currentMarker += 1;
        //    }
        //}
    }
    Marker marker;

    public void OnCollisionEnter(Collision collision)
    {
        marker = collision.collider.GetComponent<Marker>();
        //Debug.Log(collision.gameObject.tag);
        if (marker != null)
        {
            mat.mainTexture = captureTexture;
            old.mainTexture = saveTexture;
            //markers[marker.markerNumber].emitting = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        marker = collision.collider.GetComponent<Marker>();

        if(marker != null)
        {
            Graphics.CopyTexture(captureTexture, saveTexture);
            mat.mainTexture = saveTexture;
            //markers[currentMarker].emitting = false;
            //markers[currentMarker].Clear();
        }
    }
}
