using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MonsterInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject monsterInfoText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TextMeshProUGUI monsterName = GetComponent<TextMeshProUGUI>();
        monsterName.color = Color.grey;

        monsterInfoText.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TextMeshProUGUI monsterName = GetComponent<TextMeshProUGUI>();
        monsterName.color = Color.white;

        monsterInfoText.SetActive(false);
    }
}
