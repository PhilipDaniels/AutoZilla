<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" encoding="UTF-8"/>
  
  <!-- Default template copies all input to output -->
  <xsl:template match="node()|@*">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
    </xsl:copy>
  </xsl:template>

  <!-- This template stamps the document with a comment saying who generated it -->
  <xsl:template match="/">
    <xsl:comment> @@Comment </xsl:comment>
    <xsl:apply-templates />
  </xsl:template>

<!-- @@Includes -->
  
</xsl:stylesheet>
