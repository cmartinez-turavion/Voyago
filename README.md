# Voyago

**Luxury Travel & VIP Concierge Platform**

Voyago es un prototipo web de alta fidelidad orientado a la exploracion y gestion futura de experiencias de viaje de lujo. La solucion utiliza ASP.NET Core 8, Razor Pages, Entity Framework Core 8, SQLite y ASP.NET Core Identity, con una identidad visual **Luxury Warm Charcoal & Gold**.

## Estado del prototipo

| Jornada | Alcance | Estado |
|---|---|---|
| Jornada 1 | Fundaciones tecnicas, Identity, roles, autorizacion, SQLite y layout | Completada |
| Jornada 2 | Modelo de dominio, configuracion EF Core, seed demostrativo y servicios de consulta | Completada y validada |
| Jornada 3 | Catalogo publico, busqueda, detalle de destinos, favoritos autenticados y regresion E2E | Completada y validada |
| Jornada 4 | Pipeline CI/CD, publicacion de resultados y evidencias de pruebas | Pendiente |

### Validacion al cierre de la Jornada 3

- Restauracion de paquetes exitosa.
- Compilacion exitosa de `Voyago`, `Voyago.Tests` y `Voyago.E2E`.
- Migraciones aplicadas correctamente en bases temporales de prueba.
- Seed demostrativo idempotente.
- **37 pruebas xUnit aprobadas**.
- **7 pruebas E2E con Playwright aprobadas**.
- **44 pruebas automatizadas aprobadas en total**.
- 0 pruebas con errores.
- 0 pruebas omitidas.
- SQLite temporal aislada para integracion y E2E.
- Limpieza automatica de la base E2E y del servidor al finalizar.
- Pipeline CI/CD pendiente para la jornada siguiente.

## Stack tecnologico

- .NET 8.
- ASP.NET Core 8.
- Razor Pages.
- MVC y controladores API cuando corresponda.
- Bootstrap 5.
- JavaScript moderno.
- Entity Framework Core 8.
- SQLite.
- ASP.NET Core Identity.
- Inyeccion de dependencias nativa.
- Logging mediante `ILogger`.
- xUnit para pruebas unitarias e integracion HTTP.
- NUnit y Microsoft Playwright para pruebas E2E.
- Chromium como navegador automatizado.

## Arquitectura del prototipo

El prototipo utiliza un unico proyecto web productivo para reducir complejidad. Las fronteras logicas se mantienen mediante carpetas y responsabilidades diferenciadas:

- **Presentacion:** Razor Pages, Areas, Controllers y ViewModels.
- **Aplicacion:** servicios de consulta y casos de uso.
- **Dominio:** entidades y enums.
- **Infraestructura:** `ApplicationDbContext`, configuraciones EF Core, migraciones y seed.

Los proyectos de pruebas son independientes y no forman parte del despliegue productivo.

## Estructura principal

```text
Voyago.sln
├── Voyago/
│   ├── Areas/
│   │   └── Admin/
│   ├── Controllers/
│   │   └── Api/
│   ├── Data/
│   │   ├── Configurations/
│   │   ├── Migrations/
│   │   ├── Seed/
│   │   │   └── E2ETestDataInitializer.cs
│   │   └── ApplicationDbContext.cs
│   ├── Models/
│   │   ├── Entities/
│   │   ├── Enums/
│   │   └── ViewModels/
│   ├── Pages/
│   │   └── Destinations/
│   ├── Services/
│   │   └── Queries/
│   ├── wwwroot/
│   │   ├── css/
│   │   ├── images/
│   │   └── js/
│   ├── Program.cs
│   ├── appsettings.json
│   └── Voyago.csproj
├── tests/
│   ├── Voyago.Tests/
│   └── Voyago.E2E/
├── scripts/
│   ├── run-voyago-e2e.sh
│   └── test-e2e.sh
└── docs/
    ├── SDD.md
    └── jornada-3-cierre-tecnico.md
```

## Requisitos locales

