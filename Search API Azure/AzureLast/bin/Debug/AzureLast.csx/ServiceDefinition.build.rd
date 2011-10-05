<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="AzureLast" generation="1" functional="0" release="0" Id="489fb75b-7b91-4d8b-a678-c16e31e83a92" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="AzureLastGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="WebRole1:HttpIn" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/AzureLast/AzureLastGroup/LB:WebRole1:HttpIn" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="eBayPopularItemTrend:?IsSimulationEnvironment?" defaultValue="">
          <maps>
            <mapMoniker name="/AzureLast/AzureLastGroup/MapeBayPopularItemTrend:?IsSimulationEnvironment?" />
          </maps>
        </aCS>
        <aCS name="eBayPopularItemTrend:?RoleHostDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/AzureLast/AzureLastGroup/MapeBayPopularItemTrend:?RoleHostDebugger?" />
          </maps>
        </aCS>
        <aCS name="eBayPopularItemTrend:?StartupTaskDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/AzureLast/AzureLastGroup/MapeBayPopularItemTrend:?StartupTaskDebugger?" />
          </maps>
        </aCS>
        <aCS name="eBayPopularItemTrend:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AzureLast/AzureLastGroup/MapeBayPopularItemTrend:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="eBayPopularItemTrendInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AzureLast/AzureLastGroup/MapeBayPopularItemTrendInstances" />
          </maps>
        </aCS>
        <aCS name="WebRole1:?IsSimulationEnvironment?" defaultValue="">
          <maps>
            <mapMoniker name="/AzureLast/AzureLastGroup/MapWebRole1:?IsSimulationEnvironment?" />
          </maps>
        </aCS>
        <aCS name="WebRole1:?RoleHostDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/AzureLast/AzureLastGroup/MapWebRole1:?RoleHostDebugger?" />
          </maps>
        </aCS>
        <aCS name="WebRole1:?StartupTaskDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/AzureLast/AzureLastGroup/MapWebRole1:?StartupTaskDebugger?" />
          </maps>
        </aCS>
        <aCS name="WebRole1:DiagnosticsConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AzureLast/AzureLastGroup/MapWebRole1:DiagnosticsConnectionString" />
          </maps>
        </aCS>
        <aCS name="WebRole1Instances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AzureLast/AzureLastGroup/MapWebRole1Instances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:WebRole1:HttpIn">
          <toPorts>
            <inPortMoniker name="/AzureLast/AzureLastGroup/WebRole1/HttpIn" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapeBayPopularItemTrend:?IsSimulationEnvironment?" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureLast/AzureLastGroup/eBayPopularItemTrend/?IsSimulationEnvironment?" />
          </setting>
        </map>
        <map name="MapeBayPopularItemTrend:?RoleHostDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureLast/AzureLastGroup/eBayPopularItemTrend/?RoleHostDebugger?" />
          </setting>
        </map>
        <map name="MapeBayPopularItemTrend:?StartupTaskDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureLast/AzureLastGroup/eBayPopularItemTrend/?StartupTaskDebugger?" />
          </setting>
        </map>
        <map name="MapeBayPopularItemTrend:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureLast/AzureLastGroup/eBayPopularItemTrend/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapeBayPopularItemTrendInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AzureLast/AzureLastGroup/eBayPopularItemTrendInstances" />
          </setting>
        </map>
        <map name="MapWebRole1:?IsSimulationEnvironment?" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureLast/AzureLastGroup/WebRole1/?IsSimulationEnvironment?" />
          </setting>
        </map>
        <map name="MapWebRole1:?RoleHostDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureLast/AzureLastGroup/WebRole1/?RoleHostDebugger?" />
          </setting>
        </map>
        <map name="MapWebRole1:?StartupTaskDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureLast/AzureLastGroup/WebRole1/?StartupTaskDebugger?" />
          </setting>
        </map>
        <map name="MapWebRole1:DiagnosticsConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureLast/AzureLastGroup/WebRole1/DiagnosticsConnectionString" />
          </setting>
        </map>
        <map name="MapWebRole1Instances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AzureLast/AzureLastGroup/WebRole1Instances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="eBayPopularItemTrend" generation="1" functional="0" release="0" software="D:\Official\Projects\Kani\Shopping API Code\Search API Azure\AzureLast\bin\Debug\AzureLast.csx\roles\eBayPopularItemTrend" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="?IsSimulationEnvironment?" defaultValue="" />
              <aCS name="?RoleHostDebugger?" defaultValue="" />
              <aCS name="?StartupTaskDebugger?" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;eBayPopularItemTrend&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;eBayPopularItemTrend&quot; /&gt;&lt;r name=&quot;WebRole1&quot;&gt;&lt;e name=&quot;HttpIn&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AzureLast/AzureLastGroup/eBayPopularItemTrendInstances" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="WebRole1" generation="1" functional="0" release="0" software="D:\Official\Projects\Kani\Shopping API Code\Search API Azure\AzureLast\bin\Debug\AzureLast.csx\roles\WebRole1" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="HttpIn" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="?IsSimulationEnvironment?" defaultValue="" />
              <aCS name="?RoleHostDebugger?" defaultValue="" />
              <aCS name="?StartupTaskDebugger?" defaultValue="" />
              <aCS name="DiagnosticsConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WebRole1&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;eBayPopularItemTrend&quot; /&gt;&lt;r name=&quot;WebRole1&quot;&gt;&lt;e name=&quot;HttpIn&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AzureLast/AzureLastGroup/WebRole1Instances" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyID name="eBayPopularItemTrendInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="WebRole1Instances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="48dc6020-b749-48cf-977f-fdcbdb3d530d" ref="Microsoft.RedDog.Contract\ServiceContract\AzureLastContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="e75c23fd-288e-41ad-86aa-be4c699cbc11" ref="Microsoft.RedDog.Contract\Interface\WebRole1:HttpIn@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/AzureLast/AzureLastGroup/WebRole1:HttpIn" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>