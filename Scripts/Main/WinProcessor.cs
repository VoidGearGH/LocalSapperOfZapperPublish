using UnityEngine;
using System.Collections;
public class WinProcessor : MonoBehaviour
{
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private Animator _animator;
    private void OnEnable()
    {
        FillingProcessor.OnWin += ProcessWin;
    }
    private void OnDisable()
    {
        FillingProcessor.OnWin -= ProcessWin;
    }
    private void ProcessWin()
    {
        _winPanel.SetActive(true);
        _animator.SetTrigger("IsWin");
        StartCoroutine(StopTime());
    }
    private IEnumerator StopTime()
    {
        yield return new WaitForSeconds(0.7f);

        Time.timeScale = 0;
    }
}
