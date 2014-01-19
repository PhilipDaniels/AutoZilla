<!-- This file is not used within the project or build process.
     It is simply documentation, a "cookbook" for how to achieve certain
     common operations using Xsl.
     
     How Xsl is Processed
     ====================
     See http://lenzconsulting.com/how-xslt-works/
     
     How XPath Works
     ===============
     See http://msdn.microsoft.com/en-us/library/ms256086(v=vs.85).aspx
     and http://msdn.microsoft.com/en-us/library/ms754602(v=vs.85).aspx
     for examples of XPath selectors.
-->


<!-- So called "identity template" copies all input to output.
     This is already included in the Transform.template.xsl file
     and matches everything. If you have no other matches, the
     result is that the output is a copy of the input.
-->
<xsl:template match="node()|@*" name="identity">
  <xsl:copy>
    <xsl:apply-templates select="node()|@*" />
  </xsl:copy>
</xsl:template>


<!-- ============================ Dealing with Attributes ============================ -->
<!-- Remove an attribute called "debug" from a specific node -->
<xsl:template match="system.web/compilation/@debug" />


<!-- Set the "debug" attribute to "true". n.b. Single quotes in the value!!!! -->
<xsl:template match="system.web/compilation/@debug">
  <xsl:attribute name="debug">
    <xsl:value-of select="'true'"/>
  </xsl:attribute>
</xsl:template>


<!-- Rewrite attributes on a specific node. This changes (or adds) the "value" node on the
     "file" node. I think it works by first copying all the attributes, then overriding one of them.
  	 See http://www.w3schools.com/xpath/xpath_functions.asp for other functions you can use.
     
     Alternative: see "Rewrite a specific node"
-->
<xsl:template match="/log4net/appender/file">
  <xsl:copy>
    <xsl:apply-templates select="@*" />
    <xsl:attribute name="value">
      <xsl:value-of select="concat('C:\temp\logs\', @value)"/>
    </xsl:attribute>
  </xsl:copy>
</xsl:template>


<!-- ============================ Dealing with Entire Nodes ============================ -->
<!-- Rewrite a specific node (element "add" with an attribute called "name" that has the
     value "ConnStr1") and replace it with new Xml -->
<xsl:template match="//connectionStrings/add[@name='ConnStr1']">
  <add name="ConnStr1" providerName="System.Data.SqlClient"
    connectionString="Data Source=localhost;Initial Catalog=Db1ForDebug;Integrated Security=True;" />
</xsl:template>


<!-- Remove a specific node (element "add" with name "ConnStr1") -->
<xsl:template match="//connectionStrings/add[@name='ConnStr1']" />


<!-- Replace text within a node. Also shows how to define a parameter.
     Given XML:

<someNode>
  <subNode>
    <lastNode>text here is replaced with value of param1</lastNode>
  </subNode>
</someNode>
-->
<xsl:param name="param1" />
<xsl:template match="someNode/subNode/lastNode/text()">
  <xsl:value-of select="$param1"/>
</xsl:template>
