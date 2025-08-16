import { useState, useCallback } from 'react';
import { Search, MapPin, DollarSign, X } from 'lucide-react';
import { Input } from '@/components/ui/Input';
import { Button } from '@/components/ui/Button';
import { Card } from '@/components/ui/Card';
import { PropertyFilters } from '@/types/property';
import React from 'react';

interface PropertyFiltersProps {
    onFiltersChange: (filters: Partial<PropertyFilters>) => void;
    onClearFilters: () => void;
    currentFilters: PropertyFilters;
    loading?: boolean;
}

export const PropertyFiltersForm: React.FC<PropertyFiltersProps> = ({
    onFiltersChange,
    onClearFilters,
    currentFilters,
    loading = false,
}) => {
    const [localFilters, setLocalFilters] = useState({
        name: currentFilters.name || '',
        address: currentFilters.address || '',
        minPrice: currentFilters.minPrice?.toString() || '',
        maxPrice: currentFilters.maxPrice?.toString() || '',
    });

    const handleInputChange = useCallback((field: string, value: string) => {
        setLocalFilters(prev => ({ ...prev, [field]: value }));
    }, []);

    const handleSearch = useCallback(() => {
        const filters: Partial<PropertyFilters> = {};

        if (localFilters.name.trim()) filters.name = localFilters.name.trim();
        if (localFilters.address.trim()) filters.address = localFilters.address.trim();
        if (localFilters.minPrice.trim()) filters.minPrice = parseFloat(localFilters.minPrice);
        if (localFilters.maxPrice.trim()) filters.maxPrice = parseFloat(localFilters.maxPrice);
        onFiltersChange(filters);
    }, [localFilters, onFiltersChange]);

    const handleClearFilters = useCallback(() => {
        setLocalFilters({
            name: '',
            address: '',
            minPrice: '',
            maxPrice: '',
        });
        onClearFilters();
    }, [onClearFilters]);

    const hasActiveFilters =
        localFilters.name || localFilters.address || localFilters.minPrice || localFilters.maxPrice;

    return (
        <Card className="mb-6">
            <div className="flex flex-col lg:flex-row gap-4 mb-4">
                <div className="flex-1">
                    <Input
                        placeholder="Buscar por nombre..."
                        value={localFilters.name}
                        onChange={(e) => handleInputChange('name', e.target.value)}
                        icon={<Search className="h-4 w-4 text-gray-400" />}
                        disabled={loading}
                    />
                </div>

                <div className="flex-1">
                    <Input
                        placeholder="Buscar por dirección..."
                        value={localFilters.address}
                        onChange={(e) => handleInputChange('address', e.target.value)}
                        icon={<MapPin className="h-4 w-4 text-gray-400" />}
                        disabled={loading}
                    />
                </div>
            </div>

            <div className="flex flex-col sm:flex-row gap-4 mb-4">
                <div className="flex-1">
                    <Input
                        type="number"
                        placeholder="Precio mínimo"
                        value={localFilters.minPrice}
                        onChange={(e) => handleInputChange('minPrice', e.target.value)}
                        icon={<DollarSign className="h-4 w-4 text-gray-400" />}
                        disabled={loading}
                    />
                </div>

                <div className="flex-1">
                    <Input
                        type="number"
                        placeholder="Precio máximo"
                        value={localFilters.maxPrice}
                        onChange={(e) => handleInputChange('maxPrice', e.target.value)}
                        icon={<DollarSign className="h-4 w-4 text-gray-400" />}
                        disabled={loading}
                    />
                </div>
            </div>

            <div className="flex justify-end gap-2">
                <Button
                    variant="outline"
                    size="sm"
                    onClick={handleSearch}
                    disabled={loading}
                    icon={<Search className="h-4 w-4 mr-2" />}
                >
                    Buscar
                </Button>
                {hasActiveFilters && (
                    <Button
                        variant="outline"
                        size="sm"
                        onClick={handleClearFilters}
                        disabled={loading}
                        icon={<X className="h-4 w-4 mr-2" />}
                    >
                        Limpiar filtros
                    </Button>
                )}
            </div>
        </Card>
    );
};
