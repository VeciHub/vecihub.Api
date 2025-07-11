# vecihub.Api

# vecihub.Api


# VeciHub API

Backend del sistema VeciHub desarrollado con ASP.NET Core 9.0, usando arquitectura por capas y soporte para autenticación mediante JWT.

## 🧱 Requisitos

- .NET SDK 9.0
- Visual Studio 2022 o VS Code
- SQL Server o Azure SQL Database
- Entity Framework Core
- Git

## 🚀 Instalación y Ejecución

1. **Clona este repositorio:**

2. **Restaura paquetes NuGet:**

```bash
dotnet restore
```

3. **Configura la cadena de conexión:**

Abre el archivo `appsettings.json` y reemplaza la cadena de conexión por la tuya, o dejarla con los requisitos default:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=VeciHubDB;User Id=USUARIO;Password=CONTRASEÑA;TrustServerCertificate=True;"
}
```

## 📁 Estructura del Proyecto

```
veciHub.Api/
├── IAM/             # Módulo de Identidad y Autenticación
│   ├── Domain
│   ├── Application
│   ├── Infrastructure
│   └── Interfaces
├── Controllers/
├── Shared/
├── Migrations/
└── Program.cs
```

## 🔐 Autenticación

Se usa JWT para login y protección de endpoints. El token se obtiene mediante el endpoint `/api/auth/login`.

