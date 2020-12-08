using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Rendering.PostProcessing;

public class PhotoCamera : MonoBehaviour
{
    public float strength = 1f;
    public int fileCounter;
    public KeyCode screenShotModeKey;
    public KeyCode screenshotKey;

    public RenderTexture texture;

    public GameObject cameraOverlay;

    RenderTexture rt;
    Camera photoCam;
    Camera playerCam;

    public PostProcessVolume volume;
    Bloom bloom;
    DepthOfField dof;
    ColorGrading colorGrading;



    BaitThrow bt;

    private void Start()
    {
        bt = FindObjectOfType<BaitThrow>();

        bloom = volume.profile.GetSetting<Bloom>();
        dof = volume.profile.GetSetting<DepthOfField>();
        colorGrading = volume.profile.GetSetting<ColorGrading>();

        if (photoCam == null)
            photoCam = GetComponent<Camera>();
        if (playerCam == null)
            playerCam = Camera.main;

        Debug.Log(bloom + "\n" + dof + "\n" + colorGrading);
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(screenShotModeKey))
        {
            SwitchMode();
        }

        if (Input.GetKeyDown(screenshotKey))
        {
            //Capture();
            SavePNG();
        }

        if(photoCam.enabled)
            EditValues();
    }

    void SwitchMode()
    {
        photoCam.enabled = !photoCam.enabled;
        cameraOverlay.SetActive(photoCam.enabled);
        playerCam.enabled = !playerCam.enabled;
        bt.enabled = !bt.enabled;
    }

    //public void Capture()
    //{
    //    _camera = GetComponent<Camera>();

    //    RenderTexture activeRenderTexture = RenderTexture.active;
    //    RenderTexture.active = Camera.targetTexture;

    //    Camera.Render();

    //    Texture2D image = new Texture2D(Camera.targetTexture.width, Camera.targetTexture.height);
    //    image.ReadPixels(new Rect(0, 0, Camera.targetTexture.width, Camera.targetTexture.height), 0, 0);
    //    image.Apply();

    //    RenderTexture.active = activeRenderTexture;

    //    byte[] bytes = image.EncodeToPNG();
    //    Destroy(image);

    //    File.WriteAllBytes("H:\\Pictures\\" + fileCounter + ".png", bytes);
    //    fileCounter++;
    //}

    public void SavePNG()
    {
        if (photoCam.enabled)
            SwitchMode();

        photoCam = GetComponent<Camera>();
        photoCam.targetTexture = texture;
        rt = photoCam.targetTexture;

        RenderTexture mRt = new RenderTexture(rt.width, rt.height, rt.depth, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        mRt.antiAliasing = rt.antiAliasing;

        var tex = new Texture2D(mRt.width, mRt.height, TextureFormat.ARGB32, false);
        photoCam.targetTexture = mRt;
        photoCam.Render();
        RenderTexture.active = mRt;

        tex.ReadPixels(new Rect(0, 0, mRt.width, mRt.height), 0, 0);
        tex.Apply();

        File.WriteAllBytes("H:\\Pictures\\" + "Photo" + fileCounter + ".png", tex.EncodeToPNG());

        DestroyImmediate(tex);

        photoCam.targetTexture = rt;
        photoCam.Render();
        RenderTexture.active = rt;

        DestroyImmediate(mRt);

        photoCam.targetTexture = null;

        fileCounter++;

        SwitchMode();
    }

    void EditValues()
    {
        float multiplier = Input.GetKey(KeyCode.LeftShift) ? -1 : 1;
        multiplier *= Time.deltaTime * strength;

        //I changes intensity (bloom)
        bloom.intensity.value += multiplier * (Input.GetKey(KeyCode.I) ? 1 : 0);
        //P changes aperture (dof)
        dof.aperture.value += multiplier * (Input.GetKey(KeyCode.P) ? 7.5f : 0);
        //F changes focal length (dof)
        dof.focalLength.value += multiplier * (Input.GetKey(KeyCode.F) ? 20 : 0);
        //O changes focus distance (dof)
        dof.focusDistance.value += multiplier * (Input.GetKey(KeyCode.O) ? 20 : 0);
        //T changes temperature (Color grade)
        colorGrading.temperature.value += multiplier * (Input.GetKey(KeyCode.T) ? 10 : 0);
        //Y changes tint (Color Grade)
        colorGrading.tint.value += multiplier * (Input.GetKey(KeyCode.Y) ? 10 : 0);
    }
}