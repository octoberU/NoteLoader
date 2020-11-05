using UnityEngine;
using System.Collections;

namespace NoteLoader.UI
{
    public class Panel : MonoBehaviour
    {
        public void ShowPanel() 
        {
            gameObject.SetActive(true);
        }
        public void HidePanel()
        {
            gameObject.SetActive(false);
        }
    }
}