import { HTMLAttributes, forwardRef } from 'react';
import { clsx } from 'clsx';

interface CardProps extends HTMLAttributes<HTMLDivElement> {
    padding?: 'none' | 'sm' | 'md' | 'lg';
}

export const Card = forwardRef<HTMLDivElement, CardProps>(
    ({ className, padding = 'lg', children, ...props }, ref) => {
        const paddingStyles = {
            none: '',
            sm: 'p-4',
            md: 'p-6',
            lg: 'p-8',
        };

        return (
            <div
                ref={ref}
                className={clsx('card', paddingStyles[padding], className)}
                {...props}
            >
                {children}
            </div>
        );
    }
);

Card.displayName = 'Card';