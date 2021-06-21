using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Animator>();
        _animator.SetTrigger("Death");
        Invoke("Reset", 3);
    }

    private void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
