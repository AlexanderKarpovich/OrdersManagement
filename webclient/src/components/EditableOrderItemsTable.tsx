import { IOrderItem } from '../models/OrderItem'
import { useState } from 'react';
import { useEffect } from 'react';

interface EditableOrderItemsTableProps {
    items: IOrderItem[];
    errors: Record<'orderNumber' | 'name' | 'quantity' | 'unit', string[]>;

    setErrors: React.Dispatch<React.SetStateAction<Record<'orderNumber' | 'name' | 'quantity' | 'unit', string[]>>>;
    setOrderItems: React.Dispatch<React.SetStateAction<IOrderItem[]>>;
}

export default function EditableOrderItemsTable(props: EditableOrderItemsTableProps) {
    const [orderItems, setOrderItems] = useState<IOrderItem[]>([]);
    const [editId, setEditId] = useState<number>();
    const errors = props.errors;
    const setErrors = props.setErrors;

    useEffect(() => {
        if (orderItems.length === 0) {
            setOrderItems([{ id: 0, name: "New item", quantity: 10, unit: "Unit" }])
            setEditId(-1);
        }
        props.setOrderItems(orderItems);
    }, // eslint-disable-next-line
    [orderItems])

    useEffect(() => {
        setOrderItems(props.items);
    }, // eslint-disable-next-line
    [])

    const changeOrderItemName = (e: React.ChangeEvent<HTMLInputElement>, itemId: number) => {
        const value = e.target.value;

        var editData = orderItems;
        editData = editData.map(item =>
            item.id === itemId ? { ...item, name: value } as IOrderItem : item
        );

        setOrderItems(editData);

        var validationErrors: string[] = [];

        if (value.length === 0) {
            validationErrors.push("Имя не может быть пустым");
        }

        setErrors({ ...errors, name: validationErrors })
    }

    const changeOrderItemQuantity = (e: React.ChangeEvent<HTMLInputElement>, itemId: number) => {
        const value = parseFloat(e.target.value);

        var editData = orderItems;
        editData = editData.map(item =>
            item.id === itemId ? { ...item, quantity: value } as IOrderItem : item
        );

        setOrderItems(editData);

        if (value <= 0) {
            setErrors({ ...errors, quantity: ["Количество должно быть больше 0"] });
        } else {
            setErrors({ ...errors, quantity: [] })
        }
    }

    const changeOrderItemUnit = (e: React.ChangeEvent<HTMLInputElement>, itemId: number) => {
        const value = e.target.value;

        var editData = orderItems;
        editData = editData.map(item =>
            item.id === itemId ? { ...item, unit: value } as IOrderItem : item
        );

        setOrderItems(editData);

        if (value.length === 0) {
            setErrors({ ...errors, unit: ["Измерение не может быть пустым"] });
        } else {
            setErrors({ ...errors, unit: [] })
        }
    }

    const removeOrderItem = (itemId: number) => {
        var orderItem = orderItems.find(i => i.id === itemId);

        if (orderItem) {
            const itemsData = [...orderItems];
            itemsData.splice(orderItems.indexOf(orderItem), 1);
            setOrderItems(itemsData);
        }
    }

    const addOrderItem = () => {
        var orderItem = {
            id: Math.max(...orderItems.map(i => i.id)) + 1,
            name: "New item",
            quantity: 10,
            unit: "Unit"
        }
        setOrderItems([...orderItems, orderItem]);
    }

    const hasErrors = errors.name.length > 0 || errors.quantity.length > 0 || errors.unit.length > 0;

    const saveOrderItem = () => {
        if (hasErrors) {
            return;
        }

        setEditId(-1);
    }

    const setEditableItem = (itemId: number) => {
        if (hasErrors) {
            return;
        }

        setEditId(itemId);
    }

    const inputClasses = 'border-b outline-none border-black disabled:border-transparent pl-2 disabled:select-text disabled:bg-transparent';

    return (
        <div className='overflow-auto'>
            <table className='table-auto text-left'>
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
                    {orderItems.map(i =>
                        <tr key={i.id} className='border-b border-black min-h-max'>
                            <td className='py-2'>
                                <input
                                    type='text'
                                    className={`${inputClasses} cursor-text w-3/4`}
                                    value={i.name}
                                    onChange={(e) => changeOrderItemName(e, i.id)}
                                    disabled={editId !== i.id}
                                />
                                {i.id === editId && errors.name.length !== 0 && (
                                    <span className='text-xs text-red-600'>
                                        {errors.name.map(e =>
                                            <p key={e.length} className='pr-1'>{e}</p>
                                        )}
                                    </span>
                                )}
                            </td>
                            <td>
                                <input
                                    type='number'
                                    className={`${inputClasses} cursor-text w-3/4`}
                                    value={i.quantity}
                                    min="0"
                                    step="0.001"
                                    onChange={(e) => changeOrderItemQuantity(e, i.id)}
                                    disabled={editId !== i.id}
                                />
                                {i.id === editId && errors.quantity.length !== 0 && (
                                    <span className='text-xs text-red-600'>
                                        {errors.quantity.map(e =>
                                            <p key={e.length} className='pr-1'>{e}</p>
                                        )}
                                    </span>
                                )}
                            </td>
                            <td>
                                <input
                                    type='text'
                                    className={`${inputClasses} cursor-text w-3/4`}
                                    value={i.unit}
                                    onChange={(e) => changeOrderItemUnit(e, i.id)}
                                    disabled={editId !== i.id}
                                />
                                {i.id === editId && errors.unit.length !== 0 && (
                                    <span className='text-xs text-red-600'>
                                        {errors.unit.map(e =>
                                            <p key={e.length} className='pr-1'>{e}</p>
                                        )}
                                    </span>
                                )}
                            </td>
                            {i.id !== editId ? (
                                <td>
                                    <button
                                        onClick={() => setEditableItem(i.id)}
                                        className='bg-blue-500 rounded py-1 px-1 mx-2 text-white text-sm'
                                    >
                                        Изменить
                                    </button>
                                </td>
                            ) : (
                                <td>
                                    <button
                                        onClick={() => saveOrderItem()}
                                        className='bg-green-600 rounded text-white text-sm px-1 py-1'
                                    >
                                        Сохранить
                                    </button>
                                </td>
                            )}
                            {i.id !== editId ? (
                                <td>
                                    <button
                                        onClick={() => removeOrderItem(i.id)}
                                        className='bg-red-500 rounded py-1 px-3 text-sm text-white'
                                    >
                                        Удалить
                                    </button>
                                </td>
                            ) : (
                                <td></td>
                            )}
                        </tr>
                    )}
                </tbody>
            </table>
            <div className='text-end mx-6'>
                <button
                    className='text-sm mt-1 px-3 py-1 hover:underline'
                    onClick={addOrderItem}
                >
                    Новый товар
                </button>
            </div>
        </div>
    )
}