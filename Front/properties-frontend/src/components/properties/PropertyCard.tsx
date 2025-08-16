import { MapPin, DollarSign, User } from 'lucide-react';
import { Card } from '@/components/ui/Card';
import { Property } from '@/types/property';
import { formatPrice } from '@/utils/formatters';

interface PropertyCardProps {
    property: Property;
    onClick?: (property: Property) => void;
}

export const PropertyCard: React.FC<PropertyCardProps> = ({ property, onClick }) => {
    const handleClick = () => {
        if (onClick) {
            onClick(property);
        }
    };

    return (
        <Card
            className="cursor-pointer transform hover:scale-[1.02] transition-transform duration-200 animate-fade-in"
            padding="none"
            onClick={handleClick}
        >
            <div className="relative overflow-hidden rounded-t-xl">
                <img
                    src={property.image}
                    alt={property.name}
                    className="w-full h-48 object-cover transition-transform duration-300 hover:scale-110"
                    onError={(e) => {
                        const target = e.target as HTMLImageElement;
                        target.src = 'https://images.unsplash.com/photo-1570129477492-45c003edd2be?w=800&h=600&fit=crop';
                    }}
                />
                <div className="absolute top-4 right-4 bg-white/90 backdrop-blur-sm px-3 py-1 rounded-full">
                    <span className="text-sm font-semibold text-blue-600">
                        {formatPrice(property.priceProperty)}
                    </span>
                </div>
            </div>

            <div className="p-6">
                <h3 className="text-lg font-semibold text-gray-900 mb-2 line-clamp-2">
                    {property.name}
                </h3>

                <div className="flex items-start text-gray-600 mb-3">
                    <MapPin className="h-4 w-4 mt-0.5 mr-2 flex-shrink-0" />
                    <p className="text-sm line-clamp-2">
                        {property.addressProperty}
                    </p>
                </div>

                <div className="flex items-center justify-between">
                    <div className="flex items-center text-gray-600">
                        <User className="h-4 w-4 mr-1" />
                        <span className="text-sm">ID: {property.idOwner}</span>
                    </div>

                    <div className="flex items-center text-blue-600 font-semibold">
                        <DollarSign className="h-4 w-4 mr-1" />
                        <span className="text-sm">
                            {formatPrice(property.priceProperty)}
                        </span>
                    </div>
                </div>
            </div>
        </Card>
    );
};