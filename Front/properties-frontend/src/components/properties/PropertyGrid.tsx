import { Property } from '@/types/property';
import { PropertyCard } from './PropertyCard';

interface PropertyGridProps {
    properties: Property[];
    loading?: boolean;
    onPropertyClick?: (property: Property) => void;
}

export const PropertyGrid: React.FC<PropertyGridProps> = ({
    properties,
    loading = false,
    onPropertyClick,
}) => {
    if (loading) {
        return (
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
                {Array.from({ length: 8 }).map((_, index) => (
                    <div
                        key={index}
                        className="bg-white rounded-xl shadow-sm border border-gray-100 animate-pulse"
                    >
                        <div className="h-48 bg-gray-300 rounded-t-xl" />
                        <div className="p-6 space-y-3">
                            <div className="h-4 bg-gray-300 rounded w-3/4" />
                            <div className="h-3 bg-gray-300 rounded w-full" />
                            <div className="h-3 bg-gray-300 rounded w-2/3" />
                            <div className="flex justify-between">
                                <div className="h-3 bg-gray-300 rounded w-1/4" />
                                <div className="h-3 bg-gray-300 rounded w-1/3" />
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        );
    }

    if (properties.length === 0) {
        return (
            <div className="text-center py-12">
                <div className="max-w-sm mx-auto">
                    <div className="w-24 h-24 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-4">
                        <svg className="w-12 h-12 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4" />
                        </svg>
                    </div>
                    <h3 className="text-lg font-semibold text-gray-900 mb-2">
                        No se encontraron propiedades
                    </h3>
                    <p className="text-gray-600">
                        Intenta ajustar los filtros de búsqueda para encontrar propiedades.
                    </p>
                </div>
            </div>
        );
    }

    return (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
            {properties.map((property, index) => (
                <div
                    key={property.id}
                    className="animate-slide-up"
                    style={{ animationDelay: `${index * 0.1}s` }}
                >
                    <PropertyCard
                        property={property}
                        onClick={onPropertyClick}
                    />
                </div>
            ))}
        </div>
    );
};