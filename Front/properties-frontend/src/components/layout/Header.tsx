import { Building2 } from 'lucide-react';

export const Header: React.FC = () => {
    return (
        <header className="bg-white shadow-sm border-b border-gray-200">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <div className="flex items-center justify-between h-16">
                    <div className="flex items-center">
                        <Building2 className="h-8 w-8 text-blue-600 mr-3" />
                        <h1 className="text-2xl font-bold text-gray-900">
                            Inmobiliaria
                        </h1>
                    </div>
                </div>
            </div>
        </header>
    );
};