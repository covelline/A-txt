using System;
using System.Collections.Generic;
using UnityEngine;

namespace ActiveText
{
    using FloatFilterKernel = Func<IList<float>, float>;

    // 角度は360度で0に戻るするため、連続値として平均値をとるとその境界でおかしくなる
    // 角度を sin, cos に分解し連続値としてそれぞれを平滑化する
    public class AngleFilter: Filter<float>
    {
        private readonly FloatFilter sinFilter;
        private readonly FloatFilter cosFilter;

        public AngleFilter(int size, FloatFilterKernel kernel)
        {
            this.sinFilter = new FloatFilter(size, kernel);
            this.cosFilter = new FloatFilter(size, kernel);
        }

        public void Add(float angleInDegree)
        {
            float radian = Mathf.Deg2Rad * angleInDegree;
            float sinValue = Mathf.Sin(radian);
            float cosValue = Mathf.Cos(radian);

            sinFilter.Add(sinValue);
            cosFilter.Add(cosValue);
        }

        public float GetValue()
        {
            float sinAverage = sinFilter.GetValue();
            float cosAverage = cosFilter.GetValue();

            float radian = Mathf.Atan2(sinAverage, cosAverage);
            return Mathf.Rad2Deg * radian;
        }

    }
}
