/** @jsxImportSource @emotion/react */
import { ButtonHTMLAttributes, ReactNode, forwardRef } from 'react'
import { css } from '@emotion/react'

const validButtonTypes = ['button', 'reset', 'submit'] as const

type Variant = keyof typeof variantStyles

export const Button = forwardRef<
    HTMLButtonElement,
    {
            type?: (typeof validButtonTypes)[number]
            alt?: string
            disabled?: boolean
            variant?: Variant
            children?: ReactNode
    } & Pick<ButtonHTMLAttributes<HTMLButtonElement>, 'onClick' | 'onMouseDown' | 'onMouseUp' | 'onKeyDown' | 'style'>
>(({ type = 'button', alt, disabled, children, variant = 'primary', ...props }, ref) => {
        return (
            <button
                ref={ref}
                css={[
                        {
                                appearance: 'none',
                                background: 0,
                                border: 0,
                                boxSizing: 'border-box',
                                cursor: 'pointer',
                                display: 'flex',
                                alignItems: 'center',
                                justifyContent: 'center',
                                fontFamily: 'inherit',
                                fontSize: 'inherit',
                                lineHeight: 1,
                                outline: 0,
                                overflow: 'hidden',
                                padding: 0,
                                pointerEvents: 'auto',
                                position: 'relative',
                                textOverflow: 'ellipsis',
                                userSelect: 'none',
                                verticalAlign: 'middle',
                                WebkitTapHighlightColor: 'transparent',
                                WebkitTouchCallout: 'none',
                                whiteSpace: 'nowrap',
                                '&:disabled': { cursor: 'not-allowed', opacity: 0.5 },
                                color: 'inherit',
                        },
                        variantStyles[variant],
                ]}
                type={type}
                disabled={disabled}
                aria-label={alt}
                {...props}
            >
                    {children}
            </button>
        )
})

const variantStyles = {
        primary: css({
                backgroundColor: '#3b82f6',
                color: 'white',
                padding: '0.5rem 1rem',
                borderRadius: '0.75rem',
                '&:hover': {
                        backgroundColor: '#2563eb',
                },
        }),
        secondary: css({
                backgroundColor: '#e5e7eb',
                color: '#111827',
                padding: '0.5rem 1rem',
                borderRadius: '0.75rem',
                '&:hover': {
                        backgroundColor: '#d1d5db',
                },
        }),
        outline: css({
                border: '1px solid #d1d5db',
                color: '#111827',
                padding: '0.5rem 1rem',
                borderRadius: '0.75rem',
                backgroundColor: 'white',
                '&:hover': {
                        backgroundColor: '#f3f4f6',
                },
        }),
}