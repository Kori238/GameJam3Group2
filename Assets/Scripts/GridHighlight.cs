using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHighlight : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<SpriteRenderer> spriteRenderers;
    [SerializeField] private List<SpriteRenderer> fadedSpriteRenderers;
    static readonly List<float> fullOpacity = new List<float> {0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f};
    static readonly List<float> fadedOpacity = new List<float> {0f, 0.05f, 0.1f, 0.15f, 0.2f, 0.25f, 0.3f, 0.35f, 0.4f, 0.45f, 0.5f};
    [SerializeField] private int index = 0;
    public bool hovering = true;

    void Start()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        while (true)
        {
            var full = new Color(1f, 1f, 1f, fullOpacity[index]);
            var faded = new Color(1f, 1f, 1f, fadedOpacity[index]);

            foreach (SpriteRenderer renderer in spriteRenderers)
            {
                renderer.color = full;
            }
            foreach (SpriteRenderer renderer in fadedSpriteRenderers)
            { 
                renderer.color = faded;
            }
            switch (hovering)
            {
                case true when fullOpacity[index] >= 1f:
                    yield return new WaitForSeconds(0.4f); 
                    continue;
                case false when fullOpacity[index] <= 0f:
                    Destroy(this.gameObject);
                    break;
            }
            if (hovering)
            {
                yield return new WaitForSeconds(0.1f); 
                index += 1;
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
                index -= 1;
            }
        }
    }
}
        