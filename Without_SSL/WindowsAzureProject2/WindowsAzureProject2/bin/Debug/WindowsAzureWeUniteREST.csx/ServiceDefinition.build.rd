<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="WindowsAzureProject2" generation="1" functional="0" release="0" Id="04d62f54-87d5-46d6-af84-4ad4b093e755" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="WindowsAzureProject2Group" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="WeUniteRESTService:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/WindowsAzureProject2/WindowsAzureProject2Group/LB:WeUniteRESTService:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="WeUniteRESTService:?IsSimulationEnvironment?" defaultValue="">
          <maps>
            <mapMoniker name="/WindowsAzureProject2/WindowsAzureProject2Group/MapWeUniteRESTService:?IsSimulationEnvironment?" />
          </maps>
        </aCS>
        <aCS name="WeUniteRESTService:?RoleHostDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/WindowsAzureProject2/WindowsAzureProject2Group/MapWeUniteRESTService:?RoleHostDebugger?" />
          </maps>
        </aCS>
        <aCS name="WeUniteRESTService:?StartupTaskDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/WindowsAzureProject2/WindowsAzureProject2Group/MapWeUniteRESTService:?StartupTaskDebugger?" />
          </maps>
        </aCS>
        <aCS name="WeUniteRESTService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/WindowsAzureProject2/WindowsAzureProject2Group/MapWeUniteRESTService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="WeUniteRESTServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/WindowsAzureProject2/WindowsAzureProject2Group/MapWeUniteRESTServiceInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:WeUniteRESTService:Endpoint1">
          <toPorts>
            <inPortMoniker name="/WindowsAzureProject2/WindowsAzureProject2Group/WeUniteRESTService/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapWeUniteRESTService:?IsSimulationEnvironment?" kind="Identity">
          <setting>
            <aCSMoniker name="/WindowsAzureProject2/WindowsAzureProject2Group/WeUniteRESTService/?IsSimulationEnvironment?" />
          </setting>
        </map>
        <map name="MapWeUniteRESTService:?RoleHostDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/WindowsAzureProject2/WindowsAzureProject2Group/WeUniteRESTService/?RoleHostDebugger?" />
          </setting>
        </map>
        <map name="MapWeUniteRESTService:?StartupTaskDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/WindowsAzureProject2/WindowsAzureProject2Group/WeUniteRESTService/?StartupTaskDebugger?" />
          </setting>
        </map>
        <map name="MapWeUniteRESTService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/WindowsAzureProject2/WindowsAzureProject2Group/WeUniteRESTService/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapWeUniteRESTServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/WindowsAzureProject2/WindowsAzureProject2Group/WeUniteRESTServiceInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="WeUniteRESTService" generation="1" functional="0" release="0" software="D:\Official\Projects\Kani\Code\WebServicev2.0_LatestRavi\Without_SSL\WindowsAzureProject2\WindowsAzureProject2\bin\Debug\WindowsAzureWeUniteREST.csx\roles\WeUniteRESTService" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="?IsSimulationEnvironment?" defaultValue="" />
              <aCS name="?RoleHostDebugger?" defaultValue="" />
              <aCS name="?StartupTaskDebugger?" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WeUniteRESTService&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;WeUniteRESTService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/WindowsAzureProject2/WindowsAzureProject2Group/WeUniteRESTServiceInstances" />
            <sCSPolicyFaultDomainMoniker name="/WindowsAzureProject2/WindowsAzureProject2Group/WeUniteRESTServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="WeUniteRESTServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="WeUniteRESTServiceInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="42f0d89f-0810-4f03-8873-50748d387a7a" ref="Microsoft.RedDog.Contract\ServiceContract\WindowsAzureProject2Contract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="bd53f4c5-bced-4d8e-a397-f341a3c504e7" ref="Microsoft.RedDog.Contract\Interface\WeUniteRESTService:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/WindowsAzureProject2/WindowsAzureProject2Group/WeUniteRESTService:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>