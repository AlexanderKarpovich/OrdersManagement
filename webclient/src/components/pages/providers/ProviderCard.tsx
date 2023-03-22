import { FC } from "react";
import { IProvider } from './../../../models/Provider';

interface ProviderCardProps {
    provider: IProvider;
}

const ProviderCard: FC<ProviderCardProps> = ({ provider }) => {
    return (
        <div className="border-b border-black flex flex-row w-3/5 justify-between text-lg">
            <span>
                Поставщик №<span className="font-bold">{provider.id}</span>
            </span>
            <span className="font-bold">
                {provider.name}
            </span>
        </div>
    );
}

export default ProviderCard;