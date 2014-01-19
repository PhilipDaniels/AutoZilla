<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

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
