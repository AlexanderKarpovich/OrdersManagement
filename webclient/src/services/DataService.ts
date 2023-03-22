import axios, { AxiosError } from "axios";
import * as URLS from "../models/DevelopmentServerUrls";
import { IOrder } from "../models/Order";
import { IProvider } from "../models/Provider";
import { IOrderItem } from "../models/OrderItem";

export async function getOrders(props: GetOrdersProps) {
    await axios.get<IOrder[]>(URLS.ORDERS, {
        params: {
            startDate: props.startDate,
            endDate: props.endDate,
            number: props.number,
            providerId: props.providerId
        }
    })
        .then((response) => {
            props.setOrders(response.data);
            props.setError(undefined);
        })
        .catch((error: Error | AxiosError) => {
            if (axios.isAxiosError(error)) {
                props.setError(error);
            }
            else {
                console.log(error);
            }
        });
}

export async function getProviders(props: GetProvidersProps) {
    await axios.get<IProvider[]>(URLS.PROVIDERS)
        .then((response) => {
            props.setProviders(response.data);
            props.setError(undefined);
        })
        .catch((error: Error | AxiosError) => {
            if (axios.isAxiosError(error)) {
                props.setError(error);
            }
            else {
                console.log(error);
            }
        });
}

export async function getOrdersNumbers(props: GetOrdersNumbersProps) {
    await axios.get<string[]>(URLS.ORDERS + '/numbers')
        .then((response) => {
            props.setOrdersNumbers(response.data);
            props.setError(undefined);
        })
        .catch((error: Error | AxiosError) => {
            if (axios.isAxiosError(error)) {
                props.setError(error);
            }
            else {
                console.log(error);
            }
        });
}

export async function getOrder(props: GetOrderProps) {
    await axios.get<IOrder>(URLS.ORDERS + `/${props.orderId}`)
        .then((response) => {
            props.setOrder(response.data);
            props.setError(undefined);
        })
        .catch((error: Error | AxiosError) => {
            if (axios.isAxiosError(error)) {
                props.setError(error);
            }
            else {
                console.log(error);
            }
        });
}

export async function getOrderItems(props: GetOrderItemsProps) {
    await axios.get<IOrderItem[]>(URLS.ORDERS + `/${props.orderId}/orderItems`, {
        params: {
            name: props.name,
            unit: props.unit
        }
    })
        .then((response) => {
            props.setOrderItems(response.data);
            props.setError(undefined);
        })
        .catch((error: Error | AxiosError) => {
            if (axios.isAxiosError(error)) {
                props.setError(error);
            }
            else {
                console.log(error);
            }
        });
}

export async function getOrderItemsNames(props: GetOrderItemsNamesProps) {
    await axios.get<string[]>(URLS.ORDERS + `/${props.orderId}/names`)
        .then((response) => {
            props.setOrderItemsNames(response.data);
            props.setError(undefined);
        })
        .catch((error: Error | AxiosError) => {
            if (axios.isAxiosError(error)) {
                props.setError(error);
            }
            else {
                console.log(error);
            }
        });
}

export async function getOrderItemsUnits(props: GetOrderItemsUnitsProps) {
    await axios.get<string[]>(URLS.ORDERS + `/${props.orderId}/units`)
        .then((response) => {
            props.setOrderItemsUnits(response.data);
            props.setError(undefined);
        })
        .catch((error: Error | AxiosError) => {
            if (axios.isAxiosError(error)) {
                props.setError(error);
            }
            else {
                console.log(error);
            }
        });
}

export async function createNewOrder({ requestId, order, setError }: CreateOrderProps) {
    order.orderItems.forEach(oi => oi.id = 0);
    order.id = 0;

    await axios.post(URLS.ORDERS, order, {
        headers: {
            'x-requestId': requestId,
        }
    }).then((response) => {
        if (response.status === 400) {
            const problemDetails = response.data as ProblemDetails;
            const error = new AxiosError(problemDetails.detail, problemDetails.status?.toString());
            setError(error);
        } else {
            setError(undefined);
        }
    }).catch((error: Error | AxiosError) => {
        if (axios.isAxiosError(error)) {
            setError(error);
        } else {
            console.log(error);
        }
    });
}

