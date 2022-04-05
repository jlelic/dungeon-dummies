using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class FinalCutscene : MonoBehaviour
{
    Camera camera;
    [SerializeField] GameObject fires;
    [SerializeField] GameObject[] ha;
    [SerializeField] UnityEngine.Rendering.Volume volume;
    [SerializeField] Light2D global;
    private void Start()
    {
        camera = GetComponent<Camera>();
        StartCoroutine(Cutscene());
    }
    IEnumerator Cutscene()
    {
        iTween.MoveTo(camera.gameObject, new Vector3(5, -4.5f,-5), 10);
        yield return new WaitForSeconds(9);
        foreach(var h in ha)
        {
            h.SetActive(true);
            yield return new WaitForSeconds(1);
        }
        Time.timeScale = 0.5f;
        fires.SetActive(true);
        for (int i = 0; i < 30; i++)
        {
            global.intensity += 0.5f;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(0);
    }

}
