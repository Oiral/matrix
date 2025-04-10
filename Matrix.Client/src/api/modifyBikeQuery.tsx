import { doMatrixRequest } from './doRequest';
import { Bike } from "../types/Bike";

export const modifyBikeQuery = async (bikeId: string, bikeData: Omit<Bike, 'bikeId'>): Promise<Bike> => {
    if (!bikeId) {
        throw new Error('bikeId is required for updating a bike');
    }

    const formattedBikeData = {
        ...bikeData,
        year: bikeData.year !== undefined ? new Date(bikeData.year).toISOString() : undefined
    };

    try {
        const response = await doMatrixRequest({
            path: `/bike/${bikeId}`,
            method: 'patch',
            payload: formattedBikeData,
        });

        return { bikeId, ...bikeData };
    } catch (err: any) {
        const message =
            err?.response?.data?.message ||
            err?.message ||
            'An unknown error occurred while updating the bike';
        throw new Error(message);
    }
};

