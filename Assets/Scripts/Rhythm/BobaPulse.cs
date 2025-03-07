using System.Collections;
using UnityEngine;

// I don't like animation stuff so this isn't my code
public class BobaPulse : MonoBehaviour
{
    public float pulseSize = 1.2f; // Max scale multiplier
    public float pulseSpeed = 0.2f; // How fast it grows and shrinks

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void Pulse()
    {
        StopAllCoroutines(); // Stop any ongoing pulse animations
        StartCoroutine(PulseAnimation());
    }

    private IEnumerator PulseAnimation()
    {
        Vector3 targetScale = originalScale * pulseSize;
        
        // Grow
        float elapsed = 0f;
        while (elapsed < pulseSpeed)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / pulseSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;

        // Shrink back
        elapsed = 0f;
        while (elapsed < pulseSpeed)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / pulseSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale;
    }
}
