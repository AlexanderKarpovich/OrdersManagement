import { FC, useEffect, useState } from "react";
import { IOrder } from "../../../models/Order";
import List from "../../List";
import OrderCard from "./OrderCard";
import { AxiosError } from "axios";
import OrdersPageFilter from "./OrdersPageFilter";
import ErrorBanner from "../../ErrorBanner";
import { NavLink } from 'react-router-dom';
import { getOrders } from "../../../services/DataService";
import Loader from "../../Loader";
import { useLocation } from 'react-router-dom';

const OrdersPage: FC = () => {
    const [orders, setOrders] = useState<IOrder[]>([]);
    const [error, setError] = useState<AxiosError>();

    const [isLoaded, setIsLoaded] = useState<boolean>(false);

    const location = useLocation();

    useEffect(() => {
        getOrders({ setOrders, setError })
        setTimeout(() => setIsLoaded(true), 3000);
    }, [location.key])

    const reload = () => {
        setIsLoaded(false);
        getOrders({ setOrders, setError });
        setTimeout(() => setIsLoaded(true), 3000);
    }

    return (
        <div>
            {!isLoaded ? (
                <div className="flex my-6 items-center flex-row justify-center">
                    <Loader />
                </div>
            ) : (
                <div>
                    {error ? (
                        <div className="text-center">
                            <ErrorBanner error={error} />
                            <button
                                className="border-black rounded border-2 px-4 py-2"
                                onClick={reload}
                            >
                                Перезагрузить
                            </button>
                        </div>
                    ) : (
                        <div>
                            <div>
                                <OrdersPageFilter
                                    setOrders={setOrders}
                                    setError={setError}
                                    setIsLoaded={setIsLoaded}
                                />

                                <div className="flex justify-center mb-6">
                                    <hr className="w-full max-w-4xl" />
                                </div>

                                <div>
                                    <div>
                                        <div className="flex justify-center mt-6">
                                            <button
                                                className="border-black hover:bg-gray-100 rounded mx-4 my-2 border py-2 px-4 text-lg"
                                                onClick={reload}
                                            >
                                                Обновить страницу
                                            </button>
                                            <NavLink
                                                className="border-black hover:bg-gray-100 border mx-4 my-2 px-4 py-2 rounded text-lg"
                                                to="/orders/create"
                                            >
                                                Новый заказ
                                            </NavLink>
                                        </div>
                                    </div>
                                    {(orders.length !== 0) ? (
                                        <List
                                            key={orders.length}
                                            items={orders}
                                            renderItem={(order: IOrder) =>
                                                <OrderCard order={order} key={order.id} />
                                            }
                                        />
                                    ) : (
                                        <div className="text-3xl text-center m-2 font-bold">
                                            Пока что заказов нет
                                        </div>
                                    )}
                                </div>
                            </div>
                        </div>
                    )}
                </div>
            )}
        </div>
    );
};

export default OrdersPage;