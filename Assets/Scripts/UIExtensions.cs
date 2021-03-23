using System;
using UnityEngine.UI;

namespace ActiveText
{
    public static class UIExtensions
    {
        // イベントを発火しないで値をセットする
        // Unity 2019 からあるけど 2018 にないので自分で作る
        public static void SetValueWithoutNotify(this Slider slider, float value)
        {
            var tmp = slider.onValueChanged;
            slider.onValueChanged = new Slider.SliderEvent();
            slider.normalizedValue = value;
            slider.onValueChanged = tmp;
        }
    }
}
