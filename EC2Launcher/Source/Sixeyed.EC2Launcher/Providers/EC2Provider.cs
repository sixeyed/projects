using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sixeyed.EC2Launcher.Providers.Spec;
using Amazon.EC2;
using Amazon;
using Amazon.EC2.Model;
using Sixeyed.EC2Launcher.Configuration;

namespace Sixeyed.EC2Launcher.Providers
{
    /// <summary>
    /// Provider for the AWS EC2 API
    /// </summary>
    public class EC2Provider : IEC2Provider
    {
        private AmazonEC2 _ec2;
        private AmazonEC2 EC2
        {
            get
            {
                if (_ec2 == null)
                {
                    var config = new AmazonEC2Config().WithServiceURL(AwsConfiguration.ServiceUrl);
                    _ec2 = AWSClientFactory.CreateAmazonEC2Client(AccessKey, SecretKey, config);
                }
                return _ec2;
            }
        }

        /// <summary>
        /// Gets/sets the AWS Access Key
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// Gets/sets the AWS Secret Key
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Gets instance descriptions
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DescribeInstancesResponse DescribeInstances(DescribeInstancesRequest request)
        {
            return EC2.DescribeInstances(request);
        }

        /// <summary>
        /// Starts instances
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public StartInstancesResponse StartInstances(StartInstancesRequest request)
        {
            return EC2.StartInstances(request);
        }

        /// <summary>
        /// Stops instances
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public StopInstancesResponse StopInstances(StopInstancesRequest request)
        {
            return EC2.StopInstances(request);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _ec2 != null)
            {
                _ec2.Dispose();
                _ec2 = null;
            }
        }

        #endregion
    }
}
