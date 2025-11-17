<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html>
      <head>
        <title>Успішність студентів</title>
        <style>
          body { font-family: Arial, sans-serif; }
          table { border-collapse: collapse; width: 100%; margin-top: 20px; }
          th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }
	  td {
    		color: white; 
  	     }
          th { background-color: #f2f2f2; }
          .disciplines { list-style-type: none; padding-left: 0; }
        </style>
      </head>
      <body>
        <h2>Список студентів</h2>
        <table>
          <tr>
            <th>П.І.П.</th>
            <th>Факультет</th>
            <th>Кафедра</th>
            <th>Інші атрибути</th>
            <th>Дисципліни</th>
          </tr>
          <xsl:for-each select="Students/Student">
            <tr>
              <td><xsl:value-of select="Name"/></td>
              <td><xsl:value-of select="@Faculty"/></td>
              <td><xsl:value-of select="@Department"/></td>
              <td>
                <ul class="disciplines">
                  <xsl:for-each select="@*">
                    <xsl:if test="name() != 'Faculty' and name() != 'Department'">
                      <li><b><xsl:value-of select="name()"/>:</b> <xsl:value-of select="."/></li>
                    </xsl:if>
                  </xsl:for-each>
                </ul>
              </td>
              <td>
                <ul class="disciplines">
                  <xsl:for-each select="Disciplines/Discipline">
                    <li>
                      <xsl:value-of select="SubjectName"/>: <b><xsl:value-of select="Grade"/></b>
                    </li>
                  </xsl:for-each>
                </ul>
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>