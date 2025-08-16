import { useState } from 'react';
import { Header } from '@/components/layout/Header';
import { Footer } from '@/components/layout/Footer';
import { PropertyFiltersForm } from '@/components/forms/PropertyFilters';
import { PropertyGrid } from '@/components/properties/PropertyGrid';
import { PropertyModal } from '@/components/properties/PropertyModal';
import { Pagination } from '@/components/ui/Pagination';
import { useProperties } from '@/hooks/useProperties';
import { Property } from '@/types/property';

export const PropertiesPage: React.FC = () => {
    const [selectedProperty, setSelectedProperty] = useState<Property | null>(null);
    const [isModalOpen, setIsModalOpen] = useState(false);

    const {
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
    } = useProperties();

    const handlePropertyClick = (property: Property) => {
        setSelectedProperty(property);
        setIsModalOpen(true);
    };

    const handleCloseModal = () => {
        setIsModalOpen(false);
        setSelectedProperty(null);
    };
    
    return (
        <div className="min-h-screen bg-gray-50 flex flex-col">
            <Header />

            <main className="flex-1 max-w-8xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
                <div className="mb-8">
                    <h2 className="text-3xl font-bold text-gray-900 mb-2">
                        Encuentra tu propiedad ideal
                    </h2>
                    <p className="text-gray-600">
                        {totalCount > 0
                            ? `${totalCount} propiedades encontradas`
                            : 'Busca entre nuestra amplia selección de propiedades'
                        }
                    </p>
                </div>


                {error ? (
                    <div className="bg-red-50 border border-red-200 rounded-lg p-4 mb-6">
                        <div className="flex">
                            <div className="flex-shrink-0">
                                <svg className="h-5 w-5 text-red-400" viewBox="0 0 20 20" fill="currentColor">
                                    <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
                                </svg>
                            </div>
                            <div className="ml-3">
                                <h3 className="text-sm font-medium text-red-800">Error al cargar las propiedades</h3>
                            </div>
                        </div>
                    </div>
                ) : (
                    <>
                        <PropertyFiltersForm
                            onFiltersChange={handleFilterChange}
                            onClearFilters={clearFilters}
                            currentFilters={filters}
                            loading={loading}
                        />
                        <PropertyGrid
                            properties={properties}
                            loading={loading}
                            onPropertyClick={handlePropertyClick}
                        />

                        <Pagination
                            currentPage={currentPage}
                            totalPages={totalPages}
                            onPageChange={handlePageChange}
                            loading={loading}
                        />
                    </>
                )}
            </main>

            <Footer />

            <PropertyModal
                property={selectedProperty}
                isOpen={isModalOpen}
                onClose={handleCloseModal}
            />
        </div>
    );
};