<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="/configuration/connectionStrings/add[@name='ConnStr1']|/connectionStrings/add[@name='ConnStr1']">
    <add name="ConnStr1" providerName="System.Data.SqlClient" connectionString="$(csConnStr1)" />
  </xsl:template>

  <xsl:template match="/configuration/connectionStrings/add[@name='ConnStr2']|/connectionStrings/add[@name='ConnStr2']">
    <add name="ConnStr2" providerName="System.Data.SqlClient" connectionString="$(csConnStr2)" />
  </xsl:template>

  <!-- Replace <ConnectionStringsBlock /> with the whole set -->
  <xsl:template match="ConnectionStringsBlock" xml:space="preserve">
    <connectionStrings>
      <add name="ConnStr1" providerName="System.Data.SqlClient" connectionString="$(csConnStr1)" />
      <add name="ConnStr2" providerName="System.Data.SqlClient" connectionString="$(csConnStr2)" />
    </connectionStrings>
  </xsl:template>

</xsl:stylesheet>
