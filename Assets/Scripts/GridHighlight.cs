using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHighlight : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<SpriteRenderer> spriteRenderers;
    [SerializeField] private List<SpriteRenderer> fadedSpriteRenderers;
    [SerializeField] float fullOpacity = 0.1f;
    [SerializeField] float fadedOpacity = 0.05f;
    public bool hovering = true;

    void Start()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        while (true)
        {
            if (hovering)
            {
                var full = new Color(1f, 1f, 1f, fullOpacity);
                var faded = new Color(1f, 1f, 1f, fadedOpacity);

                foreach (SpriteRenderer renderer in spriteRenderers)
                {
                    renderer.color = full;
                }
                foreach (SpriteRenderer renderer in fadedSpriteRenderers)
                {
                    renderer.color = faded;
                }
                if (fullOpacity >= 1f)
                {
                    continue;
                }
                yield return new WaitForSeconds(0.4f);
                fullOpacity += 0.1f;
                fadedOpacity += 0.05f;
            }
            else
            {
                while (true)
                {
                    var full = new Color(1f, 1f, 1f, fullOpacity);
                    var faded = new Color(1f, 1f, 1f, fadedOpacity);

                    foreach (SpriteRenderer renderer in spriteRenderers)
                    {
                        renderer.color = full;
                    }
                    foreach (SpriteRenderer renderer in fadedSpriteRenderers)
                    {
                        renderer.color = faded;
                    }
                    if (fullOpacity <= 0f)
                    {
                        Destroy(this);
                    }
                    yield return new WaitForSeconds(0.05f);
                    fullOpacity -= 0.1f;
                    fadedOpacity -= 0.5f;
                }
            }
        }
    }
}
        