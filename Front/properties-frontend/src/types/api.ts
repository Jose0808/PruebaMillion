export interface ApiError {
    message: string;
    status?: number;
}

export interface PaginationMeta {
    totalCount: number;
    pageNumber: number;
    pageSize: number;
    totalPages: number;
}