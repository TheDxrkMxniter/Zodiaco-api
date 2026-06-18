# Vaede.Api

Backend base para la plataforma de compra y venta de camiones usados VAEDE Trucks Mexico.

## Requisitos

- .NET SDK 8
- PostgreSQL 14+ o compatible
- Acceso a una base de datos PostgreSQL local o remota

## Como instalar paquetes

```bash
dotnet restore
dotnet tool restore
```

El proyecto ya incluye:

- `Microsoft.EntityFrameworkCore`
- `Npgsql.EntityFrameworkCore.PostgreSQL`
- `Microsoft.EntityFrameworkCore.Design`
- `Swashbuckle.AspNetCore`

## Como configurar la conexion PostgreSQL

Puedes usar cualquiera de estas opciones:

1. `ConnectionStrings:DefaultConnection` en `appsettings.json` o `appsettings.Development.json`
2. Variable de entorno `ConnectionStrings__DefaultConnection`
3. Variable de entorno `DATABASE_URL`

La aplicacion carga `.env` automaticamente en desarrollo si existe, pero tambien funciona sin ese archivo usando `appsettings` o variables reales del entorno.

`.env` no debe subirse al repositorio.

### Opcion A: local con `appsettings.Development.json`

Ejemplo local:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=vaede_trucks_dev;Username=postgres;Password=postgres"
}
```

### Opcion B: local con `.env`

```env
DATABASE_URL=postgresql://postgres:password@localhost:5432/vaede_db?sslmode=disable
ASPNETCORE_ENVIRONMENT=Development
CORS_ALLOWED_ORIGINS=http://localhost:3000,http://localhost:5173
```

### Opcion C: Railway

En Railway normalmente basta con `DATABASE_URL` provisto por la plataforma.

Para entornos hospedados, el parser convierte `DATABASE_URL` a connection string de Npgsql y usa SSL automaticamente.

## Como crear migracion

```bash
dotnet ef migrations add MyNextMigration
```

## Como aplicar la base de datos

```bash
dotnet ef database update
```

## Como correr la API

```bash
dotnet run
```

## Como abrir Swagger

En desarrollo, Swagger UI se sirve en la ruta principal:

- `http://localhost:5187/`
- `https://localhost:7232/`

Health check:

- `GET /health`
