import { FC } from "react";
import { IOrder } from "../../../models/Order";
import { ConvertDateToInputString } from "../../../services/DateExtensions";
import { NavLink } from 'react-router-dom';

interface OrderCardProps {
    order: IOrder;
}

const OrderCard: FC<OrderCardProps> = ({ order }) => {
    const editLink = `/orders/${order.id}/edit`;
    const readLink = `/orders/${order.id}`;

    return (
        <div className="w-3/4 flex justify-center max-w-2xl">
            <div className="border border-black py-3 px-10 w-3/4 min-w-fit">
                <span className="font-bold text-center justify-center flex text-2xl">
                    Заказ №{order.id}
                </span>

                <span className="flex flex-row justify-between my-6 text-lg">
                    <span>
                        Номер заказа: <span className="font-bold">{order.number}</span>
                    </span>
                    <span>
                        Дата заказа: <span className="font-bold">{ConvertDateToInputString(new Date(order.date))}</span>
                    </span>
                </span>

                <span className="mb-28 flex text-lg">
                    <span>
                        Номер поставщика: <span className="font-bold">{order.providerId}</span>
                    </span>
                </span>

                <span className="flex justify-between mb-6 mx-6">
                    <NavLink
                        className="bg-yellow-400 mx-2 px-4 py-2 rounded hover:bg-yellow-500 text-lg"
                        to={editLink}
                    >
                        Редактировать
                    </NavLink>

                    <NavLink
                        className="bg-blue-500 mx-2 px-4 py-2 rounded text-white hover:bg-blue-600 text-lg"
                        to={readLink}
                    >
                        Просмотреть
                    </NavLink>
                </span>
            </div>
        </div>
    );
};

export default OrderCard;