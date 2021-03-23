using System;
using UnityEngine;

namespace ActiveText
{
    public class StabilizedARCamera: ARCamera
    {
        private readonly Filter<Vector3> positionFilter = new Vector3Filter(10, FloatFilterKernels.average);
        private readonly Filter<Vector3> rotationFilter = new Vector3Filter(
            new AngleFilter(20, FloatFilterKernels.average),
            new AngleFilter(20, FloatFilterKernels.average),
            new AngleFilter(20, FloatFilterKernels.average)
        );

        override protected void ApplyTracking()
        {
            base.ApplyTracking();

            // transform を平滑化する
            positionFilter.Add(transform.position);
            rotationFilter.Add(transform.rotation.eulerAngles);
            var position = positionFilter.GetValue();
            var rotation = Quaternion.Euler(rotationFilter.GetValue());
            transform.SetPositionAndRotation(position, rotation);
        }
    }

}
