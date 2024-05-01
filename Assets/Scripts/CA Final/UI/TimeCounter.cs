using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace GamesForMobileDevices.CA_Final.UI
{
    public class TimeCounter : MonoBehaviour
    {
        TMP_Text _timerText;
        private float _currentTime = 60f;
        
        private void Start()
        {
            _timerText = GetComponent<TMP_Text>();

            StartCoroutine(CountdownTimer());
        }

        private IEnumerator CountdownTimer()
        {
            while (_currentTime > 0)
            {
                if (GameManager.instance.IsGameOver)
                {
                    _timerText.gameObject.SetActive(false);
                    break;
                }
                _timerText.text = $"{_currentTime:0}";
                yield return new WaitForSeconds(1);
                _currentTime--;
            }
            
            _timerText.text = "0";
            FindObjectOfType<GameUIManager>()?.ShowLevelWon();
            GameManager.instance.IsGameOver = true;
        }
    }
}