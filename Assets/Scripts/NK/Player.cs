// ## - NK
using UnityEngine;
using UnityEngine.SceneManagement;

// IMPORTANT!!IMPORTANT!!IMPORTANT!!IMPORTANT!!IMPORTANT!!IMPORTANT!!IMPORTANT!!IMPORTANT!!IMPORTANT!!
// ----------------------------------------------------------------------------------------------------
// <NOTE> : As Class<PlayerBehaviour> now inherits directly from Entity.cs, this script is now obsolete.
// ----------------------------------------------------------------------------------------------------

// This script can replace PlayerDamage.
public class Player : Entity
{
    [SerializeField]
    private int gameOverScene = 0;

    protected override void Death()
    {
        // JV - base.Death() can no longer be called as it is now an abstract function. To reenable this code, change Entity.Death to a virtual function
        //base.Death();
        SceneManager.LoadScene(gameOverScene);
    }
}
