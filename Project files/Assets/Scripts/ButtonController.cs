using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private GameObject selectedButton;

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(selectedButton);
        }
        else
        {
            selectedButton = EventSystem.current.currentSelectedGameObject;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerEnter.transform.parent.gameObject);
        EventSystem.current.SetSelectedGameObject(eventData.pointerEnter.transform.parent.gameObject);
    }
}
