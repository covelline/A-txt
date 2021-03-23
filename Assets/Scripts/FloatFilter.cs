using System;
using System.Collections.Generic;
using System.Linq;

namespace ActiveText
{
    using FloatFilterKernel = Func<IList<float>, float>;

    public interface Filter<T>
    {
        void Add(T value);
        T GetValue();
    }

    public class FloatFilter: Filter<float>
    {
        private readonly IList<float> history = new List<float>();
        private readonly int size;
        private readonly FloatFilterKernel kernel;

        public FloatFilter(int size, FloatFilterKernel kernel)
        {
            this.size = size;
            this.kernel = kernel;
        }

        public void Add(float value)
        {
            history.Add(value);

            while (history.Count > size)
            {
                history.RemoveAt(0);
            }
        }

        public float GetValue()
        {
            return kernel(history);
        }
    }

    // フィルタの実装
    class FloatFilterKernels
    {
        // 平均値フィルタ
        public static FloatFilterKernel average = (IList<float> history) =>
        {
            var sum = 0f;
            foreach (var point in history)
            {
                sum += point;
            }
            return sum / history.Count;
        };

        // 中心値フィルタ
        public static FloatFilterKernel median = (IList<float> history) =>
        {
            var sorted = history.OrderBy(point => point).ToList();
            int centerIndex = Math.Max(0, (int)((sorted.Count() - 1) / 2f));
            return sorted[centerIndex];
        };
    }
}