- .NET 8 SDK.
- Herramienta `dotnet-ef` 8.x.
- Git.
- Git Bash para ejecutar los scripts `.sh` en Windows.
- Visual Studio 2022, Visual Studio Code o equivalente.
- Python o el lanzador `py`, utilizado para generar identificadores unicos en los scripts E2E.
- Chromium instalado mediante Playwright.

Verificar el SDK instalado:

```bash
dotnet --version
```

Instalar `dotnet-ef` si no esta disponible:

```bash
dotnet tool install --global dotnet-ef --version 8.*
```

Actualizar una instalacion existente:

```bash
dotnet tool update --global dotnet-ef --version 8.*
```

## Configuracion

La conexion SQLite de desarrollo se encuentra en `Voyago/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=Data/voyago.db;Foreign Keys=True"
  }
}
```

La base local se crea en:

```text
Voyago/Data/voyago.db
```

Los archivos SQLite locales y temporales deben estar excluidos del repositorio:

```gitignore
Voyago/Data/*.db
Voyago/Data/*.db-shm
Voyago/Data/*.db-wal
.e2e-data/
TestResults/
playwright/.auth/
**/playwright-report/
**/playwright-traces/
```

No deben almacenarse contrasenas, tokens, API keys ni credenciales en `appsettings.json`.

## Restaurar y compilar

Desde la raiz del repositorio:

```bash
dotnet restore Voyago.sln
dotnet build Voyago.sln --no-restore
```

## Migraciones

Migraciones actuales:

```text
20260829120000_InitialIdentity
20260829140534_AddCoreDomainModel
```

Aplicar las migraciones:

```bash
dotnet ef database update \
  --project Voyago/Voyago.csproj \
  --startup-project Voyago/Voyago.csproj
```

Listar las migraciones:

```bash
dotnet ef migrations list \
  --project Voyago/Voyago.csproj \
  --startup-project Voyago/Voyago.csproj
```

Comprobar que el modelo no tenga cambios pendientes:

```bash
dotnet ef migrations has-pending-model-changes \
  --project Voyago/Voyago.csproj \
  --startup-project Voyago/Voyago.csproj
```

Resultado esperado:

```text
No changes have been made to the model since the last migration.
```

En desarrollo, la base debe prepararse de forma controlada. En los ambientes de pruebas, las migraciones se aplican sobre bases SQLite temporales y aisladas.

## Ejecutar la aplicacion

```bash
dotnet run --project Voyago/Voyago.csproj
```

El perfil de desarrollo utiliza las direcciones configuradas en `launchSettings.json`. Visual Studio tambien puede iniciar el proyecto con el perfil `Voyago`.

### Rutas disponibles

```text
/                                  Inicio
/Privacy                           Privacidad
/Error                             Manejo basico de errores
/Destinations                      Catalogo publico de destinos
/Destinations/{id}                 Detalle de un destino publicado
/Admin                             Area protegida
/Identity/Account/Register         Registro
/Identity/Account/Login            Inicio de sesion
/Identity/Account/Logout           Cierre de sesion
/api/v1/health                     Estado basico de la aplicacion
```

El catalogo publico de destinos y el flujo autenticado de favoritos estan operativos. Las paginas completas de paquetes, hoteles, vuelos y reservas permanecen pendientes.

## Identity, roles y autorizacion

`ApplicationDbContext` hereda de:

```text
IdentityDbContext<ApplicationUser>
```

Identity y las entidades del dominio utilizan la misma base SQLite.

### Campos adicionales de ApplicationUser

- `FullName`.
- `MembershipTier`.
- `MilesBalance`.
- `PreferredCurrency`.

### Roles iniciales

- `Traveler`.
- `Administrator`.

Los roles se crean mediante un inicializador idempotente.

### Politica administrativa

La politica `AdminOnly` exige el rol `Administrator` y protege toda el area `/Admin`.

### Asignar manualmente Administrator

La aplicacion no contiene credenciales administrativas ni contrasenas predeterminadas.

1. Registrar normalmente la cuenta en:

   ```text
   /Identity/Account/Register
   ```

