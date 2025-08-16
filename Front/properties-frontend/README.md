# 🏠 Properties Frontend

Una aplicación moderna de React.js para la visualización y búsqueda de propiedades inmobiliarias. Construida con las mejores prácticas de desarrollo, arquitectura limpia y diseño responsive.

![React](https://img.shields.io/badge/React-18.2.0-blue?logo=react)
![TypeScript](https://img.shields.io/badge/TypeScript-5.0.2-blue?logo=typescript)
![Vite](https://img.shields.io/badge/Vite-4.4.5-646CFF?logo=vite)
![Tailwind CSS](https://img.shields.io/badge/Tailwind_CSS-3.3.0-38B2AC?logo=tailwind-css)
![Zustand](https://img.shields.io/badge/Zustand-4.4.1-FF6B35)

## 🎯 Características Principales

- ✅ **Filtros Avanzados**: Búsqueda por nombre, dirección y rango de precios
- ✅ **Paginación Inteligente**: Navegación eficiente con control de páginas
- ✅ **Diseño Responsive**: Optimizado para móviles, tablets y desktop
- ✅ **Estado Global**: Manejo eficiente con Zustand
- ✅ **Performance**: Debouncing, lazy loading y optimizaciones
- ✅ **TypeScript**: Tipado fuerte en toda la aplicación
- ✅ **Testing**: Setup completo con Vitest
- ✅ **Accessibility**: Componentes accesibles y semánticos
- ✅ **Clean Architecture**: Código mantenible y escalable

## 🛠️ Stack Tecnológico

### Core
- **React 18** - Biblioteca de UI con hooks modernos
- **TypeScript** - Tipado estático para JavaScript
- **Vite** - Build tool rápido y moderno

### Estado y Data
- **Zustand** - State management ligero y eficiente
- **React Hook Form** - Manejo de formularios optimizado

### UI y Estilos
- **Tailwind CSS** - Framework de CSS utility-first
- **Headless UI** - Componentes accesibles sin estilos
- **Lucide React** - Iconos modernos y consistentes

### Testing
- **Vitest** - Testing framework rápido
- **Testing Library** - Utilidades para testing de componentes

## 🚀 Instalación y Configuración

### Prerrequisitos
- Node.js >= 18.0.0
- npm >= 8.0.0

### Instalación

1. **Clonar el repositorio**
```bash
git clone https://github.com/Jose0808/PruebaMillion
cd Front/properties-frontend
```

2. **Instalar dependencias**
```bash
npm install
```

3. **Configurar variables de entorno**
```bash
# Copiar el archivo de ejemplo
cp .env.example .env.development

# Editar las variables necesarias
VITE_API_BASE_URL=https://localhost:5285
```

4. **Ejecutar en modo desarrollo**
```bash
npm run dev
```

La aplicación estará disponible en `http://localhost:3000`

## 📁 Estructura del Proyecto

```
src/
├── components/           # Componentes reutilizables
│   ├── ui/              # Componentes base (Button, Input, Card)
│   ├── forms/           # Formularios y filtros
│   ├── properties/      # Componentes específicos de propiedades
│   └── layout/          # Componentes de layout (Header, Footer)
├── pages/               # Páginas principales de la aplicación
├── hooks/               # Custom hooks
├── services/            # Servicios de API y comunicación
├── stores/              # Stores de Zustand
├── types/               # Definiciones de TypeScript
├── utils/               # Funciones utilitarias
├── constants/           # Constantes de la aplicación
└── tests/               # Tests unitarios y de integración
```

## 🎨 Componentes Principales

### PropertyCard
Tarjeta individual de propiedad con imagen, información básica y precio formateado.

### PropertyGrid  
Grid responsive que muestra las propiedades con estados de carga y vacío.

### PropertyFilters
Formulario de filtros con debouncing para búsqueda optimizada.

### PropertyModal
Modal detallado con información completa de la propiedad.

### Pagination
Componente de paginación inteligente con navegación por páginas.

## 🔄 Manejo de Estado

La aplicación utiliza **Zustand** para el manejo del estado global:

```typescript
interface PropertyState {
  properties: Property[];
  loading: boolean;
  error: string | null;
  filters: PropertyFilters;
  totalCount: number;
  totalPages: number;
  currentPage: number;
}
```

### Store Principal: `usePropertyStore`
- `fetchProperties()` - Obtiene propiedades de la API
- `setFilters()` - Actualiza filtros y refresca datos
- `clearFilters()` - Limpia todos los filtros
- `setPage()` - Cambia de página

## 📡 Servicios API

### ApiService
Clase principal para comunicación con el backend:

```typescript
class ApiService {
  async getProperties(filters: PropertyFilters): Promise<ApiResponse<Property>>
}
```

### Endpoints
- `GET /api/Properties` - Obtener propiedades con filtros

### Parámetros de Filtrado
- `name` (string) - Filtro por nombre
- `address` (string) - Filtro por dirección  
- `minPrice` (number) - Precio mínimo
- `maxPrice` (number) - Precio máximo
- `pageNumber` (number) - Número de página (default: 1)
- `pageSize` (number) - Tamaño de página (default: 10, max: 100)

## 🧪 Testing

### Ejecutar Tests
```bash
# Tests básicos
npm test

# Tests con interfaz gráfica
npm run test:ui

# Tests con coverage
npm run test:coverage
```

### Estructura de Tests
```
src/__tests__/
├── components/          # Tests de componentes
```

## 🎯 Scripts Disponibles

| Script | Descripción |
|--------|-------------|
| `npm run dev` | Servidor de desarrollo |
| `npm run build` | Build para producción |
| `npm run preview` | Preview del build |
| `npm test` | Ejecutar tests |
| `npm run lint` | Linter de código |
| `npm run type-check` | Verificación de tipos |

## 🔧 Variables de Entorno

```bash
# API Configuration
VITE_API_BASE_URL=https://localhost:5285
```

## 🎨 Personalización de Estilos

### Tailwind Configuration
```javascript
// tailwind.config.js
module.exports = {
  theme: {
    extend: {
      colors: {
        primary: {
          50: '#eff6ff',
          500: '#3b82f6',
          600: '#2563eb',
          700: '#1d4ed8',
        }
      }
    }
  }
}
```

### Componentes CSS Personalizados
```css
@layer components {
  .btn-primary {
    @apply bg-primary-600 hover:bg-primary-700 text-white font-medium py-2 px-4 rounded-lg transition-colors duration-200;
  }
  
  .card {
    @apply bg-white rounded-xl shadow-sm border border-gray-100 p-6 hover:shadow-md transition-shadow duration-200;
  }
}
```

## 🐛 Troubleshooting

### Problemas Comunes

**Error de CORS**
```bash
# Verificar que la API esté configurada correctamente
# Asegurar que VITE_API_BASE_URL sea correcto
```

**Imágenes no cargan**
```typescript
// Las imágenes tienen fallback automático a una imagen por defecto
onError={(e) => {
  e.target.src = 'https://images.unsplash.com/photo-1570129477492-45c003edd2be';
}}
```

**Tests fallan**
```bash
# Verificar que todas las dependencias estén instaladas
npm install
npm run test
```