# Voyago

**Luxury Travel & VIP Concierge Platform**

Voyago es un prototipo web de alta fidelidad para exploración y gestión futura de experiencias de viaje de lujo. La solución utiliza ASP.NET Core 8, Razor Pages, Entity Framework Core 8, SQLite y ASP.NET Core Identity, con una identidad visual **Luxury Warm Charcoal & Gold**.

## Estado del prototipo

| Jornada | Alcance | Estado |
|---|---|---|
| Jornada 1 | Fundaciones técnicas, Identity, roles, autorización, SQLite y layout | Completada |
| Jornada 2 | Modelo de dominio, configuración EF Core, seed demostrativo y servicios de consulta | Completada y validada |
| Jornada 3 | Pendiente de definición e inicio | No iniciada |

La Jornada 2 fue validada con:

- Restauración de paquetes exitosa.
- Compilación exitosa de `Voyago` y `Voyago.Tests`.
- Migraciones aplicadas correctamente.
- Seed demostrativo idempotente.
- 7 pruebas automatizadas aprobadas.
- 0 pruebas con errores.
- 0 pruebas omitidas.
- Sin cambios pendientes en el modelo de Entity Framework Core.

## Stack tecnológico

- .NET 8.
- ASP.NET Core 8.
- Razor Pages.
- MVC y controladores API cuando corresponda.
- Bootstrap 5.
- JavaScript moderno.
- Entity Framework Core 8.
- SQLite.
- ASP.NET Core Identity.
- Inyección de dependencias nativa.
- Logging mediante `ILogger`.
- xUnit para pruebas automatizadas.

## Arquitectura del prototipo

El prototipo utiliza un único proyecto web productivo para reducir complejidad. Las fronteras lógicas se mantienen mediante carpetas y responsabilidades diferenciadas:

```text
Presentación
  Razor Pages, Areas, Controllers, ViewModels

Aplicación
  Servicios de consulta y casos de uso

Dominio
  Entidades y enums

Infraestructura
  ApplicationDbContext, configuraciones EF Core, migraciones y seed
```

El proyecto de pruebas es independiente y no forma parte del despliegue productivo.

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
│   │   └── ApplicationDbContext.cs
│   ├── Models/
│   │   ├── Entities/
│   │   ├── Enums/
│   │   └── ViewModels/
│   ├── Pages/
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
│   └── Voyago.Tests/
└── docs/
    └── SDD.md
