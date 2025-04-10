import { useState, useEffect } from "react";
import { css } from "@emotion/react";
import { Input } from "../components/Input";
import { Label } from "../components/Label";
import { fetchBikesQuery } from "../api/fetchBikesQuery";
import { createBikeQuery } from "../api/createBikeQuery";
import { Button } from "../components/Button";
import {modifyBikeQuery} from "../api/modifyBikeQuery";

export default function BikePage() {
    const [globalError, setGlobalError] = useState<string | null>(null);

    const [bikes, setBikes] = useState([]);
    const [editingBikeId, setEditingBikeId] = useState(null);
    const [formData, setFormData] = useState({ email: "", make: "", model: "", year: "" });
    const [errors, setErrors] = useState({
        email: undefined,
        make: undefined,
        model: undefined,
        year: undefined
    });
    const [isCreatingNew, setIsCreatingNew] = useState(false);

    useEffect(() => {
        fetchBikesQuery()
            .then(setBikes)
            .catch((err) => console.error("Failed to fetch bikes:", err));
    }, []);

    const validateForm = (data, includeEmail = true) => {
        const newErrors = {
            email: undefined,
            make: undefined,
            model: undefined,
            year: undefined
        };

        if (includeEmail) {
            if (!data.email) {
                newErrors.email = "Email is required.";
            } else if (!/^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/.test(data.email)) {
                newErrors.email = "Invalid email format.";
            }
        }

        if (!data.make) newErrors.make = "Make is required.";
        if (!data.model) newErrors.model = "Model is required.";

        if (!data.year) {
            newErrors.year = "Year is required.";
        } else if (!/^\d{4}$/.test(data.year)) {
            newErrors.year = "Year must be a 4-digit number.";
        } else if (isNaN(new Date(data.year).getTime())) {
            newErrors.year = "Invalid year format.";
        }
        
        setErrors(newErrors);
        const hasErrors = Object.values(newErrors).some((val) => val !== undefined);
        setErrors(newErrors);
        return !hasErrors;

    };

    const handleEdit = (bike) => {
        setFormData({
            email: bike.email,
            make: bike.make,
            model: bike.model,
            year:
                bike.year instanceof Date
                    ? bike.year.getFullYear().toString()
                    : new Date(bike.year).getFullYear().toString(),
        });
        setEditingBikeId(bike.bikeId)
        setIsCreatingNew(false);
        setErrors({email: undefined, make: undefined, model: undefined, year: undefined});
    };

    const handleNew = () => {
        setFormData({ email: "", make: "", model: "", year: "" });
        setEditingBikeId(null);
        setIsCreatingNew(true);
        setErrors({email: undefined, make: undefined, model: undefined, year: undefined });
    };

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
        setErrors({ ...errors, [e.target.name]: "" });
    };

    const handleCancel = () => {
        setFormData({ email: "", make: "", model: "", year: "" });
        setEditingBikeId(null);
        setIsCreatingNew(false);
        setErrors({email: undefined, make: undefined, model: undefined, year: undefined });
    };

    const handleSave = async () => {
        setGlobalError(null);
        const includeEmail = isCreatingNew || !editingBikeId;

        if (!validateForm(formData, includeEmail)) return;

        try {
            const newBike = { ...formData, year: new Date(formData.year) };

            if (editingBikeId) {
                await modifyBikeQuery(editingBikeId, newBike);
                setBikes((prev) =>
                    prev.map((b) =>
                        b.bikeId === editingBikeId ? { ...newBike, bikeId: editingBikeId } : b
                    )
                );
            } else {
                const created = await createBikeQuery(newBike);
                setBikes((prev) => [...prev, created]);
            }

            handleCancel();
        } catch (error: any) {
            const errorMessage = error?.message || "Unknown error occurred.";
            const prefix = isCreatingNew ? "Error creating bike" : "Error modifying bike";
            setGlobalError(`${prefix} - "${errorMessage}"`);
            console.error(`${prefix}:`, error);
        }
    };

    const renderFormFields = (includeEmail = true) => (
        <div css={styles.formCard}>
            {includeEmail && (
                <div>
                    <Label>Email</Label>
                    <Input name="email" value={formData.email} onChange={handleChange} />
                    {errors.email && <div css={styles.error}>{errors.email}</div>}
                </div>
            )}
            <div>
                <Label>Bike Make</Label>
                <Input name="make" value={formData.make} onChange={handleChange} />
                {errors.make && <div css={styles.error}>{errors.make}</div>}
            </div>
            <div>
                <Label>Bike Model</Label>
                <Input name="model" value={formData.model} onChange={handleChange} />
                {errors.model && <div css={styles.error}>{errors.model}</div>}
            </div>
            <div>
                <Label>Year</Label>
                <Input type="number" name="year" value={formData.year} onChange={handleChange} />
                {errors.year && <div css={styles.error}>{errors.year}</div>}
            </div>
            <div css={styles.buttonGroup}>
                <Button onClick={handleSave}>Save</Button>
                <Button variant="secondary" onClick={handleCancel}>Cancel</Button>
            </div>
        </div>
    );

    return (
        <div css={styles.container}>
            <div css={styles.header}>
                <h1 css={styles.title}>My Bikes</h1>
                <Button onClick={handleNew}>+ Add Bike</Button>
            </div>

            {globalError && (
                <div css={styles.globalError}>
                    {globalError}
                </div>
            )}


            {isCreatingNew && (
                <div css={styles.bikeItem}>
                    {renderFormFields(true)}
                </div>
            )}

            <div css={styles.bikeList}>
                {bikes.map((bike) => (
                    <div key={bike.bikeId} css={styles.bikeItem}>
                        {editingBikeId === bike.bikeId ? (
                            renderFormFields(false)
                        ) : (
                            <>
                                <div css={styles.bikeInfo}>
                                    <div css={styles.bikeEmail}>{bike.email}</div>
                                    <div css={styles.bikeDetails}>
                                        {bike.make} {bike.model}
                                    </div>
                                    <div css={styles.bikeYear}>
                                        {bike.year instanceof Date
                                            ? bike.year.getFullYear()
                                            : new Date(bike.year).getFullYear()}
                                    </div>
                                </div>
                                <Button variant="outline" onClick={() => handleEdit(bike)}>Edit</Button>
                            </>
                        )}
                    </div>
                ))}
            </div>
        </div>
    );
}