2. Detener la aplicacion.
3. Configurar temporalmente el correo de una cuenta existente mediante User Secrets:

   ```bash
   dotnet user-secrets set \
     "BootstrapAdmin:Email" \
     "usuario@ejemplo.com" \
     --project Voyago/Voyago.csproj
   ```

4. Iniciar la aplicacion una vez:

   ```bash
   dotnet run --project Voyago/Voyago.csproj
   ```

5. Eliminar la configuracion temporal:

   ```bash
   dotnet user-secrets remove \
     "BootstrapAdmin:Email" \
     --project Voyago/Voyago.csproj
   ```

6. Cerrar e iniciar sesion para renovar la cookie de autenticacion.

Eliminar `BootstrapAdmin:Email` impide futuras promociones automaticas, pero no revoca el rol ya persistido en `AspNetUserRoles`.

### Revocar Administrator

La revocacion debe eliminar unicamente la asociacion del usuario con el rol. No debe eliminarse la definicion del rol `Administrator`.

Ejemplo para mantenimiento local controlado:

```sql
DELETE FROM AspNetUserRoles
WHERE UserId = (
    SELECT Id
    FROM AspNetUsers
    WHERE lower(Email) = lower('usuario@ejemplo.com')
)
AND RoleId = (
    SELECT Id
    FROM AspNetRoles
    WHERE NormalizedName = 'ADMINISTRATOR'
);
```

Realizar un respaldo de la base y detener la aplicacion antes de modificar SQLite manualmente.

## Modelo de dominio

### Catalogo

- `Destination`.
- `TourPackage`.
- `PackageItineraryDay`.
- `PackageInclusion`.
- `Hotel`.
- `HotelRoomType`.
- `FlightOffer`.

### Operacion preparada

- `Booking`.
- `Review`.
- `Favorite`.

`Favorite` ya participa en el flujo autenticado del detalle de destinos. Los flujos de reservas y publicacion de resenas todavia no estan disponibles en la interfaz.

### Enums

- `MembershipTier`.
- `FlightType`.
- `BookingProductType`.
- `BookingStatus`.
- `ReviewModerationStatus`.

## Relaciones y eliminacion

Se utilizan eliminaciones restringidas para proteger informacion operacional:

- Destino a paquetes.
- Destino a hoteles.
- Usuario a reservas.
- Usuario a resenas.
- Destino a resenas.
- Paquete a resenas.
- Destino a favoritos.

Las cascadas se limitan a dependencias internas:

- Paquete a dias de itinerario.
- Paquete a inclusiones.
- Hotel a tipos de habitacion.
- Usuario a favoritos.

`Booking.ProductId` no es una clave foranea directa porque la entidad de producto depende de `BookingProductType`. La existencia y el precio del producto deberan validarse en la capa de aplicacion cuando se implemente el flujo de reserva.

## Indices principales

- Indice unico de `Booking.BookingReference`.
- Indice unico compuesto de `Favorite.UserId + DestinationId`.
- Indice unico de `PackageItineraryDay.TourPackageId + DayNumber`.
- Indices para publicacion y destacados.
- Indices para destino, region, rutas, estados y fechas de consulta frecuente.

## Importes y fechas

Los importes se modelan como `decimal` y se configuran con precision conceptual `18,2` cuando corresponde.

SQLite conserva estos valores utilizando el tipo de almacenamiento determinado por el proveedor EF Core. Las operaciones futuras de ordenamiento, agregacion y rangos sobre importes deben mantenerse cubiertas por pruebas.

Las propiedades de auditoria utilizan nombres terminados en `Utc`. El codigo que cree o modifique reservas debera proporcionar valores UTC.

## Datos demostrativos

El seed demostrativo es idempotente y se utiliza en desarrollo y en las bases aisladas de pruebas.

| Entidad | Cantidad esperada |
|---|---:|
| Destinos | 8 |
| Paquetes | 10 |
| Dias de itinerario | 30 |
| Inclusiones y exclusiones | 30 |
| Hoteles | 8 |
| Tipos de habitacion | 16 |
| Vuelos | 8 |
| Resenas aprobadas | 10 |

