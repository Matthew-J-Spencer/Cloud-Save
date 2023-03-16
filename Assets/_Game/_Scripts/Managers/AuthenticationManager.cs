using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthenticationManager : MonoBehaviour
{
    private AsyncOperation _scene;

    private void Start()
    {
        _scene = SceneManager.LoadSceneAsync("Select");
        _scene.allowSceneActivation = false;
    }

    public async void AnonymousLoginClicked()
    {
        using (new LoaderSystem.Load())
        {
            await AuthService.LoginAnonymously();
            _scene.allowSceneActivation = true;
        }
    }

    public void GooglePlayLoginClicked()
    {
        // You have a bunch of authentication providers available while using Unity Authentication
        // Google Play, Facebook, Apple, Steam, Oculus, Apple Game Center and custom OpenID
        Debug.Log("I have not implemented this :)");
    }
}