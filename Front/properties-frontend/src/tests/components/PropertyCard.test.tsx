import { render, screen } from '@testing-library/react';
import { PropertyCard } from '@/components/properties/PropertyCard';
import { Property } from '@/types/property';

const mockProperty: Property = {
    id: '1',
    idOwner: 'OWN001',
    name: 'Test Property',
    addressProperty: 'Test Address, Test City',
    priceProperty: 500000000,
    image: 'https://example.com/image.jpg',
};

describe('PropertyCard', () => {
    it('renders property information correctly', () => {
        render(<PropertyCard property={mockProperty} />);

        expect(screen.getByText('Test Property')).toBeInTheDocument();
        expect(screen.getByText('Test Address, Test City')).toBeInTheDocument();
        expect(screen.getByText('ID: OWN001')).toBeInTheDocument();
    });

    it('displays formatted price', () => {
        render(<PropertyCard property={mockProperty} />);

        // Check if price is formatted correctly
        expect(screen.getAllByText(/500\.000\.000/)).toHaveLength(2); // Should appear twice (in overlay and footer)
    });
});