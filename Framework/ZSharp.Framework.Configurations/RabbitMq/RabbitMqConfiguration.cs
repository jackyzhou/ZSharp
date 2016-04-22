﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ZSharp.Framework.Configurations
{
    public class RabbitMqConfiguration
    {
        private const int DefaultPort = 5672;
        public ushort Port { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// Heartbeat interval seconds. (default is 10)
        /// </summary>
        public ushort RequestedHeartbeat { get; set; }
        public ushort PrefetchCount { get; set; }
        public Uri AMQPConnectionString { get; set; }
        public IDictionary<string, object> ClientProperties { get; private set; } 
        public IEnumerable<HostConfiguration> Hosts { get; set; }
        /// <summary>
        /// Operation timeout seconds. (default is 10)
        /// </summary>
        public ushort Timeout { get; set; }
        public bool PublisherConfirms { get; set; }
        public bool PersistentMessages { get; set; }
        public bool CancelOnHaFailover { get; set; }
        public string Product { get; set; }
        public string Platform { get; set; }
        public bool UseBackgroundThreads { get; set; }

        /// <summary>
        /// This flag tells the server how to react if the message cannot be routed to a queue. 
        /// If this flag is true, the server will return an unroutable message with a BasicReturn method. 
        /// If this flag is false, the server silently drops the message.
        /// </summary>
        public bool Mandatory { get; set; }

        public RabbitMqConfiguration()
        {
            // set default values
            Port = DefaultPort;
            VirtualHost = "/";
            UserName = "guest";
            Password = "guest";
            RequestedHeartbeat = 10;
            Timeout = 10; // seconds
            PublisherConfirms = false;
            PersistentMessages = true;
            CancelOnHaFailover = false;
            UseBackgroundThreads = false;
            Mandatory = false;
            // prefetchCount determines how many messages will be allowed in the local in-memory queue
            // setting to zero makes this infinite, but risks an out-of-memory exception.
            // set to 50 based on this blog post:
            // http://www.rabbitmq.com/blog/2012/04/25/rabbitmq-performance-measurements-part-2/
            PrefetchCount = 50;
            
            Hosts = new List<HostConfiguration>();

            //Ssl = new SslOption();
        }

        private void SetDefaultClientProperties(IDictionary<string, object> clientProperties)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var applicationNameAndPath = Environment.GetCommandLineArgs()[0];

            var applicationName = "unknown";
            var applicationPath = "unknown";
            if (!string.IsNullOrWhiteSpace(applicationNameAndPath))
            {
                // Note: When running the application in an Integration Services Package (SSIS) the
                // Environment.GetCommandLineArgs()[0] can return null, and therefor it is not possible to get
                // the filename or directory name.
                try
                {
                    // Will only throw an exception if the applicationName contains invalid characters, is empty, or too long
                    // Silently catch the exception, as we will just leave the application name and path to "unknown"
                    applicationName = Path.GetFileName(applicationNameAndPath);
                    applicationPath = Path.GetDirectoryName(applicationNameAndPath);
                }
                catch (ArgumentException) { }
                catch (PathTooLongException) { }
            }

            var hostname = Environment.MachineName;
            var product = Product ?? applicationName;
            var platform = Platform ?? hostname;

            clientProperties.Add("client_api", "ZRabbitMq");
            clientProperties.Add("product", product);
            clientProperties.Add("platform", platform);
            clientProperties.Add("version", version);
            clientProperties.Add("rabbitmq_version", version);
            clientProperties.Add("application", applicationName);
            clientProperties.Add("application_location", applicationPath);
            clientProperties.Add("machine_name", hostname);
            clientProperties.Add("user", UserName);
            clientProperties.Add("connected", DateTime.UtcNow.ToString("u")); // UniversalSortableDateTimePattern: yyyy'-'MM'-'dd HH':'mm':'ss'Z'
            clientProperties.Add("requested_heartbeat", RequestedHeartbeat.ToString());
            clientProperties.Add("timeout", Timeout.ToString());
            clientProperties.Add("publisher_confirms", PublisherConfirms.ToString());
            clientProperties.Add("persistent_messages", PersistentMessages.ToString());
        }

        public void Validate()
        {
            if (AMQPConnectionString != null && !Hosts.Any(h => h.Host == AMQPConnectionString.Host))
            {
                if(Port == DefaultPort && AMQPConnectionString.Port > 0) 
                        Port = (ushort) AMQPConnectionString.Port;
                Hosts = Hosts.Concat(new[] {new HostConfiguration {Host = AMQPConnectionString.Host}});
            }
            if (!Hosts.Any())
            {
                throw new FrameworkException("Invalid connection string. 'host' value must be supplied. e.g: \"host=myserver\"");
            }
            foreach (var hostConfiguration in Hosts)
            {
                if (hostConfiguration.Port == 0)
                {
                    hostConfiguration.Port = Port;
                }
            }

            ClientProperties = new Dictionary<string, object>();
            SetDefaultClientProperties(ClientProperties);
        }
    }

    public class HostConfiguration
    {
        public string Host { get; set; }

        public ushort Port { get; set; }
    }
}