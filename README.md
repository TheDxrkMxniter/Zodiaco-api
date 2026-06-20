# Zodiaco.Api

Backend .NET 8 de Universal Zodiaco listo para compilar desde la raiz de `Zodiaco backend`.

## Requisitos

- .NET SDK 8
- PostgreSQL 14+ o compatible
- Acceso a una base de datos PostgreSQL local o remota

## Estructura

- `Zodiaco.Api.sln`
- `Zodiaco.Api/`
- `.env.example`

## Nota tecnica de ramas

El hardening inicial de backend fue absorbido por la rama `feature/database-setup`.

No hace falta abrir una rama adicional `feature/backend-hardening` mientras estos cambios sigan viviendo en esa rama.

## Comandos desde la raiz

Ejecuta estos comandos parado en `Zodiaco backend`:

```bash
dotnet restore
dotnet build
dotnet run --project Zodiaco.Api
```

Para confirmar la rama actual:

```bash
git branch --show-current
```

## Entity Framework Core desde la raiz

Si `dotnet-ef` no esta instalado globalmente:

```bash
dotnet tool restore
```

Comandos EF:

```bash
dotnet ef dbcontext info --project .\Zodiaco.Api\Zodiaco.Api.csproj --startup-project .\Zodiaco.Api\Zodiaco.Api.csproj
dotnet ef migrations list --project .\Zodiaco.Api\Zodiaco.Api.csproj --startup-project .\Zodiaco.Api\Zodiaco.Api.csproj
dotnet ef migrations add AddYourMigrationName --project Zodiaco.Api --startup-project Zodiaco.Api
dotnet ef database update --project Zodiaco.Api --startup-project Zodiaco.Api
```

Nota: el repositorio ya incluye migraciones iniciales, por lo que no conviene reutilizar el nombre `InitialCreate`.

Antes de aplicar migraciones, confirma que `dotnet ef dbcontext info` apunte a `localhost` y a la base `zodiaco_api_dev`.

Si EF apunta a Railway o a otro host remoto, detente, revisa `.env` local y no ejecutes `database update`.

## Configuracion PostgreSQL

Puedes usar cualquiera de estas opciones:

1. Variable de entorno `ConnectionStrings__DefaultConnection`
2. Variable de entorno `DATABASE_URL`
3. `ConnectionStrings:DefaultConnection` en `Zodiaco.Api/appsettings.Development.json` o `Zodiaco.Api/appsettings.json`

La aplicacion busca `.env` en la raiz de `Zodiaco backend` y tambien soporta un `.env` dentro de `Zodiaco.Api/`.

Si el archivo existe, se carga sin sobrescribir variables reales del sistema. En produccion la app sigue funcionando aunque `.env` no exista.

`.env` no debe subirse al repositorio. `.env.example` si debe permanecer versionado.

### Opcion A: local con `appsettings.Development.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=zodiaco_api_dev;Username=postgres;Password=1234"
}
```

### Opcion B: local con `.env`

```env
ConnectionStrings__DefaultConnection=Host=localhost;Port=5432;Database=zodiaco_api_dev;Username=postgres;Password=1234
ASPNETCORE_ENVIRONMENT=Development
CORS_ALLOWED_ORIGINS=http://localhost:3000,http://localhost:5173
```

Si prefieres `DATABASE_URL`, usa:

```env
DATABASE_URL=postgresql://postgres:1234@localhost:5432/zodiaco_api_dev?sslmode=disable
```

### Opcion C: Railway

En Railway normalmente basta con `DATABASE_URL` provisto por la plataforma.

Para entornos hospedados, el parser convierte `DATABASE_URL` a connection string de Npgsql y usa `Ssl Mode=Require` con `Trust Server Certificate=true` por defecto.

Para hosts locales como `localhost`, `127.0.0.1` o `::1`, el parser usa `Ssl Mode=Disable` por defecto.

## Seed local del catalogo MVP

El seed solo se puede ejecutar en `Development` y contra PostgreSQL local.

```bash
dotnet run --project Zodiaco.Api -- --seed
```

Si el argumento `--seed` no esta presente, la API no inserta datos automaticamente.

Railway queda reservado para fase de test/final. No se deben aplicar migraciones ni seeds ahi durante desarrollo.

## Health y Swagger

- `GET /health` responde `{"status":"ok","service":"Zodiaco.Api"}`
- Swagger UI se sirve en desarrollo en la ruta principal
- URLs locales por defecto:
  - `http://localhost:5187/`
  - `https://localhost:7232/`
