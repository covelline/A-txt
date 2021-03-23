using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ActiveText
{
    using FloatFilterKernel = Func<IList<float>, float>;

    class Vector3Filter: Filter<Vector3>
    {
        private readonly Filter<float> xFilter;
        private readonly Filter<float> yFilter;
        private readonly Filter<float> zFilter;

        public Vector3Filter(int size, FloatFilterKernel kernel)
        {
            xFilter = new FloatFilter(size, kernel);
            yFilter = new FloatFilter(size, kernel);
            zFilter = new FloatFilter(size, kernel);
        }

        public Vector3Filter(
            Filter<float> xFilter, 
            Filter<float> yFilter, 
            Filter<float> zFilter)
        {
            this.xFilter = xFilter;
            this.yFilter = yFilter;
            this.zFilter = zFilter;
        }

        public void Add(Vector3 point)
        {
            xFilter.Add(point.x);
            yFilter.Add(point.y);
            zFilter.Add(point.z);
        }

        public Vector3 GetValue()
        {
            return new Vector3(
                xFilter.GetValue(),
                yFilter.GetValue(),
                zFilter.GetValue()
            );
        }
    }
}
