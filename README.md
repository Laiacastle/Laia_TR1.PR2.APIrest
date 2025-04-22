# SOLUCIÓ PROPOSADA 
## Descripció del problema

En aquesta ocasió havia de fer una aplicació amb diferents jocs que podian ser votats per els usuaris.
Aquesta aplicació consta d'usuaris, tan usuaris normals com administradors, una pàgina principal on es veuen els diferents jocs i votarl'los, també hi ha una página per veure els jocs més detalladament.
A part hi haurà un xat en temp real per el usuaris, la pàgina de login, registre i el perfil d lúsuari.

## Estructura

L'API REST fa servir ASP.NET Core per gestionar diverses operacions, com ara l'autenticació d'usuaris, la gestió de videojocs i un sistema de votació per a usuaris autenticats. L'arquitectura de l'API es divideix en dos controladors principals:

__AuthController__: Aquest controlador s'encarrega de tot el procés d'autenticació, que inclou el registre d'usuaris, la validació de credencials i la generació de tokens JWT per garantir la seguretat. Aquesta separació ajuda a mantenir la lògica d'autenticació independent de les altres funcionalitats de l'API, resultant en un sistema més net i escalable.

__GamesController__: Aquí és on es gestionen els videojocs. Ofereix operacions CRUD (crear, llegir, actualitzar i eliminar) per als videojocs, així com la possibilitat que els usuaris autenticats votin els jocs. Els administradors tenen permisos especials per modificar o eliminar videojocs, i les operacions estan restringides per rols gràcies a l'ús de ASP.NET Identity.

## Tecnologies i Estratègies Utilitzades
ASP.NET Core és la base de l'API, ja que permet construir aplicacions RESTful de manera eficaç i escalable, amb un enfocament clar en la seguretat i el rendiment.

Entity Framework Core s'utilitza per gestionar la persistència de dades en una base de dades SQL Server. EF Core facilita la creació i gestió de taules, incloent les relacions molts-a-molts entre usuaris i jocs, així com la creació automàtica de taules de relació.

JWT (JSON Web Tokens) s'usa per autenticar usuaris. Els tokens es generen quan l'usuari s'autentica i són necessaris per accedir a les funcionalitats protegides de l'API, assegurant que només els usuaris autenticats puguin interactuar amb els recursos protegits.

SignalR s'implementa per permetre la comunicació en temps real entre usuaris. Això és especialment útil per a funcionalitats com el xat en temps real, millorant així l'experiència de l'usuari.

## Biblografia
1. 
  Microsoft. (15 de marzo de 2025). 
  Crear una API REST con ASP.NET Core.
  Microsoft Docs. Recuperat el 17 d’abril de 2025 de:
  https://learn.microsoft.com/es-es/aspnet/core/web-api/?view=aspnetcore-7.0

2. 
  Microsoft. (07 de noviembre de 2024).
  Autorización simple en ASP.NET Core
  Microsoft Docs. Recuperat el 17 d'abril de 2025 de:
  https://learn.microsoft.com/es-es/aspnet/core/security/authorization/simple?view=aspnetcore-9.0

3.  
  Microsoft. (07 de noviembre de 2024).
  Validación de modelos de Identity en ASP.NET Core
  Microsoft Docs. Recuperat el 20 d'abril de 2025 de:
  https://learn.microsoft.com/es-es/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-9.0

4.  

   Microsoft. (1 de febrer de 2025).
   ASP.NET Core Identity.
   Microsoft Docs. Recuperat el 20 d’abril de 2025 de:
   https://learn.microsoft.com/es-es/aspnet/core/security/authentication/identity?view=aspnetcore-9.0&tabs=visual-studio

5.  

   
   Anònim, username: squidy (22 de juny de 2023.
   Reading token in ASP.net Core Razor pages WebApp
   StackOverflow. Recuperat el 21 d'abril de 2025 de:
   https://stackoverflow.com/questions/76798515/reading-token-in-asp-net-core-razor-pages-webapp

