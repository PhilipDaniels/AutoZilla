<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="/configuration/appSettings/add[@key='Setting1']|/appSettings/add[@key='Setting1']">
    <add key="Setting1" value="$(appSetting1)" />
  </xsl:template>

  <xsl:template match="/configuration/appSettings/add[@key='Setting2']|/appSettings/add[@key='Setting2']">
    <add key="Setting2" value="$(appSetting2)" />
  </xsl:template>

  <!-- Replace <AppSettingsBlock /> with the whole set -->
  <xsl:template match="AppSettingsBlock" xml:space="preserve">
    <appSettings>
      <add key="Setting1" value="$(appSetting1)" />
      <add key="Setting2" value="$(appSetting2)" />
    </appSettings>
  </xsl:template>

</xsl:stylesheet>
