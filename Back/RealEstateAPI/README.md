# Real Estate API

API REST para la gestión de propiedades inmobiliarias desarrollada con .NET 9, MongoDB.

## 🏗️ Arquitectura

El proyecto está organizado en las siguientes capas:

- **RealEstate.Domain**: Entidades del dominio e interfaces
- **RealEstate.Application**: Casos de uso, DTOs y lógica de negocio
- **RealEstate.Infrastructure**: Implementaciones de acceso a datos
- **RealEstate.API**: Controladores y configuración de la Web API
- **RealEstate.Tests**: Pruebas unitarias con NUnit

## 🛠️ Tecnologías Utilizadas

- **.NET 9.0**
- **C#**
- **MongoDB** - Base de datos NoSQL
- **Swagger/OpenAPI** - Documentación de API
- **NUnit** - Framework de pruebas
- **FluentAssertions** - Assertions fluidas para pruebas

## 📋 Prerrequisitos

- **.NET 9 SDK** (versión 9.0.303 o superior)
- **MongoDB** (versión 4.4 o superior)
- **Visual Studio 2022** o **VS Code**

## 🚀 Instalación y Configuración

### 1. Clonar el repositorio
```bash
git clone https://github.com/Jose0808/PruebaMillion
cd Back/RealEstateAPI
```

### 2. Restaurar dependencias
```bash
dotnet restore
```

### 3. Configurar MongoDB

#### Opción A: MongoDB Local
1. Instalar MongoDB Community Server
2. Iniciar el servicio de MongoDB
3. La cadena de conexión por defecto es: `mongodb://localhost:27017`

#### Opción B: MongoDB Atlas (Cloud)
1. Crear una cuenta en [MongoDB Atlas](https://www.mongodb.com/atlas)
2. Crear un cluster
3. Obtener la cadena de conexión
4. Actualizar `appsettings.json` con tu cadena de conexión

### 4. Configurar la aplicación
Editar el archivo `RealEstate.API/appsettings.json`:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "RealEstateDB",
    "PropertiesCollectionName": "Properties"
  }
}
```

### 5. Importar datos de prueba
Importar el JSON (RealEstateDB.Properties.json) ubicado en la raiz del proyecto
ó
Ejecutar el siguiente comando en MongoDB para crear la base de datos y colección:

```javascript
use RealEstateDB

db.Properties.insertOne({
  "_id": {
    "$oid": "689bb0e63f15768dbc29b336"
  },
  "IdOwner": "OWN001",
  "Name": "Casa Moderna de Lujo La Calera",
  "AddressProperty": "Calle 15 #23-45, Conjunto Reservado, La Calera, Cundinamarca",
  "PriceProperty": 850000000,
  "Image": "https://images.unsplash.com/photo-1564013799919-ab600027ffc6?w=800&h=600&fit=crop",
  "PropertyType": "Casa",
  "Status": "Available",
  "Bedrooms": 4,
  "Bathrooms": 3,
  "Area": 280.5,
  "YearBuilt": 2020,
  "Description": "Hermosa casa moderna con acabados de lujo, vista panorámica de la sabana de Bogotá.",
  "Features": [
    "Piscina",
    "Jardín privado",
    "Chimenea",
    "Estudio",
    "Cuarto de servicio"
  ],
  "ParkingSpots": 2,
  "CreatedDate": {
    "$date": "2024-01-15T00:00:00.000Z"
  },
  "UpdatedDate": {
    "$date": "2024-01-15T00:00:00.000Z"
  }
})
```


### 6. Ejecutar la aplicación

#### Desde Visual Studio
1. Establecer `RealEstate.API` como proyecto de inicio
2. Presionar F5 o Ctrl+F5

#### Desde línea de comandos
```bash
cd RealEstate.API
dotnet run
```

La API estará disponible en:
- **HTTP**: `http://localhost:5285`
- **HTTPS**: `https://localhost:5285`
- **Swagger UI**: `https://localhost:5285/index.html` 

## 📖 Documentación de la API

### Endpoints principales

#### GET /api/properties
Obtiene una lista paginada de propiedades con filtros opcionales.

