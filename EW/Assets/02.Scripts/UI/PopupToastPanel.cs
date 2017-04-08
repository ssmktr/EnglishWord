using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupToastPanel : MonoBehaviour {

    public UILabel MessageLbl;
    Coroutine _Timer = null;

    public void ViewMessage(string message)
    {
        gameObject.SetActive(true);
        MessageLbl.text = message;

        if (_Timer != null)
            StopCoroutine(_Timer);
        _Timer = StartCoroutine(_OnTimer());
    }

    IEnumerator _OnTimer()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
