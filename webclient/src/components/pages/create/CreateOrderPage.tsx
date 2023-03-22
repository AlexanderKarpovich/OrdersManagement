import { FC, useEffect, useState } from "react";
import { NavLink, useLocation, useNavigate, useParams } from "react-router-dom";
import { IOrder } from "../../../models/Order";
import { AxiosError } from 'axios';
import ErrorBanner from "../../ErrorBanner";
import { getOrder, getProviders, createNewOrder, updateExistingOrder } from "../../../services/DataService";
import { IProvider } from './../../../models/Provider';
import { IOrderItem } from './../../../models/OrderItem';
import Loader from "../../Loader";
import { ConvertDateToInputString } from './../../../services/DateExtensions';
import EditableOrderItemsTable from "../../EditableOrderItemsTable";

const CreateOrderPage: FC = () => {
    // Navigation
    const { id } = useParams();
    const location = useLocation();
    const navigate = useNavigate();

    // Data
    const [order, setOrder] = useState<IOrder>();
    const [providers, setProviders] = useState<IProvider[]>([]);
    const [error, setError] = useState<AxiosError>();

    // Visual
    const [isLoaded, setIsLoaded] = useState<boolean>(false);

    // Form values
    const [orderId, setOrderId] = useState<number>(0);
    const [number, setNumber] = useState<string>('123');
    const [date, setDate] = useState<Date>(new Date());
    const [providerId, setProviderId] = useState<number>();
    const [orderItems, setOrderItems] = useState<IOrderItem[]>([]);

    // Validation
    const [errors, setErrors] = useState<Record<'orderNumber' | 'name' | 'quantity' | 'unit', string[]>>({
        // eslint-disable-next-line
        ['orderNumber']: [],
        // eslint-disable-next-line
        ['name']: [],
        // eslint-disable-next-line
        ['quantity']: [],
        // eslint-disable-next-line
        ['unit']: []
    });

    useEffect(() => {
        getProviders({ setProviders, setError });
        if (id) {
            (async () => await getOrder({ orderId: parseInt(id), setOrder, setError }))();
        }

        setTimeout(() => {
            setIsLoaded(true);
        }, 2000);
    }, [id])

    useEffect(() => {
        const provider = providers.at(0);
        if (provider) {
            setProviderId(provider.id);
        }
    }, [providers])

    useEffect(() => {
        if (id && order) {
            setOrderId(order.id);
            setNumber(order.number);
            setDate(new Date(order.date));
            setProviderId(order.providerId);
            if (order.orderItems) {
                setOrderItems(order.orderItems);
            }
        }
    }, // eslint-disable-next-line 
    [order]);

    const setOrderNumber = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;

        setNumber(value);

        var validationErrors: string[] = [];

        if (orderItems.some(i => i.name === value)) {
            validationErrors.push("Номер заказа не может быть равен имени элемента заказа");
        }

        if (value.length === 0) {
            validationErrors.push("Номер заказа не может быть пустым")
        }

        setErrors({ ...errors, orderNumber: validationErrors });
    }

    const hasErrors = errors.name.length !== 0 || errors.orderNumber.length !== 0 ||
        errors.quantity.length !== 0 || errors.unit.length !== 0;

    var requestId = crypto.randomUUID();

    const createOrder = () => {
        setIsLoaded(false);
        const orderToCreate = {
            id: orderId,
            number: number,
            date: date,
            providerId: providerId,
            orderItems: orderItems
        } as IOrder;

        createNewOrder({ requestId, order: orderToCreate, setError });

        if (error) {
            // Re-generate UUID for new request
            requestId = crypto.randomUUID();

            alert(error);
        } else {
            navigate('/orders');
        }

        setTimeout(() => setIsLoaded(true), 3000);
    }

    const saveOrder = () => {
        setIsLoaded(false);
        const orderToUpdate = {
            id: order?.id,
            number: number,
            date: date,
            providerId: providerId,
            orderItems: orderItems
        } as IOrder;

        updateExistingOrder({ requestId, order: orderToUpdate, setError });

        if (error) {
            // Re-generate UUID for new request
            requestId = crypto.randomUUID();
        } else {
            navigate('/orders');
        }

        setTimeout(() => setIsLoaded(true), 3000);
    }

    return (
        <div>
            {!isLoaded ? (
                <div className="flex-row flex justify-center my-6">
                    <Loader />
                </div>
            ) : (
                <div>
                    {error ? (
                        <div className="flex-col items-center flex">
                            <ErrorBanner error={error} />
                            <div className="flex flex-row justify-center">
                                <NavLink
                                    to='/orders'
                                    className=" text-xl border border-black rounded px-4 py-2"
                                >
                                    К заказам
                                </NavLink>
                                <button
                                    className=" text-xl border border-black rounded px-4 py-2"
                                >
                                    Перезагрузить
                                </button>
                            </div>
                        </div>
                    ) : (
                        <div className="flex flex-col items-center">
                            <div className="container min-w-fit max-w-4xl border border-black my-4 mx-6 py-4 px-6 text-center">
                                <div className="font-bold text-2xl mb-6">
                                    {location.pathname.includes('create') ? (
                                        <p>Создание заказа</p>
                                    ) : (
                                        <p>Редактирование заказа</p>
                                    )}
                                </div>

                                <div className="flex flex-1 w-full flex-row mx-4 my-2 justify-between items-center">
                                    <div className="w-1/2 flex flex-row">
                                        <label className="flex-1 w-1/2">Номер заказа: </label>
                                        <div className="flex flex-col w-1/2 flex-1">
                                            <input
                                                type="text"
                                                placeholder="Номер заказа"
                                                value={number}
                                                onChange={(e) => setOrderNumber(e)}
                                                className="border-b border-black p-1 focus:outline-none"
                                            />
                                            {errors.orderNumber.length !== 0 && (
                                                <span className='text-xs text-red-600'>
                                                    {errors.orderNumber.map(e =>
                                                        <p className='pr-1'>{e}</p>
                                                    )}
                                                </span>
                                            )}
                                        </div>
                                    </div>

                                    <div className="w-1/2">
                                        <label className="flex-1 w-1/2">Дата: </label>
                                        <input
                                            type="date"
                                            value={ConvertDateToInputString(date)}
                                            max={ConvertDateToInputString(new Date())}
                                            onChange={(e) => setDate(new Date(e.target.value))}
                                            className="flex-1 w-1/2 grow border-b border-black p-1 focus:outline-none"
                                        />
                                    </div>
                                </div>

                                <div className="flex flex-1 w-full flex-row mx-4 my-2 justify-between items-center">
                                    <div className="w-1/2">
                                        <label className="flex-1 w-1/2">Поставщик: </label>
                                        <select
                                            className="border-b border-black w-1/2 flex-1"
                                            value={providerId}
                                            onChange={(e) => setProviderId(parseInt(e.target.value))}
                                        >
                                            {providers.map(p =>
                                                <option key={p.id} value={p.id}>{p.name}</option>
                                            )}
                                        </select>
                                    </div>
                                </div>

                                <hr className="mx-4 mb-6" />

                                <div className="flex w-full flex-col items-center justify-between my-4">
                                    <p className="text-lg text-start mb-2 font-bold">Элементы заказа</p>
                                    <div>
                                        <EditableOrderItemsTable
                                            items={orderItems}
                                            setOrderItems={setOrderItems}
                                            errors={errors}
                                            setErrors={setErrors}
                                        />
                                    </div>
                                </div>

                                <hr className="mx-4 my-6" />

                                <div className="flex flex-row justify-around mx-12 mb-6">
                                    <NavLink to='/orders'
                                        className="border-black border rounded px-4 py-2"
                                    >
                                        Назад
                                    </NavLink>

                                    <button
                                        className="bg-green-600 text-white rounded px-4 py-2 disabled:bg-red-600"
                                        disabled={hasErrors}
                                        onClick={location.pathname.includes('create') ? createOrder : saveOrder}
                                    >
                                        {location.pathname.includes('create') ? (
                                            <p>Создать заказ</p>
                                        ) : (
                                            <p>Сохранить заказ</p>
                                        )}
                                    </button>
                                </div>
                            </div>
                        </div>
                    )}
                </div>
            )}
        </div>
    );
}

export default CreateOrderPage;