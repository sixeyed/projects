using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Amazon.EC2.Model;
using Sixeyed.EC2Launcher.Configuration;
using Sixeyed.EC2Launcher.Entities;
using Sixeyed.EC2Launcher.Providers.Spec;

namespace Sixeyed.EC2Launcher.Providers
{
    /// <summary>
    /// Wrapper for accessing the current <see cref="IEC2Provider"/>
    /// </summary>
    public static class EC2
    {
        static EC2()
        {
            Container.Register<IEC2Provider, EC2Provider>(Lifetime.Singleton);
            Provider.AccessKey = AwsConfiguration.AccessKey;
            Provider.SecretKey = AwsConfiguration.SecretKey;
        }

        /// <summary>
        /// Gets the current provider
        /// </summary>
        public static IEC2Provider Provider
        {
            get { return Container.Get<IEC2Provider>(); }
        }

        /// <summary>
        /// Returns whether the provider is configured to make API calls
        /// </summary>
        /// <returns></returns>
        public static bool IsConfigured()
        {
            return !string.IsNullOrEmpty(Provider.AccessKey) &&
                   !string.IsNullOrEmpty(Provider.SecretKey);
        }

        /// <summary>
        /// Gets the current state of an EC2 instance
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public static EC2Instance GetInstance(string instanceId)
        {
            var instance = new EC2Instance() { InstanceId = instanceId };
            var request = new DescribeInstancesRequest();
            request.InstanceId = new List<string>();
            request.InstanceId.Add(instanceId);
            try
            {
                var response = Provider.DescribeInstances(request);
                var reservation = response.DescribeInstancesResult.Reservation[0];
                var runningInstance = (from i in reservation.RunningInstance
                                       where i.InstanceId == instanceId
                                       select i).FirstOrDefault();
                if (runningInstance != null)
                {
                    instance.CurrentStateName = runningInstance.InstanceState.Name;
                    if (instance.IsRunning())
                    {
                        instance.PublicDnsName = runningInstance.PublicDnsName;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error calling AWS.DescribeInstances: {0}", ex.Message));
            }
            return instance;
        }

        /// <summary>
        /// Starts an EC2 instance
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public static EC2Instance StartInstance(string instanceId)
        {
            var instance = new EC2Instance() { InstanceId = instanceId };
            var request = new StartInstancesRequest();
            request.InstanceId = new List<string>();
            request.InstanceId.Add(instanceId);
            try
            {
                var response = EC2.Provider.StartInstances(request);
                var stateChanges = response.StartInstancesResult.StartingInstances;
                var runningInstance = (from i in stateChanges
                                       where i.InstanceId == instanceId
                                       select i).FirstOrDefault();
                if (runningInstance != null)
                {
                    instance.CurrentStateName = runningInstance.CurrentState.Name;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error calling AWS.StartInstances: {0}", ex.Message));
            }
            return instance;
        }

        /// <summary>
        /// Stops an EC2 instance
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public static EC2Instance StopInstance(string instanceId)
        {
            var instance = new EC2Instance() { InstanceId = instanceId };
            var request = new StopInstancesRequest();
            request.InstanceId = new List<string>();
            request.InstanceId.Add(instanceId);
            try
            {
                var response = EC2.Provider.StopInstances(request);
                var stateChanges = response.StopInstancesResult.StoppingInstances;
                var runningInstance = (from i in stateChanges
                                       where i.InstanceId == instanceId
                                       select i).FirstOrDefault();
                instance.CurrentStateName = runningInstance.CurrentState.Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error calling AWS.StopInstances: {0}", ex.Message));
            }
            return instance;
        }
    }
}
