{WORLER ROLE

	{ID ROLE
		id = RoleEnvironment.CurrentRoleInstance.Id.Split('_')[2];
	}
	
	{INPUT CHANNEL FACTORY
		private void Connect()
        {
            ChannelFactory<IFindFilm> factory = new ChannelFactory<IFindFilm>(new NetTcpBinding(), new EndpointAddress($"net.tcp://localhost:8888/InputRequest"));
            proxy = factory.CreateChannel();
        }
	}
	
	{INTERNAL CHANNEL FACTORY
		private void ConnectToService()
        {
            foreach (RoleInstance r in RoleEnvironment.Roles["WorkerRole"].Instances)
            {
                if (r.Id != WorkerRole.id)
                {
                    IPEndPoint add = r.InstanceEndpoints["InternalRequest"].IPEndpoint;
                    ChannelFactory<IFindFilm> factory = new ChannelFactory<IFindFilm>(new NetTcpBinding(), new EndpointAddress($"net.tcp://{add}/InternalRequest"));
                    proxy.Add(factory.CreateChannel());
                }
            }
        }
	}
}

{INPUT JOB SERVER
	
	public class InputJobServer
    {
        private ServiceHost serviceHost;

        private String externalEndpointName = "InputRequest";
        public InputJobServer()
        {
            serviceHost = new ServiceHost(typeof(InputJobServerProvider));

            NetTcpBinding binding = new NetTcpBinding();
            //Default timeout je 60 sekundi pa puca na proxy tokom debug-a
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);

            RoleInstanceEndpoint internalEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[externalEndpointName];
            string endpointAddress = String.Format("net.tcp://{0}/{1}", internalEndpoint.IPEndpoint, externalEndpointName);

            try
            {
                serviceHost.AddServiceEndpoint(typeof(IFindFilm), binding, endpointAddress);
                Trace.TraceInformation("Host for {0} endpoint type created.", externalEndpointName);
            }
            catch (Exception e)
            {

                Trace.TraceError("ERROR: {0}", e.Message);
            }
        }
        public void Open()
        {
            try
            {
                serviceHost.Open();
                Trace.TraceInformation(String.Format("Host for {0} endpoint type opened successfully at {1} ", externalEndpointName, DateTime.Now));
            }
            catch (Exception e)
            {
                Trace.TraceInformation("Host open error for {0} endpoint type. Error message is: {1}. ", externalEndpointName, e.Message);
            }
        }
        public void Close()
        {
            try
            {
                serviceHost.Close();
                Trace.TraceInformation(String.Format("Host for {0} endpoint type closed successfully at {1}", externalEndpointName, DateTime.Now));
            }
            catch (Exception e)
            {
                Trace.TraceInformation("Host close error for {0} endpoint type. Error message is: {1}. ", externalEndpointName, e.Message);
            }
        }
    }
}

{INTERNAL JOB SERVER
	public class InternalJobServer
    {
        private ServiceHost serviceHost;

        private String externalEndpointName = "InternalRequest";
        public InternalJobServer()
        {
            serviceHost = new ServiceHost(typeof(InternalJobServerProvider));

            NetTcpBinding binding = new NetTcpBinding();
            //Default timeout je 60 sekundi pa puca na proxy tokom debug-a
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);

            RoleInstanceEndpoint internalEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[externalEndpointName];
            string endpointAddress = String.Format("net.tcp://{0}/{1}", internalEndpoint.IPEndpoint, externalEndpointName);

            try
            {
                serviceHost.AddServiceEndpoint(typeof(IFindFilm), binding, endpointAddress);
                Trace.TraceInformation("Host for {0} endpoint type created.", externalEndpointName);
            }
            catch (Exception e)
            {

                Trace.TraceError("ERROR: {0}", e.Message);
            }
        }
        public void Open()
        {
            try
            {
                serviceHost.Open();
                Trace.TraceInformation(String.Format("Host for {0} endpoint type opened successfully at {1} ", externalEndpointName, DateTime.Now));
            }
            catch (Exception e)
            {
                Trace.TraceInformation("Host open error for {0} endpoint type. Error message is: {1}. ", externalEndpointName, e.Message);
            }
        }
        public void Close()
        {
            try
            {
                serviceHost.Close();
                Trace.TraceInformation(String.Format("Host for {0} endpoint type closed successfully at {1}", externalEndpointName, DateTime.Now));
            }
            catch (Exception e)
            {
                Trace.TraceInformation("Host close error for {0} endpoint type. Error message is: {1}. ", externalEndpointName, e.Message);
            }
        }
    }
}