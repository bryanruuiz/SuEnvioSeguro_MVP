# SuEnvioSeguro MVP - Requisitos e Instalación

## 📋 Requisitos del Sistema

### Obligatorios
- **SQL Server** (2019, 2022 o SQL Server Express)
  - Host: `localhost,1433`
  - Usuario: sa
  - Contraseña: configurada en tu equipo
- **.NET 10 SDK** (versión 10.0.0 o superior)
  - Descargar: https://dotnet.microsoft.com/download/dotnet/10.0
- **Node.js** (versión 18.0 o superior)
  - Descargar: https://nodejs.org/
- **npm** (incluido con Node.js, versión 9.0+)

### Verificar Instalación
```powershell
# Verificar .NET
dotnet --version

# Verificar Node.js
node --version

# Verificar npm
npm --version

# Verificar SQL Server (desde SQL Server Management Studio)
# Conectar a: localhost,1433
```

---

## 🛠️ Backend (.NET 10)

### Ubicación
```
SuEnvioSeguro_MVP/
└── SuEnvioSeguro.API/
```

### Dependencias NuGet
```
BCrypt.Net-Next v4.1.0                              (Encriptación de contraseñas)
Microsoft.AspNetCore.Authentication.JwtBearer v10.0.7  (Autenticación JWT)
Microsoft.AspNetCore.OpenApi v10.0.2                (OpenAPI/Swagger)
Microsoft.EntityFrameworkCore.Design v10.0.7        (Herramientas EF Core)
Microsoft.EntityFrameworkCore.SqlServer v10.0.7     (Proveedor SQL Server)
Swashbuckle.AspNetCore v10.1.7                      (Documentación Swagger)
```

### Instalación de Dependencias
```powershell
cd SuEnvioSeguro.API
dotnet restore
```

### Configuración Base de Datos

#### 1. Crear la Base de Datos y Aplicar Migraciones
```powershell
# Desde la carpeta SuEnvioSeguro.API
dotnet ef database update
```

**Migraciones Aplicadas:**
- `InitialCreate` - Tablas base (Persona, Usuario, Cliente, Factura, Envio)
- `AddMunicipios` - Tabla de municipios con tarifas
- `IdentitiesAndBusinessRules` - Índices y claves únicas
- `JwtCrudAndNombreUsuario` - Campo NombreUsuario y ajustes JWT

#### 2. Verificar Base de Datos Creada
```sql
-- En SQL Server Management Studio
SELECT name FROM sys.databases WHERE name = 'SuEnvioSeguroDB'
```

### Configuración de Conexión

Archivo: `appsettings.Development.json` y `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=SuEnvioSeguroDB;User Id=sa;Password=TuContraseña;TrustServerCertificate=true;"
  },
  "Jwt": {
    "SecretKey": "tu-clave-secreta-minimo-32-caracteres",
    "ExpirationMinutes": 240
  }
}
```

**Variables a Configurar:**
- `TuContraseña`: Contraseña de SQL Server
- `tu-clave-secreta-minimo-32-caracteres`: Clave JWT (cambiar en producción)

### Ejecución del Backend
```powershell
cd SuEnvioSeguro.API
dotnet run
```

**API disponible en:**
- `http://localhost:5096`
- Swagger: `http://localhost:5096/swagger/ui`

### Usuario Inicial para Pruebas
```
Documento: 123456789
Usuario: admin.123456789
Contraseña: Password123!
Rol: ADMIN
```

---

## 🎨 Frontend (React + Vite)

### Ubicación
```
SuEnvioSeguro_MVP/
└── suenvioseguro-web/
```

### Dependencias npm

#### Dependencias de Producción
```
axios ^1.16.0                  (Cliente HTTP)
react ^19.2.5                  (Framework UI)
react-dom ^19.2.5              (Renderizado DOM)
react-router-dom ^7.14.2       (Enrutamiento)
```

#### Dependencias de Desarrollo
```
@eslint/js ^10.0.1
@types/react ^19.2.14
@types/react-dom ^19.2.3
@vitejs/plugin-react ^6.0.1
autoprefixer ^10.5.0
eslint ^10.2.1
eslint-plugin-react-hooks ^7.1.1
eslint-plugin-react-refresh ^0.5.2
globals ^17.5.0
postcss ^8.5.13
tailwindcss ^3.4.19
vite ^8.0.10
```

### Instalación de Dependencias
```powershell
cd suenvioseguro-web
npm install
```

### Configuración de Variables de Entorno

Archivo: `.env` (crear en la carpeta raíz de `suenvioseguro-web`)

```env
VITE_API_URL=http://localhost:5096/api
```

**Notas:**
- El backend debe estar corriendo en `http://localhost:5096`
- No incluir trailing slash en la URL

### Ejecución del Frontend

