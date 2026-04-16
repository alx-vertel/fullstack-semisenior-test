# fullstack-semisenior-test
Fullstack Semisenior Test. Tecnologías: .NET Web API, Angular and SQL Server

Este proyecto presenta una solución para la gestión de tareas y usuarios, construida utilizando .NET 10 en el Backend y Angular 21 con Tailwind CSS en el Frontend. Se decidió trabajar, como se puede ver, en las versiones más recientes de ambas tecnologías, para hacer uso de todas las herramientas y prácticas más actuales y comunes.

## Requisitos previos

Para correr el proyecto deberemos tener instalado: 

- SDK de .NET 10.0.x

- Node.js v22+ y npm

- SQL Server Express

- Angular CLI

## Configuración del entorno Backend

1. Navegar a la carpeta de la solución

```
cd TaskManagerApi
```

2. Restaurar dependencias:

```
dotnet restore
```

3. Configurar cadena de conexión en appsettings.json:

 ```
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLExpress;Database=TaskManagerDb;Trusted_Connection=True;TrustServerCertificate=True"
},
 ```

4. Ejecutar migraciones para crear la base de datos y su estructura (también se puede utilizar el script sql `create_db_script`):

```
dotnet tool restore
dotnet tool run ef database update
```

5. Iniciar servidor:

```
dotnet run
```
La documentación de la API puede observarse a través de: https://localhost:7174/scalar

## Configuración del Frontend (Angular)

1. Navega a la carpeta del cliente:

```
cd task-manager-front
```

2. Instala las dependencias:

```
npm install
```

3. Inicia la aplicación:

```
ng serve
```

La aplicación se abrirá en http://localhost:4200

## Decisiones Técnicas

### Arquitectura

Capa de Servicios: Se implementó una lógica de negocio de servicios y controladores. Esto permite centralizar las funcionalidades, mejorando su corrección y testeabilidad.

Inyección de Dependencias: Se usó AddScoped para garantizar la consistencia de los datos durante el ciclo de vida de las peticiones.

### Base de Datos y EF Core

Code-First con Fluent API: Se trabajó con Fluent API para mantener las entidades limpias y configurar restricciones complejas como el Check Constraint del JSON.

Índices Compuestos: Se creó un índice en (UserId, Status, CreatedAt) para optimizar el Dashboard, anticipando el crecimiento de la data.

### Frontend

Angular Signals: Se utilizaron Signals para el manejo del estado, aprovechando las mejoras de rendimiento y reactividad de las últimas versiones de Angular.

Tailwind CSS: Se eligió por su velocidad de desarrollo y para garantizar un diseño consistente y responsive.

Formularios Reactivos: Implementados para un control total sobre las validaciones y el mapeo de objetos complejos.

## Información adicional en formato JSON: 

El sistema maneja información no estructurada dentro de SQL Server:

1. Validación: La tabla UserTasks tiene un CHECK CONSTRAINT que usa la función `ISJSON()` para asegurar la integridad.

2. Consultas SQL: El servicio de UserTasks utiliza JSON_VALUE para realizar filtrados eficientes por prioridad directamente en el motor de base de datos:

```
SELECT * FROM UserTasks WHERE JSON_VALUE(AdditionalInfo, '$.Priority') = 'High'
```

3. Flexibilidad: El campo AdditionalInfo permite extender la aplicación (prioridad, tags, fechas estimadas) sin necesidad de alterar el esquema de la base de datos.