**Parámetros de consulta:**
- `name` (string, opcional): Filtro por nombre de la propiedad
- `address` (string, opcional): Filtro por dirección de la propiedad
- `minPrice` (decimal, opcional): Precio mínimo
- `maxPrice` (decimal, opcional): Precio máximo
- `pageNumber` (int, opcional): Número de página (por defecto 1)
- `pageSize` (int, opcional): Tamaño de página (por defecto 10, máximo 100)

**Ejemplo de solicitud:**
```http
GET /api/properties?name=casa&minPrice=500000000&maxPrice=1000000000&pageNumber=1&pageSize=10
```

**Ejemplo de respuesta:**
```json
{
  "success": true,
  "data": {
    "data": [
      {
        "id": "689bb0e63f15768dbc29b336",
        "idOwner": "OWN001",
        "name": "Casa Moderna de Lujo La Calera",
        "addressProperty": "Calle 15 #23-45, Conjunto Reservado, La Calera, Cundinamarca",
        "priceProperty": 850000000,
        "image": "https://images.unsplash.com/photo-1564013799919-ab600027ffc6?w=800&h=600&fit=crop"
      }
    ],
    "totalCount": 1,
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 1
  },
  "message": "Properties retrieved successfully"
}
```

#### GET /api/properties/{id}
Obtiene los detalles completos de una propiedad específica.

**Parámetros:**
- `id` (string): ID único de la propiedad

**Ejemplo de solicitud:**
```http
GET /api/properties/689bb0e63f15768dbc29b336
```

**Ejemplo de respuesta:**
```json
{
  "success": true,
  "data": {
    "id": "689bb0e63f15768dbc29b336",
    "idOwner": "OWN001",
    "name": "Casa Moderna de Lujo La Calera",
    "addressProperty": "Calle 15 #23-45, Conjunto Reservado, La Calera, Cundinamarca",
    "priceProperty": 850000000,
    "image": "https://images.unsplash.com/photo-1564013799919-ab600027ffc6?w=800&h=600&fit=crop",
    "propertyType": "Casa",
    "status": "Available",
    "bedrooms": 4,
    "bathrooms": 3,
    "area": 280.5,
    "yearBuilt": 2020,
    "description": "Hermosa casa moderna con acabados de lujo, vista panorámica de la sabana de Bogotá.",
    "features": ["Piscina", "Jardín privado", "Chimenea", "Estudio", "Cuarto de servicio"],
    "parkingSpots": 2
  },
  "message": "Property retrieved successfully"
}
```

#### GET /api/properties/health
Endpoint de salud para verificar el estado de la API.

**Ejemplo de respuesta:**
```json
{
  "status": "healthy",
  "timestamp": "2024-01-15T10:30:00.000Z",
  "version": "1.0.0"
}
```

## 🧪 Pruebas

### Ejecutar todas las pruebas
```bash
dotnet test
```
## 🔧 Desarrollo

### Estructura del proyecto
```
RealEstateAPI/
├── RealEstate.Domain/           # Entidades y contratos
│   ├── Entities/
│   │   └── Property.cs
│   └── Repositories/
│       └── IPropertyRepository.cs
├── RealEstate.Application/      # Casos de uso y DTOs
│   ├── DTOs/
│   ├── Services/
│   ├── Validators/
│   └── DependencyInjection.cs
├── RealEstate.Infrastructure/   # Implementación de acceso a datos
│   ├── Configuration/
│   ├── Data/
│   ├── Repositories/
│   └── DependencyInjection.cs
├── RealEstate.API/             # Web API
│   ├── Controllers/
│   ├── Program.cs
│   └── appsettings.json
├── RealEstate.Tests/           # Pruebas unitarias
└── README.md
```

## 🚨 Manejo de Errores

La API maneja errores de forma consistente:

- **400 Bad Request**: Parámetros inválidos o errores de validación
- **404 Not Found**: Recurso no encontrado
- **500 Internal Server Error**: Errores internos del servidor

Todas las respuestas de error siguen la estructura:
```json
{
  "success": false,
  "data": null,
  "message": "Descripción del error",
  "errors": ["Lista de errores específicos"]
}
```

### Índices recomendados para MongoDB:
```javascript
// Crear índices para mejorar performance
db.Properties.createIndex({ "Name": "text", "AddressProperty": "text" })
db.Properties.createIndex({ "PriceProperty": 1 })
db.Properties.createIndex({ "Status": 1 })
db.Properties.createIndex({ "PropertyType": 1 })
```