Destinos incluidos:

- Costa Amalfitana.
- Kioto.
- Zermatt.
- Serengeti.
- Bora Bora.
- Patagonia.
- Santorini.
- Maldivas.

### Usuario tecnico del seed

Las resenas demostrativas se asocian a:

```text
demo-reviewer@voyago.local
```

Esta cuenta:

- No posee contrasena.
- No recibe el rol `Administrator`.
- No representa una cuenta real.
- Existe unicamente para conservar integridad referencial.

### Idempotencia

El seeder usa identificadores deterministas y agrega solamente registros ausentes. Ejecutar el seed nuevamente no debe duplicar el contenido demostrativo.

## Servicios de consulta

Se encuentran registrados mediante inyeccion de dependencias:

- `IDestinationQueryService`.
- `IPackageQueryService`.
- `IHotelQueryService`.
- `IFlightQueryService`.
- `IReviewQueryService`.

Los servicios:

- Utilizan `AsNoTracking`.
- Retornan DTOs.
- Retornan `PagedResult<T>`.
- Materializan internamente las consultas.
- No exponen `IQueryable`.
- Propagan `CancellationToken`.
- Limitan el tamano maximo de pagina.
- Excluyen catalogos no publicados.
- Excluyen resenas no aprobadas.

## Catalogo publico y favoritos

La Jornada 3 incorporo:

- Inicio con contenido real persistido.
- Catalogo publico de destinos.
- Busqueda sin distincion entre mayusculas y minusculas.
- Filtros combinables y paginacion.
- Detalle de destinos publicados.
- Paquetes y hoteles relacionados.
- Vista cartografica referencial.
- Recursos locales con fallback.
- Respuestas 404 para destinos inexistentes.
- Enlace de login con conservacion de `returnUrl`.
- Agregar y eliminar destinos favoritos.
- Persistencia del favorito despues de recargar la pagina.
- Validacion antiforgery en operaciones mutables.

## Ambientes de ejecucion y pruebas

### Development

- Utiliza la base local configurada en `appsettings.json`.
- Inicializa roles.
- Ejecuta el seed demostrativo.
- No crea el usuario E2E.

### IntegrationTesting

Utilizado exclusivamente por `WebApplicationFactory<Program>`.

- Crea una base SQLite temporal `voyago-tests-<guid>.db`.
- Aplica migraciones.
- Inicializa roles.
- Ejecuta el seed demostrativo.
- No crea el usuario Playwright.
- Elimina los archivos temporales al finalizar.

### Testing

Utilizado exclusivamente por el servidor externo de Playwright.

- Crea una base SQLite temporal en `.e2e-data/`.
- Aplica migraciones.
- Inicializa roles.
- Ejecuta el seed demostrativo.
- Crea un usuario `Traveler` determinista.
- Limpia los favoritos del usuario antes de comenzar.
- Elimina la base temporal al finalizar.

## Pruebas automatizadas

### Voyago.Tests

El proyecto `tests/Voyago.Tests` utiliza xUnit y cubre:

- Creacion de los datos demostrativos requeridos.
- Idempotencia del seed.
- Servicios de consulta del catalogo.
- Exclusiones por publicacion y moderacion.
- Paginacion y filtros.
- Busqueda case-insensitive de Patagonia.
- PageModels de Home y destinos.
- Restricciones y relaciones SQLite.
- Unicidad de `BookingReference`.
- Unicidad de favoritos por usuario y destino.
- Rutas publicas mediante `WebApplicationFactory`.
- Recursos estaticos.
- Respuestas 404.
- Redirecciones de autenticacion y `returnUrl`.
- Rechazo de solicitudes sin antiforgery.

Ejecucion:

```bash
dotnet test \
  tests/Voyago.Tests/Voyago.Tests.csproj \
  --logger "console;verbosity=detailed"
```

Resultado validado al cierre de la Jornada 3:

```text
Pruebas totales: 37
Correcto: 37
Incorrecto: 0
Omitido: 0
```

### Voyago.E2E

El proyecto `tests/Voyago.E2E` utiliza NUnit, Playwright y Chromium.

