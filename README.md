# Voyago, Jornada 1

Base ejecutable del prototipo Voyago con .NET 8, Razor Pages, MVC/API, EF Core 8, SQLite e Identity.

## Requisitos

- .NET 8 SDK.
- Herramienta `dotnet-ef` 8.x.

## Restaurar y compilar

```bash
dotnet restore Voyago.sln
dotnet build Voyago.sln --no-restore
```

## Aplicar migración

```bash
dotnet tool install --global dotnet-ef --version 8.*
dotnet ef database update --project Voyago/Voyago.csproj --startup-project Voyago/Voyago.csproj
```

La base se crea en `Voyago/Data/voyago.db`.

## Ejecutar

```bash
dotnet run --project Voyago/Voyago.csproj
```

Abre la URL HTTPS mostrada por la consola. Identity expone registro, inicio y cierre de sesión mediante su UI integrada.

## Asignar manualmente Administrator sin credenciales en código

1. Registra normalmente el usuario desde `/Identity/Account/Register`.
2. Detén la aplicación.
3. Inicializa User Secrets y configura solamente el correo del usuario existente:

```bash
dotnet user-secrets init --project Voyago/Voyago.csproj
dotnet user-secrets set "BootstrapAdmin:Email" "usuario@ejemplo.com" --project Voyago/Voyago.csproj
```

4. Inicia la aplicación una vez. El inicializador idempotente asignará `Administrator` si la cuenta existe.
5. Elimina la configuración temporal y vuelve a iniciar sesión para renovar la cookie:

```bash
dotnet user-secrets remove "BootstrapAdmin:Email" --project Voyago/Voyago.csproj
```

No se crea ninguna contraseña administrativa ni se almacena una contraseña en configuración.

## Verificaciones manuales

- Registrar una cuenta e iniciar/cerrar sesión.
- Consultar `/api/v1/health` y verificar `status: ok`.
- Abrir `/Admin` sin sesión y comprobar redirección al login.
- Abrir `/Admin` como Traveler y comprobar acceso denegado.
- Asignar Administrator mediante User Secrets y comprobar acceso.
- Probar navbar en anchos móvil y escritorio.
