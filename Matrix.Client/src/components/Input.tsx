/** @jsxImportSource @emotion/react */
import { InputHTMLAttributes, forwardRef } from 'react'

export const Input = forwardRef<HTMLInputElement, InputHTMLAttributes<HTMLInputElement>>(
    ({ ...props }, ref) => {
        return (
            <input
                ref={ref}
                css={{
                    width: '100%',
                    padding: '0.5rem 0.75rem',
                    borderRadius: '0.75rem',
                    border: '1px solid #ccc',
                    fontSize: '0.875rem',
                    lineHeight: 1.25,
                    outline: 0,
                    boxSizing: 'border-box',
                    '&:focus': {
                        borderColor: '#3b82f6',
                    },
                }}
                {...props}
            />
        )
    }
)
