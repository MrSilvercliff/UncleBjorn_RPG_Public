using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics
{
    public static class WindowSafeAreaApplyer
    {
        public static void ApplySafeArea(Canvas canvas, RectTransform contentAnchorMin, RectTransform contentAnchorMax, RectTransform contentAnchorMinMax)
        {
            Rect safeArea = Screen.safeArea;
            var canvasPixelRect = canvas.pixelRect;

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= canvasPixelRect.width;
            anchorMin.y /= canvasPixelRect.height;

            anchorMax.x /= canvasPixelRect.width;
            anchorMax.y /= canvasPixelRect.height;

            if (contentAnchorMin != null)
                contentAnchorMin.anchorMin = anchorMin;

            if (contentAnchorMax != null)
                contentAnchorMax.anchorMax = anchorMax;

            if (contentAnchorMinMax != null)
            {
                contentAnchorMinMax.anchorMin = anchorMin;
                contentAnchorMinMax.anchorMax = anchorMax;
            }
        }
    }
}

