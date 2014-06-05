<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">

  <xsl:template match="/configuration/appSettings/add[@key='log4net.Internal.Debug']|/appSettings/add[@key='log4net.Internal.Debug']">
    <add key="log4net.Internal.Debug" value="$(logInternalDebugFlag)" />
  </xsl:template>

  <!-- Set the logging level of the root logger. -->
  <xsl:template match="/configuration/log4net/root/level|/log4net/root/level">
    <level value="$(logDefaultLogLevel)" />
  </xsl:template>
  
  
  <!-- Set all conversion patterns in all appenders 
       See http://logging.apache.org/log4net/release/sdk/log4net.Layout.PatternLayout.html for the meanings.
  -->
  <xsl:template match="//conversionPattern/@value">
    <xsl:attribute name="value">
      <xsl:value-of select="'%date [%thread] %-5level %20.20method - %message - %logger%newline'"/>
    </xsl:attribute>
  </xsl:template>

  <!-- Set the log4net log file, it will appear in the same folder as the exe. -->
  <xsl:template match="/log4net/appender/file/@value">
    <xsl:attribute name="value">
      <xsl:value-of select="'AutoZilla.log'"/>
    </xsl:attribute>
  </xsl:template>
  
</xsl:stylesheet>
