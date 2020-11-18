using UnityEngine;
using System.Collections;
using System.IO;

public class PhotoCamera : MonoBehaviour
{
    public int fileCounter;
    public KeyCode screenshotKey;

    RenderTexture rt;
    Camera cam;

    private Camera Camera
    {
        get
        {
            if (!_camera)
            {
                _camera = Camera.main;
            }
            return _camera;
        }
    }
    private Camera _camera;
    private void LateUpdate()
    {
        if (Input.GetKeyDown(screenshotKey))
        {
            //Capture();
            SavePNG();
        }
    }
    public void Capture()
    {
        _camera = GetComponent<Camera>();

        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = Camera.targetTexture;

        Camera.Render();

        Texture2D image = new Texture2D(Camera.targetTexture.width, Camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, Camera.targetTexture.width, Camera.targetTexture.height), 0, 0);
        image.Apply();

        RenderTexture.active = activeRenderTexture;

        byte[] bytes = image.EncodeToPNG();
        Destroy(image);

        File.WriteAllBytes("H:\\Pictures\\" + fileCounter + ".png", bytes);
        fileCounter++;
    }

    public void SavePNG()
    {
        cam = GetComponent<Camera>();
        rt = cam.targetTexture;

        RenderTexture mRt = new RenderTexture(rt.width, rt.height, rt.depth, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        mRt.antiAliasing = rt.antiAliasing;

        var tex = new Texture2D(mRt.width, mRt.height, TextureFormat.ARGB32, false);
        cam.targetTexture = mRt;
        cam.Render();
        RenderTexture.active = mRt;

        tex.ReadPixels(new Rect(0, 0, mRt.width, mRt.height), 0, 0);
        tex.Apply();

        File.WriteAllBytes("H:\\Pictures\\" + "Photo" + fileCounter + ".png", tex.EncodeToPNG());

        DestroyImmediate(tex);

        cam.targetTexture = rt;
        cam.Render();
        RenderTexture.active = rt;

        DestroyImmediate(mRt);

        fileCounter++;
    }
}