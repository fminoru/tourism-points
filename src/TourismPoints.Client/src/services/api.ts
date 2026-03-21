import axios from 'axios';

const API_URL = 'http://localhost:5151/api';

export interface TouristPoint {
  id: number;
  name: string;
  description: string;
  location: string;
  city: string;
  state: string;
  createdAt: string;
}

export interface PaginatedResponse {
  items: TouristPoint[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export type TouristPointPayload = Omit<TouristPoint, 'id' | 'createdAt'>;

export const api = {
  getAll: async (page: number = 1, search: string = ''): Promise<PaginatedResponse> => {
    const params = new URLSearchParams({
      page: page.toString(),
      pageSize: '10',
      ...(search && { search })
    });
    const response = await axios.get(`${API_URL}/touristpoints?${params}`);
    return response.data;
  },

  getById: async (id: number): Promise<TouristPoint> => {
    const response = await axios.get(`${API_URL}/touristpoints/${id}`);
    return response.data;
  },

  create: async (data: TouristPointPayload): Promise<TouristPoint> => {
    const response = await axios.post(`${API_URL}/touristpoints`, data);
    return response.data;
  },

  update: async (id: number, data: TouristPointPayload): Promise<TouristPoint> => {
    const response = await axios.put(`${API_URL}/touristpoints/${id}`, data);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await axios.delete(`${API_URL}/touristpoints/${id}`);
  }
};
