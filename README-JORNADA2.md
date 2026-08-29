# Voyago, Jornada 2

Este paquete es un overlay incremental sobre la versión operativa de Jornada 1. Copie su contenido sobre la raíz del repositorio conservando los demás archivos.

## Migración

La migración no se redacta manualmente. Genérela desde el modelo validado:

```bash
dotnet restore Voyago.sln
dotnet build Voyago.sln --no-restore
dotnet ef migrations add AddCoreDomainModel --project Voyago/Voyago.csproj --startup-project Voyago/Voyago.csproj --output-dir Data/Migrations
dotnet ef database update --project Voyago/Voyago.csproj --startup-project Voyago/Voyago.csproj
dotnet run --project Voyago/Voyago.csproj
```

El seed se ejecuta únicamente en Development y es idempotente por identificadores reservados. No incluye contraseñas. La cuenta `demo-reviewer@voyago.local` se crea sin contraseña y solo mantiene integridad referencial de las reseñas.

## Verificación SQLite

```sql
PRAGMA foreign_keys;
PRAGMA foreign_key_check;
SELECT COUNT(*) FROM Destinations;
SELECT COUNT(*) FROM TourPackages;
SELECT COUNT(*) FROM Hotels;
SELECT COUNT(*) FROM HotelRoomTypes;
SELECT COUNT(*) FROM FlightOffers;
SELECT COUNT(*) FROM Reviews WHERE ModerationStatus = 'Approved';
```

Cantidades esperadas: 8, 10, 8, 16, 8 y 10. Ejecute la aplicación dos veces y confirme que las cantidades no aumentan.
