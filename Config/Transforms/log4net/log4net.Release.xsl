<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

  <xsl:include href="log4net.defaults.xsl" />


  <!-- Turn off logging in release. -->
  <xsl:template match="/log4net/root/level/@value">
    <xsl:attribute name="value">
      <xsl:value-of select="'OFF'"/>
    </xsl:attribute>
  </xsl:template>

</xsl:stylesheet>
