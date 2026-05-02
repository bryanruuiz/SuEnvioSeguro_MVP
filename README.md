# 📦 SuEnvioSeguro MVP

**Plataforma de Logística y Facturación** - Sistema completo de gestión de envíos con autenticación JWT, facturas interactivas e impresión PDF.

---

## 🚀 Inicio Rápido

### 5 Minutos
```powershell
# Terminal 1 - Backend
cd SuEnvioSeguro.API
dotnet restore && dotnet ef database update && dotnet run

# Terminal 2 - Frontend
cd suenvioseguro-web
npm install && npm run dev

# Browser
http://localhost:5174
# Usuario: admin.123456789 | Contraseña: Password123!
```

**Ver:** [`QUICKSTART.md`](QUICKSTART.md) para instrucciones detalladas en 5 pasos.

---

## 📚 Documentación

| Documento | Propósito |
|-----------|-----------|
| **[REQUIREMENTS.md](REQUIREMENTS.md)** | Instalación completa, requisitos, configuración, troubleshooting |
| **[QUICKSTART.md](QUICKSTART.md)** | Inicio rápido en 5 minutos |
| **[CHECKLIST.md](CHECKLIST.md)** | Verificación paso a paso antes de ejecutar |

---

## 🏗️ Arquitectura

### Backend - ASP.NET Core 10.0
```
SuEnvioSeguro.API/
├── Controllers/          → REST endpoints
├── Services/             → Lógica de negocio (Strategy, Facade, State patterns)
├── Models/               → Entidades (TPH inheritance, Persona → Usuario/Cliente)
├── Data/                 → Entity Framework Core DbContext
└── Properties/           → Configuración JWT y CORS
```

**Stack:**
- 🔒 **Autenticación:** JWT Bearer (240 min expiration)
- 🔐 **Encriptación:** BCrypt
- 💾 **BD:** SQL Server (4 migraciones)
- 📚 **ORM:** Entity Framework Core
- 📖 **Docs:** Swagger OpenAPI

**Patrones GoF Implementados:**
- ✅ Strategy: `CalculadoraTarifas` + `EstrategiaTarifaMunicipios`
- ✅ Factory: `EnvioFactory`
- ✅ Facade: `FacturacionFacade`
- ✅ State: `IEstadoEnvio` (Enviado, Entregado, Cancelado)
- ✅ Singleton: `GeneradorCodigoFactura`

### Frontend - React 19 + Vite
```
suenvioseguro-web/
├── App.jsx               → Router y estado global
├── App.css               → Grid layouts, print styles
├── index.css             → CSS variables, design system
└── assets/               → Recursos estáticos
```

**Stack:**
- ⚛️ **Framework:** React 19
- ⚡ **Bundler:** Vite 8
- 🎨 **Estilos:** CSS Grid, Flexbox, @media print
- 🔌 **HTTP:** Axios (interceptor JWT automático)
- 🔄 **Estado:** React Hooks (useState, useEffect, useMemo, useCallback)

---

## 📋 Funcionalidades

### 🧑‍💼 Autenticación & Autorización
- Login con usuario y contraseña
- JWT Bearer tokens (240 minutos)
- Roles: ADMIN, EMPLEADO
- Control de acceso por módulo

### 📦 Módulo de Facturación
- Registro de envíos con datos de cliente
- Selección de municipio destino (10 opciones)
- Cálculo automático de tarifas por municipio
- Generación de código de factura único
- Visualización y impresión de facturas
- **Exportar a PDF** (navegador nativo)

### 🚚 Módulo de Seguimiento
- Búsqueda pública de envíos (sin login)
- Filtros: código, estado, municipio
- Actualización de estados (Registrado → Enviado → Entregado/Cancelado)
- Tabla con envíos ordenados

### 👥 Módulo de Empleados (Admin)
- CRUD de empleados (Crear, Leer, Actualizar, Eliminar)
- Asignación de roles (ADMIN, EMPLEADO)
- Desactivación/Activación (soft delete)
- Edición de contraseña

### 💾 Base de Datos
- **Tablas:** Personas, Usuarios, Clientes, Facturas, Envios, Municipios
- **Migraciones:** 4 aplicadas automáticamente
- **Índices:** DocumentoIdentidad, NombreUsuario, CodigoFactura, CodigoEnvio
- **Claves Únicas:** Garantizan integridad datos

---

## 🔧 Requisitos del Sistema

### Obligatorios
| Software | Versión | Instalador |
|----------|---------|-----------|
| SQL Server | 2019+ o Express | https://www.microsoft.com/en-us/sql-server |
| .NET SDK | 10.0+ | https://dotnet.microsoft.com/download/dotnet/10.0 |
| Node.js | 18.0+ | https://nodejs.org/ |

### Verificación
```powershell
dotnet --version  # Debe ser 10.0.0+
node --version    # Debe ser v18.0.0+
npm --version     # Debe ser 9.0.0+
```

---

## 📦 Dependencias

### Backend (NuGet)
```
BCrypt.Net-Next 4.1.0
Microsoft.AspNetCore.Authentication.JwtBearer 10.0.7
Microsoft.EntityFrameworkCore.SqlServer 10.0.7
Swashbuckle.AspNetCore 10.1.7
```

