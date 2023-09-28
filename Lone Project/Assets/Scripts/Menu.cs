using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayCampaign(){
        SceneManager.LoadScene(1);
    }
}
