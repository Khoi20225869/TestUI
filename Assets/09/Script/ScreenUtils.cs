using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ScreenUtils : MonoBehaviour
{
    public RectTransform uiRectTransform = null;
    public static ScreenOrientation currentOrientation = ScreenOrientation.Portrait;
    public static UIScreenRect uiScreenRect = new UIScreenRect();

    public Canvas canvas;
    public CanvasScaler canvasScaler;
    protected float screenRatio = 1.78f;
    public Vector2Int ratio;
    public List<DeviceScreenRatioScale> deviceScreenRatioScales = new List<DeviceScreenRatioScale>();

    [Space(20)]
    public UnityEvent<Vector2> onSizeChanged = null;
    public UnityEvent<float> is129 = null;
    public UnityEvent<float> is159 = null;
    public UnityEvent<float> is189 = null;
    public UnityEvent<float> is219 = null;

    [SerializeField] bool isChangeSize = true;
    [SerializeField] float oldSizeCam = 0;

    private void OnValidate()
    {
        canvas = GetComponent<Canvas>();
        canvasScaler = GetComponent<CanvasScaler>();
        if (deviceScreenRatioScales.Count == 0)
            deviceScreenRatioScales = new List<DeviceScreenRatioScale>
            {
                new DeviceScreenRatioScale{ ratio = new Vector2Int(12, 9), scale = 1f },
                new DeviceScreenRatioScale{ ratio = new Vector2Int(15, 9), scale = 1f },
                new DeviceScreenRatioScale{ ratio = new Vector2Int(18, 9), scale = 0f },
                new DeviceScreenRatioScale{ ratio = new Vector2Int(21, 9), scale = 0f }
            };
        foreach (var i in deviceScreenRatioScales)
            i.name = i.ratio.x + ":" + i.ratio.y;

        if (uiRectTransform != null && uiRect.size != uiRectTransform.rect.size || uiRect.position != uiRectTransform.rect.position)
        {
            uiRect = uiRectTransform.rect;
            if (uiRect.size.x > 0 && uiRect.size.y > 0)
            {
                UpdateUIScreenRect();
            }
        }
    }

    private void Start()
    {
        if (canvas.worldCamera == null)
        {
            var camera = Camera.allCameras.ToList().FirstOrDefault(x => x.orthographic);
            if (camera != null)
                canvas.worldCamera = camera;
        }

        if (uiRectTransform == null && !TryGetComponent(out uiRectTransform))
            uiRectTransform = GetComponentInChildren<RectTransform>();
    }

    protected Rect uiRect = default;
    private void LateUpdate()
    {
        if (canvas.worldCamera == null)
        {
            var camera = Camera.allCameras.ToList().FirstOrDefault(x => x.orthographic);
            if (camera != null)
                canvas.worldCamera = camera;
        }

        if (uiRect.size != uiRectTransform.rect.size || uiRect.position != uiRectTransform.rect.position)
        {
            uiRect = uiRectTransform.rect;
            if (uiRect.size.x > 0 && uiRect.size.y > 0)
            {
                UpdateUIScreenRect();
            }
        }
    }

    protected int factor;
    protected Vector2Int factorScreen;
    public void UpdateUIScreenRect()
    {
        uiScreenRect.size = uiRect.size;
        uiScreenRect.position = uiRect.position;

        factor = Factor(Screen.width, Screen.height);
        if (factor == 0)
            factor = 1;
        int wFactor = Screen.width / factor;
        int hFactor = Screen.height / factor;
        factorScreen = new Vector2Int(wFactor, hFactor);

        if (uiScreenRect.size.y < uiScreenRect.size.x)
        {
            currentOrientation = ScreenOrientation.Landscape;
            screenRatio = float.Parse((uiScreenRect.size.x / uiScreenRect.size.y).ToString("#0.00"));
            ratio = new Vector2Int((int)(screenRatio * 9), 9);
            uiScreenRect.ratio = factorScreen;
        }
        else
        {
            currentOrientation = ScreenOrientation.Portrait;
            screenRatio = float.Parse((uiScreenRect.size.y / uiScreenRect.size.x).ToString("#0.00"));
            ratio = new Vector2Int((int)(screenRatio * 9), 9);
            uiScreenRect.ratio = factorScreen;
        }

        CheckScaleRatio();

        if(isChangeSize)
            ChangeSizeCam();


        onSizeChanged?.Invoke(uiRect.size);
    }


    private void ChangeSizeCam()
    {
        if(oldSizeCam == 0)
        {
            oldSizeCam = canvas.worldCamera.orthographicSize;
        }

        if (screenRatio <= 1.44)
        {
            canvas.worldCamera.orthographicSize = oldSizeCam * (uiRect.size.y / 1920f) - 1;
        }
        else
        {
            canvas.worldCamera.orthographicSize = oldSizeCam * (uiRect.size.y / 1920f);
        }
    }

    Vector3 posR;
    Vector3 posL;
    Vector3 posT;
    Vector3 posB;

    public void ChangeSize(float minX, float maxX, float minY, float maxY)
    {
        StartCoroutine(IEChangeSize(minX, maxX, minY, maxY));


    }
    IEnumerator IEChangeSize(float minX, float maxX, float minY, float maxY)
    {
        yield return null; // Yield to allow the frame to update

        Camera cam = canvas.worldCamera;

        posR = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight / 2, -cam.transform.position.z));

        if (posR.x > maxX)
        {
            while (Mathf.Abs(posR.x - maxX) > 0.6f)
            {
                cam.orthographicSize -= 0.01f;
                posR = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight / 2, -cam.transform.position.z));
            }
        }
        else
        {
            while (posR.x - maxX > 0.6f)
            {
                cam.orthographicSize += 0.01f;
                posR = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight / 2, -cam.transform.position.z));
            }
        }

        posL = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight / 2, -cam.transform.position.z));

        while (minX - posL.x <= 0.6f)
        {
            cam.orthographicSize += 0.01f;
            posL = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight / 2, -cam.transform.position.z));
        }


        posT = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth / 2, cam.pixelHeight, -cam.transform.position.z));

        while (posT.y - maxY < 1.8f)
        {
            cam.orthographicSize += 0.01f;
            posT = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth / 2, cam.pixelHeight, -cam.transform.position.z));
            //Debug.LogError("posT: " + posT.y + " maxY: " + maxY + " minY: " + minY);
        }



        posB = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth / 2, 0, -cam.transform.position.z));

        while (minY - posB.y < 0.8f)
        {
            cam.orthographicSize += 0.01f;
            posB = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth / 2, 0, -cam.transform.position.z));
            //Debug.LogError("posB: " + posB.y + " maxY: " + maxY + " minY: " + minY);
        }


    }




    public int Factor(int a, int b)
    {
        return (b == 0) ? a : Factor(b, a % b);
    }

    protected void CheckScaleRatio()
    {
        DeviceScreenRatioScale check;
        check = deviceScreenRatioScales.LastOrDefault(x => x.ratio.x <= ratio.x);
        if (check == null)
            check = deviceScreenRatioScales.FirstOrDefault();
        canvasScaler.matchWidthOrHeight = check.scale;

   
        if (screenRatio <= 1.44)
        {
            is129?.Invoke(1.12f);
        }
        else if (screenRatio <= 1.79)
        {
            is159?.Invoke(1.0f);
        }
        else if (screenRatio <= 2.07)
        {
            is189?.Invoke(0.95f);
        }
        else
        {
            is219?.Invoke(0.9f);
        }
    }

    [Serializable]
    public class UIScreenRect
    {
        public Vector2 size = Vector2.one * -1f;
        public Vector2 position = Vector2.one * -1f;
        public Vector2 ratio = Vector2.one * -1f;
    }

    public enum ScreenOrientation
    {
        Portrait,
        Landscape
    }

    [Serializable]
    public class DeviceScreenRatioScale
    {
        [SerializeField]
        internal string name;
        [SerializeField]
        internal Vector2Int ratio = Vector2Int.one;
        internal float ratioScale => float.Parse((ratio.x * 1f / ratio.y + 0.00000000000000001f).ToString("#0.00"));
        [SerializeField] internal float scale;
    }
}