const styles = {
    container: css`
        padding: 1rem;
        max-width: 40rem;
        margin: 0 auto;
    `,
    header: css`
        display: flex;
        justify-content: space-between;
        align-items: center;
        flex-direction: column;
        margin-bottom: 1rem;
    `,
    title: css`
        font-size: 2rem;
        font-weight: bold;
    `,
    buttonGroup: css`
        display: flex;
        gap: 0.5rem;
    `,
    bikeList: css`
        display: flex;
        flex-direction: column;
        gap: 0.5rem;
    `,
    bikeItem: css`
        border: 1px solid #ddd;
        border-radius: 0.75rem;
        padding: 1rem;
        display: flex;
        justify-content: space-between;
        align-items: center;
    `,
    bikeEmail: css`
        font-size: 0.875rem;
        color: #6b7280;
    `,
    bikeInfo: css`
        flex: 1;
        align-items: center;
    `,
    bikeDetails: css`
        font-weight: 500;
    `,
    bikeYear: css`
        font-size: 0.875rem;
        color: #4b5563;
    `,
    formCard: css`
        background-color: #ffffff;
        border: 1px solid #e5e7eb;
        border-radius: 0.75rem;
        padding: 1.5rem;
        margin-bottom: 1rem;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
        display: flex;
        flex-direction: column;
        gap: 1rem;
        width: 100%;
    `,
    error: css`
        color: #dc2626;
        font-size: 0.875rem;
        margin-top: 0.25rem;
    `,
    globalError: css`
        font-weight: bold;
        background-color: #fee2e2;
        color: #b91c1c;
        padding: 0.75rem;
        border-radius: 0.5rem;
        font-size: 0.9rem;
        margin-bottom: 1rem;
        border: 1px solid #fca5a5;
    `,

};