#### Modo Desarrollo
```powershell
cd suenvioseguro-web
npm run dev
```

**Frontend disponible en:**
- `http://localhost:5174`

#### Compilar para Producción
```powershell
cd suenvioseguro-web
npm run build
```

**Salida:**
- Archivos en carpeta `dist/`
- Listo para desplegar en servidor web estático

#### Verificar Linting
```powershell
cd suenvioseguro-web
npm run lint
```

---

## 🚀 Instalación Completa (Paso a Paso)

### 1. Clonar/Descargar Repositorio
```powershell
cd C:\Users\TuUsuario\
# Si es un repositorio git
git clone <URL_REPOSITORIO> SuEnvioSeguro_MVP
cd SuEnvioSeguro_MVP
```

### 2. Configurar Backend
```powershell
cd SuEnvioSeguro.API

# Restaurar dependencias
dotnet restore

# Crear base de datos y aplicar migraciones
dotnet ef database update

# Verificar compilación
dotnet build
```

### 3. Configurar Frontend
```powershell
cd ..\suenvioseguro-web

# Instalar dependencias
npm install

# Crear archivo .env
# Agregar: VITE_API_URL=http://localhost:5096/api

# Verificar linting
npm run lint

# Compilar para verificar
npm run build
```

### 4. Ejecutar Aplicación

**Terminal 1 - Backend:**
```powershell
cd C:\Users\TuUsuario\SuEnvioSeguro_MVP\SuEnvioSeguro.API
dotnet run
# Esperar mensaje: "Now listening on: http://localhost:5096"
```

**Terminal 2 - Frontend:**
```powershell
cd C:\Users\TuUsuario\SuEnvioSeguro_MVP\suenvioseguro-web
npm run dev
# Esperar mensaje: "Local: http://localhost:5174"
```

### 5. Acceder a la Aplicación
- **Interfaz Web:** `http://localhost:5174`
- **API Documentation:** `http://localhost:5096/swagger/ui`

### 6. Login Inicial
```
Usuario: admin.123456789
Contraseña: Password123!
```

---

## 🔧 Troubleshooting

### Error: "Cannot connect to SQL Server"
```
Solución:
1. Verificar SQL Server ejecutándose: services.msc
2. Verificar credenciales en appsettings.Development.json
3. Verificar puerto 1433 abierto
4. Ejecutar: dotnet ef database update
```

### Error: "Port 5096 already in use"
```powershell
# Encontrar proceso en puerto 5096
netstat -ano | findstr :5096

# Detener proceso (PID es el número resultante)
taskkill /PID <PID> /F
```

### Error: "Port 5174 already in use"
```powershell
npm run dev -- --port 5175
```

### Error: "Module not found" en npm
```powershell
cd suenvioseguro-web
rm -r node_modules
npm cache clean --force
npm install
```

### Error: "EF Core migrations not applied"
```powershell
cd SuEnvioSeguro.API
dotnet ef database update --verbose
```

---

## 📦 Resumen de Versiones

| Componente | Versión | Propósito |
|-----------|---------|----------|
| .NET SDK | 10.0 | Runtime del backend |
| Node.js | 18+ | Runtime del frontend |
| SQL Server | 2019+ | Base de datos |
| React | 19.2.5 | Framework UI |
| Vite | 8.0.10 | Bundler frontend |
| Entity Framework Core | 10.0.7 | ORM |
| BCrypt | 4.1.0 | Encriptación |
| JWT Bearer | 10.0.7 | Autenticación |

---

## 🔐 Seguridad - Cambios Necesarios Antes de Producción

1. **Cambiar JWT SecretKey** en `appsettings.json`
   - Usar clave de mínimo 32 caracteres
   - Usar generador: https://www.uuidgenerator.net/

2. **Cambiar contraseña del usuario inicial**
   - Acceder como admin
   - Editar perfil del usuario admin.123456789

3. **Cambiar credenciales SQL Server**
   - No usar contraseña predeterminada
   - Usar usuario con permisos limitados

4. **Habilitar HTTPS**
   - Generar certificado SSL
   - Configurar en `appsettings.json`

5. **CORS Configuration**
   - Cambiar `AllowAnyOrigin()` por whitelist de dominios permitidos

---

## 📝 Notas Importantes

- **Puertos**: Backend (5096), Frontend (5174)
- **Base de Datos**: Automáticamente creada en primera ejecución
- **Sincronización**: Frontend y Backend deben correr simultáneamente
- **Token JWT**: Válido por 240 minutos (4 horas)
- **Soft Delete**: El campo `Activo` permite desactivar sin eliminar datos

---

## 📞 Soporte

Para problemas o preguntas:
1. Verificar logs en la terminal
2. Revisar archivos `appsettings.Development.json`
3. Confirmar SQL Server conectado y corriendo
4. Comprobar versiones de .NET y Node.js
