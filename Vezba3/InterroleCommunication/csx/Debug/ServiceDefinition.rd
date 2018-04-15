<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="InterroleCommunication" generation="1" functional="0" release="0" Id="79045b96-c429-4469-8d03-47bd607a227b" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="InterroleCommunicationGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="JobWorker:InputRequest" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/InterroleCommunication/InterroleCommunicationGroup/LB:JobWorker:InputRequest" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="JobWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/InterroleCommunication/InterroleCommunicationGroup/MapJobWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="JobWorkerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/InterroleCommunication/InterroleCommunicationGroup/MapJobWorkerInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:JobWorker:InputRequest">
          <toPorts>
            <inPortMoniker name="/InterroleCommunication/InterroleCommunicationGroup/JobWorker/InputRequest" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:JobWorker:InternalRequest">
          <toPorts>
            <inPortMoniker name="/InterroleCommunication/InterroleCommunicationGroup/JobWorker/InternalRequest" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapJobWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/InterroleCommunication/InterroleCommunicationGroup/JobWorker/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapJobWorkerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/InterroleCommunication/InterroleCommunicationGroup/JobWorkerInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="JobWorker" generation="1" functional="0" release="0" software="D:\GitHub\III Godina\VI Semestar\Cloud-Computing\Vezba3\InterroleCommunication\csx\Debug\roles\JobWorker" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="InputRequest" protocol="tcp" portRanges="6000" />
              <inPort name="InternalRequest" protocol="tcp" />
              <outPort name="JobWorker:InternalRequest" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/InterroleCommunication/InterroleCommunicationGroup/SW:JobWorker:InternalRequest" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;JobWorker&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;JobWorker&quot;&gt;&lt;e name=&quot;InputRequest&quot; /&gt;&lt;e name=&quot;InternalRequest&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/InterroleCommunication/InterroleCommunicationGroup/JobWorkerInstances" />
            <sCSPolicyUpdateDomainMoniker name="/InterroleCommunication/InterroleCommunicationGroup/JobWorkerUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/InterroleCommunication/InterroleCommunicationGroup/JobWorkerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="JobWorkerUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="JobWorkerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="JobWorkerInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="631af47a-0d81-40cc-919e-c43eb90995a5" ref="Microsoft.RedDog.Contract\ServiceContract\InterroleCommunicationContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="1b84ec26-4a99-4377-8e4a-01bfb3c7c4fc" ref="Microsoft.RedDog.Contract\Interface\JobWorker:InputRequest@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/InterroleCommunication/InterroleCommunicationGroup/JobWorker:InputRequest" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>