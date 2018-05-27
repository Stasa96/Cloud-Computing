using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole
{
    public class JobServer
    {
        private ServiceHost externalServiceHost;
        private String externalEPName = "InputRequest";
        RoleInstanceEndpoint externalEP;

        private ServiceHost internalServiceHost;
        private String internalEPName = "InternalRequest";
        RoleInstanceEndpoint internalEP;

        #region ExternalServiceHost
        public void AddExternalService()
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
            };

            externalServiceHost = new ServiceHost(typeof(ExternalJobProvider));
            externalEP = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[externalEPName];
            String externalAddress = $"net.tcp://{externalEP.IPEndpoint}/{externalEPName}";
            externalServiceHost.AddServiceEndpoint(typeof(IControlServiceHost), binding, externalAddress);
            Trace.TraceInformation("External service host setup. " + externalEP.IPEndpoint);

        }

        public void OpenExternalService()
        {
            try
            {
                externalServiceHost.Open();
                Trace.TraceInformation($"External service opened at {DateTime.Now} {externalEP.IPEndpoint}");
            }
            catch (Exception e)
            {
                Trace.TraceError($"ERROR: {e.Message}");
            }
        }

        public void CloseExternalService()
        {
            try
            {
                externalServiceHost.Close();
                Trace.TraceInformation($"External service closed at {DateTime.Now}");
            }
            catch (Exception e)
            {
                Trace.TraceError($"ERROR: {e.Message}");
            }
        }
        #endregion

        #region InternalServiceHost

        public void AddInternalService()
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
            };
            
            internalServiceHost = new ServiceHost(typeof(InternalJobProvider));
            internalEP = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[internalEPName];
            String internalAddress = $"net.tcp://{internalEP.IPEndpoint}/{internalEPName}";
            internalServiceHost.AddServiceEndpoint(typeof(IBrotherConnection), binding, internalAddress);
            Trace.TraceInformation("Internal service host setup. " + internalEP.IPEndpoint);
        }

        public void OpenInternalService()
        {
            try
            {
                //OBAVEZAN IF ZBOG CLOSE->OPEN->CLOSE->OPEN
                if (internalServiceHost.State == CommunicationState.Closed || internalServiceHost.State == CommunicationState.Closing)
               {
                    AddInternalService();
               }
                if(internalServiceHost.State != CommunicationState.Opened)
                    internalServiceHost.Open();
                    Trace.TraceInformation($"Internal service opened at {DateTime.Now} {internalEP.IPEndpoint}");
            }
            catch (Exception e)
            {
                Trace.TraceError($"ERROR: {e.Message}");
            }
        }

        public void CloseInternalService()
        {
            try
            {
                internalServiceHost.Close();
                Trace.TraceInformation($"Internal service closed at {DateTime.Now}");
            }
            catch (Exception e)
            {
                Trace.TraceError($"ERROR: {e.Message}");
            }
        }
        #endregion
    }
}
