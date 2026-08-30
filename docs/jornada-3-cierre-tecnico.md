# Cierre técnico de la Jornada 3

## Objetivo

Implementar y validar el catálogo público de Voyago, incluyendo navegación,
búsqueda, detalle de destinos, autenticación y gestión de favoritos.

## Funcionalidad implementada

Durante la Jornada 3 se completaron los siguientes componentes:

- Página de inicio con contenido persistido.
- Catálogo público de destinos.
- Búsqueda sin distinción entre mayúsculas y minúsculas.
- Filtros y paginación.
- Detalle de destinos.
- Paquetes relacionados.
- Hoteles relacionados.
- Vista cartográfica referencial.
- Recursos estáticos asociados al catálogo.
- Respuestas 404 para destinos inexistentes.
- Redirección a Identity conservando `returnUrl`.
- Gestión autenticada de favoritos.

## Estrategia de pruebas

La validación se dividió en dos proyectos.

### Voyago.Tests

El proyecto utiliza xUnit e incluye:

- Pruebas unitarias.
- Pruebas de servicios.
- Pruebas de PageModels.
- Pruebas de restricciones SQLite.
- Pruebas del seed demostrativo.
- Pruebas de integración HTTP con WebApplicationFactory.

Resultado final:

```text
Pruebas totales: 37
Correctas: 37
Incorrectas: 0
Omitidas: 0