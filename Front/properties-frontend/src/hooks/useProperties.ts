import { useEffect } from 'react';
import { usePropertyStore } from '@/stores/propertyStore';
import { PropertyFilters } from '@/types/property';

export const useProperties = (initialFilters?: PropertyFilters) => {
    const {
        properties,
        loading,
        error,
        totalCount,
        totalPages,
        currentPage,
        filters,
        fetchProperties,
        setFilters,
        clearFilters,
        setPage,
        resetState,
    } = usePropertyStore();

    useEffect(() => {
        if (initialFilters) {
            setFilters(initialFilters, true); // true → que haga fetch
        } else {
            fetchProperties(filters); // fetch inicial
        }

        return () => {
            resetState();
        };
    }, []);

    const handleFilterChange = (newFilters: Partial<PropertyFilters>) => {
        setFilters(newFilters, true); // dispara fetch al cambiar filtros
    };

    const handlePageChange = (page: number) => {
        setPage(page);
    };

    return {
        properties,
        loading,
        error,
        totalCount,
        totalPages,
        currentPage,
        filters,
        handleFilterChange,
        handlePageChange,
        clearFilters,
        refetch: () => fetchProperties(filters),
    };
};
