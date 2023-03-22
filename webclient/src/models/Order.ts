import { IOrderItem } from './OrderItem';

export interface IOrder {
    id: number;
    number: string;
    date: Date;
    providerId: number;
    orderItems: IOrderItem[];
}