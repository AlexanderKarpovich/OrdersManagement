import { FC, useEffect, useState } from "react";
import { IProvider } from "../../../models/Provider";
import { AxiosError } from "axios";
import { IOrder } from "../../../models/Order";
import { getOrders, getOrdersNumbers, getProviders } from "../../../services/DataService";
import { ConvertDateToInputString } from "../../../services/DateExtensions";

interface OrdersPageFilterProps {
    setOrders: React.Dispatch<React.SetStateAction<IOrder[]>>;
    setError: React.Dispatch<React.SetStateAction<AxiosError | undefined>>;
    setIsLoaded: React.Dispatch<React.SetStateAction<boolean>>;
}

const OrdersPageFilter: FC<OrdersPageFilterProps> = ({ setOrders, setError, setIsLoaded }) => {
    // Today's date for date inputs
    const today = new Date();

    // Data for filters
    const [providers, setProviders] = useState<IProvider[]>([]);
    const [ordersNumbers, setOrdersNumbers] = useState<string[]>([]);

    // Filtering
    const [number, setNumber] = useState<string | null>(null);
    const [startDate, setStartDate] = useState<Date | null>(null);
    const [endDate, setEndDate] = useState<Date | null>(null);
    const [providerId, setProviderId] = useState<number | null>(null);

    useEffect(() => {
        setIsLoaded(false);
        getProviders({ setProviders, setError });
        getOrdersNumbers({ setOrdersNumbers, setError });
        setIsLoaded(true);
    }, // eslint-disable-next-line
    [])

    const onNumberFilterChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        if (e.target.value === "") {
            setNumber(null);
        } else {
            setNumber(e.target.value);
        }
    }

    const onProviderFilterChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        if (e.target.value === "-1") {
            setProviderId(null);
        } else {
            setProviderId(parseInt(e.target.value));
        }
    }

    const resetFilters = () => {
        setStartDate(null);
        setEndDate(null);
        setNumber(null);
        setProviderId(null);

        setIsLoaded(false);
        getOrders({ setOrders, setError });
        setTimeout(() => setIsLoaded(true), 2000);
    };

    const acceptFilters = () => {
        setIsLoaded(false);
        getOrders({ startDate, endDate, number, providerId, setOrders, setError });
        setTimeout(() => setIsLoaded(true), 2000);
    };

    return (
        <div className="my-2 flex flex-col items-center py-2">
            <div className="flex-col items-center flex-1 w-max">
                <div className="flex-row items-center justify-between flex py-2">
                    <label className="px-1 flex-1">Начальная дата:</label>
                    <input
                        type="date"
                        value={ConvertDateToInputString(startDate)}
                        max={endDate ? ConvertDateToInputString(endDate) : ConvertDateToInputString(today)}
                        onChange={(e) => setStartDate(new Date(e.target.value))}
                        className="border-black border-b flex-1"
                    />

                    <label className="px-1 flex-1">Конечная дата:</label>
                    <input
                        type="date"
                        value={ConvertDateToInputString(endDate)}
                        min={ConvertDateToInputString(startDate)}
                        max={ConvertDateToInputString(today)}
                        onChange={(e) => {
                            if (e.target.value !== "") {
                                setEndDate(new Date(e.target.value));
                            } else {
                                setEndDate(null);
                            }
                        }}
                        className="border-black border-b flex-1"
                    />
                </div>

                <div className="flex-row justify-between items-center flex py-2">
                    <label className="px-1 flex-1">Номер заказа: </label>
                    <select
                        className="border-b border-black flex-1"
                        value={number === null ? "" : number}
                        onChange={(e) => onNumberFilterChange(e)}
                    >
                        <option defaultValue="">All</option>
                        {ordersNumbers.map(on =>
                            <option key={on} value={on}>{on}</option>
                        )}
                    </select>

                    <label className="px-1 flex-1">Поставщик: </label>
                    <select
                        className="border-b border-black flex-1"
                        value={providerId === null ? "-1" : providerId}
                        onChange={(e) => onProviderFilterChange(e)}
                    >
                        {[{ id: -1, name: "All" }, ...providers].map(p =>
                            <option key={p.id} value={p.id}>{p.name}</option>
                        )}
                    </select>
                </div>

                <div className="space-x-2 flex-row items-center justify-between flex py-2">
                    <button
                        className="bg-red-600 text-white px-4 py-1 rounded hover:bg-red-700"
                        onClick={resetFilters}
                    >
                        Сбросить
                    </button>

                    <button
                        className="bg-green-700 text-white px-4 py-1 rounded hover:bg-green-800"
                        onClick={acceptFilters}
                    >
                        Применить
                    </button>
                </div>
            </div>
        </div>
    );
};

export default OrdersPageFilter;