export async function updateExistingOrder({ requestId, order, setError }: UpdateOrderProps) {
    const items = order.orderItems.map(i => {
        const name = i.name;
        const quantity = i.quantity;
        const unit = i.unit;
        return { name, quantity, unit };
    });

    const data = {
        orderId: order.id,
        number: order.number,
        date: order.date,
        providerId: order.providerId,
        orderItems: [...items]
    }

    await axios.put(URLS.ORDERS, data, {
        headers: {
            'x-requestId': requestId,
            'Content-Type': 'application/json'
        }
    }).then((response) => {
        if (response.status === 400) {
            const problemDetails = response.data as ProblemDetails;
            const error = new AxiosError(problemDetails.detail, problemDetails.status?.toString());
            setError(error);
        } else {
            setError(undefined);
        }
    }).catch((error: Error | AxiosError) => {
        if (axios.isAxiosError(error)) {
            setError(error);
        } else {
            console.log(error);
        }
    });
}

export async function deleteExistingOrder({ requestId, orderId, setError }: DeleteOrderProps) {
    const data = { orderId };

    await axios.delete(URLS.ORDERS, { headers: { 'x-requestId': requestId }, data: data })
        .then((response) => {
            if (response.status === 400) {
                const problemDetails = response.data as ProblemDetails;
                const error = new AxiosError(problemDetails.detail, problemDetails.status?.toString());
                setError(error);
            } else {
                setError(undefined);
            }
        }).catch((error: Error | AxiosError) => {
            if (axios.isAxiosError(error)) {
                setError(error);
            } else {
                console.log(error);
            }
        });
}

interface GetOrdersProps {
    startDate?: Date | null;
    endDate?: Date | null;
    number?: string | null;
    providerId?: number | null;

    setOrders: React.Dispatch<React.SetStateAction<IOrder[]>>;
    setError: React.Dispatch<React.SetStateAction<AxiosError | undefined>>;
}

interface GetProvidersProps {
    setProviders: React.Dispatch<React.SetStateAction<IProvider[]>>;
    setError: React.Dispatch<React.SetStateAction<AxiosError | undefined>>;
}

interface GetOrdersNumbersProps {
    setOrdersNumbers: React.Dispatch<React.SetStateAction<string[]>>;
    setError: React.Dispatch<React.SetStateAction<AxiosError | undefined>>;
}

interface GetOrderProps {
    orderId: number;

    setOrder: React.Dispatch<React.SetStateAction<IOrder | undefined>>;
    setError: React.Dispatch<React.SetStateAction<AxiosError | undefined>>;
}

interface GetOrderItemsProps {
    orderId: number;

    name?: string | null;
    unit?: string | null;

    setOrderItems: React.Dispatch<React.SetStateAction<IOrderItem[] | undefined>>;
    setError: React.Dispatch<React.SetStateAction<AxiosError | undefined>>;
}

interface GetOrderItemsNamesProps {
    orderId: number;

    setOrderItemsNames: React.Dispatch<React.SetStateAction<string[]>>;
    setError: React.Dispatch<React.SetStateAction<AxiosError | undefined>>;
}

interface GetOrderItemsUnitsProps {
    orderId: number;

    setOrderItemsUnits: React.Dispatch<React.SetStateAction<string[]>>;
    setError: React.Dispatch<React.SetStateAction<AxiosError | undefined>>;
}

interface CreateOrderProps {
    // Should be GUID (UUID)
    requestId: string;
    order: IOrder;

    setError: React.Dispatch<React.SetStateAction<AxiosError | undefined>>;
}

interface UpdateOrderProps {
    // Should be GUID (UUID)
    requestId: string;
    order: IOrder;

    setError: React.Dispatch<React.SetStateAction<AxiosError | undefined>>;
}

interface DeleteOrderProps {
    // Should be GUID (UUID)
    requestId: string;
    orderId: number;

    setError: React.Dispatch<React.SetStateAction<AxiosError | undefined>>;
}

interface ProblemDetails {
    type?: string;
    title?: string;
    status?: number;
    detail?: string;
    instance?: string;
}