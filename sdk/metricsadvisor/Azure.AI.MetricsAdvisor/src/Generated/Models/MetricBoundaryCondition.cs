// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Azure.AI.MetricsAdvisor.Models
{
    /// <summary> The ValueCondition. </summary>
    public partial class MetricBoundaryCondition
    {

        /// <summary> Initializes a new instance of MetricBoundaryCondition. </summary>
        /// <param name="lowerBound">
        /// lower bound
        /// 
        /// 
        /// 
        /// should be specified when direction is Both or Down.
        /// </param>
        /// <param name="upperBound">
        /// upper bound
        /// 
        /// 
        /// 
        /// should be specified when direction is Both or Up.
        /// </param>
        /// <param name="direction"> value filter direction. </param>
        /// <param name="type"> data used to implement value filter. </param>
        /// <param name="companionMetricId"> the other metric unique id used for value filter. </param>
        /// <param name="shouldAlertIfDataPointMissing">
        /// trigger alert when the corresponding point is missing in the other metric
        /// 
        /// 
        /// 
        /// should be specified only when using other metric to filter.
        /// </param>
        internal MetricBoundaryCondition(double? lowerBound, double? upperBound, BoundaryDirection direction, ValueType? type, string companionMetricId, bool? shouldAlertIfDataPointMissing)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Direction = direction;
            Type = type;
            CompanionMetricId = companionMetricId;
            ShouldAlertIfDataPointMissing = shouldAlertIfDataPointMissing;
        }
    }
}
