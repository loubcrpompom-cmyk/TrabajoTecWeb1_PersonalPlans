# PersonalPlans — API para Postman

Solución .NET 9 con cuatro proyectos: Api, Core, Infrastructure y Services. Endpoints y nombres de código en inglés. Los seis DTOs están en `PersonalPlans.Core/DTOs`.

## Antes de ejecutar
1. Instala .NET 9 SDK y MySQL 8.
2. En `PersonalPlans.Api/appsettings.json`, reemplaza `YOUR_MYSQL_PASSWORD` con tu contraseña local de MySQL. Cambia también `Jwt:Key` por una clave local de desarrollo de al menos 32 caracteres. No subas secretos a Git.
3. Crea la base `personalplans` en MySQL.
4. Abre `PersonalPlans.sln` y restaura paquetes.
5. En Package Manager Console, selecciona `PersonalPlans.Infrastructure` como Default project y ejecuta `Add-Migration UpdatePasswordField -StartupProject PersonalPlans.Api`; luego `Update-Database`. Si la base contiene información importante, haz una copia antes. La migración anterior esperaba `PasswordHash`; confirma que la nueva migración transforma esa columna según corresponda en tu base.
6. Ejecuta el script `Database/seed-activities.sql` después de aplicar las migraciones para cargar actividades de prueba. Ejecuta `PersonalPlans.Api`. URL HTTP predeterminada: `http://localhost:5002` (según launchSettings). OpenAPI JSON: `/openapi/v1.json` en Development.
7. Importa `Postman/PersonalPlans.postman_collection.json`.

## Administrador inicial
Registra un usuario por `/api/auth/register`, y luego cambia manualmente su columna `Role` a `Admin` en MySQL para el primer inicio. Después inicia sesión y copia el token de la respuesta a la variable `token` de Postman. El registro público siempre crea `User` para evitar que cualquier persona se registre como administrador.

## Reglas de uso importantes
- Para generar recomendaciones primero debe haber actividades activas cargadas en la tabla `Activities`.
- `SuitableCompanies` es una lista separada por comas, por ejemplo `Solo,Pareja,Amigos,Familia`.
- El costo del plan se calcula como costo por persona × número de personas.
- El historial se crea solo cuando se genera una recomendación.
- Los endpoints autenticados requieren `Authorization: Bearer <token>`; los endpoints de administración requieren rol `Admin`.
- El requisito académico de contraseñas sin hash se ha seguido tal como se indicó, pero es inseguro fuera del entorno académico.

## Endpoints incluidos
- CU-01: `POST /api/auth/register`
- CU-02: `POST /api/auth/login`
- CU-03: `POST /api/recommendations`
- CU-04: `GET /api/activities`, `GET /api/activities/{id}`, `POST /api/activities`, `PUT /api/activities/{id}`, `DELETE /api/activities/{id}`
- CU-05: `POST /api/favorites`, `GET /api/favorites`, `DELETE /api/favorites/{activityId}`
- CU-06: `GET /api/recommendations`, `GET /api/recommendations/{id}`
- CU-07: `PUT /api/activities/{activityId}/rating`, `GET /api/activities/{activityId}/rating`
- CU-08: `GET /api/users`, `PUT /api/users/{id}/role`, `PUT /api/users/{id}/status`

## Nota de verificación
El entorno donde se preparó este ZIP no dispone del comando `dotnet`, por lo que no fue posible ejecutar `dotnet build` ni pruebas de integración aquí. Abre la solución, restaura paquetes y compila localmente; cualquier error de compilación debe corregirse antes de usar la colección. El archivo `appsettings.json` no contiene la contraseña personal anterior.
