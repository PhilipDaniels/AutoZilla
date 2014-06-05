<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

  <!-- Set debug flag -->
  <xsl:template match="/configuration/system.web/compilation/@debug|/system.web/compilation/@debug">
    <xsl:attribute name="debug">
      <xsl:value-of select="'$(wsDebugFlag)'"/>
    </xsl:attribute>
  </xsl:template>
  
</xsl:stylesheet>
