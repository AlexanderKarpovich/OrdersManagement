import { AxiosError } from 'axios';

interface ErrorBannerProps {
    error: AxiosError;
}

export default function ErrorBanner(props: ErrorBannerProps) {
    const NETWORK_ERROR_CODE = 'ERR_NETWORK';

    return (
        <div className="font-bold text-2xl text-center py-2 flex flex-col">
            <span>
                Ошибка получения данных: {props.error.message}
            </span>
            {props.error.code && props.error.code === NETWORK_ERROR_CODE &&
                <span>
                    Сервис временно недоступен
                </span>
            }
            {props.error.response && props.error.response.status === 404 &&
                <span>
                    Элемент не найден
                </span>
            }
        </div>
    )
};

