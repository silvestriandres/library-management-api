# Challenge Provincia NET (Library Management API)

API REST desarrollada en .NET 8 para la gestión de libros de biblioteca.

El proyecto fue realizado como parte de un challenge técnico orientado a backend development utilizando ASP.NET Core, Entity Framework Core y SQL Server.

La idea principal fue no solamente cumplir con el CRUD solicitado, sino también intentar construir una solución prolija, mantenible y relativamente cercana a un entorno real de desarrollo, evitando tanto el código “tirado” como la sobreingeniería innecesaria para un challenge de este tamaño.

---

# Objetivo del challenge

El challenge consistía en desarrollar una API RESTful que permita gestionar operaciones CRUD sobre una base de datos relacional.

Además de los requisitos mínimos, se intentó sumar varios puntos que suelen valorarse en proyectos backend modernos:

- Arquitectura en capas
- Separación de responsabilidades
- Repository Pattern
- Service Layer
- Validaciones desacopladas
- Middleware global de excepciones
- Testing unitario
- Dockerización
- Swagger/OpenAPI
- Paginación
- Cancellation Tokens
- Manejo de errores consistente
- Migraciones automáticas en Docker

---

# Tecnologías utilizadas

## Backend

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- FluentValidation
- Swagger / OpenAPI
- xUnit
- Moq

## Infraestructura

- Docker
- Docker Compose

---


# Cómo levantar el proyecto localmente

## Clonar repositorio

```bash
git clone https://github.com/silvestriandres/library-management-api
```

## Posicionarse en la solución
```
cd challenge
```
## Configurar connection string

Editar:
```
appsettings.json
```
Ejemplo:
```
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=LibraryManagementDb;User Id=sa;Password=YourPassword123!;TrustServerCertificate=true"
}
```
## Restaurar paquetes
```
dotnet restore
```
## Ejecutar migraciones
```
dotnet ef database update
```
## Ejecutar aplicación
```
dotnet run
```
## Swagger
```
http://localhost:5000/swagger
```

# Cómo levantar con Docker

## Tener Docker Desktop levantado

Verificar:

```bash
docker ps
```

---

## Ejecutar Docker Compose

Desde la raiz de la solución:

```bash
docker compose up --build
```

---

## Acceder a Swagger

```plaintext
http://localhost:5000/swagger
```

---

# Decisiones arquitectónicas

---

## ¿Por qué arquitectura en capas?

La idea fue mantener el proyecto organizado y desacoplado.

Aunque para un challenge podría haberse hecho todo dentro del Controller, eso rápidamente escala mal cuando la lógica crece.

Por eso se separó en distintas responsabilidades:

| Capa | Responsabilidad |
|---|---|
| Controllers | Recibir requests HTTP |
| Services | Contener lógica de negocio |
| Repositories | Acceso a datos |
| Data | Configuración de EF Core |
| DTOs | Contratos de entrada/salida |
| Validators | Validaciones |
| Middleware | Manejo global de errores |

Esto hace que:
- el código sea más mantenible
- sea más fácil testear
- se puedan reemplazar implementaciones
- la lógica no quede acoplada al framework

---

# Repository Pattern

Se implementó Repository Pattern para abstraer el acceso a datos.

La idea no fue “usar patrones porque sí”, sino desacoplar Entity Framework de la lógica de negocio.

De esta forma:
- el Service no conoce EF Core directamente
- se facilita el testing
- se evita lógica de consultas dentro del Controller

---

# Service Layer

Se agregó una capa de Services para centralizar la lógica de negocio.

Esto evita:
- Controllers gigantes
- lógica repetida
- mezcla de HTTP + reglas de negocio

El Controller solamente:
- recibe requests
- delega
- devuelve respuestas

---

# Validaciones

Se utilizó FluentValidation para desacoplar las validaciones del modelo.

En lugar de llenar las entidades con atributos, las reglas quedaron centralizadas en Validators independientes.

