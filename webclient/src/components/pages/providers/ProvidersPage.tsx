import { FC, useEffect, useState } from "react";
import { IProvider } from './../../../models/Provider';
import { getProviders } from "../../../services/DataService";
import { AxiosError } from 'axios';
import ErrorBanner from "../../ErrorBanner";
import List from "../../List";
import ProviderCard from "./ProviderCard";
import Loader from "../../Loader";

const ProvidersPage: FC = () => {
    const [providers, setProviders] = useState<IProvider[]>([]);
    const [error, setError] = useState<AxiosError>();

    const [isLoaded, setIsLoaded] = useState<boolean>(false);

    useEffect(() => {
        getProviders({ setProviders, setError });
        setTimeout(() => setIsLoaded(true), 3000);
    }, [])

    const reload = () => {
        setIsLoaded(false);
        getProviders({ setProviders, setError });
        setTimeout(() => setIsLoaded(true), 3000);
    }

    return (
        <div>
            {!isLoaded ? (
                <div className="flex flex-row justify-center items-center my-6">
                    <Loader />
                </div>
            ) : (
                <div>
                    {error ? (
                        <div className="flex flex-col justify-center items-center">
                            <ErrorBanner error={error} />
                            <button
                                className="border-black rounded border-2 px-4 py-2 w-min"
                                onClick={reload}
                            >
                                Перезагрузить
                            </button>
                        </div>
                    ) : (
                        <div>
                            <List items={providers} renderItem={(p) => <ProviderCard provider={p} />} />
                        </div>
                    )}
                </div>
            )}

        </div>
    );
}

export default ProvidersPage;