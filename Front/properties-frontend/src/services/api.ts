import { PropertyFilters, ApiResponse, Property } from '@/types/property';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5285';

class ApiService {
    private async request<T>(
        endpoint: string,
        options: RequestInit = {}
    ): Promise<T> {
        const url = `${API_BASE_URL}${endpoint}`;

        const response = await fetch(url, {
            headers: {
                'Content-Type': 'application/json',
                ...options.headers,
            },
            ...options,
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        return response.json();
    }

    async getProperties(filters: PropertyFilters = {}): Promise<ApiResponse<Property>> {
        const queryParams = new URLSearchParams();

        Object.entries(filters).forEach(([key, value]) => {
            if (value !== undefined && value !== '') {
                queryParams.append(key, value.toString());
            }
        });

        const endpoint = `/api/Properties${queryParams.toString() ? `?${queryParams.toString()}` : ''}`;
        return this.request<ApiResponse<Property>>(endpoint);
    }
}

export const apiService = new ApiService();