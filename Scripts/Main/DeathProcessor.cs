using UnityEngine;
using System.Collections;
public class DeathProcessor : MonoBehaviour
{
    [SerializeField] private GameObject _deathPanel;
    [SerializeField] private Animator _animator;
    private void OnEnable()
    {
        FillingProcessor.OnDeath += ReactOnDeath;   
    }
    private void OnDisable()
    {
        FillingProcessor.OnDeath -= ReactOnDeath;
    }
    private void ReactOnDeath()
    {
        _deathPanel.SetActive(true);
        _animator.SetTrigger("IsDead");

        StartCoroutine(StopTime());
    }
    private IEnumerator StopTime()
    {
        yield return new WaitForSeconds(0.7f);

        Time.timeScale = 0;
    }
}
