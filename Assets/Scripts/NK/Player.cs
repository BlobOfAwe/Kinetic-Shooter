using UnityEngine;
using UnityEngine.SceneManagement;

// This script can replace PlayerDamage.
public class Player : Entity
{
    [SerializeField]
    private int gameOverScene = 0;

    protected override void Death()
    {
        base.Death();
        SceneManager.LoadScene(gameOverScene);
    }
}
