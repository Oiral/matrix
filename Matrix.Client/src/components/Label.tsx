/** @jsxImportSource @emotion/react */
import { LabelHTMLAttributes } from 'react'

export const Label = ({ children, ...props }: LabelHTMLAttributes<HTMLLabelElement>) => {
    return (
        <label
            css={{
                display: 'block',
                marginBottom: '0.25rem',
                fontSize: '0.875rem',
                fontWeight: 500,
                color: '#374151',
            }}
            {...props}
        >
            {children}
        </label>
    )
}
