using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparent : MonoBehaviour
{
    public bool IsTransparent { get; private set; } = false;

    private MeshRenderer[] renderers;
    private WaitForSeconds delay = new WaitForSeconds(0.001f);
    private WaitForSeconds resetDelay = new WaitForSeconds(0.005f);
    private const float THRESHOLD_ALPHA = 0.15f;
    private const float THRESHOLD_MAX_TIMER = 0.15f;

    private bool isReseting = false;
    private float timer = 0f;               //투명 상태로 전환된 이후의 경과 시간 측정
    private Coroutine timeCheckCoroutine;
    private Coroutine resetCoroutine;
    private Coroutine becomeTransparentCoroutine;

    void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void BecomeTransparent()
   {
        if (IsTransparent)
        {
            timer = 0f;
            return;
        }

        if (resetCoroutine != null && isReseting)
        {
            isReseting = false;
            IsTransparent = false;
            StopCoroutine(resetCoroutine);
        }

        SetMaterialTransparent();
        IsTransparent = true;
        becomeTransparentCoroutine = StartCoroutine(BecomeTransparentCoroutine());
    }


    #region #런타임 중에 RenderingMode 바꾸는 메소드들
    // 모드 : 0 = Opaque, 1 = Cutout, 2 = Fade, 3 = Transparent
    private void SetMaterialRenderingMode(Material material, float mode, int renderQueue)
    {
        material.SetFloat("_Mode", mode);      
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = renderQueue;
    }
    
    //투명 모드 전환
    private void SetMaterialTransparent()
    {
        for(int i = 0; i < renderers.Length; i++)
        {
            foreach(Material material in renderers[i].materials)
                SetMaterialRenderingMode(material, 3f, 3000);
        }
    }
    
    //불투명 모드 전환
    private void SetMaterialOpaque()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            foreach (Material material in renderers[i].materials)
                SetMaterialRenderingMode(material, 0f, -1);
        }
    }
    #endregion

    //투명에서 원래 상태로 전환
    public void ResetOriginalTransparent()
    {
        SetMaterialOpaque();
        resetCoroutine = StartCoroutine(ResetOriginalTransparentCoroutine());
    }

    //투명 상태로 전환 코루틴
    private IEnumerator BecomeTransparentCoroutine()
    {
        while (true)
        {
            bool isComplete = true;

            for(int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].material.color.a > THRESHOLD_ALPHA)
                    isComplete = false;

                Color color = renderers[i].material.color;
                color.a -= Time.deltaTime;
                renderers[i].material.color = color;
            }

            if (isComplete)
            {
                CheckTimer();
                break;
            }

            yield return delay;
        }
    }

    //원래 상태로 전환 코루틴
    //일정 시간동안 알파 값을 증가시키면서 원래 상태로 되돌림
    private IEnumerator ResetOriginalTransparentCoroutine()
    {
        IsTransparent = false;

        while (true)
        {
            bool isComplete = true;

            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].material.color.a < 1f)
                    isComplete = false;

                Color color = renderers[i].material.color;
                color.a += Time.deltaTime;
                renderers[i].material.color = color;
            }

            if (isComplete)
            {
                isReseting = false;
                break;
            }

            yield return resetDelay;
        }
    }

    public void CheckTimer()
    {
        if (timeCheckCoroutine != null)
            StopCoroutine(timeCheckCoroutine);
        timeCheckCoroutine = StartCoroutine(CheckTimerCouroutine());
    }

    private IEnumerator CheckTimerCouroutine()
    {
        timer = 0f;

        while (true)
        {
            timer += Time.deltaTime;

            if(timer > THRESHOLD_MAX_TIMER)
            {
                isReseting = true;
                ResetOriginalTransparent();
                break;
            }

            yield return null;
        }
    }
}
