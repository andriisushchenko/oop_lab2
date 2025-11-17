<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html>
      <head>
        <title>Успішність студентів</title>
        <style>
          body { 
            font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif; 
            margin: 20px;
            background-color: #f9f9f9;
          }
          h2 {
            color: #333;
            border-bottom: 2px solid #007bff;
            padding-bottom: 5px;
          }
          table { 
            border-collapse: collapse; 
            width: 100%; 
            margin-top: 20px; 
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
          }
          th, td { 
            border: 1px solid #ddd; 
            padding: 12px; 
            text-align: left; 
            vertical-align: top;
          }
          th { 
            background-color: #007bff; 
            color: white;
            font-weight: bold;
          }
          tr:nth-child(even) {
            background-color: #f2f2f2; /* Чергування кольорів */
          }
          tr:hover {
            background-color: #e9ecef; /* Підсвічування при наведенні */
          }
          ul {
            margin: 0;
            padding-left: 20px;
          }
          li {
            padding-bottom: 5px;
          }
        </style>
      </head>
      <body>
        <h2>Список студентів</h2>
        <table>
          <thead>
            <tr>
              <th>П.І.П.</th>
              <th>Факультет</th>
              <th>Кафедра</th>
              <th>Дисципліни (Оцінка)</th>
              <th>Додаткова інформація (Атрибути)</th>
            </tr>
          </thead>
          <tbody>
            <xsl:for-each select="Students/Student">
              <tr>
                <td>
                  <b><xsl:value-of select="Name"/></b>
                </td>
                
                <td>
                  <xsl:value-of select="Faculty"/>
                </td>
                
                <td>
                  <xsl:value-of select="Department"/>
                </td>
                
                <td>
                  <ul>
                    <xsl:for-each select="Disciplines/Discipline">
                      <li>
                        <xsl:value-of select="SubjectName"/>: 
                        <b><xsl:value-of select="Grade"/></b>
                      </li>
                    </xsl:for-each>
                  </ul>
                </td>
                
                <td>
                  <ul>
                    <xsl:for-each select="@*">
                      <xsl:if test="name() != 'Faculty' and name() != 'Department'">
                        <li>
                          <xsl:value-of select="name()"/>: 
                          <i><xsl:value-of select="."/></i> </li>
                      </xsl:if>
                    </xsl:for-each>
                  </ul>
                </td>
                
              </tr>
            </xsl:for-each>
          </tbody>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>