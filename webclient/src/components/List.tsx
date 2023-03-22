interface ListProps<T> {
    items: T[];
    renderItem: (item: T) => React.ReactNode;
}

export default function List<T>(props: ListProps<T>) {
    return (
        <div className="flex m-1 flex-col items-center space-y-4">
            {props.items.map(props.renderItem)}
        </div>
    )
}