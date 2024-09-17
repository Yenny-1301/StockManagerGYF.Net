# Stock Manager 
## Descripción
Este proyecto es una API que permite gestionar inventarios de productos, implementada en .NET y conectada a una base de datos MySQL.
## Instalación
1. Clona el repositorio:
   ```bash
   git clone https://github.com/Yenny-1303/StockManagerGYF.Net.git
   ```

2. Instala los siguientes paquetes NuGet en StockManagerGYF:
   ```Microsoft.AspNetCore.Authentication.JwtBearer
   MySql.Data
   MySql.EntityFrameworkCore
   System.IdentityModel.Tokens.Jwt
   Swashbuckle.AspNetCore
   ```

3.  Instala los siguientes paquetes NuGet en StockManagerGYF.Data:
   ```Dapper
      MySql.Data
   ```

4. Modificar el conecctionString en el archivo appettings.json en caso de ser necesario especificando el server, user y password:
   ```
   MySqlConnection:server=server; user=user; database=database; password=password
   ```
5. Ejecutar los scripts para generar la estructura de la base de datos





