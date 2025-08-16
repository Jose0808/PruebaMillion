import { ChevronLeft, ChevronRight, MoreHorizontal } from 'lucide-react';
import { Button } from '@/components/ui/Button';

interface PaginationProps {
    currentPage: number;
    totalPages: number;
    onPageChange: (page: number) => void;
    loading?: boolean;
}

export const Pagination: React.FC<PaginationProps> = ({
    currentPage,
    totalPages,
    onPageChange,
    loading = false,
}) => {
    if (totalPages <= 1) return null;

    const generatePageNumbers = () => {
        const delta = 2;
        const range = [];
        const rangeWithDots = [];

        for (let i = Math.max(2, currentPage - delta); i <= Math.min(totalPages - 1, currentPage + delta); i++) {
            range.push(i);
        }

        if (currentPage - delta > 2) {
            rangeWithDots.push(1, '...');
        } else {
            rangeWithDots.push(1);
        }

        rangeWithDots.push(...range);

        if (currentPage + delta < totalPages - 1) {
            rangeWithDots.push('...', totalPages);
        } else {
            rangeWithDots.push(totalPages);
        }

        return rangeWithDots;
    };

    const pages = generatePageNumbers();

    return (
        <div className="flex items-center justify-center space-x-1 mt-8">
            <Button
                variant="outline"
                size="sm"
                onClick={() => onPageChange(currentPage - 1)}
                disabled={currentPage === 1 || loading}
            >
                <ChevronLeft className="h-4 w-4" />
            </Button>

            {pages.map((page, index) => (
                <div key={index}>
                    {page === '...' ? (
                        <div className="px-3 py-2">
                            <MoreHorizontal className="h-4 w-4 text-gray-400" />
                        </div>
                    ) : (
                        <Button
                            variant={currentPage === page ? 'primary' : 'outline'}
                            size="sm"
                            onClick={() => onPageChange(page as number)}
                            disabled={loading}
                        >
                            {page}
                        </Button>
                    )}
                </div>
            ))}

            <Button
                variant="outline"
                size="sm"
                onClick={() => onPageChange(currentPage + 1)}
                disabled={currentPage === totalPages || loading}
            >
                <ChevronRight className="h-4 w-4" />
            </Button>
        </div>
    );
};