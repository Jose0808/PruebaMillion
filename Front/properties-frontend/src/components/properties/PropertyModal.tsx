import { Fragment } from 'react';
import { Dialog, Transition } from '@headlessui/react';
import { X, MapPin, DollarSign, User, Calendar } from 'lucide-react';
import { Property } from '@/types/property';
import { formatPrice } from '@/utils/formatters';
import { Button } from '@/components/ui/Button';

interface PropertyModalProps {
    property: Property | null;
    isOpen: boolean;
    onClose: () => void;
}

export const PropertyModal: React.FC<PropertyModalProps> = ({
    property,
    isOpen,
    onClose,
}) => {
    if (!property) return null;

    return (
        <Transition appear show={isOpen} as={Fragment}>
            <Dialog as="div" className="relative z-50" onClose={onClose}>
                <Transition.Child
                    as={Fragment}
                    enter="ease-out duration-300"
                    enterFrom="opacity-0"
                    enterTo="opacity-100"
                    leave="ease-in duration-200"
                    leaveFrom="opacity-100"
                    leaveTo="opacity-0"
                >
                    <div className="fixed inset-0 bg-black/25 backdrop-blur-sm" />
                </Transition.Child>

                <div className="fixed inset-0 overflow-y-auto">
                    <div className="flex min-h-full items-center justify-center p-4 text-center">
                        <Transition.Child
                            as={Fragment}
                            enter="ease-out duration-300"
                            enterFrom="opacity-0 scale-95"
                            enterTo="opacity-100 scale-100"
                            leave="ease-in duration-200"
                            leaveFrom="opacity-100 scale-100"
                            leaveTo="opacity-0 scale-95"
                        >
                            <Dialog.Panel className="w-full max-w-2xl transform overflow-hidden rounded-2xl bg-white text-left align-middle shadow-xl transition-all">
                                <div className="relative">
                                    <img
                                        src={property.image}
                                        alt={property.name}
                                        className="w-full h-64 object-cover"
                                        onError={(e) => {
                                            const target = e.target as HTMLImageElement;
                                            target.src = 'https://images.unsplash.com/photo-1570129477492-45c003edd2be?w=800&h=600&fit=crop';
                                        }}
                                    />
                                    <Button
                                        variant="ghost"
                                        size="sm"
                                        onClick={onClose}
                                        className="absolute top-4 right-4 bg-white/90 backdrop-blur-sm hover:bg-white"
                                    >
                                        <X className="h-4 w-4" />
                                    </Button>

                                    <div className="absolute bottom-4 left-4 bg-white/90 backdrop-blur-sm px-4 py-2 rounded-lg">
                                        <div className="flex items-center text-blue-600 font-bold text-lg">
                                            <DollarSign className="h-5 w-5 mr-1" />
                                            {formatPrice(property.priceProperty)}
                                        </div>
                                    </div>
                                </div>

                                <div className="p-6">
                                    <Dialog.Title
                                        as="h3"
                                        className="text-2xl font-bold text-gray-900 mb-4"
                                    >
                                        {property.name}
                                    </Dialog.Title>

                                    <div className="space-y-4">
                                        <div className="flex items-start">
                                            <MapPin className="h-5 w-5 text-gray-400 mt-0.5 mr-3 flex-shrink-0" />
                                            <div>
                                                <h4 className="font-semibold text-gray-900">Ubicación</h4>
                                                <p className="text-gray-600">{property.addressProperty}</p>
                                            </div>
                                        </div>

                                        <div className="flex items-center">
                                            <User className="h-5 w-5 text-gray-400 mr-3" />
                                            <div>
                                                <h4 className="font-semibold text-gray-900">Propietario</h4>
                                                <p className="text-gray-600">ID: {property.idOwner}</p>
                                            </div>
                                        </div>

                                        <div className="flex items-center">
                                            <DollarSign className="h-5 w-5 text-gray-400 mr-3" />
                                            <div>
                                                <h4 className="font-semibold text-gray-900">Precio</h4>
                                                <p className="text-gray-600">{formatPrice(property.priceProperty)}</p>
                                            </div>
                                        </div>
                                    </div>

                                    <div className="mt-6 flex flex-col sm:flex-row gap-3">
                                        <Button
                                            onClick={() => {
                                                alert('Funcionalidad de contacto - Por implementar');
                                            }}
                                            className="flex-1"
                                        >
                                            Contactar
                                        </Button>
                                        <Button
                                            variant="outline"
                                            onClick={() => {
                                                alert('Funcionalidad de favoritos - Por implementar');
                                            }}
                                            className="flex-1"
                                        >
                                            Guardar
                                        </Button>
                                    </div>
                                </div>
                            </Dialog.Panel>
                        </Transition.Child>
                    </div>
                </div>
            </Dialog>
        </Transition>
    );
};