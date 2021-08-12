using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Background : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetScreenScale();
    }

    void SetScreenScale()
    {
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = Camera.main.aspect * cameraHeight;
        Size screenSize = new Size(3840, 2160);
        //SpriteRenderer bg = GetComponent<SpriteRenderer>();
        //bg.sprite = GetSprite(bgPaths[i], screenSize);
        Vector3 scale = transform.localScale;
        scale.y *= cameraHeight / screenSize.Height * 100;
        scale.x *= cameraWidth / screenSize.Width * 100;
        transform.localScale = scale;
    }
}
