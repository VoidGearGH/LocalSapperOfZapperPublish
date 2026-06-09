using UnityEngine;
using System.Collections;
public class ShowHideSettings : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private Animator _animator;
    public void Decide()
    {
        if (_settingsPanel.activeSelf) Hide();
        else Show();
    }
    private void Show()
    {
        _settingsPanel?.SetActive(true);
        _animator.SetBool("Show", true);
    }
    private void Hide()
    {
        _animator.SetBool("Hide", true);

        StartCoroutine(Repeat());
    }

    private IEnumerator Repeat()
    {
        yield return new WaitForSeconds(0.7f);

        _animator.SetBool("Show", false);
        _animator.SetBool("Hide", false);

        _settingsPanel.SetActive(false);
    }
}
