using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingMenu : MonoBehaviour
{
    void setFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }
}
