<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html>
      <head>
        <style>
          table, th{
          border:2px solid #DEDFE0;
          border-collapse:collapse;
          border-top-color: #ef3e32;
          border-bottom-color: #ef3e32;
          font-family: "Arial Narrow", Arial, sans-serif; font-size: 14px;
          }
          td{
          border:2px solid #DEDFE0;
          border-collapse:collapse;
          font-family: "Arial Narrow", Arial, sans-serif; font-size: 12px;
          }
          h2 { font-family: "Arial Narrow", Arial, sans-serif; font-size: 18px; font-style: normal; font-variant: normal; font-weight: 700; line-height: 15.4px; }
        </style>
      </head>
      <body>
        <h2>List of model parameter outside the typical range of values.</h2>
        <table>
          <tr>
            <th style="text-align:center">Operation Type</th>
            <th style="text-align:center">Operation Number</th>
            <th style="text-align:center">Operation Name</th>
            <th style="text-align:center">Parameter Table</th>
            <th style="text-align:center">Parameter Name</th>
            <th style="text-align:center">Constituent Name</th>
            <th style="text-align:center">Parameter Value</th>
            <th style="text-align:center">Typical Minimum</th>
            <th style="text-align:center">Typical Maximum</th>
          </tr>
          <xsl:for-each select="DocumentElement/ModelParameterInfo">
            <tr>
              <td style="text-align:left">
                <xsl:value-of select="OPN_TYPE"/>
              </td>
              <td style="text-align:center">
                <xsl:value-of select="OPN_NUM"/>
              </td>
              <td style="text-align:left">
                <xsl:value-of select="OPN_INFO"/>
              </td>
              <td style="text-align:center">
                <xsl:value-of select="PARM_Table"/>
              </td>
              <td style="text-align:center">
                <xsl:value-of select="PARM_Name"/>
              </td>
              <td style="text-align:center">
                <xsl:value-of select="ConstName"/>
              </td>
              <td style="text-align:center">
                <xsl:value-of select="ModelValue"/>
              </td>
              <td style="text-align:center">
                <xsl:value-of select="TypicalMin"/>
              </td>
              <td style="text-align:center">
                <xsl:value-of select="TypicalMax"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>

