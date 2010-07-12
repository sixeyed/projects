using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mhs.RemotePC.Providers.Spec;
using Amazon.EC2.Model;

namespace Mhs.RemotePC.Providers
{
    public class StubEC2Provider : IEC2Provider
    {
        public string AccessKey { get; set; }

        public string SecretKey { get; set; }

        public DescribeInstancesResponse DescribeInstances(DescribeInstancesRequest request)
        {
            throw new NotImplementedException();
        }

        public StartInstancesResponse StartInstances(StartInstancesRequest request)
        {
            throw new NotImplementedException();
        }

        public StopInstancesResponse StopInstances(StopInstancesRequest request)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //do nothing
        }
    }
}
