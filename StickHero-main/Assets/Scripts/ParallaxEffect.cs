using System.Collections;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform[] parallaxLayers;
    public float[] parallaxFactors;
    public Transform followTransform;
    public float smoothing = 1f;

    private Vector3[] initialPositions;

    private void Start()
    {
        followTransform = GameManager.instance.Player.transform;
        // Store the initial positions of the parallax layers
        initialPositions = new Vector3[parallaxLayers.Length];
        for (int i = 0; i < parallaxLayers.Length; i++)
        {
            initialPositions[i] = parallaxLayers[i].position;
        }

        // Start the coroutine
        StartCoroutine(ParallaxCoroutine());
    }

    private IEnumerator ParallaxCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < parallaxLayers.Length; i++)
            {
                // Calculate the target position based on the follow transform's movement
                Vector3 targetPosition = initialPositions[i] + (followTransform.position - initialPositions[i]) * parallaxFactors[i];

                // Move the layer towards the target position smoothly using Lerp
                parallaxLayers[i].position = Vector3.Lerp(parallaxLayers[i].position, targetPosition, smoothing * Time.deltaTime);
            }

            yield return null;
        }
    }
}