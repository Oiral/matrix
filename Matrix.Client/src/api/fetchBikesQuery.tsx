import { doMatrixRequest } from './doRequest';
import {Bike} from "../types/Bike";

export const fetchBikesQuery = async () => {
    try {
        const response = await doMatrixRequest({
            path: '/bike',
            method: 'get'
        });

        if (response) {
            return response as Bike[];
        } else {
            throw new Error('Failed to fetch bikes');
        }
    } catch (error) {
        console.error('Error querying bikes:', error);
        throw error;
    }
};