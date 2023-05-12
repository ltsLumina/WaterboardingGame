using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class Crossfade : MonoBehaviour
{
    [SerializeField] float transitionTime = 1f;
    public Animator transition;
    public IEnumerator LoadCrossfade()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        int newIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (newIndex >= SceneManager.sceneCountInBuildSettings)
        {
            newIndex = 0;
        } //If newIndex is bigger than or equal to the number of scenes, start over from the scene with an index of 0.

        SceneManager.LoadScene(newIndex);
    }

}
