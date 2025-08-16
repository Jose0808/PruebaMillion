export interface Property {
    id: string;
    idOwner: string;
    name: string;
    addressProperty: string;
    priceProperty: number;
    image: string;
}

export interface PropertyFilters {
    name?: string;
    address?: string;
    minPrice?: number;
    maxPrice?: number;
    pageNumber?: number;
    pageSize?: number;
}

export interface ApiResponse<T> {
    success: boolean;
    data: {
        data: T[];
        totalCount: number;
        pageNumber: number;
        pageSize: number;
        totalPages: number;
    };
    message: string;
    errors: string[] | null;
}

export interface PropertyState {
    properties: Property[];
    loading: boolean;
    error: string | null;
    filters: PropertyFilters;
    totalCount: number;
    totalPages: number;
    currentPage: number;
}