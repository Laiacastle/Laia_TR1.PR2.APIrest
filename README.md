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