Escenarios automatizados:

- `Home_LoadsRealContent`.
- `Search_Patagonia_OpensDestination`.
- `Patagonia_Detail_ShowsRelatedContent`.
- `AnonymousLogin_PreservesDestinationReturnUrl`.
- `DestinationPage_HasNoFailedLocalResources`.
- `InvalidDestination_Returns404`.
- `AuthenticatedUser_CanAddPersistAndRemoveFavorite`.

La prueba autenticada valida:

1. Apertura de Patagonia.
2. Navegacion al login.
3. Conservacion de `returnUrl`.
4. Login real con ASP.NET Core Identity.
5. Agregar el destino a favoritos.
6. Persistencia despues de recargar.
7. Eliminacion del favorito.
8. Verificacion del estado final.

### Instalar Chromium para Playwright

Despues de compilar `Voyago.E2E`, ejecutar desde PowerShell:

```powershell
./tests/Voyago.E2E/bin/Debug/net8.0/playwright.ps1 install chromium
```

En un agente Linux o contenedor puede utilizarse:

```powershell
./tests/Voyago.E2E/bin/Debug/net8.0/playwright.ps1 install chromium --with-deps
```

### Regresion E2E automatizada

Desde Git Bash:

```bash
./scripts/test-e2e.sh
```

El script realiza automaticamente:

1. Generacion de un GUID.
2. Creacion de SQLite temporal.
3. Configuracion del ambiente `Testing`.
4. Configuracion del usuario E2E.
5. Restauracion y compilacion.
6. Inicio de Voyago en `http://localhost:65138`.
7. Espera del health check.
8. Ejecucion de las siete pruebas Playwright.
9. Detencion del servidor.
10. Eliminacion de SQLite.
11. Conservacion del log cuando ocurre un error.

Resultado validado:

```text
Pruebas totales: 7
Correcto: 7
Incorrecto: 0
Omitido: 0
```

### Resultado consolidado

```text
Voyago.Tests: 37 de 37
Voyago.E2E:     7 de 7
Total:         44 de 44
```

### Cobertura

```bash
dotnet test \
  tests/Voyago.Tests/Voyago.Tests.csproj \
  --collect:"XPlat Code Coverage"
```

## Verificacion manual de SQLite

Puede utilizarse **SQLite and SQL Server Compact Toolbox** en Visual Studio 2022 o la herramienta `sqlite3`.

Abrir la base local:

```bash
sqlite3 Voyago/Data/voyago.db
```

### Integridad referencial

```sql
PRAGMA foreign_keys;
PRAGMA foreign_key_check;
```

Resultados esperados:

- `PRAGMA foreign_keys` devuelve `1`.
- `PRAGMA foreign_key_check` no devuelve filas.

### Conteos del seed

```sql
SELECT 'Destinations' AS Entity, COUNT(*) AS Total FROM Destinations
UNION ALL
SELECT 'TourPackages', COUNT(*) FROM TourPackages
UNION ALL
SELECT 'PackageItineraryDays', COUNT(*) FROM PackageItineraryDays
UNION ALL
SELECT 'PackageInclusions', COUNT(*) FROM PackageInclusions
UNION ALL
SELECT 'Hotels', COUNT(*) FROM Hotels
UNION ALL
SELECT 'HotelRoomTypes', COUNT(*) FROM HotelRoomTypes
UNION ALL
SELECT 'FlightOffers', COUNT(*) FROM FlightOffers
UNION ALL
SELECT 'ApprovedReviews', COUNT(*)
FROM Reviews
WHERE ModerationStatus = 'Approved';
```

### Migraciones aplicadas

```sql
SELECT MigrationId, ProductVersion
FROM __EFMigrationsHistory
ORDER BY MigrationId;
```

## Secuencia completa de validacion

