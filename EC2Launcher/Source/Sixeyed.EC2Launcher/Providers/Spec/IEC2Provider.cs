using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.EC2.Model;

namespace Sixeyed.EC2Launcher.Providers.Spec
{
    /// <summary>
    /// Represents an AWS EC2 API provider
    /// </summary>
    public interface IEC2Provider : IDisposable
    {
        /// <summary>
        /// Gets/sets the AWS Access Key
        /// </summary>
        string AccessKey { get; set; }

        /// <summary>
        /// Gets/sets the AWS SecretKey
        /// </summary>
        string SecretKey { get; set; }

        /// <summary>
        /// Gets instance descriptions
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DescribeInstancesResponse DescribeInstances(DescribeInstancesRequest request);

        /// <summary>
        /// Starts instances
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        StartInstancesResponse StartInstances(StartInstancesRequest request);

        /// <summary>
        /// Stops instances
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        StopInstancesResponse StopInstances(StopInstancesRequest request);
    }
}
