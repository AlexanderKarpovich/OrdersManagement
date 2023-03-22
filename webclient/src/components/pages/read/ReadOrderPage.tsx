import { ChangeEvent, FC, useEffect, useState } from "react";
import { NavLink, useNavigate, useParams } from "react-router-dom";
import { IOrder } from "../../../models/Order";
import { AxiosError } from 'axios';
import ErrorBanner from "../../ErrorBanner";
import { deleteExistingOrder, getOrder, getOrderItems, getOrderItemsNames, getOrderItemsUnits } from "../../../services/DataService";
import Loader from "../../Loader";
import { ConvertDateToInputString } from './../../../services/DateExtensions';
import { IOrderItem } from './../../../models/OrderItem';

const ReadOrderPage: FC = () => {
    // Filters
    const [orderItemsNames, setOrderItemsNames] = useState<string[]>([]);
    const [orderItemsUnits, setOrderItemsUnits] = useState<string[]>([]);

    const [name, setName] = useState<string | null>(null);
    const [unit, setUnit] = useState<string | null>(null);

    // Navigation
    const { id } = useParams();
    const navigate = useNavigate();
    const editUrl = `/orders/${id}/edit`;

    // Data
    const [order, setOrder] = useState<IOrder>();
    const [orderItems, setOrderItems] = useState<IOrderItem[] | undefined>();
    const [error, setError] = useState<AxiosError>();

    // Visual
    const [isLoaded, setIsLoaded] = useState<boolean>(false);

    useEffect(() => {
        if (id) {
            const orderId = parseInt(id);
            (async () => await getOrder({ orderId, setOrder, setError }))();
            (async () => await getOrderItemsNames({ orderId, setOrderItemsNames, setError }))();
            (async () => await getOrderItemsUnits({ orderId, setOrderItemsUnits, setError }))();
            (async () => await getOrderItems({ orderId, setOrderItems, setError }))();
        }

        setTimeout(() => setIsLoaded(true), 2000);
    }, [id])

    let requestId = crypto.randomUUID();

    const deleteOrder = () => {
        const orderId = order?.id ?? -1;

        if (window.confirm('Вы уверены, что хотите удалить данный заказ?')) {
            setIsLoaded(false);

            deleteExistingOrder({ requestId, orderId, setError })

            if (error) {
                requestId = crypto.randomUUID();

                alert(error);
            } else {
                navigate('/orders');
            }

            setTimeout(() => setIsLoaded(true), 2000);
        }
    }

    const resetFilters = () => {
        setName(null);
        setUnit(null);

        setIsLoaded(false);
        getOrderItems({ orderId: order?.id ?? 0, setOrderItems, setError });
        setTimeout(() => setIsLoaded(true), 2000);
    };

    const acceptFilters = () => {
        setIsLoaded(false);
        getOrderItems({ orderId: order?.id ?? 0, name: name, unit: unit, setOrderItems, setError });
        setTimeout(() => setIsLoaded(true), 2000);
    };

    const changeName = (e: ChangeEvent<HTMLSelectElement>) => {
        if (e.target.value !== "All") {
            setName(e.target.value);
        } else {
            setName(null);
        }
    }

    const changeUnit = (e: ChangeEvent<HTMLSelectElement>) => {
        if (e.target.value !== "All") {
            setUnit(e.target.value);
        } else {
            setUnit(null);
        }
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
                                    <p>Заказ №{order?.id}</p>
                                </div>

                                <div className="flex w-full flex-row justify-between items-center">
                                    <div className="flex flex-row text-lg mx-4">
                                        <label className="flex-1 mx-3">Номер заказа: </label>
                                        <span className="flex flex-col font-bold">
                                            {order?.number}
                                        </span>
                                    </div>
                                    <div className="flex flex-row text-lg mx-4">
                                        <label className="flex-1 mx-3">Дата: </label>
                                        <span className="flex flex-col font-bold">
                                            {ConvertDateToInputString(order?.date ? new Date(order?.date) : null)}
                                        </span>
                                    </div>
                                </div>

                                <div className="flex w-full flex-row mt-2 mb-6 justify-between items-center">
                                    <div className="flex flex-row text-lg mx-4">
                                        <label className="flex-1 mx-3">Номер поставщика: </label>
                                        <span className="flex flex-col font-bold">
                                            {order?.providerId}
                                        </span>
                                    </div>
                                </div>

                                <hr className="mx-4 mb-6" />

                                <div className="flex w-full flex-col items-center justify-between my-4">
                                    <p className="text-lg text-start mb-2 font-bold">Элементы заказа</p>
                                    <hr className="w-3/4" />

                                    <div className="flex flex-row justify-around w-3/4 my-4">
                                        <div>
                                            <label>Имя: </label>
                                            <select
                                                className="border-b border-black"
                                                value={name === null ? "All" : name}
                                                onChange={(e) => changeName(e)}
                                            >
                                                {["All", ...orderItemsNames].map(n =>
                                                    <option key={orderItems?.length} value={n}>{n}</option>
                                                )}
                                            </select>
                                        </div>
                                        <div>
                                            <label>Единица измерения: </label>
                                            <select
                                                className="border-b border-black"
                                                value={unit === null ? "All" : unit}
                                                onChange={(e) => changeUnit(e)}
                                            >
                                                {["All", ...orderItemsUnits].map(n =>
                                                    <option key={orderItems?.length} value={n}>{n}</option>
                                                )}
                                            </select>
                                        </div>
                                        <div>
                                            <button 
                                                className="bg-red-600 hover:bg-red-700 rounded py-1 px-3 text-white text-sm mx-2"
                                                onClick={resetFilters}
                                            >
                                                Сбросить
                                            </button>
                                            <button
                                                className="bg-green-600 hover:bg-green-700 rounded py-1 px-3 text-white text-sm mx-2"
                                                onClick={acceptFilters}
                                            >
                                                Применить
                                            </button>
                                        </div>
                                    </div>

                                    <hr className="w-3/4" />
                                    <div className="w-full">
                                        <table className="table-auto text-left w-full">
                                            <thead className='font-bold'>
                                                <tr className='border-b border-black'>
                                                    <th>Имя</th>
                                                    <th>Количество</th>
                                                    <th>Единица измерения</th>
                                                    <th></th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody className='my-1'>
                                                {orderItems?.map(i =>
                                                    <tr key={i.id} className='border-b border-black min-h-max'>
                                                        <td className='py-2'>
                                                            <span>{i.name}</span>
                                                        </td>
                                                        <td>
                                                            <span>{i.quantity}</span>
                                                        </td>
                                                        <td>
                                                            <span>{i.unit}</span>
                                                        </td>
                                                    </tr>
                                                )}
                                            </tbody>
                                        </table>
                                    </div>
                                </div>

                                <hr className="mx-4 my-6" />

                                <div className="flex flex-row justify-between mx-12 mb-6">
                                    <NavLink
                                        to='/orders'
                                        className="border-black border rounded px-4 py-2"
                                    >
                                        Назад
                                    </NavLink>

                                    <div className="flex flex-row">
                                        <button
                                            className="bg-red-600 text-white rounded px-4 py-2 disabled:bg-red-600 mx-2"
                                            onClick={deleteOrder}
                                        >
                                            Удалить
                                        </button>
                                        <NavLink
                                            to={editUrl}
                                            className="bg-yellow-400 rounded px-4 py-2 disabled:bg-red-600"
                                        >
                                            Редактировать
                                        </NavLink>
                                    </div>
                                </div>
                            </div>
                        </div>
                    )}
                </div>
            )}
        </div>
    );
}

export default ReadOrderPage;