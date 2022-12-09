using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MonsterInfo : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public GameObject monsterInfoText;
    public MonsterType monsterType;
    private TextMeshProUGUI monsterName;

    void Start()
    {
        monsterName = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        monsterName.color = Color.grey;
        monsterInfoText.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameUI.SelectedMonster != null)
            GameUI.SelectedMonster.GetComponent<TextMeshProUGUI>().color = Color.black;
        GameUI.SelectedMonster = this;
        monsterName.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (this == GameUI.SelectedMonster)
            monsterName.color = Color.red;
        else
            monsterName.color = Color.black;

        monsterInfoText.SetActive(false);
    }
}
