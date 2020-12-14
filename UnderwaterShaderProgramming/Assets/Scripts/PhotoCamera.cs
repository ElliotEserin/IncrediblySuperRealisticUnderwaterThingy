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

    Value value = Value.BloomInt;

    public UnityEngine.UI.Text valueDisplay;

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

        if (photoCam.enabled)
        {
            EditValues();

            if (Input.anyKeyDown)
            {
                SelectValue();
            }
        }
    }

    void SwitchMode()
    {
        photoCam.enabled = !photoCam.enabled;
        cameraOverlay.SetActive(photoCam.enabled);
        playerCam.enabled = !playerCam.enabled;
        bt.enabled = !bt.enabled;
        Debug.Log(photoCam.enabled + " " + playerCam.enabled);
    }

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

        File.WriteAllBytes("Photo" + fileCounter + ".png", tex.EncodeToPNG());

        DestroyImmediate(tex);

        photoCam.targetTexture = rt;
        photoCam.Render();
        RenderTexture.active = rt;

        DestroyImmediate(mRt);

        photoCam.targetTexture = null;

        fileCounter++;

        SwitchMode();
    }

    void SelectValue()
    {
        if (Input.GetKeyDown(KeyCode.I)) { value = Value.BloomInt; }
        else if (Input.GetKey(KeyCode.P)) { value = Value.DofAp; }
        else if (Input.GetKey(KeyCode.F)) { value = Value.DofFocLen; }
        else if (Input.GetKey(KeyCode.O)) { value = Value.DofFocDist; }
        else if (Input.GetKey(KeyCode.T)) { value = Value.ColTemp; }
        else if (Input.GetKey(KeyCode.Y)) { value = Value.ColTint; }

        UpdateUI("Use scroll wheel to adjust value");
    }

    void EditValues()
    {
        float input = Input.GetAxis("Mouse ScrollWheel");

        if (input == 0)
            return;

        switch (value)
        {
            default:
            case Value.BloomInt:
                bloom.intensity.value += input;
                UpdateUI("Intensity", bloom.intensity.value);
                break;
            case Value.ColTemp:
                colorGrading.temperature.value += input * 10f;
                UpdateUI("Temperature", colorGrading.temperature.value);
                break;
            case Value.ColTint:
                colorGrading.tint.value += input * 10;
                UpdateUI("Tint", colorGrading.tint.value);
                break;
            case Value.DofAp:
                dof.aperture.value += input * 5f;
                UpdateUI("Aperture", dof.aperture.value);
                break;
            case Value.DofFocDist:
                dof.focusDistance.value += input * 20f;
                UpdateUI("Focus Distance", dof.focusDistance.value);
                break;
            case Value.DofFocLen:
                dof.focalLength.value += input * 20f;
                UpdateUI("Focal Length", dof.focalLength.value);
                break;
        }
    }

    void UpdateUI(string valueType, float value)
    {
        valueDisplay.text = valueType + ": " + Mathf.Round(value);
    }

    void UpdateUI(string message)
    {
        valueDisplay.text = message;
    }

    enum Value
    {
        BloomInt,
        DofAp,
        DofFocLen,
        DofFocDist,
        ColTemp,
        ColTint
    }
}