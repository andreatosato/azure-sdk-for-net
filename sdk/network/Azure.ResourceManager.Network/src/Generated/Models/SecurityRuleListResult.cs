// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace Azure.ResourceManager.Network.Models
{
    /// <summary> Response for ListSecurityRule API service call. Retrieves all security rules that belongs to a network security group. </summary>
    internal partial class SecurityRuleListResult
    {
        /// <summary> Initializes a new instance of SecurityRuleListResult. </summary>
        internal SecurityRuleListResult()
        {
            Value = new ChangeTrackingList<SecurityRule>();
        }

        /// <summary> Initializes a new instance of SecurityRuleListResult. </summary>
        /// <param name="value"> The security rules in a network security group. </param>
        /// <param name="nextLink"> The URL to get the next set of results. </param>
        internal SecurityRuleListResult(IReadOnlyList<SecurityRule> value, string nextLink)
        {
            Value = value;
            NextLink = nextLink;
        }

        /// <summary> The security rules in a network security group. </summary>
        public IReadOnlyList<SecurityRule> Value { get; }
        /// <summary> The URL to get the next set of results. </summary>
        public string NextLink { get; }
    }
}
