import { doMatrixRequest } from './doRequest';
import { Bike } from "../types/Bike";

export const createBikeQuery = async (bikeData: Omit<Bike, 'bikeId'>): Promise<Bike> => {
    // Format the year as an ISO 8601 string so the dotnet api is happy
    const formattedBikeData = {
        ...bikeData,
        year: bikeData.year !== undefined ? new Date(bikeData.year).toISOString() : undefined
    };

    try {
        const response = await doMatrixRequest({
            path: '/bike',
            method: 'post',
            payload: formattedBikeData,
        });

        if (!response) {
            throw new Error('Failed to create bike');
        }

        return {...bikeData, bikeId: response} as Bike;

    } catch (err: any) {
        const message =
            err?.response?.data?.message ||
            err?.message ||
            'An unknown error occurred while updating the bike';
        throw new Error(message);
    }
};