```

## Requisitos locales

- .NET 8 SDK.
- Herramienta `dotnet-ef` 8.x.
- Git.
- Visual Studio 2022, Visual Studio Code o equivalente.

Verificar el SDK instalado:

```bash
dotnet --version
```

Instalar `dotnet-ef` si no está disponible:

```bash
dotnet tool install --global dotnet-ef --version 8.*
```

Actualizar una instalación existente:

```bash
dotnet tool update --global dotnet-ef --version 8.*
```

## Configuración

La conexión SQLite se encuentra en `Voyago/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=Data/voyago.db;Foreign Keys=True"
  }
}
```

La base se crea en:

```text
Voyago/Data/voyago.db
```

Los archivos de SQLite están excluidos del repositorio:

```text
Data/*.db
Data/*.db-shm
Data/*.db-wal
```

No deben almacenarse contraseñas, tokens, API keys ni credenciales en `appsettings.json`.

## Restaurar y compilar

Desde la raíz del repositorio:

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

Las migraciones no se aplican automáticamente durante el arranque. En entornos distintos de desarrollo deben aplicarse mediante un proceso controlado.

## Ejecutar la aplicación

```bash
dotnet run --project Voyago/Voyago.csproj
```

El perfil de desarrollo utiliza las direcciones configuradas en `launchSettings.json`. Visual Studio también puede iniciar el proyecto con el perfil `Voyago`.

### Rutas base

```text
/                                 Inicio
/Privacy                          Privacidad
/Error                            Manejo básico de errores
/Admin                            Área protegida
/Identity/Account/Register        Registro
/Identity/Account/Login           Inicio de sesión
/Identity/Account/Logout          Cierre de sesión
/api/v1/health                    Estado básico de la aplicación
```

Las páginas completas del catálogo todavía no están implementadas. Los enlaces correspondientes permanecen deshabilitados hasta una jornada posterior.

## Identity, roles y autorización

`ApplicationDbContext` hereda de:

```csharp
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

Los roles se crean mediante un inicializador idempotente durante el arranque.

### Política administrativa

La política `AdminOnly` exige el rol `Administrator` y protege toda el área `/Admin`.

## Asignar manualmente Administrator

La aplicación no contiene credenciales administrativas ni contraseñas predeterminadas.

1. Registrar normalmente la cuenta en:

```text
/Identity/Account/Register
```

2. Detener la aplicación.

3. Configurar temporalmente el correo de una cuenta existente mediante User Secrets:

```bash
dotnet user-secrets set \
  "BootstrapAdmin:Email" \
  "usuario@ejemplo.com" \
  --project Voyago/Voyago.csproj
```

4. Iniciar la aplicación una vez:

```bash
dotnet run --project Voyago/Voyago.csproj
```

5. Eliminar la configuración temporal:

```bash
dotnet user-secrets remove \
  "BootstrapAdmin:Email" \
  --project Voyago/Voyago.csproj
```

6. Cerrar e iniciar sesión para renovar la cookie de autenticación.

> Eliminar `BootstrapAdmin:Email` impide futuras promociones automáticas, pero no revoca el rol ya persistido en `AspNetUserRoles`.

### Revocar Administrator

La revocación debe eliminar únicamente la asociación del usuario con el rol. No debe eliminarse la definición del rol `Administrator`.

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

Realizar un respaldo de la base y detener la aplicación antes de modificar SQLite manualmente.

## Modelo de dominio

### Catálogo

- `Destination`.
- `TourPackage`.
- `PackageItineraryDay`.
- `PackageInclusion`.
- `Hotel`.
- `HotelRoomType`.
- `FlightOffer`.

### Operación preparada

- `Booking`.
- `Review`.
- `Favorite`.

La presencia de estas entidades no implica que los flujos de reserva, reseñas o favoritos estén disponibles en la interfaz.

### Enums

- `MembershipTier`.
- `FlightType`.
- `BookingProductType`.
- `BookingStatus`.
- `ReviewModerationStatus`.

## Relaciones y eliminación

Se utilizan eliminaciones restringidas para proteger información operacional:

- Destino a paquetes.
- Destino a hoteles.
- Usuario a reservas.
- Usuario a reseñas.
- Destino a reseñas.
- Paquete a reseñas.
- Destino a favoritos.

Las cascadas se limitan a dependencias internas:

- Paquete a días de itinerario.
- Paquete a inclusiones.
- Hotel a tipos de habitación.
- Usuario a favoritos.

`Booking.ProductId` no es una clave foránea directa porque la entidad de producto depende de `BookingProductType`. La existencia y el precio del producto deberán validarse en la capa de aplicación cuando se implemente el flujo de reserva.

## Índices principales

- Índice único de `Booking.BookingReference`.
- Índice único compuesto de `Favorite.UserId + DestinationId`.
- Índice único de `PackageItineraryDay.TourPackageId + DayNumber`.
- Índices para publicación y destacados.
- Índices para destino, región, rutas, estados y fechas de consulta frecuente.

## Importes y fechas

Los importes se modelan como `decimal` y se configuran con precisión conceptual `18,2` cuando corresponde.

SQLite conserva estos valores utilizando el tipo de almacenamiento determinado por el proveedor EF Core. Las operaciones futuras de ordenamiento, agregación y rangos sobre importes deben mantenerse cubiertas por pruebas.

Las propiedades de auditoría utilizan nombres terminados en `Utc`. El código que cree o modifique reservas deberá proporcionar valores UTC.

## Datos demostrativos

El seed se ejecuta únicamente cuando el ambiente es `Development` y presupone que las migraciones ya fueron aplicadas.

Contenido esperado:

| Entidad | Cantidad |
|---|---:|
| Destinos | 8 |
| Paquetes | 10 |
| Días de itinerario | 30 |
| Inclusiones y exclusiones | 30 |
| Hoteles | 8 |
| Tipos de habitación | 16 |
| Vuelos | 8 |
| Reseñas aprobadas | 10 |

Destinos incluidos:

- Costa Amalfitana.
- Kioto.
- Zermatt.
- Serengeti.
- Bora Bora.
- Patagonia.
- Santorini.
- Maldivas.

### Usuario técnico del seed

Las reseñas demostrativas se asocian a:

```text
demo-reviewer@voyago.local
```

Esta cuenta:

- no posee contraseña;
- no recibe el rol `Administrator`;
- no representa una cuenta real;
- existe únicamente para conservar integridad referencial.

### Idempotencia

El seeder usa identificadores deterministas y agrega solamente registros ausentes. Ejecutar la aplicación nuevamente no debe duplicar el contenido demostrativo.

## Servicios de consulta

Se encuentran registrados mediante inyección de dependencias:

- `IDestinationQueryService`.
- `IPackageQueryService`.
- `IHotelQueryService`.
- `IFlightQueryService`.
- `IReviewQueryService`.

Los servicios:

- utilizan `AsNoTracking`;
- retornan DTOs;
- retornan `PagedResult<T>`;
- materializan internamente las consultas;
- no exponen `IQueryable`;
- propagan `CancellationToken`;
- limitan el tamaño máximo de página;
- excluyen catálogos no publicados;
- excluyen reseñas no aprobadas.

## Pruebas automatizadas

Ejecutar:

```bash
dotnet test Voyago.sln --no-build
```

Salida validada al cierre de Jornada 2:

```text
Pruebas totales: 7
Correcto: 7
Errores: 0
Omitido: 0
```

Las pruebas cubren:

1. Creación de los datos demostrativos requeridos.
2. Idempotencia del seed.
3. Retorno de datos desde todos los servicios del catálogo.
4. Exclusión de destinos no publicados y paginación.
5. Exclusión de reseñas no aprobadas.
6. Unicidad de `BookingReference` y round trip de importes y fechas.
7. Unicidad de favorito por usuario y destino.

Las pruebas emplean SQLite en memoria con claves foráneas habilitadas. No leen ni modifican `Voyago/Data/voyago.db`.

### Ejecución detallada

```bash
dotnet test Voyago.sln \
  --no-build \
  --logger "console;verbosity=detailed"
```

### Cobertura

```bash
dotnet test Voyago.sln \
  --collect:"XPlat Code Coverage"
```

## Verificación manual de SQLite

Puede utilizarse **SQLite and SQL Server Compact Toolbox** en Visual Studio 2022 o la herramienta `sqlite3`.

Abrir:

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

## Secuencia completa de validación

```bash
dotnet clean Voyago.sln
dotnet restore Voyago.sln
dotnet build Voyago.sln --no-restore
dotnet test Voyago.sln --no-build

dotnet ef migrations has-pending-model-changes \
  --project Voyago/Voyago.csproj \
  --startup-project Voyago/Voyago.csproj

dotnet ef migrations list \
  --project Voyago/Voyago.csproj \
  --startup-project Voyago/Voyago.csproj
```

## Seguridad y secretos

- No se almacenan contraseñas administrativas en el repositorio.
- No se almacenan datos de tarjetas.
- No se implementan pagos reales.
- No existen API keys de Google, IA o Workspace.
- User Secrets se utiliza para configuración local sensible o temporal.
- Los archivos SQLite no se versionan.
- El área administrativa está protegida por política.

## Funcionalidades aún no implementadas

- Páginas completas de destinos.
- Páginas completas de paquetes.
- Páginas completas de hoteles.
- Páginas completas de vuelos.
- Flujo de reserva.
- Perfil VIP.
- Favoritos desde la interfaz.
- Publicación de reseñas desde la interfaz.
- CRUD administrativo.
- Google Maps.
- Concierge y planificador de IA.
- Google Workspace.
- Dashboard operacional.
- Pagos.

## Flujo Git recomendado

La implementación de Jornada 2 se desarrolló en:

```text
feature/jornada-2-domain-model
```

Verificar el estado:

```bash
git status
git log --oneline --decorate -10
```

Publicar la rama:

```bash
git push -u origin feature/jornada-2-domain-model
```

No deben versionarse:

```text
.vs/
bin/
obj/
Voyago/Data/voyago.db
Voyago/Data/voyago.db-shm
Voyago/Data/voyago.db-wal
Voyago-Jornada2-Overlay/
Voyago-Jornada2-Tests-Overlay/
```

## Documentación

La SDD maestra es la fuente principal de requisitos y decisiones del producto. El archivo `docs/SDD.md` referencia el documento maestro entregado al proyecto.

Cualquier cambio de arquitectura, integraciones, persistencia, autenticación o alcance debe contrastarse primero con la SDD.

## Estado de cierre

La Jornada 2 se considera completada y validada al 29 de agosto de 2026.

El siguiente incremento corresponde a definir y ejecutar la Jornada 3 sin adelantar funcionalidades no autorizadas.