### Frontend (npm)
```
react 19.2.5
react-dom 19.2.5
axios 1.16.0
vite 8.0.10
tailwindcss 3.4.19
```

**Ver** `REQUIREMENTS.md` para lista completa.

---

## 🎨 Diseño

**Paleta de Colores:**
- 🟢 **Primario:** `#10b981` (Verde - confianza, logística)
- 🔵 **Secundario:** `#0ea5e9` (Azul - información)
- 🎨 **Acentos:** Gradientes sutiles, bordes claros, sombras profesionales

**Componentes:**
- Tablas interactivas con hover
- Formularios validados
- Alertas de error/éxito
- Modales y dialogs
- Cards y paneles

**Responsive:**
- Mobile (320px+)
- Tablet (768px+)
- Desktop (1024px+)
- **Impresión:** Optimizado para PDF A4

---

## 📊 Estructura de Base de Datos

```
Personas (TPH Base)
├── Usuarios (UsuarioId FK)
└── Clientes (ClienteId FK)

Facturas
├── ClienteId (FK → Clientes)
├── UsuarioId (FK → Usuarios)
└── CodigoFactura (UNIQUE)

Envios
├── FacturaId (FK → Facturas)
├── MunicipioDestino (FK → Municipios)
└── CodigoEnvio (UNIQUE)

Municipios (Seeded con 10 ciudades)
└── Tarifa (COP)
```

---

## 🔑 Usuario de Prueba

```
📧 Usuario: admin.123456789
🔑 Contraseña: Password123!
👤 Rol: ADMIN
📄 Documento: 123456789
```

---

## 🌐 URLs en Desarrollo

| Servicio | URL |
|----------|-----|
| Frontend | `http://localhost:5174` |
| Backend | `http://localhost:5096` |
| API Docs | `http://localhost:5096/swagger/ui` |
| BD | `localhost,1433` |

---

## 🚀 Despliegue

### Build Frontend
```powershell
cd suenvioseguro-web
npm run build
# Archivos listos en dist/ para servidor web estático
```

### Publicar Backend
```powershell
cd SuEnvioSeguro.API
dotnet publish -c Release
# Archivos en bin/Release/net10.0/publish/
```

### Cambios Necesarios para Producción
1. ✏️ **JWT SecretKey** → clave de 32+ caracteres
2. 🔐 **SQL Server** → usuario con permisos limitados
3. 🌐 **CORS** → whitelist de dominios permitidos
4. 🔒 **HTTPS** → certificado SSL
5. 🔑 **Usuario Admin** → cambiar contraseña

---

## 📞 Troubleshooting

### Error de Conexión Base de Datos
```powershell
# 1. Verificar SQL Server corriendo
Get-Service MSSQLSERVER | Start-Service

# 2. Aplicar migraciones
cd SuEnvioSeguro.API
dotnet ef database update
```

### Puerto en Uso
```powershell
# Encontrar proceso
netstat -ano | findstr :5096
# Matar proceso
taskkill /PID <PID> /F
```

### Dependencias Incompletas
```powershell
# Backend
cd SuEnvioSeguro.API
dotnet restore --no-cache

# Frontend
cd suenvioseguro-web
rm -r node_modules
npm cache clean --force
npm install
```

**Ver** `REQUIREMENTS.md` para troubleshooting completo.

---

## 📝 Notas Técnicas

### SOLID Principles
- ✅ **S**ingle Responsibility: Cada controller maneja un dominio
- ✅ **O**pen/Closed: Extensible con nuevas estrategias de tarifa
- ✅ **L**iskov: Sustitución de estrategias de estado
- ✅ **I**nterface Segregation: Interfaces mínimas
- ✅ **D**ependency Injection: DI nativa de ASP.NET Core

### Seguridad
- 🔐 Contraseñas: BCrypt hashing (never plain text)
- 🔒 Datos: Soft delete (Activo field)
- ✅ Validación: Inputs validados en backend
- 🛡️ CORS: Configurado para frontend
- 🔑 JWT: Claims con roles y permisos

### Performance
- 📊 Índices en BD: Búsquedas rápidas
- 🔄 Interceptor HTTP: Token automático
- 💾 LocalStorage: Sesión persistente
- 📦 Vite: HMR para desarrollo rápido

---

## 📖 Recursos Adicionales

- [Microsoft .NET Docs](https://learn.microsoft.com/dotnet/)
- [React 19 Docs](https://react.dev/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [JWT.io](https://jwt.io/) - Decodificar tokens

---

## 📄 Licencia

Proyecto educativo - MVP de demostración.

---

## 🎯 Próximos Pasos

1. **Instalar dependencias:** Ver [`REQUIREMENTS.md`](REQUIREMENTS.md)
2. **Verificar setup:** Usar [`CHECKLIST.md`](CHECKLIST.md)
3. **Ejecutar aplicación:** Seguir [`QUICKSTART.md`](QUICKSTART.md)
4. **Explorar API:** Acceder a `/swagger/ui` en backend

---

**¿Preguntas?** Revisar documentación en orden: QUICKSTART.md → REQUIREMENTS.md → CHECKLIST.md