```bash
dotnet clean Voyago.sln

dotnet restore Voyago.sln

dotnet build Voyago.sln \
  --no-restore

dotnet test \
  tests/Voyago.Tests/Voyago.Tests.csproj \
  --no-build \
  --logger "console;verbosity=detailed"

./scripts/test-e2e.sh

dotnet ef migrations has-pending-model-changes \
  --project Voyago/Voyago.csproj \
  --startup-project Voyago/Voyago.csproj

dotnet ef migrations list \
  --project Voyago/Voyago.csproj \
  --startup-project Voyago/Voyago.csproj
```

Cada vez que se ejecute `dotnet clean`, debe ejecutarse posteriormente `dotnet build` antes de utilizar `dotnet test --no-build`.

## Seguridad y secretos

- No se almacenan contrasenas administrativas en el repositorio.
- No se almacenan datos de tarjetas.
- No se implementan pagos reales.
- No existen API keys de Google, IA o Workspace.
- User Secrets se utiliza para configuracion local sensible o temporal.
- Las bases SQLite locales y temporales no se versionan.
- Los archivos de estado autenticado de Playwright no se versionan.
- El area administrativa esta protegida por politica.
- El usuario E2E existe solo dentro de una base temporal de pruebas.
- Las credenciales E2E pueden sobrescribirse mediante variables de entorno.

Variables utilizadas por la regresion E2E:

```text
VOYAGO_E2E_BASE_URL
VOYAGO_E2E_EMAIL
VOYAGO_E2E_PASSWORD
```

## Funcionalidades aun no implementadas

- Paginas completas de paquetes.
- Paginas completas de hoteles.
- Paginas completas de vuelos.
- Flujo de reserva.
- Perfil VIP.
- Publicacion de resenas desde la interfaz.
- CRUD administrativo completo.
- Integracion real con Google Maps.
- Concierge y planificador de IA.
- Google Workspace.
- Dashboard operacional.
- Pagos.
- Pipeline CI/CD.
- Publicacion automatica de resultados TRX y evidencias Playwright.

## Flujo Git recomendado

La Jornada 3 se desarrollo en:

```text
feature/jornada-3-public-catalog
```

Verificar el estado:

```bash
git status
git log --oneline --decorate -10
```

Preparar los cambios:

```bash
git add -A
git diff --cached --stat
git diff --cached --check
```

Crear el commit de cierre:

```bash
git commit \
  -m "feat: cerrar jornada 3 con regresion automatizada" \
  -m "Incorpora catalogo publico, pruebas HTTP, Playwright, favoritos autenticados, SQLite temporal y documentacion. CI/CD queda pendiente para la siguiente jornada."
```

Publicar la rama:

```bash
git push -u origin feature/jornada-3-public-catalog
```

No deben versionarse:

```text
.vs/
bin/
obj/
Voyago/Data/voyago.db
Voyago/Data/voyago.db-shm
Voyago/Data/voyago.db-wal
.e2e-data/
TestResults/
playwright/.auth/
```

## Documentacion

La SDD maestra es la fuente principal de requisitos y decisiones del producto. El archivo `docs/SDD.md` referencia el documento maestro entregado al proyecto.

El cierre tecnico de la Jornada 3 debe registrarse en:

```text
docs/jornada-3-cierre-tecnico.md
```

Cualquier cambio de arquitectura, integraciones, persistencia, autenticacion o alcance debe contrastarse primero con la SDD.

## Pendiente para la jornada siguiente

El siguiente incremento corresponde a incorporar la regresion automatizada al pipeline CI/CD:

- Compilar la solucion.
- Ejecutar las 37 pruebas de `Voyago.Tests`.
- Instalar Chromium en el agente.
- Ejecutar las 7 pruebas de `Voyago.E2E`.
- Publicar resultados TRX.
- Conservar logs, capturas o trazas ante fallos.
- Configurar validaciones obligatorias para pull requests.

## Estado de cierre

La Jornada 3 se considera completada y validada al **30 de agosto de 2026**.

```text
Voyago.Tests: 37 de 37 aprobadas
Voyago.E2E:     7 de 7 aprobadas
Total:         44 de 44 aprobadas
```

El codigo y la documentacion quedan preparados para su publicacion en GitHub. El pipeline CI/CD se implementara en la jornada siguiente.
