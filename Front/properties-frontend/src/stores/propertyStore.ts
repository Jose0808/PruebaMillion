import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import { Property, PropertyFilters, PropertyState } from '@/types/property';
import { apiService } from '@/services/api';
import { DEFAULT_PAGE_SIZE, DEFAULT_PAGE_NUMBER } from '@/constants/api';

interface PropertyActions {
    fetchProperties: (filters?: PropertyFilters) => Promise<void>;
    setFilters: (filters: Partial<PropertyFilters>, fetch?: boolean) => void;
    clearFilters: () => void;
    setPage: (page: number) => void;
    setError: (error: string | null) => void;
    resetState: () => void;
}

const initialState: PropertyState = {
    properties: [],
    loading: false,
    error: null,
    filters: {
        pageNumber: DEFAULT_PAGE_NUMBER,
        pageSize: DEFAULT_PAGE_SIZE,
    },
    totalCount: 0,
    totalPages: 0,
    currentPage: DEFAULT_PAGE_NUMBER,
};

export const usePropertyStore = create<PropertyState & PropertyActions>()(
    devtools(
        (set, get) => ({
            ...initialState,

            fetchProperties: async (filters?: PropertyFilters) => {
                const currentFilters = get().filters;
                const finalFilters = filters || currentFilters;

                if (JSON.stringify(finalFilters) === JSON.stringify(currentFilters) && get().properties.length > 0) {
                    return;
                }

                set({ loading: true, error: null });

                try {
                    const response = await apiService.getProperties(finalFilters);

                    if (response.success) {
                        set({
                            properties: response.data.data,
                            totalCount: response.data.totalCount,
                            totalPages: response.data.totalPages,
                            currentPage: response.data.pageNumber,
                            filters: finalFilters,
                            loading: false,
                        });
                    } else {
                        set({
                            error: response.message || 'Error fetching properties',
                            loading: false,
                        });
                    }
                } catch (error) {
                    set({
                        error: error instanceof Error ? error.message : 'Unknown error occurred',
                        loading: false,
                    });
                }
            },

            setFilters: (newFilters, fetch = false) => {
                const updatedFilters = {
                    // ...get().filters,
                    ...newFilters,
                    pageNumber: DEFAULT_PAGE_NUMBER, // reset al inicio al cambiar filtros
                };

                if (fetch) get().fetchProperties(updatedFilters);
                set({ filters: updatedFilters });
            },

            clearFilters: () => {
                const clearedFilters = {
                    pageNumber: DEFAULT_PAGE_NUMBER,
                    pageSize: DEFAULT_PAGE_SIZE,
                };
                set({ filters: clearedFilters });
                get().fetchProperties(clearedFilters);
            },

            setPage: (page) => {
                const filters = { ...get().filters, pageNumber: page };
                get().fetchProperties(filters);
                set({ filters });
            },

            setError: (error) => set({ error }),

            resetState: () => set(initialState),
        }),
        { name: 'property-store' }
    )
);