Esto mejora:
- legibilidad
- mantenibilidad
- testing
- escalabilidad

---

# Middleware global de excepciones

Se implementó un middleware global para manejar errores de manera consistente.

La idea fue:
- evitar try/catch en todos los Controllers
- centralizar respuestas de error
- devolver respuestas uniformes

Formato utilizado:

```json
{
  "success": false,
  "message": "An unexpected error occurred.",
  "errors": []
}
```

# Entity Framework Core

Se utilizó EF Core como ORM principal.

Se implementaron:
- DbContext
- Configuración mediante Fluent API
- Migrations
- SQL Server Provider

La idea fue aprovechar:
- productividad
- tipado fuerte
- migraciones versionadas
- integración natural con .NET

También se buscó mantener la configuración desacoplada utilizando clases de configuración separadas en lugar de sobrecargar las entidades.

---

# Dockerización

El proyecto fue dockerizado utilizando:
- Dockerfile multi-stage
- Docker Compose
- SQL Server containerizado

La idea fue permitir levantar todo el entorno con un único comando.

Además:
- la API aplica migrations automáticamente al iniciar
- se configuró resiliencia ante fallos transitorios de SQL Server
- se agregó retry policy con `EnableRetryOnFailure`

Esto es particularmente útil porque SQL Server suele tardar más tiempo en iniciar que la API cuando se levanta con Docker Compose.

---

# Retry Policy SQL Server

Se agregó:

```csharp
EnableRetryOnFailure()
```

para manejar fallos transitorios de conexión.

Esto evita errores comunes durante el startup containerizado.

---

# Testing

Se agregaron tests unitarios utilizando:
- xUnit
- Moq

La idea fue validar:
- comportamiento de Services
- lógica desacoplada
- respuestas esperadas

No se apuntó a cobertura extrema, sino a demostrar conocimiento de testing backend.

---

# Estructura del proyecto

```plaintext
LibraryManagementApi.API
│
├── Controllers
├── Data
│   ├── Configurations
│   └── Migrations
├── DTOs
├── Middleware
├── Models
├── Repositories
│   └── Interfaces
├── Services
│   └── Interfaces
├── Validators
└── Program.cs
```

# Endpoints principales

| Método | Endpoint | Descripción |
|---|---|---|
| GET | /api/books | Obtener libros |
| GET | /api/books/{id} | Obtener libro por ID |
| POST | /api/books | Crear libro |
| PUT | /api/books/{id} | Actualizar libro |
| DELETE | /api/books/{id} | Eliminar libro |

---

# Paginación

Se implementó paginación básica mediante:

```plaintext
?page=1&pageSize=10
```

Ejemplo:

```plaintext
/api/books?page=1&pageSize=10
```

---

# Cancellation Tokens

Se agregaron `CancellationToken` en operaciones async para permitir cancelación de requests largos y alinearse con buenas prácticas modernas de ASP.NET Core.

---

# Manejo de errores

La API devuelve respuestas consistentes ante errores.

Ejemplo:

```json
{
  "success": false,
  "message": "An unexpected error occurred.",
  "errors": [
    "Detailed error"
  ]
}
```

---

# Posibles mejoras futuras

Si esto evolucionara a un proyecto más grande, probablemente se pueda agregar:

- JWT Authentication
- Authorization por roles
- Refresh Tokens
- Logging estructurado
- Serilog
- Health Checks
- CI/CD
- Cache distribuida
- Soft Delete
- Auditoría
- CQRS
- MediatR
- Rate Limiting
- Integración con Azure/AWS

---

# Consideraciones finales

La intención del proyecto no fue solamente hacer un CRUD sino intentar demostrar:
- organización
- buenas prácticas
- separación de responsabilidades
- mantenibilidad
- capacidad de escalar la solución

Intentando mantener un equilibrio razonable entre:
- simplicidad
- claridad
- buenas prácticas
- y evitar complejidad innecesaria para el alcance del challenge.
 paginación básica mediante:
