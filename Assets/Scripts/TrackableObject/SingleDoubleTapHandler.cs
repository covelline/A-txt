using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
///  EventTrigger はダブルタップでもシングルタップのイベントが発生するので
/// ダブルタップの場合はシングルタップをキャンセルするやつ.
/// </summary>
[RequireComponent(typeof(EventTrigger))]
public class SingleDoubleTapHandler : MonoBehaviour
{
    [Serializable]
    class OnClickEvent : UnityEvent<PointerEventData> { }

    [SerializeField]
    private float SingleClickDelaySecond = 0.5f;

    [SerializeField]
    private OnClickEvent OnSingleClick;
    [SerializeField]
    private OnClickEvent OnDoubleClick;

    private IEnumerator OnInvokeClickEnumrator = null;
    private int tapCount = 0;

    void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnPointerClick((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void OnPointerClick(PointerEventData data)
    {
        tapCount += 1;
        if (tapCount == 1) 
        {
            OnInvokeClickEnumrator = InvokeClick(data);
            StartCoroutine(OnInvokeClickEnumrator);
        }
    }

    private IEnumerator InvokeClick(PointerEventData data)
    {
        yield return new WaitForEndOfFrame();
        float time = 0;
        // 毎フレームごとに時間が過ぎていないかチェックする
        // WaitForSeconds だとなんか微妙な挙動だった
        while (time < SingleClickDelaySecond)
        {
            time += Time.deltaTime;
            yield return null;
        }

        switch (tapCount) 
        {
            case 1:
                Debug.Log("Invoke Single Click");
                OnSingleClick.Invoke(data);
                break;
            case 2:
                Debug.Log("Invoke Double Click");
                OnDoubleClick.Invoke(data);
                break;
            default:
                Debug.Log("Ignore Invoke Clicke count(" + tapCount + ")");
                break;
        }
        tapCount = 0;
    }

    void Update() { }
}
